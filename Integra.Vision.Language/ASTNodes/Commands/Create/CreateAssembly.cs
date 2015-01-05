//-----------------------------------------------------------------------
// <copyright file="CreateAssembly.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
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
    /// CreateAssembly class
    /// </summary>
    internal sealed class CreateAssembly : AstNodeBase
    {
        /// <summary>
        /// reserved word
        /// </summary>
        private string createWord;

        /// <summary>
        /// reserved word
        /// </summary>
        private string assemblyWord;

        /// <summary>
        /// name of the source
        /// </summary>
        private AstNodeBase idAssembly;

        /// <summary>
        /// reserved word
        /// </summary>
        private string fromWord;

        /// <summary>
        /// path to the assembly
        /// </summary>
        private AstNodeBase path;

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
            this.assemblyWord = (string)ChildrenNodes[1].Token.Value;
            this.idAssembly = AddChild(NodeUseType.Parameter, SR.UserDefinedObjectRole, ChildrenNodes[2]) as AstNodeBase;
            this.fromWord = (string)ChildrenNodes[3].Token.Value;
            this.path = AddChild(NodeUseType.Parameter, SR.AssemblyPathRole, ChildrenNodes[4]) as AstNodeBase;

            this.result = new PlanNode();
            this.result.Column = ChildrenNodes[0].Token.Location.Column;
            this.result.Line = ChildrenNodes[0].Token.Location.Line;
            this.result.NodeType = PlanNodeTypeEnum.CreateAssembly;
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
            PlanNode auxIdAssembly = (PlanNode)this.idAssembly.Evaluate(thread);
            PlanNode auxPath = (PlanNode)this.path.Evaluate(thread);
            this.EndEvaluate(thread);

            this.result.NodeText = SR.AssemblyNodeText(this.createWord, this.assemblyWord, auxIdAssembly.NodeText, this.fromWord, auxPath.NodeText);
            
            this.result.Children = new System.Collections.Generic.List<PlanNode>();
            this.result.Children.Add(auxIdAssembly);
            this.result.Children.Add(auxPath);

            return this.result;
        }
    }
}
