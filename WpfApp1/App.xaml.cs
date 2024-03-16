using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Services;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string ConnectionString = "Server=DESKTOP-VBT9PRT;Database=myDataBase;Trusted_Connection=True;";

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            CreateDatabaseIfNotExists();
        }

        private void CreateDatabaseIfNotExists()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string databaseQuery = "IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'myDataBase') CREATE DATABASE myDataBase";
                using (SqlCommand command = new SqlCommand(databaseQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            DatabaseService databaseService = new DatabaseService(ConnectionString);
        }
    }
}
