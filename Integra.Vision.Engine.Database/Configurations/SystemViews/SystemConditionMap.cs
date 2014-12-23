//-----------------------------------------------------------------------
// <copyright file="SystemConditionMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations.SystemViews
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models.SystemViews;

    /// <summary>
    /// SystemCondition configuration class
    /// </summary>
    internal sealed class SystemConditionMap : EntityTypeConfiguration<SystemCondition>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemConditionMap"/> class
        /// </summary>
        public SystemConditionMap()
        {
            this.ToTable("System_Conditions");
            this.HasKey(x => new { x.Id, x.Type });
        }
    }
}
