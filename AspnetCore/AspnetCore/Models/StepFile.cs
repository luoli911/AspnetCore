using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspnetCore.Models
{
    public class StepFile
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public bool IsTemplate { get; set; }
        [Required]
        public int TemplateId { get; set; }
        public virtual ICollection<Step> Steps { get; set; }
        public virtual ICollection<StepQueue> StepQueues { get; set; }
    }
}