namespace UnigisTest
{
    public class UnigisLog
    {
        #region "Singleton"

        private static UnigisLog instance = null;

        private static object mutex = new object();
        private UnigisLog()
        {
        }
        public static UnigisLog GetInstance()
        {

            if (instance == null)
            {
                lock ((mutex))
                {
                    instance = new UnigisLog();
                }
            }

            return instance;

        }

        #endregion
        private string root = @"C:\UnigisTest";
        public void Log(string result, string method, string message)
        {
            SaveLog(result, method, message);
        }

        private void SaveLog(string result, string method, string message)
        {
            try
            {
                CreateDirectoryLog();

                string pathLog = @$"{root}\{DateTime.Now.ToString("yyyyMMdd")}.txt";
                string textWrite = $"**{result}** [{method}] - [{DateTime.Now}] {message}";

                if (!File.Exists(pathLog))
                {
                    using (StreamWriter streamWriter = File.CreateText(pathLog))
                    {
                        streamWriter.WriteLine(textWrite);
                    }
                }
                else
                {
                    using (StreamWriter streamWriter = File.AppendText(pathLog))
                    {
                        streamWriter.WriteLine(textWrite);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }

        }
        private void CreateDirectoryLog()
        {
            try
            {
                if(!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }
        }
    }
}
