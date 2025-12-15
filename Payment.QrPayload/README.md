# Payment.QrPayload

Library for generating QR Code payloads for payments (EMVCo/VietQR).

## Features
- **SOLID Design**: Extensible interface `IQrConfig` for different standards.
- **Easy to Use**: Simple static methods for quick integration.
- **VietQR Support**: Out-of-the-box support for Vietnam Banking QR (Napas247).
- **No Dependencies**: Lightweight `netstandard2.0` library.

## Installation
Install via NuGet (Package name TBD once published).

## Usage

### Simple Usage (Vietnam)
This method mimics the legacy parameters for easy migration.

```csharp
using Payment.QrPayload;

var result = QrPayloadGenerator.GeneratePayload(
    bankAccount: "123456789",
    bankId: "970422", // MBBank
    message: "Payment for Order #123",
    amount: "50000"
);

if (result.IsSuccess)
{
    Console.WriteLine($"QR Content: {result.Content}");
    // Use the content to generate a QR Image (e.g., using QRCoder or Google Charts API)
}
else
{
    Console.WriteLine($"Error: {result.Message}");
}
```

### Advanced Usage (Object-Oriented)

You can use the `VnQrConfig` class or implement your own `IQrConfig`.

```csharp
using Payment.QrPayload;
using Payment.QrPayload.Models;

var config = new VnQrConfig
{
    BankId = "970422",
    BankAccount = "123456789",
    Amount = "50000",
    Message = "Order 123",
    // Optional
    Currency = "704",
    CountryCode = "VN"
};

var result = QrPayloadGenerator.Generate(config);
```

## Extending to other countries
Implement `IQrConfig` interface:

```csharp
public class MyCountryQrConfig : IQrConfig
{
    public string GetPayload()
    {
        // Custom EMVCo string construction
        return "...";
    }
}
```
