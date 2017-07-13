using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzApp.Utility
{
    public static class StringResources
    {
        public static string ConnectionString { get; } = "Data Source=" + Properties.Resources.DBFile + ";Version=3;";
        public static string DuplicateCategoryError { get; } = "Category with this name already exists";
        public static string DuplicateQuestionError { get; } = "Question with this name already exists";
    }
}
