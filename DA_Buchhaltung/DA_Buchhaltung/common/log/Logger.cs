using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA_Buchhaltung.common.config;

namespace DA_Buchhaltung.common.log
{
    public static class Logger
    {
        private static int _ALL = 3;
        private static int _WARNING = 2;
        private static int _ERROR = 1;
        private static int _OFF = 0;
        private static String logDirectory = ConfigWrapper.LogDirectory;
        public static int ALL
        {
            get { return _ALL; }
        }
        public static int ERROR
        {
            get { return _ERROR; }
        }
        public static int INFO
        {
            get { return _WARNING; }
        }
        public static int OFF
        {
            get { return _OFF; }
        }
        public static void append(String message, int level)
        {
            int logLevel = ConfigWrapper.LogLevel;
            try
            {

            
            if (logLevel >= level)
            {
                DateTime dt = DateTime.Now;
                String filePath = Path.Combine(logDirectory, dt.ToString("yyyyMMdd") + ".log");
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
                if (!File.Exists(filePath))
                {
                    FileStream fs = File.Create(filePath);
                    fs.Close();
                }
                try
                {
                    StreamWriter sw = File.AppendText(filePath);
                    sw.WriteLine(dt.ToString("hh:mm:ss") + " | " + message);
                    sw.Flush();
                    sw.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Fehler beim Logger");
                    Debug.WriteLine(e.Message);
                }
            }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Fehler beim Logger");
                Debug.WriteLine(e.Message);
            }
        }
    }
}

