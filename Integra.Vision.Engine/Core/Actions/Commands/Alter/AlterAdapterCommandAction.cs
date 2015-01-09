//-----------------------------------------------------------------------
// <copyright file="AlterAdapterCommandAction.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;
    using Integra.Vision.Engine.Database.Repositories;

    /// <summary>
    /// Implements all the process of alter an adapter.
    /// </summary>
    internal sealed class AlterAdapterCommandAction : ExecutionCommandAction
    {
        /// <inheritdoc />
        protected override CommandActionResult OnExecuteCommand(CommandBase command)
        {
            try
            {
                // Initialize the context
                ViewsContext context = new ViewsContext("EngineDatabase");

                this.UpdateObject(context, command as AlterAdapterCommand);

                // return the result
                return new QueryCommandResult();
            }
            catch (Exception e)
            {
                return new ErrorCommandResult(e);
            }
        }

        /// <summary>
        /// Contains alter adapter logic.
        /// </summary>
        /// <param name="vc">Current context</param>
        /// <param name="command">Alter adapter command</param>
        private void UpdateObject(ViewsContext vc, AlterAdapterCommand command)
        {
            // create repository
            Repository<Database.Models.Adapter> repoAdapter = new Repository<Database.Models.Adapter>(vc);
            Repository<Database.Models.Arg> repoArg = new Repository<Database.Models.Arg>(vc);
            Repository<Database.Models.Dependency> repoDependency = new Repository<Database.Models.Dependency>(vc);
            Repository<Database.Models.Assembly> repoAssembly = new Repository<Database.Models.Assembly>(vc);

            // get the adapter
            Database.Models.Adapter adapter = repoAdapter.Find(x => x.Name == command.Name);

            // delete the dependencies
            repoDependency.Delete(x => x.PrincipalObjectId == adapter.Id);

            // delete the adapter internal arguments
            repoArg.Delete(x => x.AdapterId == adapter.Id);
            
            // get the assembly
            Database.Models.Assembly assembly = repoAssembly.Find(x => x.Name == command.AssemblyName);
            
            // update the adapter arguments
            adapter.CreationDate = DateTime.Now;
            adapter.IsSystemObject = false;
            adapter.Type = ObjectTypeEnum.Adapter.ToString();
            adapter.State = (int)UserDefinedObjectStateEnum.Stopped;
            adapter.AdapterType = (int)command.AdapterType;
            adapter.AssemblyId = assembly.Id;

            // update the adapter
            repoAdapter.Update(adapter);

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

            // close connection
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
