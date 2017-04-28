namespace AspnetCore.Migrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AspnetCore.Models.RunTestContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "AspnetCore.Models.RunTestContext";
        }

        protected override void Seed(AspnetCore.Models.RunTestContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var stepfile = new List<StepFile>
            {
                new StepFile {Id=1,FileName="step-HelloWorldMvc-linux-Nginx.json",FilePath="C:\\RunRemote\\step-HelloWorldMvc-linux-Nginx.json",IsTemplate=true },
                new StepFile {Id=2,FileName="step-MusicStoreHome-linux-Nginx.json",FilePath="C:\\RunRemote\\step-MusicStoreHome-linux-Nginx.json",IsTemplate=true },
                new StepFile {Id=3,FileName="step-MusicStoreE2E-linux-Nginx.json",FilePath="C:\\RunRemote\\step-MusicStoreE2E-linux-Nginx.json",IsTemplate=true },
                new StepFile {Id=4,FileName="step-StressMvc-linux-Nginx.json",FilePath="C:\\RunRemote\\step-StressMvc-linux-Nginx.json",IsTemplate=true },
                new StepFile {Id=5,FileName="step-HelloWorldMvc-linux-Kestrel.json",FilePath="C:\\RunRemote\\step-HelloWorldMvc-linux-Kestrel.json",IsTemplate=true }
            };
            stepfile.ForEach(s => context.StepFiles.AddOrUpdate(s));
            context.SaveChanges();
            var globalfile = new List<GlobalFile>
            {
                new GlobalFile {Id=1,FileName="global.json",FilePath="C:\\RunRemote\\global.json",IsTemplate=true }
            };
            globalfile.ForEach(s => context.GlobalFiles.AddOrUpdate(s));
            context.SaveChanges();

            var cells = new List<Cell>
            {
                new Cell { CellId=1,CellName="lcell1",Machine="Azure-A2",ARM="x64",OS="Linux",Username="asplab",Password="Asplab@123",ServerName="win1",ClientName="linux1"},
                new Cell { CellId=2,CellName="lcell2",Machine="Azure-A2",ARM="x64",OS="Linux",Username="asplab",Password="Asplab@123",ServerName="win1",ClientName="linux1" },
                new Cell { CellId=3,CellName="lcell3",Machine="Azure-A2",ARM="x64",OS="Linux",Username="asplab",Password="Asplab@123",ServerName="win1",ClientName="linux1"},
                new Cell { CellId=4,CellName="lcell4",Machine="Azure-A2",ARM="x64",OS="Linux",Username="asplab",Password="Asplab@123",ServerName="win1",ClientName="linux1" },
                new Cell { CellId=5,CellName="lcell5",Machine="Azure-A2",ARM="x64",OS="Linux",Username="asplab",Password="Asplab@123",ServerName="win1",ClientName="linux1"},
                new Cell { CellId=6,CellName="lcell6",Machine="Azure-A2",ARM="x64",OS="Linux",Username="asplab",Password="Asplab@123",ServerName="win1",ClientName="linux1" }
            };
            cells.ForEach(s => context.Cells.AddOrUpdate(s));
            context.SaveChanges();

            var scenarios = new List<Scenario>
            {
                new Scenario { ScenarioId=1,CellId=1,Testapp="HelloWorldMvc",ServerOS="Linux" ,WebServer="Nginx",DotnetVersion="1.1",PerformanceBranch="dev",MusicStoreBranch="dev",ScenarioName="HelloWorldMvc-Linux-Nginx-dev",Duration=24,VirtualClients=300},
                new Scenario { ScenarioId=2,CellId=2,Testapp="MusicStoreHome",ServerOS="Linux",WebServer="Nginx",DotnetVersion="1.1",PerformanceBranch="dev",MusicStoreBranch="dev",ScenarioName="MusicStoreHome-Linux-Nginx-dev",Duration=24,VirtualClients=300},
                new Scenario { ScenarioId=3,CellId=3,Testapp="MusicStoreE2E",ServerOS="Linux" ,WebServer="Nginx",DotnetVersion="1.1",PerformanceBranch="dev",MusicStoreBranch="dev",ScenarioName="MusicStoreE2E-Linux-Nginx-dev",Duration=24,VirtualClients=300},
                new Scenario { ScenarioId=4,CellId=4,Testapp="StressMvc",ServerOS="Linux",WebServer="Nginx",DotnetVersion="1.1",PerformanceBranch="dev",MusicStoreBranch="dev",ScenarioName="StressMvc-Linux-Nginx-dev",Duration=24,VirtualClients=100},
                new Scenario { ScenarioId=5,CellId=5,Testapp="HelloWorldMvc",ServerOS="Linux" ,WebServer="Kestrel",DotnetVersion="1.1",PerformanceBranch="dev",MusicStoreBranch="dev",ScenarioName="HelloWorldMvc-Linux-Kestrel-dev",Duration=24,VirtualClients=300}

            };
            scenarios.ForEach(s => context.Scenarios.AddOrUpdate(s));
            context.SaveChanges();


            var reliabilityresults = new List<ReliabilityResult>
            {
                new ReliabilityResult {ReliabilityResultId=1,Reliability=100,Throughput=3116.8,MedianLatency=46,P95Latency=52 },
                new ReliabilityResult {ReliabilityResultId=2,Reliability=100,Throughput=7983.93,MedianLatency=22,P95Latency=22 }
            };
            reliabilityresults.ForEach(s => context.ReliabilityResults.AddOrUpdate(s));
            context.SaveChanges();

            var testruns = new List<TestRun>
            {
                new TestRun {RunId=1,RunName="Reliability",ScenarioId=1,ReliabilityResultId=1,StartTime=DateTime.Now,Tester="Alice",State=State.Available },
                 new TestRun {RunId=2,RunName="Reliability",ScenarioId=5,ReliabilityResultId=2,StartTime=DateTime.Now,Tester="Alice",State=State.Available }
            };
            testruns.ForEach(s => context.TestRuns.AddOrUpdate(s));
            context.SaveChanges();

        }
    }
}
