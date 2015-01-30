//-----------------------------------------------------------------------
// <copyright file="EventPropertiesNode.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------}
namespace Integra.Vision.Language.ASTNodes.Objects.Event
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Event;
    using Integra.Vision.Language.ASTNodes.Base;
    using Integra.Vision.Language.Resources;
    using Irony.Ast;
    using Irony.Interpreter;
    using Irony.Interpreter.Ast;
    using Irony.Parsing;

    /// <summary>
    /// EventPropertiesNode class
    /// </summary>
    internal sealed class EventPropertiesNode : AstNodeBase
    {
        /// <summary>
        /// event node
        /// </summary>
        private AstNodeBase ev;

        /// <summary>
        /// property of the event
        /// </summary>
        private string property;

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

            this.ev = AddChild(NodeUseType.Parameter, SR.EventRole, ChildrenNodes[0]) as AstNodeBase;
            this.property = (string)ChildrenNodes[2].Token.Value;
            this.result = new PlanNode();
            this.result.Column = ChildrenNodes[1].Token.Location.Column;
            this.result.Line = ChildrenNodes[1].Token.Location.Line;
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
            PlanNode auxEvent = (PlanNode)this.ev.Evaluate(thread);
            this.EndEvaluate(thread);

            this.result.NodeType = PlanNodeTypeEnum.Property;
            this.result.NodeText = auxEvent.NodeText + "." + this.property;
            this.result.Children = new List<PlanNode>();
            this.result.Children.Add(auxEvent);
            this.result.Properties.Add(SR.PrpoertyProperty, this.property);

            if (this.property.ToLower() == SR.TimestampProperty)
            {
                this.result.Properties.Add(SR.DataTypeProperty, typeof(DateTime).ToString());
            }
            else if (this.property.ToLower() == SR.NameProperty)
            {
                this.result.Properties.Add(SR.DataTypeProperty, typeof(string).ToString());
            }
            else if (this.property.ToLower() == SR.AdapterProperty)
            {
                this.result.Properties.Add(SR.DataTypeProperty, typeof(EventAdapter).ToString());
            }
            else if (this.property.ToLower() == SR.AgentProperty)
            {
                this.result.Properties.Add(SR.DataTypeProperty, typeof(EventAgent).ToString());
            }
            else if (this.property.ToLower() == SR.MessageProperty)
            {
                this.result.Properties.Add(SR.DataTypeProperty, typeof(EventMessage).ToString());
                this.result.NodeType = PlanNodeTypeEnum.ObjectMessage;
            }
            else
            {
                this.result.Properties.Add(SR.DataTypeProperty, typeof(string).ToString());
            }

            return this.result;
        }
    }
}
