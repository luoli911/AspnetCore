using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspnetCore.Models
{
    public class GlobalFile
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public bool IsTemplate { get; set; }
        public int TemplateId { get; set; }
        public virtual ICollection<Global> Globals { get; set; }
        public virtual ICollection<GlobalQueue> GlobalQueues { get; set; }

    }
}