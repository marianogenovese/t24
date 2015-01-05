//-----------------------------------------------------------------------
// <copyright file="SystemArgMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations.SystemViews
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models.SystemViews;

    /// <summary>
    /// SystemArgs configuration class
    /// </summary>
    internal sealed class SystemArgMap : EntityTypeConfiguration<SystemArg>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemArgMap"/> class
        /// </summary>
        public SystemArgMap()
        {
            this.ToTable("System_Args");
            this.HasKey(x => new { x.Id, x.Name });
        }
    }
}
