//-----------------------------------------------------------------------
// <copyright file="SystemPermissionMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations.SystemViews
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models.SystemViews;

    /// <summary>
    /// SystemPermission configuration class
    /// </summary>
    internal sealed class SystemPermissionMap : EntityTypeConfiguration<SystemPermission>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemPermissionMap"/> class
        /// </summary>
        public SystemPermissionMap()
        {
            this.ToTable("System_Permissions");
            this.HasKey(x => new { x.Sid, x.Oid });
        }
    }
}
