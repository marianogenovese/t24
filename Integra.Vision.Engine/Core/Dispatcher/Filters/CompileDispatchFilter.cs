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
                    // CREATE
                    case PlanNodeTypeEnum.CreateAssembly:
                        command = new CreateAssemblyCommand(node);
                        break;
                    case PlanNodeTypeEnum.CreateAdapter:
                        command = new CreateAdapterCommand(node);
                        break;
                    case PlanNodeTypeEnum.CreateSource:
                        command = new CreateSourceCommand(node);
                        break;
                    case PlanNodeTypeEnum.CreateStream:
                        command = new CreateStreamCommand(node);
                        break;
                    case PlanNodeTypeEnum.CreateTrigger:
                        command = new CreateTriggerCommand(node);
                        break;
                    case PlanNodeTypeEnum.CreateUser:
                        command = new CreateUserCommand(node);
                        break;
                    case PlanNodeTypeEnum.CreateRole:
                        command = new CreateRoleCommand(node);
                        break;

                    // DROP
                    case PlanNodeTypeEnum.DropAssembly:
                        command = new DropAssemblyCommand(node);
                        break;
                    case PlanNodeTypeEnum.DropAdapter:
                        command = new DropAdapterCommand(node);
                        break;
                    case PlanNodeTypeEnum.DropSource:
                        command = new DropSourceCommand(node);
                        break;
                    case PlanNodeTypeEnum.DropStream:
                        command = new DropStreamCommand(node);
                        break;
                    case PlanNodeTypeEnum.DropTrigger:
                        command = new DropTriggerCommand(node);
                        break;
                    case PlanNodeTypeEnum.DropUser:
                        command = new DropUserCommand(node);
                        break;
                    case PlanNodeTypeEnum.DropRole:
                        command = new DropRoleCommand(node);
                        break;

                    // ALTER
                    case PlanNodeTypeEnum.AlterAdapter:
                        break;
                    case PlanNodeTypeEnum.AlterSource:
                        break;
                    case PlanNodeTypeEnum.AlterStream:
                        break;
                    case PlanNodeTypeEnum.AlterTrigger:
                        break;
                    case PlanNodeTypeEnum.AlterUser:
                        break;

                    // PERMISSION
                    case PlanNodeTypeEnum.Grant:
                        command = new GrantPermissionCommand(node);
                        break;
                    case PlanNodeTypeEnum.Deny:
                        command = new DenyPermissionCommand(node);
                        break;
                    case PlanNodeTypeEnum.Revoke:
                        command = new RevokePermissionCommand(node);
                        break;

                    // START
                    case PlanNodeTypeEnum.StartAdapter:
                        break;
                    case PlanNodeTypeEnum.StartSource:
                        break;
                    case PlanNodeTypeEnum.StartStream:
                        break;
                    case PlanNodeTypeEnum.StartTrigger:
                        break;

                    // STOP
                    case PlanNodeTypeEnum.StopAdapter:
                        break;
                    case PlanNodeTypeEnum.StopSource:
                        break;
                    case PlanNodeTypeEnum.StopStream:
                        break;
                    case PlanNodeTypeEnum.StopTrigger:
                        break;

                    // TRACE
                    case PlanNodeTypeEnum.SetTraceAdapter:
                        break;
                    case PlanNodeTypeEnum.SetTraceEngine:
                        break;
                    case PlanNodeTypeEnum.SpecificObject:
                        break;
                    case PlanNodeTypeEnum.SetTraceSource:
                        break;
                    case PlanNodeTypeEnum.SetTraceStream:
                        break;
                    case PlanNodeTypeEnum.SetTraceTrigger:
                        break;

                    // QUERY
                    case PlanNodeTypeEnum.SystemQuery:
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
