using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DirigoEdge.Entities
{
    public class Plugin
    {
        [Key]
        public virtual int PluginId { get; set; }

        /// <summary>
        /// Used for Display In the App Store
        /// </summary>
        public virtual string PluginDisplayName { get; set; }

        // References the ID of the plugin in the app store. This is a unique identifier
        public virtual int AppStoreId { get; set; }

        public virtual double Version { get; set; }
        public virtual string Description { get; set; }

        // Location of .zip file on app store
        public virtual string FileLocation { get; set; }
    }
}