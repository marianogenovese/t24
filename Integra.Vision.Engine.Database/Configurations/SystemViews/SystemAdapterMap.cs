//-----------------------------------------------------------------------
// <copyright file="SystemAdapterMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations.SystemViews
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models.SystemViews;

    /// <summary>
    /// SystemAdapters configuration class
    /// </summary>
    internal sealed class SystemAdapterMap : EntityTypeConfiguration<SystemAdapter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemAdapterMap"/> class
        /// </summary>
        public SystemAdapterMap()
        {
            this.ToTable("System_Adapters");
            this.HasKey(x => x.Id);
        }
    }
}
