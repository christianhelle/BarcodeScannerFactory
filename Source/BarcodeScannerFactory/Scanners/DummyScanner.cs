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
                Scanned.Invoke(this, new ScannedDataEventArgs(new[] {new BarcodeData {Text = "NO_BARCODE_SCANNER"}}));
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