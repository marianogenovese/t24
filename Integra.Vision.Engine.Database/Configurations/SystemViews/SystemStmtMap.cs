//-----------------------------------------------------------------------
// <copyright file="SystemStmtMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations.SystemViews
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models.SystemViews;

    /// <summary>
    /// SystemStatementMap configuration class
    /// </summary>
    internal sealed class SystemStmtMap : EntityTypeConfiguration<SystemStmt>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemStmtMap"/> class
        /// </summary>
        public SystemStmtMap()
        {
            this.ToTable("System_Stmts");
            this.HasKey(x => new { x.Id, x.AdapterId });
        }
    }
}
