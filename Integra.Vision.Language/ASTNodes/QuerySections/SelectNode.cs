//-----------------------------------------------------------------------
// <copyright file="SelectNode.cs" company="Integra.Vision.Language">
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
    /// SelectNode class
    /// </summary>
    internal sealed class SelectNode : AstNodeBase
    {
        /// <summary>
        /// reserved word "select"
        /// </summary>
        private string select;

        /// <summary>
        /// list of values for the projection
        /// </summary>
        private AstNodeBase listOfValues;

        /// <summary>
        /// result of the evaluation
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
            this.select = (string)ChildrenNodes[0].Token.Value;
            this.listOfValues = AddChild(NodeUseType.Parameter, "listOfValues", ChildrenNodes[1]) as AstNodeBase;

            this.result = new PlanNode();
            this.result.Column = ChildrenNodes[0].Token.Location.Column;
            this.result.Line = ChildrenNodes[0].Token.Location.Line;
            this.result.NodeType = (uint)PlanNodeTypeEnum.Select;
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

            Dictionary<PlanNode, PlanNode> projection = new Dictionary<PlanNode, PlanNode>();
            Binding b1 = thread.Bind("ObjectList", BindingRequestFlags.Write | BindingRequestFlags.ExistingOrNew);
            b1.SetValueRef(thread, projection);
            
            this.listOfValues.Evaluate(thread);

            Binding b2 = thread.Bind("ObjectList", BindingRequestFlags.Read);

            this.EndEvaluate(thread);

            this.result.NodeText = this.select;

            projection = (Dictionary<PlanNode, PlanNode>)b2.GetValueRef(thread);
            this.result.Children = new List<PlanNode>();

            foreach (var tupla in projection)
            {
                PlanNode plan = new PlanNode();
                plan.NodeType = (uint)PlanNodeTypeEnum.TupleProjection;
                plan.Children = new List<PlanNode>();
                plan.Children.Add(tupla.Key);
                plan.Children.Add(tupla.Value);

                this.result.NodeText += " " + tupla.Key.NodeText + ", " + tupla.Value.NodeText;
                this.result.Children.Add(plan);
            }

            return this.result;
        }
    }
}
