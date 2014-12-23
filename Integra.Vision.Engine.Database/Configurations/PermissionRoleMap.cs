//-----------------------------------------------------------------------
// <copyright file="PermissionRoleMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models;

    /// <summary>
    /// PermissionRoleMap class
    /// </summary>
    internal sealed class PermissionRoleMap : EntityTypeConfiguration<PermissionRole>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionRoleMap"/> class
        /// </summary>
        public PermissionRoleMap()
        {
            this.ToTable("PermissionRoles");
            this.HasKey(x => new { x.RoleId, x.UserDefinedObjectId });
            this.HasRequired(x => x.Role)
                .WithMany(x => x.Permissions)
                .HasForeignKey(x => x.RoleId);
        }
    }
}
