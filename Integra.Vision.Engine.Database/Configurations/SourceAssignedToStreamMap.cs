//-----------------------------------------------------------------------
// <copyright file="SourceAssignedToStreamMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models;

    /// <summary>
    /// SourceAssignedToStreamMap class
    /// </summary>
    internal sealed class SourceAssignedToStreamMap : EntityTypeConfiguration<SourceAssignedToStream>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceAssignedToStreamMap"/> class
        /// </summary>
        public SourceAssignedToStreamMap() 
        {
            this.ToTable("SourcesAsignedToStreams");
            this.HasKey(x => new { x.StreamId, x.SourceId, x.Alias });
            this.Property(x => x.IsWithSource).IsRequired();
            this.HasRequired(x => x.Source)
                .WithMany(x => x.AsignedStreams)
                .HasForeignKey(x => x.SourceId);
            this.HasRequired(x => x.Stream)
                .WithMany(x => x.AsignedSources)
                .HasForeignKey(x => x.StreamId);
        }
    }
}
