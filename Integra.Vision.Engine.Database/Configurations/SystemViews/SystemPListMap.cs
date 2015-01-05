//-----------------------------------------------------------------------
// <copyright file="SystemPListMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations.SystemViews
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models.SystemViews;

    /// <summary>
    /// SystemPList configuration class
    /// </summary>
    internal sealed class SystemPListMap : EntityTypeConfiguration<SystemPList>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemPListMap"/> class
        /// </summary>
        public SystemPListMap()
        {
            this.ToTable("System_PList");
            this.HasKey(x => new { x.Alias, x.Id });
        }
    }
}
