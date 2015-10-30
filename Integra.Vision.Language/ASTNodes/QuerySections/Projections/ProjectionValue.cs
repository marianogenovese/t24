//-----------------------------------------------------------------------
// <copyright file="ProjectionValue.cs" company="Ingetra.Vision.Language">
//     Copyright (c) Ingetra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.ASTNodes.QuerySections
{
    using System.Collections.Generic;
    using System.Linq;
    using Integra.Vision.Language.ASTNodes.Base;
    using Irony.Ast;
    using Irony.Interpreter;
    using Irony.Interpreter.Ast;
    using Irony.Parsing;

    /// <summary>
    /// Projection value node.
    /// </summary>
    internal class ProjectionValue : AstNodeBase
    {
        /// <summary>
        /// function name
        /// </summary>
        private string functionName;

        /// <summary>
        /// value node
        /// </summary>
        private AstNodeBase value;

        /// <summary>
        /// result execution plan
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
            int childrenCount = ChildrenNodes.Count;
            if (childrenCount == 1)
            {
                this.value = AddChild(NodeUseType.Keyword, SR.SelectRole, ChildrenNodes[0]) as AstNodeBase;
            }
            else if (childrenCount == 3)
            {
                this.functionName = (string)ChildrenNodes[0].Token.Value;

                this.result.Column = ChildrenNodes[0].Token.Location.Column;
                this.result.Line = ChildrenNodes[0].Token.Location.Line;
            }
            else if (childrenCount == 4)
            {
                this.functionName = (string)ChildrenNodes[0].Token.Value;
                this.value = AddChild(NodeUseType.Keyword, SR.SelectRole, ChildrenNodes[2]) as AstNodeBase;

                this.result.Column = ChildrenNodes[0].Token.Location.Column;
                this.result.Line = ChildrenNodes[0].Token.Location.Line;
            }
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

            int childrenCount = ChildrenNodes.Count;
            if (childrenCount == 1)
            {
                this.result = (PlanNode)this.value.Evaluate(thread);
            }
            else if (childrenCount == 3)
            {
                this.result.NodeType = PlanNodeTypeEnum.EnumerableCount;
                this.result.NodeText = string.Format("{0}()", this.functionName);
                this.result.Children = new List<PlanNode>();
            }
            else if (childrenCount == 4)
            {
                PlanNode valueAux = (PlanNode)this.value.Evaluate(thread);

                this.result.NodeType = PlanNodeTypeEnum.EnumerableSum;
                this.result.NodeText = string.Format("{0}({1})", this.functionName, valueAux.NodeText);
                this.result.Children = new List<PlanNode>();

                PlanNode newScope = new PlanNode();
                newScope.NodeType = PlanNodeTypeEnum.NewScope;
                newScope.Children = new List<PlanNode>();

                this.result.Children.Add(newScope);
                this.result.Children.Add(valueAux);
            }

            this.EndEvaluate(thread);

            return this.result;
        }
    }
}
