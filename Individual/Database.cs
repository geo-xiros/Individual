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

        public static bool ConnectToDb()
        {
            if (!ConnectWithDb())
            {
                return false;
            }

            if (!Exists("admin"))
            {
                User user = new User("admin", "Super", "Admin", "admin", "Super");
                if (!Application.TryToRunAction<User>(user, Database.Insert,
                    string.Empty,
                    string.Empty,
                    "Unable to insert Admin user Account try again [Y/N] "))
                {
                    return false;
                }
            }

            return true;

        }

        private static bool ConnectWithDb()
        {
            bool tryAgain = false;
            do
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
                    Alerts.Error(e.Message);
                    ConnectionForm connectionForm = new ConnectionForm();

                    connectionForm.Open();

                    tryAgain = !connectionForm.EscapePressed;
                }

            } while (tryAgain);

            return false;

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
            return Query<User>("GetUsers", new { userId = 0, userName = "" });
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
        private static T QueryFirst<T>(string procedure, object parameters)
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

        #region User  Insert Update Delete
        public static bool Insert(User user)
        {
            user.UserId = QueryFirst<int>("InsertUser", new
            {
                userName = user.UserName,
                firstName = user.FirstName,
                lastName = user.LastName,
                UserPassword = user.Password,
                UserRole = user.Role.ToString()
            });
            return user.UserId != 0;
        }

        public static bool Update(User user)
        {
            return ExecuteProcedure("UpdateUser", new
            {
                userId = user.UserId,
                userName = user.UserName,
                firstName = user.FirstName,
                lastName = user.LastName,
                userPassword = user.Password,
                userRole = user.Role.ToString()
            }) == 1;
        }
        public static bool Delete(User user)
        {
            if (!GetPasswordIfNeeded(out string deletePassword, user.UserId, "Delete Selected User"))
                return false;

            return ExecuteProcedure("DeleteUser", new
            {
                superUserId = Application.LoggedUser.UserId,
                superUserPassword = deletePassword,
                userId = user.UserId
            }) == 1;
        }
        #endregion

        #region Message Insert Update Delete
        public static bool Insert(Message message)
        {
            message.MessageId = QueryFirst<int>("InsertMessage", new
            {
                senderUserId = message.SenderUserId,
                receiverUserId = message.ReceiverUserId,
                subject = message.Subject,
                body = message.Body
            });
            return true;
        }

        public static bool Update(Message message)
        {
            if (!GetPasswordIfNeeded(out string updatePassword, message.SenderUserId, "Update Selected Message"))
                return false;

            return ExecuteProcedure("UpdateMessage", new
            {
                updateUserId = Application.LoggedUser.UserId,
                updateUserPassword = updatePassword,
                messageId = message.MessageId,
                subject = message.Subject,
                body = message.Body
            }) == 1;
        }

        public static bool Delete(Message message)
        {
            if (!GetPasswordIfNeeded(out string deletePassword, message.SenderUserId, "Delete Selected Message"))
                return false;

            return ExecuteProcedure("DeleteMessage", new
            {
                deleteUserId = Application.LoggedUser.UserId,
                deleteUserPassword = deletePassword,
                messageId = message.MessageId
            }) == 1;

        }
        #endregion

        public static bool GetPasswordIfNeeded(out string returnPassword, int userId, string passwordForAction )
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
    }
}
