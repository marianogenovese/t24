//-----------------------------------------------------------------------
// <copyright file="ActionResultPipelineDispatchFilter.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Implements the logic of invoking the actions pipeline in which receive a action pipeline and return a action result as result of the execution.
    /// </summary>
    internal sealed class ActionResultPipelineDispatchFilter : DispatchFilter<IEnumerable<ICommandActionExecutionContext>, OperationContext>
    {
        /// <summary>
        /// Context used for execute the action result filters.
        /// </summary>
        private DispatchContext dispatchContext = null;

        /*
         * Este filtro implementa el paso de recibir contextos de ejecución de comandos (CommandActionDispatchContext)
         * y ejecutar los pipelines relacionados al contexto.
         */

        /// <inheritdoc />
        public override OperationContext Execute(IEnumerable<ICommandActionExecutionContext> contexts)
        {
            foreach (ICommandActionExecutionContext context in contexts)
            {
                /*
                 * Se van encadenando los resultados para contruir un pipeline de resultados por cada comando
                 * ejecutado.
                 */
                if (this.dispatchContext == null)
                {
                    this.dispatchContext = context as DispatchContext;
                }

                // Se agrega el resultado.
                this.dispatchContext.Response.Results.Add(context.Result);
            }

            /*
             * El resultado es que se tiene un pipeline con todos los resultados de los comandos ejecutados,
             * y este pipeline se ejecuta para que cada acción pueda retornar información al cliente.
             */
            return this.dispatchContext;
        }
    }
}
