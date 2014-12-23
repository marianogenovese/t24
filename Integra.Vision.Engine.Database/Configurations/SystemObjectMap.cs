//-----------------------------------------------------------------------
// <copyright file="SystemObjectMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models;

    /// <summary>
    /// SystemObjectMap class
    /// </summary>
    internal sealed class SystemObjectMap : EntityTypeConfiguration<Integra.Vision.Engine.Database.Models.SystemObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemObjectMap"/> class
        /// </summary>
        public SystemObjectMap()
        {
            this.ToTable("Objects");
            this.HasKey(x => x.Id);
            this.Property(x => x.Id)
                .HasColumnName("ObjectId")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            this.Property(x => x.CreationDate).IsRequired();
            this.Property(x => x.Type).IsRequired();
        }
    }
}
