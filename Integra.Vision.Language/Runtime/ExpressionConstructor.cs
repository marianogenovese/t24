//-----------------------------------------------------------------------
// <copyright file="ExpressionConstructor.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    
    using Integra.Vision.Language.General;

    /// <summary>
    /// ExpressionConstructor class
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:ClosingParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1111:ClosingParenthesisMustBeOnLineOfLastParameter", Justification = "Reviewed.")]
    internal sealed class ExpressionConstructor
    {
        /// <summary>
        /// Event parameter
        /// </summary>
        private ParameterExpression eventParameter;

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="plan">plan node to convert</param>
        /// <returns>expression tree of actual plan</returns>
        public Expression GenerateExpressionTree(PlanNode plan)
        {
            if (plan == null)
            {
                return null;
            }

            Expression leftExp = null;
            Expression rightExp = null;

            if (plan.Children != null)
            {
                if (plan.Children.Count() == 2)
                {
                    leftExp = this.GenerateExpressionTree(plan.Children.First<PlanNode>());
                    rightExp = this.GenerateExpressionTree(plan.Children.ElementAt<PlanNode>(1));
                }
                else if (plan.Children.Count() == 1)
                {
                    leftExp = this.GenerateExpressionTree(plan.Children.First<PlanNode>());
                }
            }

            return this.CreateExpressionNode(plan, leftExp, rightExp);
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="leftNode">left child expression</param>
        /// <param name="rightNode">right child expression</param>
        /// <returns>expression tree of actual plan</returns>
        public Expression CreateExpressionNode(PlanNode actualNode, Expression leftNode, Expression rightNode)
        {
            PlanNodeTypeEnum nodeType = actualNode.NodeType;

            switch (nodeType)
            {
                case PlanNodeTypeEnum.Constant:
                    return this.GenerateConstant(actualNode);
                case PlanNodeTypeEnum.Cast:
                    return this.GenerateCast(actualNode, leftNode);
                case PlanNodeTypeEnum.Equal:
                    return this.GenerarEqual(actualNode, leftNode, rightNode);
                case PlanNodeTypeEnum.NotEqual:
                    return this.GenerarNotEqual(actualNode, leftNode, rightNode);
                case PlanNodeTypeEnum.LessThan:
                    return this.GenerarLessThan(actualNode, leftNode, rightNode);
                case PlanNodeTypeEnum.LessThanOrEqual:
                    return this.GenerarLessThanOrEqual(actualNode, leftNode, rightNode);
                case PlanNodeTypeEnum.GreaterThan:
                    return this.GenerarGreaterThan(actualNode, leftNode, rightNode);
                case PlanNodeTypeEnum.GreaterThanOrEqual:
                    return this.GenerarGreaterThanOrEqual(actualNode, leftNode, rightNode);
                case PlanNodeTypeEnum.Not:
                    return this.GenerarNot(actualNode, leftNode);
                case PlanNodeTypeEnum.Like:
                    return this.GenerarLike(actualNode, leftNode, rightNode);
                case PlanNodeTypeEnum.Or:
                    return this.GenerateOr(actualNode, leftNode, rightNode);
                case PlanNodeTypeEnum.And:
                    return this.GenerateAnd(actualNode, leftNode, rightNode);
                case PlanNodeTypeEnum.Event:
                    return this.GenerateEvent(actualNode);
                case PlanNodeTypeEnum.ObjectPart:
                    return this.GenerateObjectPart(actualNode, leftNode, rightNode);
                case PlanNodeTypeEnum.ObjectField:
                    if (actualNode.Children.First<PlanNode>().NodeType.Equals(PlanNodeTypeEnum.ObjectPart))
                    {
                        return this.GenerateObjectFieldFromPart(actualNode, leftNode, rightNode);
                    }
                    else
                    {
                        return this.GenerateObjectFieldFromField(actualNode, leftNode, rightNode);
                    }

                case PlanNodeTypeEnum.ObjectValue:
                    return this.GenerateValueOfObject(actualNode, leftNode);
                case PlanNodeTypeEnum.ObjectMessage:
                    return this.GenerateObjectMessage(actualNode, (ParameterExpression)leftNode);
                case PlanNodeTypeEnum.Negate:
                    return this.GenerateNegate(actualNode, leftNode);
                case PlanNodeTypeEnum.Subtract:
                    return this.GenerateSubtract(actualNode, leftNode, rightNode);
                case PlanNodeTypeEnum.Property:
                    return this.GenerateProperty(actualNode, leftNode);
                case PlanNodeTypeEnum.From:
                    return this.GenerateFrom(actualNode, leftNode);
                default:
                    return Expression.Constant(null);
            }
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="plan">plan node to convert</param>
        /// <returns>expression tree of actual plan</returns>
        public object GetConstantExpressionCompiled(PlanNode plan)
        {
            return this.GetExpressionCompiled(plan, new EventObject());
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="plan">actual plan</param>
        /// <param name="eventObject">incoming event</param>
        /// <returns>expression tree of actual plan</returns>
        public object GetExpressionCompiled(PlanNode plan, EventObject eventObject)
        {
            Expression resultExp = this.GenerateExpressionTree(plan);

            if (!plan.Properties.ContainsKey("DataType"))
            {
                return Expression.Lambda<Func<object>>(resultExp).Compile()();
            }

            if (this.eventParameter == null)
            {
                this.eventParameter = Expression.Parameter(typeof(EventObject));
            }

            string a = resultExp.ToString();
            try
            {
                switch (plan.Properties["DataType"].ToString())
                {
                    case "System.String":
                        return Expression.Lambda<Func<EventObject, string>>(resultExp, new[] { this.eventParameter }).Compile()(eventObject);
                    case "System.Boolean":
                        return Expression.Lambda<Func<EventObject, bool>>(resultExp, new[] { this.eventParameter }).Compile()(eventObject);
                    case "System.DateTime":
                        return Expression.Lambda<Func<EventObject, DateTime>>(resultExp, new[] { this.eventParameter }).Compile()(eventObject);
                    case "System.Object":
                        return Expression.Lambda<Func<EventObject, object>>(resultExp, new[] { this.eventParameter }).Compile()(eventObject);
                    case "System.Int16":
                        return Expression.Lambda<Func<EventObject, short>>(resultExp, new[] { this.eventParameter }).Compile()(eventObject);
                    case "System.Int32":
                        return Expression.Lambda<Func<EventObject, int>>(resultExp, new[] { this.eventParameter }).Compile()(eventObject);
                    case "System.Int64":
                        return Expression.Lambda<Func<EventObject, long>>(resultExp, new[] { this.eventParameter }).Compile()(eventObject);
                    case "System.Decimal":
                        return Expression.Lambda<Func<EventObject, decimal>>(resultExp, new[] { this.eventParameter }).Compile()(eventObject);
                    case "System.Double":
                        return Expression.Lambda<Func<EventObject, double>>(resultExp, new[] { this.eventParameter }).Compile()(eventObject);
                    case "System.Float":
                        return Expression.Lambda<Func<EventObject, float>>(resultExp, new[] { this.eventParameter }).Compile()(eventObject);
                    case "System.TimeSpan":
                        return Expression.Lambda<Func<EventObject, TimeSpan>>(resultExp, new[] { this.eventParameter }).Compile()(eventObject);
                    default:
                        return Expression.Lambda<Func<object>>(resultExp).Compile()();
                }
            }
            catch (Exception e)
            {
                // aqui tambien entran errores de Exceptions
                return Expression.Lambda<Func<object>>(Expression.Constant(null)).Compile()();
            }
        }

        /// <summary>
        /// GetSelectValues
        /// Gets the projection
        /// </summary>
        /// <param name="plans">Plan that contains the projection plans</param>
        /// <param name="eventObject">Incoming event</param>
        /// <returns>Dictionary of functions</returns>
        public Dictionary<string, object> GetSelectValues(PlanNode plans, EventObject eventObject)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (var plan in plans.Children)
            {
                var key = this.GetExpressionCompiled(plan.Children[0], eventObject);
                var value = this.GetExpressionCompiled(plan.Children[1], eventObject);
                result.Add(key.ToString(), value);
            }

            return result;
        }

        /// <summary>
        /// CompileSelect
        /// Doc go here
        /// </summary>
        /// <param name="plan">Plan to compile</param>
        /// <returns>compiled function</returns>
        public Func<EventObject, IDictionary<string, object>> CompileSelect(PlanNode plan)
        {
            return null;
        }

        /// <summary>
        /// CompileJoinSelect
        /// Doc go here
        /// </summary>
        /// <param name="plan">Plan to compile</param>
        /// <returns>compiled function</returns>
        public Func<EventObject, EventObject, IDictionary<string, object>> CompileJoinSelect(PlanNode plan)
        {
            return null;
        }

        /// <summary>
        /// CompileWhere
        /// Doc go here
        /// </summary>
        /// <param name="plan">Plan to compile</param>
        /// <returns>compiled function</returns>
        public Func<EventObject, bool> CompileWhere(PlanNode plan)
        {
            return null;
        }

        /// <summary>
        /// CompileJoinWhere
        /// Doc go here
        /// </summary>
        /// <param name="plan">Plan to compile</param>
        /// <returns>compiled function</returns>
        public Func<EventObject, EventObject, bool> CompileJoinWhere(PlanNode plan)
        {
            return null;
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="leftNode">left child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerateFrom(PlanNode actualNode, Expression leftNode)
        {
            return leftNode;
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="leftNode">left child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerateProperty(PlanNode actualNode, Expression leftNode)
        {
            Type tipo = Type.GetType(actualNode.Properties["DataType"].ToString());
            string propiedad = actualNode.Properties["Property"].ToString();

            try
            {
                ParameterExpression param = Expression.Variable(tipo, "variable");

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.Assign(
                                    param,
                                    Expression.Property(leftNode, propiedad)),
                                Expression.Empty()),
                            Expression.Catch(
                                typeof(Exception),
                                Expression.Block(
                                    Expression.Throw(
                                        Expression.New(
                                            typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                            Expression.Constant("No fue posible obtener la propiedad " + propiedad + ", error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)
                                        )
                                    )
                                )
                            )
                        ),
                        param);

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                return Expression.Constant(null);
            }
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">plan node to convert</param>
        /// <returns>expression tree of actual plan</returns>
        private ParameterExpression GenerateEvent(PlanNode actualNode)
        {
            if (this.eventParameter == null)
            {
                this.eventParameter = Expression.Parameter(typeof(EventObject), "EventObjectParameter");
            }

            return this.eventParameter;
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="leftNode">left child expression</param>
        /// <param name="rightNode">right child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerateSubtract(PlanNode actualNode, Expression leftNode, Expression rightNode)
        {
            Type tipo = Type.GetType(actualNode.Properties["DataType"].ToString());

            if (tipo.Equals(typeof(TimeSpan)))
            {
                try
                {
                    ParameterExpression param = Expression.Variable(tipo, "variable");

                    Expression tryCatchExpr =
                        Expression.Block(
                            new[] { param },
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.Assign(param, Expression.Call(leftNode, typeof(DateTime).GetMethod("Subtract", new Type[] { typeof(DateTime) }), rightNode)),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    typeof(Exception),
                                    Expression.Block(
                                        Expression.Throw(
                                            Expression.New(
                                                typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                                Expression.Constant("Error con la expresion aritmetica de resta '-' en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)
                                            )
                                        )
                                    )
                                )
                            ),
                            param
                            );

                    return tryCatchExpr;
                }
                catch (Exception e)
                {
                    return Expression.Constant(null);
                }
            }
            else
            {
                try
                {
                    ParameterExpression param = Expression.Variable(tipo, "variable");

                    Expression tryCatchExpr =
                        Expression.Block(
                            new[] { param },
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.Assign(param, Expression.Subtract(leftNode, rightNode)),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    typeof(Exception),
                                    Expression.Block(
                                        Expression.Throw(
                                            Expression.New(
                                                typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                                Expression.Constant("Error con la expresion aritmetica de resta '-' en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)
                                            )
                                        )
                                    )
                                )
                            ),
                            param
                            );

                    return tryCatchExpr;
                }
                catch (Exception e)
                {
                    return Expression.Constant(null);
                }
            }
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="leftNode">left child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerateNegate(PlanNode actualNode, Expression leftNode)
        {
            Type tipo;

            if (!actualNode.Properties.ContainsKey("DataType"))
            {
                tipo = typeof(decimal);
            }
            else
            {
                tipo = Type.GetType(actualNode.Properties["DataType"].ToString());
            }

            try
            {
                ParameterExpression param = Expression.Variable(tipo, "variable");

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.Assign(param, Expression.Negate(leftNode)),
                                Expression.Empty()
                                ),
                            Expression.Catch(
                                typeof(Exception),
                                Expression.Block(
                                    Expression.Throw(
                                        Expression.New(
                                            typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                            Expression.Constant("Error con la expresion aritmetica unaria de negacion '-' en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)
                                        )
                                    )
                                )
                            )
                        ),
                        param
                        );

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                return Expression.Constant(null);
            }
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="leftNode">left child expression</param>
        /// <param name="rightNode">right child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerateAnd(PlanNode actualNode, Expression leftNode, Expression rightNode)
        {
            try
            {
                ParameterExpression param = Expression.Variable(typeof(bool), "variable");

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.Assign(param, Expression.AndAlso(leftNode, rightNode)),
                                Expression.Empty()
                                ),
                            Expression.Catch(
                                typeof(Exception),
                                Expression.Block(
                                    Expression.Throw(
                                        Expression.New(
                                            typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                        Expression.Constant("Error con la expresion booleana 'or' en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)))
                                )
                            )
                        ),
                        param
                        );

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                return Expression.Constant(null);
            }
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="leftNode">left child expression</param>
        /// <param name="rightNode">right child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerateOr(PlanNode actualNode, Expression leftNode, Expression rightNode)
        {
            try
            {
                ParameterExpression param = Expression.Variable(typeof(bool), "variable");

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.Assign(param, Expression.OrElse(leftNode, rightNode)),
                                Expression.Empty()
                                ),
                            Expression.Catch(
                                typeof(Exception),
                                Expression.Block(
                                    Expression.Throw(
                                        Expression.New(
                                            typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                            Expression.Constant("Error con la expresion booleana 'or' en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)))
                                )
                            )
                        ),
                        param
                        );

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                return Expression.Constant(null);
            }
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="objeto">left child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerateValueOfObject(PlanNode actualNode, Expression objeto)
        {
            Expression valor = Expression.Constant(null);

            try
            {
                ParameterExpression param = Expression.Variable(typeof(object), "variable");

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.Assign(param, Expression.Call(objeto, typeof(Integra.Messaging.MessageField).GetMethod("get_Value"))),
                                Expression.Empty()
                                ),
                            Expression.Catch(
                                typeof(Exception),
                                    Expression.Throw(
                                        Expression.New(
                                            typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                            Expression.Constant("No fue posible obtener el valor del campo del mensaje, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)))
                            )
                        ),
                        param
                        );

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                return Expression.Constant(null);
            }
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="eventNode">left child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerateObjectMessage(PlanNode actualNode, ParameterExpression eventNode)
        {
            try
            {
                Expression mensaje = Expression.Block(
                                        Expression.Property(
                                            eventNode,
                                            "Message"
                                         )
                 );

                return mensaje;
            }
            catch (Exception e)
            {
                return Expression.Constant(new Integra.Messaging.MessagePart(-1, "There is no object"));
            }
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="field">left child expression</param>
        /// <param name="subFieldId">right child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerateObjectFieldFromField(PlanNode actualNode, Expression field, Expression subFieldId)
        {
            Expression methodCall = Expression.Constant(null);
            ParameterExpression v = Expression.Variable(typeof(Integra.Messaging.MessageField), "bloque");
            ConstantExpression auxSubField = (ConstantExpression)subFieldId;
            Type tipo = auxSubField.Type;

            try
            {
                methodCall = Expression.Block(
                    new ParameterExpression[] { v },
                    Expression.IfThenElse(
                        Expression.Call(
                        field,
                        typeof(Integra.Messaging.MessageField).GetMethod("Contains", new Type[] { tipo }),
                        auxSubField
                        ),
                        Expression.Assign(
                        v,
                        Expression.Call(
                            field,
                            typeof(Integra.Messaging.MessageField).GetMethod("get_Item", new Type[] { tipo }),
                            auxSubField
                        )
                        ),
                        Expression.Throw(
                            Expression.New(
                                typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                Expression.Constant("El subcampo no existe")
                            )
                        )),
                    v);
            }
            catch (Exception e)
            {
                return Expression.Constant(new Integra.Messaging.MessageField(-1, "There is no object") { Value = null });
            }

            return methodCall;
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="part">left child expression</param>
        /// <param name="fieldId">right child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerateObjectFieldFromPart(PlanNode actualNode, Expression part, Expression fieldId)
        {
            Expression methodCall = Expression.Constant(null);
            ParameterExpression v = Expression.Variable(typeof(Integra.Messaging.MessageField), "campoDeParte");
            ConstantExpression auxField = (ConstantExpression)fieldId;
            Type tipo = auxField.Type;

            try
            {
                methodCall = Expression.Block(
                    new ParameterExpression[] { v },
                    Expression.IfThenElse(
                        Expression.Call(
                        part,
                        typeof(Integra.Messaging.MessagePart).GetMethod("Contains", new Type[] { tipo }),
                        auxField
                        ),
                        Expression.Assign(
                        v, 
                        Expression.Call(
                            part,
                            typeof(Integra.Messaging.MessagePart).GetMethod("get_Item", new Type[] { tipo }),
                            auxField
                        )
                        ),
                        Expression.Throw(
                            Expression.New(
                                typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                Expression.Constant("El campo no existe no existe"))
                        )),
                    v);
            }
            catch (Exception e)
            {
                return Expression.Constant(new Integra.Messaging.MessageField(-1, "There is no object") { Value = null });
            }

            return methodCall;
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="mensaje">left child expression</param>
        /// <param name="partId">right child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerateObjectPart(PlanNode actualNode, Expression mensaje, Expression partId)
        {
            Expression parte = Expression.Constant(null);
            ParameterExpression v = Expression.Variable(typeof(Integra.Messaging.MessagePart), "bloque");
            ConstantExpression auxPart = (ConstantExpression)partId;
            Type tipo = auxPart.Type;

            try
            {
                parte = Expression.Block(
                    new ParameterExpression[] { v },
                    Expression.TryCatch(
                        Expression.IfThenElse(
                            Expression.Call(
                                mensaje,
                                typeof(Integra.Messaging.Message).GetMethod("Contains", new Type[] { tipo }),
                                auxPart
                            ),
                            Expression.Assign(
                                v,
                                Expression.Call(
                                    mensaje,
                                    typeof(Integra.Messaging.Message).GetMethod("get_Item", new Type[] { tipo }),
                                    auxPart
                                )
                            ),
                                    Expression.Throw(
                                        Expression.New(
                                            typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                            Expression.Constant("No fue posible negar la expresión de comparación, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)))
                        ),
                        Expression.Catch(
                                typeof(Exception),
                                    Expression.Throw(
                                        Expression.New(
                                            typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                            Expression.Constant("No fue posible negar la expresión de comparación, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)))
                            )
                    ),
                    v);

                return parte;
            }
            catch (Exception e)
            {
                return Expression.Constant(new Integra.Messaging.MessagePart(-1, "There is no object"));
            }
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="leftNode">left child expression</param>
        /// <param name="rightNode">right child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerarLike(PlanNode actualNode, Expression leftNode, Expression rightNode)
        {
            Expression resultExp = Expression.Constant(null);

            try
            {
                string cadenaLike = rightNode.ToString().Replace("\"", string.Empty);
                string cadenaAComparar = cadenaLike.Replace("%", string.Empty);
                ParameterExpression param = Expression.Variable(typeof(bool), "variable");

                if (cadenaLike.StartsWith("%") && cadenaLike.EndsWith("%"))
                {
                    Expression tryCatchExpr =
                        Expression.Block(
                            new[] { param },
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.Assign(param, Expression.Call(leftNode, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), Expression.Constant(cadenaAComparar))),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    typeof(Exception),
                                    Expression.Block(
                                        Expression.Throw(
                                            Expression.New(
                                                typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                                Expression.Constant("No fue posible realizar la operación like '%...%', error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)))
                                    )
                                )
                            ),
                            param
                            );

                    return tryCatchExpr;
                }
                else if (cadenaLike.StartsWith("%"))
                {
                    Expression tryCatchExpr =
                        Expression.Block(
                            new[] { param },
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.Assign(param, Expression.Call(leftNode, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), Expression.Constant(cadenaAComparar))),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    typeof(Exception),
                                    Expression.Block(
                                        Expression.Throw(
                                            Expression.New(
                                                typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                                Expression.Constant("No fue posible realizar la operación like '%..., error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)))
                                    )
                                )
                            ),
                            param
                            );

                    return tryCatchExpr;
                }
                else if (cadenaLike.EndsWith("%"))
                {
                    Expression tryCatchExpr =
                        Expression.Block(
                            new[] { param },
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.Assign(param, Expression.Call(leftNode, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), Expression.Constant(cadenaAComparar))),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    typeof(Exception),
                                    Expression.Block(
                                        Expression.Throw(
                                            Expression.New(
                                                typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                                Expression.Constant("No fue posible realizar la operación like '...%, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)))
                                    )
                                )
                            ),
                            param
                            );

                    return tryCatchExpr;
                }
                else
                {
                    Expression tryCatchExpr =
                        Expression.Block(
                            new[] { param },
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.Assign(param, Expression.Call(leftNode, typeof(string).GetMethod("Equals", new Type[] { typeof(string) }), Expression.Constant(cadenaAComparar))),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    typeof(Exception),
                                    Expression.Block(
                                        Expression.Throw(
                                            Expression.New(
                                                typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                                Expression.Constant("No fue posible realizar la operación like sin comodines, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)))
                                    )
                                )
                            ),
                            param
                            );

                    return tryCatchExpr;
                }
            }
            catch (Exception e)
            {
                return Expression.Constant(null);
            }
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="leftNode">left child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerarNot(PlanNode actualNode, Expression leftNode)
        {
            try
            {
                ParameterExpression param = Expression.Variable(typeof(bool), "variable");

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.Assign(param, Expression.Not(leftNode)),
                                Expression.Empty()
                                ),
                            Expression.Catch(
                                typeof(Exception),
                                Expression.Block(
                                    Expression.Throw(
                                        Expression.New(
                                            typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                            Expression.Constant("No fue posible negar la expresión de comparación, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)))
                                )
                            )
                        ),
                        param
                        );

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                return Expression.Constant(null);
            }
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="leftNode">left child expression</param>
        /// <param name="rightNode">right child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerarGreaterThanOrEqual(PlanNode actualNode, Expression leftNode, Expression rightNode)
        {
            try
            {
                ParameterExpression param = Expression.Variable(typeof(bool), "variable");

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.Assign(param, Expression.GreaterThanOrEqual(leftNode, rightNode)),
                                Expression.Empty()
                                ),
                            Expression.Catch(
                                typeof(Exception),
                                Expression.Block(
                                    Expression.Throw(
                                        Expression.New(
                                            typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                            Expression.Constant("No fue posible realizar la operación mayor o igual que '>=', error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)))
                                )
                            )
                        ),
                        param
                        );

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                return Expression.Constant(null);
            }
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="leftNode">left child expression</param>
        /// <param name="rightNode">right child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerarGreaterThan(PlanNode actualNode, Expression leftNode, Expression rightNode)
        {
            try
            {
                ParameterExpression param = Expression.Variable(typeof(bool), "variable");

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.Assign(param, Expression.GreaterThan(leftNode, rightNode)),
                                Expression.Empty()
                                ),
                            Expression.Catch(
                                typeof(Exception),
                                Expression.Block(
                                    Expression.Throw(
                                        Expression.New(
                                            typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                            Expression.Constant("No fue posible realizar la operación mayor que '>', error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)))
                                )
                            )
                        ),
                        param
                        );

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                return Expression.Constant(null);
            }
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="leftNode">left child expression</param>
        /// <param name="rightNode">right child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerarLessThanOrEqual(PlanNode actualNode, Expression leftNode, Expression rightNode)
        {
            try
            {
                ParameterExpression param = Expression.Variable(typeof(bool), "variable");

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.Assign(param, Expression.LessThanOrEqual(leftNode, rightNode)),
                                Expression.Empty()
                                ),
                            Expression.Catch(
                                typeof(Exception),
                                Expression.Block(
                                    Expression.Throw(
                                        Expression.New(
                                            typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                            Expression.Constant("No fue posible realizar la operación menor o igual que '<=', error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)))
                                )
                            )
                        ),
                        param
                        );

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                return Expression.Constant(null);
            }
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="leftNode">left child expression</param>
        /// <param name="rightNode">right child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerarLessThan(PlanNode actualNode, Expression leftNode, Expression rightNode)
        {
            try
            {
                ParameterExpression param = Expression.Variable(typeof(bool), "variable");

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.Assign(param, Expression.LessThan(leftNode, rightNode)),
                                Expression.Empty()
                                ),
                            Expression.Catch(
                                typeof(Exception),
                                Expression.Block(
                                    Expression.Throw(
                                        Expression.New(
                                            typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                            Expression.Constant("No fue posible realizar la operación menor que '<', error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)))
                                )
                            )
                        ),
                        param
                        );

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                return Expression.Constant(null);
            }
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="leftNode">left child expression</param>
        /// <param name="rightNode">right child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerarNotEqual(PlanNode actualNode, Expression leftNode, Expression rightNode)
        {
            try
            {
                ParameterExpression param = Expression.Variable(typeof(bool), "variable");

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.Assign(param, Expression.NotEqual(leftNode, rightNode)),
                                Expression.Empty()
                                ),
                            Expression.Catch(
                                typeof(Exception),
                                Expression.Block(
                                    Expression.Throw(
                                        Expression.New(
                                            typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                            Expression.Constant("Error con la expresion de desigualdad en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)))
                                )
                            )
                        ),
                        param
                        );

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                return Expression.Constant(null);
            }
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="leftNode">left child expression</param>
        /// <param name="rightNode">right child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerarEqual(PlanNode actualNode, Expression leftNode, Expression rightNode)
        {
            try
            {
                ParameterExpression param = Expression.Variable(typeof(bool), "variable");

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.Assign(param, Expression.Equal(leftNode, rightNode)),
                                Expression.Empty()
                                ),
                            Expression.Catch(
                                typeof(Exception),
                                Expression.Block(
                                    Expression.Throw(
                                        Expression.New(
                                            typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                            Expression.Constant("Error con la expresion de igualdad en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)))
                                )
                            )
                        ),
                        param
                        );

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                return Expression.Constant(null);
            }
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="leftNode">left child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerateCast(PlanNode actualNode, Expression leftNode)
        {
            Expression resultExp = Expression.Constant(null);
            Type typeToCast = Type.GetType(actualNode.Properties["DataType"].ToString());
            string originalType = actualNode.Children.First<PlanNode>().Properties["DataType"].ToString();

            try
            {
                ParameterExpression retorno = Expression.Parameter(typeToCast);
                resultExp =
                Expression.Block(
                    new[] { retorno },
                    Expression.TryCatch(
                        Expression.Block(
                                Expression.Assign(retorno, Expression.Unbox(leftNode, typeToCast)),
                                Expression.Empty()
                            ),
                        Expression.Catch(
                                typeof(Exception),
                                    Expression.Throw(
                                        Expression.New(
                                            typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                            Expression.Constant("Error de casteo en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText + "\nTipos incompatibles: " + originalType + " con " + typeToCast.ToString())))
                            )
                    ),
                    retorno
                );
            }
            catch (Exception e)
            {
                try
                {
                    ParameterExpression retorno = Expression.Parameter(typeToCast);
                    resultExp =
                    Expression.Block(
                        new[] { retorno },
                        Expression.TryCatch(
                            Expression.Block(
                                    Expression.Assign(retorno, Expression.Convert(leftNode, typeToCast)),
                                    Expression.Empty()
                                ),
                            Expression.Catch(
                                typeof(Exception),
                                Expression.Block(
                                        Expression.Throw(
                                            Expression.New(
                                                typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                                Expression.Constant("Error de casteo en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText + "\nTipos incompatibles: " + originalType + " con " + typeToCast.ToString())))
                                )
                            )
                        ),
                        retorno
                    );
                }
                catch (Exception ex)
                {
                    resultExp = Expression.Constant(null);
                }
            }

            return resultExp;
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="plan">plan node to convert</param>
        /// <returns>expression tree of actual plan</returns>
        private ConstantExpression GenerateConstant(PlanNode plan)
        {
            if (!plan.Properties.ContainsKey("DataType"))
            {
                return Expression.Constant(null);
            }

            return Expression.Constant(plan.Properties["Value"], Type.GetType(plan.Properties["DataType"].ToString()));
        }        
    }
}
