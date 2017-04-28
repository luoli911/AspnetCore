using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspnetCore.Models
{
   
        public class Global
        {
            [Key]
            [JsonIgnore]
            public int GlobalId { get; set; }
            [JsonIgnore]
            public int GlobalFileId { get; set; }
            public Window[] Windows { get; set; }
            public Linux[] Linux { get; set; }
            public Docker[] Docker { get; set; }

            [JsonIgnore]
            public virtual GlobalFile GlobalFile { get; set; }
    }

        public class Window
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        public class Linux
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        public class Docker
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

    
}