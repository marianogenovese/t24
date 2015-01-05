//-----------------------------------------------------------------------
// <copyright file="SetTraceMap.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models;

    /// <summary>
    /// SetTraceMap class
    /// </summary>
    internal sealed class SetTraceMap : EntityTypeConfiguration<SetTrace>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetTraceMap"/> class
        /// </summary>
        public SetTraceMap()
        {
            this.ToTable("SetTraces");
            this.HasKey(x => x.UserDefinedObjectId);
            this.HasRequired(x => x.UserDefinedObject).WithRequiredDependent(x => x.SetTrace);
            this.Property(x => x.Level).IsRequired();
        }
    }
}
