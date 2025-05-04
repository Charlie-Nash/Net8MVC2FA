using QRCoder;

namespace WebAppSSO.Helpers
{
    public class QR
    {
        public static string GenerateCodeUri(string user, string? secretKey)
        {
            string issuer = "MVC_2FA_App";
            string uri = $"otpauth://totp/{issuer}:{user}?secret={secretKey}&issuer={issuer}&digits=6";

            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(uri, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new SvgQRCode(qrCodeData);
            string svgImage = qrCode.GetGraphic(5);

            return "data:image/svg+xml;base64," + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(svgImage));
        }
    }
}
