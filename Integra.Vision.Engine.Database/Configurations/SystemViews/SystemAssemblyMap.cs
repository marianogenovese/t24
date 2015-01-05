//-----------------------------------------------------------------------
// <copyright file="SystemAssemblyMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations.SystemViews
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models.SystemViews;

    /// <summary>
    /// SystemAssembly configuration class
    /// </summary>
    internal sealed class SystemAssemblyMap : EntityTypeConfiguration<SystemAssembly>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemAssemblyMap"/> class
        /// </summary>
        public SystemAssemblyMap()
        {
            this.ToTable("System_Assemblies");
            this.HasKey(x => x.Id);
        }
    }
}
