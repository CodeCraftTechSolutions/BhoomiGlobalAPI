using DinkToPdf;

namespace BhoomiGlobalAPI.HelperClass
{
    public static class PrintPdfHelper
    {
        public static HtmlToPdfDocument CreateTablePDF(string templateHtml)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
                //Out = @"D:\PDFCreator\Employee_Report.pdf"  USE THIS PROPERTY TO SAVE PDF TO A PROVIDED LOCATION
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templateHtml,
                //Page = "https://code-maze.com/", //USE THIS PROPERTY TO GENERATE PDF CONTENT FROM AN HTML PAGE
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };

            return new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
        }

        public static HtmlToPdfDocument CreatePDF(string templateHtml)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                //DocumentTitle = "PDF Report",
                //Out = @"D:\PDFCreator\Employee_Report.pdf"  USE THIS PROPERTY TO SAVE PDF TO A PROVIDED LOCATION
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templateHtml,
                //Page = "https://code-maze.com/", //USE THIS PROPERTY TO GENERATE PDF CONTENT FROM AN HTML PAGE
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };

            return new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
        }

        public static HtmlToPdfDocument CreatePagePDF(string templateHtml)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Domestic Ticket",
                //Out = @"D:\PDFCreator\Employee_Report.pdf"  USE THIS PROPERTY TO SAVE PDF TO A PROVIDED LOCATION
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templateHtml,
                //Page = "https://code-maze.com/", //USE THIS PROPERTY TO GENERATE PDF CONTENT FROM AN HTML PAGE
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "ticketstyles.css") },
                //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Left = "SNS Travel Pvt. Ltd. Kathmandu, Nepal", Right = "Phone No.: 01-4484848" }
            };

            return new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
        }
        public static HtmlToPdfDocument CreatePagePDFDomestic(string templateHtml, string phone, string Adress)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Domestic Ticket",
                //Out = @"D:\PDFCreator\Employee_Report.pdf"  USE THIS PROPERTY TO SAVE PDF TO A PROVIDED LOCATION
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templateHtml,
                //Page = "https://code-maze.com/", //USE THIS PROPERTY TO GENERATE PDF CONTENT FROM AN HTML PAGE
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "ticketstyles.css") },
                //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Left = "Jio Travel Pvt. Ltd. " + Adress, Right = "Phone No:" + phone }
            };

            return new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
        }
        public static HtmlToPdfDocument CreatePagePDFForPackage(string templateHtml, string phone, string Adress)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10, Bottom = 10 },
                DocumentTitle = "Tour Package",
                //Out = @"D:\PDFCreator\Employee_Report.pdf"  USE THIS PROPERTY TO SAVE PDF TO A PROVIDED LOCATION
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templateHtml,
                //Page = "https://code-maze.com/", //USE THIS PROPERTY TO GENERATE PDF CONTENT FROM AN HTML PAGE
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "ticketstyles.css") },
                //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Left = "Jio Travel Pvt. Ltd. " + Adress, Right = "Phone No:" + phone }
            };

            return new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
        }

        public static HtmlToPdfDocument CreatePagePDFInternational(string templateHtml)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "International Ticket",
                //Out = @"D:\PDFCreator\Employee_Report.pdf"  USE THIS PROPERTY TO SAVE PDF TO A PROVIDED LOCATION
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templateHtml,
                //Page = "https://code-maze.com/", //USE THIS PROPERTY TO GENERATE PDF CONTENT FROM AN HTML PAGE
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "ticketstyles.css") },
                //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                //FooterSettings = {FontName = "Arial", FontSize = 9, Line = true, Left = "SNS Travel Pvt. Ltd. Kathmandu, Nepal", Right = "Phone No.: 01-4484848" }
            };

            return new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
        }
    }
}
