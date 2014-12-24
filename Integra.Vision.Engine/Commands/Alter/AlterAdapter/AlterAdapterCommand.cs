//-----------------------------------------------------------------------
// <copyright file="AlterAdapterCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Alter.AlterAdapter
{
    using System;
    using Integra.Vision.Engine.Commands.Create.CreateAdapter;
    using Integra.Vision.Engine.Commands.Drop.DropAdapter;
    using Integra.Vision.Engine.Database.Repositories;
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for modify adapters
    /// </summary>
    internal sealed class AlterAdapterCommand : AlterObjectCommandBase<CreateAdapterCommand, DropAdapterCommand>
    {
        /// <summary>
        /// Execution plan node
        /// </summary>
        private readonly PlanNode node;
        
        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new AlterAdapterArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new AlterAdapterDependencyEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="AlterAdapterCommand"/> class
        /// </summary>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public AlterAdapterCommand(PlanNode node) : base(node)
        {
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
        /// Contains alter adapter logic
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();
            
            // drop the especified adapter
            DropObject dropAdapter = new DropObject(this.Arguments, this.Dependencies);
            dropAdapter.Execute();

            // create the new adapter
            CreateObject createAdapter = new CreateObject(this.Arguments, this.Dependencies);
            createAdapter.Execute();
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

        /// <summary>
        /// class for create an adapter without chain execution
        /// </summary>
        private class CreateObject : CreateAdapterCommand
        {
            /// <summary>
            /// Argument enumerator implementation for this command
            /// </summary>
            private IReadOnlyNamedElementCollection<CommandArgument> arguments;

            /// <summary>
            /// Dependency enumerator implementation for this command
            /// </summary>
            private IReadOnlyNamedElementCollection<CommandDependency> dependencies;

            /// <summary>
            /// Initializes a new instance of the <see cref="CreateObject"/> class
            /// </summary>
            /// <param name="arguments">alter command arguments</param>
            /// /// <param name="dependencies">alter command dependencies</param>
            public CreateObject(IReadOnlyNamedElementCollection<CommandArgument> arguments, IReadOnlyNamedElementCollection<CommandDependency> dependencies)
                : base(null)
            {
                this.arguments = arguments;
                this.dependencies = dependencies;
            }

            /// <summary>
            /// Gets the alter command arguments passed to this class
            /// </summary>
            protected override IReadOnlyNamedElementCollection<CommandArgument> Arguments
            {
                get
                {
                    return this.arguments;
                }
            }

            /// <summary>
            /// Gets the alter command dependencies passed to this class
            /// </summary>
            protected override IReadOnlyNamedElementCollection<CommandDependency> Dependencies
            {
                get
                {
                    return this.dependencies;
                }
            }

            /// <summary>
            /// Save command arguments
            /// </summary>
            protected override void OnExecute()
            {
                this.SaveArguments();
            }
        }

        /// <summary>
        /// class for drop an adapter without chain execution
        /// </summary>
        private class DropObject : DropAdapterCommand
        {
            /// <summary>
            /// Argument enumerator implementation for this command
            /// </summary>
            private IReadOnlyNamedElementCollection<CommandArgument> arguments;

            /// <summary>
            /// Dependency enumerator implementation for this command
            /// </summary>
            private IReadOnlyNamedElementCollection<CommandDependency> dependencies;

            /// <summary>
            /// Initializes a new instance of the <see cref="DropObject"/> class
            /// </summary>
            /// <param name="arguments">alter command arguments</param>
            /// /// <param name="dependencies">alter command dependencies</param>
            public DropObject(IReadOnlyNamedElementCollection<CommandArgument> arguments, IReadOnlyNamedElementCollection<CommandDependency> dependencies)
                : base(null)
            {
                this.arguments = arguments;
                this.dependencies = dependencies;
            }

            /// <summary>
            /// Gets the alter command arguments passed to this class
            /// </summary>
            protected override IReadOnlyNamedElementCollection<CommandArgument> Arguments
            {
                get
                {
                    return this.arguments;
                }
            }

            /// <summary>
            /// Gets the alter command dependencies passed to this class
            /// </summary>
            protected override IReadOnlyNamedElementCollection<CommandDependency> Dependencies
            {
                get
                {
                    return this.dependencies;
                }
            }

            /// <summary>
            /// Save command arguments
            /// </summary>
            protected override void OnExecute()
            {
                this.DeleteArguments();
            }
        }
    }
}
