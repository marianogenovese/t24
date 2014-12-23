//-----------------------------------------------------------------------
// <copyright file="ObjectToStartOrStopNode.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.ASTNodes.Commands.StartStop
{
    using Integra.Vision.Language.ASTNodes.Base;
    using Irony.Ast;
    using Irony.Interpreter;
    using Irony.Parsing;

    /// <summary>
    /// ObjectToStartOrStopNode class
    /// </summary>
    internal sealed class ObjectToStartOrStopNode : AstNodeBase
    {
        /// <summary>
        /// user defined object: adapter, stream, source, trigger
        /// </summary>
        private string userDefinedObject;

        /// <summary>
        /// First method called
        /// </summary>
        /// <param name="context">Contains the actual context</param>
        /// <param name="treeNode">Contains the tree of the context</param>
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            this.userDefinedObject = (string)ChildrenNodes[0].Token.Value;
        }

        /// <summary>
        /// DoEvaluate
        /// Doc go here
        /// </summary>
        /// <param name="thread">Thread of the evaluated grammar</param>
        /// <returns>return a plan node</returns>
        protected override object DoEvaluate(ScriptThread thread)
        {
            return this.userDefinedObject;
        }
    }
}
