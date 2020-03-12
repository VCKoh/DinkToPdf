using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DinkToPdf.Contracts;
using System.IO;
using Microsoft.Extensions.Logging;

namespace DinkToPdf.TestWebServer.Controllers
{
    [Route("api/[controller]")]
    public class ConvertController : Controller
    {
        private IConverter _converter;
        private readonly ILogger<ConvertController> _logger;

        public ConvertController(
            IConverter converter,
            ILogger<ConvertController> logger)
        {
            _converter = converter;
            _logger = logger;
        }

        // GET api/convert
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var doc = new HtmlToPdfDocument()
                {
                    GlobalSettings = {
                    PaperSize = PaperKind.A3,
                    Orientation = Orientation.Landscape,
                },

                    Objects = {
                    new ObjectSettings()
                    {
                        Page = "http://google.com/",
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                    },
                    // new ObjectSettings()
                    //{
                    //    Page = "https://github.com/",

                    //}
                }
                };

                byte[] pdf = _converter.Convert(doc);

                return File(pdf, "application/pdf", "Test.pdf");
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to generate pdf.", e);
                throw;
            }
        }
    }
}
