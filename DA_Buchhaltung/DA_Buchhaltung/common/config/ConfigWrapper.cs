using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DA_Buchhaltung.common.config
{
    public static class ConfigWrapper
    {
        public static string LogDirectory
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["LogDirectory"];
            }
            set
            {
                System.Configuration.ConfigurationManager.AppSettings["LogDirectory"] = value;
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["LogDirectory"].Value = value;
                config.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        public static int LogLevel
        {
            get
            {
                return int.Parse(System.Configuration.ConfigurationManager.AppSettings["LogLevel"]);
            }
            set
            {
                System.Configuration.ConfigurationManager.AppSettings["LogLevel"] = value.ToString();
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["LogLevel"].Value = value.ToString();
                config.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection("appSettings");
            }
        }


    }
}

