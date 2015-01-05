//-----------------------------------------------------------------------
// <copyright file="AssemblyMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models;

    /// <summary>
    /// AssemblyMap class
    /// </summary>
    internal sealed class AssemblyMap : EntityTypeConfiguration<Assembly>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyMap"/> class
        /// </summary>
        public AssemblyMap()
        {
            this.ToTable("Assemblies");
            this.Property(x => x.LocalPath).IsRequired();
        }
    }
}
