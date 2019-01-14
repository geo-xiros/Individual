using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Security.Cryptography;
using Individual.Models;

namespace Individual
{
    class Database
    {
        static string ConnectionString() => $"Server={Properties.Settings.Default.SqlServer};Database={Properties.Settings.Default.Database};User Id={Properties.Settings.Default.User};Password={Properties.Settings.Default.Pass}";
        public static bool DatabaseError { get; private set; }

        #region UserFunctions
        public static IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = null;

            Database.TryToRun((dbcon) =>
            {
                users = Query<User>(dbcon, "GetUsers", new { userId = 0, userName = "" });
            }, "Do you want to try get message again ? [y/n] ");

            return users;
        }

        public static User GetUserBy(int userId)
        {
            User user = null;

            Database.TryToRun((dbcon) =>
            {
                user = QueryFirst<User>(dbcon, "GetUsers", new { userId, userName = "" });
            }, "Do you want to try again ? [y/n] ");

            return user;
        }

        public static User GetUserBy(string userName)
        {
            User user = null;

            Database.TryToRun((dbcon) =>
            {
                user = dbcon
                    .Query<dynamic>("GetUsers", new { userId = 0, userName }, commandType: CommandType.StoredProcedure)
                    .Select<dynamic, User>(u =>
                    {
                        switch (u.userRole)
                        {
                            case "Super":
                                return new SuperUser((int)u.userId, (string)u.userName, (string)u.firstName, (string)u.lastName, (string)u.userRole);
                            case "View":
                                return new ViewUser(u.userId, u.userName, u.firstName, u.lastName, u.userRole);
                            case "ViewEdit":
                                return new ViewEditUser(u.userId, u.userName, u.firstName, u.lastName, u.userRole);
                            case "ViewEditDelete":
                                return new ViewEditDeleteUser(u.userId, u.userName, u.firstName, u.lastName, u.userRole);
                            default:
                                return new User(u.userId, u.userName, u.firstName, u.lastName, u.userRole);
                        }
                    }).FirstOrDefault();

                //user = QueryFirst<User>(dbcon, "GetUsers", new { userId = 0, userName });
            }, "Do you want to try again ? [y/n] ");

            return user;
        }
        public static bool Exists(string userName)
        {
            return GetUserBy(userName) != null;
        }
        public static bool ValidateUserPassword(string username, string password)
        {
            bool validUserPassword = false;

            Database.TryToRun((dbcon) =>
            {
                validUserPassword = QueryFirst<int>(dbcon, "Validate_User", new { userName = username, userPassword = password }) == 1;
            }, "Do you want to try validating user/password again ? [y/n] ");

            return validUserPassword;
        }
        #endregion

        #region Messages Functions
        public static Message GetMessageById(int messageId)
        {
            Message message = null;

            Database.TryToRun((dbcon) =>
            {
                message = QueryFirst<Message>(dbcon, "GetMessages", new { messageId, userId = 0 });
            }, "Do you want to try get message again ? [y/n] ");

            return message;
        }
        public static IEnumerable<Message> GetUserMessages(int userId)
        {
            IEnumerable<Message> messages = null;

            Database.TryToRun((dbcon) =>
            {
                messages = Query<Message>(dbcon, "GetMessages", new { messageId = 0, userId });
            }, "Do you want to try get message again ? [y/n] ");

            return messages;


        }
        #endregion

        public static bool TryToRun(Action<SqlConnection> execute, string onFailMessage)
        {
            do
            {
                try
                {
                    using (SqlConnection dbcon = new SqlConnection(ConnectionString()))
                    {
                        dbcon.Open();
                        execute(dbcon);
                        return true;
                    }
                }
                catch (SqlException e)
                {
                    Alerts.Error(e.Message);
                    Console.Clear();
                }

            } while (MessageBox.Show(onFailMessage) == MessageBox.MessageBoxResult.Yes);

            return false;
        }
        public static int ExecuteProcedure(SqlConnection sqlConnection, string procedure, object parameters)
        {
            return sqlConnection.Execute(procedure, parameters, commandType: CommandType.StoredProcedure);
        }

        private static IEnumerable<T> Query<T>(SqlConnection sqlConnection, string procedure, object parameters)
        {
            return sqlConnection
                .Query<T>(procedure, parameters, commandType: CommandType.StoredProcedure);
        }
        public static T QueryFirst<T>(SqlConnection sqlConnection, string procedure, object parameters)
        {
            return sqlConnection
              .Query<T>(procedure, parameters, commandType: CommandType.StoredProcedure)
              .FirstOrDefault();
        }

    }
}
