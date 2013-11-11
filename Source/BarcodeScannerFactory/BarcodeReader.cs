#region Imported Namespaces

using System;

#endregion

namespace ChristianHelle.Barcode
{
    /// <summary>
    /// Class for reading barcodes provided by a barcode scanner
    /// </summary>
    [Obsolete("Use the BarcodeScannerFacade instead", false)]
    public class BarcodeReader : IDisposable
    {
        /// <summary>
        /// Creates the default instance of 
        /// </summary>
        public BarcodeReader()
        {
            var factory = new ReaderFactory();
            Scanner = factory.GetReader();
        }

        /// <summary>
        /// Creates an instance of the <see cref="BarcodeReader"/>
        /// </summary>
        /// <param name="typeName">
        /// Specified type name of the assembly to load as <see cref="IBarcodeScanner"/>
        /// </param>
        public BarcodeReader(string typeName)
        {
            LoadType(typeName);
        }

        /// <summary>
        /// Creates an instance of the <see cref="BarcodeReader"/>
        /// </summary>
        /// <param name="type">
        /// Specified type of the assembly to load as <see cref="IBarcodeScanner"/>
        /// </param>
        public BarcodeReader(Type type)
        {
            LoadType(type);
        }

        /// <summary>
        /// Event raised when new data has been scanned
        /// </summary>
        public IBarcodeScanner Scanner { get; private set; }

        /// <summary>
        /// Disposes the <see cref="BarcodeReader"/>
        /// </summary>
        public void Dispose()
        {
            if (Scanner == null)
                return;
            Scanner.Dispose();
            Scanner = null;
        }

        private void LoadType(string typeName)
        {
            var type = Type.GetType(typeName);
            LoadType(type);
        }

        private void LoadType(Type type)
        {
            var typeLoadEx = new TypeLoadException(
                string.Format("Unable to GetType() for {0}. Check if the Type name is correct", type));

            var typeInitEx = new TypeInitializationException(
                string.Format("Unable to load {0}. Check if the Type name is correct", type),
                typeLoadEx);
            if (type == null)
                throw typeLoadEx;

            Scanner = (IBarcodeScanner)Activator.CreateInstance(type);
            if (Scanner == null)
                throw typeInitEx;
        }
    }
}