//-----------------------------------------------------------------------
// <copyright file="AlterObjectCommandBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using System;
    using System.Reflection;
    using Integra.Vision.Language;

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
        /// <param name="node">Execution plan node that have the command arguments</param>
        public AlterObjectCommandBase(PlanNode node) : base(node)
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
