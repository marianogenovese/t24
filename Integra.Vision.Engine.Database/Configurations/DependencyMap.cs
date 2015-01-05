//-----------------------------------------------------------------------
// <copyright file="DependencyMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models;

    /// <summary>
    /// DependencyMap class
    /// </summary>
    internal sealed class DependencyMap : EntityTypeConfiguration<Dependency>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyMap"/> class
        /// </summary>
        public DependencyMap()
        {
            this.ToTable("Dependencies");
            this.HasKey(x => new { x.DependencyObjectId, x.PrincipalObjectId });
            this.HasRequired(x => x.PrincipalObject).WithMany(x => x.Dependencies).HasForeignKey(x => x.DependencyObjectId);
            this.HasRequired(x => x.PrincipalObject).WithMany(x => x.PrincipalObjects).HasForeignKey(x => x.PrincipalObjectId);
        }
    }
}
