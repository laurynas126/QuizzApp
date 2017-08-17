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
                command.CommandText = SQLQuerryGenerator(category, true, questionCount);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var question = new Question();
                    question.Id = (long)reader["id"];
                    question.QuestionText = reader["question"].ToString();
                    question.ImageName = reader["image"].ToString();
                    if (question.QuestionText.Equals(string.Empty))
                        continue;
                    var correctAnswer = reader["correct_answer"].ToString();
                    if (correctAnswer.Equals(string.Empty))
                        continue;
                    question.Answers.Add(new Answer(correctAnswer, true));
                    question.Answers.Add(new Answer(reader["alt_answer1"].ToString(), false));
                    question.Answers.Add(new Answer(reader["alt_answer2"].ToString(), false));
                    question.Answers.Add(new Answer(reader["alt_answer3"].ToString(), false));
                    question.Answers.Shuffle();
                    questionList.Add(question);
                }
                connection.Close();
            }
        }

        public static List<Question> GetQuestionsByCategory(long category = 0, bool inverted = false)
        {
            List<Question> questions = new List<Question>();
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
                    question.ImageName = reader["image"].ToString();
                    question.QuestionText = reader["question"].ToString();
                    question.Answers.Add(new Answer(reader["correct_answer"].ToString(), true));
                    question.Answers.Add(new Answer(reader["alt_answer1"].ToString(), false));
                    question.Answers.Add(new Answer(reader["alt_answer2"].ToString(), false));
                    question.Answers.Add(new Answer(reader["alt_answer3"].ToString(), false));
                    questions.Add(question);
                }
                connection.Close();
            }
            return questions;
        }

        public static string SQLQuerryGenerator(long category, bool random = false, int count = 10)
        {
            string result = "SELECT * FROM multi_question";
            if (category != 0)
            {
                result += " JOIN category_question ON category_question.questionID = multi_question.id " +
                   $"WHERE category_question.categoryID = {category}";
            }
            if (random)
            {
                result += $" ORDER BY RANDOM() LIMIT {count}";
            }
            return result;
        }

        public static int SaveQuestion(Question question)
        {
            int result;
            using (var connection = new SQLiteConnection(StringResources.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    result = SaveQuestion(connection, question);
                    transaction.Commit();
                }
                connection.Close();
            }
            return result;
        }

        public static int SaveQuestion(SQLiteConnection connection, Question question)
        {
            int result = -1;
            var command = new SQLiteCommand(connection);
            string image = "NULL";
            if (question.ImageName != string.Empty)
                image = question.ImageName;
            SQLiteParameter questionCol = new SQLiteParameter("question", question.QuestionText);
            SQLiteParameter imageParam = new SQLiteParameter("image", image);
            SQLiteParameter answer0 = new SQLiteParameter("correct", question.Answers[0]);
            SQLiteParameter answer1 = new SQLiteParameter("alt1", question.Answers[1]);
            SQLiteParameter answer2 = new SQLiteParameter("alt2", question.Answers[2]);
            SQLiteParameter answer3 = new SQLiteParameter("alt3", question.Answers[3]);

            if (question.Id == -1)
                command.CommandText = "INSERT INTO multi_question VALUES(NULL, @question, @image, @correct, @alt1, @alt2, @alt3)";
            else
                command.CommandText = "UPDATE multi_question SET question = @question, image = @image, correct_answer = @correct, alt_answer1 = @alt1, alt_answer2 = @alt2, alt_answer3 = @alt3" +
                    $" WHERE id = {question.Id}";
            command.Parameters.Add(questionCol);
            command.Parameters.Add(imageParam);
            command.Parameters.Add(answer0);
            command.Parameters.Add(answer1);
            command.Parameters.Add(answer2);
            command.Parameters.Add(answer3);
            result = command.ExecuteNonQuery();
            if (question.Id == -1)
                question.Id = GetQuestionId(connection, question.QuestionText);
            return result;
        }

        public static long GetQuestionId(string question)
        {
            long result;
            using (var connection = new SQLiteConnection(StringResources.ConnectionString))
            {
                connection.Open();
                result = GetQuestionId(connection, question);
                connection.Close();
            }
            return result;
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

        public static int DeleteQuestion(Question question)
        {
            int result = -1;
            var connection = new SQLiteConnection(StringResources.ConnectionString);
            connection.Open();
            using (var command = new SQLiteCommand("DELETE FROM multi_question WHERE id = @id", connection))
            {
                command.Parameters.Add(new SQLiteParameter("id", question.Id));
                result = command.ExecuteNonQuery();
            }
            connection.Close();
            return result;
        }

    }
}
