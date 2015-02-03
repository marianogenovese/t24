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
    using Integra.Vision.Event;
    using Integra.Vision.Language.Exceptions;

    /// <summary>
    /// ExpressionConstructor class
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:ClosingParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1111:ClosingParenthesisMustBeOnLineOfLastParameter", Justification = "Reviewed.")]
    internal sealed class ExpressionConstructor
    {
        /// <summary>
        /// Parameter list
        /// </summary>
        private List<ParameterExpression> parameterList = new List<ParameterExpression>();

        /// <summary>
        /// Object prefix
        /// </summary>
        private string objectPrefix;

        /// <summary>
        /// Get the compiled function
        /// </summary>
        /// <param name="type">Return type</param>
        /// <param name="expression">Expression tree</param>
        /// <returns>Compiled function</returns>
        public object GetCompiledFunction(string type, Expression expression)
        {
            if (this.parameterList.Count == 0)
            {
                return this.CreateFunctionWithoutParameters(type, expression);
            }
            else if (this.parameterList.Count == 1)
            {
                return this.CreateFunctionWithOneParameter(type, expression);
            }
            else if (this.parameterList.Count == 2)
            {
                return this.CreateFunctionWithTwoParameters(type, expression);
            }
            else
            {
                throw new Exception("El numero de parametros es incorrecto.");
            }
        }

        /// <summary>
        /// GetSelectValues
        /// Gets the projection
        /// </summary>
        /// <param name="plan">Plan that contains the projection plans</param>
        /// <returns>Dictionary of functions</returns>
        public IDictionary<string, object> GetSelectValues(PlanNode plan)
        {
            this.parameterList = new List<ParameterExpression>();

            Expression result = this.GenerateSelectExpression(plan);
            return result.CompileExpression<Func<IDictionary<string, object>>>(this.parameterList)();
        }

        /// <summary>
        /// CompileSelect
        /// Doc go here
        /// </summary>
        /// <param name="plan">Plan to compile</param>
        /// <returns>compiled function</returns>
        public Func<EventObject, IDictionary<string, object>> CompileSelect(PlanNode plan)
        {
            this.parameterList = new List<ParameterExpression>();

            Expression result = this.GenerateSelectExpression(plan);

            if (this.parameterList.Count < 1)
            {
                this.parameterList.Add(Expression.Parameter(typeof(EventObject), "otro"));
            }

            return result.CompileExpression<Func<EventObject, Dictionary<string, object>>>(this.parameterList);
        }

        /// <summary>
        /// CompileJoinSelect
        /// Doc go here
        /// </summary>
        /// <param name="plan">Plan to compile</param>
        /// <returns>compiled function</returns>
        public Func<EventObject, EventObject, IDictionary<string, object>> CompileJoinSelect(PlanNode plan)
        {
            this.parameterList = new List<ParameterExpression>();

            Expression result = this.GenerateSelectExpression(plan);

            if (this.parameterList.Count < 1)
            {
                this.parameterList.Add(Expression.Parameter(typeof(EventObject), "otro"));
            }

            if (this.parameterList.Count < 2)
            {
                this.parameterList.Add(Expression.Parameter(typeof(EventObject), "otroMas"));
            }

            return result.CompileExpression<Func<EventObject, EventObject, Dictionary<string, object>>>(this.parameterList);
        }

        /// <summary>
        /// GetWhereValues
        /// Doc go here
        /// </summary>
        /// <param name="plan">Plan that contains the where conditions</param>
        /// <returns>Result of the where condition</returns>
        public bool GetWhereValues(PlanNode plan)
        {
            this.parameterList = new List<ParameterExpression>();

            Expression resultExp = this.GenerateExpressionTree(plan);
            return resultExp.CompileExpression<Func<bool>>(this.parameterList)();
        }

        /// <summary>
        /// CompileWhere
        /// Doc go here
        /// </summary>
        /// <param name="plan">Plan to compile</param>
        /// <returns>Compiled function</returns>
        public Func<EventObject, bool> CompileWhere(PlanNode plan)
        {
            this.parameterList = new List<ParameterExpression>();

            Expression resultExp = this.GenerateExpressionTree(plan);

            if (this.parameterList.Count < 1)
            {
                this.parameterList.Add(Expression.Parameter(typeof(EventObject), "otro"));
            }

            return resultExp.CompileExpression<Func<EventObject, bool>>(this.parameterList);
        }

        /// <summary>
        /// CompileJoinWhere
        /// Doc go here
        /// </summary>
        /// <param name="plan">Plan to compile</param>
        /// <returns>Compiled function</returns>
        public Func<EventObject, EventObject, bool> CompileJoinWhere(PlanNode plan)
        {
            this.parameterList = new List<ParameterExpression>();

            Expression resultExp = this.GenerateExpressionTree(plan);

            if (this.parameterList.Count < 1)
            {
                this.parameterList.Add(Expression.Parameter(typeof(EventObject), "otro"));
            }

            if (this.parameterList.Count < 2)
            {
                this.parameterList.Add(Expression.Parameter(typeof(EventObject), "otroMas"));
            }

            return resultExp.CompileExpression<Func<EventObject, EventObject, bool>>(this.parameterList);
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="plan">plan node to convert</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerateExpressionTree(PlanNode plan)
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
        private Expression CreateExpressionNode(PlanNode actualNode, Expression leftNode, Expression rightNode)
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
                case PlanNodeTypeEnum.ObjectWithPrefix:
                    return this.GetEventWithPrefixValue(actualNode, leftNode, rightNode);
                case PlanNodeTypeEnum.Event:
                    return this.GenerateEvent(actualNode);
                case PlanNodeTypeEnum.ObjectPrefix:
                    return this.GenerateObjectPrefix(actualNode);
                case PlanNodeTypeEnum.DateTimeFunction:
                    return this.GenerateDateFunction(actualNode, leftNode);
                default:
                    return Expression.Constant(null);
            }
        }

        /// <summary>
        /// Creates a function based on a expression tree with two parameter.
        /// </summary>
        /// <param name="type">Type to return</param>
        /// <param name="expression">Expression tree</param>
        /// <returns>Compiled function</returns>
        private object CreateFunctionWithTwoParameters(string type, Expression expression)
        {
            try
            {
                switch (type)
                {
                    case "System.String":
                        return Expression.Lambda<Func<EventObject, EventObject, string>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.Boolean":
                        return Expression.Lambda<Func<EventObject, EventObject, bool>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.DateTime":
                        return Expression.Lambda<Func<EventObject, EventObject, DateTime>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.Object":
                        return Expression.Lambda<Func<EventObject, EventObject, object>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.Int16":
                        return Expression.Lambda<Func<EventObject, EventObject, short>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.Int32":
                        return Expression.Lambda<Func<EventObject, EventObject, int>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.Int64":
                        return Expression.Lambda<Func<EventObject, EventObject, long>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.Decimal":
                        return Expression.Lambda<Func<EventObject, EventObject, decimal>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.Double":
                        return Expression.Lambda<Func<EventObject, EventObject, double>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.Float":
                        return Expression.Lambda<Func<EventObject, EventObject, float>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.TimeSpan":
                        return Expression.Lambda<Func<EventObject, EventObject, TimeSpan>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.Collections.Generic.Dictionary`2[System.String,System.Object]":
                        return expression.CompileExpression<Func<EventObject, EventObject, Dictionary<string, object>>>(this.parameterList);
                    default:
                        return Expression.Lambda<Func<object>>(expression).Compile()();
                }
            }
            catch (Exception e)
            {
                // aqui tambien entran errores de Exceptions
                return Expression.Lambda<Func<object>>(Expression.Constant(null)).Compile()();
            }
        }

        /// <summary>
        /// Creates a function based on a expression tree with one parameter.
        /// </summary>
        /// <param name="type">Type to return</param>
        /// <param name="expression">Expression tree</param>
        /// <returns>Compiled function</returns>
        private object CreateFunctionWithOneParameter(string type, Expression expression)
        {
            try
            {
                switch (type)
                {
                    case "System.String":
                        return Expression.Lambda<Func<EventObject, string>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.Boolean":
                        return Expression.Lambda<Func<EventObject, bool>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.DateTime":
                        return Expression.Lambda<Func<EventObject, DateTime>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.Object":
                        return Expression.Lambda<Func<EventObject, object>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.Int16":
                        return Expression.Lambda<Func<EventObject, short>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.Int32":
                        return Expression.Lambda<Func<EventObject, int>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.Int64":
                        return Expression.Lambda<Func<EventObject, long>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.Decimal":
                        return Expression.Lambda<Func<EventObject, decimal>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.Double":
                        return Expression.Lambda<Func<EventObject, double>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.Float":
                        return Expression.Lambda<Func<EventObject, float>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.TimeSpan":
                        return Expression.Lambda<Func<EventObject, TimeSpan>>(expression, this.parameterList.ToArray()).Compile();
                    case "System.Collections.Generic.Dictionary`2[System.String,System.Object]":
                        return expression.CompileExpression<Func<EventObject, Dictionary<string, object>>>(this.parameterList);
                    default:
                        return Expression.Lambda<Func<object>>(expression).Compile()();
                }
            }
            catch (Exception e)
            {
                // aqui tambien entran errores de Exceptions
                return Expression.Lambda<Func<object>>(Expression.Constant(null)).Compile()();
            }
        }

        /// <summary>
        /// Creates a function based on a expression tree without parameters.
        /// </summary>
        /// <param name="type">Type to return</param>
        /// <param name="expression">Expression tree</param>
        /// <returns>Compiled function</returns>
        private object CreateFunctionWithoutParameters(string type, Expression expression)
        {
            try
            {
                Type a = typeof(string);
                switch (type)
                {
                    case "System.String":
                        return expression.CompileExpression<Func<string>>(this.parameterList);
                    case "System.Boolean":
                        return expression.CompileExpression<Func<bool>>(this.parameterList);
                    case "System.DateTime":
                        return expression.CompileExpression<Func<DateTime>>(this.parameterList);
                    case "System.Object":
                        return expression.CompileExpression<Func<object>>(this.parameterList);
                    case "System.Int16":
                        return expression.CompileExpression<Func<short>>(this.parameterList);
                    case "System.Int32":
                        return expression.CompileExpression<Func<int>>(this.parameterList);
                    case "System.Int64":
                        return expression.CompileExpression<Func<long>>(this.parameterList);
                    case "System.Decimal":
                        return expression.CompileExpression<Func<decimal>>(this.parameterList);
                    case "System.Double":
                        return expression.CompileExpression<Func<double>>(this.parameterList);
                    case "System.Float":
                        return expression.CompileExpression<Func<float>>(this.parameterList);
                    case "System.TimeSpan":
                        return expression.CompileExpression<Func<TimeSpan>>(this.parameterList);
                    case "System.Collections.Generic.Dictionary`2[System.String,System.Object]":
                        return expression.CompileExpression<Func<Dictionary<string, object>>>(this.parameterList);
                    default:
                        return expression.CompileExpression<Func<object>>(this.parameterList);
                }
            }
            catch (Exception e)
            {
                // aqui tambien entran errores de Exceptions
                return Expression.Lambda<Func<object>>(Expression.Constant(null)).Compile()();
            }
        }

        /// <summary>
        /// Generate the select block expression
        /// </summary>
        /// <param name="plans">Plan that contains the projection plans</param>
        /// <returns>Select block expression</returns>
        private Expression GenerateSelectExpression(PlanNode plans)
        {
            ConstantExpression dictionaryExpression = Expression.Constant(new Dictionary<string, object>(), typeof(Dictionary<string, object>));
            List<Expression> expressionList = new List<Expression>();

            foreach (var plan in plans.Children)
            {
                Expression key = this.GenerateExpressionTree(plan.Children[0]);
                Expression value = this.GenerateExpressionTree(plan.Children[1]);

                Expression tryCatchExpr =
                    Expression.Block(
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Generando columna de proyección '" + plan.Children[0].NodeText + "' con valor: '" + plan.Children[1].NodeText + "'")),
                                Expression.Call(dictionaryExpression, typeof(Dictionary<string, object>).GetMethod("Add", new Type[] { typeof(string), value.GetType() }), new[] { Expression.ConvertChecked(key, typeof(string)), Expression.ConvertChecked(value, typeof(object)) }),
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Columna de proyección generada")),
                                Expression.Empty()
                                ),
                            Expression.Catch(
                                typeof(Exception),
                                Expression.Block(
                                    Expression.Throw(
                                        Expression.New(
                                            typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                            Expression.Constant("Error al agregar los valores al diccionario, linea: " + plan.Line + " columna: " + plan.Column + " con " + plan.NodeText)
                                        )
                                    )
                                )
                            )
                        ),
                        dictionaryExpression
                        );

                expressionList.Add(tryCatchExpr);
            }

            return Expression.Block(expressionList.ToArray());
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
        /// <param name="actualNode">plan node to convert</param>
        /// <param name="leftNode">Left expression</param>
        /// <param name="rightNode">Right expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GetEventWithPrefixValue(PlanNode actualNode, Expression leftNode, Expression rightNode)
        {
            return rightNode;
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">plan node to convert</param>
        /// <returns>expression tree of actual plan</returns>
        private ParameterExpression GenerateEvent(PlanNode actualNode)
        {
            ParameterExpression currentParameter = null;
            bool existsParameter = false;
            string parameterName;

            if (this.objectPrefix == null)
            {
                parameterName = actualNode.Properties["Value"].ToString();
            }
            else
            {
                parameterName = this.objectPrefix;
            }

            foreach (ParameterExpression parameter in this.parameterList)
            {
                if (parameter.Name.Equals(parameterName))
                {
                    currentParameter = parameter;
                    existsParameter = true;
                }
            }

            if (!existsParameter)
            {
                currentParameter = Expression.Parameter(typeof(EventObject), parameterName);
                this.parameterList.Add(currentParameter);
            }

            return currentParameter;
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
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion aritmetica de resta para timespan '-' entre los siguientes valores: ")),
                                    Expression.Assign(param, Expression.Call(leftNode, typeof(DateTime).GetMethod("Subtract", new Type[] { typeof(DateTime) }), rightNode)),
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la expresion aritmetica de resta para timespan '-'")),
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
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion aritmetica de resta '-' entre los siguientes valores: ")),
                                    Expression.Assign(param, Expression.Subtract(leftNode, rightNode)),
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la expresion aritmetica de resta '-'")),
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
                    throw new CompilationException("Error al compilar la expresiones aritmetica de resta", e);
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
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion aritmetica de negacion '-' entre los siguientes valores: ")),
                                Expression.Assign(param, Expression.Negate(leftNode)),
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion aritmetica de negacion '-' entre los siguientes valores: ")),
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
                throw new CompilationException("Error al compilar la expresion unaria de negacion", e);
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
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion logica 'And' entre los siguientes valores: ")),
                                Expression.Assign(param, Expression.AndAlso(leftNode, rightNode)),
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la expresion logica 'And'")),
                                Expression.Empty()
                                ),
                            Expression.Catch(
                                typeof(Exception),
                                Expression.Block(
                                    Expression.Throw(
                                        Expression.New(
                                            typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                        Expression.Constant("Error con la expresion booleana 'and' en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)))
                                )
                            )
                        ),
                        param
                        );

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                throw new CompilationException("Error al compilar la expresion logica 'and'", e);
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
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion logica 'Or' entre los siguientes valores: ")),
                                    Expression.Assign(param, Expression.OrElse(leftNode, rightNode)),
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la expresion logica 'Or'")),
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
                throw new CompilationException("Error al compilar la expresion logica 'or'", e);
            }
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="leftNode">left child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerateDateFunction(PlanNode actualNode, Expression leftNode)
        {
            Type tipo = (Type)actualNode.Properties["DataType"];
            string propiedad = actualNode.Properties["Property"].ToString();

            ParameterExpression param = Expression.Variable(tipo, "variable");

            if (leftNode.Type.GetProperty(propiedad) != null)
            {
                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.IfThenElse(
                            Expression.Equal(Expression.Constant(leftNode.Type.GetProperty(propiedad)), Expression.Constant(null)),
                            Expression.Assign(param, Expression.Default(tipo)),
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Se obtendrá la siguiente propiedad: " + propiedad)),
                                    Expression.Assign(param, Expression.Call(leftNode, leftNode.Type.GetProperty(propiedad).GetMethod)),
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Se obtuvo la siguiente propiedad: " + propiedad)),
                                    Expression.Empty()
                                ),
                                Expression.Catch(
                                    typeof(Exception),
                                    Expression.Block(
                                        Expression.Throw(
                                            Expression.New(
                                                typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                                Expression.Constant("Error al compilar la funcion '" + propiedad + "', no es posible obtener el valor solicitado. Error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)
                                            )
                                        )
                                    )
                                )
                            )
                        ),
                        param);

                return tryCatchExpr;
            }
            else
            {
                throw new CompilationException("Error al compilar la funcion '" + propiedad + "', no es posible obtener el valor solicitado. Error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText);
            }
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="leftNode">left child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerateProperty(PlanNode actualNode, Expression leftNode)
        {
            Type tipo = (Type)actualNode.Properties["DataType"];
            string propiedad = actualNode.Properties["Property"].ToString();

            try
            {
                ParameterExpression param = Expression.Variable(tipo, "variable");

                if (leftNode.Type.GetProperty(propiedad) != null)
                {
                    Expression tryCatchExpr =
                        Expression.Block(
                            new[] { param },
                            Expression.IfThenElse(
                                Expression.Equal(Expression.Constant(leftNode.Type.GetProperty(propiedad)), Expression.Constant(null)),
                                Expression.Assign(param, Expression.Default(tipo)),
                                Expression.TryCatch(
                                    Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Se obtendrá la siguiente propiedad: " + propiedad)),
                                        Expression.Assign(param, Expression.Call(leftNode, leftNode.Type.GetProperty(propiedad).GetMethod)),
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Se obtuvo la siguiente propiedad: " + propiedad)),
                                        Expression.Empty()
                                    ),
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
                                )
                            ),
                            param);

                    return tryCatchExpr;
                }

                return Expression.Constant(null);
            }
            catch (Exception e)
            {
                throw new CompilationException("Error al compilar la propiedad", e);
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
            try
            {
                ParameterExpression param = Expression.Variable(typeof(object), "variable");

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.IfThenElse(
                            Expression.Equal(objeto, Expression.Constant(null)),
                            Expression.Assign(param, Expression.Constant(null)),
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Se obtendra el valor del objeto " + actualNode.NodeText)),
                                    Expression.Assign(param, Expression.Call(objeto, typeof(MessageSubsection).GetMethod("get_Value"))),
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("Write", new Type[] { typeof(object) }), Expression.Constant("**El valor del objeto " + actualNode.NodeText + " es: ")),
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), param),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    typeof(Exception),
                                        Expression.Throw(
                                            Expression.New(
                                                typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                                Expression.Constant("No fue posible obtener el valor del campo del mensaje, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)))
                                )
                            )
                        ),
                        param
                        );

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                throw new CompilationException("Error al compilar, no fue posible obtener el valor del mensaje", e);
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
            ParameterExpression param = Expression.Variable(typeof(EventMessage), "message");

            try
            {
                Expression mensaje =
                        Expression.Block(
                            new ParameterExpression[] { param },
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Obteniendo el mensaje")),
                                    Expression.Assign(param, Expression.Property(eventNode, "Message")),
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("**Se obtuvo el mensaje"))
                                ),
                                Expression.Catch(
                                    typeof(Exception),
                                    Expression.Throw(
                                        Expression.New(
                                            typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                            Expression.Constant("No fue posible obtener la propiedad 'Message' del evento, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)
                                        )
                                    )
                                )
                            ),
                            param
                            );

                return mensaje;
            }
            catch (Exception e)
            {
                throw new CompilationException("Error al compilar, no fue posible obtener el mensaje del evento", e);
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
            ParameterExpression v = Expression.Variable(typeof(MessageSubsection), "bloque");
            ConstantExpression auxSubField = (ConstantExpression)subFieldId;
            Type tipo = auxSubField.Type;

            try
            {
                methodCall = Expression.Block(
                    new ParameterExpression[] { v },
                    Expression.IfThen(
                        Expression.Call(
                            field,
                            typeof(MessageSubsection).GetMethod("Contains", new Type[] { tipo }),
                            auxSubField
                        ),
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Obteniendo el subcampo: " + auxSubField.Value)),
                                Expression.Assign(v, Expression.Call(field, typeof(MessageSubsection).GetMethod("get_Item", new Type[] { tipo }), auxSubField)),
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("**Se obtuvo el subcampo: " + auxSubField.Value))
                            ),
                            Expression.Catch(
                                typeof(Exception),
                                Expression.Throw(
                                    Expression.New(
                                        typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                        Expression.Constant("No fue posible obtener la subsección de la subsección del objeto, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)
                                    )
                                )
                            )
                        )
                        ),
                    v);
            }
            catch (Exception e)
            {
                throw new CompilationException("Error al compilar, no fue posible obtener la subsección de la subsección del mensaje", e);
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
            ParameterExpression v = Expression.Variable(typeof(MessageSubsection), "campoDeParte");
            ConstantExpression auxField = (ConstantExpression)fieldId;
            Type tipo = auxField.Type;

            try
            {
                Expression methodCall = Expression.Block(
                    new ParameterExpression[] { v },
                    Expression.IfThen(
                        Expression.Call(
                            part,
                            typeof(MessageSection).GetMethod("Contains", new Type[] { tipo }),
                            auxField
                        ),
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Obteniendo el campo: " + auxField.Value)),
                                Expression.Assign(v, Expression.Call(part, typeof(MessageSection).GetMethod("get_Item", new Type[] { tipo }), auxField)),
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("**Se obtuvo el campo: " + auxField.Value))
                            ),
                            Expression.Catch(
                                typeof(Exception),
                                Expression.Throw(
                                    Expression.New(
                                        typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                        Expression.Constant("No fue posible obtener la subsección de la sección del objeto, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)
                                    )
                                )
                            )
                        )
                        ),
                     v);

                return methodCall;
            }
            catch (Exception e)
            {
                throw new CompilationException("Error al compilar, no fue posible obtener la subsección de la sección del mensaje", e);
            }
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
            ParameterExpression v = Expression.Parameter(typeof(Event.MessageSection), "bloque");
            ConstantExpression auxPart = (ConstantExpression)partId;
            Type tipo = auxPart.Type;

            try
            {
                Expression parte = Expression.Block(
                    new ParameterExpression[] { v },
                    Expression.TryCatch(
                        Expression.IfThen(
                            Expression.Call(
                                mensaje,
                                typeof(EventMessage).GetMethod("Contains", new Type[] { tipo }),
                                auxPart
                            ),
                            Expression.Block(
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Obteniendo la parte: " + auxPart.Value)),
                                Expression.Assign(v, Expression.Call(mensaje, typeof(EventMessage).GetMethod("get_Item", new Type[] { tipo }), auxPart)),
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("**Se obtuvo la parte: " + auxPart.Value))
                            )
                        ),
                        Expression.Catch(
                                typeof(Exception),
                                    Expression.Throw(
                                        Expression.New(
                                            typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                            Expression.Constant("No fue posible obtener la sección del objeto, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)))
                            )
                        ),
                        v);

                return parte;
            }
            catch (Exception e)
            {
                throw new CompilationException("Error al compilar, no fue posible obtener la sección del mensaje", e);
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
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion 'like' con ambos comodines entre los siguientes valores: ")),
                                    Expression.Assign(param, Expression.Call(leftNode, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), Expression.Constant(cadenaAComparar))),
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la expresion de comparacion 'like' con ambos comodines")),
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
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion 'like' con comodin izquierdo entre los siguientes valores: ")),
                                    Expression.Assign(param, Expression.Call(leftNode, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), Expression.Constant(cadenaAComparar))),
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la expresion de comparacion 'like' con comodin izquierdo")),
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
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion 'like' con comodin derecho entre los siguientes valores: ")),
                                    Expression.Assign(param, Expression.Call(leftNode, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), Expression.Constant(cadenaAComparar))),
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la expresion de comparacion 'like' con comodin derecho")),
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
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion 'like' sin comodines entre los siguientes valores: ")),
                                    Expression.Assign(param, Expression.Call(leftNode, typeof(string).GetMethod("Equals", new Type[] { typeof(string) }), Expression.Constant(cadenaAComparar))),
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la expresion de comparacion 'like' sin comodines")),
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
                throw new CompilationException("Error al compilar, no fue posible compilar la expresion 'like'", e);
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
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion logica 'not' del siguiente valor: ")),
                                Expression.Assign(param, Expression.Not(leftNode)),
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la expresion logica 'not'")),
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
                throw new CompilationException("Error al compilar, no fue compilar la expresion 'not'", e);
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
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion '>=' entre los siguientes valores: ")),
                                Expression.Assign(param, Expression.GreaterThanOrEqual(leftNode, rightNode)),
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la condicion '>='")),
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
                throw new CompilationException("Error al compilar, no fue posible compilar la expresion '>='", e);
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
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion '>' entre los siguientes valores: ")),
                                Expression.Assign(param, Expression.GreaterThan(leftNode, rightNode)),
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la condicion '>'")),
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
                throw new CompilationException("Error al compilar, no fue posible compilar la expresion '>'", e);
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
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion '<=' entre los siguientes valores: ")),
                                Expression.Assign(param, Expression.LessThanOrEqual(leftNode, rightNode)),
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la condicion '<='")),
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
                throw new CompilationException("Error al compilar, no fue posible compilar la expresion '<='", e);
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
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion '<' entre los siguientes valores: ")),
                                Expression.Assign(param, Expression.LessThan(leftNode, rightNode)),
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la condicion '<'")),
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
                throw new CompilationException("Error al compilar, no fue posible compilar la expresion '<'", e);
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
                        new ParameterExpression[] { param },
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion '!=' entre los siguientes valores: ")),
                                Expression.Assign(param, Expression.NotEqual(leftNode, rightNode)),
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la condicion de No igualdad")),
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
                throw new CompilationException("Error al compilar, no fue posible compilar la expresion '!='", e);
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
                        new ParameterExpression[] { param },
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion '==' entre los siguientes valores: ")),
                                Expression.Assign(param, Expression.Equal(leftNode, rightNode)),
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Finalizacion de la condicion de igualdad")),
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
                throw new CompilationException("Error al compilar, no fue posible compilar la expresion '=='", e);
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
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Unbox a " + typeToCast + " del siguiente valor: ")),
                                Expression.Assign(retorno, Expression.Unbox(leftNode, typeToCast)),
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la operacion de 'Unbox' hacia el tipo " + typeToCast)),
                                Expression.Empty()
                            ),
                        Expression.Catch(
                                typeof(Exception),
                                    Expression.Throw(
                                        Expression.New(
                                            typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                            Expression.Constant("Error de casteo en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText + "Tipos incompatibles: " + originalType + " con " + typeToCast.ToString())))
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
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Convert a " + typeToCast + " del siguiente valor: ")),
                                    Expression.Assign(retorno, Expression.Convert(leftNode, typeToCast)),
                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la operacion de 'Convert' hacia el tipo " + typeToCast)),
                                    Expression.Empty()
                                ),
                            Expression.Catch(
                                typeof(Exception),
                                Expression.Block(
                                        Expression.Throw(
                                            Expression.New(
                                                typeof(Exception).GetConstructor(new Type[] { typeof(string) }),
                                                Expression.Constant("Error de casteo en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText + "Tipos incompatibles: " + originalType + " con " + typeToCast.ToString())))
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

        /// <summary>
        /// Get the object prefix value
        /// </summary>
        /// <param name="actualNode">Current node</param>
        /// <returns>Object prefix expression</returns>
        private Expression GenerateObjectPrefix(PlanNode actualNode)
        {
            this.objectPrefix = actualNode.Properties["Value"].ToString();
            return Expression.Constant(actualNode.Properties["Value"], Type.GetType(actualNode.Properties["DataType"].ToString()));
        }
    }
}
