using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizzApp.Model;
using QuizzApp.Utility;
using System.Data.SQLite;

namespace QuizzApp.DataManagement
{
    public class CategoryQuestionTable
    {
        public int AddCategoryQuestions(Category cat, List<Question> questionList)
        {
            var count = 0;
            var connection = new SQLiteConnection(StringResources.ConnectionString);
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                if(cat.Id == -1)
                    CategoryTable.SaveCategory(connection, cat);
                foreach(var question in questionList)
                {
                    if (question.Id == -1) QuestionTable.AddQuestion(connection, question);
                }
                var command = new SQLiteCommand("INSERT INTO category_question VALUES (@category,@question)", connection);
                var categoryCol = new SQLiteParameter("category", cat.Id);
                var questionCol = new SQLiteParameter("question");
                command.Parameters.Add(categoryCol);
                command.Parameters.Add(questionCol);
                foreach(var question in questionList)
                {
                    questionCol.Value = question.Id;
                    count += command.ExecuteNonQuery();
                }
                transaction.Commit();
            }
            connection.Close();
            return count;
        }

        public static void DeleteQuestionsFromCategory(Category category)
        {
            if (category.Id == -1)
                return;
            var connection = new SQLiteConnection(StringResources.ConnectionString);
            connection.Open();
            using (var command = new SQLiteCommand("DELETE FROM category_question WHERE categoryID = @id", connection))
            {
                var categoryCol = new SQLiteParameter("id", category.Id);
                command.Parameters.Add(categoryCol);
                command.ExecuteNonQuery();
            }
            connection.Close();

        }
    }
}
