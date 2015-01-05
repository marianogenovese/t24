//-----------------------------------------------------------------------
// <copyright file="WindowsEventTraceWriter.cs" company="Integra.Vision.Diagnostics">
//     Copyright (c) Integra.Vision.Diagnostics. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Diagnostics
{
    using System;
    using System.Diagnostics;
    using System.Text;
    
    /// <summary>
    /// This class write trace events in the Windows event log system.
    /// </summary>
    internal sealed class WindowsEventTraceWriter : TraceWriterBase
    {
        /// <summary>
        /// Flag for checking if the tracer can log.
        /// </summary>
        private bool canLogEvent = false;
        
        /// <summary>
        /// Windows event logger.
        /// </summary>
        private EventLog logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsEventTraceWriter"/> class.
        /// </summary>
        /// <param name="eventLogName">The windows event log name.</param>
        public WindowsEventTraceWriter(string eventLogName) : base(eventLogName)
        {
            try
            {
                if (!EventLog.SourceExists(eventLogName))
                {
                    EventLog.CreateEventSource(eventLogName, eventLogName);
                }

                this.logger = new EventLog();
                this.logger.Source = eventLogName;
                this.logger.Log = eventLogName;
                this.AddDomainEventHandlersForCleanup();
                this.canLogEvent = true;
            }
            catch
            {
                this.canLogEvent = false;
            }
        }

        /// <summary>
        /// Gets or sets the trace level.
        /// </summary>
        public override SourceLevels Level
        {
            get
            {
                return SourceLevels.All;
            }
            
            set
            {
            }
        }

        /// <summary>
        /// Check if the tracer is enable.
        /// </summary>
        /// <returns>True if the tracer is enable, otherwise false.</returns>
        public override bool IsEnable()
        {
            return this.canLogEvent;
        }

        /// <summary>
        /// Check if the trace writer have listeners attached.
        /// </summary>
        /// <returns>True if the tracer has attached listeners; otherwise false.</returns>
        public override bool HaveListeners()
        {
            return true;
        }
        
        /// <summary>
        /// Invoked during the trace event writing.
        /// </summary>
        /// <param name="traceRecord">The string buffer where is writing the event.</param>
        /// <param name="type">The type of the event to write.</param>
        /// <param name="source">The object source of the event to write.</param>
        /// <param name="code">The object code id of the event to write.</param>
        /// <param name="description">The description of the event to write.</param>
        /// <param name="exception">The exception of the event to write.</param>
        /// <param name="entry">The trace entry.</param>
        public void InternalWriteCore(StringBuilder traceRecord, TraceEventType type, object source, int code, string description, Exception exception, TraceEntry entry)
        {
            this.OnWriteCore(traceRecord, type, source, code, description, exception, entry);
        }
        
        /// <summary>
        /// Invoked during the trace event writing.
        /// </summary>
        /// <param name="traceRecord">The string buffer where is writing the event.</param>
        /// <param name="type">The type of the event to write.</param>
        /// <param name="source">The object source of the event to write.</param>
        /// <param name="eventId">The id of the event to write.</param>
        /// <param name="description">The description of the event to write.</param>
        /// <param name="exception">The exception of the event to write.</param>
        /// <param name="entry">The trace entry.</param>
        protected override void OnWriteCore(StringBuilder traceRecord, TraceEventType type, object source, int eventId, string description, Exception exception, TraceEntry entry)
        {
            if ((type == TraceEventType.Error) || (type == TraceEventType.Critical))
            {
                this.logger.WriteEntry(traceRecord.ToString(), EventLogEntryType.Error, eventId);
                return;
            }

            if (type == TraceEventType.Warning)
            {
                this.logger.WriteEntry(traceRecord.ToString(), EventLogEntryType.Warning, eventId);
                return;
            }

            this.logger.WriteEntry(traceRecord.ToString(), EventLogEntryType.Information, eventId);
        }

        /// <summary>
        /// Invoked during the unhandled exception event.
        /// </summary>
        /// <param name="exception">The unhandled exception occurred.</param>
        protected override void OnUnhandledException(Exception exception)
        {
            this.WriteCore(TraceEventType.Error, null, DiagnosticsEventIds.AppDomainUnload, Resources.SR.UnhandledException, exception, null);
        }

        /// <summary>
        /// Invoked during the shutdown trace routine.
        /// </summary>
        protected override void OnShutdownTracing()
        {
            try
            {
                this.logger.Dispose();
            }
            catch
            {
            }
        }
        
        /// <summary>
        /// Invoked during the unobserved task event.
        /// </summary>
        /// <param name="exception">The unobserved task exception.</param>
        protected override void OnUnobservedTaskException(Exception exception)
        {
            this.WriteCore(TraceEventType.Error, null, DiagnosticsEventIds.UnobservedTask, Resources.SR.UnobservedTaskException, exception, null);
        }
    }
}
