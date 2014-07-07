using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DirigoEdge.Entities;

namespace DirigoEdge.Areas.Admin.Models.ViewModels
{
    public class EditSchemaViewModel
    {
        public Schema TheSchema;

        public EditSchemaViewModel(int id)
        {
            using (var context = new DataContext())
            {
                TheSchema = context.Schemas.FirstOrDefault(x => x.SchemaId == id);

                TheSchema.JSONData = TheSchema.JSONData ?? "{ }";
            }
        }
    }
}