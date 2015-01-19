//-----------------------------------------------------------------------
// <copyright file="EventNode.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.ASTNodes.Objects.Event
{
    using Integra.Vision.Language.ASTNodes.Base;
    
    using Integra.Vision.Language.General;
    using Integra.Vision.Language.Resources;
    using Irony.Ast;
    using Irony.Interpreter;
    using Irony.Parsing;

    /// <summary>
    /// EventNode class
    /// </summary>
    internal sealed class EventNode : AstNodeBase
    {
        /// <summary>
        /// symbol @
        /// </summary>
        private string symbol;

        /// <summary>
        /// rightNode of the rule
        /// </summary>
        private string rightNode;

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
            this.symbol = (string)ChildrenNodes[0].Token.Value;
            this.rightNode = (string)ChildrenNodes[1].Token.Value;

            this.result = new PlanNode();
            this.result.Column = ChildrenNodes[0].Token.Location.Column;
            this.result.Line = ChildrenNodes[0].Token.Location.Line;
            this.result.NodeText = this.symbol + this.rightNode;
            this.result.NodeType = PlanNodeTypeEnum.Event;
            this.result.Properties.Add("Value", this.rightNode);
            this.result.Properties.Add("DataType", typeof(string).ToString());
        }

        /// <summary>
        /// DoEvaluate
        /// Doc go here
        /// </summary>
        /// <param name="thread">Thread of the evaluated grammar</param>
        /// <returns>return a plan node</returns>
        protected override object DoEvaluate(ScriptThread thread)
        {
            return this.result;
        }
    }
}
