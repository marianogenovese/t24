//-----------------------------------------------------------------------
// <copyright file="TraceEntry.cs" company="Integra.Vision.Diagnostics">
//     Copyright (c) Integra.Vision.Diagnostics. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Diagnostics
{
    using System.Xml;
    
    /// <summary>
    /// Represents a trace record or entry.
    /// </summary>
    internal abstract class TraceEntry
    {
        /// <summary>
        /// Id base of this entry.
        /// </summary>
        protected const string EventIdBase = "http://Integra.Vision/Diagnostics/";
        
        /// <summary>
        /// Namespace default.
        /// </summary>
        protected const string NamespaceSuffix = "TraceEntry";

        /// <summary>
        /// Gets the entry Id.
        /// </summary>
        internal virtual string EventId
        {
            get
            {
                return this.BuildEventId("Empty");
            }
        }

        /// <summary>
        /// This method allows you to customize the format of writing the event.
        /// </summary>
        /// <param name="writer">The writer context.</param>
        internal virtual void WriteTo(XmlWriter writer)
        {
        }

        /// <summary>
        /// Build a full event id.
        /// </summary>
        /// <param name="eventId">The id of the event.</param>
        /// <returns>The event id.</returns>
        protected string BuildEventId(string eventId)
        {
            return TraceEntry.EventIdBase + eventId + TraceEntry.NamespaceSuffix;
        }

        /// <summary>
        /// Method to encode a string into a proper XML format.
        /// </summary>
        /// <param name="text">The string to be encoded.</param>
        /// <returns>The proper string.</returns>
        protected string XmlEncode(string text)
        {
            return DiagnosticHelper.XmlEncode(text);
        }
    }
}
