//-----------------------------------------------------------------------
// <copyright file="BeginDispatchFilter.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Collections.Generic;
    
    /// <summary>
    /// Represents the first step in a dispatch pipeline
    /// </summary>
    internal sealed class BeginDispatchFilter : DispatchFilter<OperationContext, DispatchContext>
    {
        /*
         * Este es el filtro que se ejecuta al inicio que cada procesamiento de los requerimientos,
         * aqui se puede poner lógica común para todos.
         */

        /// <inheritdoc />
        public override DispatchContext Execute(OperationContext context)
        {
            return new DispatchContext(context);
        }
    }
}
