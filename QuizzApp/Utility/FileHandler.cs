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
            if (File.Exists(StringResources.ResourceFolder + "\\" + originalFile))
                return originalFile;
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

                    fileName = file.Name.Replace(file.Extension, "") + $"({count++})" + file.Extension;
                }
                file.CopyTo(StringResources.ResourceFolder + "\\" + fileName);
                return fileName;
            }
            return file.Name;
        }
    }
}
