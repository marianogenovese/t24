//-----------------------------------------------------------------------
// <copyright file="EndDispatchFilter.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Collections.Generic;
    
    /// <summary>
    /// Represents the last step in a dispatch pipeline
    /// </summary>
    internal sealed class EndDispatchFilter : DispatchFilter<OperationContext, OperationContext>
    {
        /*
         * Este es el filtro que se ejecuta al final que cada procesamiento de los requerimientos,
         * aqui se puede poner lógica común para todos.
         */

        /// <inheritdoc />
        public override OperationContext Execute(OperationContext input)
        {
            return input;
        }
    }
}
