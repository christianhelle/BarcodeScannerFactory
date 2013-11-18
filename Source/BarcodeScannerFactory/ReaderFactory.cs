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
using ChristianHelle.Barcode.Scanners;

#endregion

namespace ChristianHelle.Barcode
{
    /// <summary>
    /// Factory class for creating instances of <see cref="IBarcodeScanner"/>
    /// </summary>
    /// <example>
    /// This example shows how you might use this class:
    /// 
    /// <code>
    /// ReaderFactory factory = new ReaderFactory();
    /// IBarcodeScanner reader = factory.GetReader();
    /// </code>
    /// 
    /// 
    /// Developers have the option of creating their own <see cref="IBarcodeScanner"/>
    /// imlpementation. It is recommended to use the <see cref="GetReader{T}"/> method 
    /// because it provides type checking. Call <see cref="GetReader{T}"/> 
    /// where T is the <see cref="IBarcodeScanner"/> implementation. 
    /// 
    /// Here is an example:
    /// 
    /// <code>
    /// try
    /// {
    ///     ReaderFactory factory = new ReaderFactory();
    ///     IBarcodeScanner reader = factory.GetReader<![CDATA[<IntermecScanner>]]>();
    /// }
    /// catch (TypeLoadException e) 
    /// {
    ///     Trace.WriteLine(e.Mesage);
    /// }
    /// </code>
    /// </example>
    public class ReaderFactory
    {
        private const int SPI_GETOEMINFO = 258;

        /// <summary>
        /// Gets an implementation instance of <see cref="IBarcodeScanner"/>
        /// </summary>
        /// <returns>
        /// Returns an instance of an <see cref="IBarcodeScanner"/> implementation based on the
        /// OEM info of the device
        /// </returns>
        /// <example>
        /// This example shows how you might use this method:
        /// 
        /// <code>
        /// ReaderFactory factory = new ReaderFactory();
        /// IBarcodeScanner reader = factory.GetReader();
        /// </code>
        /// </example>
        public IBarcodeScanner GetReader()
        {
            if (Environment.OSVersion.Platform != PlatformID.WinCE)
                return new DummyScanner();

            var oemInfo = Device.GetManufacturerInfo();
            oemInfo = oemInfo.ToUpper();

            if (oemInfo.Contains("INTERMEC"))
                return new IntermecScanner();
            if (oemInfo.Contains("SYMBOL"))
                return new SymbolScanner();
            if (oemInfo.Contains("MOTOROLA"))
                return new SymbolScanner();
            if (oemInfo.Contains("PY055"))
                return new CasioScanner();

            return new DummyScanner();
        }

        /// <summary>
        /// Gets an instance of <see cref="IBarcodeScanner"/> from the specified <see cref="Type"/>
        /// </summary>
        /// <typeparam name="T">An implementation of <see cref="IBarcodeScanner"/></typeparam>
        /// <returns>Returns an instance of <see cref="IBarcodeScanner"/></returns>
        /// <exception cref="TypeLoadException">
        /// Thrown if the type provided does not implement <see cref="IBarcodeScanner"/>
        /// </exception>
        /// <example>
        /// This example shows how you might use this method:
        /// 
        /// <code>
        /// try
        /// {
        ///     ReaderFactory factory = new ReaderFactory();
        ///     IBarcodeScanner reader = factory.GetReader<![CDATA[<IntermecScanner>]]>();
        /// }
        /// catch (TypeLoadException e) 
        /// {
        ///     Trace.WriteLine(e.Mesage);
        /// }
        /// </code>
        /// </example>
        public IBarcodeScanner GetReader<T>()
        {
            var instance = Activator.CreateInstance<T>() as IBarcodeScanner;
            if (instance == null)
                throw new TypeLoadException("Type provided does not implement IBarcodeScanner");

            return instance;
        }
    }
}