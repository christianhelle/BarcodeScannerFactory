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
using System.Threading;
using Symbol;
using Symbol.Barcode;

namespace ChristianHelle.Barcode.Scanners
{
    /// <summary>
    ///     Symbol Barcode scanner device
    /// </summary>
    public class SymbolScanner : IBarcodeScanner
    {
        private const int TimeoutMilliseconds = 10000;
        private readonly EventHandler scanEvent;

        private Reader reader;
        private ReaderData readerData;
        private ScannerStatus scannerStatus;
        private Timer timeout;

        /// <summary>
        ///     Create an instance of the Symbol Barcode Scanner device
        /// </summary>
        public SymbolScanner()
        {
            scanEvent = ScannedDataEvent;
            scannerStatus = ScannerStatus.Closed;
        }

        /// <summary>
        ///     Event raised when the scanned data changes
        /// </summary>
        public event EventHandler<ScannedDataEventArgs> Scanned;

        /// <summary>
        ///     Get the scannerStatus of the scanner device
        /// </summary>
        public ScannerStatus Status
        {
            get
            {
                if (reader != null)
                {
                    try
                    {
                        if (reader.Info.SoftTrigger)
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
        ///     Open the scanner device
        /// </summary>
        public void Open()
        {
            if (scannerStatus == ScannerStatus.Opened)
                return;

            try
            {
                reader = new Reader();
                readerData = new ReaderData(ReaderDataTypes.Text, ReaderDataLengths.MaximumLabel);
                reader.Actions.Enable();

                // SuperGros ProductNumber
                reader.Decoders.I2OF5.MinimumLength = 6;
                reader.Decoders.I2OF5.MaximumLength = 14;

                StartRead();

                scannerStatus = ScannerStatus.Opened;
            }
            catch
            {
                Close();
                throw;
            }
        }

        /// <summary>
        ///     Close the scanner device
        /// </summary>
        public void Close()
        {
            if (reader == null)
            {
                scannerStatus = ScannerStatus.Closed;
                return;
            }

            StopRead();

            reader.Actions.Disable();
            reader.Dispose();
            reader = null;
            readerData.Dispose();
            readerData = null;

            scannerStatus = ScannerStatus.Closed;
        }

        /// <summary>
        ///     Start Scanning
        /// </summary>
        public void Scan()
        {
            if (reader == null || Status != ScannerStatus.Opened)
                return;

            scannerStatus = ScannerStatus.Scanning;
            Thread.Sleep(10);

            reader.Actions.ToggleSoftTrigger();
            timeout = new Timer(TimeoutCallback, this, TimeoutMilliseconds, Timeout.Infinite);
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Called to free resources.
        /// </summary>
        /// <param name="disposing">Should be true when calling from Dispose().</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (timeout != null)
                {
                    timeout.Dispose();
                    timeout = null;
                }
                Close();
            }
        }

        private void ScannedDataEvent(object sender, EventArgs e)
        {
            if (reader == null)
                return;

            var data = reader.GetNextReaderData();
            if (data == null)
                return;

            if (data.Result == Results.SUCCESS)
            {
                HandleData(data);
                StartRead();
            }
        }

        private void StartRead()
        {
            reader.ReadNotify += scanEvent;
            reader.Actions.Read(readerData);
        }

        private void StopRead()
        {
            reader.Actions.Flush();
            reader.ReadNotify -= scanEvent;
        }

        private void HandleData(ReaderData data)
        {
            if (Scanned != null)
            {
                var barcodeData = new BarcodeData
                                  {
                                      Text = data.Text,
                                      BarcodeType = (BarcodeTypes) data.Type
                                  };
                Scanned.Invoke(this, new ScannedDataEventArgs(new[] {barcodeData}));
            }

            scannerStatus = ScannerStatus.Opened;
        }

        private void TimeoutCallback(object state)
        {
            var instance = (SymbolScanner) state;
            instance.reader.Info.SoftTrigger = false;
            instance.scannerStatus = ScannerStatus.Opened;
            timeout.Dispose();
        }
    }
}