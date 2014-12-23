//-----------------------------------------------------------------------
// <copyright file="ActionPipelineInvokerDispatchFilter.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implements the logic of invoking the actions pipeline in which receive a action pipeline and return a action result as result of the execution.
    /// </summary>
    internal sealed class ActionPipelineInvokerDispatchFilter : DispatchFilter<IEnumerable<ICommandActionExecutionContext>, IEnumerable<ICommandActionExecutionContext>>
    {
        /*
         * Este filtro implementa el paso de recibir contextos de ejecución de comandos (CommandActionDispatchContext)
         * y ejecutar los pipelines relacionados al contexto.
         */

        /// <inheritdoc />
        public override IEnumerable<ICommandActionExecutionContext> Execute(IEnumerable<ICommandActionExecutionContext> contexts)
        {
            /*
             * Aqui se controla la ejecución del pipeline para comando. Si existe algun error en la ejecución de la acción, 
             * se crea un UnhandledExceptionResult contiendo la excepción.
             */
            foreach (ICommandActionExecutionContext context in contexts)
            {
                DispatchContext dispatchContext = context as DispatchContext;
                CommandExecutingContext actionContext = new CommandExecutingContext(dispatchContext);
                try
                {
                    context.ActionPipeline.Execute(actionContext);
                }
                catch (Exception e)
                {
                    actionContext.Result = new ErrorCommandResult(e);
                }

                // Se asigna el resultado al contexto de despacho y se retorna el contexto, en esta propiedad
                // siempre estará el ultimo resultado.
                dispatchContext.Result = actionContext.Result;
                yield return dispatchContext;
            }
        }
    }
}
