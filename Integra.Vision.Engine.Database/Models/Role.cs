//-----------------------------------------------------------------------
// <copyright file="Role.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Role class
    /// </summary>
    internal sealed class Role : UserDefinedObject
    {
        /// <summary>
        /// Gets or sets a value indicating whether is server role
        /// </summary>
        public bool IsServerRole { get; set; }

        /// <summary>
        /// Gets or sets the permission list
        /// </summary>
        public ICollection<PermissionRole> Permissions { get; set; }

        /// <summary>
        /// Gets or sets the role members
        /// </summary>
        public ICollection<RoleMember> Users { get; set; }
    }
}
