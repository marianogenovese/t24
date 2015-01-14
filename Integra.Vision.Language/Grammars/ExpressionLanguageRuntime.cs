//-----------------------------------------------------------------------
// <copyright file="ExpressionLanguageRuntime.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.Grammars
{
    using Irony.Interpreter;
    using Irony.Parsing;

    /// <summary>
    /// EQLLanguageRuntime class
    /// </summary>
    internal class ExpressionLanguageRuntime : LanguageRuntime
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionLanguageRuntime"/> class
        /// </summary>
        public ExpressionLanguageRuntime()
            : base(new LanguageData(new ExpressionGrammar()))
        { 
        }
    }
}
