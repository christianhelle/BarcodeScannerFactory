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
