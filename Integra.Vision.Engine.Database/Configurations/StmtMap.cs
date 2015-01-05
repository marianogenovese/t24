//-----------------------------------------------------------------------
// <copyright file="StmtMap.cs" company="CompanyName">
//     Copyright (c) CompanyName. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models;

    /// <summary>
    /// Statement Map class
    /// </summary>
    internal sealed class StmtMap : EntityTypeConfiguration<Stmt>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StmtMap"/> class
        /// </summary>
        public StmtMap()
        {
            this.ToTable("Stmts");
            this.HasKey(x => new { x.TriggerId, x.AdapterId });
            this.Property(x => x.Type).IsRequired();
            this.Property(x => x.Order).IsRequired();
            this.HasRequired(x => x.Trigger)
                .WithMany(x => x.Stmts)
                .HasForeignKey(x => x.TriggerId);
            this.HasRequired(x => x.Adapter)
                .WithMany(x => x.Stmts)
                .HasForeignKey(x => x.AdapterId);
        }
    }
}
