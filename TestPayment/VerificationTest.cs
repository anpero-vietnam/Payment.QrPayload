
using Payment.QrPayload;
using Payment.QrPayload.Enums;
using Payment.QrPayload.Models;

namespace TestPayment
{
    [TestClass]
    public class VerificationTest
    {
        [TestMethod]
        public void TestGeneratePayload_WithEnums_ReturnsSuccess()
        {
            // Arrange
            string bankAccount = "005704060117832";
            string bankId = "970415";
            string message = "Test Payment";
            string amount = "10000";

            // Act
            QrPayloadResult result = QrPayloadGenerator.GeneratePayload(
                bankAccount,
                bankId,
                message,
                amount,
                CurrencyCode.VND,
                PaymentService.Napas247_Account,
                CountryCode.VN,
                PaymentProvider.Napas
            );

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsNotNull(result.Content);
            // Verify payload contains known segments
            // 000201 -> Format
            // 010212 -> Initiation
            // 38.. -> Merchant Info
            // 5303704 -> Currency VND
            // 540510000 -> Amount (10000 is len 5)
            // 5802VN -> Country
            // 62.. -> Additional Data

            Assert.IsTrue(result.Content.StartsWith("000201010212"));
            Assert.IsTrue(result.Content.Contains("5303704"));
            Assert.IsTrue(result.Content.Contains("5802VN"));
        }

        [TestMethod]
        public void TestGeneratePayload_DefaultParams_ReturnsSuccess()
        {
            // Arrange
            string bankAccount = "005704060117832";
            string bankId = "970415";
            string message = "Test Default";
            string amount = "50000";

            // Act
            QrPayloadResult result = QrPayloadGenerator.GeneratePayload(
                bankAccount,
                bankId,
                message,
                amount
            );

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(result.Content.Contains("5303704")); // VNDDefault
            Assert.IsTrue(result.Content.Contains("5802VN")); // VN Default
        }
    }
}
