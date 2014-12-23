//-----------------------------------------------------------------------
// <copyright file="CreateAdapterNode.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.ASTNodes.Commands.Create
{
    using System.Collections.Generic;
    using Integra.Vision.Language.ASTNodes.Base;
    using Integra.Vision.Language.Resources;
    using Irony.Ast;
    using Irony.Interpreter;
    using Irony.Interpreter.Ast;
    using Irony.Parsing;

    /// <summary>
    /// CreateAdapterNode class
    /// </summary>
    internal sealed class CreateAdapterNode : AstNodeBase
    {
        /// <summary>
        /// reserved word
        /// </summary>
        private string createWord;

        /// <summary>
        /// reserved word
        /// </summary>
        private string adapterWord;

        /// <summary>
        /// name of the source
        /// </summary>
        private AstNodeBase idAdapter;

        /// <summary>
        /// reserved word
        /// </summary>
        private string forWord;

        /// <summary>
        /// input / output
        /// </summary>
        private AstNodeBase adapterType;

        /// <summary>
        /// reserved word
        /// </summary>
        private string asWord;

        /// <summary>
        /// adapter parameter list
        /// </summary>
        private AstNodeBase parameters;

        /// <summary>
        /// reserved word
        /// </summary>
        private string referenceWord;

        /// <summary>
        /// adapter reference
        /// </summary>
        private AstNodeBase reference;

        /// <summary>
        /// result planNode
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
            this.createWord = (string)ChildrenNodes[0].Token.Value;
            this.adapterWord = (string)ChildrenNodes[1].Token.Value;
            this.idAdapter = AddChild(NodeUseType.Parameter, SR.UserDefinedObjectRole, ChildrenNodes[2]) as AstNodeBase;
            this.forWord = (string)ChildrenNodes[3].Token.Value;
            this.adapterType = AddChild(NodeUseType.Parameter, SR.AdapterTypeRole, ChildrenNodes[4]) as AstNodeBase;
            this.asWord = (string)ChildrenNodes[5].Token.Value;
            this.parameters = AddChild(NodeUseType.Parameter, SR.ParametersRole, ChildrenNodes[6]) as AstNodeBase;
            this.referenceWord = (string)ChildrenNodes[7].Token.Value;
            this.reference = AddChild(NodeUseType.Parameter, SR.ReferenceRole, ChildrenNodes[8]) as AstNodeBase;

            this.result = new PlanNode();
            this.result.Column = ChildrenNodes[0].Token.Location.Column;
            this.result.Line = ChildrenNodes[0].Token.Location.Line;
            this.result.NodeType = (uint)Integra.Vision.Engine.Commands.CommandTypeEnum.CreateAdapter;
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
            PlanNode auxIdAdapter = (PlanNode)this.idAdapter.Evaluate(thread);
            PlanNode auxAdapterType = (PlanNode)this.adapterType.Evaluate(thread);
            List<PlanNode> auxParameters = (List<PlanNode>)this.parameters.Evaluate(thread);
            PlanNode auxReference = (PlanNode)this.reference.Evaluate(thread);
            this.EndEvaluate(thread);

            PlanNode paramListNode = new PlanNode();
            paramListNode.Children = new List<PlanNode>();
            string paramListText = string.Empty;
            foreach (PlanNode param in auxParameters)
            {
                paramListNode.Children.Add(param);
                paramListText += " " + param.NodeText + " ";
            }

            this.result.NodeText = SR.AdapterNodeText(this.createWord, this.adapterWord, auxIdAdapter.NodeText, this.forWord, auxAdapterType.NodeText, this.asWord, paramListText, this.referenceWord, auxReference.NodeText);

            this.result.Children = new System.Collections.Generic.List<PlanNode>();
            this.result.Children.Add(auxIdAdapter);
            this.result.Children.Add(auxAdapterType);
            this.result.Children.Add(paramListNode);
            this.result.Children.Add(auxReference);

            return this.result;
        }
    }
}
