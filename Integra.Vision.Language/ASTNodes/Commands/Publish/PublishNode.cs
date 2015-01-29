//-----------------------------------------------------------------------
// <copyright file="PublishNode.cs" company="Ingetra.Vision.Language">
//     Copyright (c) Ingetra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.ASTNodes.Commands.Publish
{
    using Integra.Vision.Language.ASTNodes.Base;
    using Irony.Ast;
    using Irony.Interpreter;
    using Irony.Interpreter.Ast;
    using Irony.Parsing;

    /// <summary>
    /// Publish command node.
    /// </summary>
    internal sealed class PublishNode : AstNodeBase
    {
        /// <summary>
        /// Reserved word 'publish'
        /// </summary>
        private string publishWord;

        /// <summary>
        /// Reserved word 'event'
        /// </summary>
        private string eventWord;

        /// <summary>
        /// Reserved word 'to'
        /// </summary>
        private string toWord;

        /// <summary>
        /// Source name
        /// </summary>
        private AstNodeBase sourceName;

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

            this.publishWord = (string)ChildrenNodes[0].Token.Value;
            this.eventWord = (string)ChildrenNodes[1].Token.Value;
            this.toWord = (string)ChildrenNodes[2].Token.Value;
            this.sourceName = AddChild(NodeUseType.Parameter, SR.SourceRole, ChildrenNodes[3]) as AstNodeBase;

            this.result = new PlanNode();
            this.result.Column = ChildrenNodes[0].Token.Location.Column;
            this.result.Line = ChildrenNodes[0].Token.Location.Line;
            this.result.NodeType = PlanNodeTypeEnum.Publish;
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
            PlanNode sourceNameAux = (PlanNode)this.sourceName.Evaluate(thread);
            this.EndEvaluate(thread);

            this.result.NodeText = string.Format("{0} {1} {2} {3}", this.publishWord, this.eventWord, this.toWord, sourceNameAux.NodeText);

            this.result.Children = new System.Collections.Generic.List<PlanNode>();
            this.result.Children.Add(sourceNameAux);

            return this.result;
        }
    }
}
