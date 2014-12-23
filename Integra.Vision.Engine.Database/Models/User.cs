//-----------------------------------------------------------------------
// <copyright file="User.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// User class
    /// </summary>
    internal sealed class User : UserDefinedObject
    {
        /// <summary>
        /// Gets or sets the security id
        /// </summary>
        public string SId { get; set; }

        /// <summary>
        /// Gets or sets the permission list
        /// </summary>
        public ICollection<PermissionUser> Permissions { get; set; }

        /// <summary>
        /// Gets or sets the role members
        /// </summary>
        public ICollection<RoleMember> Roles { get; set; }

        /// <summary>
        /// Gets or sets the user password
        /// </summary>
        public string Password { get; set; }
    }
}
