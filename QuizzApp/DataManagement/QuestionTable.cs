using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizzApp.Utility;
using QuizzApp.Model;

namespace QuizzApp.DataManagement
{
    public class QuestionTable
    {
        private List<Question> questionList;
        private int counter = 0;
        private int questionCount = 0;

        public QuestionTable(int TotalQuestions=10)
        {
            questionCount = TotalQuestions;
            questionList = new List<Question>();
        }

        public Question GetNextQuestion()
        {
            if (counter < questionCount && counter < questionList.Count)
            {
                return questionList[counter++];
            }
            return null;
        }

        public void LoadMultiQuestionsByCategory(long category=0)
        {
            using (var connection = new SQLiteConnection(StringResources.ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(connection);
                command.CommandText = SQLQuerryGenerator(category);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var question = new Question();
                    question.Id = (long)reader["id"];
                    question.QuestionText = reader["question"].ToString();
                    if (question.QuestionText.Equals(string.Empty))
                        continue;
                    question.Answers.Add(new Answer(reader["correct_answer"].ToString(), true));
                    AddQuestions(question, false, 
                        reader["alt_answer1"].ToString(),
                        reader["alt_answer2"].ToString(),
                        reader["alt_answer3"].ToString());
                    question.Answers.Shuffle();
                    questionList.Add(question);
                }
                connection.Close();
            }
        }

        public static List<Question> GetQuestionsByCategory(long category = 0, bool inverted = false)
        {
            List<Question> questions = new List<Question>();
            string equalSign = "=";
            if(inverted)
            {
                equalSign = "!=";
            }
            using (var connection = new SQLiteConnection(StringResources.ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(connection);
                command.CommandText = "SELECT * FROM multi_question JOIN category_question ON category_question.questionID = multi_question.id " +
                   $"WHERE category_question.categoryID {equalSign} {category}";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var question = new Question();
                    question.Id = (long)reader["id"];
                    question.QuestionText = reader["question"].ToString();
                    if (question.QuestionText.Equals(string.Empty))
                        continue;
                    question.Answers.Add(new Answer(reader["correct_answer"].ToString(), true));
                    AddQuestions(question, false,
                        reader["alt_answer1"].ToString(),
                        reader["alt_answer2"].ToString(),
                        reader["alt_answer3"].ToString());
                    questions.Add(question);
                }
                connection.Close();
            }
            return questions;
        }

        public static void AddQuestions(Question question, bool correct, params string[] values)
        {
            foreach (var el in values.Where(x => x != string.Empty))
            {
                question.Answers.Add(new Answer(el, correct));
            }
                
        }

        public string SQLQuerryGenerator(long category)
        {
            string result = "SELECT * FROM multi_question";
            if (category != 0)
            {
                result += " JOIN category_question ON category_question.questionID = multi_question.id " +
                   $"WHERE category_question.categoryID = {category}";
            }
            result += $" ORDER BY RANDOM() LIMIT {questionCount}";
            return result;
        }

        public void AddQuestion(Question question)
        {
            using (var connection = new SQLiteConnection(StringResources.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    AddQuestion(connection, question);
                    transaction.Commit();
                }
                connection.Close();
            }
        }

        public static void AddQuestion(SQLiteConnection connection, Question question)
        {
            var command = new SQLiteCommand(connection);
            SQLiteParameter questionCol = new SQLiteParameter("question", question.QuestionText);
            SQLiteParameter answer0 = new SQLiteParameter("correct", question.Answers[0]);
            SQLiteParameter answer1 = new SQLiteParameter("alt1", question.Answers[1]);
            SQLiteParameter answer2 = new SQLiteParameter("alt2", question.Answers[2]);
            SQLiteParameter answer3 = new SQLiteParameter("alt3", question.Answers[3]);
            command.CommandText = "INSERT INTO multi_question VALUES(NULL, @question, @correct, @alt1, @alt2, @alt3)";
            command.Parameters.Add(questionCol);
            command.Parameters.Add(answer0);
            command.Parameters.Add(answer1);
            command.Parameters.Add(answer2);
            command.Parameters.Add(answer3);
            command.ExecuteNonQuery();

            question.Id = GetQuestionId(connection, question.QuestionText);
        }

        public static long GetQuestionId(SQLiteConnection connection, string question)
        {
            long result = -1;
            using (var command = new SQLiteCommand("SELECT id FROM multi_question WHERE question = @question", connection))
            {
                command.Parameters.Add(new SQLiteParameter("question", question));
                var resultQuerry = command.ExecuteScalar();
                if (resultQuerry != null)
                {
                    result = (long)resultQuerry;
                }
            }
            return result;
        }

    }
}
