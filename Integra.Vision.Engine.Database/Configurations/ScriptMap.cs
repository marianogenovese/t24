//-----------------------------------------------------------------------
// <copyright file="ScriptMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models;

    /// <summary>
    /// Script configuration class.
    /// </summary>
    internal sealed class ScriptMap : EntityTypeConfiguration<Script>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptMap"/> class
        /// </summary>
        public ScriptMap()
        {
            this.ToTable("Scripts");
            this.HasKey(x => x.Id);
            this.Property(x => x.Id)
                .HasColumnName("ScriptId")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            this.Property(x => x.ScriptText).IsRequired();
            this.Property(x => x.LastUpdate).IsRequired();
            this.HasRequired(x => x.UserDefinedObject)
                .WithMany(x => x.Scripts)
                .HasForeignKey(x => x.UserDefinedObjectId);
        }
    }
}
