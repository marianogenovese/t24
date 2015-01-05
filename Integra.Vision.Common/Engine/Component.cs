//-----------------------------------------------------------------------
// <copyright file="Component.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Provides a common base implementation for the basic state machine common to all component.
    /// </summary>
    internal abstract class Component : IComponent
    {
        /// <summary>
        /// For aborted logic
        /// </summary>
        private bool aborted;

        /// <summary>
        /// When stop event has been called
        /// </summary>
        private bool stopCalled;
#if DEBUG
        /// <summary>
        /// Save stack trace on stop
        /// </summary>
        private StackTrace stopStack;

        /// <summary>
        /// Safe stack trace on fault
        /// </summary>
        private StackTrace faultedStack;
#endif

        /// <summary>
        /// Queue used for store ordered exceptions.
        /// </summary>
        private ExceptionQueue exceptionQueue;

        /// <summary>
        /// Exclusive lock
        /// </summary>
        private object mutex;

        /// <summary>
        /// For stopping state machine event
        /// </summary>
        private bool onStoppingCalled;

        /// <summary>
        /// For stopped state machine event.
        /// </summary>
        private bool onStoppedCalled;

        /// <summary>
        /// For starting state machine event.
        /// </summary>
        private bool onStartingCalled;

        /// <summary>
        /// For started state machine event.
        /// </summary>
        private bool onStartedCalled;

        /// <summary>
        /// For raise stopped state machine event.
        /// </summary>
        private bool raisedStopped;

        /// <summary>
        /// For raise stopping state machine event.
        /// </summary>
        private bool raisedStopping;

        /// <summary>
        /// For raise faulted state machine event.
        /// </summary>
        private bool raisedFaulted;

        /// <summary>
        /// Can trace start/stop events.
        /// </summary>
        private bool traceStartAndStop;

        /// <summary>
        /// Sender context on events.
        /// </summary>
        private object eventSender;

        /// <summary>
        /// Component state.
        /// </summary>
        private ComponentState state;

        /// <summary>
        /// Initializes a new instance of the <see cref="Component"/> class.
        /// </summary>
        /// <param name="mutex">The mutually exclusive lock that protects the class instance during a state transition.</param>
        /// <param name="eventSender">The object used as sender on events</param>
        internal Component(object mutex, object eventSender)
        {
            this.mutex = mutex;
            this.eventSender = eventSender;
            this.state = ComponentState.Created;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Component"/> class.
        /// </summary>
        protected Component() : this(new object())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Component"/> class.
        /// </summary>
        /// <param name="mutex">The mutually exclusive lock that protects the class instance during a state transition.</param>
        protected Component(object mutex)
        {
            this.mutex = mutex;
            this.eventSender = this;
            this.state = ComponentState.Created;
        }

        /// <summary>
        /// Occurs when a component transitions into the stopped state.
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// Occurs when a component transitions into the stopping state.
        /// </summary>
        public event EventHandler Stopping;

        /// <summary>
        /// Occurs when a component transitions into the faulted state.
        /// </summary>
        public event EventHandler Faulted;

        /// <summary>
        /// Occurs when a component transitions into the started state.
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Occurs when a component transitions into the starting state.
        /// </summary>
        public event EventHandler Starting;

        /// <summary>
        /// Gets a value indicating whether the component has disposed.
        /// </summary>
        public ComponentState State
        {
            get
            {
                return this.state;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the component has aborted.
        /// </summary>
        internal bool Aborted
        {
            get
            {
                return this.aborted;
            }
        }

        /// <summary>
        /// Gets or sets the sender context on events.
        /// </summary>
        internal object EventSender
        {
            get
            {
                return this.eventSender;
            }
            
            set
            {
                this.eventSender = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the component has disposed.
        /// </summary>
        protected bool IsDisposed
        {
            get
            {
                return this.state == ComponentState.Stopped;
            }
        }

        /// <summary>
        /// Gets the exclusive lock
        /// </summary>
        protected object ThisLock
        {
            get
            {
                return this.mutex;
            }
        }

        /// <summary>
        /// Causes a component to transition immediately from its current state into the stopping state.
        /// </summary>
        public void Abort()
        {
            lock (this.ThisLock)
            {
                if (this.aborted || this.state == ComponentState.Stopped)
                {
                    return;
                }

                this.aborted = true;
#if DEBUG
                if (this.stopStack == null)
                {
                    this.stopStack = new StackTrace();
                }
#endif

                this.state = ComponentState.Stopping;
            }

            /*
            if (DiagnosticUtility.ShouldTraceInformation)
            {
                TraceUtility.TraceEvent(TraceEventType.Information, TraceCode.CommunicationObjectAborted, SR.GetString(SR.TraceCodeCommunicationObjectAborted, TraceUtility.CreateSourceString(this)), this);
            }
            */
            bool throwing = true;

            try
            {                
                this.OnStopping();
                if (!this.onStoppingCalled)
                {
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                    // throw TraceUtility.ThrowHelperError(this.CreateBaseClassMethodNotCalledException("OnClosing"), Guid.Empty, this);
                }
                
                this.OnAbort();
                this.OnStopped();
                if (!this.onStoppedCalled)
                {
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                    // throw TraceUtility.ThrowHelperError(this.CreateBaseClassMethodNotCalledException("OnClosed"), Guid.Empty, this);
                }

                throwing = false;
            }
            finally
            {
                if (throwing)
                {
                    /*
                    if (DiagnosticUtility.ShouldTraceWarning)
                        TraceUtility.TraceEvent(TraceEventType.Warning, TraceCode.CommunicationObjectAbortFailed, SR.GetString(SR.TraceCodeCommunicationObjectAbortFailed, this.GetCommunicationObjectType().ToString()), this);
                     */
                }
            }
        }

        /// <summary>
        /// Causes a component to transition from its current state into the stopped state.
        /// </summary>
        public void Stop()
        {
            ComponentState originalState;
            lock (this.ThisLock)
            {
                originalState = this.state;
#if DEBUG
                if (this.stopStack == null)
                {
                    this.stopStack = new StackTrace();
                }
#endif
                if (originalState != ComponentState.Stopped)
                {
                    this.state = ComponentState.Stopping;
                }

                this.stopCalled = true;
            }

            switch (originalState)
            {
                case ComponentState.Created:
                case ComponentState.Starting:
                case ComponentState.Faulted:
                    this.Abort();
                    if (originalState == ComponentState.Faulted)
                    {
                        throw new InvalidCastException(); // NO va, va la linea de abajo
                        // throw TraceUtility.ThrowHelperError(this.CreateFaultedException(), Guid.Empty, this);
                    }

                    break;

                case ComponentState.Started:
                    {
                        bool throwing = true;
                        try
                        {                            
                            this.OnStopping();
                            if (!this.onStoppingCalled)
                            {
                                throw new InvalidCastException(); // NO va, va la linea de abajo
                                // throw TraceUtility.ThrowHelperError(this.CreateBaseClassMethodNotCalledException("OnClosing"), Guid.Empty, this);
                            }
                            
                            this.OnStop();
                            this.OnStopped();
                            if (!this.onStoppedCalled)
                            {
                                throw new InvalidCastException(); // NO va, va la linea de abajo
                                // throw TraceUtility.ThrowHelperError(this.CreateBaseClassMethodNotCalledException("OnClosed"), Guid.Empty, this);
                            }

                            throwing = false;
                        }
                        finally
                        {
                            if (throwing)
                            {
                                /*
                                if (DiagnosticUtility.ShouldTraceWarning)
                                {
                                    TraceUtility.TraceEvent(TraceEventType.Warning, TraceCode.CommunicationObjectCloseFailed, SR.GetString(SR.TraceCodeCommunicationObjectCloseFailed, this.GetCommunicationObjectType().ToString()), this);
                                }
                                */

                                this.Abort();
                            }
                        }

                        break;
                    }

                case ComponentState.Stopping:
                case ComponentState.Stopped:
                    break;

                default:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw Fx.AssertAndThrow("CommunicationObject.BeginClose: Unknown ComponentState");
            }
        }

        /// <summary>
        /// Causes a component to transition from the created state into the started state.
        /// </summary>
        public void Start()
        {
            lock (this.ThisLock)
            {
                this.ThrowIfDisposedOrImmutable();
                this.state = ComponentState.Starting;
            }

            bool throwing = true;
            try
            {
                this.OnStarting();
                if (!this.onStartingCalled)
                {
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                    // throw TraceUtility.ThrowHelperError(this.CreateBaseClassMethodNotCalledException("OnOpening"), Guid.Empty, this);
                }
                
                this.OnStart();
                this.OnStarted();
                if (!this.onStartedCalled)
                {
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                    // throw TraceUtility.ThrowHelperError(this.CreateBaseClassMethodNotCalledException("OnOpened"), Guid.Empty, this);
                }
                
                throwing = false;
            }
            finally
            {
                if (throwing)
                {
                    /*
                    if (DiagnosticUtility.ShouldTraceWarning)
                    {
                        TraceUtility.TraceEvent(TraceEventType.Warning, TraceCode.CommunicationObjectOpenFailed, SR.GetString(SR.TraceCodeCommunicationObjectOpenFailed, this.GetCommunicationObjectType().ToString()), this);
                    }
                    */

                    this.Fault();
                }
            }
        }
        
        /// <summary>
        /// Create a exception used in validation of Stopped state.
        /// </summary>
        /// <returns>A <see cref="ObjectDisposedException"/> exception.</returns>
        internal Exception CreateStoppedException()
        {
            if (!this.stopCalled)
            {
                return this.CreateAbortedException();
            }
            else
            {
#if DEBUG
                string originalStack = this.stopStack.ToString().Replace("\r\n", "\r\n    ");
                return new ObjectDisposedException(this.GetComponentObjectType().ToString() + ", Object already stopped:\r\n    " + originalStack);
#else
                return new ObjectDisposedException(this.GetComponentObjectType().ToString());
#endif
            }
        }
        
        /// <summary>
        /// Create a exception used in validation of faulted state.
        /// </summary>
        /// <returns>A <see cref="ComponentFaultedException"/> exception.</returns>
        internal Exception CreateFaultedException()
        {
#if DEBUG
            string originalStack = this.faultedStack.ToString().Replace("\r\n", "\r\n    ");
            string message = "CAMBIAR VA LO DE LA DERECHA"; // SR.GetString(SR.CommunicationObjectFaultedStack2, this.GetCommunicationObjectType().ToString(), originalStack);
#else
            string message = SR.GetString(SR.CommunicationObjectFaulted1, this.GetCommunicationObjectType().ToString());
#endif
            return new ComponentFaultedException(message);
        }

        /// <summary>
        /// Create a exception used in validation of aborted state.
        /// </summary>
        /// <returns>A <see cref="ComponentFaultedException"/> exception.</returns>
        internal Exception CreateAbortedException()
        {
#if DEBUG
            string originalStack = this.stopStack.ToString().Replace("\r\n", "\r\n    ");
            return new ComponentFaultedException(); // No va, la la linea de abajo
            // return new ComponentFaultedException(SR.GetString(SR.CommunicationObjectAbortedStack2, this.GetCommunicationObjectType().ToString(), originalStack));
#else
            return new ComponentFaultedException(SR.GetString(SR.CommunicationObjectAborted1, this.GetCommunicationObjectType().ToString()));
#endif
        }

        /// <summary>
        /// Causes a component to transition from its current state into the faulted state.
        /// </summary>
        /// <param name="exception">The exception that caused the failure.</param>
        internal void Fault(Exception exception)
        {
            lock (this.ThisLock)
            {
                if (this.exceptionQueue == null)
                {
                    this.exceptionQueue = new ExceptionQueue(this.ThisLock);
                }
            }

            /*
            if (exception != null && DiagnosticUtility.ShouldTraceInformation)
            {
                TraceUtility.TraceEvent(TraceEventType.Information, TraceCode.CommunicationObjectFaultReason, SR.GetString(SR.TraceCodeCommunicationObjectFaultReason), exception, null);
            }
            */
            this.exceptionQueue.AddException(exception);
            this.Fault();
        }

        /// <summary>
        /// Add a exception in the exception queue.
        /// </summary>
        /// <param name="exception">The exception that caused the failure.</param>
        internal void AddPendingException(Exception exception)
        {
            lock (this.ThisLock)
            {
                if (this.exceptionQueue == null)
                {
                    this.exceptionQueue = new ExceptionQueue(this.ThisLock);
                }
            }

            this.exceptionQueue.AddException(exception);
        }

        /// <summary>
        /// Gets the exception in the top of the queue.
        /// </summary>
        /// <returns>The exception on top.</returns>
        internal Exception GetPendingException()
        {
            ComponentState currentState = this.state;

            // Fx.Assert(currentState == ComponentState.Closing || currentState == ComponentState.Closed || currentState == ComponentState.Faulted, "CommunicationObject.GetPendingException(currentState == CommunicationState.Closing || currentState == CommunicationState.Closed || currentState == CommunicationState.Faulted)");
            ExceptionQueue queue = this.exceptionQueue;
            if (queue != null)
            {
                return queue.GetException();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Terminal is loosely defined as an interruption to stop or a fault.
        /// </summary>
        /// <returns>The exception that caused the failure.</returns>
        internal Exception GetTerminalException()
        {
            Exception exception = this.GetPendingException();

            if (exception != null)
            {
                return exception;
            }

            switch (this.state)
            {
                case ComponentState.Stopping:
                case ComponentState.Stopped:
                    return new ComponentException(); // No va, va la linea de abajo
                    // return new CommunicationException(SR.GetString(SR.CommunicationObjectCloseInterrupted1, this.GetCommunicationObjectType().ToString()));
                case ComponentState.Faulted:
                    return this.CreateFaultedException();
                default:
                    throw new InvalidCastException(); // NO VA, va la linea de abajo
                // throw Fx.AssertAndThrow("GetTerminalException: Invalid CommunicationObject.state");
            }
        }

        /// <summary>
        /// Checks if exists a pending exception, if exists throw the exception.
        /// </summary>
        internal void ThrowPending()
        {
            ExceptionQueue queue = this.exceptionQueue;

            if (queue != null)
            {
                Exception exception = queue.GetException();

                if (exception != null)
                {
                    throw new InvalidCastException(); // No va, va la linea de abajo
                    // throw TraceUtility.ThrowHelperError(exception, Guid.Empty, this);
                }
            }
        }

        /// <summary>
        /// If the component is not in started or stopping state, throw a exception.
        /// </summary>
        internal void ThrowIfStoppedOrNotStarted()
        {
            this.ThrowPending();

            switch (this.state)
            {
                case ComponentState.Created:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw TraceUtility.ThrowHelperError(this.CreateNotOpenException(), Guid.Empty, this);
                case ComponentState.Starting:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw TraceUtility.ThrowHelperError(this.CreateNotOpenException(), Guid.Empty, this);
                case ComponentState.Started:
                    break;

                case ComponentState.Stopping:
                    break;

                case ComponentState.Stopped:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw TraceUtility.ThrowHelperError(this.CreateClosedException(), Guid.Empty, this);
                case ComponentState.Faulted:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw TraceUtility.ThrowHelperError(this.CreateFaultedException(), Guid.Empty, this);
                default:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw Fx.AssertAndThrow("ThrowIfClosedOrNotOpen: Unknown CommunicationObject.state");
            }
        }

        /// <summary>
        /// If the component is in fault state, throw a exception.
        /// </summary>
        internal void ThrowIfFaulted()
        {
            this.ThrowPending();

            switch (this.state)
            {
                case ComponentState.Created:
                    break;

                case ComponentState.Starting:
                    break;

                case ComponentState.Started:
                    break;

                case ComponentState.Stopping:
                    break;

                case ComponentState.Stopped:
                    break;
                case ComponentState.Faulted:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw TraceUtility.ThrowHelperError(this.CreateFaultedException(), Guid.Empty, this);                
                default:
                    throw new InvalidCastException(); // NO VA, va la linea de abajo
                // throw Fx.AssertAndThrow("ThrowIfFaulted: Unknown CommunicationObject.state");
            }
        }
        
        /// <summary>
        /// If the component was not aborted and the method stop was not called throws an exception.
        /// </summary>
        internal void ThrowIfAborted()
        {
            if (this.aborted && !this.stopCalled)
            {
                throw new InvalidCastException(); // NO VA, va la linea de abajo
                // throw TraceUtility.ThrowHelperError(CreateAbortedException(), Guid.Empty, this);
            }
        }

        /// <summary>
        /// If the component is in stop or fault state, throw a exception.
        /// </summary>
        internal void ThrowIfStopped()
        {
            this.ThrowPending();

            switch (this.state)
            {
                case ComponentState.Created:
                    break;

                case ComponentState.Starting:
                    break;

                case ComponentState.Started:
                    break;

                case ComponentState.Stopping:
                    break;

                case ComponentState.Stopped:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw TraceUtility.ThrowHelperError(this.CreateClosedException(), Guid.Empty, this);
                case ComponentState.Faulted:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw TraceUtility.ThrowHelperError(this.CreateFaultedException(), Guid.Empty, this);
                default:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw Fx.AssertAndThrow("ThrowIfClosed: Unknown CommunicationObject.state");
            }
        }

        /// <summary>
        /// If the component is not in created or starting state, throw a exception.
        /// </summary>
        internal void ThrowIfStoppedOrStarted()
        {
            this.ThrowPending();

            switch (this.state)
            {
                case ComponentState.Created:
                    break;

                case ComponentState.Starting:
                    break;

                case ComponentState.Started:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw TraceUtility.ThrowHelperError(this.CreateImmutableException(), Guid.Empty, this);
                case ComponentState.Stopping:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw TraceUtility.ThrowHelperError(this.CreateImmutableException(), Guid.Empty, this);
                case ComponentState.Stopped:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw TraceUtility.ThrowHelperError(this.CreateClosedException(), Guid.Empty, this);
                case ComponentState.Faulted:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw TraceUtility.ThrowHelperError(this.CreateFaultedException(), Guid.Empty, this);
                default:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw Fx.AssertAndThrow("ThrowIfClosedOrOpened: Unknown CommunicationObject.state");
            }
        }

        /// <summary>
        /// If the component is not in create or starting state, throw a exception.
        /// </summary>
        internal void ThrowIfNotStarted()
        {
            if (this.state == ComponentState.Created || this.state == ComponentState.Starting)
            {
                throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw TraceUtility.ThrowHelperError(this.CreateNotOpenException(), Guid.Empty, this);
            }
        }

        /// <summary>
        /// If the component is not in create, starting or started state, throw a exception.
        /// </summary>
        protected internal void ThrowIfDisposed()
        {
            this.ThrowPending();

            switch (this.state)
            {
                case ComponentState.Created:
                    break;

                case ComponentState.Starting:
                    break;

                case ComponentState.Started:
                    break;

                case ComponentState.Stopping:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw TraceUtility.ThrowHelperError(this.CreateClosedException(), Guid.Empty, this);
                case ComponentState.Stopped:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw TraceUtility.ThrowHelperError(this.CreateClosedException(), Guid.Empty, this);
                case ComponentState.Faulted:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw TraceUtility.ThrowHelperError(this.CreateFaultedException(), Guid.Empty, this);
                default:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw Fx.AssertAndThrow("ThrowIfDisposed: Unknown CommunicationObject.state");
            }
        }

        /// <summary>
        /// If the component is not in created state, throw a exception.
        /// </summary>
        protected internal void ThrowIfDisposedOrImmutable()
        {
            this.ThrowPending();

            switch (this.state)
            {
                case ComponentState.Created:
                    break;

                case ComponentState.Starting:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw TraceUtility.ThrowHelperError(this.CreateImmutableException(), Guid.Empty, this);
                case ComponentState.Started:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw TraceUtility.ThrowHelperError(this.CreateImmutableException(), Guid.Empty, this);
                case ComponentState.Stopping:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw TraceUtility.ThrowHelperError(this.CreateClosedException(), Guid.Empty, this);
                case ComponentState.Stopped:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw TraceUtility.ThrowHelperError(this.CreateClosedException(), Guid.Empty, this);
                case ComponentState.Faulted:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw TraceUtility.ThrowHelperError(this.CreateFaultedException(), Guid.Empty, this);
                default:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                // throw Fx.AssertAndThrow("ThrowIfDisposedOrImmutable: Unknown CommunicationObject.state");
            }
        }

        /// <summary>
        /// If the component is not in started state, throw a exception.
        /// </summary>
        protected internal void ThrowIfDisposedOrNotStarted()
        {
            this.ThrowPending();

            switch (this.state)
            {
                case ComponentState.Created:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                    // throw TraceUtility.ThrowHelperError(this.CreateNotOpenException(), Guid.Empty, this);
                case ComponentState.Starting:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                    // throw TraceUtility.ThrowHelperError(this.CreateNotOpenException(), Guid.Empty, this);
                case ComponentState.Started:
                    break;

                case ComponentState.Stopping:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                    // throw TraceUtility.ThrowHelperError(this.CreateClosedException(), Guid.Empty, this);
                case ComponentState.Stopped:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                    // throw TraceUtility.ThrowHelperError(this.CreateClosedException(), Guid.Empty, this);
                case ComponentState.Faulted:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                    // throw TraceUtility.ThrowHelperError(this.CreateFaultedException(), Guid.Empty, this);
                default:
                    throw new InvalidCastException(); // NO va, va la linea de abajo
                    // throw Fx.AssertAndThrow("ThrowIfDisposedOrNotOpen: Unknown CommunicationObject.state");
            }
        }

        /// <summary>
        /// Causes a component to transition from its current state into the faulted state.
        /// </summary>
        protected void Fault()
        {
            lock (this.ThisLock)
            {
                if (this.state == ComponentState.Stopped || this.state == ComponentState.Stopping)
                {
                    return;
                }

                if (this.state == ComponentState.Faulted)
                {
                    return;
                }
#if DEBUG
                if (this.faultedStack == null)
                {
                    this.faultedStack = new StackTrace();
                }
#endif
                this.state = ComponentState.Faulted;
            }

            this.OnFaulted();
        }
        
        /// <summary>
        /// Invoked during the transition of a component into the stopped state.
        /// </summary>
        protected virtual void OnStopped()
        {
            this.onStoppedCalled = true;

            lock (this.ThisLock)
            {
                if (this.raisedStopped)
                {
                    return;
                }
                
                this.raisedStopped = true;
                this.state = ComponentState.Stopped;
            }
            
            /*
            if (DiagnosticUtility.ShouldTraceVerbose)
            {
                TraceUtility.TraceEvent(TraceEventType.Verbose, TraceCode.CommunicationObjectClosed, SR.GetString(SR.TraceCodeCommunicationObjectClosed, TraceUtility.CreateSourceString(this)), this);
            }
            */
            EventHandler handler = this.Stopped;
            if (handler != null)
            {
                try
                {
                    handler(this.eventSender, EventArgs.Empty);
                }
                catch (Exception exception)
                {
                    // if (Fx.IsFatal(exception))
                        throw;

                    // throw DiagnosticUtility.ExceptionUtility.ThrowHelperCallback(exception);
                }
            }
        }
        
        /// <summary>
        /// Invoked during the transition of a component into the stopping state.
        /// </summary>
        protected virtual void OnStopping()
        {
            this.onStoppingCalled = true;

            lock (this.ThisLock)
            {
                if (this.raisedStopping)
                {
                    return;
                }
                
                this.raisedStopping = true;
            }

            /*
            if (DiagnosticUtility.ShouldTraceVerbose)
            {
                TraceUtility.TraceEvent(TraceEventType.Verbose, TraceCode.CommunicationObjectClosing, SR.GetString(SR.TraceCodeCommunicationObjectClosing, TraceUtility.CreateSourceString(this)), this);
            }
            */
            
            EventHandler handler = this.Stopping;
            if (handler != null)
            {
                try
                {
                    handler(this.eventSender, EventArgs.Empty);
                }
                catch (Exception exception)
                {
                    // if (Fx.IsFatal(exception))
                        throw;

                    // throw DiagnosticUtility.ExceptionUtility.ThrowHelperCallback(exception);
                }
            }
        }
        
        /// <summary>
        /// Inserts processing on a component after it transitions to the faulted state due to the invocation of a synchronous fault operation.
        /// </summary>
        protected virtual void OnFaulted()
        {
            lock (this.ThisLock)
            {
                if (this.raisedFaulted)
                {
                    return;
                }
                
                this.raisedFaulted = true;
            }
            
            /*
            if (DiagnosticUtility.ShouldTraceWarning)
            {
                TraceUtility.TraceEvent(TraceEventType.Warning, TraceCode.CommunicationObjectFaulted, SR.GetString(SR.TraceCodeCommunicationObjectFaulted, this.GetCommunicationObjectType().ToString()), this);
            }
            */
            EventHandler handler = this.Faulted;
            if (handler != null)
            {
                try
                {
                    handler(this.eventSender, EventArgs.Empty);
                }
                catch (Exception exception)
                {
                    // if (Fx.IsFatal(exception))
                        throw;

                    // throw DiagnosticUtility.ExceptionUtility.ThrowHelperCallback(exception);
                }
            }
        }
        
        /// <summary>
        /// Invoked during the transition of a component into the started state.
        /// </summary>
        protected virtual void OnStarted()
        {
            this.onStartedCalled = true;

            lock (this.ThisLock)
            {
                if (this.aborted || this.state != ComponentState.Starting)
                {
                    return;
                }
                
                this.state = ComponentState.Started;
            }

            /*
            if (DiagnosticUtility.ShouldTraceVerbose)
                TraceUtility.TraceEvent(TraceEventType.Verbose, TraceCode.CommunicationObjectOpened, SR.GetString(SR.TraceCodeCommunicationObjectOpened, TraceUtility.CreateSourceString(this)), this);
            */
            EventHandler handler = this.Started;
            if (handler != null)
            {
                try
                {
                    handler(this.eventSender, EventArgs.Empty);
                }
                catch (Exception exception)
                {
                    // if (Fx.IsFatal(exception))
                        throw;

                    // throw DiagnosticUtility.ExceptionUtility.ThrowHelperCallback(exception);
                }
            }
        }
        
        /// <summary>
        /// Invoked during the transition of a component into the starting state.
        /// </summary>
        protected virtual void OnStarting()
        {
            this.onStartingCalled = true;
            /*
            if (DiagnosticUtility.ShouldTraceVerbose)
            {
                TraceUtility.TraceEvent(TraceEventType.Verbose, TraceCode.CommunicationObjectOpening, SR.GetString(SR.TraceCodeCommunicationObjectOpening, TraceUtility.CreateSourceString(this)), this);
            }
            */
            EventHandler handler = this.Starting;
            if (handler != null)
            {
                try
                {
                    handler(this.eventSender, EventArgs.Empty);
                }
                catch (Exception exception)
                {
                    // if (Fx.IsFatal(exception))
                        throw;

                    // throw DiagnosticUtility.ExceptionUtility.ThrowHelperCallback(exception);
                }
            }
        }

        /*

        internal bool TraceOpenAndClose
        {
            get
            {
                return this.traceOpenAndClose;
            }
            set
            {
                this.traceOpenAndClose = value && DiagnosticUtility.ShouldUseActivity;
            }
        }
        */
        
        /// <summary>
        ///  Gets the type of component object.
        /// </summary>
        /// <returns>The type of component object.</returns>
        protected virtual Type GetComponentObjectType()
        {
            return this.GetType();
        }
        
        /// <summary>
        /// Inserts processing on a component after it transitions to the stopping state due to the invocation of a synchronous abort operation.
        /// </summary>
        protected abstract void OnAbort();
        
        /// <summary>
        /// Inserts processing on a component after it transitions to the stopping state due to the invocation of a synchronous stop operation.
        /// </summary>
        protected abstract void OnStop();
        
        /// <summary>
        /// Inserts processing on a component after it transitions into the starting state.
        /// </summary>
        protected abstract void OnStart();

        /// <summary>
        /// Create a exception used when the component is in create or starting state.
        /// </summary>
        /// <returns>A <see cref="InvalidOperationException"/></returns>
        private Exception CreateNotStartException()
        {
            return new InvalidOperationException(); // No va, va la linea de abajo

            // return new InvalidOperationException(SR.GetString(SR.CommunicationObjectCannotBeUsed, this.GetCommunicationObjectType().ToString(), this.state.ToString()));
        }

        /// <summary>
        /// Create a exception used when the component is in a immutable state.
        /// </summary>
        /// <returns>A <see cref="InvalidOperationException"/></returns>
        private Exception CreateImmutableException()
        {
            return new InvalidOperationException(); // No va, va la linea de abajo

            // return new InvalidOperationException(SR.GetString(SR.CommunicationObjectCannotBeModifiedInState, this.GetCommunicationObjectType().ToString(), this.state.ToString()));
        }
        
        /// <summary>
        /// Create a exception used when in the derived classes are no following the state machine rules.
        /// </summary>
        /// <param name="method">Name of the method that was not called</param>
        /// <returns>A <see cref="InvalidOperationException"/></returns>
        private Exception CreateBaseClassMethodNotCalledException(string method)
        {
            return new InvalidOperationException(); // No va, va la linea de abajo
            
            // return new InvalidOperationException(SR.GetString(SR.CommunicationObjectBaseClassMethodNotCalled, this.GetCommunicationObjectType().ToString(), method));
        }

        /// <summary>
        /// This class represents a queue of exceptions
        /// </summary>
        private class ExceptionQueue
        {
            /// <summary>
            /// A internal queue used for store
            /// </summary>
            private Queue<Exception> exceptions = new Queue<Exception>();

            /// <summary>
            /// Exclusive lock for concurrency
            /// </summary>
            private object thisLock;

            /// <summary>
            /// Initializes a new instance of the <see cref="ExceptionQueue"/> class.
            /// </summary>
            /// <param name="thisLock">The mutually exclusive lock that protects the class instance.</param>
            internal ExceptionQueue(object thisLock)
            {
                this.thisLock = thisLock;
            }

            /// <summary>
            /// Gets the mutually exclusive lock.
            /// </summary>
            private object ThisLock
            {
                get { return this.thisLock; }
            }

            /// <summary>
            /// Adds a new exception in the queue
            /// </summary>
            /// <param name="exception">Exception occurred</param>
            public void AddException(Exception exception)
            {
                if (exception == null)
                {
                    return;
                }

                lock (this.ThisLock)
                {
                    this.exceptions.Enqueue(exception);
                }
            }

            /// <summary>
            /// Gets the exception in the top of the queue
            /// </summary>
            /// <returns>The exception object</returns>
            public Exception GetException()
            {
                lock (this.ThisLock)
                {
                    if (this.exceptions.Count > 0)
                    {
                        return this.exceptions.Dequeue();
                    }
                }

                return null;
            }
        }
    }
}
