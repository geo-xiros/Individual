using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Security.Cryptography;

namespace Individual
{
    class Database
    {
        static string ConnectionString() => $"Server={Properties.Settings.Default.SqlServer};Database={Properties.Settings.Default.Database};User Id={Properties.Settings.Default.User};Password={Properties.Settings.Default.Pass}";
        public static bool DatabaseError { get; private set; }

        public static bool OpenConnection(Action<SqlConnection> execute)
        {
            DatabaseError = false;

            using (SqlConnection dbcon = new SqlConnection(ConnectionString()))
            {
                try
                {
                    execute(dbcon);
                }
                catch (SqlException e)
                {
                    DatabaseError = true;
                    Alerts.Error(e.Message);
                }
                
            }

            return DatabaseError;
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
        #region UserFunctions
        public static IEnumerable<User> GetUsers()
        {
            using (SqlConnection dbcon = new SqlConnection(ConnectionString()))
            {
                dbcon.Open();
                return dbcon.Query<User>("Select * From Get_Users");
            }
            //return Query<User>("Get_Users").Where(filterPredicate);//, new { userId = 0, userName = "" });
        }

        public static User GetUserBy(int userId)
        {
            return QueryFirst<User>("GetUsers", new { userId, userName = "" });
        }

        public static User GetUserBy(string userName)
        {
            return QueryFirst<User>("GetUsers", new { userId = 0, userName });
        }
        public static bool Exists(string userName)
        {
            return GetUserBy(userName) != null;
        }
        public static bool ValidateUserPassword(string username, string password)
        {
            return QueryFirst<int>("Validate_User", new { userName = username, userPassword = password }) == 1;
        }
        #endregion
        #region Messages Functions
        public static Message GetMessageById(int messageId)
        {
            return QueryFirst<Message>("GetMessages", new { messageId, userId = 0 });
        }
        public static IEnumerable<Message> GetUserMessages(int userId)
        {
            return Query<Message>("GetMessages", new { messageId = 0, userId });
        }
        #endregion
        private static IEnumerable<T> Query<T>(string procedure, object parameters)
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
        public static T QueryFirst<T>(string procedure, object parameters)
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



        public static bool GetPasswordIfNeeded(out string returnPassword, int userId, int loggedUserId, string passwordForAction)
        {
            string password = "";

            if (loggedUserId != userId)
            {
                PasswordForm passwordForm = new PasswordForm(passwordForAction);
                passwordForm.OnFormFilled = () => password = passwordForm["Password"];
                passwordForm.Open();
            }

            returnPassword = password;

            return loggedUserId == userId
                || password.Length != 0;
        }
    }
}
