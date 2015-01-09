//-----------------------------------------------------------------------
// <copyright file="SetTraceObjectCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Class for trace a stream.
    /// </summary>
    internal sealed class SetTraceObjectCommand : SetTraceCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetTraceObjectCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public SetTraceObjectCommand(PlanNode node)
            : base(node)
        {
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get 
            {
                return CommandTypeEnum.SetTraceObject;
            }
        }

        /// <summary>
        /// Gets the name of the object to trace
        /// </summary>
        public string ObjectName
        {
            get
            {
                return this.Arguments["ObjectName"].Value.ToString();
            }
        }
    }
}
