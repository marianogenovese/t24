//-----------------------------------------------------------------------
// <copyright file="SourceConditionMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models;

    /// <summary>
    /// SourceConditionMap class
    /// </summary>
    internal sealed class SourceConditionMap : EntityTypeConfiguration<SourceCondition>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceConditionMap"/> class
        /// </summary>
        public SourceConditionMap()
        {
            this.ToTable("SourceConditions");
            this.HasKey(x => new { x.SourceId, x.Type });
            this.Property(x => x.Expression).IsRequired();
            this.Property(x => x.Type).IsRequired();
            HasRequired(x => x.Source)
                .WithMany(x => x.Conditions)
                .HasForeignKey(x => x.SourceId);
        }
    }
}
