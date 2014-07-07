using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirigoEdge.Entities;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
    public class ManageSchemasViewModel
    {
        public List<Schema> Schemas;

        public ManageSchemasViewModel()
        {
            using (var context = new DataContext())
            {
                Schemas = context.Schemas.ToList();
            }
        }
    }
}