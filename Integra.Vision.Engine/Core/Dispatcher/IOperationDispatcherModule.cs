//-----------------------------------------------------------------------
// <copyright file="IOperationDispatcherModule.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    /// <summary>
    /// This class allow to implement a command dispatcher module for the Integra Vision Engine system.
    /// </summary>
    internal interface IOperationDispatcherModule : IModule
    {
        /// <summary>
        /// Allow to implement operation dispatching.
        /// </summary>
        /// <param name="context">The context of the operation to dispatch.</param>
        /// <returns>A operator used for run the dispatch action.</returns>
        DispatchOperator Dispatch(OperationContext context);
    }
}
