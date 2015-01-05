//-----------------------------------------------------------------------
// <copyright file="SystemSourceAssignedToStreamMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations.SystemViews
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models.SystemViews;

    /// <summary>
    /// SystemSourceAssignedToStream configuration class
    /// </summary>
    internal sealed class SystemSourceAssignedToStreamMap : EntityTypeConfiguration<SystemSourceAssignedToStream>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSourceAssignedToStreamMap"/> class
        /// </summary>
        public SystemSourceAssignedToStreamMap()
        {
            this.ToTable("System_SourcesAsignedToStreams");
            this.HasKey(x => new { x.SourceId, x.StreamId, x.Alias });
        }
    }
}
