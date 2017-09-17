using QuizzApp.Utility;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizzApp.Model;

namespace QuizzApp.DataManagement
{
    public class CategoryTable
    {

        public CategoryTable()
        {
        }

        public static List<Category> GetAllCategories()
        {
            List<Category> resultList = new List<Category>();
            using (var connection = new SQLiteConnection(StringResources.ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(connection);
                command.CommandText = "SELECT * FROM category ORDER BY title;";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var category = new Category()
                    {
                        Id = reader.GetInt64(0),
                        Title = reader.GetString(1)
                };
                    resultList.Add(category);
                }
                connection.Close();
            }
            return resultList;
        }

        public int GetTotalCount()
        {
            int result;
            using (var connection = new SQLiteConnection(StringResources.ConnectionString))
            {
                connection.Open();
                var command = new SQLiteCommand(connection);
                command.CommandText = "SELECT COUNT(id) as count FROM multi_question";
                var reader = command.ExecuteReader();
                reader.Read();
                result = reader.GetInt32(0);
            }
            return result;
        }

        public static void SaveCategory(Category category)
        {
            using (var connection = new SQLiteConnection(StringResources.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    SaveCategory(connection, category);
                    transaction.Commit();
                }
                connection.Close();
            }
        }

        public static void SaveCategory(SQLiteConnection connection, Category category)
        {
            if (category == null)
                return;
            var command = new SQLiteCommand(connection);
            SQLiteParameter param = new SQLiteParameter("title", category.Title);
            if (category.Id == -1)
                command.CommandText = "INSERT INTO category VALUES(NULL, @title)";
            else
                command.CommandText = $"UPDATE category SET title = @title WHERE id = {category.Id}";
            command.Parameters.Add(param);
            command.ExecuteNonQuery();
            if (category.Id == -1) category.Id = GetCategoryId(connection, category.Title);
        }


        public static long GetCategoryId(SQLiteConnection connection, string categoryTitle)
        {
            long result = -1;
            using (var command = new SQLiteCommand("SELECT id FROM category WHERE title = @title", connection))
            {
                command.Parameters.Add(new SQLiteParameter("title", categoryTitle));
                var resultQuerry = command.ExecuteScalar();
                if (resultQuerry != null)
                {
                    result = (long)resultQuerry;
                }
            }
            return result;
        }

        public static void DeleteCategory(Category category)
        {
            var connection = new SQLiteConnection(StringResources.ConnectionString);
            connection.Open();
            using (var command = new SQLiteCommand("DELETE FROM category WHERE id = @id", connection))
            {
                command.Parameters.Add(new SQLiteParameter("id", category.Id));
                command.ExecuteNonQuery();           
            }
            connection.Close();
        }

    }
}
