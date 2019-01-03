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
            CreateDirectory(SaveFolder);

            string content = Convert(message);

            File.WriteAllText(FullFileName(message.MessageId), content);
        }

        public static void Delete(Message message)
        {
            File.Delete(FullFileName(message.MessageId));
        }

        private static void CreateDirectory(string saveToPath)
        {
            if (!Directory.Exists(saveToPath))
            {
                Directory.CreateDirectory(saveToPath);
            }
        }

        private static string Convert(Message message)
        {
            return $"Date\t: {message.SendAt.ToLongDateString()}\nTime\t: {message.SendAt.ToLongTimeString()}\nSubject\t: {message.Subject}\nBody\t: {message.Body}";
        }
    }
}
