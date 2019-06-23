using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EduTest.Controllers
{
    public class UploadController : Controller
    {
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        public UploadController(ILogger<UploadController> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PostFiles(List<IFormFile> files)
        {
            //用户上传文件可能存在同名，使用guid和附件表做映射，不影响用户下载

            if (files.Count<=0)
            {
                files = HttpContext.Request.Form.Files.ToList();
                if (files.Count<=0)
                {
                    return  Json(new { count = files.Count, size=0, filePath = "" });
                }
            }
            long size = files.Sum(f => f.Length);

            string urlPath = "/File/Dicussion/";
            string filePath = string.Empty;

            string webRootPath = _hostingEnvironment.WebRootPath.Replace('\\', '/');
            string contentRootPath = _hostingEnvironment.ContentRootPath.Replace('\\','/');
            String path = webRootPath + urlPath;
            if (!System.IO.Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string ex = Path.GetExtension(files[0].FileName);
            var filePathName = Guid.NewGuid().ToString("N") + ex;
            filePath = Path.Combine(path, filePathName);
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Json(new { count = files.Count, size, filePath= urlPath+ filePathName});
        }
    }
}