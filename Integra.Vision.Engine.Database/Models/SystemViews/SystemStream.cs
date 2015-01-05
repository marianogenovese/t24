//-----------------------------------------------------------------------
// <copyright file="SystemStream.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models.SystemViews
{
    /// <summary>
    /// SystemStream class
    /// </summary>
    internal sealed class SystemStream
    {
        /// <summary>
        /// Gets or sets the adapter identifier
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the Duration time of the window
        /// </summary>
        public double DurationTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is join expression or not
        /// </summary>
        public bool UseJoin { get; set; }

        /// <summary>
        /// Gets or sets the stream name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the stream state
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is a system object
        /// </summary>
        public bool IsSystemObject { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the stream
        /// </summary>
        public System.DateTime CreationDate { get; set; }
    }
}
