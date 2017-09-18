using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using QuizzApp.Utility;

namespace QuizzApp.DataManagement
{
    public class DatabaseCreator
    {
        public static void CreateDatabase(string filePath)
        {
            SQLiteConnection.CreateFile(filePath);
        }

        public static int CreateTable(string filePath, string sqlCommandString)
        {
            var connection = new SQLiteConnection($"Data Source = {filePath}; Version = 3;");
            connection.Open();
            var result = CreateTable(connection, sqlCommandString);
            connection.Close();
            return result;
        }

        public static int CreateTable(SQLiteConnection connection, string sqlCommandString)
        {
            return new SQLiteCommand(sqlCommandString, connection).ExecuteNonQuery();
        }

        public static void CreateQuizzDatabase(string filePath)
        {
            CreateDatabase(filePath);
            var connection = new SQLiteConnection($"Data Source = {filePath}; Version = 3;");
            connection.Open();
            CreateTable(connection, StringResources.CreateCategoryTable);
            CreateTable(connection, StringResources.CreateQuestionTable);
            CreateTable(connection, StringResources.CreateCategoryQuestionTable);
            connection.Close();

        }
    }
}
