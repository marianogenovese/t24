//-----------------------------------------------------------------------
// <copyright file="ExpressionGrammar.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.Grammars
{
    using System;
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
        /// Initializes a new instance of the <see cref="ExpressionGrammar"/> class
        /// </summary>
        public ExpressionGrammar()
            : base(false)
        {
            this.Grammar(false);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionGrammar"/> class
        /// </summary>
        /// <param name="prueba">flag for tests</param>
        public ExpressionGrammar(bool prueba)
            : base(false)
        {
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

            /* OPERADORES LOGICOS */
            KeyTerm terminalAnd = ToTerm("and", "and");
            KeyTerm terminalOr = ToTerm("or", "or");
            
            /* SIMBOLOS */
            KeyTerm terminalParentesisIz = ToTerm("(", "parentesisIz");
            KeyTerm terminalParentesisDer = ToTerm(")", "parentesisDer");
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
            Terminal terminalId = TerminalFactory.CreateCSharpIdentifier("identificador");
            terminalId.AstConfig.NodeType = null;
            terminalId.AstConfig.DefaultNodeCreator = () => new IdentifierNode();

            /* NO TERMINALES */
            NonTerminal nt_VALUES = new NonTerminal("VALUES", typeof(ConstantValueNode));
            nt_VALUES.AstConfig.NodeType = null;
            nt_VALUES.AstConfig.DefaultNodeCreator = () => new ConstantValueNode();
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
            NonTerminal nt_ARITHMETIC_EXPRESSION = new NonTerminal("ARITHMETIC_EXPRESSION", typeof(ArithmeticExpressionNode));
            nt_ARITHMETIC_EXPRESSION.AstConfig.NodeType = null;
            nt_ARITHMETIC_EXPRESSION.AstConfig.DefaultNodeCreator = () => new ArithmeticExpressionNode();
            NonTerminal nt_COMPARATIVE_EXPRESSION = new NonTerminal("COMPARATIVE_EXPRESSION", typeof(ComparativeExpressionNode));
            nt_COMPARATIVE_EXPRESSION.AstConfig.NodeType = null;
            nt_COMPARATIVE_EXPRESSION.AstConfig.DefaultNodeCreator = () => new ComparativeExpressionNode();
            this.logicExpression = new NonTerminal("LOGIC_EXPRESSION", typeof(LogicExpressionNode));
            this.logicExpression.AstConfig.NodeType = null;
            this.logicExpression.AstConfig.DefaultNodeCreator = () => new LogicExpressionNode();

            this.logicExpression.Rule = this.logicExpression + terminalAnd + this.logicExpression
                                    | this.logicExpression + terminalOr + this.logicExpression
                                    | terminalParentesisIz + this.logicExpression + terminalParentesisDer
                                    | terminalNot + terminalParentesisIz + this.logicExpression + terminalParentesisDer
                                    | nt_COMPARATIVE_EXPRESSION;
            /* **************************** */
            /* EXPRESIONES COMPARATIVAS */
            nt_COMPARATIVE_EXPRESSION.Rule = nt_COMPARATIVE_EXPRESSION + terminalIgualIgual + nt_COMPARATIVE_EXPRESSION
                                            | nt_COMPARATIVE_EXPRESSION + terminalNoIgual + nt_COMPARATIVE_EXPRESSION
                                            | nt_COMPARATIVE_EXPRESSION + terminalMayorIgual + nt_COMPARATIVE_EXPRESSION
                                            | nt_COMPARATIVE_EXPRESSION + terminalMayorQue + nt_COMPARATIVE_EXPRESSION
                                            | nt_COMPARATIVE_EXPRESSION + terminalMenorIgual + nt_COMPARATIVE_EXPRESSION
                                            | nt_COMPARATIVE_EXPRESSION + terminalMenorQue + nt_COMPARATIVE_EXPRESSION
                                            | nt_COMPARATIVE_EXPRESSION + terminalLike + terminalCadena
                                            | terminalParentesisIz + nt_COMPARATIVE_EXPRESSION + terminalParentesisDer
                                            | terminalNot + nt_COMPARATIVE_EXPRESSION
                                            | nt_ARITHMETIC_EXPRESSION;
            /* **************************** */
            /* EXPRESIONES ARITMETICAS */
            nt_ARITHMETIC_EXPRESSION.Rule = nt_ARITHMETIC_EXPRESSION + terminalMenos + nt_ARITHMETIC_EXPRESSION
                                            | nt_VALUES;
            /* **************************** */
            /* VALORES PERMITIDOS PARA LOS PARAMETROS DE UN ADAPTADOR */
            nt_PARAMETER_VALUES.Rule = terminalDateTimeValue
                                        | terminalBool
                                        | terminalNull
                                        | terminalNumero
                                        | terminalCadena;
            /* **************************** */
            /* CONSTANTES */
            nt_VALUES.Rule = terminalDateTimeValue
                            | terminalBool
                            | terminalNull
                            | terminalNumero
                            | terminalCadena
                            | nt_EVENT_PROPERTIES
                            | nt_UNARY_ARITHMETIC_EXPRESSION
                            | nt_EVENT_WITH_SOURCE
                            | nt_OBJECT_VALUE
                            | nt_DATE_FUNCTIONS;
            /* **************************** */
            /* FUNCIONES DE FECHAS */
            nt_DATE_FUNCTIONS.Rule = terminalHour + terminalParentesisIz + terminalDateTimeValue + terminalParentesisDer
                                    | terminalMinute + terminalParentesisIz + terminalDateTimeValue + terminalParentesisDer
                                    | terminalSecond + terminalParentesisIz + terminalDateTimeValue + terminalParentesisDer;
            /* **************************** */
            /* OPERACION ARITMETICA UNARIA */
            nt_UNARY_ARITHMETIC_EXPRESSION.Rule = terminalMenos + terminalNumero
                                                    | terminalMas + terminalNumero;
            /* **************************** */
            /* OBJETOS CON/SIN ALIAS */
            nt_OBJECT_VALUE.Rule = nt_OBJECT;
            /* **************************** */
            /* OBJETOS */
            nt_OBJECT.Rule = nt_OBJECT + terminalPunto + nt_OBJECT_ID_OR_NUMBER
                                | nt_EVENT + terminalPunto + terminalMessage + terminalPunto + nt_OBJECT_ID_OR_NUMBER + terminalPunto + nt_OBJECT_ID_OR_NUMBER;
            /* **************************** */
            /* IDENTIFICADORES DE PARTES Y CAMPOS DE OBJETOS */
            nt_OBJECT_ID_OR_NUMBER.Rule = terminalNumeral + terminalNumero
                                        | terminalId
                                        | terminalCorcheteIz + terminalCadena + terminalCorcheteDer;
            /* **************************** */
            /* VALORES DEL EVENTO */
            nt_EVENT_PROPERTIES.Rule = nt_EVENT_PROPERTIES + terminalPunto + terminalId
                                        | nt_EVENT + terminalPunto + terminalAdapter
                                        | nt_EVENT + terminalPunto + terminalAgent;
            /* **************************** */
            /* EVENTO */
            nt_EVENT.Rule = terminalArroba + terminalEvent;
            /* **************************** */
            /* EVENTO CON FUENTE */
            nt_EVENT_WITH_SOURCE.Rule = terminalId + terminalPunto + nt_OBJECT_VALUE
                                        | terminalId + terminalPunto + nt_EVENT_PROPERTIES;
            /* **************************** */

            this.LanguageFlags = Irony.Parsing.LanguageFlags.CreateAst;
        }
    }
}
