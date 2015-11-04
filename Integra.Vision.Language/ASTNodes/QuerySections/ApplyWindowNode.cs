//-----------------------------------------------------------------------
// <copyright file="ApplyWindowNode.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.ASTNodes.QuerySections
{
    using System.Collections.Generic;
    using Integra.Vision.Language.ASTNodes.Base;
    using Irony.Ast;
    using Irony.Interpreter;
    using Irony.Interpreter.Ast;
    using Irony.Parsing;

    /// <summary>
    /// ApplyWindowNode class
    /// </summary>
    internal sealed class ApplyWindowNode : AstNodeBase
    {
        /// <summary>
        /// reserved word apply
        /// </summary>
        private string applyWord;

        /// <summary>
        /// reserved word window
        /// </summary>
        private string windowWord;

        /// <summary>
        /// reserved word of
        /// </summary>
        private string reservedWordOf;

        /// <summary>
        /// window size
        /// </summary>
        private AstNodeBase windowSize;
        
        /// <summary>
        /// result plan
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
            this.applyWord = (string)ChildrenNodes[0].Token.Value;
            this.windowWord = (string)ChildrenNodes[1].Token.Value;
            this.reservedWordOf = (string)ChildrenNodes[2].Token.Value;
            this.windowSize = AddChild(NodeUseType.Parameter, "windowSize", ChildrenNodes[3]) as AstNodeBase;

            this.result = new PlanNode();
            this.result.Column = ChildrenNodes[0].Token.Location.Column;
            this.result.Line = ChildrenNodes[0].Token.Location.Line;
            this.result.NodeType = PlanNodeTypeEnum.ObservableBuffer;
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
            PlanNode windowSizeAux = (PlanNode)this.windowSize.Evaluate(thread);
            this.EndEvaluate(thread);

            PlanNode fromLambdaForBuffer = new PlanNode();
            fromLambdaForBuffer.NodeType = PlanNodeTypeEnum.ObservableFromForLambda;

            this.result.NodeText = this.applyWord + " " + this.windowWord + " " + this.reservedWordOf + " " + windowSizeAux.NodeText;

            this.result.Children = new List<PlanNode>();
            this.result.Children.Add(fromLambdaForBuffer);
            this.result.Children.Add(windowSizeAux);

            return this.result;
        }
    }
}
