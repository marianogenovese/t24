//-----------------------------------------------------------------------
// <copyright file="SystemRoleMemberMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations.SystemViews
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models.SystemViews;

    /// <summary>
    /// SystemRoleMember configuration class
    /// </summary>
    internal sealed class SystemRoleMemberMap : EntityTypeConfiguration<SystemRoleMember>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemRoleMemberMap"/> class
        /// </summary>
        public SystemRoleMemberMap()
        {
            this.ToTable("System_RoleMembers");
            this.HasKey(x => new { x.Id, x.Uid });
        }
    }
}
