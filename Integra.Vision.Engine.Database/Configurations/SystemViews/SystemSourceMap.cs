//-----------------------------------------------------------------------
// <copyright file="SystemSourceMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations.SystemViews
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models.SystemViews;

    /// <summary>
    /// SystemSource configuration class
    /// </summary>
    internal sealed class SystemSourceMap : EntityTypeConfiguration<SystemSource>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSourceMap"/> class
        /// </summary>
        public SystemSourceMap()
        {
            this.ToTable("System_Sources");
            this.HasKey(x => x.Id);
        }
    }
}
