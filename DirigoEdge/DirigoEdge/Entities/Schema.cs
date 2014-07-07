using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DirigoEdge.Entities
{
    public class Schema
    {
        [Key]
        public virtual int SchemaId { get; set; }

        public virtual String DisplayName { get; set; }
        public virtual String JSONData { get; set; }
    }
}