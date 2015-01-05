//-----------------------------------------------------------------------
// <copyright file="RoleMemberMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models;

    /// <summary>
    /// RoleMemberMap class
    /// </summary>
    internal sealed class RoleMemberMap : EntityTypeConfiguration<RoleMember>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleMemberMap"/> class
        /// </summary>
        public RoleMemberMap()
        {
            this.ToTable("RoleMembers");
            this.HasKey(x => new { x.RoleId, x.UserId });
            this.HasRequired(x => x.User)
                .WithMany(x => x.Roles)
                .HasForeignKey(x => x.UserId);
            this.HasRequired(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId);
        }
    }
}
