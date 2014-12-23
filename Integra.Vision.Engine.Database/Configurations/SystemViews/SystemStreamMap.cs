//-----------------------------------------------------------------------
// <copyright file="SystemStreamMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations.SystemViews
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models.SystemViews;

    /// <summary>
    /// SystemStream configuration class
    /// </summary>
    internal sealed class SystemStreamMap : EntityTypeConfiguration<SystemStream>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemStreamMap"/> class
        /// </summary>
        public SystemStreamMap()
        {
            this.ToTable("System_Streams");
            this.HasKey(x => x.Id);
        }
    }
}
