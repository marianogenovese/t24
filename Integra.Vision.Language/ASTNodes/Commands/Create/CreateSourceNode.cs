//-----------------------------------------------------------------------
// <copyright file="CreateSourceNode.cs" company="ARITEC">
//     Integra Vision. All rights reserved.
// </copyright>
// <author>Oscar Canek</author>
//-----------------------------------------------------------------------

namespace Integra.Vision.Language.ASTNodes.Commands.Create
{
    using Integra.Vision.Language.ASTNodes.Base;
    using Integra.Vision.Language.Resources;
    using Irony.Ast;
    using Irony.Interpreter;
    using Irony.Interpreter.Ast;
    using Irony.Parsing;

    /// <summary>
    /// CreateSourceNode
    /// Doc go here
    /// </summary>
    internal sealed class CreateSourceNode : AstNodeBase
    {
        /// <summary>
        /// reserved word
        /// </summary>
        private string createOrAlterWord;

        /// <summary>
        /// reserved word
        /// </summary>
        private string sourceWord;

        /// <summary>
        /// name of the source
        /// </summary>
        private AstNodeBase idSource;

        /// <summary>
        /// reserved word
        /// </summary>
        private string asWord;

        /// <summary>
        /// from node
        /// </summary>
        private AstNodeBase from;

        /// <summary>
        /// where node
        /// </summary>
        private AstNodeBase where;

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
            this.createOrAlterWord = (string)ChildrenNodes[0].Token.Value;
            this.sourceWord = (string)ChildrenNodes[1].Token.Value;
            this.idSource = AddChild(NodeUseType.Parameter, SR.UserDefinedObjectRole, ChildrenNodes[2]) as AstNodeBase;
            this.asWord = (string)ChildrenNodes[3].Token.Value;
            this.from = AddChild(NodeUseType.Parameter, SR.FromRole, ChildrenNodes[4]) as AstNodeBase;
            this.where = AddChild(NodeUseType.Parameter, SR.WhereRole, ChildrenNodes[5]) as AstNodeBase;

            this.result = new PlanNode();
            this.result.Column = ChildrenNodes[0].Token.Location.Column;
            this.result.Line = ChildrenNodes[1].Token.Location.Line;

            if (this.createOrAlterWord.Equals("create", System.StringComparison.InvariantCultureIgnoreCase))
            {
                this.result.NodeType = PlanNodeTypeEnum.CreateSource;
            }
            else if (this.createOrAlterWord.Equals("alter", System.StringComparison.InvariantCultureIgnoreCase))
            {
                this.result.NodeType = PlanNodeTypeEnum.AlterSource;
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
            PlanNode idSourceAux = (PlanNode)this.idSource.Evaluate(thread);
            PlanNode fromAux = (PlanNode)this.from.Evaluate(thread);
            PlanNode whereAux = (PlanNode)this.where.Evaluate(thread);
            this.EndEvaluate(thread);

            this.result.NodeText = SR.SourceNodeText(this.createOrAlterWord, this.sourceWord, idSourceAux.NodeText, this.asWord, fromAux.NodeText, whereAux.NodeText);

            this.result.Children = new System.Collections.Generic.List<PlanNode>();
            this.result.Children.Add(idSourceAux);
            this.result.Children.Add(fromAux);
            this.result.Children.Add(whereAux);

            return this.result;
        }
    }
}
