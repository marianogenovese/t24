//-----------------------------------------------------------------------
// <copyright file="ObservableConstructor.cs" company="Integra.Vision.Language">
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
    using System.Reactive.Linq;
    using System.Reflection;
    using Exceptions;
    using Integra.Vision.Event;

    /// <summary>
    /// Observable constructor class
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:ClosingParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1111:ClosingParenthesisMustBeOnLineOfLastParameter", Justification = "Reviewed.")]
    internal class ObservableConstructor
    {
        /// <summary>
        /// Flag to indicate if the log will be print in console
        /// </summary>
        private bool printLog = false;

        /// <summary>
        /// List to store parameter expressions
        /// </summary>
        private List<ParameterExpression> parameterList;

        /// <summary>
        /// Stack to store scope variables
        /// </summary>
        private Stack<ParameterExpression> scopeParam;

        /// <summary>
        /// Group expression
        /// </summary>
        private Expression groupExpression;

        /// <summary>
        /// Object prefix
        /// </summary>
        private string objectPrefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableConstructor"/> class
        /// </summary>
        /// <param name="printLog">Log print flag</param>
        public ObservableConstructor(bool printLog = false)
        {
            this.printLog = printLog;
            this.scopeParam = new Stack<ParameterExpression>();
            this.parameterList = new List<ParameterExpression>();
        }

        /// <summary>
        /// Creates a delegate
        /// </summary>
        /// <param name="plan">Execution plan</param>
        /// <param name="inputType">Input type.</param>
        /// <param name="outputType">Output type.</param>
        /// <returns>Result delegate.</returns>
        public Delegate Compile(PlanNode plan, Type inputType, Type outputType)
        {
            var delegateType = typeof(Func<,>).MakeGenericType(inputType, outputType);
            Delegate result = Expression.Lambda(delegateType, this.GenerateExpressionTree(plan), this.parameterList.ToArray()).Compile();

            Console.WriteLine("La función fue compilada exitosamente.");
            return result;
        }

        /// <summary>
        /// Compile the result function.
        /// </summary>
        /// <typeparam name="In">Input type.</typeparam>
        /// <typeparam name="Out">Output type.</typeparam>
        /// <param name="plan">Execution plan.</param>
        /// <returns>Result function.</returns>
        public Func<In, Out> Compile<In, Out>(PlanNode plan)
        {
            Func<In, Out> funcResult = this.CreateLambda<In, Out>(plan).Compile();
            Console.WriteLine("La función fue compilada exitosamente.");
            return funcResult;
        }

        /// <summary>
        /// Compile the result function.
        /// </summary>
        /// <typeparam name="Out">Output type.</typeparam>
        /// <param name="plan">Execution plan.</param>
        /// <returns>Result function.</returns>
        public Func<Out> Compile<Out>(PlanNode plan)
        {
            Func<Out> funcResult = this.CreateLambda<Out>(plan).Compile();
            Console.WriteLine("La función fue compilada exitosamente.");
            return funcResult;
        }

        /// <summary>
        /// Creates a lambda expression
        /// </summary>
        /// <typeparam name="In">Input type</typeparam>
        /// <typeparam name="Out">Output type</typeparam>
        /// <param name="plan">Execution plan</param>
        /// <returns>Expression lambda</returns>
        public Expression<Func<In, Out>> CreateLambda<In, Out>(PlanNode plan)
        {
            Expression<Func<In, Out>> result = Expression.Lambda<Func<In, Out>>(this.GenerateExpressionTree(plan), this.parameterList.ToArray());
            return result;
        }

        /// <summary>
        /// Creates a lambda expression
        /// </summary>
        /// <typeparam name="Out">Output type</typeparam>
        /// <param name="plan">Execution plan</param>
        /// <returns>Expression lambda</returns>
        public Expression<Func<Out>> CreateLambda<Out>(PlanNode plan)
        {
            Expression<Func<Out>> result = Expression.Lambda<Func<Out>>(this.GenerateExpressionTree(plan), this.parameterList.ToArray());
            return result;
        }

        /// <summary>
        /// Create a expression tree.
        /// </summary>
        /// <param name="plan">plan node to convert.</param>
        /// <returns>expression tree of actual plan.</returns>
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

                    // se hace porque la proyección es un árbol independiente y solo el resultado, un lambda expression, debe ser el nodo derecho
                    if (plan.Children.ElementAt<PlanNode>(1).NodeType.Equals(PlanNodeTypeEnum.Projection))
                    {
                        rightExp = this.CreateProjectionExpression(plan.Children.ElementAt<PlanNode>(1));
                    }
                    else
                    {
                        rightExp = this.GenerateExpressionTree(plan.Children.ElementAt<PlanNode>(1));
                    }
                }
                else if (plan.Children.Count() == 1)
                {
                    leftExp = this.GenerateExpressionTree(plan.Children.First<PlanNode>());
                }
            }

            return this.CreateExpressionNode(plan, leftExp, rightExp);
        }

        /// <summary>
        /// Creates a expression tree.
        /// </summary>
        /// <param name="actualNode">Actual plan.</param>
        /// <param name="leftNode">Left child expression.</param>
        /// <param name="rightNode">Right child expression.</param>
        /// <returns>Expression tree of actual plan.</returns>
        private Expression CreateExpressionNode(PlanNode actualNode, Expression leftNode, Expression rightNode)
        {
            PlanNodeTypeEnum nodeType = actualNode.NodeType;
            Expression expResult;

            switch (nodeType)
            {
                case PlanNodeTypeEnum.Constant:
                    expResult = this.GenerateConstant(actualNode);
                    break;
                case PlanNodeTypeEnum.Projection:
                    expResult = this.CreateProjectionExpression(actualNode);
                    break;
                case PlanNodeTypeEnum.EnumerableCount:
                    Func<Expression, Expression> enumerableCountMethod = this.CreateEnumerableCount<int>;
                    expResult = enumerableCountMethod.Method.GetGenericMethodDefinition()
                                            .MakeGenericMethod(new[] { leftNode.Type })
                                            .Invoke(this, new object[] { leftNode }) as Expression;
                    break;
                case PlanNodeTypeEnum.EnumerableSum:
                    Func<Expression, Expression, Expression> enumerableSumMethod = this.CreateEnumerableSum<int>;
                    expResult = enumerableSumMethod.Method.GetGenericMethodDefinition()
                                            .MakeGenericMethod(new[] { leftNode.Type })
                                            .Invoke(this, new object[] { leftNode, rightNode }) as Expression;
                    break;
                case PlanNodeTypeEnum.ObservableWhere:
                    Func<Expression, Expression, Expression> whereMethod = this.CreateWhere<int>;
                    expResult = whereMethod.Method.GetGenericMethodDefinition()
                                            .MakeGenericMethod(new[] { leftNode.Type.GetGenericArguments()[0] })
                                            .Invoke(this, new object[] { leftNode, rightNode }) as Expression;
                    break;
                case PlanNodeTypeEnum.ObservableGroupBy:
                    Func<Expression, Expression, Expression> groupbyMethod = this.CreateGroupBy<int, int>;
                    expResult = groupbyMethod.Method.GetGenericMethodDefinition()
                                            .MakeGenericMethod(new[] { leftNode.Type.GetGenericArguments()[0], rightNode.Type.GetGenericArguments()[1] })
                                            .Invoke(this, new object[] { leftNode, rightNode }) as Expression;
                    break;
                case PlanNodeTypeEnum.ObservableSelectForGroupBy:
                    Func<Expression, Expression, Expression> selectForGroupByMethod = this.CreateSelectForGroupBy<int, int>;
                    expResult = selectForGroupByMethod.Method.GetGenericMethodDefinition()
                                            .MakeGenericMethod(new[] { leftNode.Type.GetGenericArguments()[0], rightNode.Type.GetGenericArguments()[0] })
                                            .Invoke(this, new object[] { leftNode, rightNode }) as Expression;
                    break;
                case PlanNodeTypeEnum.ObservableSelectForBuffer:
                    Func<PlanNode, Expression, Expression, Expression> selectForBufferMethod = this.CreateSelectForBuffer<int, int>;
                    expResult = selectForBufferMethod.Method.GetGenericMethodDefinition()
                                            .MakeGenericMethod(new[] { leftNode.Type.GetGenericArguments()[0], rightNode.Type.GetGenericArguments()[1] })
                                            .Invoke(this, new object[] { actualNode, leftNode, rightNode }) as Expression;
                    break;
                case PlanNodeTypeEnum.ObservableBuffer:
                    Func<Expression, Expression, Expression> bufferMethod = this.CreateBuffer<int>;
                    expResult = bufferMethod.Method.GetGenericMethodDefinition()
                                            .MakeGenericMethod(new[] { leftNode.Type.GetGenericArguments()[1] })
                                            .Invoke(this, new object[] { leftNode, rightNode }) as Expression;
                    break;
                case PlanNodeTypeEnum.ObservableFrom:
                    expResult = this.CreateFrom(actualNode, (ConstantExpression)leftNode);
                    break;
                case PlanNodeTypeEnum.ObservableFromForLambda:
                    expResult = this.CreateFromForLambda(actualNode, leftNode);
                    break;
                case PlanNodeTypeEnum.ObservableMerge:
                    Func<Expression, Expression> mergeMethod = this.CreateMerge<int, int>;
                    expResult = mergeMethod.Method.GetGenericMethodDefinition()
                                            .MakeGenericMethod(new[] { leftNode.Type, leftNode.Type.GetGenericArguments()[0] })
                                            .Invoke(this, new object[] { leftNode }) as Expression;
                    break;
                case PlanNodeTypeEnum.NewScope:
                    this.CreateNewScope(leftNode);
                    return leftNode;
                case PlanNodeTypeEnum.GroupKey:
                    expResult = this.GenerateGroupKey(actualNode);
                    break;
                case PlanNodeTypeEnum.GroupKeyProperty:
                    expResult = this.GenerateGroupKeyProperty(actualNode, leftNode);
                    break;
                case PlanNodeTypeEnum.GroupKeyValue:
                    expResult = this.GenerateGroupPropertyValue(actualNode, leftNode);
                    break;
                /********************************************************************************************************************************************************************************************************************/
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
                case PlanNodeTypeEnum.Event:
                    return this.GenerateEvent(actualNode);
                case PlanNodeTypeEnum.ObjectPrefix:
                    return this.GenerateObjectPrefix(actualNode);
                case PlanNodeTypeEnum.DateTimeFunction:
                    return this.GenerateDateFunction(actualNode, leftNode);
                case PlanNodeTypeEnum.Negate:
                    return this.GenerateNegate(actualNode, leftNode);
                case PlanNodeTypeEnum.Subtract:
                    return this.GenerateSubtract(actualNode, leftNode, rightNode);
                case PlanNodeTypeEnum.Property:
                    return this.GenerateProperty(actualNode, leftNode);
                case PlanNodeTypeEnum.ObjectWithPrefix:
                    return this.GetEventWithPrefixValue(actualNode, leftNode, rightNode);
                default:
                    throw new CompilationException("Error en el arbol de ejecución, el tipo de nodo no existe.");
            }

            return expResult;
        }

        /// <summary>
        /// Creates the source parameter expression.
        /// </summary>
        /// <param name="actualNode">Actual plan node.</param>
        /// <param name="source">Name of the source.</param>
        /// <returns>Source parameter expression.</returns>
        private Expression CreateFrom(PlanNode actualNode, ConstantExpression source)
        {
            // se obtiene el SpaceContext del proyecto Integra.Space.Services
            /*string sourceName = source.Value.ToString() + "_converted";
            PropertyInfo propertyInfo = typeof(Integra.Space.Services.Signalr.Contexts.SpaceContext).GetProperty(sourceName);

            if (propertyInfo == null)
            {
                throw new NotImplementedException(string.Format("Fuente '{0}' no implementada.", sourceName));
            }*/

            ParameterExpression currentParameter = null;
            bool existsParameter = false;
            string parameterName;
            parameterName = source.Value.ToString();

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
                currentParameter = Expression.Parameter(typeof(IQbservable<EventObject>), parameterName);
                this.parameterList.Add(currentParameter);
            }

            return currentParameter;
        }

        /// <summary>
        /// Return the scope variable, representing a source, for the actual lambda expression scope.
        /// </summary>
        /// <param name="actualNode">Actual node.</param>
        /// <param name="leftNode">Left expression node.</param>
        /// <returns>The scope variable.</returns>
        private Expression CreateFromForLambda(PlanNode actualNode, Expression leftNode)
        {
            return this.scopeParam.Peek();
        }

        /// <summary>
        /// Add a new scope variable to the scope variables stack.
        /// </summary>
        /// <param name="incomingObservable">Incoming observable expression.</param>
        private void CreateNewScope(Expression incomingObservable)
        {
            // si tiene parametros genéricos, creo una nueva variable global para el nuevo ambiente
            // de lo contrario se utilizará la que se tiene actualmente al tope de la pila
            if (incomingObservable.Type.GetGenericArguments().Count() > 0)
            {
                this.scopeParam.Push(Expression.Parameter(incomingObservable.Type.GetGenericArguments()[0]));
                if (incomingObservable.Type.GetGenericArguments()[0].Name.Equals("IGroupedObservable`2"))
                {
                    this.groupExpression = this.scopeParam.Peek();
                }
            }
        }

        /// <summary>
        /// Creates a new Enumerable.Count expression.
        /// </summary>
        /// <typeparam name="I">Input type.</typeparam>
        /// <param name="incomingObservable">Incoming observable expression.</param>
        /// <returns>Enumerable count expression.</returns>
        private Expression CreateEnumerableCount<I>(Expression incomingObservable)
        {
            try
            {
                ParameterExpression param = Expression.Variable(typeof(int), "resultExtensionWithoutParametersObservable");
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                MethodInfo methodCount = typeof(System.Linq.Enumerable).GetMethods().Where(m => m.Name == "Count" && m.GetParameters().Length == 1).Single().MakeGenericMethod(typeof(I).GenericTypeArguments[0]);

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion '<=' entre los siguientes valores: "))),
                                    Expression.Assign(param, Expression.Call(methodCount, incomingObservable)),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la condicion '<='"))),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    paramException,
                                     Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Error")),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
                                    )
                                )
                            ),
                        param
                        );

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                throw new CompilationException("Error al compilar, no fue posible compilar la expresion", e);
            }
        }

        /// <summary>
        /// Creates a new Enumerable.Sum expression.
        /// </summary>
        /// <typeparam name="I">Input type.</typeparam>
        /// <param name="incomingObservable">Incoming observable expression.</param>
        /// <param name="selector">Selector expression.</param>
        /// <returns>Enumerable sum expression.</returns>
        private Expression CreateEnumerableSum<I>(Expression incomingObservable, Expression selector)
        {
            try
            {
                var delegateType = typeof(Func<,>).MakeGenericType(incomingObservable.Type.GetGenericArguments()[0], selector.Type);
                Expression selectorLambda = Expression.Lambda(delegateType, selector, new ParameterExpression[] { this.scopeParam.Pop() });

                ParameterExpression param = Expression.Variable(selector.Type, "resultExtensionWithoutParametersObservable");
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                MethodInfo methodCount = typeof(System.Linq.Enumerable).GetMethods().Where(m => m.Name == "Sum" && m.GetParameters().Length == 2 && m.GetParameters()[1].ParameterType.ToString().Equals("System.Func`2[TSource," + selector.Type.ToString() + "]")).Single().MakeGenericMethod(typeof(I).GenericTypeArguments[0]);

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion '<=' entre los siguientes valores: "))),
                                    Expression.Assign(param, Expression.Call(methodCount, incomingObservable, selectorLambda)),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la condicion '<='"))),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    paramException,
                                     Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Error")),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
                                    )
                                )
                            ),
                        param
                        );

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                throw new CompilationException("Error al compilar, no fue posible compilar la expresion", e);
            }
        }

        /// <summary>
        /// Creates a new Observable.Buffer expression.
        /// </summary>
        /// <typeparam name="I">Input type.</typeparam>
        /// <param name="incomingObservable">Incoming observable expression.</param>
        /// <param name="timespanInMilliseconds">Time expression for the buffer.</param>
        /// <returns>Observable buffer expression.</returns>
        private Expression CreateBuffer<I>(Expression incomingObservable, Expression timespanInMilliseconds)
        {
            try
            {
                ParameterExpression result = Expression.Variable(typeof(IObservable<IList<I>>), "resultGroupByObservable");
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                MethodInfo methodGroupBy = typeof(System.Reactive.Linq.Observable).GetMethods().Where(m =>
                {
                    return m.Name == "Buffer" && m.GetParameters().Length == 2 && m.GetParameters().Length == 2 && m.GetParameters()[1].ParameterType.Equals(typeof(System.TimeSpan));
                })
                .Single().MakeGenericMethod(typeof(I));

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { result },
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion '<=' entre los siguientes valores: "))),
                                    Expression.Assign(result, Expression.Call(methodGroupBy, incomingObservable, timespanInMilliseconds)),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la condicion '<='"))),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    paramException,
                                     Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Error")),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
                                    )
                                )
                            ),
                        result
                        );

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                throw new CompilationException("Error al compilar, no fue posible compilar la expresion", e);
            }
        }

        /// <summary>
        /// Creates a new Observable.Where expression.
        /// </summary>
        /// <typeparam name="I">Input type.</typeparam>
        /// <param name="incomingObservable">Incoming observable expression.</param>
        /// <param name="filter">Filter expression.</param>
        /// <returns>Observable where expression.</returns>
        private Expression CreateWhere<I>(Expression incomingObservable, Expression filter)
        {
            try
            {
                var delegateType = typeof(Func<,>).MakeGenericType(incomingObservable.Type.GetGenericArguments()[0], filter.Type);
                Expression selectorLambda = Expression.Lambda(delegateType, filter, new ParameterExpression[] { this.scopeParam.Pop() });

                ParameterExpression result = Expression.Variable(typeof(IObservable<I>), "resultGroupByObservable");
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                MethodInfo methodGroupBy = typeof(System.Reactive.Linq.Observable).GetMethods().Where(m =>
                {
                    return m.Name == "Where" && m.GetParameters().Length == 2 && m.GetParameters()[1].ParameterType.ToString().Equals("System.Func`2[TSource,System.Boolean]");
                })
                .Single().MakeGenericMethod(typeof(I));

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { result },
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion '<=' entre los siguientes valores: "))),
                                    Expression.Assign(result, Expression.Call(methodGroupBy, incomingObservable, selectorLambda)),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la condicion '<='"))),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    paramException,
                                     Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Error")),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
                                    )
                                )
                            ),
                        result
                        );

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                throw new CompilationException("Error al compilar, no fue posible compilar la expresion", e);
            }
        }

        /// <summary>
        /// Creates a select for group by expression
        /// </summary>
        /// <typeparam name="I">Input type.</typeparam>
        /// <typeparam name="O">Output type, IObservable of IObservable of O.</typeparam>
        /// <param name="incomingObservable">Incoming observable expression.</param>
        /// <param name="expSelect">Selector expression.</param>
        /// <returns>Select for group by expression.</returns>
        private Expression CreateSelectForGroupBy<I, O>(Expression incomingObservable, Expression expSelect)
        {
            try
            {
                var delegateType = typeof(Func<,>).MakeGenericType(incomingObservable.Type.GetGenericArguments()[0], expSelect.Type);
                Expression selectorLambda = Expression.Lambda(delegateType, expSelect, new ParameterExpression[] { this.scopeParam.Pop() });

                ParameterExpression result = Expression.Variable(typeof(IObservable<IObservable<O>>), "newObservable");
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                MethodInfo methodSelectMany = typeof(System.Reactive.Linq.Observable).GetMethods()
                                                .Where(m => { return m.Name == "Select" && m.GetParameters().Length == 2 && m.GetParameters()[1].ParameterType.ToString().Equals("System.Func`2[TSource,TResult]"); })
                                                .Single().MakeGenericMethod(typeof(I), typeof(IObservable<O>));

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { result },
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion '<=' entre los siguientes valores: "))),
                                    Expression.Assign(result, Expression.Call(methodSelectMany, incomingObservable, selectorLambda)),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la condicion '<='"))),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    paramException,
                                     Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Error")),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
                                    )
                                )
                            ),
                        result
                        );

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                throw new CompilationException("Error al compilar, no fue posible compilar la expresion", e);
            }
        }

        /// <summary>
        /// Generate the select block expression.
        /// </summary>
        /// <param name="plans">Plan that contains the projection plans.</param>
        /// <returns>Select block expression.</returns>
        private Expression CreateProjectionExpression(PlanNode plans)
        {
            List<Expression> expressionList = new List<Expression>();
            Dictionary<string, Tuple<ConstantExpression, Expression>> keyValueList = new Dictionary<string, Tuple<ConstantExpression, Expression>>();
            List<dynamic> listOfFields = new List<dynamic>();

            foreach (var plan in plans.Children)
            {
                ConstantExpression key = (ConstantExpression)this.GenerateExpressionTree(plan.Children[0]);
                Expression value = this.GenerateExpressionTree(plan.Children[1]);
                if (keyValueList.ContainsKey(key.Value.ToString()))
                {
                    throw new CompilationException("Existe mas de un alias con el mismo nombre, verifique la proyección o la sentencia de agrupación.");
                }
                else
                {
                    keyValueList.Add(key.Value.ToString(), new Tuple<ConstantExpression, Expression>(key, value));
                }
            }

            dynamic newField = new System.Dynamic.ExpandoObject();
            
            foreach (KeyValuePair<string, Tuple<ConstantExpression, Expression>> c in keyValueList)
            {
                newField = new System.Dynamic.ExpandoObject();
                newField.FieldName = c.Value.Item1.Value;
                newField.FieldType = c.Value.Item2.Type;
                listOfFields.Add(newField);
            }

            Type myType = LanguageTypeBuilder.CompileResultType(listOfFields);
            ParameterExpression y = Expression.Variable(myType);
            expressionList.Add(Expression.Assign(y, Expression.New(myType)));

            foreach (PropertyInfo p in myType.GetProperties())
            {
                if (keyValueList.ContainsKey(p.Name))
                {
                    expressionList.Add(Expression.Call(y, p.GetSetMethod(), keyValueList[p.Name].Item2));
                }
            }

            expressionList.Add(y);
            Expression expProjectionObject = Expression.Block(
                                                        new[] { y },
                                                        expressionList
                                                );

            ParameterExpression inParam = this.scopeParam.Pop();
            var delegateType = typeof(Func<,>).MakeGenericType(inParam.Type, myType);
            Expression lambda2 = Expression.Lambda(delegateType, expProjectionObject, new ParameterExpression[] { inParam });

            return lambda2;
        }

        /// <summary>
        /// Creates a select for buffer expression.
        /// </summary>
        /// <typeparam name="I">Input type.</typeparam>
        /// <typeparam name="O">Output type, IObservable of O.</typeparam>
        /// <param name="actualNode">Actual execution plan node.</param>
        /// <param name="incomingObservable">Incoming observable expression.</param>
        /// <param name="expProjection">Projection lambda expression.</param>
        /// <returns>Select for buffer expression.</returns>
        private Expression CreateSelectForBuffer<I, O>(PlanNode actualNode, Expression incomingObservable, Expression expProjection)
        {
            try
            {
                ParameterExpression result = Expression.Variable(typeof(IObservable<O>), "newObservable");
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                MethodInfo methodSelectMany = typeof(System.Reactive.Linq.Observable).GetMethods()
                                                .Where(m => { return m.Name == "Select" && m.GetParameters().Length == 2 && m.GetParameters()[1].ParameterType.ToString().Equals("System.Func`2[TSource,TResult]"); })
                                                .Single().MakeGenericMethod(typeof(I), typeof(O));

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { result },
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion '<=' entre los siguientes valores: "))),
                                    Expression.Assign(result, Expression.Call(methodSelectMany, incomingObservable, expProjection)),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la condicion '<='"))),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    paramException,
                                     Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Error")),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
                                    )
                                )
                            ),
                        result
                        );

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                throw new CompilationException("Error al compilar, no fue posible compilar la expresion", e);
            }
        }

        /// <summary>
        /// Creates a Observable.GroupBy expression
        /// </summary>
        /// <typeparam name="I">Input type.</typeparam>
        /// <typeparam name="O">Output type, IObservable of IGroupedObservable of (O, I).</typeparam>
        /// <param name="incomingObservable">Incoming observable expression.</param>
        /// <param name="keySelector">Key selector lambda expression</param>
        /// <returns>Group by expression</returns>
        private Expression CreateGroupBy<I, O>(Expression incomingObservable, Expression keySelector)
        {
            try
            {
                ParameterExpression result = Expression.Variable(typeof(IObservable<IGroupedObservable<O, I>>), "resultGroupByObservable");
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                MethodInfo methodGroupBy = typeof(System.Reactive.Linq.Observable).GetMethods().Where(m =>
                {
                    return m.Name == "GroupBy" && m.GetParameters().Length == 3 && m.GetParameters()[2].ParameterType.ToString().Equals("System.Collections.Generic.IEqualityComparer`1[TKey]");
                })
                .Single().MakeGenericMethod(typeof(I), typeof(O));

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { result },
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion '<=' entre los siguientes valores: "))),
                                    Expression.Assign(result, Expression.Call(methodGroupBy, incomingObservable, keySelector, Expression.New(typeof(GroupByKeyComparer<O>)))),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la condicion '<='"))),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                     paramException,
                                     Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Error")),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
                                    )
                                )
                            ),
                        result
                        );

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                throw new CompilationException("Error al compilar, no fue posible compilar la expresion", e);
            }
        }

        /// <summary>
        /// Create and Expression representing a Observable.Merge
        /// </summary>
        /// <typeparam name="I">Input type</typeparam>
        /// <typeparam name="O">Output type</typeparam>
        /// <param name="incomingObservable">Incoming observable</param>
        /// <returns>Result expression</returns>
        private Expression CreateMerge<I, O>(Expression incomingObservable)
        {
            try
            {
                ParameterExpression result = Expression.Variable(typeof(O), "resultGroupByObservable");
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                MethodInfo methodGroupBy = typeof(System.Reactive.Linq.Observable).GetMethods().Where(m =>
                {
                    return m.Name == "Merge" && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType.ToString().Equals("System.IObservable`1[System.IObservable`1[TSource]]");
                })
                .Single().MakeGenericMethod(typeof(I).GetGenericArguments()[0].GetGenericArguments()[0]);

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { result },
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion '<=' entre los siguientes valores: "))),
                                    Expression.Assign(result, Expression.Call(methodGroupBy, incomingObservable)),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la condicion '<='"))),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    paramException,
                                     Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Error")),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
                                    )
                                )
                            ),
                        result
                        );

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                throw new CompilationException("Error al compilar, no fue posible compilar la expresion", e);
            }
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="plan">plan node to convert</param>
        /// <returns>expression tree of actual plan</returns>
        private ConstantExpression GenerateConstant(PlanNode plan)
        {
            if (!plan.Properties.ContainsKey("Value"))
            {
                return Expression.Constant(null);
            }

            if (!plan.Properties.ContainsKey("DataType"))
            {
                return Expression.Constant(null);
            }

            try
            {
                if (plan.Properties["Value"] == null)
                {
                    return Expression.Constant(null);
                }

                Type tipo = Type.GetType(plan.Properties["DataType"].ToString());
                return Expression.Constant(plan.Properties["Value"], tipo);
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
            return this.scopeParam.Peek();
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
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.IfThenElse(
                            Expression.Equal(objeto, Expression.Constant(null)),
                            Expression.Assign(param, Expression.Constant(null)),
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Se obtendra el valor del objeto " + actualNode.NodeText))),
                                    Expression.Assign(param, Expression.Call(objeto, typeof(MessageSubsection).GetMethod("get_Value"))),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("Write", new Type[] { typeof(object) }), Expression.Constant("**El valor del objeto " + actualNode.NodeText + " es: "))),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), param)),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    paramException,
                                       Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("No fue posible obtener el valor del campo del mensaje, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
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
            ParameterExpression paramException = Expression.Variable(typeof(Exception));

            try
            {
                Expression mensaje =
                        Expression.Block(
                            new ParameterExpression[] { param },
                            Expression.IfThenElse(
                                Expression.Equal(Expression.Constant(eventNode.Type.GetProperty("Message")), Expression.Constant(null)),
                                Expression.Assign(param, Expression.Default(typeof(EventMessage))),
                                Expression.TryCatch(
                                    Expression.Block(
                                        Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Obteniendo el mensaje"))),
                                        Expression.Assign(param, Expression.Property(eventNode, "Message")),
                                        Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("**Se obtuvo el mensaje"))),
                                        Expression.Empty()
                                    ),
                                    Expression.Catch(
                                        paramException,
                                        Expression.Block(
                                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("No fue posible obtener la propiedad 'Message' del evento, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                                Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
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
            ParameterExpression paramException = Expression.Variable(typeof(Exception));
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
                                Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Obteniendo el subcampo: " + auxSubField.Value))),
                                Expression.Assign(v, Expression.Call(field, typeof(MessageSubsection).GetMethod("get_Item", new Type[] { tipo }), auxSubField)),
                                Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("**Se obtuvo el subcampo: " + auxSubField.Value)))
                            ),
                            Expression.Catch(
                                paramException,
                                Expression.Block(
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("No fue posible obtener la subsección de la subsección del mensaje, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                    Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
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
            ParameterExpression paramException = Expression.Variable(typeof(Exception));
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
                                Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Obteniendo el campo: " + auxField.Value))),
                                Expression.Assign(v, Expression.Call(part, typeof(MessageSection).GetMethod("get_Item", new Type[] { tipo }), auxField)),
                                Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("**Se obtuvo el campo: " + auxField.Value)))
                            ),
                            Expression.Catch(
                                paramException,
                                Expression.Block(
                                    Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("No fue posible obtener la subsección de la sección del mensaje, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                    Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
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
            ParameterExpression v = Expression.Parameter(typeof(MessageSection), "bloque");
            ParameterExpression paramException = Expression.Variable(typeof(Exception));
            ConstantExpression auxPart = (ConstantExpression)partId;
            Type tipo = auxPart.Type;

            try
            {
                Expression parte = Expression.Block(
                    new ParameterExpression[] { v },
                        Expression.IfThen(
                            Expression.Call(
                                mensaje,
                                typeof(EventMessage).GetMethod("Contains", new Type[] { tipo }),
                                auxPart
                            ),
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Obteniendo la parte: " + auxPart.Value))),
                                    Expression.Assign(v, Expression.Call(mensaje, typeof(EventMessage).GetMethod("get_Item", new Type[] { tipo }), auxPart)),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("**Se obtuvo la parte: " + auxPart.Value)))
                                ),
                                Expression.Catch(
                                    paramException,
                                        Expression.Block(
                                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("No fue posible obtener la sección del objeto, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                                Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
                                            )
                                )
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
        /// Get the object prefix value
        /// </summary>
        /// <param name="actualNode">Current node</param>
        /// <returns>Object prefix expression</returns>
        private Expression GenerateObjectPrefix(PlanNode actualNode)
        {
            if (!actualNode.Properties.ContainsKey("Value"))
            {
                return Expression.Constant(null);
            }

            if (!actualNode.Properties.ContainsKey("DataType"))
            {
                return Expression.Constant(null);
            }

            this.objectPrefix = actualNode.Properties["Value"].ToString();
            return Expression.Constant(actualNode.Properties["Value"], Type.GetType(actualNode.Properties["DataType"].ToString()));
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

            if (leftNode is ConstantExpression)
            {
                if ((leftNode as ConstantExpression).Value == null)
                {
                    return this.StandardizeType(leftNode, this.ConvertToNullable(typeToCast));
                }
            }

            ParameterExpression paramException = Expression.Variable(typeof(Exception));

            try
            {
                ParameterExpression retorno = Expression.Parameter(this.ConvertToNullable(typeToCast));
                resultExp =
                Expression.Block(
                    new[] { retorno },
                    Expression.IfThen(
                        Expression.NotEqual(Expression.Convert(leftNode, this.ConvertToNullable(leftNode.Type)), Expression.Constant(null)),
                        Expression.TryCatch(
                            Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Unbox a " + typeToCast + " del siguiente valor: "))),
                                    Expression.Assign(retorno, Expression.Unbox(leftNode, this.ConvertToNullable(typeToCast))),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la operacion de 'Unbox' hacia el tipo " + typeToCast))),
                                    Expression.Empty()
                                ),
                            Expression.Catch(
                                    paramException,
                                        Expression.Block(
                                            Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("No fue castear (unbox) el valor u objeto, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                            Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
                                            )
                                )
                        )
                    ),
                    retorno
                );
            }
            catch (Exception e)
            {
                try
                {
                    ParameterExpression retorno = Expression.Parameter(this.ConvertToNullable(typeToCast));
                    resultExp =
                    Expression.Block(
                        new[] { retorno },
                        Expression.IfThen(
                            Expression.NotEqual(Expression.Convert(leftNode, this.ConvertToNullable(leftNode.Type)), Expression.Constant(null)),
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Convert a " + typeToCast + " del siguiente valor: "))),
                                    Expression.Assign(retorno, Expression.Convert(leftNode, this.ConvertToNullable(typeToCast))),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la operacion de 'Convert' hacia el tipo " + typeToCast))),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    paramException,
                                    Expression.Block(
                                            Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("No fue castear (convert) el valor u objeto, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                            Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
                                    )
                                )
                            )
                        ),
                        retorno
                    );
                }
                catch (Exception ex)
                {
                    resultExp = Expression.Constant(null);
                    throw new CompilationException("Error al castear.", e);
                }
            }

            return resultExp;
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
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                if (cadenaLike.StartsWith("%") && cadenaLike.EndsWith("%"))
                {
                    Expression tryCatchExpr =
                        Expression.Block(
                            new[] { param },
                            Expression.IfThenElse(
                                Expression.Or(Expression.Equal(this.StandardizeType(leftNode, leftNode.Type), Expression.Constant(null, this.ConvertToNullable(leftNode.Type))), Expression.Equal(this.StandardizeType(rightNode, rightNode.Type), Expression.Constant(null, this.ConvertToNullable(rightNode.Type)))),
                                Expression.Assign(param, Expression.Constant(false)),
                                Expression.TryCatch(
                                    Expression.Block(
                                        Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion 'like' con ambos comodines entre los siguientes valores: "))),
                                        Expression.Assign(param, Expression.Call(leftNode, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), Expression.Constant(cadenaAComparar))),
                                        Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la expresion de comparacion 'like' con ambos comodines"))),
                                        Expression.Empty()
                                        ),
                                    Expression.Catch(
                                        paramException,
                                        Expression.Block(
                                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("No fue posible realizar la operación like '%...%', error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                                Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
                                            )
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
                            Expression.IfThenElse(
                                Expression.Or(Expression.Equal(this.StandardizeType(leftNode, leftNode.Type), Expression.Constant(null, this.ConvertToNullable(leftNode.Type))), Expression.Equal(this.StandardizeType(rightNode, rightNode.Type), Expression.Constant(null, this.ConvertToNullable(rightNode.Type)))),
                                Expression.Assign(param, Expression.Constant(false)),
                                Expression.TryCatch(
                                    Expression.Block(
                                        Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion 'like' con comodin izquierdo entre los siguientes valores: "))),
                                        Expression.Assign(param, Expression.Call(leftNode, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), Expression.Constant(cadenaAComparar))),
                                        Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la expresion de comparacion 'like' con comodin izquierdo"))),
                                        Expression.Empty()
                                        ),
                                    Expression.Catch(
                                        paramException,
                                        Expression.Block(
                                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("No fue posible realizar la operación like '%..., error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                                Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
                                            )
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
                            Expression.IfThenElse(
                                Expression.Or(Expression.Equal(this.StandardizeType(leftNode, leftNode.Type), Expression.Constant(null, this.ConvertToNullable(leftNode.Type))), Expression.Equal(this.StandardizeType(rightNode, rightNode.Type), Expression.Constant(null, this.ConvertToNullable(rightNode.Type)))),
                                Expression.Assign(param, Expression.Constant(false)),
                                Expression.TryCatch(
                                    Expression.Block(
                                        Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion 'like' con comodin derecho entre los siguientes valores: "))),
                                        Expression.Assign(param, Expression.Call(leftNode, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), Expression.Constant(cadenaAComparar))),
                                        Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la expresion de comparacion 'like' con comodin derecho"))),
                                        Expression.Empty()
                                        ),
                                    Expression.Catch(
                                        paramException,
                                        Expression.Block(
                                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("No fue posible realizar la operación like '...%, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                                Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
                                            )
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
                            Expression.IfThenElse(
                                Expression.Or(Expression.Equal(this.StandardizeType(leftNode, leftNode.Type), Expression.Constant(null, this.ConvertToNullable(leftNode.Type))), Expression.Equal(this.StandardizeType(rightNode, rightNode.Type), Expression.Constant(null, this.ConvertToNullable(rightNode.Type)))),
                                Expression.Assign(param, Expression.Constant(false)),
                                Expression.TryCatch(
                                    Expression.Block(
                                        Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion 'like' sin comodines entre los siguientes valores: "))),
                                        Expression.Assign(param, Expression.Call(leftNode, typeof(string).GetMethod("Equals", new Type[] { typeof(string) }), Expression.Constant(cadenaAComparar))),
                                        Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la expresion de comparacion 'like' sin comodines"))),
                                        Expression.Empty()
                                        ),
                                    Expression.Catch(
                                        paramException,
                                        Expression.Block(
                                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("No fue posible realizar la operación like sin comodines, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                                Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
                                            )
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
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.IfThenElse(
                            Expression.TypeEqual(leftNode, typeof(bool)),
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion logica 'not' del siguiente valor: "))),
                                    Expression.Assign(param, Expression.Not(leftNode)),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la expresion logica 'not'"))),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    paramException,
                                    Expression.Block(
                                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("No fue posible negar la expresión de comparación, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                                Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
                                            )
                                )
                            ),
                            Expression.Assign(param, Expression.Constant(false))
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
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.IfThenElse(
                            Expression.Or(Expression.Equal(this.StandardizeType(leftNode, leftNode.Type), Expression.Constant(null, this.ConvertToNullable(leftNode.Type))), Expression.Equal(this.StandardizeType(rightNode, rightNode.Type), Expression.Constant(null, this.ConvertToNullable(rightNode.Type)))),
                            Expression.Assign(param, Expression.Constant(false)),
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion '>=' entre los siguientes valores: "))),
                                    Expression.Assign(param, Expression.GreaterThanOrEqual(leftNode, rightNode)),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la condicion '>='"))),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    paramException,
                                    Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("No fue posible realizar la operación mayor o igual que '>=', error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
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
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.IfThenElse(
                            Expression.Or(Expression.Equal(this.StandardizeType(leftNode, leftNode.Type), Expression.Constant(null, this.ConvertToNullable(leftNode.Type))), Expression.Equal(this.StandardizeType(rightNode, rightNode.Type), Expression.Constant(null, this.ConvertToNullable(rightNode.Type)))),
                            Expression.Assign(param, Expression.Constant(false)),
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion '>' entre los siguientes valores: "))),
                                    Expression.Assign(param, Expression.GreaterThan(this.StandardizeType(leftNode, leftNode.Type), this.StandardizeType(rightNode, rightNode.Type))),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la condicion '>'"))),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    paramException,
                                    Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("No fue posible realizar la operación mayor que '>', error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
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
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.IfThenElse(
                            Expression.Or(Expression.Equal(this.StandardizeType(leftNode, leftNode.Type), Expression.Constant(null, this.ConvertToNullable(leftNode.Type))), Expression.Equal(this.StandardizeType(rightNode, rightNode.Type), Expression.Constant(null, this.ConvertToNullable(rightNode.Type)))),
                            Expression.Assign(param, Expression.Constant(false)),
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion '<=' entre los siguientes valores: "))),
                                    Expression.Assign(param, Expression.LessThanOrEqual(leftNode, rightNode)),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la condicion '<='"))),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    paramException,
                                     Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("No fue posible realizar la operación menor o igual que '<=', error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
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
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                         Expression.IfThenElse(
                                Expression.Or(Expression.Equal(this.StandardizeType(leftNode, leftNode.Type), Expression.Constant(null, this.ConvertToNullable(leftNode.Type))), Expression.Equal(this.StandardizeType(rightNode, rightNode.Type), Expression.Constant(null, this.ConvertToNullable(rightNode.Type)))),
                                Expression.Assign(param, Expression.Constant(false)),
                                Expression.TryCatch(
                                    Expression.Block(
                                        Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion '<' entre los siguientes valores: "))),
                                        Expression.Assign(param, Expression.LessThan(this.StandardizeType(leftNode, leftNode.Type), this.StandardizeType(rightNode, rightNode.Type))),
                                        Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la condicion '<'"))),
                                        Expression.Empty()
                                        ),
                                    Expression.Catch(
                                        paramException,
                                        Expression.Block(
                                                Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("No fue posible realizar la operación menor que '<', error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                                Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
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
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                Expression tryCatchExpr =
                    Expression.Block(
                        new ParameterExpression[] { param },
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion '!=' entre los siguientes valores: "))),
                                Expression.Assign(param, Expression.NotEqual(this.StandardizeType(leftNode, leftNode.Type), this.StandardizeType(rightNode, rightNode.Type))),
                                Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la condicion de No igualdad"))),
                                Expression.Empty()
                                ),
                            Expression.Catch(
                                paramException,
                                Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Error con la expresion de desigualdad en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
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
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                Expression tryCatchExpr =
                    Expression.Block(
                        new ParameterExpression[] { param },
                        Expression.TryCatch(
                            Expression.Block(
                                Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion de comparacion '==' entre los siguientes valores: "))),
                                Expression.Assign(param, Expression.Equal(this.StandardizeType(leftNode, leftNode.Type), this.StandardizeType(rightNode, rightNode.Type))),
                                Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Finalizacion de la condicion de igualdad"))),
                                Expression.Empty()
                                ),
                            Expression.Catch(
                                paramException,
                                Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Error con la expresion de igualdad en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
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
        /// <param name="rightNode">right child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerateSubtract(PlanNode actualNode, Expression leftNode, Expression rightNode)
        {
            Type tipo = Type.GetType(actualNode.Properties["DataType"].ToString());

            ParameterExpression paramException = Expression.Variable(typeof(Exception));

            if (tipo.Equals(typeof(TimeSpan)))
            {
                try
                {
                    ParameterExpression param = Expression.Variable(tipo, "variable");

                    Expression tryCatchExpr =
                        Expression.Block(
                            new[] { param },
                            Expression.IfThen(
                                Expression.And(Expression.NotEqual(this.StandardizeType(leftNode, leftNode.Type), Expression.Constant(null, this.ConvertToNullable(leftNode.Type))), Expression.NotEqual(this.StandardizeType(rightNode, rightNode.Type), Expression.Constant(null, this.ConvertToNullable(rightNode.Type)))),
                                Expression.TryCatch(
                                    Expression.Block(
                                        Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion aritmetica de resta para timespan '-' entre los siguientes valores: "))),
                                        Expression.Assign(param, Expression.Call(leftNode, typeof(DateTime).GetMethod("Subtract", new Type[] { typeof(DateTime) }), rightNode)),
                                        Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la expresion aritmetica de resta para timespan '-'"))),
                                        Expression.Empty()
                                        ),
                                    Expression.Catch(
                                        paramException,
                                        Expression.Block(
                                         Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Error con la expresion aritmetica de resta '-' en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                         Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
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
                            Expression.IfThen(
                                Expression.And(Expression.NotEqual(this.StandardizeType(leftNode, leftNode.Type), Expression.Constant(null, this.ConvertToNullable(leftNode.Type))), Expression.NotEqual(this.StandardizeType(rightNode, rightNode.Type), Expression.Constant(null, this.ConvertToNullable(rightNode.Type)))),
                                Expression.TryCatch(
                                    Expression.Block(
                                        Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion aritmetica de resta '-' entre los siguientes valores: "))),
                                        Expression.Assign(param, Expression.Subtract(leftNode, rightNode)),
                                        Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la expresion aritmetica de resta '-'"))),
                                        Expression.Empty()
                                        ),
                                    Expression.Catch(
                                        paramException,
                                        Expression.Block(
                                        Expression.Constant("Error con la expresion aritmetica de resta '-' en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
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
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.IfThen(
                            Expression.NotEqual(this.StandardizeType(leftNode, leftNode.Type), Expression.Constant(null, this.ConvertToNullable(leftNode.Type))),
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion aritmetica de negacion '-' entre los siguientes valores: "))),
                                    Expression.Assign(param, Expression.Negate(leftNode)),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion aritmetica de negacion '-' entre los siguientes valores: "))),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    paramException,
                                    Expression.Block(
                                            Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Error con la expresion aritmetica unaria de negacion '-' en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                            Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
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
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.IfThenElse(
                            Expression.And(Expression.TypeEqual(leftNode, typeof(bool)), Expression.TypeEqual(rightNode, typeof(bool))),
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion logica 'And' entre los siguientes valores: "))),
                                    Expression.Assign(param, Expression.AndAlso(leftNode, rightNode)),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la expresion logica 'And'"))),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    paramException,
                                     Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Error con la expresion booleana 'and' en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
                                )
                                )
                            ),
                            Expression.Assign(param, Expression.Constant(false))
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
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.IfThenElse(
                            Expression.And(Expression.TypeEqual(leftNode, typeof(bool)), Expression.TypeEqual(rightNode, typeof(bool))),
                            Expression.TryCatch(
                                Expression.Block(
                                        Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Inicio de la expresion logica 'Or' entre los siguientes valores: "))),
                                        Expression.Assign(param, Expression.OrElse(leftNode, rightNode)),
                                        Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Fin de la expresion logica 'Or'"))),
                                        Expression.Empty()
                                    ),
                                Expression.Catch(
                                    paramException,
                                    Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Error con la expresion booleana 'or' en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
                                    )
                                )
                            ),
                            Expression.Assign(param, Expression.Constant(false))
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
            ParameterExpression paramException = Expression.Variable(typeof(Exception));

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
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Se obtendrá la siguiente propiedad: " + propiedad))),
                                    Expression.Assign(param, Expression.Call(leftNode, leftNode.Type.GetProperty(propiedad).GetMethod)),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Se obtuvo la siguiente propiedad: " + propiedad))),
                                    Expression.Empty()
                                ),
                                Expression.Catch(
                                    paramException,
                                    Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Error al compilar la funcion '" + propiedad + "', no es posible obtener el valor solicitado. Error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
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
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.IfThenElse(
                            Expression.Equal(Expression.Constant(leftNode.Type.GetProperty(propiedad)), Expression.Constant(null)),
                            Expression.Assign(param, Expression.Default(tipo)),
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Se obtendrá la siguiente propiedad: " + propiedad))),
                                    Expression.Assign(param, Expression.Property(leftNode, propiedad)),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Se obtuvo la siguiente propiedad: " + propiedad))),
                                    Expression.Empty()
                                ),
                                Expression.Catch(
                                    paramException,
                                    Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("No fue posible obtener la propiedad " + propiedad + ", error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
                                    )
                                )
                            )
                        ),
                        param);

                return tryCatchExpr;
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
        /// <returns>expression tree to get the IGroupedObservable Key property</returns>
        private Expression GenerateGroupKey(PlanNode actualNode)
        {
            if (this.groupExpression == null)
            {
                throw new CompilationException("Error al llamar a 'Key', no existe group by en la consulta.");
            }

            Type tipo = this.groupExpression.Type;
            string propiedad = actualNode.Properties["Value"].ToString();

            try
            {
                ParameterExpression param = Expression.Variable(tipo.GetGenericArguments()[0], "variable");
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.IfThenElse(
                            Expression.Equal(Expression.Constant(this.groupExpression.Type.GetProperty(propiedad)), Expression.Constant(null)),
                            Expression.Assign(param, Expression.Default(this.groupExpression.Type.GetProperty(propiedad).PropertyType)),
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Se obtendrá la siguiente propiedad: " + propiedad))),
                                    Expression.Assign(param, Expression.Property(this.groupExpression, propiedad)),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Se obtuvo la siguiente propiedad: " + propiedad))),
                                    Expression.Empty()
                                ),
                                Expression.Catch(
                                    paramException,
                                    Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("No fue posible obtener la propiedad " + propiedad + ", error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
                                    )
                                )
                            )
                        ),
                        param);

                return tryCatchExpr;
            }
            catch (Exception e)
            {
                throw new CompilationException("Error al compilar la propiedad '" + propiedad + "' del grupo", e);
            }
        }

        /// <summary>
        /// Create a expression tree
        /// </summary>
        /// <param name="actualNode">actual plan</param>
        /// <param name="leftNode">left child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerateGroupKeyProperty(PlanNode actualNode, Expression leftNode)
        {
            Type tipo = leftNode.Type;
            string propiedad = actualNode.Properties["Value"].ToString();

            try
            {
                ParameterExpression param = Expression.Variable(tipo.GetProperty(propiedad).PropertyType, "variable");
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.IfThenElse(
                            Expression.Equal(Expression.Constant(leftNode.Type.GetProperty(propiedad)), Expression.Constant(null)),
                            Expression.Assign(param, Expression.Default(tipo.GetProperty(propiedad).PropertyType)),
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Se obtendrá la siguiente propiedad: " + propiedad))),
                                    Expression.Assign(param, Expression.Property(leftNode, propiedad)),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Se obtuvo la siguiente propiedad: " + propiedad))),
                                    Expression.Empty()
                                ),
                                Expression.Catch(
                                    paramException,
                                    Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("No fue posible obtener la propiedad " + propiedad + ", error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
                                    )
                                )
                            )
                        ),
                        param);

                return tryCatchExpr;
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
        /// <param name="groupKeyProperty">left child expression</param>
        /// <returns>expression tree of actual plan</returns>
        private Expression GenerateGroupPropertyValue(PlanNode actualNode, Expression groupKeyProperty)
        {
            try
            {
                ParameterExpression param = Expression.Variable(groupKeyProperty.Type, "variable");
                ParameterExpression paramException = Expression.Variable(typeof(Exception));

                Expression tryCatchExpr =
                    Expression.Block(
                        new[] { param },
                        Expression.IfThenElse(
                            Expression.Equal(groupKeyProperty, Expression.Constant(null)),
                            Expression.Assign(param, Expression.Constant(null)),
                            Expression.TryCatch(
                                Expression.Block(
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("Se obtendra el valor del objeto " + actualNode.NodeText))),
                                    Expression.Assign(param, groupKeyProperty),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("Write", new Type[] { typeof(object) }), Expression.Constant("**El valor del objeto " + actualNode.NodeText + " es: "))),
                                    Expression.IfThen(Expression.Constant(this.printLog), Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), param)),
                                    Expression.Empty()
                                    ),
                                Expression.Catch(
                                    paramException,
                                       Expression.Block(
                                        Expression.Call(typeof(System.Diagnostics.Debug).GetMethod("WriteLine", new Type[] { typeof(object) }), Expression.Constant("No fue posible obtener el valor del campo del mensaje, error en la linea: " + actualNode.Line + " columna: " + actualNode.Column + " con " + actualNode.NodeText)),
                                        Expression.Throw(Expression.New(typeof(Exception).GetConstructor(new Type[] { typeof(string), typeof(Exception) }), Expression.Constant("Error en la ejecución", typeof(string)), paramException))
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
                throw new CompilationException("Error al compilar, no fue posible obtener el valor del mensaje", e);
            }
        }

        /// <summary>
        /// Try to convert the no null-able type to a null-able type
        /// </summary>
        /// <param name="tipo">Type to convert</param>
        /// <returns>Converted type</returns>
        private Type ConvertToNullable(Type tipo)
        {
            if (tipo.IsValueType)
            {
                if (tipo.IsGenericType && tipo.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return tipo;
                }
                else
                {
                    return typeof(Nullable<>).MakeGenericType(tipo);
                }
            }
            else
            {
                return tipo;
            }
        }

        /// <summary>
        /// Standardize the type of the expression
        /// </summary>
        /// <param name="exp">Expression to standardize</param>
        /// <param name="type">Type to convert</param>
        /// <returns>Expression standardized</returns>
        private Expression StandardizeType(Expression exp, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return exp;
            }

            if (exp is ConstantExpression)
            {
                ConstantExpression expAux = exp as ConstantExpression;

                if (expAux.Value == null)
                {
                    return Expression.Constant(null, this.ConvertToNullable(type));
                }
                else
                {
                    return Expression.Convert(exp, this.ConvertToNullable(type));
                }
            }
            else
            {
                return Expression.Convert(exp, this.ConvertToNullable(type));
            }
        }
    }
}