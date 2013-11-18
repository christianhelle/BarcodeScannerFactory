#region License

// The MIT License (MIT)
// 
// Copyright (c) 2013 Christian Resma Helle
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.Text;
using System.Threading;
using Calib;

namespace ChristianHelle.Barcode.Scanners
{
    /// <summary>
    ///     Casio Barcode scanner device
    /// </summary>
    public class CasioScanner : IBarcodeScanner
    {
        private static readonly object syncLock = new object();
        private volatile bool running;
        private Thread scanningThread;

        /// <summary>
        /// Gets the status of the scanner
        /// </summary>
        public ScannerStatus Status { get; private set; }

        /// <summary>
        /// Event raised when new data has been scanned
        /// </summary>
        public event EventHandler<ScannedDataEventArgs> Scanned;

        /// <summary>
        /// Opens the connection to the scanner
        /// </summary>
        public void Open()
        {
            if (Status == ScannerStatus.Opened)
                return;

            lock (syncLock)
            {
                //var result = OBReadLibNet.Api.OBROpen(Handle.GetValueOrDefault(IntPtr.Zero),
                //                                      OBReadLibNet.Def.OBR_ALL | OBReadLibNet.Def.OBR_OUT_ON);
                //CheckCasioResult(result);

                var result = OBReadLibNet.Api.OBRLoadConfigFile(); //ini File read default value set
                CheckCasioResult(result);

                result = OBReadLibNet.Api.OBRSetDefaultSymbology(); //1D(OBR) driver mode will be ini File vallue
                CheckCasioResult(result);

                result =
                    OBReadLibNet.Api.OBRSetScanningKey(OBReadLibNet.Def.OBR_TRIGGERKEY_L |
                                                       OBReadLibNet.Def.OBR_TRIGGERKEY_R |
                                                       OBReadLibNet.Def.OBR_CENTERTRIGGER);
                CheckCasioResult(result);

                result = OBReadLibNet.Api.OBRSetScanningCode(OBReadLibNet.Def.OBR_ALL);
                CheckCasioResult(result);

                result = OBReadLibNet.Api.OBRSetBuffType(OBReadLibNet.Def.OBR_BUFOBR);
                //1D(OBR) driver mode will be OBR_BUFOBR
                CheckCasioResult(result);

                result = OBReadLibNet.Api.OBRSetScanningNotification(OBReadLibNet.Def.OBR_EVENT, Handle);
                //1D(OBR) driver mode will be OBR_EVENT
                CheckCasioResult(result);

                result = OBReadLibNet.Api.OBRSetBuzzer(OBReadLibNet.Def.OBR_BUZON); //enable sound notification
                CheckCasioResult(result);

                result = OBReadLibNet.Api.OBRSetVibrator(OBReadLibNet.Def.OBR_VIBON); //enable sound notification
                CheckCasioResult(result);

                result = OBReadLibNet.Api.OBROpen(Handle, 0); //OBRDRV open
                CheckCasioResult(result);

                result = OBReadLibNet.Api.OBRClearBuff();
                CheckCasioResult(result);

                OBReadLibNet.Api.OBRSaveConfigFile();

                Status = ScannerStatus.Opened;

                scanningThread = new Thread(GetScannerStatusWorker) {IsBackground = true};
                scanningThread.Start();
            }
        }

        /// <summary>
        /// Closes the connection to the scanner
        /// </summary>
        public void Close()
        {
            if (Status != ScannerStatus.Opened)
                return;

            lock (syncLock)
            {
                running = false;
                scanningThread.Abort();
                scanningThread = null;

                var result = OBReadLibNet.Api.OBRClose();
                CheckCasioResult(result);

                Status = ScannerStatus.Closed;
            }
        }

        /// <summary>
        /// Manually initiate the scan
        /// </summary>
        /// <remarks>
        /// The scanned barcode will be received in the <see cref="ScannedDataEventArgs.Data"/>
        /// parameter when the <see cref="IBarcodeScanner.Scanned"/> event is fired
        /// </remarks>
        public void Scan()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// The handle to the window in which notifications are sent (required for certain scanner API's)
        /// </summary>
        public IntPtr Handle { get; set; }

        /// <summary>
        ///     Clean up resources used by the scanner device
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        private void CheckCasioResult(int p)
        {
            if (p != OBReadLibNet.Def.OBR_OK)
            {
                switch (p)
                {
                    case (OBReadLibNet.Def.OBR_NONDT):
                        throw new Exception("error end");
                    case (OBReadLibNet.Def.OBR_PON):
                        throw new Exception("Already open");
                    case (OBReadLibNet.Def.OBR_POF):
                        throw new Exception("Not open");
                    case (OBReadLibNet.Def.OBR_PRM):
                        throw new Exception("parameter error");
                    case (OBReadLibNet.Def.OBR_NOT_DEVICE):
                        throw new Exception("OBR Driver(Scanner) device is not available");
                    case (OBReadLibNet.Def.OBR_NOT_DEVICE_DECODE):
                        throw new Exception("OBR Driver(decode) device is not available");
                    case (OBReadLibNet.Def.OBR_ERROR_HOTKEY):
                        throw new Exception("RegisterHotKey error");
                    default:
                        throw new Exception("Unknown error: " + p.ToString("X"));
                }
            }
        }

        private void GetScannerStatusWorker()
        {
            running = true;
            try
            {
                while (running)
                {
                    SystemLibNet.Api.SysWaitForEvent(Handle, OBReadLibNet.Def.OBR_NAME_EVENT, 2000
                        /*timeout SystemLibNet.Def.INFINITE*/); //Wait event

                    int size = 0, code = 0;
                    byte number = 0, len = 0;
                    var result = OBReadLibNet.Api.OBRGetStatus(ref size, ref number);
                    CheckCasioResult(result);

                    if (number > 0)
                    {
                        var buffer = new byte[size];
                        result = OBReadLibNet.Api.OBRGets(buffer, ref code, ref len);
                        CheckCasioResult(result);

                        result = OBReadLibNet.Api.OBRClearBuff();
                        CheckCasioResult(result);

                        if (Scanned != null)
                        {
                            var barcode = Encoding.Default.GetString(buffer, 0, buffer.Length).Trim();
                            var barcodeData = new BarcodeData {Text = barcode};
                            Scanned.Invoke(this, new ScannedDataEventArgs(new[] {barcodeData}));
                        }
                    }
                }
            }
            catch (ThreadAbortException)
            {
            }
            finally
            {
                running = false;
            }
        }
    }
}