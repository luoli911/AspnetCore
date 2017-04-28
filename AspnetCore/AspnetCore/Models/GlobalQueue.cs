using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspnetCore.Models
{
    public class GlobalQueue
    {
        [Key]
        [Required]
        public int QueueId { get; set; }
        public int GlobalFileId { get; set; }
        [Required]
        public string GlobalName { get; set; }
        [Required]
        public string GlobalValue { get; set; }
        public string State { get; set; }
        public virtual GlobalFile GlobalFile { get; set; }
    }
}