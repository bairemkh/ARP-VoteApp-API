using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebAPI_SQL.Entities;
using WebAPI_SQL.Services;
using WebAPI_SQL.Useful_Stuff;
using Wkhtmltopdf.NetCore;

namespace WebAPI_SQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDFCreatorController : ControllerBase
    {
       
        [HttpGet("GetPdf")]
        public byte[] GetPdf()
        {
            try
            {

                return Useful_Methodes.GeneratePdf();
            }
            catch (WebException e)
            {
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
