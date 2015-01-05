//-----------------------------------------------------------------------
// <copyright file="SystemUserDefinedObjectMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations.SystemViews
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models.SystemViews;

    /// <summary>
    /// SystemUserDefinedObjectMap configuration class
    /// </summary>
    internal sealed class SystemUserDefinedObjectMap : EntityTypeConfiguration<SystemUserDefinedObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemUserDefinedObjectMap"/> class
        /// </summary>
        public SystemUserDefinedObjectMap()
        {
            this.ToTable("System_UserDefinedObjects");
            this.HasKey(x => x.Id);
        }
    }
}
