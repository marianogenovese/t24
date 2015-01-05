//-----------------------------------------------------------------------
// <copyright file="CompileDispatchFilter.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System.Collections.Generic;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Language;
    
    /// <summary>
    /// Implements the logic of compiling in which receive a node and return a command as result of the compiling.
    /// </summary>
    internal sealed class CompileDispatchFilter : DispatchFilter<IParseFilterContext, ICompileFilterContext>
    {
        /*
         * Este filtro implementa el paso de transformar los nodos que contiene el contexto de parseo y
         * con esos nodos construir los comandos que se intentan ejecutar en el script.
         */

        /// <inheritdoc />
        public override ICompileFilterContext Execute(IParseFilterContext context)
        {
            ICompileFilterContext compileContext = context as ICompileFilterContext;

            /*
             * Aqui se debe tomar los nodos y convertirlos a comandos
             * en la clase DispatchContext agregar un elemento al diccionario de Data con los comandos.
             */

            List<CommandBase> commands = new List<CommandBase>();

            foreach (PlanNode node in context.Nodes)
            {
                PlanNodeTypeEnum nodeType = node.NodeType;
                CommandBase command = null;
                switch (nodeType)
                {
                    case PlanNodeTypeEnum.CreateAssembly:
                        command = new CreateAssemblyCommand(node);
                        break;
                    case PlanNodeTypeEnum.CreateAdapter:
                        break;
                }

                if (command != null)
                {
                    commands.Add(command);
                }
            }

            compileContext.Commands = commands.ToArray();
            return compileContext;
        }
    }
}
