using FileLoad.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.IO;

namespace FileLoad.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        FileOnFileSystemModel fileOnFileSystemModel = new FileOnFileSystemModel();
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            fileOnFileSystemModel.FilePath = configuration["FileConfiguration:Filepath"];
            fileOnFileSystemModel.FileType = "application/txt";
        }

        public IActionResult Index()
        {
            return View();
        }
        private async Task<FileOnFileSystemModel> Writedatatofile(Login login)
        {
            fileOnFileSystemModel.FileName = String.Format("{0}-{1}.txt", DateTime.Now.ToString("yyyyMMddhhmmss"), "Output");
            fileOnFileSystemModel.Filenamewithpath = Path.Combine(fileOnFileSystemModel.FilePath, fileOnFileSystemModel.FileName);
            ViewBag.path = fileOnFileSystemModel.Filenamewithpath;
            TempData["file"] = fileOnFileSystemModel.FileName;
            byte[] utf8bytesJson = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(login);
            string strResult = System.Text.Encoding.UTF8.GetString(utf8bytesJson);
            using (StreamWriter sw = (System.IO.File.Exists(fileOnFileSystemModel.Filenamewithpath)) ? System.IO.File.AppendText(fileOnFileSystemModel.Filenamewithpath) : System.IO.File.CreateText(fileOnFileSystemModel.Filenamewithpath))

            {
                await sw.WriteAsync(strResult);
            }
            return fileOnFileSystemModel;
        }
        [HttpPost]
        public async Task<IActionResult> Index(Login login)
        {
            var fileName = await Writedatatofile(login);

            return View("View", fileName);
        }
        public async Task<FileResult> GetTxt(string fileName)
        {//open the file and read content
            var fs = System.IO.File.OpenRead(fileName);
            await Task.Delay(3000);
            return File(fs, fileOnFileSystemModel.FileType, TempData["file"].ToString());
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
