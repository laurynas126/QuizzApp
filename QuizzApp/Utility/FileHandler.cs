using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzApp.Utility
{
    public class FileHandler
    {
        public static string SaveFileToResourceFolder(string originalFile)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(StringResources.ResourceFolder);
            if (!File.Exists(originalFile))
                return null;
            FileInfo file = new FileInfo(originalFile);
            if (file.DirectoryName != StringResources.ResourceFolder)
            {
                string fileName = file.Name;
                int count = 1;
                while (File.Exists(StringResources.ResourceFolder + "\\" + fileName))
                {
                    var length = new FileInfo(StringResources.ResourceFolder + "\\" + fileName).Length;
                    if (file.Length == length)
                        return file.Name;
                    fileName = file.Name + $"({count++})";
                }
                file.CopyTo(StringResources.ResourceFolder + "\\" + fileName);
            }
            return file.Name;
        }
    }
}
