namespace AspnetCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createnewdb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GlobalFiles", "TemplateId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GlobalFiles", "TemplateId");
        }
    }
}
