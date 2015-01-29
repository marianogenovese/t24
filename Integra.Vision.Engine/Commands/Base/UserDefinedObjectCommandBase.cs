//-----------------------------------------------------------------------
// <copyright file="UserDefinedObjectCommandBase.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Base class for user defined object commands
    /// </summary>
    internal abstract class UserDefinedObjectCommandBase : CommandBase
    {
        /// <summary>
        /// Name of user defined object
        /// </summary>
        private string name = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDefinedObjectCommandBase"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public UserDefinedObjectCommandBase(PlanNode node)
            : base(node)
        {
        }

        /// <summary>
        /// Gets the name of user defined object in current command
        /// </summary>
        public string Name
        {
            get
            {
                if (this.name == null)
                {
                    this.name = this.Arguments["Name"].Value.ToString();
                }

                return this.name;
            }
        }
    }
}
