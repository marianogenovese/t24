//-----------------------------------------------------------------------
// <copyright file="UserQueryNode.cs" company="Ingetra.Vision.Language">
//     Copyright (c) Ingetra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.ASTNodes.UserQuery
{
    using Integra.Vision.Language.ASTNodes.Base;
    using Irony.Ast;
    using Irony.Interpreter;
    using Irony.Interpreter.Ast;
    using Irony.Parsing;

    /// <summary>
    /// User query node.
    /// </summary>
    internal sealed class UserQueryNode : AstNodeBase
    {
        /// <summary>
        /// Query from section.
        /// </summary>
        private AstNodeBase from;

        /// <summary>
        /// Query where section.
        /// </summary>
        private AstNodeBase where;

        /// <summary>
        /// Query select section.
        /// </summary>
        private AstNodeBase select;

        /// <summary>
        /// Result plan node
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

            int childrenCount = ChildrenNodes.Count;
            if (childrenCount == 3)
            {
                this.from = AddChild(NodeUseType.Keyword, SR.FromRole, ChildrenNodes[0]) as AstNodeBase;
                this.where = AddChild(NodeUseType.Keyword, SR.WhereRole, ChildrenNodes[1]) as AstNodeBase;
                this.select = AddChild(NodeUseType.Keyword, SR.SelectRole, ChildrenNodes[2]) as AstNodeBase;
            }
            else if (childrenCount == 2)
            {
                this.from = AddChild(NodeUseType.Keyword, SR.FromRole, ChildrenNodes[0]) as AstNodeBase;
                this.select = AddChild(NodeUseType.Keyword, SR.SelectRole, ChildrenNodes[1]) as AstNodeBase;
            }

            this.result = new PlanNode();
            this.result.NodeType = PlanNodeTypeEnum.UserQuery;
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

            this.result.Children = new System.Collections.Generic.List<PlanNode>();
            int childrenCount = ChildrenNodes.Count;

            if (childrenCount == 3)
            {
                PlanNode fromAux = (PlanNode)this.from.Evaluate(thread);
                PlanNode whereAux = (PlanNode)this.where.Evaluate(thread);
                PlanNode selectAux = (PlanNode)this.select.Evaluate(thread);

                this.result.NodeText = string.Format("{0} {1} {2}", fromAux.NodeText, whereAux.NodeText, selectAux.NodeText);

                this.result.Children.Add(fromAux);
                this.result.Children.Add(whereAux);
                this.result.Children.Add(selectAux);

                this.result.Column = fromAux.Column;
                this.result.Line = fromAux.Line;
            }
            else if (childrenCount == 2)
            {
                PlanNode fromAux = (PlanNode)this.from.Evaluate(thread);
                PlanNode selectAux = (PlanNode)this.select.Evaluate(thread);

                this.result.NodeText = string.Format("{0} {1}", fromAux.NodeText, selectAux.NodeText);

                this.result.Children.Add(fromAux);
                this.result.Children.Add(selectAux);

                this.result.Column = fromAux.Column;
                this.result.Line = fromAux.Line;
            }

            this.EndEvaluate(thread);
            
            return this.result;
        }
    }
}
