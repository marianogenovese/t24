//-----------------------------------------------------------------------
// <copyright file="SystemRoleMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations.SystemViews
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models.SystemViews;

    /// <summary>
    /// SystemRole configuration class
    /// </summary>
    internal sealed class SystemRoleMap : EntityTypeConfiguration<SystemRole>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemRoleMap"/> class
        /// </summary>
        public SystemRoleMap()
        {
            this.ToTable("System_Roles");
            this.HasKey(x => x.Id);
        }
    }
}
