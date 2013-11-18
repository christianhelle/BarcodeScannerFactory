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

namespace ChristianHelle.Barcode
{
    /// <summary>
    /// Contains the barcode data
    /// </summary>
    public class BarcodeData
    {
        /// <summary>
        /// Returns the scanned data text.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// A string of the scanned data if the memory buffer has been set to text mode.
        /// 
        /// </value>
        /// <exception cref="T:Symbol.Exceptions.InvalidDataTypeException">An InvalidDataTypeException
        ///             is thrown if this property is accessed and they memory buffer has been set
        ///             to binary mode. </exception>
        public string Text { get; set; }

        /// <summary>
        /// Barcode decode type
        /// </summary>
        public BarcodeTypes BarcodeType { get; set; }
    }
}