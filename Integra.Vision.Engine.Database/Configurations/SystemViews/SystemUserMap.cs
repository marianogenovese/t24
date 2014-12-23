//-----------------------------------------------------------------------
// <copyright file="SystemUserMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations.SystemViews
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models.SystemViews;

    /// <summary>
    /// SystemUserDefinedObjectMap configuration class
    /// </summary>
    internal sealed class SystemUserMap : EntityTypeConfiguration<SystemUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemUserMap"/> class
        /// </summary>
        public SystemUserMap()
        {
            this.ToTable("System_Users");
            this.HasKey(x => x.Id);
        }
    }
}
