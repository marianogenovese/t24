//-----------------------------------------------------------------------
// <copyright file="PListMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models;

    /// <summary>
    /// PListMap class
    /// </summary>
    internal sealed class PListMap : EntityTypeConfiguration<PList>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PListMap"/> class
        /// </summary>
        public PListMap()
        {
            this.ToTable("PLists");
            this.HasKey(x => new { x.Alias, x.StreamId });
            this.Property(x => x.Expression).IsRequired();
            this.Property(x => x.Order).IsRequired();
            this.Property(x => x.Alias).IsRequired();
            HasRequired(x => x.Stream)
                .WithMany(x => x.PList)
                .HasForeignKey(x => x.StreamId);
        }
    }
}
