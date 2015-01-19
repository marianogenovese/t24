//-----------------------------------------------------------------------
// <copyright file="ExpressionCompiler.cs" company="Ingetra.Vision.Language">
//     Copyright (c) Ingetra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Class for compile expression tree
    /// </summary>
    internal static class ExpressionCompiler
    {
        /// <summary>
        /// Creates a function based on a expression tree with two parameter.
        /// </summary>
        /// <typeparam name="TFunc">Function to return</typeparam>
        /// <param name="expression">Expression tree</param>
        /// <param name="parameterList">List of parameters</param>
        /// <returns>Function compiled</returns>
        public static TFunc CompileExpression<TFunc>(this Expression expression, List<ParameterExpression> parameterList) where TFunc : class
        {
            try
            {
                return Expression.Lambda<TFunc>(expression, parameterList.ToArray()).Compile();
            }
            catch (Exception e)
            {
                // aqui tambien entran errores de Exceptions
                return Expression.Lambda<TFunc>(Expression.Constant(null)).Compile();
            }
        }
    }
}
