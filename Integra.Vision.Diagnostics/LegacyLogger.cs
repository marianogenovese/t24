//-----------------------------------------------------------------------
// <copyright file="LegacyLogger.cs" company="Integra.Vision.Diagnostics">
//     Copyright (c) Integra.Vision.Diagnostics. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Diagnostics
{
    using System;

    /// <summary>
    /// This logger implementation use the LegacyTraceWriter.
    /// </summary>
    internal class LegacyLogger : LegacyTraceWriter, ILogger
    {
        /// <summary>
        /// The object that generates the log events.
        /// </summary>
        private object source;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LegacyLogger"/> class.
        /// </summary>
        /// <param name="name">The trace name.</param>
        /// <param name="source">The object that generates the log events.</param>
        public LegacyLogger(string name, object source) : base(name)
        {
            this.source = source;
        }
        
        /// <summary>Logs an info message.</summary>
        /// <param name="eventId">Identification of the event.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="formatArgs">String.Format arguments (if applicable).</param>
        public void Info(int eventId, string message, params object[] formatArgs)
        {
            this.WriteCore(System.Diagnostics.TraceEventType.Information, this.source, eventId, string.Format(message, formatArgs), null, null);
        }

        /// <summary>Logs a warning.</summary>
        /// <param name="eventId">Identification of the event.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="formatArgs">String.Format arguments (if applicable).</param>
        public void Warning(int eventId, string message, params object[] formatArgs)
        {
            this.WriteCore(System.Diagnostics.TraceEventType.Warning, this.source, eventId, string.Format(message, formatArgs), null, null);
        }

        /// <summary>Logs a debug message.</summary>
        /// <param name="eventId">Identification of the event.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="formatArgs">String.Format arguments (if applicable).</param>
        public void Debug(int eventId, string message, params object[] formatArgs)
        {
            this.WriteCore(System.Diagnostics.TraceEventType.Verbose, this.source, eventId, string.Format(message, formatArgs), null, null);
        }

        /// <summary>Logs an error message resulting from an exception.</summary>
        /// <param name="eventId">Identification of the event.</param>
        /// <param name="exception">An exception to be logged.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="formatArgs">String.Format arguments (if applicable).</param>
        public void Error(int eventId, Exception exception, string message, params object[] formatArgs)
        {
            this.WriteCore(System.Diagnostics.TraceEventType.Error, this.source, eventId, string.Format(message, formatArgs), exception, null);
        }

        /// <summary>Logs an error message.</summary>
        /// <param name="eventId">Identification of the event.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="formatArgs">String.Format arguments (if applicable).</param>
        public void Error(int eventId, string message, params object[] formatArgs)
        {
            this.WriteCore(System.Diagnostics.TraceEventType.Error, this.source, eventId, string.Format(message, formatArgs), null, null);
        }
    }
}
