using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspnetCore.Models
{
    public class Scenario
    {
        [Key]
        [Required]
        public int ScenarioId { get; set; }
        public string ScenarioName { get; set; }
        public int CellId { get; set; }
        [Required]
        public string Testapp { get; set; }
        [Required]
        public string ServerOS { get; set; }
        [Required]
        public string WebServer { get; set; }
        [Required]
        public string DotnetVersion { get; set; }
        [Required]
        public string PerformanceBranch { get; set; }
        [Required]
        public string MusicStoreBranch { get; set; }
        public int Duration { get; set; }
        public int VirtualClients { get; set; }
        public bool isAvailable { get; set; }
        public virtual Cell Cell { get; set; }
        public virtual ICollection<TestRun> TestRuns { get; set; }
    }
}