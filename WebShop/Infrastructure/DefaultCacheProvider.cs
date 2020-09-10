using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace WebShop.Infrastructure
{
    public class DefaultCacheProvider : ICacheProvider
    {
        private Cache Cache
        {   
            get
            {
                return HttpContext.Current.Cache;
            }
        }
        public object Get(string Key)
        {
            return Cache[Key];
        }
          
        public void Invalidate(string Key)
        {
            Cache.Remove(Key);
        }

        public bool IsSet(string Key)
        {
            return (Cache[Key] != null);
        }

        public void Set(string Key, object data, int cacheTime)
        {
            var expirationTime = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            Cache.Insert(Key, data, null, expirationTime, Cache.NoSlidingExpiration);

        }
    }
}