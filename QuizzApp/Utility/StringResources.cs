using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzApp.Utility
{
    public static class StringResources
    {
        //App strings
        public static string AppDir { get; } = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\QuizzApp";
        public static string ResourceFolder { get; } = AppDir + "\\" + Properties.Settings.Default.ResourceFolder;
        public static string DbFile { get; } = AppDir + "\\" + QuizzApp.Properties.Settings.Default.DBFile;

        public static string Title => Properties.Resources.Title;
        public static string DefaultImage { get; } = AppDir + "\\" + QuizzApp.Properties.Settings.Default.DefaultImage;

        //DB Strings
        public static string ConnectionString => "Data Source=" + DbFile + ";Version=3;";
        public static string CreateCategoryTable => "CREATE TABLE \"category\" ( `id` INTEGER PRIMARY KEY AUTOINCREMENT, `title` TEXT NOT NULL UNIQUE )";
        public static string CreateQuestionTable => "CREATE TABLE \"multi_question\" ( `id` INTEGER PRIMARY KEY AUTOINCREMENT, `is_free_text` INTEGER, `question` TEXT NOT NULL, `image` TEXT, `correct_answer` TEXT NOT NULL, `alt_answer1` TEXT, `alt_answer2` TEXT, `alt_answer3` TEXT )";
        public static string CreateCategoryQuestionTable => "CREATE TABLE \"category_question\" ( `categoryID` INTEGER NOT NULL, `questionID` INTEGER NOT NULL, FOREIGN KEY(`categoryID`) REFERENCES `category`(`id`), FOREIGN KEY(`questionID`) REFERENCES `multi_question`(`id`) )";

        //Error strings
        public static string DuplicateCategoryError => "Category with this name already exists";
        public static string DuplicateQuestionError => "Question with this name already exists";
        public static string EmptyQuestionError => "Question must have title and correct answer";
    }
}
