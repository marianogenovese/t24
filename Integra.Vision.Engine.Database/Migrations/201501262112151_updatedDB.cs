namespace Integra.Vision.Engine.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedDB : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Args", "AdapterId", "dbo.Adapters");
            DropForeignKey("dbo.Stmts", "AdapterId", "dbo.Adapters");
            DropForeignKey("dbo.Stmts", "TriggerId", "dbo.Triggers");
            DropForeignKey("dbo.Adapters", "ObjectId", "dbo.UserDefinedObjects");
            DropForeignKey("dbo.Adapters", "Reference", "dbo.Assemblies");
            DropForeignKey("dbo.Sources", "AdapterId", "dbo.Adapters");
            DropForeignKey("dbo.Triggers", "ObjectId", "dbo.UserDefinedObjects");
            DropForeignKey("dbo.Triggers", "StreamId", "dbo.Streams");
            DropForeignKey("dbo.Assemblies", "ObjectId", "dbo.UserDefinedObjects");
            DropIndex("dbo.Args", new[] { "AdapterId" });
            DropIndex("dbo.Stmts", new[] { "TriggerId" });
            DropIndex("dbo.Stmts", new[] { "AdapterId" });
            DropIndex("dbo.Adapters", new[] { "ObjectId" });
            DropIndex("dbo.Adapters", new[] { "Reference" });
            DropIndex("dbo.Sources", new[] { "AdapterId" });
            DropIndex("dbo.Triggers", new[] { "ObjectId" });
            DropIndex("dbo.Triggers", new[] { "StreamId" });
            DropIndex("dbo.Assemblies", new[] { "ObjectId" });
            DropColumn("dbo.Sources", "AdapterId");
            DropTable("dbo.Args");
            DropTable("dbo.Stmts");
            DropTable("dbo.Adapters");
            DropTable("dbo.Triggers");
            DropTable("dbo.Assemblies");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Assemblies",
                c => new
                    {
                        ObjectId = c.Guid(nullable: false),
                        LocalPath = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ObjectId);
            
            CreateTable(
                "dbo.Triggers",
                c => new
                    {
                        ObjectId = c.Guid(nullable: false),
                        DurationTime = c.Double(),
                        StreamId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ObjectId);
            
            CreateTable(
                "dbo.Adapters",
                c => new
                    {
                        ObjectId = c.Guid(nullable: false),
                        AdapterType = c.Int(nullable: false),
                        Reference = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ObjectId);
            
            CreateTable(
                "dbo.Stmts",
                c => new
                    {
                        TriggerId = c.Guid(nullable: false),
                        AdapterId = c.Guid(nullable: false),
                        Order = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TriggerId, t.AdapterId });
            
            CreateTable(
                "dbo.Args",
                c => new
                    {
                        AdapterId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 128),
                        Type = c.Int(nullable: false),
                        Value = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => new { t.AdapterId, t.Name });
            
            AddColumn("dbo.Sources", "AdapterId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Assemblies", "ObjectId");
            CreateIndex("dbo.Triggers", "StreamId");
            CreateIndex("dbo.Triggers", "ObjectId");
            CreateIndex("dbo.Sources", "AdapterId");
            CreateIndex("dbo.Adapters", "Reference");
            CreateIndex("dbo.Adapters", "ObjectId");
            CreateIndex("dbo.Stmts", "AdapterId");
            CreateIndex("dbo.Stmts", "TriggerId");
            CreateIndex("dbo.Args", "AdapterId");
            AddForeignKey("dbo.Assemblies", "ObjectId", "dbo.UserDefinedObjects", "ObjectId");
            AddForeignKey("dbo.Triggers", "StreamId", "dbo.Streams", "ObjectId");
            AddForeignKey("dbo.Triggers", "ObjectId", "dbo.UserDefinedObjects", "ObjectId");
            AddForeignKey("dbo.Sources", "AdapterId", "dbo.Adapters", "ObjectId");
            AddForeignKey("dbo.Adapters", "Reference", "dbo.Assemblies", "ObjectId");
            AddForeignKey("dbo.Adapters", "ObjectId", "dbo.UserDefinedObjects", "ObjectId");
            AddForeignKey("dbo.Stmts", "TriggerId", "dbo.Triggers", "ObjectId");
            AddForeignKey("dbo.Stmts", "AdapterId", "dbo.Adapters", "ObjectId");
            AddForeignKey("dbo.Args", "AdapterId", "dbo.Adapters", "ObjectId");
        }
    }
}
