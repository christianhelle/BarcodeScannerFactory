#region Imported Namespaces

using System;

#endregion

namespace ChristianHelle.Barcode
{
    /// <summary>
    /// Generic Interface that represents a barcode scanner device
    /// </summary>
    /// <remarks>
    /// It is recommended to use the <see cref="ReaderFactory"/> for creating instances
    /// of <see cref="IBarcodeScanner"/>. Since <see cref="IBarcodeScanner"/> implements <see cref="IDisposable"/>
    /// and the implementations must release resources in the Dispose method. It is 
    /// recommended to required  that the <see cref="IBarcodeScanner"/> instances is 
    /// Disposed when no longer to be in use.
    /// </remarks>
    /// <example>
    /// This example shows how you might use this interface through the ReaderFactory
    /// 
    /// <code>
    /// private IBarcodeScanner reader;
    /// 
    /// private string ScanBarcode()
    /// {
    ///     if (reader == null)
    ///         reader = new ReaderFactory().GetReader()
    ///     reader.Open();
    ///     reader.Scanned += OnScan;
    ///     reader.Scan();
    /// }
    /// 
    /// private void OnScan(object sender, ScannedDataEventArgs e)
    /// {
    ///     reader.Scanned += OnScan;
    ///     MessageBox.Show(e.Data[0].Text);
    /// }
    /// </code>
    /// </example>
    /// <example>
    /// This example shows how you might use this interface through the BarcodeScannerFacade
    /// 
    /// <code> 
    /// private string ScanBarcode()
    /// {
    ///     BarcodeScannerFacade.Instance.Open();
    ///     BarcodeScannerFacade.Instance.Scanned += OnScan;
    ///     BarcodeScannerFacade.Instance.Scan();
    /// }
    /// 
    /// private void OnScan(object sender, ScannedDataEventArgs e)
    /// {
    ///     BarcodeScannerFacade.Instance.Scanned += OnScan;
    ///     MessageBox.Show(e.Data[0].Text);
    /// }
    /// </code>
    /// </example>
    /// <example>
    /// This example shows how you might use this interface through the BarcodeReader. 
    /// This is not the recommended usage, and is marked as [Obsolete]
    /// 
    /// <code>
    /// private BarcodeReader reader;
    /// 
    /// private string ScanBarcode()
    /// {
    ///     if (reader == null)
    ///         reader = new BarcodeReader();
    ///     reader.Scanner.Open();
    ///     reader.Scanner.Scanned += OnScan;
    ///     reader.Scanner.Scan();
    /// }
    /// 
    /// private void OnScan(object sender, ScannedDataEventArgs e)
    /// {
    ///     reader.Scanner.Scanned += OnScan;
    ///     MessageBox.Show(e.Data[0].Text);
    /// }
    /// </code>
    /// </example>
    public interface IBarcodeScanner : IDisposable
    {
        /// <summary>
        /// Gets the status of the scanner
        /// </summary>
        ScannerStatus Status { get; }

        /// <summary>
        /// Event raised when new data has been scanned
        /// </summary>
        event EventHandler<ScannedDataEventArgs> Scanned;

        /// <summary>
        /// Opens the connection to the scanner
        /// </summary>
        void Open();

        /// <summary>
        /// Closes the connection to the scanner
        /// </summary>
        void Close();

        /// <summary>
        /// Manually initiate the scan
        /// </summary>
        /// <remarks>
        /// The scanned barcode will be received in the <see cref="ScannedDataEventArgs.Data"/>
        /// parameter when the <see cref="Scanned"/> event is fired
        /// </remarks>
        void Scan();

        /// <summary>
        /// The handle to the window in which notifications are sent (required for certain scanner API's)
        /// </summary>
        IntPtr Handle { get; set; }
    }
}