﻿//-----------------------------------------------------------------------
// <copyright file="ValuesParser.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.Parsers
{
    using System;
    using System.Collections.Generic;
    using Integra.Vision.Language.Grammars;
    using Irony.Parsing;

    /// <summary>
    /// Class that implements the logic to parse command projections
    /// </summary>
    internal class ValuesParser
    {
        /// <summary>
        /// Command text
        /// </summary>
        private string commandText;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValuesParser"/> class
        /// </summary>
        /// <param name="commandText">Command text</param>
        public ValuesParser(string commandText)
        {
            this.commandText = commandText;
        }

        /// <summary>
        /// Implements the logic to parse private commands
        /// </summary>
        /// <returns>Execution plan</returns>
        public PlanNode Parse()
        {
            PlanNode nodes = null;

            try
            {
                ValuesGrammar grammar = new ValuesGrammar();
                LanguageData language = new LanguageData(grammar);
                Parser parser = new Parser(language);
                ParseTree parseTree = parser.Parse(this.commandText);
                if (parseTree.HasErrors())
                {
                    foreach (var parserMessage in parseTree.ParserMessages)
                    {
                        throw new Exception(Resources.SR.SyntaxError(parserMessage.Message, parserMessage.Location.Line, parserMessage.Location.Column));
                    }
                }
                else
                {
                    Irony.Interpreter.ScriptApp app = new Irony.Interpreter.ScriptApp(new Integra.Vision.Language.Grammars.ProjectionLanguageRuntime());
                    nodes = (PlanNode)app.Evaluate(parseTree);
                }
            }
            catch (System.Exception e)
            {
                throw new Exception(Resources.SR.InterpretationException, e);
            }

            return nodes;
        }
    }
}