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