//-----------------------------------------------------------------------
// <copyright file="SendNode.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.ASTNodes.Commands.General
{
    using Integra.Vision.Language.ASTNodes.Base;
    using Integra.Vision.Language.Resources;
    using Irony.Ast;
    using Irony.Interpreter;
    using Irony.Interpreter.Ast;
    using Irony.Parsing;

    /// <summary>
    /// SendNode class
    /// </summary>
    internal sealed class SendNode : AstNodeBase
    {
        /// <summary>
        /// reserved word send
        /// </summary>
        private string sendWord;

        /// <summary>
        /// event node
        /// </summary>
        private AstNodeBase evt;

        /// <summary>
        /// reserved word to
        /// </summary>
        private string toWord;

        /// <summary>
        /// result planNode
        /// </summary>
        private PlanNode result;

        /// <summary>
        /// output adapter name
        /// </summary>
        private AstNodeBase outputAdapterId;

        /// <summary>
        /// First method called
        /// </summary>
        /// <param name="context">Contains the actual context</param>
        /// <param name="treeNode">Contains the tree of the context</param>
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            this.sendWord = (string)ChildrenNodes[0].Token.Value;
            this.evt = AddChild(NodeUseType.Parameter, SR.EventRole, ChildrenNodes[1]) as AstNodeBase;
            this.toWord = (string)ChildrenNodes[2].Token.Value;
            this.outputAdapterId = AddChild(NodeUseType.Parameter, SR.IdentifierRole, ChildrenNodes[3]) as AstNodeBase;

            this.result = new PlanNode();
            this.result.Column = ChildrenNodes[0].Token.Location.Column;
            this.result.Line = ChildrenNodes[0].Token.Location.Line;
            this.result.NodeType = PlanNodeTypeEnum.Send;
        }

        /// <summary>
        /// DoEvaluate
        /// Doc go here ¬¬
        /// </summary>
        /// <param name="thread">Thread of the evaluated grammar</param>
        /// <returns>return a plan node</returns>
        protected override object DoEvaluate(ScriptThread thread)
        {
            this.BeginEvaluate(thread);
            PlanNode eventNodeAux = (PlanNode)this.evt.Evaluate(thread);
            PlanNode outputAdapterIdAux = (PlanNode)this.outputAdapterId.Evaluate(thread);
            this.EndEvaluate(thread);

            this.result.NodeText = SR.SendNodeText(this.sendWord, eventNodeAux.NodeText, this.toWord, outputAdapterIdAux.NodeText);

            this.result.Children = new System.Collections.Generic.List<PlanNode>();
            this.result.Children.Add(eventNodeAux);
            this.result.Children.Add(outputAdapterIdAux);

            return this.result;
        }
    }
}
