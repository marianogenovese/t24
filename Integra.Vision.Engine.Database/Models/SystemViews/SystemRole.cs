//-----------------------------------------------------------------------
// <copyright file="SystemRole.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models.SystemViews
{
    /// <summary>
    /// SystemRole class
    /// </summary>
    internal sealed class SystemRole
    {
        /// <summary>
        /// Gets or sets the role id
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is server role
        /// </summary>
        public bool IsServerRole { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the role
        /// </summary>
        public System.DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the role name
        /// </summary>
        public string Name { get; set; }
    }
}
