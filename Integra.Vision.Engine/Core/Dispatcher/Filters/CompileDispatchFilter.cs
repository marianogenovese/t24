﻿//-----------------------------------------------------------------------
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
            DispatchContext dispatchContext = context as DispatchContext;

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
                    case PlanNodeTypeEnum.AlterAssembly:
                        command = new AlterAssemblyCommand(node);
                        break;
                    case PlanNodeTypeEnum.AlterAdapter:
                        command = new AlterAdapterCommand(node);
                        break;
                    case PlanNodeTypeEnum.AlterSource:
                        command = new AlterSourceCommand(node);
                        break;
                    case PlanNodeTypeEnum.AlterStream:
                        command = new AlterStreamCommand(node);
                        break;
                    case PlanNodeTypeEnum.AlterTrigger:
                        command = new AlterTriggerCommand(node);
                        break;
                    case PlanNodeTypeEnum.AlterUser:
                        command = new AlterUserCommand(node);
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
                        command = new StartAdapterCommand(node);
                        break;
                    case PlanNodeTypeEnum.StartSource:
                        command = new StartSourceCommand(node);
                        break;
                    case PlanNodeTypeEnum.StartStream:
                        command = new StartStreamCommand(node);
                        break;
                    case PlanNodeTypeEnum.StartTrigger:
                        command = new StartTriggerCommand(node);
                        break;

                    // STOP
                    case PlanNodeTypeEnum.StopAdapter:
                        command = new StopAdapterCommand(node);
                        break;
                    case PlanNodeTypeEnum.StopSource:
                        command = new StopSourceCommand(node);
                        break;
                    case PlanNodeTypeEnum.StopStream:
                        command = new StopStreamCommand(node);
                        break;
                    case PlanNodeTypeEnum.StopTrigger:
                        command = new StopTriggerCommand(node);
                        break;

                    // TRACE
                    case PlanNodeTypeEnum.SetTraceAdapter:
                        command = new SetTraceAdapterCommand(node);
                        break;
                    case PlanNodeTypeEnum.SetTraceEngine:
                        command = new SetTraceEngineCommand(node);
                        break;
                    case PlanNodeTypeEnum.SetTraceObject:
                        command = new SetTraceObjectCommand(node);
                        break;
                    case PlanNodeTypeEnum.SetTraceSource:
                        command = new SetTraceSourceCommand(node);
                        break;
                    case PlanNodeTypeEnum.SetTraceStream:
                        command = new SetTraceStreamCommand(node);
                        break;
                    case PlanNodeTypeEnum.SetTraceTrigger:
                        command = new SetTraceTriggerCommand(node);
                        break;

                    // QUERY
                    case PlanNodeTypeEnum.SystemQuery:
                        command = new SystemQueriesCommand(node);
                        break;

                    // BOOT ENGINE
                    case PlanNodeTypeEnum.BootEngine:
                        command = new BootEngineCommand(node);
                        break;

                    // LOAD ASSEMBLY
                    case PlanNodeTypeEnum.LoadAssembly:
                        command = new LoadAssemblyCommand(node);
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