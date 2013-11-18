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

#region Imported Namespaces

using System;
using System.Collections.Generic;

#endregion

namespace ChristianHelle.Barcode
{
    /// <summary>
    /// Event arguments for the <see cref="IBarcodeScanner.Scanned"/> event
    /// </summary>
    public class ScannedDataEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ScannedDataEventArgs"/>
        /// </summary>
        /// <param name="data"></param>
        public ScannedDataEventArgs(IEnumerable<BarcodeData> data)
        {
            Data = data;
        }

        /// <summary>
        /// Gets the scanned data
        /// </summary>
        /// <remarks>
        /// Some scanners are capable of reading several barcodes at once,
        /// if the scanner is not capable of this then the barcode is obviously
        /// stored in the first index of the string array (<see cref="Data"/>[0])
        /// </remarks>
        public IEnumerable<BarcodeData> Data { get; private set; }
    }
}