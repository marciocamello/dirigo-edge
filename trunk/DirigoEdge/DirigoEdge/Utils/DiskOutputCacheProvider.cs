using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace DirigoEdge.Utils
{
    // Implementation based on http://www.4guysfromrolla.com/articles/061610-1.aspx
    public class DiskOutputCacheProvider : OutputCacheProvider
    {
        readonly IDictionary<string, CacheItem> cacheItems = new ConcurrentDictionary<string, CacheItem>();
        string cacheFolder;

        public override void Initialize(string name, NameValueCollection config)
        {
            HttpServerUtility serverUtil = HttpContext.Current.Server;

            const string cacheFolderKey = "cacheFolder";
            string cacheFolderValue = config[cacheFolderKey];
            string folder;

            if (!String.IsNullOrEmpty(cacheFolderValue))
            {
                folder = serverUtil.MapPath(cacheFolderValue);

                config.Remove(cacheFolderKey);
            }
            else
            {
                throw new ArgumentException(String.Format("The attribute '{0}' is missing in the configuration of the '{1}' provider.", cacheFolderKey, name));
            }

            if (folder[folder.Length - 1] != Path.DirectorySeparatorChar)
            {
                folder += Path.DirectorySeparatorChar;   
            }

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            this.cacheFolder = folder;

            base.Initialize(name, config);
        }

        public override object Add(string key, object entry, DateTime utcExpiry)
        {
            // See if this key already exists in the cache. If so, we need to return it and NOT overwrite it!
            object results = Get(key);
            
            if (results != null)
            {
                return results;
            }
                
            // If the item is NOT in the cache, then save it!
            Set(key, entry, utcExpiry);

            return entry;
        }

        public override object Get(string key)
        {
            CacheItem item;

            if (!this.cacheItems.TryGetValue(key, out item))
            {
                return null;
            }

            // If user is logged in, don't retrieve cached data, and remove any references to old data
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                // Item has expired
                Remove(key, item);

                return null;
            }
                
            // Check if item has expired
            if (item.UtcExpiry < DateTime.UtcNow)
            {
                // Item has expired
                Remove(key, item);

                return null;
            }

            return GetCacheData(item);
        }

        private object GetCacheData(CacheItem item)
        {
            string fileToRetrieve = GetFilePath(item);

            var formatter = new BinaryFormatter();
            Stream source = null;

            try
            {
                source = new FileStream(fileToRetrieve, FileMode.Open, FileAccess.Read, FileShare.Read);

                return formatter.Deserialize(source);
            }
            catch (IOException)
            {

            }
            finally
            {
                if (source != null)
                {
                    source.Dispose();
                }
            }

            return null;
        }

        public override void Remove(string key)
        {
            CacheItem item;

            if (this.cacheItems.TryGetValue(key, out item))
            {
                Remove(key, item);
            }
        }

        void Remove(string key, CacheItem item)
        {
            RemoveCacheData(item);
            this.cacheItems.Remove(key);
        }

        void RemoveCacheData(CacheItem item)
        {
            string fileToDelete = GetFilePath(item);

            try
            {
                File.Delete(fileToDelete);
            }
            catch (IOException) { }
        }

        public override void Set(string key, object entry, DateTime utcExpiry)
        {
            // Do not cache any data if the user is logged in
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return;
            }

            // Create a DiskOutputCacheItem object
            var item = new CacheItem(key, utcExpiry);

            WriteCacheData(item, entry);

            // Add item to CacheItems, if needed, or update the existing key, if it already exists
            this.cacheItems[key] = item;
        }

        void WriteCacheData(CacheItem item, object entry)
        {
            string fileToWrite = GetFilePath(item);

            BinaryFormatter formatter = new BinaryFormatter();
            Stream destination = null;

            try
            {
                destination = new FileStream(fileToWrite, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
                formatter.Serialize(destination, entry);
            }
            catch (IOException)
            {

            }
            finally
            {
                if (destination != null)
                {
                    destination.Dispose();
                }
            }
        }

        string GetFilePath(CacheItem item)
        {
            return this.cacheFolder + item.FileName;
        }

        class CacheItem
        {
            static readonly char[] invalidFileNameChars = Path.GetInvalidFileNameChars();

            public string FileName { get; private set; }
            public DateTime UtcExpiry { get; private set; }

            public CacheItem(string key, DateTime utcExpiry)
            {
                this.FileName = GetSafeFileName(key);
                this.UtcExpiry = utcExpiry;
            }

            string GetSafeFileName(string unsafeFileName)
            {

                char[] invalid = unsafeFileName.ToCharArray()
                .Where(c => invalidFileNameChars.Contains(c))
                .ToArray();

                if (invalid.Length > 0)
                {
                    var sb = new StringBuilder(unsafeFileName, unsafeFileName.Length);

                    for (int i = 0; i < invalid.Length; i++)
                    {
                        sb.Replace(invalid[i], '_');
                    }

                    return sb.ToString();
                }

                return unsafeFileName;
            }
        }
    }
}