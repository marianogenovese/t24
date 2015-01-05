//-----------------------------------------------------------------------
// <copyright file="SystemRoleMember.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models.SystemViews
{
    /// <summary>
    /// SystemRoleMember class
    /// </summary>
    internal sealed class SystemRoleMember
    {
        /// <summary>
        /// Gets or sets the role id
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        public System.Guid Uid { get; set; }

        /// <summary>
        /// Gets or sets the assignation date
        /// </summary>
        public System.DateTime CreationDate { get; set; }
    }
}
