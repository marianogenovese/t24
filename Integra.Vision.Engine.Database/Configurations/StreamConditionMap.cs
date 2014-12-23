//-----------------------------------------------------------------------
// <copyright file="StreamConditionMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models;

    /// <summary>
    /// StreamConditionMap class
    /// </summary>
    internal sealed class StreamConditionMap : EntityTypeConfiguration<StreamCondition>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamConditionMap"/> class
        /// </summary>
        public StreamConditionMap()
        {
            this.ToTable("StreamConditions");
            this.HasKey(x => new { x.StreamId, x.Type });
            this.Property(x => x.Expression).IsRequired();
            this.Property(x => x.Type).IsRequired();
            HasRequired(x => x.Stream)
                .WithMany(x => x.Conditions)
                .HasForeignKey(x => x.StreamId);
        }
    }
}
