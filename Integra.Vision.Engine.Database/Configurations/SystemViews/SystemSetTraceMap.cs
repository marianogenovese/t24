//-----------------------------------------------------------------------
// <copyright file="SystemSetTraceMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations.SystemViews
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models.SystemViews;

    /// <summary>
    /// SystemSetTrace configuration class
    /// </summary>
    internal sealed class SystemSetTraceMap : EntityTypeConfiguration<SystemSetTrace>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSetTraceMap"/> class
        /// </summary>
        public SystemSetTraceMap()
        {
            this.ToTable("System_SetTrace");
            this.HasKey(x => x.Id);
        }
    }
}
