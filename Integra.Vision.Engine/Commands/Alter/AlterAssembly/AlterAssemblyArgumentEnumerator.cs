//-----------------------------------------------------------------------
// <copyright file="AlterAssemblyArgumentEnumerator.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands
{
    using Integra.Vision.Language;

    /// <summary>
    /// Contains argument enumerator logic for Create Assembly command
    /// </summary>
    internal sealed class AlterAssemblyArgumentEnumerator : IArgumentEnumerator
    {
        /// <summary>
        /// Execution plan node that have the command arguments
        /// </summary>
        private readonly PlanNode node;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlterAssemblyArgumentEnumerator"/> class
        /// </summary>
        /// <param name="node">Execution plan node that have the command arguments</param>
        public AlterAssemblyArgumentEnumerator(PlanNode node)
        {
            this.node = node;
        }

        /// <summary>
        /// Argument enumeration implementation
        /// </summary>
        /// <param name="command">Command in context used for enumerate their arguments</param>
        /// <returns>Collection of command arguments</returns>
        public CommandArgument[] Enumerate(CommandBase command)
        {
            System.Collections.Generic.List<CommandArgument> arguments = new System.Collections.Generic.List<CommandArgument>();

            try
            {
                arguments.Add(new CommandArgument("Name", this.node.Children[0].Properties["Value"].ToString()));
                arguments.Add(new CommandArgument("LocalPath", this.node.Children[1].Properties["Value"].ToString()));
                return arguments.ToArray();
            }
            catch (System.Exception e)
            {
                throw new ArgumentEnumerationException(Resources.SR.EnumerationException, e);
            }
        }
    }
}
