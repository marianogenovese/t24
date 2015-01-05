//-----------------------------------------------------------------------
// <copyright file="SystemUser.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models.SystemViews
{
    /// <summary>
    /// SystemUser class
    /// </summary>
    internal sealed class SystemUser
    {
        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the security id
        /// </summary>
        public string Sid { get; set; }

        /// <summary>
        /// Gets or sets the user password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the user state
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the user
        /// </summary>
        public System.DateTime CreationDate { get; set; }
    }
}
