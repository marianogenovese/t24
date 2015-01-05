//-----------------------------------------------------------------------
// <copyright file="ILogger.cs" company="Integra.Vision.Diagnostics">
//     Copyright (c) Integra.Vision.Diagnostics. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Diagnostics
{
    using System;

    /// <summary>
    /// An interface for loggers.
    /// </summary>
    internal interface ILogger
    {
        /// <summary>Logs an info message.</summary>
        /// <param name="eventId">Identification of the event.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="formatArgs">String.Format arguments (if applicable).</param>
        void Info(int eventId, string message, params object[] formatArgs);

        /// <summary>Logs a warning.</summary>
        /// <param name="eventId">Identification of the event.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="formatArgs">String.Format arguments (if applicable).</param>
        void Warning(int eventId, string message, params object[] formatArgs);

        /// <summary>Logs a debug message.</summary>
        /// <param name="eventId">Identification of the event.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="formatArgs">String.Format arguments (if applicable).</param>
        void Debug(int eventId, string message, params object[] formatArgs);

        /// <summary>Logs an error message resulting from an exception.</summary>
        /// <param name="eventId">Identification of the event.</param>
        /// <param name="exception">An exception to be logged.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="formatArgs">String.Format arguments (if applicable).</param>
        void Error(int eventId, Exception exception, string message, params object[] formatArgs);

        /// <summary>Logs an error message.</summary>
        /// <param name="eventId">Identification of the event.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="formatArgs">String.Format arguments (if applicable).</param>
        void Error(int eventId, string message, params object[] formatArgs);
    }
}
