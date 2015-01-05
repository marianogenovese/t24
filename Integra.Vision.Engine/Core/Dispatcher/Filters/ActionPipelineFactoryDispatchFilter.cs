//-----------------------------------------------------------------------
// <copyright file="ActionPipelineFactoryDispatchFilter.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Collections.Generic;
    
    /// <summary>
    /// Implements the logic of create new actions pipeline in which receive a action pipeline factory and return a action pipeline as result of the creation.
    /// </summary>
    internal sealed class ActionPipelineFactoryDispatchFilter : DispatchFilter<IEnumerable<ICommandActionExecutionContext>, IEnumerable<ICommandActionExecutionContext>>
    {
        /*
         * Este filtro implementa el paso de recibir contextos de ejecución de comandos (CommandActionDispatchContext) e inicializa las mismas.
         * Este filtro queda abierto para que se puedan injectar dependencias o personalizar la creación del pipeline para el comando.
         */

        /// <inheritdoc />
        public override IEnumerable<ICommandActionExecutionContext> Execute(IEnumerable<ICommandActionExecutionContext> contexts)
        {
            foreach (ICommandActionExecutionContext context in contexts)
            {
                context.ActionPipeline = context.CreateActionPipeline();
                yield return context;
            }
        }
    }
}
