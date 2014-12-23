//-----------------------------------------------------------------------
// <copyright file="PermissionUserMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models;

    /// <summary>
    /// PermissionUserMap class
    /// </summary>
    internal sealed class PermissionUserMap : EntityTypeConfiguration<PermissionUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionUserMap"/> class
        /// </summary>
        public PermissionUserMap()
        {
            this.ToTable("PermissionUsers");
            this.HasKey(x => new { x.UserId, x.UserDefinedObjectId });
            this.HasRequired(x => x.User)
                .WithMany(x => x.Permissions)
                .HasForeignKey(x => x.UserId);
        }
    }
}
