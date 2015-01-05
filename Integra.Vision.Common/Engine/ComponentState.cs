//-----------------------------------------------------------------------
// <copyright file="ComponentState.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine
{
    /// <summary>
    /// Defines the states in which an Integra.Vision.Engine.IComponent can exist.
    /// </summary>
    internal enum ComponentState
    {
        /// <summary>
        /// Indicates that the system component has been instantiated and is configurable, but not yet open or ready for use.
        /// </summary>
        Created = 0,
        
        /// <summary>
        /// Indicates that the system component is being transitioned from the Integra.Vision.Engine.ComponentState.Created
        /// state to the Integra.Vision.Engine.ComponentState.Starting state.
        /// </summary>
        Starting = 1,
        
        /// <summary>
        /// Indicates that the system component is now started and ready to be used.
        /// </summary>
        Started = 2,
        
        /// <summary>
        /// Indicates that the system component is transitioning to the Integra.Vision.Engine.ComponentState.Stopped state.
        /// </summary>
        Stopping = 3,
        
        /// <summary>
        /// Indicates that the system component has been stopped and is no longer usable.
        /// </summary>
        Stopped = 4,        
        
        /// <summary>
        /// Indicates that the system component has encountered an error or fault from which it cannot recover and from which it is no longer usable.
        /// </summary>
        Faulted = 5
    }
}
