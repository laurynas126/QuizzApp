using QuizzApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizzApp.DataManagement;
using QuizzApp.Utility;

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
            LoadNumbers();
            LoadCategories(loader);
            TotalCount = loader.GetTotalCount() + " Questions in total";
        }

        private void LoadNumbers()
        {
            NumberValues = new List<int>
            {
                10, 15, 20, 30
            };
        }

        private void LoadCategories(CategoryTable loader)
        {
            //var cqt = new CategoryQuestionTable();
            //var category = new Category("CAT1\");SELECT * FROM category;--");
            //var catTable = new CategoryTable();
            //catTable.AddCategory(category);
            //var question = new Question("TESTQ1", new string[] { "A", "B", "C", "D" });
            //var qTable = new QuestionTable();
            //qTable.AddQuestion(question);
            //cqt.AddCategoryQuestions(category, new List<Question>() { question, question2 });
            //var question2 = new Question("TESTQ2", new string[] { "AA", "BB", "CC", "DD" });


            CategoryList = loader.LoadAllCategories();
            CategoryList.Insert(0, new Category(0, "All"));
        }
    }
}
