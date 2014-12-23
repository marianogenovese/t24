//-----------------------------------------------------------------------
// <copyright file="ViewsContext.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Contexts
{
    using System.Data.Entity;
    using Integra.Vision.Engine.Database.Configurations;
    using Integra.Vision.Engine.Database.Configurations.SystemViews;
    using Integra.Vision.Engine.Database.Initializers;
    using Integra.Vision.Engine.Database.Models;
    using Integra.Vision.Engine.Database.Models.SystemViews;

    /// <summary>
    /// System tables context
    /// </summary>
    internal sealed class ViewsContext : DbContext, System.IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewsContext"/> class
        /// </summary>
        public ViewsContext() : base("EngineDatabase")
        {
            Database.SetInitializer<ViewsContext>(new EQLViewsInitializer());
            
            // Database.SetInitializer<ViewsContext>(new DropCreateDatabaseAlways<ViewsContext>());
            // Database.SetInitializer<ViewsContext>(new DropCreateDatabaseIfModelChanges<ViewsContext>());
            // Database.SetInitializer<ViewsContext>(new CreateDatabaseIfNotExists<ViewsContext>());
            // Database.SetInitializer<ViewsContext>(new EQLDatabaseInitializer());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewsContext"/> class
        /// </summary>
        /// <param name="cadenaDeConexion">string connection</param>
        public ViewsContext(string cadenaDeConexion) : base(cadenaDeConexion)
        {
            Database.SetInitializer<ViewsContext>(new EQLViewsInitializer());
        }

        /// <summary>
        /// Gets or sets the user defined object
        /// </summary>
        public DbSet<UserDefinedObject> UserDefinedObject { get; set; }

        /// <summary>
        /// Gets or sets the adapter
        /// </summary>
        public DbSet<Adapter> Adapter { get; set; }

        /// <summary>
        /// Gets or sets the arguments
        /// </summary>
        public DbSet<Arg> Arg { get; set; }

        /// <summary>
        /// Gets or sets the source
        /// </summary>
        public DbSet<Source> Source { get; set; }

        /// <summary>
        /// Gets or sets the stream
        /// </summary>
        public DbSet<Stream> Stream { get; set; }

        /// <summary>
        /// Gets or sets the Stream Condition
        /// </summary>
        public DbSet<StreamCondition> StreamCondition { get; set; }

        /// <summary>
        /// Gets or sets the Source Condition
        /// </summary>
        public DbSet<SourceCondition> SourceCondition { get; set; }

        /// <summary>
        /// Gets or sets the projection list
        /// </summary>
        public DbSet<PList> PList { get; set; }

        /// <summary>
        /// Gets or sets the trigger
        /// </summary>
        public DbSet<Trigger> Trigger { get; set; }

        /// <summary>
        /// Gets or sets the statement
        /// </summary>
        public DbSet<Stmt> Stmt { get; set; }

        /// <summary>
        /// Gets or sets the user
        /// </summary>
        public DbSet<User> User { get; set; }

        /// <summary>
        /// Gets or sets the role
        /// </summary>
        public DbSet<Role> Role { get; set; }

        /// <summary>
        /// Gets or sets the role member
        /// </summary>
        public DbSet<RoleMember> RoleMember { get; set; }

        /// <summary>
        /// Gets or sets the permission role
        /// </summary>
        public DbSet<PermissionRole> PermissionRole { get; set; }

        /// <summary>
        /// Gets or sets the permission user
        /// </summary>
        public DbSet<PermissionUser> PermissionUser { get; set; }
                
        /// <summary>
        /// Model create event
        /// </summary>
        /// <param name="modelBuilder">model builder</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Tables
            modelBuilder.Configurations.Add(new SystemObjectMap());
            modelBuilder.Configurations.Add(new UserDefinedObjectMap());
            modelBuilder.Configurations.Add(new AdapterMap());
            modelBuilder.Configurations.Add(new ArgMap());
            modelBuilder.Configurations.Add(new SourceMap());
            modelBuilder.Configurations.Add(new StreamMap());
            modelBuilder.Configurations.Add(new StreamConditionMap());
            modelBuilder.Configurations.Add(new SourceConditionMap());
            modelBuilder.Configurations.Add(new PListMap());
            modelBuilder.Configurations.Add(new TriggerMap());
            modelBuilder.Configurations.Add(new StmtMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new PermissionUserMap());
            modelBuilder.Configurations.Add(new PermissionRoleMap());
            modelBuilder.Configurations.Add(new RoleMemberMap());
            modelBuilder.Configurations.Add(new SetTraceMap());
            modelBuilder.Configurations.Add(new AssemblyMap());
            modelBuilder.Configurations.Add(new DependencyMap());
            modelBuilder.Configurations.Add(new SourceAssignedToStreamMap());
                        
            base.OnModelCreating(modelBuilder);
        }
    }
}
