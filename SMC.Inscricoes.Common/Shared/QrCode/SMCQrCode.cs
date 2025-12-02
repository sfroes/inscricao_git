using MessagingToolkit.QRCode.Codec;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SMC.Inscricoes.Common.Shared.QrCode
{
    public static class SMCQrCode
    {
        private const string CHARSET = "UTF-8";

        /// <summary>
        /// Transformar o texto em QrCode (imagem)
        /// </summary>
        /// <param name="dados"></param>
        /// <returns></returns>
        public static Bitmap CreateQrCode(string dados)
        {
            return CreateQrCodeCustom(dados, System.Drawing.Color.White, System.Drawing.Color.Black);
        }

        /// <summary>
        /// Transformar o texto em QrCode (imagem), customiza cores de fundo e de preenchimento
        /// </summary>
        /// <param name="dados"></param>
        /// <param name="BackgroundColor"> Cor de fundo </param>
        /// <param name="ForegroundColor">Cor do primeiro plano </param>
        /// <returns></returns>
        public static Bitmap CreateQrCodeCustom(string dados, Color BackgroundColor, Color ForegroundColor)
        {
            QRCodeEncoder qrCodecEncoder = new QRCodeEncoder();
            qrCodecEncoder.QRCodeBackgroundColor = BackgroundColor;
            qrCodecEncoder.QRCodeForegroundColor = ForegroundColor;
            qrCodecEncoder.CharacterSet = SMCQrCode.CHARSET;
            qrCodecEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodecEncoder.QRCodeScale = 6;
            qrCodecEncoder.QRCodeVersion = 0;
            qrCodecEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;

            var imageQRCode = qrCodecEncoder.Encode(dados);
            return imageQRCode;
        }

        public static string ConvertBitmapInToBase64(Bitmap img, ImageFormat output)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, output);

                byte[] imgBytes = ms.ToArray();

                return Convert.ToBase64String(imgBytes);
            }
        }

        public static string ConvertToWeb(Bitmap img, ImageFormat output)
        {
            var format = output.ToString();

            var conversion = ConvertBitmapInToBase64(img, output);

            var concat = "data:" + "image/" + format + ";base64," + conversion;

            return concat;

        }
    }
}
