using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DirigoEdge.PluginFramework
{
    public static class DB
    {
        public static DataContext GetContext()
        {
            return new DataContext();
        }
    }
}