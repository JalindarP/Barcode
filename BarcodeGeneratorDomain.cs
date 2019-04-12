using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eurofins.Online.OrderQuery.Contract;
using Eurofins.Online.OrderQuery.Contract.Models;
using Eurofins.Online.OrderQuery.Domain.Helpers;

namespace Eurofins.Online.OrderQuery.Domain
{
    public class BarcodeGeneratorDomain : IBarcodeGeneratorService
    {
        byte[] IBarcodeGeneratorService.GetBarcodeBytes(BarcodeRequest barcodeRequest)
        {
            return BarCodeHelper.GetBarcodeImage(barcodeRequest);
        }

        IList<byte[]> IBarcodeGeneratorService.GetBarcodesBytes(BarcodesRequest barcodesRequest)
        {
            IList<byte[]> list = new List<byte[]>();
            barcodesRequest.Contents.ToList().ForEach(x =>
            {
              var bytes = BarCodeHelper.GetBarcodeImage(new BarcodeRequest()
                {
                    BackgroundColor = barcodesRequest.BackgroundColor,
                    ForegroundColor = barcodesRequest.ForegroundColor,
                    BarcodeType = barcodesRequest.BarcodeType,
                    Height = barcodesRequest.Height,
                    Width = barcodesRequest.Width,
                    Margin = barcodesRequest.Margin,
                    Content = new BarcodeContent()
                    {
                        Content = x.Content,
                        DisplayText = x.DisplayText
                    },
                    ImageFormat = barcodesRequest.ImageFormat
                });

                list.Add(bytes);
            });

            return list;
        }
    }
}
