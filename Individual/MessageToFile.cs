using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace Individual
{
    class MessageToFile
    {

        public static bool Save(IEnumerable<Message> messages)
        {
            string SaveFolder = Path.Combine(Environment.CurrentDirectory, "Messages");

            try
            {
                CreateDirectory(SaveFolder);

                foreach (var message in messages)
                {
                    string fullFileName = Path.Combine(SaveFolder, CreateFileName(message));
                    string content = Convert(message);
                    System.IO.File.WriteAllText(fullFileName, content);
                }
                return true;
            }
            catch (IOException)
            {
                Alert.Warning("Error Creating File or path !!!");
            }

            return false;
        }
        private static void CreateDirectory(string saveToPath)
        {
            if (!Directory.Exists(saveToPath))
            {
                Directory.CreateDirectory(saveToPath);
            }
        }
        private static string CreateFileName(Message message)
        {
            return $"Message_{message.MessageId.ToString("D10")}.txt";
        }
        private static string Convert(Message message)
        {
            return $"Date\t: {message.SendAt.ToLongDateString()}\nTime\t: {message.SendAt.ToLongTimeString()}\nSubject\t: {message.Subject}\nBody\t: {message.Body}";
        }
    }
}
