//-----------------------------------------------------------------------
// <copyright file="ArgMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models;

    /// <summary>
    /// Argument Map class
    /// </summary>
    internal sealed class ArgMap : EntityTypeConfiguration<Arg>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgMap"/> class
        /// </summary>
        public ArgMap()
        {
            this.ToTable("Args");
            this.HasKey(x => new { x.AdapterId, x.Name });
            this.Property(x => x.Type).IsRequired();
            this.Property(x => x.Name).IsRequired();
            this.Property(x => x.Value).IsRequired();
            this.Property(x => x.AdapterId).HasColumnName("AdapterId");
            this.HasRequired(x => x.Adapter)
                .WithMany(x => x.Args)
                .HasForeignKey(x => x.AdapterId);
        }
    }
}
