namespace AspnetCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cells",
                c => new
                    {
                        CellId = c.Int(nullable: false, identity: true),
                        CellName = c.String(),
                        Machine = c.String(),
                        OS = c.String(),
                        ARM = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                        ServerName = c.String(),
                        ClientName = c.String(),
                    })
                .PrimaryKey(t => t.CellId);
            
            CreateTable(
                "dbo.Scenarios",
                c => new
                    {
                        ScenarioId = c.Int(nullable: false, identity: true),
                        ScenarioName = c.String(),
                        CellId = c.Int(nullable: false),
                        Testapp = c.String(nullable: false),
                        ServerOS = c.String(nullable: false),
                        WebServer = c.String(nullable: false),
                        DotnetVersion = c.String(nullable: false),
                        PerformanceBranch = c.String(nullable: false),
                        MusicStoreBranch = c.String(nullable: false),
                        Duration = c.Int(nullable: false),
                        VirtualClients = c.Int(nullable: false),
                        isAvailable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ScenarioId)
                .ForeignKey("dbo.Cells", t => t.CellId, cascadeDelete: true)
                .Index(t => t.CellId);
            
            CreateTable(
                "dbo.TestRuns",
                c => new
                    {
                        RunId = c.Int(nullable: false, identity: true),
                        RunName = c.String(nullable: false),
                        ScenarioId = c.Int(nullable: false),
                        ReliabilityResultId = c.Int(),
                        PerformanceResultId = c.Int(),
                        State = c.Int(),
                        ScriptPath = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        Tester = c.String(),
                    })
                .PrimaryKey(t => t.RunId)
                .ForeignKey("dbo.PerformanceResults", t => t.PerformanceResultId)
                .ForeignKey("dbo.ReliabilityResults", t => t.ReliabilityResultId)
                .ForeignKey("dbo.Scenarios", t => t.ScenarioId, cascadeDelete: true)
                .Index(t => t.ScenarioId)
                .Index(t => t.ReliabilityResultId)
                .Index(t => t.PerformanceResultId);
            
            CreateTable(
                "dbo.PerformanceResults",
                c => new
                    {
                        PerformanceResultId = c.Int(nullable: false, identity: true),
                        TestRunId = c.Int(nullable: false),
                        Throughput = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Latency = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Memory = c.Int(nullable: false),
                        CPU = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Reliability = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Requests = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PerformanceResultId);
            
            CreateTable(
                "dbo.ReliabilityResults",
                c => new
                    {
                        ReliabilityResultId = c.Int(nullable: false, identity: true),
                        Reliability = c.Int(nullable: false),
                        Throughput = c.Double(nullable: false),
                        MedianLatency = c.Double(nullable: false),
                        P95Latency = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ReliabilityResultId);
            
            CreateTable(
                "dbo.GlobalFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        FilePath = c.String(),
                        IsTemplate = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GlobalQueues",
                c => new
                    {
                        QueueId = c.Int(nullable: false, identity: true),
                        GlobalFileId = c.Int(nullable: false),
                        GlobalName = c.String(nullable: false),
                        GlobalValue = c.String(nullable: false),
                        State = c.String(),
                    })
                .PrimaryKey(t => t.QueueId)
                .ForeignKey("dbo.GlobalFiles", t => t.GlobalFileId, cascadeDelete: true)
                .Index(t => t.GlobalFileId);
            
            CreateTable(
                "dbo.Globals",
                c => new
                    {
                        GlobalId = c.Int(nullable: false, identity: true),
                        GlobalFileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GlobalId)
                .ForeignKey("dbo.GlobalFiles", t => t.GlobalFileId, cascadeDelete: true)
                .Index(t => t.GlobalFileId);
            
            CreateTable(
                "dbo.StepFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(nullable: false),
                        FilePath = c.String(),
                        IsTemplate = c.Boolean(nullable: false),
                        TemplateId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StepQueues",
                c => new
                    {
                        QueueId = c.Int(nullable: false, identity: true),
                        StepId = c.Int(nullable: false),
                        StepFileId = c.Int(nullable: false),
                        StepName = c.String(nullable: false),
                        StepValue = c.String(nullable: false),
                        State = c.String(),
                    })
                .PrimaryKey(t => t.QueueId)
                .ForeignKey("dbo.StepFiles", t => t.StepFileId, cascadeDelete: true)
                .Index(t => t.StepFileId);
            
            CreateTable(
                "dbo.Steps",
                c => new
                    {
                        StepId = c.Int(nullable: false, identity: true),
                        StepFileId = c.Int(nullable: false),
                        ServerName = c.String(),
                        ClientName = c.String(),
                        DotnetVersion = c.String(),
                        TestappName = c.String(),
                        WebServer = c.String(),
                        ServerOS = c.String(),
                    })
                .PrimaryKey(t => t.StepId)
                .ForeignKey("dbo.StepFiles", t => t.StepFileId, cascadeDelete: true)
                .Index(t => t.StepFileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Steps", "StepFileId", "dbo.StepFiles");
            DropForeignKey("dbo.StepQueues", "StepFileId", "dbo.StepFiles");
            DropForeignKey("dbo.Globals", "GlobalFileId", "dbo.GlobalFiles");
            DropForeignKey("dbo.GlobalQueues", "GlobalFileId", "dbo.GlobalFiles");
            DropForeignKey("dbo.TestRuns", "ScenarioId", "dbo.Scenarios");
            DropForeignKey("dbo.TestRuns", "ReliabilityResultId", "dbo.ReliabilityResults");
            DropForeignKey("dbo.TestRuns", "PerformanceResultId", "dbo.PerformanceResults");
            DropForeignKey("dbo.Scenarios", "CellId", "dbo.Cells");
            DropIndex("dbo.Steps", new[] { "StepFileId" });
            DropIndex("dbo.StepQueues", new[] { "StepFileId" });
            DropIndex("dbo.Globals", new[] { "GlobalFileId" });
            DropIndex("dbo.GlobalQueues", new[] { "GlobalFileId" });
            DropIndex("dbo.TestRuns", new[] { "PerformanceResultId" });
            DropIndex("dbo.TestRuns", new[] { "ReliabilityResultId" });
            DropIndex("dbo.TestRuns", new[] { "ScenarioId" });
            DropIndex("dbo.Scenarios", new[] { "CellId" });
            DropTable("dbo.Steps");
            DropTable("dbo.StepQueues");
            DropTable("dbo.StepFiles");
            DropTable("dbo.Globals");
            DropTable("dbo.GlobalQueues");
            DropTable("dbo.GlobalFiles");
            DropTable("dbo.ReliabilityResults");
            DropTable("dbo.PerformanceResults");
            DropTable("dbo.TestRuns");
            DropTable("dbo.Scenarios");
            DropTable("dbo.Cells");
        }
    }
}
