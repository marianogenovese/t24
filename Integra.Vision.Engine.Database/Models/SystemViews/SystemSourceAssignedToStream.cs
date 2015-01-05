//-----------------------------------------------------------------------
// <copyright file="SystemSourceAssignedToStream.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models.SystemViews
{
    /// <summary>
    /// SystemSourceAssignedToStream class
    /// </summary>
    internal sealed class SystemSourceAssignedToStream
    {
        /// <summary>
        /// Gets or sets the source alias
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is OnSource
        /// </summary>
        public bool IsWithSource { get; set; }

        /// <summary>
        /// Gets or sets the assigned source id
        /// </summary>
        public System.Guid SourceId { get; set; }

        /// <summary>
        /// Gets or sets the stream id
        /// </summary>
        public System.Guid StreamId { get; set; }
    }
}
