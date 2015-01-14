//-----------------------------------------------------------------------
// <copyright file="EQLLanguageRuntime.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.Grammars
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Irony.Interpreter;
    using Irony.Parsing;

    /// <summary>
    /// EQLLanguageRuntime class
    /// </summary>
    internal class EQLLanguageRuntime : LanguageRuntime
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EQLLanguageRuntime"/> class
        /// </summary>
        public EQLLanguageRuntime()
            : base(new LanguageData(new EQLGrammar()))
        { 
        }
    }
}
