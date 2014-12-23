﻿//-----------------------------------------------------------------------
// <copyright file="ObjectValueNode.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.ASTNodes.Objects.Object
{
    using System.Collections.Generic;
    using System.Linq;
    using Integra.Vision.Language.ASTNodes.Base;
    using Integra.Vision.Language.Resources;
    using Irony.Ast;
    using Irony.Interpreter;
    using Irony.Interpreter.Ast;
    using Irony.Parsing;

    /// <summary>
    /// ObjectWithSuffix class
    /// </summary>
    internal sealed class ObjectValueNode : AstNodeBase
    {
        /// <summary>
        /// result plan
        /// </summary>
        private PlanNode result;

        /// <summary>
        /// object node
        /// </summary>
        private AstNodeBase objeto;

        /// <summary>
        /// First method called
        /// </summary>
        /// <param name="context">Contains the actual context</param>
        /// <param name="treeNode">Contains the tree of the context</param>
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            this.objeto = AddChild(NodeUseType.Keyword, SR.ObjectRole, ChildrenNodes[0]) as AstNodeBase;
            this.result = new PlanNode();
        }

        /// <summary>
        /// DoEvaluate
        /// Doc go here
        /// </summary>
        /// <param name="thread">Thread of the evaluated grammar</param>
        /// <returns>return a plan node</returns>
        protected override object DoEvaluate(ScriptThread thread)
        {
            int cantHijos = ChildrenNodes.Count();

            this.BeginEvaluate(thread);
            PlanNode field = (PlanNode)this.objeto.Evaluate(thread);
            this.EndEvaluate(thread);

            this.result.Column = field.Column;
            this.result.Line = field.Line;
            this.result.NodeText = field.NodeText;
            this.result.Properties.Add(SR.DataTypeProperty, typeof(object).ToString());
            this.result.NodeType = (uint)PlanNodeTypeEnum.ObjectValue;
            this.result.Children = new List<PlanNode>();
            this.result.Children.Add(field);

            return this.result;
        }
    }
}
