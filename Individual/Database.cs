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

        public static bool Init()
        {
            bool tryAgain = false;
            do
            {
                try
                {
                    CheckDatabaseConnection();

                    if (!User.Exists("admin"))
                    {
                        User user = new User("admin", "Super", "Admin", "admin", "Super");
                        user.Insert();
                    }

                    return true;
                }
                catch (DatabaseException e)
                {
                    Alerts.Error(e.Message);
                }

                ConnectionForm connectionForm = new ConnectionForm();

                connectionForm.Open();

                tryAgain = !connectionForm.EscapePressed;

            } while (tryAgain);

            return false;

        }

        public static void CheckDatabaseConnection()
        {
            try
            {
                using (SqlConnection dbcon = new SqlConnection(ConnectionString()))
                {
                    dbcon.Open();
                }
            }
            catch (SqlException e)
            {
                throw new DatabaseException(e.Message, e);
            }
        }
        public static int ExecuteProcedure(string procedure, object parameters)
        {
            try
            {
                using (SqlConnection dbcon = new SqlConnection(ConnectionString()))
                {
                    dbcon.Open();
                    return dbcon.Execute(procedure, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (SqlException e)
            {
                throw new DatabaseException(e.Message, e);
            }
        }
        static public IEnumerable<T> Query<T>(string procedure, object parameters)
        {
            try
            {
                using (SqlConnection dbcon = new SqlConnection(ConnectionString()))
                {
                    dbcon.Open();
                    return dbcon.Query<T>(procedure, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (SqlException e)
            {
                throw new DatabaseException(e.Message, e);
            }
        }
        static public T QueryFirst<T>(string procedure, object parameters)
        {
            try
            {
                using (SqlConnection dbcon = new SqlConnection(ConnectionString()))
                {
                    dbcon.Open();
                    return dbcon
                      .Query<T>(procedure, parameters, commandType: CommandType.StoredProcedure)
                      .FirstOrDefault();
                }
            }
            catch (SqlException e)
            {
                throw new DatabaseException(e.Message, e);
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
    }
}
