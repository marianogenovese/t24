//-----------------------------------------------------------------------
// <copyright file="IOperationSchedulerModule.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Threading.Tasks;
    
    /// <summary>
    /// This class allow to implement a queue module for scheduling a request.
    /// </summary>
    internal interface IOperationSchedulerModule : IModule
    {
        /// <summary>
        /// Allow to schedule a execution command request.
        /// </summary>
        /// <param name="context">The context created for request.</param>
        void Schedule(OperationContext context);
    }
}
