//-----------------------------------------------------------------------
// <copyright file="SystemViewsContext.cs" company="Integra.Vision.Engine.Database">
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
    /// System views context
    /// </summary>
    internal sealed class SystemViewsContext : DbContext, System.IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemViewsContext"/> class
        /// </summary>
        public SystemViewsContext() : base("EngineDatabase")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemViewsContext"/> class
        /// </summary>
        /// <param name="cadenaDeConexion">string connection</param>
        public SystemViewsContext(string cadenaDeConexion)
            : base(cadenaDeConexion)
        {
        }
                
        /// <summary>
        /// Gets or sets the SystemAdapter view
        /// </summary>
        public DbSet<SystemAdapter> SystemAdapter { get; set; }

        /// <summary>
        /// Gets or sets the SystemArgument view
        /// </summary>
        public DbSet<SystemArg> SystemArg { get; set; }

        /// <summary>
        /// Gets or sets the SystemAssembly view
        /// </summary>
        public DbSet<SystemAssembly> SystemAssembly { get; set; }

        /// <summary>
        /// Gets or sets the SystemCondition view
        /// </summary>
        public DbSet<SystemCondition> SystemCondition { get; set; }

        /// <summary>
        /// Gets or sets the SystemDependency view
        /// </summary>
        public DbSet<SystemDependency> SystemDependency { get; set; }

        /// <summary>
        /// Gets or sets the SystemPermission view
        /// </summary>
        public DbSet<SystemPermission> SystemPermission { get; set; }

        /// <summary>
        /// Gets or sets the SystemPList view
        /// </summary>
        public DbSet<SystemPList> SystemPList { get; set; }

        /// <summary>
        /// Gets or sets the SystemRole view
        /// </summary>
        public DbSet<SystemRole> SystemRole { get; set; }

        /// <summary>
        /// Gets or sets the SystemRoleMember view
        /// </summary>
        public DbSet<SystemRoleMember> SystemRoleMember { get; set; }

        /// <summary>
        /// Gets or sets the SystemSetTrace view
        /// </summary>
        public DbSet<SystemSetTrace> SystemSetTrace { get; set; }

        /// <summary>
        /// Gets or sets the SystemSourceAssignedToStream view
        /// </summary>
        public DbSet<SystemSourceAssignedToStream> SystemSourceAssignedToStream { get; set; }

        /// <summary>
        /// Gets or sets the SystemSource view
        /// </summary>
        public DbSet<SystemSource> SystemSource { get; set; }

        /// <summary>
        /// Gets or sets the SystemStatement view
        /// </summary>
        public DbSet<SystemStmt> SystemStmt { get; set; }

        /// <summary>
        /// Gets or sets the SystemStream view
        /// </summary>
        public DbSet<SystemStream> SystemStream { get; set; }

        /// <summary>
        /// Gets or sets the SystemTrigger view
        /// </summary>
        public DbSet<SystemTrigger> SystemTrigger { get; set; }

        /// <summary>
        /// Gets or sets the SystemUserDefinedObject view
        /// </summary>
        public DbSet<SystemUserDefinedObject> SystemUserDefinedObject { get; set; }

        /// <summary>
        /// Gets or sets the SystemUser view
        /// </summary>
        public DbSet<SystemUser> SystemUser { get; set; }

        /// <summary>
        /// Model create event
        /// </summary>
        /// <param name="modelBuilder">model builder</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // System views
            modelBuilder.Configurations.Add(new SystemUserDefinedObjectMap());
            modelBuilder.Configurations.Add(new SystemAdapterMap());
            modelBuilder.Configurations.Add(new SystemArgMap());
            modelBuilder.Configurations.Add(new SystemSourceMap());
            modelBuilder.Configurations.Add(new SystemStreamMap());
            modelBuilder.Configurations.Add(new SystemConditionMap());
            modelBuilder.Configurations.Add(new SystemPListMap());
            modelBuilder.Configurations.Add(new SystemTriggerMap());
            modelBuilder.Configurations.Add(new SystemStmtMap());
            modelBuilder.Configurations.Add(new SystemUserMap());
            modelBuilder.Configurations.Add(new SystemRoleMap());
            modelBuilder.Configurations.Add(new SystemPermissionMap());
            modelBuilder.Configurations.Add(new SystemRoleMemberMap());
            modelBuilder.Configurations.Add(new SystemSetTraceMap());
            modelBuilder.Configurations.Add(new SystemAssemblyMap());
            modelBuilder.Configurations.Add(new SystemDependencyMap());
            modelBuilder.Configurations.Add(new SystemSourceAssignedToStreamMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
