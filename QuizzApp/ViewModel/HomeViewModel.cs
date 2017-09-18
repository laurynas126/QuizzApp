using QuizzApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizzApp.DataManagement;
using QuizzApp.Utility;
using System.Windows;
using System.IO;

namespace QuizzApp.ViewModel
{
    public class HomeViewModel
    {
        public List<int> NumberValues { get; set; }
        public List<Category> CategoryList { get; set; }
        public string TotalCount { get; set; }
        public string AppTitle => Properties.Resources.Title;

        public HomeViewModel()
        {
            var loader = new CategoryTable();
            InitDirectory();
            LoadNumbers();
            LoadCategories(loader);
            TotalCount = loader.GetTotalCount() + " Questions in total";
        }

        private void InitDirectory()
        {
            if (!Directory.Exists(StringResources.ResourceFolder))
                Directory.CreateDirectory(StringResources.ResourceFolder);
            if (!File.Exists(StringResources.DbFile))
            {
                InitDatabase();
            }
        }

        private void InitDatabase()
        {
            DatabaseCreator.CreateQuizzDatabase(StringResources.DbFile);
            var demo = new Category("Demo");
            var question = new Question("Demo question", new string[] { "Correct answer", "Alternative 1", "Alternative 2", "Alternative 3" });
            var textQuestion = new Question("This is text question", new string[] { "Accepted answer;separate;correct;answers;by;semicolon" });
            textQuestion.IsFreeText = true;
            QuestionTable.SaveQuestion(textQuestion);
            CategoryQuestionTable.AddCategoryQuestion(demo, question);
        }

        private void LoadNumbers()
        {
            NumberValues = new List<int>
            {
                10, 20, 30, 50
            };
        }

        private void LoadCategories(CategoryTable loader)
        {
            CategoryList = CategoryTable.GetAllCategories();
            CategoryList.Insert(0, new Category(0, "All"));
        }
    }
}
