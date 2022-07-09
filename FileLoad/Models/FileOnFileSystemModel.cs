using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileLoad.Models
{
    public class FileOnFileSystemModel:FileModel
    {
       
        public string FilePath { get; set; }
        public string Filenamewithpath { get; set; }

        public override string GetFullpath(string FilePath)
        {
            FileName = String.Format("{0}-{1}.txt", DateTime.Now.ToString("yyyyMMddhhmmss"), "Output");
            Filenamewithpath = Path.Combine(FilePath,FileName);

            return Filenamewithpath;
        }
    }
}
