# Payment.QrPayload

> A robust, lightweight, and extensible .NET library for generating EMVCo-compliant QR Code payloads (focused on VietQR/Napas247 support).

![License](https://img.shields.io/badge/license-Unlicense-green)
![Platform](https://img.shields.io/badge/platform-.NET%20Standard%202.0-blue)

## 📖 Overview

**Payment.QrPayload** simplifies the process of generating QR strings (payloads) used in banking and payment applications. It handles the complexity of the **EMVCo** standard and **VietQR** specifications, allowing you to focus on your business logic.

Unlike other libraries, it provides a strictly typed, object-oriented approach while remaining easy to use for simple cases.

## ✨ Features

- **No External Dependencies**: Built on pure `.NET Standard 2.0`, ensuring compatibility with .NET Framework, .NET Core, .NET 5/6/7/8+, and Xamarin/MAUI.
- **Strongly Typed**: Uses Enums for `CurrencyCode`, `CountryCode`, `PaymentService`, and schemes to eliminate "magic strings" and runtime errors.
- **SOLID Architecture**: Built around the `IQrConfig` interface, making it easy to extend for new payment providers or country standards.
- **VietQR Ready**: Out-of-the-box support for Napas247 (VietQR) with default GUIDs and service codes.
- **High Performance**: Optimized string builder and CRC16 calculation.

# Payment Providers 🎉💳

This project includes support or recognition for various payment providers. Below is a lively list of providers with their GUIDs (as found in Payment.QrPayload/Enums/PaymentProvider.cs) and a short note.

- Napas — `0010A000000727` — Vietnam national payment network 🇻🇳
- Visa — `0010A000000003` — Visa (global) 🌐
- Mastercard — `0010A000000004` — Mastercard (global) 🌐
- UnionPay — `0010A000000333` — UnionPay (China) 🇨🇳
- JCB — `0010A000000065` — JCB (Japan) 🇯🇵
- AmericanExpress — `0010A000000025` — American Express (global) ✨
- Discover — `0010A000000152` — Discover (US-centric) 🇺🇸
- PromptPay — `0010A000000677` — Thailand PromptPay (GUID: A000000677) 🇹🇭
- PayNow — `0009SG.PAYNOW` — Singapore PayNow (GUID: SG.PAYNOW) 🇸🇬
- DuitNow — `0010A000000762` — Malaysia DuitNow (GUID: A000000762) 🇲🇾
- QRIS — `0014ID.CO.QRIS.WWW` — Indonesia QRIS (GUID: ID.CO.QRIS.WWW) 🇮🇩
- Pix — `0014BR.GOV.BCB.PIX` — Brazil Pix (GUID: BR.GOV.BCB.PIX) 🇧🇷
- AliPay — `0010ALIPAY.COM` — AliPay (ALIPAY.COM) 🧧
- WeChatPay — `0010TENPAY.COM` — WeChat Pay / Tenpay (TENPAY.COM) 🟢

Have fun — feel free to tweak emojis or add links to provider docs for extra "xôm" ✨

# Top Payment Services 💸

Below are the most common service codes available in the library (`Payment.QrPayload/Enums/PaymentService.cs`), supporting various QR standards.

- 🏦 **Napas247 Account** — `0208QRIBFTTA` — Instant transfer to bank account (Vietnam) 🇻🇳
- 💳 **Napas247 Card** — `0208QRIBFTTC` — Instant transfer to card number (Vietnam) 🇻🇳
- 👛 **PromptPay E-Wallet** — `29` — E-Wallet ID (Thailand) 🇹🇭
- 🧾 **PromptPay Bill Payment** — `30` — Bill Payment (Thailand) 🇹🇭
- 🏢 **PayNow UEN** — `2` — Unique Entity Number (Singapore B2B) 🇸🇬
- 🆔 **PayNow NRIC** — `1` — National Registration Identity Card (Singapore) 🇸🇬
- 📱 **PayNow Mobile** — `0` — Mobile Number (Singapore) 🇸🇬
- 📤 **UPI Collect** — `UPI` — Unified Payments Interface (India) 🇮🇳
- 🔑 **Pix Key** — *(Dynamic)* — Brazil Instant Payment (Brazil) 🇧🇷
- 🧧 **AliPay User** — `ALIPAY` — AliPay User ID (China) 🇨🇳
- 💬 **WeChat Pay User** — `TENPAY` — WeChat Pay / Tenpay (China) 🇨🇳

## 📦 Installation

Install usage via NuGet (replace `Version` with the latest release):

```bash
dotnet add package Anpero.Payment.QrPayload --version 1.0.1
```

Or via Package Manager Console:

```powershell
Install-Package Anpero.Payment.QrPayload
```

## 🚀 Usage

### 1. Simple Usage (Quick Start)

If you just need to generate a VietQR for a bank transfer, use the static helper method. This is great for quick integrations or migrating from legacy code.

```csharp
using Payment.QrPayload;
using Payment.QrPayload.Enums;

var result = QrPayloadGenerator.GeneratePayload(
    bankAccount: "1234567890",
    bankId: "970422",       // Example: MBBank Bin Code
    message: "Payment for Order 123",
    amount: "50000"         // Amount in VND
);

if (result.IsSuccess)
{
    Console.WriteLine($"QR Raw String: {result.Content}");
    // Output: 00020101021238570010A00000072701270006970422011012345678900208QribftTA53037045405500005802VN62230819Payment for Order 1236304D91C
    
    // Now pass 'result.Content' to any QR Code rendering library (e.g. QRCoder)
}
else
{
    Console.WriteLine($"Error: {result.Message}");
}
```

### 2. Advanced Usage (Object-Oriented)

For more control, or when dealing with different currencies/services, use the `VnQrConfig` class. This adheres to the cleaner `IQrConfig` pattern.

```csharp
using Payment.QrPayload;
using Payment.QrPayload.Models;
using Payment.QrPayload.Enums;

// Create a configuration object
var config = new VnQrConfig
{
    BankId = "970415",          // VietinBank
    BankAccount = "1133667788",
    Amount = "150000",
    Message = "Dinner split",
    
    // Optional Overrides (Defaults are naturally set for VietQR)
    CountryCode = CountryCode.VN,
    Currency = CurrencyCode.VND, 
    ServiceCode = PaymentService.Napas247_Account,
    Guid = PaymentProvider.Napas
};

// Generate
var result = QrPayloadGenerator.Generate(config);

if (result.IsSuccess)
{
    // Success logic
}
```

### 3. Extending to Other Payement Standards

The library is designed to be extensible. You can implement the `IQrConfig` interface to support other EMVCo implementations (e.g., PromptPay, PayNow).

```csharp
public class MyCustomQrConfig : IQrConfig
{
    public string GetPayload()
    {
        // Build your custom EMVCo string here
        // The Generator will automatically handle CRC16 appending
        return "000201..."; 
    }
}
```

## 🤝 Contributing

Contributions are welcome! Please follow these steps:
1. Fork the repository.
2. Create a feature branch (`git checkout -b feature/NewProvider`).
3. Commit your changes.
4. Push to the branch.
5. Open a Pull Request.

## 📄 License

This project is released under the **Unlicense** (Public Domain). You are free to use it for any purpose, commercial or private.
