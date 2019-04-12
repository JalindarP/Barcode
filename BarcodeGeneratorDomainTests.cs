using System.Collections.Generic;
using System.IO;
using Eurofins.Online.OrderQuery.Contract;
using Eurofins.Online.OrderQuery.Contract.Models;
using Eurofins.Online.OrderQuery.Domain;
using Shouldly;
using Xunit;

namespace Eurofins.Online.OrderQuery.UnitTests
{
    public sealed class BarcodeGeneratorDomainTests
    {
        private readonly IBarcodeGeneratorService _subjectUnderTest;

        public BarcodeGeneratorDomainTests()
        {
            _subjectUnderTest = new BarcodeGeneratorDomain();
        }

        [Fact(DisplayName = "Legacy bar code", Skip = "as failing on git lab linux server")]
        public void Legacy_Barcode()
        {
            var request = new BarcodeRequest
            {
                Content = new BarcodeContent { Content = "005079500003995086", DisplayText = "005-07950-0003995086-58" },
                Margin = 0,
                Width = 200,
                Height = 35,
                BarcodeType = "CODE_128",
                ForegroundColor = "Black",
                BackgroundColor = "White",
                ImageFormat = "bmp",
                IsTextDisplay = true
            };
            var response = _subjectUnderTest.GetBarcodeBytes(request);
            response.ShouldNotBeNull();

            string[] datas = Spire.Barcode.BarcodeScanner.Scan(new MemoryStream(response));
            datas[0].ShouldBe("005079500003995086");
        }

        [Fact(DisplayName = "Ng bar code", Skip = "as failing on git lab linux server")]
        public void Ng_Barcode()
        {
            var request = new BarcodeRequest
            {
                Content = new BarcodeContent { Content = "005-7950-3995086", DisplayText = "005-7950-3995086" },
                Margin = 0,
                Width = 200,
                Height = 35,
                BarcodeType = "CODE_128",
                ForegroundColor = "Black",
                BackgroundColor = "White",
                ImageFormat = "jpg",
                IsTextDisplay = true
            };
            var response = _subjectUnderTest.GetBarcodeBytes(request);
            response.ShouldNotBeNull();

            string[] datas = Spire.Barcode.BarcodeScanner.Scan(new MemoryStream(response));
            datas[0].ShouldBe("005-7950-3995086");
        }

        [Fact(DisplayName = "Legacy bar codes", Skip = "as failing on git lab linux server")]
        public void Legacy_Barcodes()
        {
            var request = new BarcodesRequest
            {
                Contents = new List<BarcodeContent>
                {
                new BarcodeContent { Content = "005079500003995086", DisplayText = "005-07950-0003995086-58" },
                new BarcodeContent { Content = "005079500003995087", DisplayText = "005-07950-0003995087-59" }
                },
                Margin = 0,
                Width = 200,
                Height = 35,
                BarcodeType = "CODE_128",
                ForegroundColor = "Black",
                BackgroundColor = "White",
                ImageFormat = "jpg",
                IsTextDisplay = true
            };
            var response = _subjectUnderTest.GetBarcodesBytes(request);
            response.ShouldNotBeNull();
            response.Count.ShouldBe(2);

            string[] datas = Spire.Barcode.BarcodeScanner.Scan(new MemoryStream(response[0]));
            datas[0].ShouldBe("005079500003995086");

            datas = Spire.Barcode.BarcodeScanner.Scan(new MemoryStream(response[1]));
            datas[0].ShouldBe("005079500003995087");
        }

        [Fact(DisplayName = "Ng bar codes", Skip ="as failing on git lab linux server")]
        public void Ng_Barcodes()
        {
            var request = new BarcodesRequest
            {
                Contents = new List<BarcodeContent>
                {
                new BarcodeContent { Content = "005-7950-3995086", DisplayText = "005-7950-3995086" },
                new BarcodeContent { Content = "005-7950-3995087", DisplayText = "005-7950-3995087" }
                },
                Margin = 0,
                Width = 200,
                Height = 35,
                BarcodeType = "CODE_128",
                ForegroundColor = "Black",
                BackgroundColor = "White",
                ImageFormat = "jpg",
                IsTextDisplay = true
            };
            var response = _subjectUnderTest.GetBarcodesBytes(request);
            response.ShouldNotBeNull();
            response.Count.ShouldBe(2);

            string[] datas = Spire.Barcode.BarcodeScanner.Scan(new MemoryStream(response[0]));
            datas[0].ShouldBe("005-7950-3995086");

            datas = Spire.Barcode.BarcodeScanner.Scan(new MemoryStream(response[1]));
            datas[0].ShouldBe("005-7950-3995087");
        }
    }
}
