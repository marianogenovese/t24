﻿//-----------------------------------------------------------------------
// <copyright file="UserQueryNode.cs" company="Ingetra.Vision.Language">
//     Copyright (c) Ingetra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.ASTNodes.UserQuery
{
    using System.Collections.Generic;
    using System.Linq;
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
        /// Query apply window section
        /// </summary>
        private AstNode applyWindow;

        /// <summary>
        /// Query group by section
        /// </summary>
        private AstNode groupBy;

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
            else if (childrenCount == 4)
            {
                this.from = AddChild(NodeUseType.Keyword, SR.FromRole, ChildrenNodes[0]) as AstNodeBase;
                this.applyWindow = AddChild(NodeUseType.Keyword, SR.ApplyWindowRole, ChildrenNodes[1]) as AstNodeBase;
                this.groupBy = AddChild(NodeUseType.Keyword, SR.GroupByRole, ChildrenNodes[2]) as AstNodeBase;
                this.select = AddChild(NodeUseType.Keyword, SR.SelectRole, ChildrenNodes[3]) as AstNodeBase;
            }
            else if (childrenCount == 5)
            {
                this.from = AddChild(NodeUseType.Keyword, SR.FromRole, ChildrenNodes[0]) as AstNodeBase;
                this.where = AddChild(NodeUseType.Keyword, SR.WhereRole, ChildrenNodes[1]) as AstNodeBase;
                this.applyWindow = AddChild(NodeUseType.Keyword, SR.ApplyWindowRole, ChildrenNodes[2]) as AstNodeBase;
                this.groupBy = AddChild(NodeUseType.Keyword, SR.GroupByRole, ChildrenNodes[3]) as AstNodeBase;
                this.select = AddChild(NodeUseType.Keyword, SR.SelectRole, ChildrenNodes[4]) as AstNodeBase;
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

            if (childrenCount == 2)
            {
                PlanNode fromAux = (PlanNode)this.from.Evaluate(thread);
                PlanNode projectionAux = (PlanNode)this.select.Evaluate(thread);

                this.result.NodeText = string.Format("{0} {1}", fromAux.NodeText, projectionAux.NodeText);

                /* ******************************************************************************************************************************************************** */
                PlanNode scopeSelectForBuffer = new PlanNode();
                scopeSelectForBuffer.NodeType = PlanNodeTypeEnum.NewScope;
                scopeSelectForBuffer.Children = new List<PlanNode>();

                scopeSelectForBuffer.Children.Add(fromAux);

                PlanNode selectForBuffer = new PlanNode();
                selectForBuffer.NodeType = PlanNodeTypeEnum.ObservableSelectForBuffer;
                selectForBuffer.Children = new List<PlanNode>();

                selectForBuffer.Children.Add(scopeSelectForBuffer);
                selectForBuffer.Children.Add(projectionAux);
                /* ******************************************************************************************************************************************************** */

                this.result = selectForBuffer;
                this.result.Column = fromAux.Column;
                this.result.Line = fromAux.Line;
            }
            else if (childrenCount == 3)
            {
                PlanNode fromAux = (PlanNode)this.from.Evaluate(thread);
                PlanNode whereAux = (PlanNode)this.where.Evaluate(thread);
                PlanNode projectionAux = (PlanNode)this.select.Evaluate(thread);

                this.result.NodeText = string.Format("{0} {1} {2}", fromAux.NodeText, whereAux.NodeText, projectionAux.NodeText);

                /* ******************************************************************************************************************************************************** */
                whereAux.Children.ElementAt(0).Children.Add(fromAux);
                /* ******************************************************************************************************************************************************** */
                PlanNode scopeSelectForBuffer = new PlanNode();
                scopeSelectForBuffer.NodeType = PlanNodeTypeEnum.NewScope;
                scopeSelectForBuffer.Children = new List<PlanNode>();

                scopeSelectForBuffer.Children.Add(whereAux);

                PlanNode selectForBuffer = new PlanNode();
                selectForBuffer.NodeType = PlanNodeTypeEnum.ObservableSelectForBuffer;
                selectForBuffer.Children = new List<PlanNode>();

                selectForBuffer.Children.Add(scopeSelectForBuffer);
                selectForBuffer.Children.Add(projectionAux);
                /* ******************************************************************************************************************************************************** */

                this.result = selectForBuffer;
                this.result.Column = fromAux.Column;
                this.result.Line = fromAux.Line;
            }
            else if (childrenCount == 4)
            {
                PlanNode fromAux = (PlanNode)this.from.Evaluate(thread);
                PlanNode applyWindowAux = (PlanNode)this.applyWindow.Evaluate(thread);
                PlanNode groupByAux = (PlanNode)this.groupBy.Evaluate(thread);
                PlanNode projectionAux = (PlanNode)this.select.Evaluate(thread);

                this.result.NodeText = string.Format("{0} {1} {2} {3}", fromAux.NodeText, applyWindowAux.NodeText, groupByAux.NodeText, projectionAux.NodeText);

                /* ******************************************************************************************************************************************************** */
                groupByAux.Children.ElementAt(0).Children.Add(fromAux);
                /* ******************************************************************************************************************************************************** */
                PlanNode scopeSelectForBuffer = new PlanNode();
                scopeSelectForBuffer.NodeType = PlanNodeTypeEnum.NewScope;
                scopeSelectForBuffer.Children = new List<PlanNode>();

                scopeSelectForBuffer.Children.Add(applyWindowAux);

                PlanNode selectForBuffer = new PlanNode();
                selectForBuffer.NodeType = PlanNodeTypeEnum.ObservableSelectForBuffer;
                selectForBuffer.Children = new List<PlanNode>();

                selectForBuffer.Children.Add(scopeSelectForBuffer);
                selectForBuffer.Children.Add(projectionAux);
                /* ******************************************************************************************************************************************************** */
                PlanNode scopeSelectForGroupBy = this.result;
                scopeSelectForGroupBy.NodeType = PlanNodeTypeEnum.NewScope;
                scopeSelectForGroupBy.Children = new List<PlanNode>();

                scopeSelectForGroupBy.Children.Add(groupByAux);

                PlanNode selectForGroupBy = new PlanNode();
                selectForGroupBy.NodeType = PlanNodeTypeEnum.ObservableSelectForGroupBy;
                selectForGroupBy.Children = new List<PlanNode>();

                selectForGroupBy.Children.Add(scopeSelectForGroupBy);
                selectForGroupBy.Children.Add(selectForBuffer);
                /* ******************************************************************************************************************************************************** */
                PlanNode merge = new PlanNode();
                merge.NodeType = PlanNodeTypeEnum.ObservableMerge;
                merge.Children = new List<PlanNode>();

                merge.Children.Add(selectForGroupBy);

                this.result = merge;
                this.result.Column = fromAux.Column;
                this.result.Line = fromAux.Line;
            }
            else if (childrenCount == 5)
            {
                // this.result.NodeType = PlanNodeTypeEnum.ObservableMerge;
                PlanNode fromAux = (PlanNode)this.from.Evaluate(thread);
                PlanNode whereAux = (PlanNode)this.where.Evaluate(thread);
                PlanNode applyWindowAux = (PlanNode)this.applyWindow.Evaluate(thread);
                PlanNode groupByAux = (PlanNode)this.groupBy.Evaluate(thread);
                PlanNode projectionAux = (PlanNode)this.select.Evaluate(thread);

                this.result.NodeText = string.Format("{0} {1} {2} {3} {4}", fromAux.NodeText, whereAux.NodeText, applyWindowAux.NodeText, groupByAux.NodeText, projectionAux.NodeText);

                /* ******************************************************************************************************************************************************** */
                whereAux.Children.ElementAt(0).Children.Add(fromAux);
                groupByAux.Children.ElementAt(0).Children.Add(whereAux);
                /* ******************************************************************************************************************************************************** */                
                PlanNode scopeSelectForBuffer = new PlanNode();
                scopeSelectForBuffer.NodeType = PlanNodeTypeEnum.NewScope;
                scopeSelectForBuffer.Children = new List<PlanNode>();

                scopeSelectForBuffer.Children.Add(applyWindowAux);

                PlanNode selectForBuffer = new PlanNode();
                selectForBuffer.NodeType = PlanNodeTypeEnum.ObservableSelectForBuffer;
                selectForBuffer.Children = new List<PlanNode>();

                selectForBuffer.Children.Add(scopeSelectForBuffer);
                selectForBuffer.Children.Add(projectionAux);
                /* ******************************************************************************************************************************************************** */
                PlanNode scopeSelectForGroupBy = this.result;
                scopeSelectForGroupBy.NodeType = PlanNodeTypeEnum.NewScope;
                scopeSelectForGroupBy.Children = new List<PlanNode>();

                scopeSelectForGroupBy.Children.Add(groupByAux);

                PlanNode selectForGroupBy = new PlanNode();
                selectForGroupBy.NodeType = PlanNodeTypeEnum.ObservableSelectForGroupBy;
                selectForGroupBy.Children = new List<PlanNode>();

                selectForGroupBy.Children.Add(scopeSelectForGroupBy);
                selectForGroupBy.Children.Add(selectForBuffer);
                /* ******************************************************************************************************************************************************** */
                PlanNode merge = new PlanNode();
                merge.NodeType = PlanNodeTypeEnum.ObservableMerge;
                merge.Children = new List<PlanNode>();

                merge.Children.Add(selectForGroupBy);

                this.result = merge;
                this.result.Column = fromAux.Column;
                this.result.Line = fromAux.Line;
            }

            this.EndEvaluate(thread);

            return this.result;
        }
    }
}
