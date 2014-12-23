//-----------------------------------------------------------------------
// <copyright file="IComponent.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine
{
    using System;
    
    /// <summary>
    /// Defines the contract for the basic state machine for all components.
    /// </summary>
    internal interface IComponent
    {        
        /// <summary>
        /// Occurs when the component completes its transition from the stopping
        /// </summary>
        event EventHandler Stopped;
        
        /// <summary>
        /// Occurs when the component first enters the stopping state.
        /// </summary>
        event EventHandler Stopping;
        
        /// <summary>
        /// Occurs when the component first enters the faulted state.
        /// </summary>
        event EventHandler Faulted;
        
        /// <summary>
        /// Occurs when the component completes its transition from the starting state into the started state.
        /// </summary>
        event EventHandler Started;
        
        /// <summary>
        /// Occurs when the component first enters the starting state.
        /// </summary>
        event EventHandler Starting;

        /// <summary>
        /// Gets the current state of the component.
        /// </summary>
        ComponentState State
        {
            get;
        }
        
        /// <summary>
        /// Causes a component to transition immediately from its current state into the stopped state.
        /// </summary>
        void Abort();
        
        /// <summary>
        /// Causes a component to transition from its current state into the stopped state.
        /// </summary>
        void Stop();
        
        /// <summary>
        /// Causes a component to transition from the created state into the started state.
        /// </summary>
        void Start();
    }
}
