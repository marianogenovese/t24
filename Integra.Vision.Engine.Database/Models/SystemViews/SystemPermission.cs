//-----------------------------------------------------------------------
// <copyright file="SystemPermission.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models.SystemViews
{
    /// <summary>
    /// SystemPermission class
    /// </summary>
    internal sealed class SystemPermission
    {
        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        public System.Guid Sid { get; set; }

        /// <summary>
        /// Gets or sets the object id
        /// </summary>
        public System.Guid Oid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is denied or granted
        /// </summary>
        public bool Type { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the permission
        /// </summary>
        public System.DateTime CreationDate { get; set; }
    }
}
