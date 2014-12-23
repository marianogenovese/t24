//-----------------------------------------------------------------------
// <copyright file="AlterObjectCommandBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Base class for alter object command
    /// </summary>
    /// <typeparam name="TCreate">Create command class</typeparam>
    /// <typeparam name="TDrop">Drop command class</typeparam>
    internal abstract class AlterObjectCommandBase<TCreate, TDrop> : ReferenceableObjectCommandBase
        where TCreate : CreateObjectCommandBase
        where TDrop : DropObjectCommandBase
    {
        /// <summary>
        /// create object command
        /// </summary>
        private TCreate createCommand = null;
        
        /// <summary>
        /// drop object command
        /// </summary>
        private TDrop dropCommand = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlterObjectCommandBase{TCreate,TDrop}"/> class
        /// </summary>
        /// <param name="commandType">Indicate what type of command is</param>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public AlterObjectCommandBase(CommandTypeEnum commandType, string commandText, ISecurityContext securityContext) : base(commandType, commandText, securityContext)
        {
        }

        /// <summary>
        /// Gets the create object command
        /// </summary>
        protected virtual TCreate CreateCommand
        {
            get
            {
                return this.createCommand;
            }
        }

        /// <summary>
        /// Gets the drop object command
        /// </summary>
        protected virtual TDrop DropCommand
        {
            get
            {
                return this.dropCommand;
            }
        }
    }
}
