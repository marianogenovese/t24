//-----------------------------------------------------------------------
// <copyright file="CreateTriggerNode.cs" company="Integra.Vision.Language">
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
    /// CreateTriggerNode class
    /// </summary>
    internal sealed class CreateTriggerNode : AstNodeBase
    {
        /// <summary>
        /// reserved word create
        /// </summary>
        private string createWord;

        /// <summary>
        /// reserved word trigger
        /// </summary>
        private string triggerWord;

        /// <summary>
        /// trigger name node
        /// </summary>
        private AstNodeBase triggerName;

        /// <summary>
        /// reserved word on
        /// </summary>
        private string onWord;

        /// <summary>
        /// stream name node
        /// </summary>
        private AstNodeBase streamName;

        /// <summary>
        /// reserved word as
        /// </summary>
        private string asWord;

        /// <summary>
        /// send expression list
        /// </summary>
        private AstNodeBase sendExpressionList;

        /// <summary>
        /// apply window node
        /// </summary>
        private AstNodeBase applyWindow;

        /// <summary>
        /// list of send expressions or if expressions
        /// </summary>
        private AstNodeBase ifSendList;

        /// <summary>
        /// result plan node
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
            this.triggerWord = (string)ChildrenNodes[1].Token.Value;
            this.triggerName = AddChild(NodeUseType.Parameter, " ", ChildrenNodes[2]) as AstNodeBase;
            this.onWord = (string)ChildrenNodes[3].Token.Value;
            this.streamName = AddChild(NodeUseType.Parameter, SR.ReferenceRole, ChildrenNodes[4]) as AstNodeBase;

            int childrenCount = ChildrenNodes.Count;
            if (childrenCount == 7)
            {
                this.asWord = (string)ChildrenNodes[5].Token.Value;
                this.sendExpressionList = AddChild(NodeUseType.Parameter, SR.SendRole, ChildrenNodes[6]) as AstNodeBase;
            }
            else if (childrenCount == 8)
            {
                this.applyWindow = AddChild(NodeUseType.Parameter, SR.ApplyWindowRole, ChildrenNodes[5]) as AstNodeBase;
                this.asWord = (string)ChildrenNodes[6].Token.Value;
                this.ifSendList = AddChild(NodeUseType.Parameter, SR.SendRole, ChildrenNodes[7]) as AstNodeBase;
            }

            this.result = new PlanNode();
            this.result.Column = ChildrenNodes[0].Token.Location.Column;
            this.result.Line = ChildrenNodes[0].Token.Location.Line;
            this.result.NodeType = PlanNodeTypeEnum.CreateTrigger;
        }

        /// <summary>
        /// DoEvaluate
        /// Doc go here ¬¬
        /// </summary>
        /// <param name="thread">Thread of the evaluated grammar</param>
        /// <returns>return a plan node</returns>
        protected override object DoEvaluate(ScriptThread thread)
        {
            this.BeginEvaluate(thread);

            PlanNode triggerNameAux = (PlanNode)this.triggerName.Evaluate(thread);
            PlanNode streamNameAux = (PlanNode)this.streamName.Evaluate(thread);

            int childrenCount = ChildrenNodes.Count;
            if (childrenCount == 7)
            {
                List<PlanNode> sendExpressionsAux = (List<PlanNode>)this.sendExpressionList.Evaluate(thread);

                this.result.NodeText = SR.TriggerNodeText1(this.createWord, this.triggerWord, triggerNameAux.NodeText, this.onWord, streamNameAux.NodeText, this.asWord);

                this.result.Children = new System.Collections.Generic.List<PlanNode>();
                this.result.Children.Add(triggerNameAux);
                this.result.Children.Add(streamNameAux);

                PlanNode planSendList = new PlanNode();
                planSendList.Children = new List<PlanNode>();
                foreach (PlanNode plan in sendExpressionsAux)
                {
                    planSendList.Children.Add(plan);
                    planSendList.NodeText += " " + plan.NodeText;
                } 

                this.result.Children.Add(planSendList);
                this.result.NodeText += " " + planSendList.NodeText;
            }
            else if (childrenCount == 8)
            {
                PlanNode applyWindowAux = (PlanNode)this.applyWindow.Evaluate(thread);
                List<PlanNode> ifSendListAux = (List<PlanNode>)this.ifSendList.Evaluate(thread);

                this.result.NodeText = SR.TriggerNodeText2(this.createWord, this.triggerWord, triggerNameAux.NodeText, this.onWord, streamNameAux.NodeText, applyWindowAux.NodeText, this.asWord);

                this.result.Children = new System.Collections.Generic.List<PlanNode>();
                this.result.Children.Add(triggerNameAux);
                this.result.Children.Add(streamNameAux);
                this.result.Children.Add(applyWindowAux);

                PlanNode planIfList = new PlanNode();
                planIfList.Children = new List<PlanNode>();
                foreach (PlanNode plan in ifSendListAux)
                {
                    planIfList.Children.Add(plan);
                    planIfList.NodeText += " " + plan.NodeText;
                }

                this.result.Children.Add(planIfList);
                this.result.NodeText += " " + planIfList.NodeText;
            }

            this.EndEvaluate(thread);
            
            return this.result;
        }
    }
}
