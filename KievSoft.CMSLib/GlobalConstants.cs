using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KievSoft.CMSLib
{
    public class GlobalConstants
    {
        public static string ROOT_PATH = ConfigurationManager.AppSettings["ROOT_PATH"] ?? string.Empty;
        public static string CONNECTION_STRING_NAME = ConfigurationManager.AppSettings["CONNECTION_STRING_NAME"] ?? "CMS_CONSTR";
        public static string CONNECTION_STRING = ConfigurationManager.AppSettings["CONNECTION_STRING"] ?? string.Empty;
        public static string USER_STORAGE = ConfigurationManager.AppSettings["USER_STORAGE"] ?? "KIEV_USER_STORAGE";
        public static byte UserActiveStatusId = Convert.ToByte(ConfigurationManager.AppSettings["UserActiveStatusId"] ?? "1");
    }
}
