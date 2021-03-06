﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Individual
{
    class MessageToFile
    {
        private static string SaveFolder => Path.Combine(Environment.CurrentDirectory, "Messages");
        private static string FullFileName(int messageId) => Path.Combine(SaveFolder, $"Message_{messageId.ToString("D10")}.txt");

        public static void Save(Message message)
        {
            MessageToFile.TryToRun(() =>
            {
                CreateDirectory(SaveFolder);

                string content = Convert(message);

                File.WriteAllText(FullFileName(message.MessageId), content);
            }, "Failed to save message to file. Try again ? [y/n]");
        }

        public static void Delete(Message message)
        {
            MessageToFile.TryToRun(() =>
            {
                File.Delete(FullFileName(message.MessageId));
            }, "Failed to delete message to file. Try again ? [y/n]");

        }

        private static void CreateDirectory(string saveToPath)
        {
            if (!Directory.Exists(saveToPath))
            {
                Directory.CreateDirectory(saveToPath);
            }
        }
        public static void TryToRun(Action execute, string onFailMessage)
        {
            do
            {
                try
                {
                    execute();
                    return;
                }
                catch (IOException e)
                {
                    Alerts.Error(e.Message);
                    Console.Clear();
                }

            } while (MessageBox.Show(onFailMessage) == MessageBox.MessageBoxResult.Yes);

        }
        private static string Convert(Message message)
        {
            return $"From\t: {message.SenderUserName}\r\nTo\t: {message.ReceiverUserName}\r\nDate\t: {message.SendAt.ToLongDateString()}\r\nTime\t: {message.SendAt.ToLongTimeString()}\r\nSubject\t: {message.Subject}\r\nBody\t: {message.Body}";
        }
    }
}
