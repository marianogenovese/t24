//-----------------------------------------------------------------------
// <copyright file="CreateObjectCommandBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for create commands of user defined object
    /// </summary>
    internal abstract class CreateObjectCommandBase : UserDefinedObjectCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateObjectCommandBase"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public CreateObjectCommandBase(PlanNode node)
            : base(node)
        {
        }

        /// <summary>
        /// Gets the script text of the create command
        /// </summary>
        public string Script
        {
            get
            {
                return this.Arguments["Script"].Value.ToString();
            }
        }

        /// <summary>
        /// Gets a value indicating whether
        /// </summary>
        public bool IsBootObject
        {
            get
            {
                return (bool)this.Arguments["IsBootObject"].Value;
            }
        }
    }
}