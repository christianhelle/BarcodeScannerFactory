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

using ChristianHelle.Barcode.Scanners;

namespace ChristianHelle.Barcode
{
    /// <summary>
    /// Represents the barcode scanner as a facade
    /// </summary>
    /// <remarks>
    /// This class creates an instance of the <see cref="IBarcodeScanner"/> using the 
    /// <see cref="ReaderFactory.GetReader"/>, this detects the manufacturer of the device
    /// and loads the corresponding <see cref="IBarcodeScanner"/> implementations.
    /// 
    /// Currently the <see cref="ReaderFactory"/> only supports Intermec devices (<see cref="IntermecScanner"/>),
    /// Symbol devices (<see cref="SymbolScanner"/>), and Casio devices (<see cref="CasioScanner"/>).
    /// 
    /// The developer is responsible of maintaining the state of the scanner and it is
    /// very important that the scanner is closed before the application exits. This is
    /// due to the fact it is the application who holds the instance of the scanner 
    /// and only the application that openned it can close it. If the scanner is not 
    /// closed properly by the application then no other application can open the scanner.
    /// In most cases, not even the application that openned and forgot to close it in the
    /// first place can open the scanner.
    /// </remarks>
    /// <example>
    /// This example shows how you might use this class
    /// 
    /// <code> 
    /// private string ScanBarcode()
    /// {
    ///     BarcodeScannerFacade.Instance.Scanned += OnScan;
    ///     BarcodeScannerFacade.Instance.Open();
    ///     BarcodeScannerFacade.Instance.Scan();
    /// }
    /// 
    /// private void OnScan(object sender, ScannedDataEventArgs e)
    /// {
    ///     MessageBox.Show(e.Data[0]);
    /// }
    /// </code>
    /// </example>
    public static class BarcodeScannerFacade
    {
        private static readonly object syncLock = new object();
        private static IBarcodeScanner instance;

        /// <summary>
        /// Singleton instance of the barcode scanner 
        /// </summary>
        public static IBarcodeScanner Instance
        {
            get
            {
                lock (syncLock)
                {
                    if (instance == null)
                    {
                        var factory = new ReaderFactory();
                        instance = factory.GetReader();
                    }
                    return instance;
                }
            }
        }
    }
}