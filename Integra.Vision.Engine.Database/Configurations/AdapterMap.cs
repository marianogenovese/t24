//-----------------------------------------------------------------------
// <copyright file="AdapterMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models;

    /// <summary>
    /// Adapter configuration class
    /// </summary>
    internal sealed class AdapterMap : EntityTypeConfiguration<Adapter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdapterMap"/> class
        /// </summary>
        public AdapterMap()
        {
            this.ToTable("Adapters");
            this.Property(x => x.AdapterType).IsRequired();
            this.Property(x => x.AssemblyId).IsRequired().HasColumnName("Reference");
            this.HasRequired(x => x.Assembly)
                .WithMany(x => x.Adapters)
                .HasForeignKey(x => x.AssemblyId);
        }
    }
}
