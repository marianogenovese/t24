//-----------------------------------------------------------------------
// <copyright file="UserDefinedObjectMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models;

    /// <summary>
    /// Base object configuration class
    /// </summary>
    internal sealed class UserDefinedObjectMap : EntityTypeConfiguration<UserDefinedObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDefinedObjectMap"/> class
        /// </summary>
        public UserDefinedObjectMap()
        {
            this.ToTable("UserDefinedObjects");
            this.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_UniqueName") { IsUnique = true }));
            this.Property(x => x.State).IsRequired();
            this.Property(x => x.IsSystemObject).IsRequired();
        }
    }
}
