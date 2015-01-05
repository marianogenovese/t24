﻿//-----------------------------------------------------------------------
// <copyright file="WhereNode.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.ASTNodes.QuerySections
{
    using Integra.Vision.Language.ASTNodes.Base;
    using Irony.Ast;
    using Irony.Interpreter;
    using Irony.Interpreter.Ast;
    using Irony.Parsing;

    /// <summary>
    /// WhereNode class
    /// </summary>
    internal sealed class WhereNode : AstNodeBase
    {
        /// <summary>
        /// conditions of the where expression
        /// </summary>
        private AstNodeBase condition;

        /// <summary>
        /// reserved word 'where'
        /// </summary>
        private string where;

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
            this.where = (string)ChildrenNodes[0].Token.Value;
            this.condition = AddChild(NodeUseType.Parameter, "whereConditions", ChildrenNodes[1]) as AstNodeBase;
            this.result = new PlanNode();
            this.result.Column = ChildrenNodes[0].Token.Location.Column;
            this.result.Line = ChildrenNodes[0].Token.Location.Line;
            this.result.NodeType = PlanNodeTypeEnum.Where;
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
            PlanNode conditionAux = (PlanNode)this.condition.Evaluate(thread);
            this.EndEvaluate(thread);

            this.result.NodeText = this.where + " " + conditionAux.NodeText;

            this.result.Children = new System.Collections.Generic.List<PlanNode>();
            this.result.Children.Add(conditionAux);

            return this.result;
        }
    }
}
