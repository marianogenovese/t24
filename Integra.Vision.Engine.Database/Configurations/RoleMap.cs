//-----------------------------------------------------------------------
// <copyright file="RoleMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models;

    /// <summary>
    /// RoleMap class
    /// </summary>
    internal sealed class RoleMap : EntityTypeConfiguration<Role>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleMap"/> class
        /// </summary>
        public RoleMap()
        {
            this.ToTable("Roles");
            this.Property(x => x.IsServerRole).IsRequired();
        }
    }
}
