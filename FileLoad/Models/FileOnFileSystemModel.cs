using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileLoad.Models
{
    public class FileOnFileSystemModel:FileModel
    {
        public string FilePath { get; set; }
        public string Filenamewithpath { get; set; }
    }
}
