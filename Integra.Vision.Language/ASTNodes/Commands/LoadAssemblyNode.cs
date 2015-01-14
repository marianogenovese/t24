//-----------------------------------------------------------------------
// <copyright file="LoadAssemblyNode.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.ASTNodes.Commands
{
    using Integra.Vision.Language.ASTNodes.Base;
    using Irony.Ast;
    using Irony.Interpreter;
    using Irony.Interpreter.Ast;
    using Irony.Parsing;

    /// <summary>
    /// Engine actions node
    /// </summary>
    internal sealed class LoadAssemblyNode : AstNodeBase
    {
        /// <summary>
        /// reserved word 'load'
        /// </summary>
        private string loadWord;

        /// <summary>
        /// reserved word 'assembly'
        /// </summary>
        private string assemblyWord;

        /// <summary>
        /// assembly name
        /// </summary>
        private string assemblyName;

        /// <summary>
        /// result plan node
        /// </summary>
        private PlanNode result;

        /// <summary>
        /// First method called
        /// </summary>
        /// <param name="context">Contains the actual context</param>
        /// <param name="treeNode">Contains the tree of the context</param>
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);

            this.loadWord = (string)ChildrenNodes[0].Token.Value;
            this.assemblyWord = (string)ChildrenNodes[1].Token.Value;
            this.assemblyName = (string)ChildrenNodes[2].Token.Value;

            this.result = new PlanNode();
            this.result.Column = ChildrenNodes[0].Token.Location.Column;
            this.result.Line = ChildrenNodes[0].Token.Location.Line;
            this.result.NodeText = this.loadWord + " " + this.assemblyWord + " " + this.assemblyName;
            this.result.NodeType = PlanNodeTypeEnum.LoadAssembly;
        }

        /// <summary>
        /// DoEvaluate
        /// Doc go here
        /// </summary>
        /// <param name="thread">Thread of the evaluated grammar</param>
        /// <returns>return a plan node</returns>
        protected override object DoEvaluate(ScriptThread thread)
        {
            this.BeginEvaluate(thread);
            this.EndEvaluate(thread);

            this.result.Properties.Add("Action", this.loadWord);
            this.result.Properties.Add("ObjectType", this.assemblyName);

            return this.result;
        }
    }
}
