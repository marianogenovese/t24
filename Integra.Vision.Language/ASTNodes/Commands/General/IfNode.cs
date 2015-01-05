//-----------------------------------------------------------------------
// <copyright file="IfNode.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.ASTNodes.Commands.General
{
    using System.Collections.Generic;
    using Integra.Vision.Language.ASTNodes.Base;
    using Integra.Vision.Language.Resources;
    using Irony.Ast;
    using Irony.Interpreter;
    using Irony.Interpreter.Ast;
    using Irony.Parsing;

    /// <summary>
    /// IfNode class
    /// </summary>
    internal sealed class IfNode : AstNodeBase
    {
        /// <summary>
        /// reserved word if
        /// </summary>
        private string ifWord;

        /// <summary>
        /// reserved word @
        /// </summary>
        private string arrobaWord;

        /// <summary>
        /// reserved word hasEvents
        /// </summary>
        private string hasEventsWord;

        /// <summary>
        /// reserved word not
        /// </summary>
        private string notWord;

        /// <summary>
        /// send expressions node
        /// </summary>
        private AstNodeBase sendExpressions;

        /// <summary>
        /// reserved word endif
        /// </summary>
        private string endIfWord;

        /// <summary>
        /// result planNode
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

            this.result = new PlanNode();
            this.result.Column = ChildrenNodes[0].Token.Location.Column;
            this.result.Line = ChildrenNodes[0].Token.Location.Line;

            this.ifWord = (string)ChildrenNodes[0].Token.Value;

            int childrenCount = ChildrenNodes.Count;
            if (childrenCount == 5)
            {
                this.arrobaWord = (string)ChildrenNodes[1].Token.Value;
                this.hasEventsWord = (string)ChildrenNodes[2].Token.Value;
                this.sendExpressions = AddChild(NodeUseType.Parameter, SR.SendRole, ChildrenNodes[3]) as AstNodeBase;
                this.endIfWord = (string)ChildrenNodes[4].Token.Value;
                this.result.NodeText = this.ifWord + " " + this.arrobaWord + " " + this.hasEventsWord;
                this.result.NodeType = PlanNodeTypeEnum.IfHasEvents;
            }
            else if (childrenCount == 6)
            {
                this.notWord = (string)ChildrenNodes[1].Token.Value;
                this.arrobaWord = (string)ChildrenNodes[2].Token.Value;
                this.hasEventsWord = (string)ChildrenNodes[3].Token.Value;
                this.sendExpressions = AddChild(NodeUseType.Parameter, SR.SendRole, ChildrenNodes[4]) as AstNodeBase;
                this.endIfWord = (string)ChildrenNodes[5].Token.Value;
                this.result.NodeText = this.ifWord + " " + this.notWord + " " + this.arrobaWord + " " + this.hasEventsWord;
                this.result.NodeType = PlanNodeTypeEnum.IfNotHasEvents;
            }
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

            List<PlanNode> sendExpressionsAux = (List<PlanNode>)this.sendExpressions.Evaluate(thread);

            this.result.Children = new System.Collections.Generic.List<PlanNode>();

            foreach (PlanNode plan in sendExpressionsAux)
            {
                this.result.Children.Add(plan);
                this.result.NodeText += " " + plan.NodeText;
            }

            this.result.NodeText += " " + this.endIfWord;

            this.EndEvaluate(thread);

            return this.result;
        }
    }
}
