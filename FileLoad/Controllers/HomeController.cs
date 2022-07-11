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
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace FileLoad.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        FileOnFileSystemModel fileOnFileSystemModel = new FileOnFileSystemModel();
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            fileOnFileSystemModel.Filenamewithpath = configuration["FileConfiguration:Filepath"];
            fileOnFileSystemModel.FileType = "application/txt";
        }

        public IActionResult Index()
        {
            return View();
        }
        private async Task<FileOnFileSystemModel> Writedatatofile(Login login)
        {
            try
            {
                _logger.LogInformation("Started Writing to file");
                //  fileOnFileSystemModel.FileName = String.Format("{0}-{1}.txt", DateTime.Now.ToString("yyyyMMddhhmmss"), "Output");
               // fileOnFileSystemModel.Filenamewithpath = fileOnFileSystemModel.GetFullpath(fileOnFileSystemModel.FilePath);
                //Path.Combine(fileOnFileSystemModel.FilePath, fileOnFileSystemModel.FileName);
                //ViewBag.path = fileOnFileSystemModel.Filenamewithpath;
                TempData["file"] = fileOnFileSystemModel.Filenamewithpath;
      
                if (System.IO.File.Exists(fileOnFileSystemModel.Filenamewithpath) && System.IO.File.ReadAllText(fileOnFileSystemModel.Filenamewithpath) != "")
                {
                    var jsonData = System.IO.File.ReadAllText(fileOnFileSystemModel.Filenamewithpath);
                    await Task.Delay(3000);
                    var loginList = JsonConvert.DeserializeObject<List<Login>>(jsonData) ?? new List<Login>();
                    loginList.Add(new Login()
                    {
                        FirstName = login.FirstName,
                        LastName = login.LastName,
                        Email = login.Email
                    });
                    var newjsonData = JsonConvert.SerializeObject(loginList);
                     System.IO.File.WriteAllText(fileOnFileSystemModel.Filenamewithpath, newjsonData);
                }

                else
                {
                    using (StreamWriter sw = System.IO.File.CreateText(fileOnFileSystemModel.Filenamewithpath))
                    {
                        List<Login> data = new List<Login>();
                        
                        data.Add(new Login()
                        {
                            FirstName = login.FirstName,
                            LastName = login.LastName,
                            Email = login.Email
                        }); 
                        string newjsonData = JsonConvert.SerializeObject(data);
                        sw.Close();
                        System.IO.File.WriteAllText(fileOnFileSystemModel.Filenamewithpath, newjsonData);
                    }
                }
                
                _logger.LogInformation("Completed Writing to file");
                return fileOnFileSystemModel;
            }
         
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }
           
        }
        [HttpPost]
        public async Task<IActionResult> Index(Login login)
        {
            try
            {
             

                var fileName = await Writedatatofile(login);

                _logger.LogInformation("Json data has been written to file successfully");
                return View("View", fileName);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }
        }
        public async Task<FileResult> GetTxt(string fileName)
        {
            try
            {
               var  fs = System.IO.File.OpenRead(fileName);
                _logger.LogInformation("File has been read successfully");
                await Task.Delay(3000);
                return File(fs, fileOnFileSystemModel.FileType, TempData["file"].ToString());
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }
           
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
