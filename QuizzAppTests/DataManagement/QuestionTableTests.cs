using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuizzApp.DataManagement;
using QuizzApp.Model;
using QuizzApp.Utility;
using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzApp.DataManagement.Tests
{
    [TestClass()]
    public class QuestionTableTests
    {
        Question defaultQuestion = new Question("QUESTION TEXT ABC", new string[] { "A", "B", "C", "D" });
        [TestMethod()]
        public void QuestionTableTest()
        {
            QuestionTable qt = new QuestionTable();
            Assert.IsNotNull(qt);
        }

        //[TestMethod()]
        //public void GetNextQuestionTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void LoadMultiQuestionsByCategoryTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void GetQuestionsByCategoryTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void SQLQuerryGeneratorTest()
        //{
        //    Assert.Fail();
        //}

        [TestMethod()]
        public void SaveQuestionTest_Default()
        {
            int res = QuestionTable.SaveQuestion(defaultQuestion);
            Assert.AreEqual(res, 1);
        }

        [TestMethod()]
        public void SaveQuestionTest_Modify()
        {
            string temp = defaultQuestion.QuestionText;
            QuestionTable.SaveQuestion(defaultQuestion);
            defaultQuestion.QuestionText = "MODIFIED QUESTION";
            int res = QuestionTable.SaveQuestion(defaultQuestion);
            defaultQuestion.QuestionText = temp;

            Assert.AreEqual(res, 1);
        }

        [TestMethod()]
        public void SaveQuestionTest_Modify2()
        {
            string temp = defaultQuestion.QuestionText;
            QuestionTable.SaveQuestion(defaultQuestion);
            defaultQuestion.QuestionText = "MODIFIED QUESTION";
            QuestionTable.SaveQuestion(defaultQuestion);
            long id = QuestionTable.GetQuestionId(defaultQuestion.QuestionText);
            defaultQuestion.QuestionText = temp;

            Assert.AreNotEqual(id, -1);
        }

        [TestMethod()]
        public void GetQuestionIdTest_Exists()
        {
            QuestionTable.SaveQuestion(defaultQuestion);
            long res = QuestionTable.GetQuestionId(defaultQuestion.QuestionText);

            Assert.AreNotEqual(-1, res);
        }

        [TestMethod()]
        public void GetQuestionIdTest_DoesNotExist()
        {
            QuestionTable.SaveQuestion(defaultQuestion);
            QuestionTable.DeleteQuestion(defaultQuestion);
            long res = QuestionTable.GetQuestionId(defaultQuestion.QuestionText);

            Assert.AreEqual(-1, res);
        }

        [TestMethod()]
        public void DeleteQuestionTest_Default()
        {
            QuestionTable.SaveQuestion(defaultQuestion);
            int res = QuestionTable.DeleteQuestion(defaultQuestion);
            Assert.AreEqual(res, 1);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            QuestionTable.DeleteQuestion(defaultQuestion);
        }

    }
}