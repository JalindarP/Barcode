using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

using ZXing;
using ZXing.Common;
using ZXing.OneD;
using ZXing.Rendering;

namespace Eurofins.Online.OrderQuery.Domain.Helpers
{
    public class BitmapRenderer : IBarcodeRenderer<byte[]>
    {
        private readonly Font _textFont = new Font("Arial", 10f, FontStyle.Regular);

        private readonly Color _foreground;

        private readonly Color _background;

        private readonly string _displayText;

        public BitmapRenderer()
            : this(string.Empty, string.Empty, string.Empty)
        {
        }

        public BitmapRenderer(string foregroundColor, string backgroundColor, string displayText)
        {
            _displayText = displayText;
            this._foreground = string.IsNullOrEmpty(foregroundColor) ? Color.Empty : Color.FromName(foregroundColor);
            this._background = string.IsNullOrEmpty(backgroundColor) ? Color.Empty : Color.FromName(backgroundColor);
        }

        public byte[] Render(BitMatrix matrix, BarcodeFormat format, string content) => Render(matrix, format, content, null);

        public virtual byte[] Render(BitMatrix matrix, BarcodeFormat format, string content, EncodingOptions options)
        {
            int width = matrix.Width;
            int height1 = matrix.Height;
            int num1 = 0;
            if (options != null)
            {
                if (options.Width > width)
                    width = options.Width;
                if (options.Height > height1)
                    height1 = options.Height;
            }

            int num2 = width / matrix.Width;
            int num3 = height1 / matrix.Height;
            using (MemoryStream ms = new MemoryStream())
            using (Bitmap bitmap = new Bitmap(width, height1, PixelFormat.Format24bppRgb))
            using (Graphics graphics = Graphics.FromImage((Image)bitmap))
            {
                BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                try
                {
                    byte[] source = new byte[bitmapdata.Stride * height1];
                    int num4 = bitmapdata.Stride - (3 * width);
                    int num5 = 0;
                    for (int index1 = 0; index1 < matrix.Height; ++index1)
                    {
                        for (int index2 = 0; index2 < num3; ++index2)
                        {
                            for (int index3 = 0; index3 < matrix.Width; ++index3)
                            {
                                Color color = matrix[index3, index1] ? this._foreground : this._background;
                                for (int index4 = 0; index4 < num2; ++index4)
                                {
                                    byte[] numArray1 = source;
                                    int index5 = num5;
                                    int num6 = 1;
                                    int num7 = index5 + num6;
                                    int num8 = (int)color.B;
                                    numArray1[index5] = (byte)num8;
                                    byte[] numArray2 = source;
                                    int index6 = num7;
                                    int num9 = 1;
                                    int num10 = index6 + num9;
                                    int num11 = (int)color.G;
                                    numArray2[index6] = (byte)num11;
                                    byte[] numArray3 = source;
                                    int index7 = num10;
                                    int num12 = 1;
                                    num5 = index7 + num12;
                                    int num13 = (int)color.R;
                                    numArray3[index7] = (byte)num13;
                                }
                            }

                            for (int index3 = num2 * matrix.Width; index3 < width; ++index3)
                            {
                                byte[] numArray1 = source;
                                int index4 = num5;
                                int num6 = 1;
                                int num7 = index4 + num6;
                                int num8 = (int)this._background.B;
                                numArray1[index4] = (byte)num8;
                                byte[] numArray2 = source;
                                int index5 = num7;
                                int num9 = 1;
                                int num10 = index5 + num9;
                                int num11 = (int)this._background.G;
                                numArray2[index5] = (byte)num11;
                                byte[] numArray3 = source;
                                int index6 = num10;
                                int num12 = 1;
                                num5 = index6 + num12;
                                int num13 = (int)this._background.R;
                                numArray3[index6] = (byte)num13;
                            }

                            num5 += num4;
                        }
                    }

                    for (int index1 = num3 * matrix.Height; index1 < height1; ++index1)
                    {
                        for (int index2 = 0; index2 < width; ++index2)
                        {
                            byte[] numArray1 = source;
                            int index3 = num5;
                            int num6 = 1;
                            int num7 = index3 + num6;
                            int num8 = (int)this._background.B;
                            numArray1[index3] = (byte)num8;
                            byte[] numArray2 = source;
                            int index4 = num7;
                            int num9 = 1;
                            int num10 = index4 + num9;
                            int num11 = (int)this._background.G;
                            numArray2[index4] = (byte)num11;
                            byte[] numArray3 = source;
                            int index5 = num10;
                            int num12 = 1;
                            num5 = index5 + num12;
                            int num13 = (int)this._background.R;
                            numArray3[index5] = (byte)num13;
                        }

                        num5 += num4;
                    }

                    if (options != null && !options.PureBarcode)
                    {
                        int height2 = _textFont.Height;
                        num1 = height1 + 10 > height2 ? height2 : 0;
                        if (num1 > 0)
                        {
                            int num6 = ((width * 3) + num4) * (height1 - num1);
                            for (int index1 = height1 - num1; index1 < height1; ++index1)
                            {
                                for (int index2 = 0; index2 < width; ++index2)
                                {
                                    byte[] numArray1 = source;
                                    int index3 = num6;
                                    int num7 = 1;
                                    int num8 = index3 + num7;
                                    int num9 = (int)this._background.B;
                                    numArray1[index3] = (byte)num9;
                                    byte[] numArray2 = source;
                                    int index4 = num8;
                                    int num10 = 1;
                                    int num11 = index4 + num10;
                                    int num12 = (int)this._background.G;
                                    numArray2[index4] = (byte)num12;
                                    byte[] numArray3 = source;
                                    int index5 = num11;
                                    int num13 = 1;
                                    num6 = index5 + num13;
                                    int num14 = (int)this._background.R;
                                    numArray3[index5] = (byte)num14;
                                }

                                num6 += num4;
                            }
                        }
                    }

                    Marshal.Copy(source, 0, bitmapdata.Scan0, source.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapdata);
                }

                if (num1 > 0)
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

                    SolidBrush solidBrush = new SolidBrush(this._foreground);
                    StringFormat format1 = new StringFormat()
                    {
                        Alignment = StringAlignment.Center
                    };
                    graphics.DrawString(string.IsNullOrEmpty(_displayText) ? content : _displayText, _textFont, (Brush)solidBrush, (float)(num2 * matrix.Width / 2), (float)(height1 - num1), format1);
                }

                bitmap.Save(ms, ImageFormat.Gif);
                return ms.ToArray();
            }
        }
    }
}
