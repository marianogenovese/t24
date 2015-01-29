//-----------------------------------------------------------------------
// <copyright file="SourceMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models;

    /// <summary>
    /// SourceMap class
    /// </summary>
    internal sealed class SourceMap : EntityTypeConfiguration<Source>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceMap"/> class
        /// </summary>
        public SourceMap()
        {
            this.ToTable("Sources");
        }
    }
}
