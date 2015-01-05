//-----------------------------------------------------------------------
// <copyright file="PermissionUser.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models
{
    /// <summary>
    /// PermissionUser class
    /// </summary>
    internal sealed class PermissionUser
    {
        /// <summary>
        /// Gets or sets a value indicating whether is denied or granted
        /// </summary>
        public bool Type { get; set; }

        /// <summary>
        /// Gets or sets the role 
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the role id
        /// </summary>
        public System.Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the user defined object
        /// </summary>
        public UserDefinedObject UserDefinedObject { get; set; }

        /// <summary>
        /// Gets or sets the user defined object id
        /// </summary>
        public System.Guid UserDefinedObjectId { get; set; }

        /// <summary>
        /// Gets or sets the permission creation date
        /// </summary>
        public System.DateTime CreationDate { get; set; }
    }
}
