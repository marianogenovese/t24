//-----------------------------------------------------------------------
// <copyright file="CreateAdapterCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Create.CreateAdapter
{
    using System;
    using Integra.Vision.Engine.Database.Repositories;
    using Integra.Vision.Language;
 
    /// <summary>
    /// Base class for create adapters
    /// </summary>
    internal class CreateAdapterCommand : CreateObjectCommandBase
    {
        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new CreateAdapterArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new CreateAdapterDependencyEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAdapterCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public CreateAdapterCommand(PlanNode node) : base(node)
        {
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get
            {
                return CommandTypeEnum.CreateAdapter;
            }
        }

        /// <summary>
        /// Gets command dependency enumerator
        /// </summary>
        protected override IDependencyEnumerator DependencyEnumerator
        {
            get
            {
                return this.dependencyEnumerator;
            }
        }

        /// <summary>
        /// Gets command argument enumerator
        /// </summary>
        protected override IArgumentEnumerator ArgumentEnumerator
        {
            get
            {
                return this.argumentEnumerator;
            }
        }
        
        /// <summary>
        /// save the command arguments
        /// </summary>
        public virtual void SaveArguments()
        {
            // Initialize the context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            int adapterType = -1;
            if (this.Arguments["Type"].Value.ToString().ToLower().Equals(AdapterTypeEnum.Input.ToString().ToLower()))
            {
                adapterType = (int)AdapterTypeEnum.Input;
            }
            else if (this.Arguments["Type"].Value.ToString().ToLower().Equals(AdapterTypeEnum.Output.ToString().ToLower()))
            {
                adapterType = (int)AdapterTypeEnum.Output;
            }

            Repository<Database.Models.Assembly> repoAssembly = new Repository<Database.Models.Assembly>(vc);
            string assemblyName = this.Arguments["AssemblyName"].Value.ToString();
            Database.Models.Assembly assembly = repoAssembly.Find(x => x.Name == assemblyName);

            if (assembly == null)
            {
                throw new Integra.Vision.Engine.Exceptions.NonExistentObjectException("The assembly '" + assemblyName + "' does not exist");
            }

            Repository<Database.Models.Adapter> repoAdapter = new Repository<Database.Models.Adapter>(vc);
            Database.Models.Adapter adapter = new Database.Models.Adapter() { CreationDate = DateTime.Now, IsSystemObject = false, Type = ObjectTypeEnum.Adapter.ToString(), State = (int)UserDefinedObjectStateEnum.Stopped, Name = this.Arguments["Name"].Value.ToString(), AdapterType = adapterType, AssemblyId = assembly.Id };
            repoAdapter.Create(adapter);
            repoAdapter.Commit();

            Repository<Database.Models.Arg> repoArg = new Repository<Database.Models.Arg>(vc);
            System.Collections.Generic.List<object> parametersList = this.Arguments["Parameters"].Value as System.Collections.Generic.List<object>;
            foreach (Integra.Vision.Language.PlanNode planNode in parametersList)
            {
                Database.Models.Arg arg = new Database.Models.Arg() { AdapterId = adapter.Id, Type = this.GetParameterDataType(planNode.Children[0].Properties["Value"].ToString()), Name = planNode.Children[1].Properties["Value"].ToString(), Value = this.GetBytes(planNode.Children[2].Properties["Value"].ToString()) };
                repoArg.Create(arg);
            }

            repoArg.Commit();

            // close connections
            repoAdapter.Dispose();
            repoArg.Dispose();
            repoAssembly.Dispose();
            vc.Dispose();

            // save dependencies of the adapter
            this.SaveDependencies(adapter);
        }

        /// <summary>
        /// Contains create adapter logic
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            // save arguments
            this.SaveArguments();
        }

        /// <summary>
        /// save the dependencies of the actual object
        /// </summary>
        /// <param name="adapter">the actual adapter</param>
        private void SaveDependencies(Database.Models.Adapter adapter)
        {
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            Repository<Database.Models.Dependency> repoDependency = new Repository<Database.Models.Dependency>(vc);
            Repository<Database.Models.Assembly> repoAssembly = new Repository<Database.Models.Assembly>(vc);

            foreach (var adapterDependency in this.Dependencies)
            {
                Database.Models.Assembly assembly = repoAssembly.Find(x => x.Name == adapterDependency.Name);

                if (assembly == null)
                {
                    throw new Integra.Vision.Engine.Exceptions.NonExistentObjectException("The assembly '" + adapterDependency.Name + "' does not exist");
                }

                Database.Models.Dependency dependency = new Database.Models.Dependency() { DependencyObjectId = assembly.Id, PrincipalObjectId = adapter.Id };
                repoDependency.Create(dependency);
                repoDependency.Commit();
            }

            repoDependency.Dispose();
            repoAssembly.Dispose();
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
