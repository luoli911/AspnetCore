using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace AspnetCore.Models
{
    public class ContextInitializer:System.Data.Entity.DropCreateDatabaseIfModelChanges<RunTestContext>
    {
        protected override void Seed(RunTestContext context)
        {
            

            var stepfile = new List<StepFile>
            {
                new StepFile {Id=1,FileName="step-HelloWorldMvc-linux-Nginx",IsTemplate=true },
                new StepFile {Id=2,FileName="step-MusicStoreHome-linux-Nginx",IsTemplate=true }
            };
            stepfile.ForEach(s => context.StepFiles.AddOrUpdate(s));
            context.SaveChanges();

            var scenarios = new List<Scenario>
            {
                new Scenario { ScenarioId=1,Testapp="HelloWorldMvc",ServerOS="Linux" ,WebServer="Nginx",DotnetVersion="1.1" },
                new Scenario { ScenarioId=2,Testapp="MusicStoreHome",ServerOS="Linux",WebServer="Nginx",DotnetVersion="1.1" }
            };
            scenarios.ForEach(s => context.Scenarios.AddOrUpdate(s));
            context.SaveChanges();
            

        }
    }
}