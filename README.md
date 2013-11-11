BarcodeScannerFactory
=====================

A simplified abstraction over a collection of barcode scanner SDK's 
with device detetction support for the .NET Compact Framework 3.5

Supports the following device: 
 - Intermec
 - Motorola
 - Symbol
 - Casio


It is recommended to use the **ReaderFactory** for creating instances
of **IBarcodeScanner**. Since **IBarcodeScanner** implements **IDisposable**
and the implementations must release resources in the Dispose method. It is 
recommended to require that the **IBarcodeScanner** instances is 
Disposed when no longer to be in use, otherwise the device might end up in 
a very unstable state and might require a restart.

Another way to get an instance of **IBarcodeScanner** is through the **BarcodeScannerFacade**. 
This class implements a facade over retrieving the correct implementation of **IBarcodeScanner**
based on the OEM information provided by the operating system. The **BarcodeScannerFacade**
also exposes a singleton instance of **IBarcodeScanner** which might be useful in a scenario
in which changing the state of the scanner takes too much time, or where barcode scanning is
enabled everywhere in the application


This example shows how you might use this interface through the **ReaderFactory**
```c#
private IBarcodeScanner reader;

private string ScanBarcode()
{
    if (reader == null)
        reader = new ReaderFactory().GetReader()
    reader.Open();
    reader.Scanned += OnScan;
    reader.Scan();
}

private void OnScan(object sender, ScannedDataEventArgs e)
{
    reader.Scanned += OnScan;
    MessageBox.Show(e.Data[0]);
}
```

This example shows how you might use this interface through the **BarcodeReader**
```c#
private BarcodeReader reader;

private string ScanBarcode()
{
    if (reader == null)
        reader = new BarcodeReader();
    reader.Scanner.Scanned += OnScan;
    reader.Scanner.Open();
    reader.Scanner.Scan();
}

private void OnScan(object sender, ScannedDataEventArgs e)
{
    MessageBox.Show(e.Data[0]);
}
```

This example shows how you might use this interface through the **BarcodeScannerFacade**
```c#
private string ScanBarcode()
{
    BarcodeScannerFacade.Instance.Scanned += OnScan;
    BarcodeScannerFacade.Instance.Open();
    BarcodeScannerFacade.Instance.Scan();
}

private void OnScan(object sender, ScannedDataEventArgs e)
{
    MessageBox.Show(e.Data[0]);
}
```
