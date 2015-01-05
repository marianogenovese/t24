//-----------------------------------------------------------------------
// <copyright file="TriggerMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models;

    /// <summary>
    /// TriggerMap class
    /// </summary>
    internal sealed class TriggerMap : EntityTypeConfiguration<Trigger>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerMap"/> class
        /// </summary>
        public TriggerMap()
        {
            this.ToTable("Triggers");
            this.Property(x => x.DurationTime).IsOptional();
            this.HasRequired(x => x.Stream)
                .WithMany(x => x.Triggers)
                .HasForeignKey(x => x.StreamId);
        }
    }
}
