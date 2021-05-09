using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_SQL.Services;

namespace WebAPI_SQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDFCreatorController : ControllerBase
    {
        private IConverter _converter;
        private IWebHostEnvironment _webHostEnvironment;

        
        public PDFCreatorController(IConverter converter, IWebHostEnvironment webHostEnvironment)
        {
            _converter = converter;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet("CreatePdf/VoteResult.pdf")]
        public IActionResult CreatePdf()
        {
                
            var Settings = new GlobalSettings
            {
                DocumentTitle = "Last Voting Session Result",
                ColorMode = ColorMode.Color,
                PaperSize = PaperKind.A4,
                Margins=new MarginSettings { Top = 10 },
                //Out= @"C:\ResultVote.pdf"

            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = PDFTemplateGenerator.GetHtmlString(),
                WebSettings = {DefaultEncoding="utf-8"},
                HeaderSettings = {FontName="Arial",FontSize=9}
            };
            var pdf = new HtmlToPdfDocument
            {
                GlobalSettings = Settings,
                Objects = {objectSettings}
            };
            var Pdf=_converter.Convert(pdf);
            var PdfStream = new MemoryStream();
            PdfStream.Write(Pdf, 0, Pdf.Length);
            PdfStream.Position = 0;
            
           var result = new FileStreamResult(PdfStream, "application/pdf");
           
            return result;
        }
    }
}
