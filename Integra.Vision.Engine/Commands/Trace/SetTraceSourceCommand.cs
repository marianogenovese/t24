//-----------------------------------------------------------------------
// <copyright file="SetTraceSourceCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Class for trace a source.
    /// </summary>
    internal sealed class SetTraceSourceCommand : SetTraceCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetTraceSourceCommand"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public SetTraceSourceCommand(PlanNode node)
            : base(node)
        {
        }

        /// <inheritdoc />
        public override CommandTypeEnum Type
        {
            get 
            {
                return CommandTypeEnum.SetTraceSource;
            }
        }

        /// <summary>
        /// Gets the object name or object family
        /// </summary>
        public ObjectTypeEnum FamilyName
        {
            get
            {
                return (ObjectTypeEnum)this.Arguments["FamilyName"].Value;
            }
        }
    }
}
