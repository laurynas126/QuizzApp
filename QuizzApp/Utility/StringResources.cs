using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzApp.Utility
{
    public static class StringResources
    {
        public static string AppDir { get; } = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\QuizzApp";
        public static string ResourceFolder { get; } = AppDir + "\\" + Properties.Settings.Default.ResourceFolder;
        public static string DbFile { get; } = AppDir + "\\" + QuizzApp.Properties.Settings.Default.DBFile;
        public static string ConnectionString { get; } = "Data Source=" + DbFile + ";Version=3;";
        public static string DuplicateCategoryError { get; } = "Category with this name already exists";
        public static string DuplicateQuestionError { get; } = "Question with this name already exists";
        public static string Title => Properties.Resources.Title;
        public static string DefaultImage { get; } = AppDir + "\\" + QuizzApp.Properties.Settings.Default.DefaultImage;
    }
}
