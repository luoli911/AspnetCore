using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AspnetCore.Models
{
    public enum State
    {
        Available, InProcess, Success, Fail
    }
    public class TestRun
    {
        [Key]
        [Required]
        public int RunId { get; set; }
        [Required]
        public string RunName { get; set; }
        [Required]
        public int ScenarioId { get; set; }
        public int? ReliabilityResultId { get; set; }
        public int? PerformanceResultId { get; set; }
        public State? State { get; set; }
        public string ScriptPath { get; set; }
        public DateTime StartTime { get; set; }
        public string Tester { get; set; }
        public virtual Scenario Scenario { get; set; }
        public virtual ReliabilityResult ReliabilityResult { get; set; }
        public virtual PerformanceResult PerformanceResult { get; set; }
    }
}