using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace AspnetCore.Models
{
    public class RunTestContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public RunTestContext() : base("name=RunTestContext")
        {
        }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //    //base.OnModelCreating(modelBuilder);
        //}
        public System.Data.Entity.DbSet<AspnetCore.Models.Scenario> Scenarios { get; set; }
        public System.Data.Entity.DbSet<AspnetCore.Models.Cell> Cells { get; set; }
        public System.Data.Entity.DbSet<AspnetCore.Models.Step> Steps { get; set; }
        public System.Data.Entity.DbSet<AspnetCore.Models.Global> Globals { get; set; }
        public System.Data.Entity.DbSet<AspnetCore.Models.StepFile> StepFiles { get; set; }
        public System.Data.Entity.DbSet<AspnetCore.Models.GlobalFile> GlobalFiles { get; set; }
        public System.Data.Entity.DbSet<AspnetCore.Models.StepQueue> StepQueues { get; set; }
        public System.Data.Entity.DbSet<AspnetCore.Models.GlobalQueue> GlobalQueues { get; set; }
        public System.Data.Entity.DbSet<AspnetCore.Models.TestRun> TestRuns { get; set; }
        public System.Data.Entity.DbSet<AspnetCore.Models.ReliabilityResult> ReliabilityResults { get; set; }
        public System.Data.Entity.DbSet<AspnetCore.Models.PerformanceResult> PerformanceResults { get; set; }
    }
}
