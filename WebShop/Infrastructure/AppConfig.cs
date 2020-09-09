using Glimpse.AspNet.Tab;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebShop.Infrastructure
{
    public class AppConfig
    {
        private static string _photosFolderRelative = ConfigurationManager.AppSettings["PhotosFolder"];
        public static string PhotosFolderRelative
        {
            get
            {
                return _photosFolderRelative;
            }
        }

    }
}