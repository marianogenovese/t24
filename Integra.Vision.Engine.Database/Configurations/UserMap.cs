//-----------------------------------------------------------------------
// <copyright file="UserMap.cs" company="Integra.Vision.Engine.Languaje">
//     Copyright (c) Integra.Vision.Engine.Languaje. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using Integra.Vision.Engine.Database.Models;

    /// <summary>
    /// UserMap class
    /// </summary>
    internal sealed class UserMap : EntityTypeConfiguration<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserMap"/> class
        /// </summary>
        public UserMap()
        {
            this.ToTable("Users");
            this.Property(x => x.SId).IsRequired();
        }
    }
}
