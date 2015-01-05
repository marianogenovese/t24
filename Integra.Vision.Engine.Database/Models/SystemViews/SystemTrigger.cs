//-----------------------------------------------------------------------
// <copyright file="SystemTrigger.cs" company="Integra.Vision.Engine.Database">
//     Copyright (c) Integra.Vision.Engine.Database. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Database.Models.SystemViews
{
    /// <summary>
    /// SystemTrigger class
    /// </summary>
    internal sealed class SystemTrigger
    {
        /// <summary>
        /// Gets or sets the trigger identifier
        /// </summary>
        public System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the window duration
        /// </summary>
        public double DurationTime { get; set; }

        /// <summary>
        /// Gets or sets the trigger id
        /// </summary>
        public System.Guid StreamId { get; set; }

        /// <summary>
        /// Gets or sets the trigger name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the trigger state
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is a system object
        /// </summary>
        public bool IsSystemObject { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the trigger
        /// </summary>
        public System.DateTime CreationDate { get; set; }
    }
}
