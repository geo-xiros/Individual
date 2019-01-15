using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    class Application
    {
        private ProjectDb _db;

        public void Run()
        {
            _db = new ProjectDb();

            DatabaseConnectionChecker databaseConnectionChecker = new DatabaseConnectionChecker(_db);

            if (!databaseConnectionChecker.CheckDbConnection())
            {
                return;
            }
        }
    }

    class DatabaseConnectionChecker
    {

        private ProjectDb _db;
        public string ConnectionString { get; private set; }

        public DatabaseConnectionChecker(ProjectDb db)
        {
            _db = db;
        }
        public bool CheckDbConnection()
        {
            while (!_db.TryToRun(OpenDbConnection))
            {
                Console.Write("Set connection properties and try again [y/n]: "); if (Console.ReadLine() == "n") return false;

                Console.Write("Server:"); string server = Console.ReadLine();
                Console.Write("Database:"); string database = Console.ReadLine();
                Console.Write("User Id:"); string user = Console.ReadLine();
                Console.Write("Password:"); string password = Console.ReadLine();

                _db.SetConnectionString(server, database, user, password);

            }

            return true;
        }
        private void OpenDbConnection(SqlConnection sqlConnection)
        {
            sqlConnection.Open();
        }
    }
    interface IConnectionString
    {
        string GetConnectionString();
        void SetConnectionString(string server, string database, string user, string password);

    }
    class DatabaseConnectionString : IConnectionString

    {
        public string ConnectionString => $"Server={_Server};Database={_Database};User Id={_User};Password={_Password}";
        private string _Server;
        private string _Database;
        private string _User;
        private string _Password;

        public DatabaseConnectionString()
        {
            _Server = Properties.Settings.Default.SqlServer;
            _Database = Properties.Settings.Default.Database;
            _User = Properties.Settings.Default.User;
            _Password = Properties.Settings.Default.Pass;

        }
        public string GetConnectionString() => $"Server={_Server};Database={_Database};User Id={_User};Password={_Password}";

        public void SetConnectionString(string server, string database, string user, string password)
        {
            Properties.Settings.Default.SqlServer = server;
            Properties.Settings.Default.Database = database;
            Properties.Settings.Default.User = user;
            Properties.Settings.Default.Pass = password;
            Properties.Settings.Default.Save();
        }
    }
    class ProjectDb : IConnectionString
    {

        private DatabaseConnectionString _databaseConnectionString;

        public ProjectDb()
        {
            _databaseConnectionString = new DatabaseConnectionString();
        }

        public string ConnectionString()
        {
            return _databaseConnectionString.ConnectionString;
        }

        public void SetConnectionString(string server, string database, string user, string password)
        {
            _databaseConnectionString.SetConnectionString(server, database, user, password);
        }

        public bool TryToRun(Action<SqlConnection> execute)
        {
            bool Success = false;

            using (SqlConnection sqlConnection = new SqlConnection(_databaseConnectionString.ConnectionString))
            {
                try
                {
                    execute(sqlConnection);
                    Success = true;
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return Success;
        }

        public string GetConnectionString()
        {
            throw new NotImplementedException();
        }
    }
}
