using QRCoder;

namespace CleanArchitectureTemplate.SharedKernel.Utilities
{
    public static class QRGenerator
    {
        public static byte[] Generate(string data)
        {
            //return BitmapByteQRCodeHelper.GetQRCode(data, 20, "#ff265d", "#000000", QRCodeGenerator.ECCLevel.Q);

            QRCodeGenerator qrGenerator = new();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);

            var pngQR = new QRCoder.PngByteQRCode(qrCodeData);
            return pngQR.GetGraphic(20);
        }
    }
}
