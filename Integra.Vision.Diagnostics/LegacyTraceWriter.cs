//-----------------------------------------------------------------------
// <copyright file="LegacyTraceWriter.cs" company="Integra.Vision.Diagnostics">
//     Copyright (c) Integra.Vision.Diagnostics. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Diagnostics
{
    using System;
    using System.Diagnostics;
    using System.Text;
    
    /// <summary>
    /// This class write trace events in EventLog, ETW and .Net diagnostics framework.
    /// </summary>
    internal class LegacyTraceWriter : TraceWriterBase
    {
        /// <summary>
        /// Current level.
        /// </summary>
        private SourceLevels level;
        
        /// <summary>
        /// Flag for checking if the tracer can log.
        /// </summary>
        private bool isEnable = false;
        
        /// <summary>
        /// Mutual exclusive lock for concurrency.
        /// </summary>
        private object thisLock = new object();
        
        /// <summary>
        /// Flag for checking if the tracer has listeners attached.
        /// </summary>
        private bool haveListeners = false;
        
        /// <summary>
        /// .Net Diagnostic Framework tracer.
        /// </summary>
        private TraceSource legacyTracer;

        /// <summary>
        /// Windows event log tracer.
        /// </summary>
        private WindowsEventTraceWriter eventLogTracer;
        
        // ETWTraceWriter _ETWTracer;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LegacyTraceWriter"/> class.
        /// </summary>
        /// <param name="name">The trace name.</param>
        public LegacyTraceWriter(string name) : base(name)
        {
            this.legacyTracer = new TraceSource(name);
            this.eventLogTracer = new WindowsEventTraceWriter(name);
            
            // _ETWTracer = new ETWTraceWriter(traceName);
            this.haveListeners = this.legacyTracer.Listeners.Count > 0;
            this.SetLevel(this.legacyTracer.Switch.Level);
            this.AddDomainEventHandlersForCleanup();
        }

        /// <summary>
        /// Gets or sets the trace level.
        /// </summary>
        public override SourceLevels Level
        {
            get
            {
                if (this.legacyTracer.Switch.Level != this.level)
                {
                    this.level = this.legacyTracer.Switch.Level;
                }

                return this.level;
            }
            
            set
            {
                this.SetLevelThreadSafe(value);
            }
        }
        
        /// <summary>
        /// Check if the tracer is enable.
        /// </summary>
        /// <returns>True if the tracer is enable, otherwise false.</returns>
        public override bool IsEnable()
        {
            return this.isEnable;
        }

        /// <summary>
        /// Check if the trace writer have listeners attached.
        /// </summary>
        /// <returns>True if the tracer has attached listeners; otherwise false.</returns>
        public override bool HaveListeners()
        {
            return this.haveListeners;
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
            try
            {
                if (this.IsEnable() && this.ShouldTrace(type))
                {
                    this.legacyTracer.TraceEvent(type, eventId, traceRecord.ToString());
                }
            }
            catch
            {
            }

            try
            {
                this.eventLogTracer.InternalWriteCore(traceRecord, type, source, eventId, description, exception, entry);
            }
            catch
            {
            }

            /*
            try
            {
                _ETWTracer.ExternalWriteCore(traceRecord, type, source, eventId, description, exception, entry);
            }
            catch
            {
            }
             */
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
            this.legacyTracer.Flush();
        }

        /// <summary>
        /// Invoked during the unobserved task event.
        /// </summary>
        /// <param name="exception">The unobserved task exception.</param>
        protected override void OnUnobservedTaskException(Exception exception)
        {
            this.WriteCore(TraceEventType.Error, null, DiagnosticsEventIds.UnobservedTask, Resources.SR.UnobservedTaskException, exception, null);
        }

        /// <summary>
        /// Change the current source level.
        /// </summary>
        /// <param name="level">The new level.</param>
        private void SetLevelThreadSafe(SourceLevels level)
        {
            lock (this.thisLock)
            {
                this.SetLevel(level);
            }
        }
        
        /// <summary>
        /// Change the current source level.
        /// </summary>
        /// <param name="level">The new level.</param>
        private void SetLevel(SourceLevels level)
        {
            this.level = this.FixLevel(level);
            this.haveListeners = this.legacyTracer.Listeners.Count > 0;
            this.isEnable = this.haveListeners && (this.level != SourceLevels.Off);
            this.legacyTracer.Switch.Level = level;
        }
        
        /// <summary>
        /// Change the level for internal use.
        /// </summary>
        /// <param name="level">The original level.</param>
        /// <returns>The changed level.</returns>
        private SourceLevels FixLevel(SourceLevels level)
        {
            if (((level & ~SourceLevels.Information) & SourceLevels.Verbose) != SourceLevels.Off)
            {
                level |= SourceLevels.Verbose;
            }            
            else if (((level & ~SourceLevels.Warning) & SourceLevels.Information) != SourceLevels.Off)
            {
                level |= SourceLevels.Information;
            }            
            else if (((level & ~SourceLevels.Error) & SourceLevels.Warning) != SourceLevels.Off)
            {
                level |= SourceLevels.Warning;
            }
            
            if (((level & ~SourceLevels.Critical) & SourceLevels.Error) != SourceLevels.Off)
            {
                level |= SourceLevels.Error;
            }
            
            if ((level & SourceLevels.Critical) != SourceLevels.Off)
            {
                level |= SourceLevels.Critical;
            }
            
            if (level == SourceLevels.ActivityTracing)
            {
                level = SourceLevels.Off;
            }
            
            return level;
        }
    }
}
