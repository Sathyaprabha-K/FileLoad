using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileLoad.Models
{
    public abstract class FileModel
    {
        public string FileName { get; set; }
        public string FileType { get; set; }

        //public abstract string  GetFullpath(string FileName);

    }
}
