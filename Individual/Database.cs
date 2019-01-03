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
        static private readonly string ConnectionString = Properties.Settings.Default.ConnectionString;

        static Database()
        {
            if (!User.Exists("admin"))
            {
                User user = new User("admin", "Super", "Admin", "1234", "Super");
                user.Insert();
            }
        }

        public static int ExecuteProcedure(string procedure, object parameters)
        {
            try
            {
                using (SqlConnection dbcon = new SqlConnection(ConnectionString))
                {
                    dbcon.Open();
                    int affectedRows = dbcon.Execute(procedure, parameters, commandType: CommandType.StoredProcedure);
                    return affectedRows;
                }

            }
            catch (Exception e)
            {
                Alert.Warning(e.Message);
            }
            return 0;
        }
        static public IEnumerable<T> Query<T>(string procedure, object parameters)
        {
            using (SqlConnection dbcon = new SqlConnection(ConnectionString))
            {
                dbcon.Open();
                return dbcon.Query<T>(procedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }
        static public T QueryFirst<T>(string procedure, object parameters)
        {
            using (SqlConnection dbcon = new SqlConnection(ConnectionString))
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
                passwordForm.Run();
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
