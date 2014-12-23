//-----------------------------------------------------------------------
// <copyright file="RoleMember.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models
{
    /// <summary>
    /// RoleMember class
    /// </summary>
    internal sealed class RoleMember
    {
        /// <summary>
        /// Gets or sets the role
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// Gets or sets the role id
        /// </summary>
        public System.Guid RoleId { get; set; }

        /// <summary>
        /// Gets or sets the user
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        public System.Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the assignation date
        /// </summary>
        public System.DateTime CreationDate { get; set; }
    }
}
