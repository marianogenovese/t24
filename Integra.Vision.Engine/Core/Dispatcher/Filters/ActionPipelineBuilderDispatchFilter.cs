//-----------------------------------------------------------------------
// <copyright file="ActionPipelineBuilderDispatchFilter.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Integra.Vision.Engine.Commands;
    
    /// <summary>
    /// Implements the logic of building new actions pipeline in which receive a command and return a action pipeline factory as result of the build.
    /// </summary>
    internal sealed class ActionPipelineBuilderDispatchFilter : DispatchFilter<ICompileFilterContext, IEnumerable<ICommandActionExecutionContext>>
    {
        /*
         * Aqui se deberia usar un cache tipo diccionario para mantener las instancias de los contructores de pipelines por tipo de comando
         */

        /// <summary>
        /// The pipeline factory cache
        /// </summary>
        private static ConcurrentDictionary<CommandTypeEnum, CommandActionFactory> builderCache = new ConcurrentDictionary<CommandTypeEnum, CommandActionFactory>();

        /// <summary>
        /// Private static of the action pipeline factory builder.
        /// </summary>
        private static CommandActionPipelineFactoryBuilder actionPipelineFactoryBuilder = new CommandActionPipelineFactoryBuilder();

        /*
         * Este filtro implementa el paso de recibir comandos (CompileFilterContext) y basado en la información del comando
         * crear nuevos pipelines que permitan ejecutar acciones especificas para cada comando.
         */

        /// <inheritdoc />
        public override IEnumerable<ICommandActionExecutionContext> Execute(ICompileFilterContext context)
        {
            /*
             * Aqui se deben tomar los comandos y buscar la fabrica correspondiente. Revisar en el app.config.
             */
            ICommandActionExecutionContext actionContext = context as ICommandActionExecutionContext;

            foreach (CommandBase command in context.Commands)
            {
                actionContext.ActionPipelineFactory = builderCache.GetOrAdd(command.Type, _ => actionPipelineFactoryBuilder.Build(command.Type));                
                yield return actionContext;
            }
            
            /*
            // Se usa un dictionario concurrente pues este metodo puede ser llamado concurrentemente y ademas el diccionario es estatico.
            for (int i = 0; i <= 4; i++)
            {
                CommandActionFactory factory = null;
                switch (i)
                {
                    case 0:
                        factory = builderCache.GetOrAdd(CommandTypeEnum.CreateAssembly, _ => actionPipelineFactoryBuilder.Build(CommandTypeEnum.CreateAssembly));
                        break;
                    case 1:
                        factory = builderCache.GetOrAdd(CommandTypeEnum.CreateAdapter, _ => actionPipelineFactoryBuilder.Build(CommandTypeEnum.CreateAdapter));
                        break;
                    case 2:
                        factory = builderCache.GetOrAdd(CommandTypeEnum.CreateSource, _ => actionPipelineFactoryBuilder.Build(CommandTypeEnum.CreateSource));
                        break;
                    case 3:
                        factory = builderCache.GetOrAdd(CommandTypeEnum.CreateStream, _ => actionPipelineFactoryBuilder.Build(CommandTypeEnum.CreateStream));
                        break;
                    case 4:
                        factory = builderCache.GetOrAdd(CommandTypeEnum.CreateTrigger, _ => actionPipelineFactoryBuilder.Build(CommandTypeEnum.CreateTrigger));
                        break;
                }
                
                yield return factory;
            }
            */
        }
    }
}
