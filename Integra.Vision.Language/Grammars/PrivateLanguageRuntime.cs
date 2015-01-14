//-----------------------------------------------------------------------
// <copyright file="PrivateLanguageRuntime.cs" company="Integra.Vision.Language">
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
    /// PrivateLanguageRuntime class
    /// </summary>
    internal class PrivateLanguageRuntime : LanguageRuntime
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrivateLanguageRuntime"/> class
        /// </summary>
        public PrivateLanguageRuntime()
            : base(new LanguageData(new PrivateGrammar()))
        { 
        }
    }
}
