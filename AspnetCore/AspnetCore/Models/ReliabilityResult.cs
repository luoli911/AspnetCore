using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AspnetCore.Models
{
    public class ReliabilityResult
    {
        [Key]
        public int ReliabilityResultId { get; set; }
        //public int ScenarioId { get; set; }
        //public int TestRunId { get; set; }
        //public int CellId { get; set; }
        public int Reliability { get; set; }
        public double Throughput { get; set; }
        public double MedianLatency { get; set; }
        public double P95Latency { get; set; }
        //public virtual Cell cell { get; set; }
        //public virtual Scenario Scenario { get; set; }
        public virtual ICollection<TestRun> TestRuns { get; set; }

        
    }
}