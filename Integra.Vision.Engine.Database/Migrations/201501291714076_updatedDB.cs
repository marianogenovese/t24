namespace Integra.Vision.Engine.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PermissionRoles",
                c => new
                    {
                        RoleId = c.Guid(nullable: false),
                        UserDefinedObjectId = c.Guid(nullable: false),
                        Type = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserDefinedObjectId })
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .ForeignKey("dbo.UserDefinedObjects", t => t.UserDefinedObjectId)
                .Index(t => t.RoleId)
                .Index(t => t.UserDefinedObjectId);
            
            CreateTable(
                "dbo.BaseObjects",
                c => new
                    {
                        ObjectId = c.Guid(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                        Type = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ObjectId);
            
            CreateTable(
                "dbo.Dependencies",
                c => new
                    {
                        DependencyObjectId = c.Guid(nullable: false),
                        PrincipalObjectId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.DependencyObjectId, t.PrincipalObjectId })
                .ForeignKey("dbo.UserDefinedObjects", t => t.DependencyObjectId)
                .ForeignKey("dbo.UserDefinedObjects", t => t.PrincipalObjectId)
                .Index(t => t.DependencyObjectId)
                .Index(t => t.PrincipalObjectId);
            
            CreateTable(
                "dbo.Scripts",
                c => new
                    {
                        ScriptId = c.Guid(nullable: false, identity: true),
                        ScriptText = c.String(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                        UserDefinedObjectId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ScriptId)
                .ForeignKey("dbo.UserDefinedObjects", t => t.UserDefinedObjectId)
                .Index(t => t.UserDefinedObjectId);
            
            CreateTable(
                "dbo.SetTraces",
                c => new
                    {
                        UserDefinedObjectId = c.Guid(nullable: false),
                        Level = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserDefinedObjectId)
                .ForeignKey("dbo.UserDefinedObjects", t => t.UserDefinedObjectId)
                .Index(t => t.UserDefinedObjectId);
            
            CreateTable(
                "dbo.SourcesAsignedToStreams",
                c => new
                    {
                        StreamId = c.Guid(nullable: false),
                        SourceId = c.Guid(nullable: false),
                        Alias = c.String(nullable: false, maxLength: 128),
                        IsWithSource = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.StreamId, t.SourceId, t.Alias })
                .ForeignKey("dbo.Sources", t => t.SourceId)
                .ForeignKey("dbo.Streams", t => t.StreamId)
                .Index(t => t.StreamId)
                .Index(t => t.SourceId);
            
            CreateTable(
                "dbo.StreamConditions",
                c => new
                    {
                        StreamId = c.Guid(nullable: false),
                        Type = c.Int(nullable: false),
                        Expression = c.String(nullable: false),
                        IsOnCondition = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.StreamId, t.Type })
                .ForeignKey("dbo.Streams", t => t.StreamId)
                .Index(t => t.StreamId);
            
            CreateTable(
                "dbo.PLists",
                c => new
                    {
                        Alias = c.String(nullable: false, maxLength: 128),
                        StreamId = c.Guid(nullable: false),
                        Expression = c.String(nullable: false),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Alias, t.StreamId })
                .ForeignKey("dbo.Streams", t => t.StreamId)
                .Index(t => t.StreamId);
            
            CreateTable(
                "dbo.PermissionUsers",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        UserDefinedObjectId = c.Guid(nullable: false),
                        Type = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.UserDefinedObjectId })
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.UserDefinedObjects", t => t.UserDefinedObjectId)
                .Index(t => t.UserId)
                .Index(t => t.UserDefinedObjectId);
            
            CreateTable(
                "dbo.RoleMembers",
                c => new
                    {
                        RoleId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserDefinedObjects",
                c => new
                    {
                        ObjectId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 60),
                        State = c.Int(nullable: false),
                        IsSystemObject = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ObjectId)
                .ForeignKey("dbo.BaseObjects", t => t.ObjectId)
                .Index(t => t.ObjectId)
                .Index(t => t.Name, unique: true, name: "IX_UniqueName");
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        ObjectId = c.Guid(nullable: false),
                        IsServerRole = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ObjectId)
                .ForeignKey("dbo.UserDefinedObjects", t => t.ObjectId)
                .Index(t => t.ObjectId);
            
            CreateTable(
                "dbo.Sources",
                c => new
                    {
                        ObjectId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ObjectId)
                .ForeignKey("dbo.UserDefinedObjects", t => t.ObjectId)
                .Index(t => t.ObjectId);
            
            CreateTable(
                "dbo.Streams",
                c => new
                    {
                        ObjectId = c.Guid(nullable: false),
                        DurationTime = c.Double(),
                        UseJoin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ObjectId)
                .ForeignKey("dbo.UserDefinedObjects", t => t.ObjectId)
                .Index(t => t.ObjectId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ObjectId = c.Guid(nullable: false),
                        SId = c.String(nullable: false),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.ObjectId)
                .ForeignKey("dbo.UserDefinedObjects", t => t.ObjectId)
                .Index(t => t.ObjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "ObjectId", "dbo.UserDefinedObjects");
            DropForeignKey("dbo.Streams", "ObjectId", "dbo.UserDefinedObjects");
            DropForeignKey("dbo.Sources", "ObjectId", "dbo.UserDefinedObjects");
            DropForeignKey("dbo.Roles", "ObjectId", "dbo.UserDefinedObjects");
            DropForeignKey("dbo.UserDefinedObjects", "ObjectId", "dbo.BaseObjects");
            DropForeignKey("dbo.PermissionRoles", "UserDefinedObjectId", "dbo.UserDefinedObjects");
            DropForeignKey("dbo.PermissionRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Dependencies", "PrincipalObjectId", "dbo.UserDefinedObjects");
            DropForeignKey("dbo.RoleMembers", "UserId", "dbo.Users");
            DropForeignKey("dbo.RoleMembers", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.PermissionUsers", "UserDefinedObjectId", "dbo.UserDefinedObjects");
            DropForeignKey("dbo.PermissionUsers", "UserId", "dbo.Users");
            DropForeignKey("dbo.SourcesAsignedToStreams", "StreamId", "dbo.Streams");
            DropForeignKey("dbo.PLists", "StreamId", "dbo.Streams");
            DropForeignKey("dbo.StreamConditions", "StreamId", "dbo.Streams");
            DropForeignKey("dbo.SourcesAsignedToStreams", "SourceId", "dbo.Sources");
            DropForeignKey("dbo.SetTraces", "UserDefinedObjectId", "dbo.UserDefinedObjects");
            DropForeignKey("dbo.Scripts", "UserDefinedObjectId", "dbo.UserDefinedObjects");
            DropForeignKey("dbo.Dependencies", "DependencyObjectId", "dbo.UserDefinedObjects");
            DropIndex("dbo.Users", new[] { "ObjectId" });
            DropIndex("dbo.Streams", new[] { "ObjectId" });
            DropIndex("dbo.Sources", new[] { "ObjectId" });
            DropIndex("dbo.Roles", new[] { "ObjectId" });
            DropIndex("dbo.UserDefinedObjects", "IX_UniqueName");
            DropIndex("dbo.UserDefinedObjects", new[] { "ObjectId" });
            DropIndex("dbo.RoleMembers", new[] { "UserId" });
            DropIndex("dbo.RoleMembers", new[] { "RoleId" });
            DropIndex("dbo.PermissionUsers", new[] { "UserDefinedObjectId" });
            DropIndex("dbo.PermissionUsers", new[] { "UserId" });
            DropIndex("dbo.PLists", new[] { "StreamId" });
            DropIndex("dbo.StreamConditions", new[] { "StreamId" });
            DropIndex("dbo.SourcesAsignedToStreams", new[] { "SourceId" });
            DropIndex("dbo.SourcesAsignedToStreams", new[] { "StreamId" });
            DropIndex("dbo.SetTraces", new[] { "UserDefinedObjectId" });
            DropIndex("dbo.Scripts", new[] { "UserDefinedObjectId" });
            DropIndex("dbo.Dependencies", new[] { "PrincipalObjectId" });
            DropIndex("dbo.Dependencies", new[] { "DependencyObjectId" });
            DropIndex("dbo.PermissionRoles", new[] { "UserDefinedObjectId" });
            DropIndex("dbo.PermissionRoles", new[] { "RoleId" });
            DropTable("dbo.Users");
            DropTable("dbo.Streams");
            DropTable("dbo.Sources");
            DropTable("dbo.Roles");
            DropTable("dbo.UserDefinedObjects");
            DropTable("dbo.RoleMembers");
            DropTable("dbo.PermissionUsers");
            DropTable("dbo.PLists");
            DropTable("dbo.StreamConditions");
            DropTable("dbo.SourcesAsignedToStreams");
            DropTable("dbo.SetTraces");
            DropTable("dbo.Scripts");
            DropTable("dbo.Dependencies");
            DropTable("dbo.BaseObjects");
            DropTable("dbo.PermissionRoles");
        }
    }
}