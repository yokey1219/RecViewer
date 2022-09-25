using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationConfigManager;

namespace RecViewer
{
    public class GerneralConfig
    {
        private static AbstractConfigManager settingManager = AbstractConfigManager.createFileConfigManager("", "setting");
        private static AbstractConfigManager userDataManager = AbstractConfigManager.createFileConfigManager("", "userdata");

        public static Boolean Is999
        {
            get
            {
                if (settingManager.readConfig("is999") != null)
                {
                    return Convert.ToBoolean(settingManager.readConfig("is999").Value);
                }
                else
                    return false;
            }
            set
            {
                settingManager.addConfig(new ConfigItem("is999", value));
            }
        }

        public static String[] BaudRate
        {
            get
            {
                if (settingManager.readConfig("baudrate") != null)
                {
                    String baudrate_str = (String)settingManager.readConfig("baudrate").Value;
                    return baudrate_str.Split(',');
                }
                else
                    return new String[] { "9600" };
            }
        }

        public static void setUserData(String key, String value)
        {
            userDataManager.addConfig(new ConfigItem(key, value));
        }

        public static object getUserData(String key)
        {
            ConfigItem item = userDataManager.readConfig(key);
            return item == null ? null : item.Value;
        }
    }
}
