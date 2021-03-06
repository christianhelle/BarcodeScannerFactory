﻿#region License

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

#region Imported Namespaces

using System;
using System.Threading;
using Intermec.DataCollection;
using IntermecBarcodeReader = Intermec.DataCollection.BarcodeReader;

#endregion

namespace ChristianHelle.Barcode.Scanners
{
    /// <summary>
    /// Intermec Barcode Scanner device
    /// </summary>
    public class IntermecScanner : IBarcodeScanner
    {
        private const int TIMEOUT_MS = 10000;
        private readonly BarcodeReadEventHandler scanEvent;
        private IntermecBarcodeReader reader;
        private string[] scannedData;
        private ScannerStatus scannerStatus;
        private Timer timeout;

        /// <summary>
        /// Creates an instance of the Intermec barcode scanner device
        /// </summary>
        public IntermecScanner()
        {
            scanEvent = OnBarcodeRead;
            scannerStatus = ScannerStatus.Closed;

            reader = new IntermecBarcodeReader {ScannerOn = false, ScannerEnable = false};
        }

        #region IBarcodeScanner Members

        /// <summary>
        /// Raised when the scanned data changes
        /// </summary>
        public event EventHandler<ScannedDataEventArgs> Scanned;

        /// <summary>
        /// Get the scannerStatus of the scanner device
        /// </summary>
        public ScannerStatus Status
        {
            get
            {
                if (reader != null)
                {
                    try
                    {
                        if (reader.ScannerOn)
                            return ScannerStatus.Scanning;
                        return scannerStatus;
                    }
                    catch
                    {
                        return scannerStatus;
                    }
                }
                return scannerStatus;
            }
        }

        /// <summary>
        /// Open the scanner device
        /// </summary>
        public void Open()
        {
            if (scannerStatus == ScannerStatus.Opened)
                return;

            try
            {
                if (reader == null)
                    reader = new IntermecBarcodeReader {ScannerOn = false, ScannerEnable = false};

                reader.BarcodeRead += scanEvent;
                reader.ScannerEnable = true;
                reader.ThreadedRead(true);

                scannedData = new string[1];
                scannerStatus = ScannerStatus.Opened;
            }
            catch
            {
                Close();
                throw;
            }
        }

        /// <summary>
        /// Close the scanner device
        /// </summary>
        public void Close()
        {
            if (timeout != null)
                timeout.Dispose();

            if (scannerStatus == ScannerStatus.Closed)
                return;

            if (reader != null)
            {
                reader.ScannerOn = false;
                reader.ScannerEnable = false;
                reader.BarcodeRead -= scanEvent;
                reader.CancelRead(true);
                reader.Dispose();
                reader = null;
            }

            scannedData = null;
            scannerStatus = ScannerStatus.Closed;
        }

        /// <summary>
        /// Start Scanning
        /// </summary>
        public void Scan()
        {
            if (reader == null || scannerStatus != ScannerStatus.Opened)
                return;

            if (!reader.ScannerEnable)
                return;

            scannerStatus = ScannerStatus.Scanning;
            Thread.Sleep(10);

            reader.ScannerOn = true;
            timeout = new Timer(TimeoutCallback, this, TIMEOUT_MS, Timeout.Infinite);
        }

        /// <summary>
        /// The handle to the window in which notifications are sent (required for certain scanner API's)
        /// </summary>
        public IntPtr Handle { get; set; }

        /// <summary>
        /// Clean up resources used by the scanner device
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        /// Called to free resources.
        /// </summary>
        /// <param name="disposing">Should be true when calling from Dispose().</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            if (timeout != null)
            {
                timeout.Dispose();
                timeout = null;
            }
            Close();
        }

        private void OnBarcodeRead(object sender, BarcodeReadEventArgs bre)
        {
            scannedData[0] = bre.strDataBuffer;

            if (Scanned != null)
                Scanned.Invoke(this, new ScannedDataEventArgs(new[] {new BarcodeData {Text = bre.strDataBuffer}}));

            reader.ScannerOn = false;
            scannerStatus = ScannerStatus.Opened;
        }

        private static void TimeoutCallback(object state)
        {
            var instance = (IntermecScanner) state;
            instance.reader.ScannerOn = false;
            instance.scannerStatus = ScannerStatus.Opened;
        }
    }
}