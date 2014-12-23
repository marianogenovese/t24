//-----------------------------------------------------------------------
// <copyright file="StreamMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models;

    /// <summary>
    /// StreamMap class
    /// </summary>
    internal sealed class StreamMap : EntityTypeConfiguration<Stream>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamMap"/> class
        /// </summary>
        public StreamMap()
        {
            this.ToTable("Streams");
            this.Property(x => x.DurationTime).IsOptional();
            this.Property(x => x.UseJoin).IsRequired();
        }
    }
}
