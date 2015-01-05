//-----------------------------------------------------------------------
// <copyright file="SystemTriggerMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations.SystemViews
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models.SystemViews;

    /// <summary>
    /// SystemTrigger configuration class
    /// </summary>
    internal sealed class SystemTriggerMap : EntityTypeConfiguration<SystemTrigger>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemTriggerMap"/> class
        /// </summary>
        public SystemTriggerMap()
        {
            this.ToTable("System_Triggers");
            this.HasKey(x => x.Id);
        }
    }
}
