﻿//-----------------------------------------------------------------------
// <copyright file="ConstantValueWithoutAliasNode.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.ASTNodes.Constants
{
    using System.Collections.Generic;
    using Integra.Vision.Language.ASTNodes.Base;
    using Irony.Ast;
    using Irony.Interpreter;
    using Irony.Interpreter.Ast;
    using Irony.Parsing;

    /// <summary>
    /// ConstantValueWithAliasNode class
    /// </summary>
    internal sealed class ConstantValueWithoutAliasNode : AstNodeBase
    {
        /// <summary>
        /// value node
        /// </summary>
        private AstNodeBase valueNode;

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

            this.valueNode = AddChild(NodeUseType.Parameter, "ValueNode", ChildrenNodes[0]) as AstNodeBase;

            this.result = new PlanNode();
            this.result.NodeType = PlanNodeTypeEnum.ValueWithoutAlias;
            this.result.Children = new List<PlanNode>();
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
            PlanNode alias = (PlanNode)this.valueNode.Evaluate(thread);
            this.EndEvaluate(thread);

            PlanNode value = new PlanNode();
            value.NodeText = alias.NodeText;
            value.NodeType = PlanNodeTypeEnum.Property;
            value.Properties.Add("Property", alias.Properties["Value"]);
            
            this.result.Column = value.Column;
            this.result.Line = value.Line;
            this.result.NodeText = value.NodeText;
            this.result.Children.Add(value);
            this.result.Children.Add(alias);

            return this.result;
        }
    }
}
