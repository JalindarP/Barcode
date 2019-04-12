using System;
using Eurofins.Online.OrderQuery.Contract.Models;
using ZXing;
using ZXing.Common;

namespace Eurofins.Online.OrderQuery.Domain.Helpers
{
    public static class BarCodeHelper
    {
        public static byte[] GetBarcodeImage(BarcodeRequest barcodeRequest)
        {
            if (!Enum.TryParse(barcodeRequest.BarcodeType.ToUpper(), out BarcodeFormat barcodeFormat))
                barcodeFormat = BarcodeFormat.CODE_128;

            return new BarcodeWriter<byte[]>()
            {
                Format = barcodeFormat,
                Options = new EncodingOptions
                {
                    Height = barcodeRequest.Height,
                    Width = barcodeRequest.Width,
                    Margin = barcodeRequest.Margin,
                    PureBarcode = !barcodeRequest.IsTextDisplay,
                },
                Renderer = new PixelTextDataRenderer(barcodeRequest.ForegroundColor, barcodeRequest.BackgroundColor, barcodeRequest.Content.DisplayText)
            }.Write(barcodeRequest.Content.Content);

            //return new BarcodeWriter<byte[]>()
            //{
            //    Format = barcodeFormat,
            //    Options = new EncodingOptions
            //    {
            //        Height = barcodeRequest.Height,
            //        Width = barcodeRequest.Width,
            //        Margin = barcodeRequest.Margin,
            //        PureBarcode = !barcodeRequest.IsTextDisplay,
            //    },
            //    Renderer = new BitmapRenderer(barcodeRequest.ForegroundColor, barcodeRequest.BackgroundColor, barcodeRequest.Content.DisplayText)
            //}.Write(barcodeRequest.Content.Content);
        }
    }
}
