using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AspnetCore.Models
{
    public class PerformanceResult
    {
        [Key]
        public int PerformanceResultId { get; set; }
        //public int ScenarioId { get; set; }
        //public int CellId { get; set; }
        public int TestRunId { get; set; }
        public decimal Throughput { get; set; }
        public decimal Latency { get; set; }
        public Int32 Memory { get; set; }
        public decimal CPU { get; set; }
        public decimal Reliability { get; set; }
        public int Requests { get; set; }
        public virtual ICollection<TestRun> TestRuns { get; set; }
    }
}