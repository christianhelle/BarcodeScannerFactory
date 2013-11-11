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