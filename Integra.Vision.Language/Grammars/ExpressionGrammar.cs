//-----------------------------------------------------------------------
// <copyright file="ExpressionGrammar.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.Grammars
{
    using System;
    using System.Linq;
    using Integra.Vision.Language.ASTNodes.Constants;
    using Integra.Vision.Language.ASTNodes.Lists;
    using Integra.Vision.Language.ASTNodes.Objects.Event;
    using Integra.Vision.Language.ASTNodes.Objects.Object;
    using Integra.Vision.Language.ASTNodes.Operations;
    using Irony.Interpreter;
    using Irony.Parsing;

    /// <summary>
    /// EQLGrammar grammar for the commands and the predicates 
    /// </summary>
    [Language("ExpressionsGrammar", "0.1", "")]
    internal sealed class ExpressionGrammar : InterpretedLanguageGrammar
    {
        /// <summary>
        /// Grammar to add the expression rules
        /// </summary>
        private NonTerminal logicExpression;

        /// <summary>
        /// Values grammar
        /// </summary>
        private ValuesGrammar valuesGrammar;
                
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionGrammar"/> class
        /// </summary>
        public ExpressionGrammar()
            : base(false)
        {
            this.valuesGrammar = new ValuesGrammar();
            this.Grammar(false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionGrammar"/> class
        /// </summary>
        /// <param name="prueba">flag for tests</param>
        public ExpressionGrammar(bool prueba)
            : base(false)
        {
            this.valuesGrammar = new ValuesGrammar();
            this.Grammar(prueba);
        }

        /// <summary>
        /// Gets the nonterminal for expression conditions
        /// </summary>
        public NonTerminal LogicExpression
        {
            get
            {
                return this.logicExpression;
            }
        }

        /// <summary>
        /// Specify the language grammar
        /// </summary>
        /// <param name="prueba">flag for tests</param>
        public void Grammar(bool prueba)
        {
            /* PALABRAS RESERVADAS */
            KeyTerm terminalLike = ToTerm("like", "like");
            KeyTerm terminalNot = ToTerm("not", "not");
            KeyTerm terminalIn = ToTerm("in", "in");

            /* FUNCIONES */
            KeyTerm terminalHour = ToTerm("hour", "hour");
            KeyTerm terminalMinute = ToTerm("minute", "minute");
            KeyTerm terminalSecond = ToTerm("second", "second");

            /* EVENTOS */
            KeyTerm terminalEvent = ToTerm("event", "event");
            KeyTerm terminalMessage = ToTerm("Message", "Message");
            KeyTerm terminalTimestamp = ToTerm("Timestamp", "Timestamp");
            KeyTerm terminalAgent = ToTerm("Agent", "Agent");
            KeyTerm terminalAdapter = ToTerm("Adapter", "Adapter");
            KeyTerm terminalName = ToTerm("Name", "Name");

            /* OPERADORES LOGICOS */
            KeyTerm terminalAnd = ToTerm("and", "and");
            KeyTerm terminalOr = ToTerm("or", "or");

            // Marcamos los terminales, definidos hasta el momento, como palabras reservadas
            this.MarkReservedWords(this.KeyTerms.Keys.ToArray());

            /* OPERADORES ARITMETICOS */
            KeyTerm terminalMenos = ToTerm("-", "menos");
            KeyTerm terminalMas = ToTerm("+", "mas");

            /* OPERADORES COMPARATIVOS */
            KeyTerm terminalIgualIgual = ToTerm("==", "igualIgual");
            KeyTerm terminalNoIgual = ToTerm("!=", "noIgual");
            KeyTerm terminalMayorIgual = ToTerm(">=", "mayorIgual");
            KeyTerm terminalMenorIgual = ToTerm("<=", "menorIgual");
            KeyTerm terminalMayorQue = ToTerm(">", "mayorQue");
            KeyTerm terminalMenorQue = ToTerm("<", "menorQue");
                                    
            /* SIMBOLOS */
            KeyTerm terminalParentesisIz = ToTerm("(", "parentesisIzExpressionGrammar");
            KeyTerm terminalCorcheteIz = ToTerm("[", "corcheteIz");
            KeyTerm terminalCorcheteDer = ToTerm("]", "corchete_der");
            KeyTerm terminalLlaveIz = ToTerm("{", "llave_iz");
            KeyTerm terminalLlaveDer = ToTerm("}", "llave_der");
            KeyTerm terminalPunto = ToTerm(".", "punto");
            KeyTerm terminalPorcentaje = ToTerm("%", "porcentaje");
            KeyTerm terminalNumeral = ToTerm("#", "numeral");
            KeyTerm terminalComillaSimple = ToTerm("'", "comillaSimple");
            KeyTerm terminalComa = ToTerm(",", "coma");
            KeyTerm terminalArroba = ToTerm("@", "arroba");
            KeyTerm terminalIgual = ToTerm("=", "igual");

            /* COMENTARIOS */
            CommentTerminal comentarioLinea = new CommentTerminal("comentario_linea", "//", "\n", "\r\n");
            CommentTerminal comentarioBloque = new CommentTerminal("comentario_bloque", "/*", "*/");
            NonGrammarTerminals.Add(comentarioLinea);
            NonGrammarTerminals.Add(comentarioBloque);

            /* CONSTANTES E IDENTIFICADORES */
            Terminal terminalNumero = TerminalFactory.CreateCSharpNumber("numero");
            terminalNumero.AstConfig.NodeType = null;
            terminalNumero.AstConfig.DefaultNodeCreator = () => new NumberNode();
            Terminal terminalCadena = TerminalFactory.CreateCSharpString("cadena");
            terminalCadena.AstConfig.NodeType = null;
            terminalCadena.AstConfig.DefaultNodeCreator = () => new StringNode();
            ConstantTerminal terminalBool = new ConstantTerminal("constanteBool");
            terminalBool.Add("true", true);
            terminalBool.Add("false", false);
            terminalBool.AstConfig.NodeType = null;
            terminalBool.AstConfig.DefaultNodeCreator = () => new BooleanNode();
            Terminal terminalDateTimeValue = new QuotedValueLiteral("datetimeValue", "'", TypeCode.String);
            terminalDateTimeValue.AstConfig.NodeType = null;
            terminalDateTimeValue.AstConfig.DefaultNodeCreator = () => new DateTimeOrTimespanNode();
            ConstantTerminal terminalNull = new ConstantTerminal("constanteNull");
            terminalNull.Add("null", null);
            terminalNull.AstConfig.NodeType = null;
            terminalNull.AstConfig.DefaultNodeCreator = () => new NullValueNode();

            RegexBasedTerminal terminalId = new RegexBasedTerminal("[a-zA-Z]+([a-zA-Z]|[0-9]|[_])*");
            terminalId.AstConfig.NodeType = null;
            terminalId.AstConfig.DefaultNodeCreator = () => new IdentifierNode();

            /* PRECEDENCIA Y ASOCIATIVIDAD */
            this.RegisterBracePair("(", ")");
            this.RegisterBracePair("[", "]");
            this.RegisterBracePair("{", "}");
            this.RegisterOperators(40, Associativity.Right, terminalParentesisIz, this.valuesGrammar.ParentesisDer);
            this.RegisterOperators(35, Associativity.Right, terminalMenos);
            this.RegisterOperators(30, Associativity.Right, terminalIgualIgual, terminalNoIgual, terminalMayorIgual, terminalMayorQue, terminalMenorIgual, terminalMenorQue, terminalLike, terminalIn);
            this.RegisterOperators(20, Associativity.Right, terminalAnd);
            this.RegisterOperators(10, Associativity.Right, terminalOr);
            this.RegisterOperators(5, Associativity.Right, terminalNot);
            this.MarkPunctuation(terminalParentesisIz, this.valuesGrammar.ParentesisDer, terminalCorcheteIz, terminalCorcheteDer, terminalLlaveIz, terminalLlaveDer);

            /* NO TERMINALES */
            NonTerminal nt_EXPRESSION_VALUES = new NonTerminal("EXPRESSION_VALUES", typeof(ConstantValueNode));
            nt_EXPRESSION_VALUES.AstConfig.NodeType = null;
            nt_EXPRESSION_VALUES.AstConfig.DefaultNodeCreator = () => new ConstantValueNode();

            NonTerminal nt_PARAMETER_VALUES = new NonTerminal("PARAMETER_VALUES", typeof(ConstantValueNode));
            nt_PARAMETER_VALUES.AstConfig.NodeType = null;
            nt_PARAMETER_VALUES.AstConfig.DefaultNodeCreator = () => new ConstantValueNode();
            NonTerminal nt_VALUES_WITH_ALIAS = new NonTerminal("VALUES_WITH_ALIAS", typeof(ConstantValueWithAliasNode));
            nt_VALUES_WITH_ALIAS.AstConfig.NodeType = null;
            nt_VALUES_WITH_ALIAS.AstConfig.DefaultNodeCreator = () => new ConstantValueWithAliasNode();
            NonTerminal nt_LIST_OF_VALUES = new NonTerminal("LIST_OF_VALUES", typeof(PlanNodeListNode));
            nt_LIST_OF_VALUES.AstConfig.NodeType = null;
            nt_LIST_OF_VALUES.AstConfig.DefaultNodeCreator = () => new PlanNodeListNode();
            NonTerminal nt_DATE_FUNCTIONS = new NonTerminal("DATE_FUNCTIONS", typeof(DateFunctionNode));
            nt_DATE_FUNCTIONS.AstConfig.NodeType = null;
            nt_DATE_FUNCTIONS.AstConfig.DefaultNodeCreator = () => new DateFunctionNode();
            NonTerminal nt_EVENT = new NonTerminal("EVENT", typeof(EventNode));
            nt_EVENT.AstConfig.NodeType = null;
            nt_EVENT.AstConfig.DefaultNodeCreator = () => new EventNode();
            NonTerminal nt_EVENT_PROPERTIES = new NonTerminal("EVENT_VALUES", typeof(EventPropertiesNode));
            nt_EVENT_PROPERTIES.AstConfig.NodeType = null;
            nt_EVENT_PROPERTIES.AstConfig.DefaultNodeCreator = () => new EventPropertiesNode();
            NonTerminal nt_EVENT_WITH_SOURCE = new NonTerminal("EVENT_WITH_SOURCE", typeof(EventWithSource));
            nt_EVENT_WITH_SOURCE.AstConfig.NodeType = null;
            nt_EVENT_WITH_SOURCE.AstConfig.DefaultNodeCreator = () => new EventWithSource();
            NonTerminal nt_OBJECT_ID_OR_NUMBER = new NonTerminal("OBJECT_ID_OR_NUMBER", typeof(ObjectIdOrNumberNode));
            nt_OBJECT_ID_OR_NUMBER.AstConfig.NodeType = null;
            nt_OBJECT_ID_OR_NUMBER.AstConfig.DefaultNodeCreator = () => new ObjectIdOrNumberNode();
            NonTerminal nt_OBJECT = new NonTerminal("OBJECT", typeof(ObjectNode));
            nt_OBJECT.AstConfig.NodeType = null;
            nt_OBJECT.AstConfig.DefaultNodeCreator = () => new ObjectNode();
            NonTerminal nt_OBJECT_VALUE = new NonTerminal("OBJECT_VALUE", typeof(ObjectValueNode));
            nt_OBJECT_VALUE.AstConfig.NodeType = null;
            nt_OBJECT_VALUE.AstConfig.DefaultNodeCreator = () => new ObjectValueNode();
            NonTerminal nt_UNARY_ARITHMETIC_EXPRESSION = new NonTerminal("UNARY_ARITHMETIC_EXPRESSION", typeof(UnaryArithmeticExpressionNode));
            nt_UNARY_ARITHMETIC_EXPRESSION.AstConfig.NodeType = null;
            nt_UNARY_ARITHMETIC_EXPRESSION.AstConfig.DefaultNodeCreator = () => new UnaryArithmeticExpressionNode();
            NonTerminal nt_COMPARATIVE_EXPRESSION = new NonTerminal("COMPARATIVE_EXPRESSION", typeof(ComparativeExpressionNode));
            nt_COMPARATIVE_EXPRESSION.AstConfig.NodeType = null;
            nt_COMPARATIVE_EXPRESSION.AstConfig.DefaultNodeCreator = () => new ComparativeExpressionNode();
            this.logicExpression = new NonTerminal("LOGIC_EXPRESSION", typeof(LogicExpressionNode));
            this.logicExpression.AstConfig.NodeType = null;
            this.logicExpression.AstConfig.DefaultNodeCreator = () => new LogicExpressionNode();

            /* EXPRESIONES LÓGICAS */
            this.logicExpression.Rule = this.logicExpression + terminalAnd + this.logicExpression
                                    | this.logicExpression + terminalOr + this.logicExpression
                                    | terminalParentesisIz + this.logicExpression + this.valuesGrammar.ParentesisDer
                                    | terminalNot + terminalParentesisIz + this.logicExpression + this.valuesGrammar.ParentesisDer
                                    | nt_COMPARATIVE_EXPRESSION;
            /* **************************** */
            /* EXPRESIONES COMPARATIVAS */
            nt_COMPARATIVE_EXPRESSION.Rule = nt_EXPRESSION_VALUES + terminalIgualIgual + nt_EXPRESSION_VALUES
                                            | nt_EXPRESSION_VALUES + terminalNoIgual + nt_EXPRESSION_VALUES
                                            | nt_EXPRESSION_VALUES + terminalMayorIgual + nt_EXPRESSION_VALUES
                                            | nt_EXPRESSION_VALUES + terminalMayorQue + nt_EXPRESSION_VALUES
                                            | nt_EXPRESSION_VALUES + terminalMenorIgual + nt_EXPRESSION_VALUES
                                            | nt_EXPRESSION_VALUES + terminalMenorQue + nt_EXPRESSION_VALUES
                                            | nt_EXPRESSION_VALUES + terminalLike + terminalCadena
                                            | terminalParentesisIz + nt_COMPARATIVE_EXPRESSION + this.valuesGrammar.ParentesisDer
                                            | terminalNot + nt_COMPARATIVE_EXPRESSION
                                            | nt_EXPRESSION_VALUES;
            /* **************************** */
            
            /* CONSTANTES */
            nt_EXPRESSION_VALUES.Rule = this.valuesGrammar.OtherValues
                                        | this.valuesGrammar.NonConstantValues
                                        | this.valuesGrammar.NumericValues;

            this.Root = this.logicExpression;

            this.LanguageFlags = Irony.Parsing.LanguageFlags.CreateAst;
        }
    }
}
