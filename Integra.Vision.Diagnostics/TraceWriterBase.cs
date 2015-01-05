//-----------------------------------------------------------------------
// <copyright file="TraceWriterBase.cs" company="Integra.Vision.Diagnostics">
//     Copyright (c) Integra.Vision.Diagnostics. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Diagnostics
{
    using System;
    using System.Diagnostics;
    using System.Runtime.ExceptionServices;
    using System.Security;
    using System.Security.Permissions;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;

    /// <summary>
    /// Provides a common base implementation for the basic trace writer.
    /// </summary>
    internal abstract class TraceWriterBase
    {
        /// <summary>
        /// Namespace of the trace record.
        /// </summary>
        private string @namespace;

        /// <summary>
        /// Check when tracer is in shutdown routine.
        /// </summary>
        private bool calledShutdown;

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceWriterBase"/> class.
        /// </summary>
        /// <param name="name">Name of the tracer</param>
        public TraceWriterBase(string name)
        {
            this.Name = name;
            this.@namespace = string.Format("http://Integra.Vision/Diagnostics/{0}/TraceRecord", name);
        }

        /// <summary>
        /// Gets or sets the trace level.
        /// </summary>
        public abstract SourceLevels Level
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the tracer.
        /// </summary>
        protected string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Checks whether an event should be tracing.
        /// </summary>
        /// <param name="type">Type of the event.</param>
        /// <returns>true if the event should be tracing; otherwise false.</returns>
        public virtual bool ShouldTrace(TraceEventType type)
        {
            return (this.IsEnable() && this.HaveListeners()) && (TraceEventType)0 != (type & ((TraceEventType)((int)this.Level)));
        }

        /// <summary>
        /// Core method for writing a trace event.
        /// </summary>
        /// <param name="type">The type of the event to write.</param>
        /// <param name="source">The object source of the event to write.</param>
        /// <param name="eventId">The id of the event to write.</param>
        /// <param name="description">The description of the event to write.</param>
        /// <param name="exception">The exception of the event to write.</param>
        /// <param name="entry">The trace entry.</param>
        public virtual void WriteCore(TraceEventType type, object source, int eventId, string description, Exception exception, TraceEntry entry)
        {
            // La intensión es reusar StringBuilders para que el uso de memoria sea mejor, la clase
            // StringBuilderPoolItem implementa IDisposable entonces al terminal el using se retorna
            // esta clase al pool y se hace clear del StringBuilder para que pueda ser usada posteriormente
            using (StringBuilderPoolItem item = StringBuilderPool.Take())
            {
                try
                {
                    using (XmlWriter writer = XmlWriter.Create(item.StringBuilder))
                    {
                        writer.WriteStartElement("TraceRecord", this.@namespace);
                        writer.WriteAttributeString("Severity", DiagnosticHelper.LookupSeverity(type));
                        writer.WriteAttributeString("EventId", eventId.ToString());
                        writer.WriteElementString("Description", description);
                        writer.WriteElementString("AppDomain", DiagnosticHelper.AppDomainFriendlyName);
                        if (source != null)
                        {
                            writer.WriteElementString("Source", DiagnosticHelper.CreateSourceString(source));
                        }

                        if (entry != null)
                        {
                            writer.WriteStartElement("Payload");
                            writer.WriteAttributeString("xmlns", entry.EventId);
                            entry.WriteTo(writer);
                            writer.WriteEndElement();
                        }

                        if (exception != null)
                        {
                            writer.WriteStartElement("Exception");
                            DiagnosticHelper.AddExceptionToTraceString(writer, exception);
                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                    }

                    this.OnWriteCore(item.StringBuilder, type, source, eventId, description, exception, entry);
                }
                catch (Exception e)
                {
                }
                finally
                {
                    string a = "NO VA";
                }
            }
        }

        /// <summary>
        /// Check if the tracer is enable.
        /// </summary>
        /// <returns>True if the tracer is enable, otherwise false.</returns>
        public abstract bool IsEnable();

        /// <summary>
        /// Check if the trace writer have listeners attached.
        /// </summary>
        /// <returns>True if the tracer has attached listeners; otherwise false.</returns>
        public abstract bool HaveListeners();

        /// <summary>
        /// This method subscribes to various events which are executed when the process is complete.
        /// </summary>
        protected void AddDomainEventHandlersForCleanup()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.UnhandledExceptionHandler);
            AppDomain.CurrentDomain.DomainUnload += new EventHandler(this.ExitOrUnloadEventHandler);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(this.ExitOrUnloadEventHandler);
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += new EventHandler<UnobservedTaskExceptionEventArgs>(this.UnobservedTaskException);
        }

        /// <summary>
        /// Handler of unobserved task exceptions.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The unobserved task exception event args.</param>
        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        [SecurityPermission(SecurityAction.Demand, ControlAppDomain = true)]
        protected void UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            this.OnUnobservedTaskException((Exception)e.Exception);
            e.SetObserved();
        }

        /// <summary>
        /// Handler of unhandled exceptions.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The unobserved task exception event args.</param>
        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        [SecurityPermission(SecurityAction.Demand, ControlAppDomain = true)]
        protected void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            this.OnUnhandledException((Exception)args.ExceptionObject);
            this.ShutdownTracing();
        }

        /// <summary>
        /// Invoked during the unhandled exception event.
        /// </summary>
        /// <param name="exception">The unhandled exception occurred.</param>
        protected abstract void OnUnhandledException(Exception exception);

        /// <summary>
        /// Invoked during the shutdown trace routine.
        /// </summary>
        protected abstract void OnShutdownTracing();

        /// <summary>
        /// Invoked during the unobserved task event.
        /// </summary>
        /// <param name="exception">The unobserved task exception.</param>
        protected abstract void OnUnobservedTaskException(Exception exception);

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
        protected abstract void OnWriteCore(StringBuilder traceRecord, TraceEventType type, object source, int eventId, string description, Exception exception, TraceEntry entry);

        /// <summary>
        /// Exit process or unload domain event handler.
        /// </summary>
        /// <param name="sender">The event sender object.</param>
        /// <param name="e">The event args.</param>
        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        private void ExitOrUnloadEventHandler(object sender, EventArgs e)
        {
            this.ShutdownTracing();
        }

        /// <summary>
        /// Shutdown the tracing facilities.
        /// </summary>
        private void ShutdownTracing()
        {
            if (!this.calledShutdown)
            {
                this.calledShutdown = true;
                try
                {
                    this.OnShutdownTracing();
                }
                catch (Exception exception)
                {
                    if (DiagnosticHelper.IsFatal(exception))
                    {
                        throw;
                    }

                    // this.LogTraceFailure(null, exception);
                }
            }
        }
    }
}
