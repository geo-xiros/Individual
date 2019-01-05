using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Security.Cryptography;

namespace Individual
{
    static class Database
    {
        static string ConnectionString() => $"Server={Properties.Settings.Default.SqlServer};Database={Properties.Settings.Default.Database};User Id={Properties.Settings.Default.User};Password={Properties.Settings.Default.Pass}";
        static string LastErrorMessage;//4060 database error //18456 user error

        public static bool Init()
        {

            while (!CheckDatabaseConnection())
            {
                Alerts.Warning(LastErrorMessage);

                ConnectionForm connectionForm = new ConnectionForm();

                connectionForm.Open();

                if (connectionForm.EscapePressed)
                {
                    return false;
                }
            }

            if (!User.Exists("admin"))
            {
                User user = new User("admin", "Super", "Admin", "admin", "Super");
                user.Insert();
            }

            return true;
        }
        public static bool CheckDatabaseConnection()
        {
            try
            {
                using (SqlConnection dbcon = new SqlConnection(ConnectionString()))
                {
                    dbcon.Open();
                }
                return true;
            }
            catch (SqlException e)
            {
                LastErrorMessage = e.Message.Replace("\r\n","");

                if (LastErrorMessage.Length > 80)
                {
                    LastErrorMessage = LastErrorMessage.Substring(1, 80) + "...";
                }

            }
            return false;
        }
        public static int ExecuteProcedure(string procedure, object parameters)
        {
            using (SqlConnection dbcon = new SqlConnection(ConnectionString()))
            {
                dbcon.Open();
                return dbcon.Execute(procedure, parameters, commandType: CommandType.StoredProcedure);
            }

        }
        static public IEnumerable<T> Query<T>(string procedure, object parameters)
        {
            using (SqlConnection dbcon = new SqlConnection(ConnectionString()))
            {
                dbcon.Open();
                return dbcon.Query<T>(procedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }
        static public T QueryFirst<T>(string procedure, object parameters)
        {
            using (SqlConnection dbcon = new SqlConnection(ConnectionString()))
            {
                dbcon.Open();
                return dbcon
                  .Query<T>(procedure, parameters, commandType: CommandType.StoredProcedure)
                  .FirstOrDefault();
            }
        }
        public static bool GetPasswordIfNeeded(out string returnPassword, int userId, string passwordForAction)
        {
            string password = "";

            if (Application.LoggedUser.UserId != userId)
            {
                PasswordForm passwordForm = new PasswordForm(passwordForAction);
                passwordForm.OnFormFilled = () => password = passwordForm["Password"];
                passwordForm.Open();
            }

            returnPassword = password;

            return Application.LoggedUser.UserId == userId
                || password.Length != 0;
        }
        public static byte[] GetPasswordCrypted(string password)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(password);
            SHA512 shaM = new SHA512Managed();
            return shaM.ComputeHash(data);
        }

        static public class FieldSize
        {
            public const int SqlServer = 80;
            public const int Database = 80;
            public const int UserId = 30;
            public const int Password = 30;
        }
        static public class FieldName
        {
            public const string SqlServer = "Sql Server";
            public const string Database = "Database Name";
            public const string UserId = "User";
            public const string Password = "Password";
        }
    }
}
