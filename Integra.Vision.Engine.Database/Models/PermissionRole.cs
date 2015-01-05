//-----------------------------------------------------------------------
// <copyright file="PermissionRole.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// PermissionRole class
    /// </summary>
    internal sealed class PermissionRole
    {
        /// <summary>
        /// Gets or sets a value indicating whether is denied or granted
        /// </summary>
        public bool Type { get; set; }

        /// <summary>
        /// Gets or sets the role 
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// Gets or sets the role id
        /// </summary>
        public System.Guid RoleId { get; set; }

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
