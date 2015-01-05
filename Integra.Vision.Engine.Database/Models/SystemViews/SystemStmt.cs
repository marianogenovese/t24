//-----------------------------------------------------------------------
// <copyright file="SystemStmt.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models.SystemViews
{
    /// <summary>
    /// SystemStatement class
    /// </summary>
    internal sealed class SystemStmt
    {
        /// <summary>
        /// Gets or sets the statement position
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the statement type
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the adapter id
        /// </summary>
        public System.Guid AdapterId { get; set; }

        /// <summary>
        /// Gets or sets the stream id
        /// </summary>
        public System.Guid Id { get; set; }
    }
}
