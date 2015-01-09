//-----------------------------------------------------------------------
// <copyright file="CreateAdapterCommandAction.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Repositories;

    /// <summary>
    /// Implements all the process of create a new adapter.
    /// </summary>
    internal class CreateAdapterCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                // Initialize the context
                Integra.Vision.Engine.Database.Contexts.ViewsContext context = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

                // save the arguments
                this.SaveArguments(context, command as CreateAdapterCommand);

                // return the result
                return new QueryCommandResult();
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Save the command arguments.
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Create adapter command</param>
        protected void SaveArguments(Integra.Vision.Engine.Database.Contexts.ViewsContext vc, CreateAdapterCommand command)
        {
            Repository<Database.Models.Assembly> repoAssembly = new Repository<Database.Models.Assembly>(vc);
            string assemblyName = command.AssemblyName;
            Database.Models.Assembly assembly = repoAssembly.Find(x => x.Name == assemblyName);

            Repository<Database.Models.Adapter> repoAdapter = new Repository<Database.Models.Adapter>(vc);
            Database.Models.Adapter adapter = new Database.Models.Adapter() { CreationDate = DateTime.Now, IsSystemObject = false, Type = ObjectTypeEnum.Adapter.ToString(), State = (int)UserDefinedObjectStateEnum.Stopped, Name = command.Name, AdapterType = (int)command.AdapterType, AssemblyId = assembly.Id };
            repoAdapter.Create(adapter);

            Repository<Database.Models.Arg> repoArg = new Repository<Database.Models.Arg>(vc);
            foreach (Integra.Vision.Language.PlanNode planNode in command.Parameters)
            {
                Database.Models.Arg arg = new Database.Models.Arg() { AdapterId = adapter.Id, Type = this.GetParameterDataType(planNode.Children[0].Properties["Value"].ToString()), Name = planNode.Children[1].Properties["Value"].ToString(), Value = this.GetBytes(planNode.Children[2].Properties["Value"].ToString()) };
                repoArg.Create(arg);
            }

            // save dependencies of the adapter
            DependencyActions dependencyAction = new DependencyActions(vc, command.Dependencies);
            dependencyAction.SaveDependencies(adapter);

            // save changes
            vc.SaveChanges();

            // close connections
            vc.Dispose();
        }

        /// <summary>
        /// Gets the parameter data type from the enumerator Data Type Argument Enumerator
        /// </summary>
        /// <param name="name">parameter name</param>
        /// <returns>data type value</returns>
        private int GetParameterDataType(string name)
        {
            name = name.ToLower();

            switch (name)
            {
                case "string":
                    return (int)ArgDataTypeEnum.String;
                case "bool":
                    return (int)ArgDataTypeEnum.Bool;
                case "char":
                    return (int)ArgDataTypeEnum.Char;
                case "byte":
                    return (int)ArgDataTypeEnum.Byte;
                case "sbyte":
                    return (int)ArgDataTypeEnum.SByte;
                case "short":
                    return (int)ArgDataTypeEnum.Short;
                case "ushort":
                    return (int)ArgDataTypeEnum.UShort;
                case "int":
                    return (int)ArgDataTypeEnum.Int;
                case "uint":
                    return (int)ArgDataTypeEnum.UInt;
                case "long":
                    return (int)ArgDataTypeEnum.Long;
                case "ulong":
                    return (int)ArgDataTypeEnum.ULong;
                case "float":
                    return (int)ArgDataTypeEnum.Float;
                case "double":
                    return (int)ArgDataTypeEnum.Double;
                case "datetime":
                    return (int)ArgDataTypeEnum.DateTime;
                case "object":
                    return (int)ArgDataTypeEnum.Object;
                case "decimal":
                    return (int)ArgDataTypeEnum.Decimal;
                default:
                    return (int)ArgDataTypeEnum.UserDefinedDataType;
            }
        }

        /// <summary>
        /// Gets the byte array of a string
        /// </summary>
        /// <param name="str">string to convert</param>
        /// <returns>byte array</returns>
        private byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
