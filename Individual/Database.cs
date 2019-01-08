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
        private readonly Application _application;
        public Database(Application application)
        {
            _application = application;
        }
        public bool ConnectWithDb()
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

        public int ExecuteProcedure(string procedure, object parameters)
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
        public  IEnumerable<User> GetUsers()
        {
            return Query<User>("GetUsers", new { userId = 0, userName = "" });
        }
        public  User GetUserBy(int userId)
        {
            return QueryFirst<User>("GetUsers", new { userId, userName = "" });
        }
        public  User GetUserBy(string userName)
        {
            return QueryFirst<User>("GetUsers", new { userId = 0, userName });
        }
        public  bool Exists(string userName)
        {
            return GetUserBy(userName) != null;
        }
        public  bool ValidateUserPassword(string username, string password)
        {
            return QueryFirst<int>("Validate_User", new { userName = username, userPassword = GetPasswordCrypted(password) }) == 1;
        }
        #endregion
        #region Messages Functions
        public  Message GetMessageById(int messageId)
        {
            return QueryFirst<Message>("GetMessages", new { messageId, userId = 0 });
        }
        public IEnumerable<Message> GetUserMessages(int userId)
        {
            return Query<Message>("GetMessages", new { messageId = 0, userId });
        }
        #endregion
        private IEnumerable<T> Query<T>(string procedure, object parameters)
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
        private T QueryFirst<T>(string procedure, object parameters)
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

        private byte[] GetPasswordCrypted(string password)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(password);
            SHA512 shaM = new SHA512Managed();
            return shaM.ComputeHash(data);
        }
        #region User  Insert Update Delete
        public bool Insert(User user)
        {
            user.UserId = QueryFirst<int>("InsertUser", new
            {
                userName = user.UserName,
                firstName = user.FirstName,
                lastName = user.LastName,
                UserPassword = GetPasswordCrypted(user.Password),
                UserRole = user.Role.ToString()
            });
            return user.UserId != 0;
        }

        public bool Update(User user)
        {
            return ExecuteProcedure("UpdateUser", new
            {
                userId = user.UserId,
                userName = user.UserName,
                firstName = user.FirstName,
                lastName = user.LastName,
                userPassword = GetPasswordCrypted(user.Password),
                userRole = user.Role.ToString()
            }) == 1;
        }
        public bool Delete(User user)
        {
            if (!GetPasswordIfNeeded(out string deletePassword, user.UserId, "Delete Selected User"))
                return false;

            return ExecuteProcedure("DeleteUser", new
            {
                superUserId = _application.LoggedUser.UserId,
                superUserPassword = GetPasswordCrypted(deletePassword),
                userId = user.UserId
            }) == 1;
        }
        #endregion

        #region Message Insert Update Delete
        public bool Insert(Message message)
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

        public bool Update(Message message)
        {
            if (!GetPasswordIfNeeded(out string updatePassword, message.SenderUserId, "Update Selected Message"))
                return false;

            return ExecuteProcedure("UpdateMessage", new
            {
                updateUserId = _application.LoggedUser.UserId,
                updateUserPassword = GetPasswordCrypted(updatePassword),
                messageId = message.MessageId,
                subject = message.Subject,
                body = message.Body
            }) == 1;
        }

        public bool Delete(Message message)
        {
            if (!GetPasswordIfNeeded(out string deletePassword, message.SenderUserId, "Delete Selected Message"))
                return false;

            return ExecuteProcedure("DeleteMessage", new
            {
                deleteUserId = _application.LoggedUser.UserId,
                deleteUserPassword = GetPasswordCrypted(deletePassword),
                messageId = message.MessageId
            }) == 1;

        }
        #endregion

        public bool GetPasswordIfNeeded(out string returnPassword, int userId, string passwordForAction )
        {
            string password = "";

            if (_application.LoggedUser.UserId != userId)
            {
                PasswordForm passwordForm = new PasswordForm(passwordForAction);
                passwordForm.OnFormFilled = () => password = passwordForm["Password"];
                passwordForm.Open();
            }

            returnPassword = password;

            return _application.LoggedUser.UserId == userId
                || password.Length != 0;
        }
    }
}
