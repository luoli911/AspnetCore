using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspnetCore.Models
{
    public class StepQueue
    {
        [Key]
        [Required]
        public int QueueId { get; set; }
        public int StepId { get; set; }
        public int StepFileId { get; set; }
        [Required]
        public string StepName { get; set; }
        [Required]
        public string StepValue { get; set; }
        public string State { get; set; }
        public virtual StepFile StepFile { get; set; }
    }
}