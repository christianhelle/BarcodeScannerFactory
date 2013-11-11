using System;
using System.Diagnostics;

namespace ChristianHelle.Barcode.Scanners
{
    /// <summary>
    /// 
    /// </summary>
    public class DummyScanner : IBarcodeScanner
    {
        /// <summary>
        /// 
        /// </summary>
        public DummyScanner()
        {
            Debug.WriteLine("Dummy Scanner Loaded");
        }

        #region IBarcodeScanner Members

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
            Status = ScannerStatus.Opened;
        }

        /// <summary>
        /// Closes the connection to the scanner
        /// </summary>
        public void Close()
        {
            Status = ScannerStatus.Closed;
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
            if (Scanned != null)
                Scanned.Invoke(this, new ScannedDataEventArgs(new[] { new BarcodeData { Text = "NO_BARCODE_SCANNER" } }));
        }

        /// <summary>
        /// The handle to the window in which notifications are sent (required for certain scanner API's)
        /// </summary>
        public IntPtr Handle { get; set; }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        #endregion
    }
}
