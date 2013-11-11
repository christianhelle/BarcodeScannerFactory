using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace ChristianHelle.Barcode
{
    /// <summary>
    /// Retrieves Device and OEM specific information
    /// </summary>
    public static class Device
    {
        private const int DEVICE_ID_DW_PLATFORM_ID_OFFSET = 0xc;
        private const int DEVICE_ID_DW_PLATFORM_ID_SIZE = 0x10;
        private const int DEVICE_ID_DW_PRESET_ID_OFFSET = 0x4;
        private const int DEVICE_ID_DW_PRESET_SIZE = 0x8;
        private const int FILE_ANY_ACCESS = 0;
        private const int FILE_DEVICE_HAL = 0x00000101;

        private const int IOCTL_HAL_GET_DEVICEID =
            ((FILE_DEVICE_HAL) << 16) | ((FILE_ANY_ACCESS) << 14) | ((21) << 2) | (METHOD_BUFFERED);

        private const int MAX_PATH = 260;
        private const int METHOD_BUFFERED = 0;
        private const int SPI_GETOEMINFO = 258;

        /// <summary>
        /// Gets the Device ID
        /// </summary>
        public static string DeviceID
        {
            get
            {
                if (Environment.OSVersion.Platform == PlatformID.WinCE)
                {
                    try
                    {
                        return GetDeviceID();
                    }
                    catch
                    {
                    }
                }

                return "UNKNOWN_DEVICE_ID";
            }
        }

        /// <summary>
        /// Gets the a shortened version of the Device ID
        /// </summary>
        /// <remarks>
        /// A standard device ID could be something like this: "0B019E0169492F317800-0050BF7A60E2" 
        /// This version only returns the string after the "-" seperator
        /// </remarks>
        public static string ShortDeviceID
        {
            get
            {
                if (Environment.OSVersion.Platform == PlatformID.WinCE)
                    if (!string.IsNullOrEmpty(DeviceID))
                    {
                        var split = DeviceID.Split('-');
                        return split.Length > 0 ? split[1] : DeviceID;
                    }
                return "UNKNOWN_DEVICE_ID";
            }
        }

        /// <summary>
        /// Retrieves the OEM information from the device.
        /// </summary>
        /// <returns>OEM Information (manufacturer and model)</returns>
        public static string GetManufacturerInfo()
        {
            var oemInfo = new string(' ', 50);
            var result = SystemParametersInfo(SPI_GETOEMINFO, 50, oemInfo, 0);
            return result != 0 ? oemInfo.Substring(0, oemInfo.IndexOf('\0')) : null;
        }

        [DllImport("coredll.dll")]
        private static extern int SystemParametersInfo(int uiAction, int uiParam, string pvParam, int fWinIni);

        /// <summary>
        /// This function returns an application-specific hash of the device identifier. 
        /// The application can use this hash to uniquely identify the device.
        /// </summary>
        public static string GetUniqueDeviceID(string appString)
        {
            if (Environment.OSVersion.Platform == PlatformID.WinCE)
            {
                try
                {
                    var data = new byte[appString.Length];
                    for (var count = 0; count < appString.Length; count++)
                        data[count] = (byte) appString[count];

                    var size = data.Length;
                    var deviceOutput = new byte[20];
                    var sizeOut = 20;

                    GetDeviceUniqueID(data, size, 1, deviceOutput, ref sizeOut);
                    return Encoding.ASCII.GetString(deviceOutput, 0, deviceOutput.Length);
                }
                catch (MissingMethodException)
                {
                    Debug.WriteLine("GetDeviceUniqueID is not supported in this device");

                    try
                    {
                        return GetDeviceID();
                    }
                    catch
                    {
                    }
                }
            }

            return "UNKNOWN_DEVICE_ID";
        }

        [DllImport("coredll.dll", SetLastError = true)]
        private static extern int GetDeviceUniqueID(byte[] pbApplicationData,
                                                    int cbApplictionData,
                                                    int dwDeviceIDVersion,
                                                    byte[] pbDeviceIDOutput,
                                                    ref int pcbDeviceIDOutput);

        [DllImport("coredll.dll", SetLastError = true)]
        private static extern bool KernelIoControl(int dwIoControlCode,
                                                   byte[] inBuf,
                                                   int inBufSize,
                                                   byte[] outBuf,
                                                   int outBufSize,
                                                   ref int bytesReturned);

        private static byte[] GetRawDeviceID()
        {
            var bytesReturned = 0;
            var outputBuffer = new byte[MAX_PATH];
            var size = outputBuffer.Length;

            BitConverter.GetBytes(MAX_PATH).CopyTo(outputBuffer, 0);
            if (!KernelIoControl(IOCTL_HAL_GET_DEVICEID, null, 0, outputBuffer, size, ref bytesReturned))
                throw new Exception("Unexpected error in retrieving Raw Device ID");

            return outputBuffer;
        }

        private static string GetDeviceID()
        {
            var outbuff = GetRawDeviceID();

            var dwPresetIDOffset = BitConverter.ToInt32(outbuff, DEVICE_ID_DW_PRESET_ID_OFFSET);
            var dwPresetIDSize = BitConverter.ToInt32(outbuff, DEVICE_ID_DW_PRESET_SIZE);
            var dwPlatformIDOffset = BitConverter.ToInt32(outbuff, DEVICE_ID_DW_PLATFORM_ID_OFFSET);
            var dwPlatformIDSize = BitConverter.ToInt32(outbuff, DEVICE_ID_DW_PLATFORM_ID_SIZE);
            var sb = new StringBuilder();

            for (var i = dwPresetIDOffset; i < dwPresetIDOffset + dwPresetIDSize; i++)
                sb.Append(String.Format("{0:X2}", outbuff[i]));

            sb.Append("-");
            for (var i = dwPlatformIDOffset; i < dwPlatformIDOffset + dwPlatformIDSize; i++)
                sb.Append(String.Format("{0:X2}", outbuff[i]));

            return sb.ToString();
        }

        /// <summary>
        /// Restarts the device
        /// </summary>
        public static void Restart()
        {
            SetSystemPowerState(null, 0x00800000, 0); // POWER_STATE_RESET
        }

        [DllImport("coredll.dll")]
        internal static extern int SetSystemPowerState(string pwsSystemState, int dwStateFlags, int options);
    }
}