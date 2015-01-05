//-----------------------------------------------------------------------
// <copyright file="SystemDependencyMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations.SystemViews
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models.SystemViews;

    /// <summary>
    /// SystemDependency configuration class
    /// </summary>
    internal sealed class SystemDependencyMap : EntityTypeConfiguration<SystemDependency>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemDependencyMap"/> class
        /// </summary>
        public SystemDependencyMap()
        {
            this.ToTable("System_Dependencies");
            this.HasKey(x => new { x.DependenceId, x.Id });
        }
    }
}
