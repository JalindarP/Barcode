using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using ZXing;
using ZXing.Common;
using ZXing.OneD;
using ZXing.Rendering;

namespace Eurofins.Online.OrderQuery.Domain.Helpers
{
    public class PixelTextDataRenderer : IBarcodeRenderer<byte[]>
    {
        private readonly PixelDataRenderer _dataRenderer;
        private readonly string _displayText;
        private readonly Font _defaultTextFont = new Font("Arial", 10f, FontStyle.Regular);

        public PixelTextDataRenderer()
            : this(string.Empty, string.Empty, string.Empty)
        {
        }

        public PixelTextDataRenderer(string foregroundColor, string backgroundColor, string displayText)
        {
            _displayText = displayText;
            _dataRenderer = new PixelDataRenderer()
            {
                Background = new PixelDataRenderer.Color((string.IsNullOrEmpty(backgroundColor) ? Color.White : Color.FromName(backgroundColor)).ToArgb()),
                Foreground = new PixelDataRenderer.Color((string.IsNullOrEmpty(foregroundColor) ? Color.Black : Color.FromName(foregroundColor)).ToArgb())
            };
        }

        public byte[] Render(BitMatrix matrix, BarcodeFormat format, string content) => Render(matrix, format, content, null);

        public byte[] Render(BitMatrix matrix, BarcodeFormat format, string content, EncodingOptions options)
        {
            PixelData pixelData = _dataRenderer.Render(matrix, format, content, options);
            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb))
            using (var memoryStream = new MemoryStream())
            {
                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                    ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);

                byte[] source = new byte[bitmapData.Stride * matrix.Height];

                var num1 = matrix.Height + 16 > _defaultTextFont.Height ? _defaultTextFont.Height : 0;

                pixelData.Pixels.CopyTo(source, 0);

                try
                {
                    if (!options.PureBarcode && num1 > 0)
                    {
                        IsPureBarcodeWithText(matrix.Height, matrix.Width, bitmapData.Stride - (4 * matrix.Width), source, num1);
                    }

                    System.Runtime.InteropServices.Marshal.Copy(source, 0, bitmapData.Scan0, source.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }

                if (!options.PureBarcode && num1 > 0)
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        switch (format)
                        {
                            case BarcodeFormat.EAN_8:
                                if (content.Length < 8)
                                    content = OneDimensionalCodeWriter.CalculateChecksumDigitModulo10(content);
                                content = content.Insert(4, "   ");
                                break;
                            case BarcodeFormat.EAN_13:
                                if (content.Length < 13)
                                    content = OneDimensionalCodeWriter.CalculateChecksumDigitModulo10(content);
                                content = content.Insert(7, "   ");
                                content = content.Insert(1, "   ");
                                break;
                        }

                        SolidBrush solidBrush = new SolidBrush(Color.FromArgb(_dataRenderer.Foreground.A, _dataRenderer.Foreground.R, _dataRenderer.Foreground.G, _dataRenderer.Foreground.B));
                        StringFormat format1 = new StringFormat()
                        {
                            Alignment = StringAlignment.Center
                        };
                        graphics.DrawString(string.IsNullOrEmpty(_displayText) ? content : _displayText, _defaultTextFont, (Brush)solidBrush, (float)(1 * matrix.Width / 2), (float)(options.Height - num1), format1);
                    }
                }

                bitmap.Save(memoryStream, ImageFormat.Png);

                return memoryStream.ToArray();
            }
        }

        private void IsPureBarcodeWithText(int height, int width, int num4, byte[] source, int num1)
        {
            int num6 = ((width * 4) + num4) * (height - num1);
            for (int index1 = height - num1; index1 < height; ++index1)
            {
                for (int index2 = 0; index2 < width; ++index2)
                {
                    byte[] numArray1 = source;
                    int index3 = num6;
                    numArray1[index3] = this._dataRenderer.Background.B;

                    byte[] numArray2 = source;
                    int index4 = index3 + 1;
                    numArray2[index4] = this._dataRenderer.Background.G;

                    byte[] numArray3 = source;
                    int index5 = index4 + 1;
                    numArray3[index5] = this._dataRenderer.Background.R;

                    byte[] numArray4 = source;
                    int index6 = index5 + 1;
                    numArray4[index6] = this._dataRenderer.Background.A;

                    num6 = index6 + 1;
                }
            }
        }
    }
}
