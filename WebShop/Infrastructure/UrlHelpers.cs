using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebShop.Infrastructure
{   
    public static class UrlHelpers
    {
        public static string ProductPhotoPath(this UrlHelper helper, string productFilename)
        {
            var productPhotoPath = AppConfig.PhotosFolderRelative;
            var path = Path.Combine(productPhotoPath, productFilename);
            var absolutePath = helper.Content(path);
            return absolutePath;
        }
    }
}