//-----------------------------------------------------------------------
// <copyright file="UserQueryNode.cs" company="Ingetra.Vision.Language">
//     Copyright (c) Ingetra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.ASTNodes.UserQuery
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Integra.Vision.Language.ASTNodes.Base;
    using Irony.Ast;
    using Irony.Interpreter;
    using Irony.Interpreter.Ast;
    using Irony.Parsing;
    using Runtime;

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
                PlanNode buffer = new PlanNode();
                buffer.NodeType = PlanNodeTypeEnum.ObservableBuffer;
                buffer.Children = new List<PlanNode>();

                PlanNode bufferSize = new PlanNode();
                bufferSize.NodeType = PlanNodeTypeEnum.Constant;
                bufferSize.Properties.Add("Value", int.Parse(ConfigurationManager.AppSettings["DefaultWindowSize"]));
                bufferSize.Properties.Add("DataType", typeof(int));

                buffer.Children.Add(selectForBuffer);
                buffer.Children.Add(bufferSize);
                /* ******************************************************************************************************************************************************** */

                this.result = buffer;
                this.result.Column = fromAux.Column;
                this.result.Line = fromAux.Line;
                this.result.NodeText = string.Format("{0} {1}", fromAux.NodeText, projectionAux.NodeText);
            }
            else if (childrenCount == 3)
            {
                PlanNode fromAux = (PlanNode)this.from.Evaluate(thread);
                PlanNode secondArgument = (PlanNode)this.where.Evaluate(thread);
                PlanNode projectionAux = (PlanNode)this.select.Evaluate(thread);

                if (PlanNodeTypeEnum.ObservableWhere.Equals(secondArgument.NodeType))
                {
                    /* ******************************************************************************************************************************************************** */
                    secondArgument.Children.ElementAt(0).Children.Add(fromAux);
                    /* ******************************************************************************************************************************************************** */
                    PlanNode scopeSelectForBuffer = new PlanNode();
                    scopeSelectForBuffer.NodeType = PlanNodeTypeEnum.NewScope;
                    scopeSelectForBuffer.Children = new List<PlanNode>();

                    scopeSelectForBuffer.Children.Add(secondArgument);

                    PlanNode selectForBuffer = new PlanNode();
                    selectForBuffer.NodeType = PlanNodeTypeEnum.ObservableSelectForBuffer;
                    selectForBuffer.Children = new List<PlanNode>();

                    selectForBuffer.Children.Add(scopeSelectForBuffer);
                    selectForBuffer.Children.Add(projectionAux);
                    /* ******************************************************************************************************************************************************** */
                    PlanNode buffer = new PlanNode();
                    buffer.NodeType = PlanNodeTypeEnum.ObservableBuffer;
                    buffer.Children = new List<PlanNode>();

                    PlanNode bufferSize = new PlanNode();
                    bufferSize.NodeType = PlanNodeTypeEnum.Constant;
                    bufferSize.Properties.Add("Value", int.Parse(ConfigurationManager.AppSettings["DefaultWindowSize"]));
                    bufferSize.Properties.Add("DataType", typeof(int));

                    buffer.Children.Add(selectForBuffer);
                    buffer.Children.Add(bufferSize);
                    /* ******************************************************************************************************************************************************** */

                    this.result = buffer;
                }
                else if (PlanNodeTypeEnum.ObservableBuffer.Equals(secondArgument.NodeType) || PlanNodeTypeEnum.ObservableBufferTimeAndSize.Equals(secondArgument.NodeType))
                {
                    if (this.CheckProjectionIfAllAreFunctions(projectionAux))
                    {
                        /* ******************************************************************************************************************************************************** */
                        secondArgument.NodeType = PlanNodeTypeEnum.ObservableBuffer;
                        secondArgument.Children[1] = secondArgument.Children[1].Children[0].Children[1];
                        secondArgument.Children[0] = fromAux;
                        /* ******************************************************************************************************************************************************** */
                        PlanNode scopeSelectForBuffer = new PlanNode();
                        scopeSelectForBuffer.NodeType = PlanNodeTypeEnum.NewScope;
                        scopeSelectForBuffer.Children = new List<PlanNode>();

                        scopeSelectForBuffer.Children.Add(secondArgument);

                        PlanNode selectForBuffer = new PlanNode();
                        selectForBuffer.NodeType = PlanNodeTypeEnum.ObservableSelectForBuffer;
                        selectForBuffer.Children = new List<PlanNode>();
                        /* ******************************************************************************************************************************************************** */
                        selectForBuffer.Children.Add(scopeSelectForBuffer);
                        selectForBuffer.Children.Add(projectionAux);
                        /* ******************************************************************************************************************************************************** */
                        PlanNode buffer = new PlanNode();
                        buffer.NodeType = PlanNodeTypeEnum.ObservableBuffer;
                        buffer.Children = new List<PlanNode>();

                        PlanNode bufferSize = new PlanNode();
                        bufferSize.NodeType = PlanNodeTypeEnum.Constant;
                        bufferSize.Properties.Add("Value", secondArgument.Children[1].Properties["Value"]);
                        bufferSize.Properties.Add("DataType", secondArgument.Children[1].Properties["DataType"]);

                        buffer.Children.Add(selectForBuffer);
                        buffer.Children.Add(bufferSize);
                        /* ******************************************************************************************************************************************************** */

                        this.result = buffer;
                    }
                    else
                    {
                        secondArgument.Children[0] = fromAux;
                        /* ******************************************************************************************************************************************************** */
                        PlanNode scopeSelectForBuffer = new PlanNode();
                        scopeSelectForBuffer.NodeType = PlanNodeTypeEnum.NewScope;
                        scopeSelectForBuffer.Children = new List<PlanNode>();

                        scopeSelectForBuffer.Children.Add(secondArgument);

                        PlanNode selectForBuffer = new PlanNode();
                        selectForBuffer.NodeType = PlanNodeTypeEnum.ObservableSelectForBuffer;
                        selectForBuffer.Children = new List<PlanNode>();
                        /* ******************************************************************************************************************************************************** */
                        PlanNode fromForLambda = new PlanNode();
                        fromForLambda.NodeType = PlanNodeTypeEnum.ObservableFromForLambda;

                        PlanNode scopeSelectForEnumerable = new PlanNode();
                        scopeSelectForEnumerable.NodeType = PlanNodeTypeEnum.NewScope;
                        scopeSelectForEnumerable.Children = new List<PlanNode>();

                        scopeSelectForEnumerable.Children.Add(fromForLambda);

                        PlanNode selectForEnumerable = new PlanNode();
                        selectForEnumerable.NodeType = PlanNodeTypeEnum.EnumerableSelectForEnumerable;
                        selectForEnumerable.Children = new List<PlanNode>();

                        selectForEnumerable.Children.Add(scopeSelectForEnumerable);
                        selectForEnumerable.Children.Add(projectionAux);
                        /* ******************************************************************************************************************************************************** */
                        selectForBuffer.Children.Add(scopeSelectForBuffer);
                        selectForBuffer.Children.Add(selectForEnumerable);
                        /* ******************************************************************************************************************************************************** */

                        this.result = selectForBuffer;
                    }
                }

                this.result.Column = fromAux.Column;
                this.result.Line = fromAux.Line;
                this.result.NodeText = string.Format("{0} {1} {2}", fromAux.NodeText, secondArgument.NodeText, projectionAux.NodeText);
            }
            else if (childrenCount == 4)
            {
                PlanNode fromAux = (PlanNode)this.from.Evaluate(thread);
                PlanNode secondArgument = (PlanNode)this.applyWindow.Evaluate(thread);
                PlanNode thirdArgument = (PlanNode)this.groupBy.Evaluate(thread);
                PlanNode projectionAux = (PlanNode)this.select.Evaluate(thread);

                if (PlanNodeTypeEnum.ObservableGroupBy.Equals(thirdArgument.NodeType) || PlanNodeTypeEnum.EnumerableGroupBy.Equals(thirdArgument.NodeType))
                {
                    /* ******************************************************************************************************************************************************** */
                    secondArgument.Children[0] = fromAux;
                    secondArgument.NodeType = PlanNodeTypeEnum.ObservableBuffer;
                    secondArgument.Children[1] = secondArgument.Children[1].Children[0].Children[1];
                    /* ******************************************************************************************************************************************************** */
                    NodesFinder nf = new NodesFinder();
                    List<PlanNode> lpn = nf.FindNode(projectionAux, PlanNodeTypeEnum.TupleProjection);
                    foreach (PlanNode tuple in lpn)
                    {
                        PlanNode tupleValue = tuple.Children[1];
                        if (tupleValue.NodeType.Equals(PlanNodeTypeEnum.Identifier))
                        {
                            PlanNode groupKey = new PlanNode();
                            groupKey.NodeType = PlanNodeTypeEnum.GroupKey;
                            groupKey.Column = tupleValue.Column;
                            groupKey.Line = tupleValue.Line;
                            groupKey.NodeText = tupleValue.NodeText;
                            groupKey.Properties.Add("Value", "Key");

                            tupleValue.NodeType = PlanNodeTypeEnum.GroupKeyProperty;
                            tupleValue.Children = new List<PlanNode>();
                            tupleValue.Children.Add(groupKey);
                        }
                    }

                    /* ******************************************************************************************************************************************************** */
                    PlanNode scopeSelectForBuffer = new PlanNode();
                    scopeSelectForBuffer.NodeType = PlanNodeTypeEnum.NewScope;
                    scopeSelectForBuffer.Children = new List<PlanNode>();

                    scopeSelectForBuffer.Children.Add(secondArgument);

                    PlanNode selectForBuffer = new PlanNode();
                    selectForBuffer.NodeType = PlanNodeTypeEnum.ObservableSelectForBuffer;
                    selectForBuffer.Children = new List<PlanNode>();
                    /* ******************************************************************************************************************************************************** */
                    PlanNode fromForLambda = new PlanNode();
                    fromForLambda.NodeType = PlanNodeTypeEnum.ObservableFromForLambda;

                    PlanNode scopeKeySelectorForGroupBy = new PlanNode();
                    scopeKeySelectorForGroupBy.NodeType = PlanNodeTypeEnum.NewScope;
                    scopeKeySelectorForGroupBy.Children = new List<PlanNode>();

                    scopeKeySelectorForGroupBy.Children.Add(fromForLambda);
                    thirdArgument.Children[0].Children.Add(fromForLambda);

                    PlanNode scopeSelectForGroupBy = new PlanNode();
                    scopeSelectForGroupBy.NodeType = PlanNodeTypeEnum.NewScope;
                    scopeSelectForGroupBy.Children = new List<PlanNode>();

                    scopeSelectForGroupBy.Children.Add(thirdArgument);

                    PlanNode selectForGroupBy = new PlanNode();
                    selectForGroupBy.NodeType = PlanNodeTypeEnum.EnumerableSelectForGroupBy;
                    selectForGroupBy.Children = new List<PlanNode>();

                    selectForGroupBy.Children.Add(scopeSelectForGroupBy);
                    selectForGroupBy.Children.Add(projectionAux);
                    /* ******************************************************************************************************************************************************** */
                    selectForBuffer.Children.Add(scopeSelectForBuffer);
                    selectForBuffer.Children.Add(selectForGroupBy);
                    /* ******************************************************************************************************************************************************** */

                    this.result = selectForBuffer;
                }
                else if (PlanNodeTypeEnum.ObservableBuffer.Equals(thirdArgument.NodeType) || PlanNodeTypeEnum.ObservableBufferTimeAndSize.Equals(thirdArgument.NodeType))
                {
                    if (this.CheckProjectionIfAllAreFunctions(projectionAux))
                    {
                        /* ******************************************************************************************************************************************************** */
                        secondArgument.Children.ElementAt(0).Children.Add(fromAux);

                        thirdArgument.NodeType = PlanNodeTypeEnum.ObservableBuffer;
                        thirdArgument.Children[1] = thirdArgument.Children[1].Children[0].Children[1];
                        thirdArgument.Children[0] = secondArgument;
                        /* ******************************************************************************************************************************************************** */
                        PlanNode scopeSelectForBuffer = new PlanNode();
                        scopeSelectForBuffer.NodeType = PlanNodeTypeEnum.NewScope;
                        scopeSelectForBuffer.Children = new List<PlanNode>();

                        scopeSelectForBuffer.Children.Add(thirdArgument);

                        PlanNode selectForBuffer = new PlanNode();
                        selectForBuffer.NodeType = PlanNodeTypeEnum.ObservableSelectForBuffer;
                        selectForBuffer.Children = new List<PlanNode>();
                        /* ******************************************************************************************************************************************************** */
                        selectForBuffer.Children.Add(scopeSelectForBuffer);
                        selectForBuffer.Children.Add(projectionAux);
                        /* ******************************************************************************************************************************************************** */
                        PlanNode buffer = new PlanNode();
                        buffer.NodeType = PlanNodeTypeEnum.ObservableBuffer;
                        buffer.Children = new List<PlanNode>();

                        PlanNode bufferSize = new PlanNode();
                        bufferSize.NodeType = PlanNodeTypeEnum.Constant;
                        bufferSize.Properties.Add("Value", thirdArgument.Children[1].Properties["Value"]);
                        bufferSize.Properties.Add("DataType", thirdArgument.Children[1].Properties["DataType"]);

                        buffer.Children.Add(selectForBuffer);
                        buffer.Children.Add(bufferSize);
                        /* ******************************************************************************************************************************************************** */

                        this.result = buffer;
                    }
                    else
                    {
                        /* ******************************************************************************************************************************************************** */
                        secondArgument.Children.ElementAt(0).Children.Add(fromAux);
                        thirdArgument.Children[0] = secondArgument;
                        /* ******************************************************************************************************************************************************** */
                        PlanNode scopeSelectForBuffer = new PlanNode();
                        scopeSelectForBuffer.NodeType = PlanNodeTypeEnum.NewScope;
                        scopeSelectForBuffer.Children = new List<PlanNode>();

                        scopeSelectForBuffer.Children.Add(thirdArgument);

                        PlanNode selectForBuffer = new PlanNode();
                        selectForBuffer.NodeType = PlanNodeTypeEnum.ObservableSelectForBuffer;
                        selectForBuffer.Children = new List<PlanNode>();
                        /* ******************************************************************************************************************************************************** */
                        PlanNode fromForLambda = new PlanNode();
                        fromForLambda.NodeType = PlanNodeTypeEnum.ObservableFromForLambda;

                        PlanNode scopeSelectForEnumerable = new PlanNode();
                        scopeSelectForEnumerable.NodeType = PlanNodeTypeEnum.NewScope;
                        scopeSelectForEnumerable.Children = new List<PlanNode>();

                        scopeSelectForEnumerable.Children.Add(fromForLambda);

                        PlanNode selectForEnumerable = new PlanNode();
                        selectForEnumerable.NodeType = PlanNodeTypeEnum.EnumerableSelectForEnumerable;
                        selectForEnumerable.Children = new List<PlanNode>();

                        selectForEnumerable.Children.Add(scopeSelectForEnumerable);
                        selectForEnumerable.Children.Add(projectionAux);
                        /* ******************************************************************************************************************************************************** */
                        selectForBuffer.Children.Add(scopeSelectForBuffer);
                        selectForBuffer.Children.Add(selectForEnumerable);
                        /* ******************************************************************************************************************************************************** */

                        this.result = selectForBuffer;
                    }
                }

                this.result.Column = fromAux.Column;
                this.result.Line = fromAux.Line;
                this.result.NodeText = string.Format("{0} {1} {2} {3}", fromAux.NodeText, secondArgument.NodeText, thirdArgument.NodeText, projectionAux.NodeText);
            }
            else if (childrenCount == 5)
            {
                PlanNode fromAux = (PlanNode)this.from.Evaluate(thread);
                PlanNode whereAux = (PlanNode)this.where.Evaluate(thread);
                PlanNode applyWindowAux = (PlanNode)this.applyWindow.Evaluate(thread);
                PlanNode groupByAux = (PlanNode)this.groupBy.Evaluate(thread);
                PlanNode projectionAux = (PlanNode)this.select.Evaluate(thread);

                /* ******************************************************************************************************************************************************** */
                whereAux.Children.ElementAt(0).Children.Add(fromAux);
                applyWindowAux.NodeType = PlanNodeTypeEnum.ObservableBuffer;
                applyWindowAux.Children[1] = applyWindowAux.Children[1].Children[0].Children[1];
                applyWindowAux.Children[0] = whereAux;
                /* ******************************************************************************************************************************************************** */
                NodesFinder nf = new NodesFinder();
                List<PlanNode> lpn = nf.FindNode(projectionAux, PlanNodeTypeEnum.TupleProjection);
                foreach (PlanNode tuple in lpn)
                {
                    PlanNode tupleValue = tuple.Children[1];
                    if (tupleValue.NodeType.Equals(PlanNodeTypeEnum.Identifier))
                    {
                        PlanNode groupKey = new PlanNode();
                        groupKey.NodeType = PlanNodeTypeEnum.GroupKey;
                        groupKey.Column = tupleValue.Column;
                        groupKey.Line = tupleValue.Line;
                        groupKey.NodeText = tupleValue.NodeText;
                        groupKey.Properties.Add("Value", "Key");

                        tupleValue.NodeType = PlanNodeTypeEnum.GroupKeyProperty;
                        tupleValue.Children = new List<PlanNode>();
                        tupleValue.Children.Add(groupKey);
                    }
                }

                /* ******************************************************************************************************************************************************** */
                PlanNode scopeSelectForBuffer = new PlanNode();
                scopeSelectForBuffer.NodeType = PlanNodeTypeEnum.NewScope;
                scopeSelectForBuffer.Children = new List<PlanNode>();

                scopeSelectForBuffer.Children.Add(applyWindowAux);

                PlanNode selectForBuffer = new PlanNode();
                selectForBuffer.NodeType = PlanNodeTypeEnum.ObservableSelectForBuffer;
                selectForBuffer.Children = new List<PlanNode>();
                /* ******************************************************************************************************************************************************** */
                PlanNode fromForLambda = new PlanNode();
                fromForLambda.NodeType = PlanNodeTypeEnum.ObservableFromForLambda;

                PlanNode scopeKeySelectorForGroupBy = new PlanNode();
                scopeKeySelectorForGroupBy.NodeType = PlanNodeTypeEnum.NewScope;
                scopeKeySelectorForGroupBy.Children = new List<PlanNode>();

                scopeKeySelectorForGroupBy.Children.Add(fromForLambda);
                groupByAux.Children[0].Children.Add(fromForLambda);

                PlanNode scopeSelectForGroupBy = new PlanNode();
                scopeSelectForGroupBy.NodeType = PlanNodeTypeEnum.NewScope;
                scopeSelectForGroupBy.Children = new List<PlanNode>();

                scopeSelectForGroupBy.Children.Add(groupByAux);

                PlanNode selectForGroupBy = new PlanNode();
                selectForGroupBy.NodeType = PlanNodeTypeEnum.EnumerableSelectForGroupBy;
                selectForGroupBy.Children = new List<PlanNode>();

                selectForGroupBy.Children.Add(scopeSelectForGroupBy);
                selectForGroupBy.Children.Add(projectionAux);
                /* ******************************************************************************************************************************************************** */
                selectForBuffer.Children.Add(scopeSelectForBuffer);
                selectForBuffer.Children.Add(selectForGroupBy);
                /* ******************************************************************************************************************************************************** */

                this.result = selectForBuffer;
                this.result.Column = fromAux.Column;
                this.result.Line = fromAux.Line;
                this.result.NodeText = string.Format("{0} {1} {2} {3} {4}", fromAux.NodeText, whereAux.NodeText, applyWindowAux.NodeText, groupByAux.NodeText, projectionAux.NodeText);
            }

            this.EndEvaluate(thread);

            return this.result;
        }

        /// <summary>
        /// Check if all projection columns are functions.
        /// </summary>
        /// <param name="projection">Projection plan.</param>
        /// <returns>True or false.</returns>
        private bool CheckProjectionIfAllAreFunctions(PlanNode projection)
        {
            return projection.Children.All(x => PlanNodeTypeEnum.EnumerableCount.Equals(x.Children[1].NodeType) || PlanNodeTypeEnum.EnumerableCount.Equals(x.Children[1].NodeType) || PlanNodeTypeEnum.EnumerableSum.Equals(x.Children[1].NodeType));
        }

        /// <summary>
        /// Check if all projection columns are values.
        /// </summary>
        /// <param name="projection">Projection plan.</param>
        /// <returns>True or false.</returns>
        private bool CheckProjectionIfAllAreValues(PlanNode projection)
        {
            return projection.Children.All(x => PlanNodeTypeEnum.Event.Equals(x.Children[1].NodeType) || PlanNodeTypeEnum.Constant.Equals(x.Children[1].NodeType) || PlanNodeTypeEnum.Identifier.Equals(x.Children[1].NodeType) || PlanNodeTypeEnum.GroupKey.Equals(x.Children[1].NodeType));
        }

        /// <summary>
        /// Check if the projection columns are values and functions.
        /// </summary>
        /// <param name="projection">Projection plan.</param>
        /// <returns>True or false.</returns>
        private bool CheckProjectionIfAreValuesAndFunctions(PlanNode projection)
        {
            if (!this.CheckProjectionIfAllAreFunctions(projection) && !this.CheckProjectionIfAllAreValues(projection))
            {
                return true;
            }

            return false;
        }
    }
}
