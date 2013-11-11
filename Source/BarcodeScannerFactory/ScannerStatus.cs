namespace ChristianHelle.Barcode
{
    /// <summary>
    /// Enum describing scanner status
    /// </summary>
    public enum ScannerStatus
    {
        /// <summary>
        /// Scanner is disposed or unavailable
        /// </summary>
        NotAvailable = 0,
        /// <summary>
        /// Scanner closed
        /// </summary>
        Closed,
        /// <summary>
        /// Scanner open and ready to scan
        /// </summary>
        Opened,
        /// <summary>
        /// Scanner is scanning
        /// </summary>
        Scanning
    }
}