//-----------------------------------------------------------------------
// <copyright file="EQLGrammar.cs" company="Integra.Vision.Language">
//     Copyright (c) Integra.Vision.Language. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Language.Grammars
{
    using System;
    using Integra.Vision.Language.ASTNodes.Commands.Create;
    using Integra.Vision.Language.ASTNodes.Commands.Drop;
    using Integra.Vision.Language.ASTNodes.Commands.General;
    using Integra.Vision.Language.ASTNodes.Commands.StartStop;
    using Integra.Vision.Language.ASTNodes.Commands.Trace;
    using Integra.Vision.Language.ASTNodes.Constants;
    using Integra.Vision.Language.ASTNodes.Lists;
    using Integra.Vision.Language.ASTNodes.Objects.Event;
    using Integra.Vision.Language.ASTNodes.Objects.Object;
    using Integra.Vision.Language.ASTNodes.Operations;
    using Integra.Vision.Language.ASTNodes.Permissions;
    using Integra.Vision.Language.ASTNodes.QuerySections;
    using Integra.Vision.Language.ASTNodes.Root;
    using Integra.Vision.Language.ASTNodes.SystemViews;
    using Irony.Interpreter;
    using Irony.Parsing;

    /// <summary>
    /// EQLGrammar grammar for the commands and the predicates 
    /// </summary>
    [Language("EQLGrammar", "0.2", "")]
    internal sealed class EQLGrammar : InterpretedLanguageGrammar
    {
        /// <summary>
        /// Initializes a new instance of the EQLGrammar class
        /// </summary>
        public EQLGrammar()
            : base(false)
        {
            this.Grammar(false);
        }

        /// <summary>
        /// Initializes a new instance of the EQLGrammar class
        /// </summary>
        /// <param name="prueba">flag for tests</param>
        public EQLGrammar(bool prueba)
            : base(false)
        {
            this.Grammar(prueba);
        }

        /// <summary>
        /// Specify the language grammar
        /// </summary>
        /// <param name="prueba">flag for tests</param>
        public void Grammar(bool prueba)
        {
            /*** TERMINALES DE LA GRAMATICA ***/

            /* PALABRAS RESERVADAS */
            KeyTerm terminalSelect = ToTerm("select", "select");
            KeyTerm terminalFrom = ToTerm("from", "from");
            KeyTerm terminalWhere = ToTerm("where", "where");
            KeyTerm terminalNot = ToTerm("not", "not");
            KeyTerm terminalIn = ToTerm("in", "in");
            KeyTerm terminalLike = ToTerm("like", "like");
            KeyTerm terminalAs = ToTerm("as", "as");
            KeyTerm terminalStream = ToTerm("stream", "stream");
            KeyTerm terminalSource = ToTerm("source", "source");
            KeyTerm terminalTrigger = ToTerm("trigger", "trigger");
            KeyTerm terminalAssembly = ToTerm("assembly", "assembly");
            KeyTerm terminalJoin = ToTerm("join", "join");
            KeyTerm terminalWith = ToTerm("with", "with");
            KeyTerm terminalOn = ToTerm("on", "on");
            KeyTerm terminalApply = ToTerm("apply", "apply");
            KeyTerm terminalReference = ToTerm("reference", "reference");
            KeyTerm terminalFor = ToTerm("for", "for");
            KeyTerm terminalWindow = ToTerm("window", "window");
            KeyTerm terminalSend = ToTerm("send", "send");
            KeyTerm terminalTo = ToTerm("to", "to");
            KeyTerm terminalHasEvents = ToTerm("hasevents", "hasevents");
            KeyTerm terminalIf = ToTerm("if", "if");
            KeyTerm terminalEndIf = ToTerm("endif", "endif");
            KeyTerm terminalRole = ToTerm("role", "role");
            KeyTerm terminalUser = ToTerm("user", "user");
            KeyTerm terminalPassword = ToTerm("password", "password");
            KeyTerm terminalStatus = ToTerm("status", "status");
            KeyTerm terminalDrop = ToTerm("drop", "drop");
            KeyTerm terminalGrant = ToTerm("grant", "grant");
            KeyTerm terminalRevoke = ToTerm("revoke", "revoke");
            KeyTerm terminalDeny = ToTerm("deny", "deny");
            KeyTerm terminalServer = ToTerm("server", "server");
            KeyTerm terminalStart = ToTerm("start", "start");
            KeyTerm terminalStop = ToTerm("stop", "stop");
            KeyTerm terminalTrace = ToTerm("trace", "trace");
            KeyTerm terminalSet = ToTerm("set", "set");
            KeyTerm terminalLevel = ToTerm("level", "level");
            KeyTerm terminalEngine = ToTerm("engine", "engine");

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
            ConstantTerminal terminalAdapterType = new ConstantTerminal("terminalAdateperType");
            terminalAdapterType.Add("input", "input");
            terminalAdapterType.Add("output", "output");
            terminalAdapterType.AstConfig.NodeType = null;
            terminalAdapterType.AstConfig.DefaultNodeCreator = () => new AdapterTypeNode();
            ConstantTerminal terminalUserStatus = new ConstantTerminal("terminalUserStatus");
            terminalUserStatus.Add("enable", "enable");
            terminalUserStatus.Add("disable", "disable");
            terminalUserStatus.AstConfig.NodeType = null;
            terminalUserStatus.AstConfig.DefaultNodeCreator = () => new ConstantStringValueNode();
            ConstantTerminal terminalCreateAlter = new ConstantTerminal("terminalCreateAlter");
            terminalCreateAlter.Add("create", "create");
            terminalCreateAlter.Add("alter", "alter");
            terminalCreateAlter.AstConfig.NodeType = null;
            terminalCreateAlter.AstConfig.DefaultNodeCreator = () => new ConstantStringValueNode();

            /* VISTAS DEL SISTEMA */
            RegexBasedTerminal terminalUnidadDeTexto = new RegexBasedTerminal("texto", @"((?!where)|(?!from)|(?!select)).+");
            terminalUnidadDeTexto.AstConfig.NodeType = null;
            terminalUnidadDeTexto.AstConfig.DefaultNodeCreator = () => new StringNode();

            /* PRECEDENCIA Y ASOCIATIVIDAD */
            this.RegisterBracePair("(", ")");
            this.RegisterBracePair("[", "]");
            this.RegisterBracePair("{", "}");
            this.RegisterOperators(40, Associativity.Right, terminalParentesisIz, terminalParentesisDer);
            this.RegisterOperators(35, Associativity.Right, terminalMenos);
            this.RegisterOperators(30, Associativity.Right, terminalIgualIgual, terminalNoIgual, terminalMayorIgual, terminalMayorQue, terminalMenorIgual, terminalMenorQue, terminalLike, terminalIn);
            this.RegisterOperators(20, Associativity.Right, terminalAnd);
            this.RegisterOperators(10, Associativity.Right, terminalOr);
            this.RegisterOperators(5, Associativity.Right, terminalNot);
            this.MarkPunctuation(terminalParentesisIz, terminalParentesisDer, terminalCorcheteIz, terminalCorcheteDer, terminalLlaveIz, terminalLlaveDer);

            /* COMENTARIOS */
            CommentTerminal comentarioLinea = new CommentTerminal("comentario_linea", "//", "\n", "\r\n");
            CommentTerminal comentarioBloque = new CommentTerminal("comentario_bloque", "/*", "*/");
            NonGrammarTerminals.Add(comentarioLinea);
            NonGrammarTerminals.Add(comentarioBloque);

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
            NonTerminal nt_LOGIC_EXPRESSION = new NonTerminal("LOGIC_EXPRESSION", typeof(LogicExpressionNode));
            nt_LOGIC_EXPRESSION.AstConfig.NodeType = null;
            nt_LOGIC_EXPRESSION.AstConfig.DefaultNodeCreator = () => new LogicExpressionNode();
            NonTerminal nt_WHERE = new NonTerminal("WHERE", typeof(WhereNode));
            nt_WHERE.AstConfig.NodeType = null;
            nt_WHERE.AstConfig.DefaultNodeCreator = () => new WhereNode();
            NonTerminal nt_SELECT = new NonTerminal("SELECT", typeof(SelectNode));
            nt_SELECT.AstConfig.NodeType = null;
            nt_SELECT.AstConfig.DefaultNodeCreator = () => new SelectNode();
            NonTerminal nt_FROM = new NonTerminal("FROM", typeof(FromNode));
            nt_FROM.AstConfig.NodeType = null;
            nt_FROM.AstConfig.DefaultNodeCreator = () => new FromNode();
            NonTerminal nt_JOIN = new NonTerminal("JOIN", typeof(JoinNode));
            nt_JOIN.AstConfig.NodeType = null;
            nt_JOIN.AstConfig.DefaultNodeCreator = () => new JoinNode();
            NonTerminal nt_ON = new NonTerminal("ON", typeof(OnConditionNode));
            nt_ON.AstConfig.NodeType = null;
            nt_ON.AstConfig.DefaultNodeCreator = () => new OnConditionNode();
            NonTerminal nt_WITH = new NonTerminal("WITH", typeof(WithNode));
            nt_WITH.AstConfig.NodeType = null;
            nt_WITH.AstConfig.DefaultNodeCreator = () => new WithNode();
            NonTerminal nt_APPLY_WINDOW = new NonTerminal("APPLY_WINDOW", typeof(ApplyWindowNode));
            nt_APPLY_WINDOW.AstConfig.NodeType = null;
            nt_APPLY_WINDOW.AstConfig.DefaultNodeCreator = () => new ApplyWindowNode();
            NonTerminal nt_PARAMETER = new NonTerminal("PARAMETER", typeof(ParameterNode));
            nt_PARAMETER.AstConfig.NodeType = null;
            nt_PARAMETER.AstConfig.DefaultNodeCreator = () => new ParameterNode();
            NonTerminal nt_PARAMETER_LIST = new NonTerminal("PARAMETER_LIST", typeof(PlanNodeListNode));
            nt_PARAMETER_LIST.AstConfig.NodeType = null;
            nt_PARAMETER_LIST.AstConfig.DefaultNodeCreator = () => new ParameterListNode();
            NonTerminal nt_SEND = new NonTerminal("SEND", typeof(SendNode));
            nt_SEND.AstConfig.NodeType = null;
            nt_SEND.AstConfig.DefaultNodeCreator = () => new SendNode();
            NonTerminal nt_SEND_LIST = new NonTerminal("SEND_LIST", typeof(ParameterListNode));
            nt_SEND_LIST.AstConfig.NodeType = null;
            nt_SEND_LIST.AstConfig.DefaultNodeCreator = () => new ParameterListNode();
            NonTerminal nt_IF = new NonTerminal("IF", typeof(IfNode));
            nt_IF.AstConfig.NodeType = null;
            nt_IF.AstConfig.DefaultNodeCreator = () => new IfNode();
            NonTerminal nt_IF_SEND_LIST = new NonTerminal("IF_SEND_LIST", typeof(ParameterListNode));
            nt_IF_SEND_LIST.AstConfig.NodeType = null;
            nt_IF_SEND_LIST.AstConfig.DefaultNodeCreator = () => new ParameterListNode();

            NonTerminal nt_CREATE_ADAPTER = new NonTerminal("CREATE_ADAPTER", typeof(CreateAdapterNode));
            nt_CREATE_ADAPTER.AstConfig.NodeType = null;
            nt_CREATE_ADAPTER.AstConfig.DefaultNodeCreator = () => new CreateAdapterNode();
            NonTerminal nt_CREATE_STREAM = new NonTerminal("CREATE_STREAM", typeof(CreateStreamNode));
            nt_CREATE_STREAM.AstConfig.NodeType = null;
            nt_CREATE_STREAM.AstConfig.DefaultNodeCreator = () => new CreateStreamNode();
            NonTerminal nt_CREATE_SOURCE = new NonTerminal("CREATE_SOURCE", typeof(CreateSourceNode));
            nt_CREATE_SOURCE.AstConfig.NodeType = null;
            nt_CREATE_SOURCE.AstConfig.DefaultNodeCreator = () => new CreateSourceNode();
            NonTerminal nt_CREATE_TRIGGER = new NonTerminal("CREATE_TRIGGER", typeof(CreateTriggerNode));
            nt_CREATE_TRIGGER.AstConfig.NodeType = null;
            nt_CREATE_TRIGGER.AstConfig.DefaultNodeCreator = () => new CreateTriggerNode();
            NonTerminal nt_CREATE_ASSEMBLY = new NonTerminal("CREATE_ASSEMBLY", typeof(CreateAssembly));
            nt_CREATE_ASSEMBLY.AstConfig.NodeType = null;
            nt_CREATE_ASSEMBLY.AstConfig.DefaultNodeCreator = () => new CreateAssembly();
            NonTerminal nt_CREATE_ROLE = new NonTerminal("CREATE_ROLE", typeof(CreateRole));
            nt_CREATE_TRIGGER.AstConfig.NodeType = null;
            nt_CREATE_ROLE.AstConfig.DefaultNodeCreator = () => new CreateRole();
            NonTerminal nt_CREATE_USER = new NonTerminal("CREATE_USER", typeof(CreateUser));
            nt_CREATE_USER.AstConfig.NodeType = null;
            nt_CREATE_USER.AstConfig.DefaultNodeCreator = () => new CreateUser();
            NonTerminal nt_USER_DEFINED_OBJECTS = new NonTerminal("USER_DEFINED_OBJECTS", typeof(UserDefinedObjectsNode));
            nt_USER_DEFINED_OBJECTS.AstConfig.NodeType = null;
            nt_USER_DEFINED_OBJECTS.AstConfig.DefaultNodeCreator = () => new UserDefinedObjectsNode();
            NonTerminal nt_DROP = new NonTerminal("DROP", typeof(DropUserDefinedObjectNode));
            nt_DROP.AstConfig.NodeType = null;
            nt_DROP.AstConfig.DefaultNodeCreator = () => new DropUserDefinedObjectNode();
            NonTerminal nt_PERMISSIONS = new NonTerminal("PERMISSIONS", typeof(PermissionsNode));
            nt_PERMISSIONS.AstConfig.NodeType = null;
            nt_PERMISSIONS.AstConfig.DefaultNodeCreator = () => new PermissionsNode();
            NonTerminal nt_USER_OR_ROLE = new NonTerminal("PERMISSIONS_BODY", typeof(PermissionsBodyNode));
            nt_USER_OR_ROLE.AstConfig.NodeType = null;
            nt_USER_OR_ROLE.AstConfig.DefaultNodeCreator = () => new PermissionsBodyNode();
            NonTerminal nt_SECURE_OBJECTS = new NonTerminal("SECURE_OBJECTS", typeof(SecureObjectsNode));
            nt_SECURE_OBJECTS.AstConfig.NodeType = null;
            nt_SECURE_OBJECTS.AstConfig.DefaultNodeCreator = () => new SecureObjectsNode();
            NonTerminal nt_OBJECTS_TO_START_OR_STOP = new NonTerminal("OBJECTS_TO_START_OR_STOP", typeof(ObjectToStartOrStopNode));
            nt_OBJECTS_TO_START_OR_STOP.AstConfig.NodeType = null;
            nt_OBJECTS_TO_START_OR_STOP.AstConfig.DefaultNodeCreator = () => new ObjectToStartOrStopNode();
            NonTerminal nt_START_STOP = new NonTerminal("START_STOP", typeof(StartStopNode));
            nt_START_STOP.AstConfig.NodeType = null;
            nt_START_STOP.AstConfig.DefaultNodeCreator = () => new StartStopNode();
            NonTerminal nt_OBJECTS_TO_TRACE = new NonTerminal("OBJECTS_TO_TRACE", typeof(ObjectToStartOrStopNode));
            nt_OBJECTS_TO_TRACE.AstConfig.NodeType = null;
            nt_OBJECTS_TO_TRACE.AstConfig.DefaultNodeCreator = () => new ObjectToStartOrStopNode();
            NonTerminal nt_SET_TRACE = new NonTerminal("SET_TRACE", typeof(SetTraceNode));
            nt_SET_TRACE.AstConfig.NodeType = null;
            nt_SET_TRACE.AstConfig.DefaultNodeCreator = () => new SetTraceNode();
            NonTerminal nt_SYSTEM_VIEW = new NonTerminal("SYSTEM_VIEW", typeof(SystemViewNode));
            nt_SYSTEM_VIEW.AstConfig.NodeType = null;
            nt_SYSTEM_VIEW.AstConfig.DefaultNodeCreator = () => new SystemViewNode();

            NonTerminal nt_COMMAND_NODE = new NonTerminal("COMMAND_NODE", typeof(CommandNode));
            nt_COMMAND_NODE.AstConfig.NodeType = null;
            nt_COMMAND_NODE.AstConfig.DefaultNodeCreator = () => new CommandNode();
            NonTerminal nt_COMMAND_LIST = new NonTerminal("COMMAND_LIST", typeof(CommandListNode));
            nt_COMMAND_LIST.AstConfig.NodeType = null;
            nt_COMMAND_LIST.AstConfig.DefaultNodeCreator = () => new CommandListNode();
            
            /* SYSTEM VIEWS */
            nt_SYSTEM_VIEW.Rule = terminalFrom + terminalUnidadDeTexto 
                                    + terminalWhere + terminalUnidadDeTexto
                                    + terminalSelect + terminalUnidadDeTexto;
            /* **************************** */
            /* SET TRACE */
            nt_SET_TRACE.Rule = terminalSet + terminalTrace + terminalLevel + terminalNumero + terminalTo + nt_OBJECTS_TO_TRACE;

            nt_OBJECTS_TO_TRACE.Rule = terminalAdapter
                                                | terminalSource
                                                | terminalStream
                                                | terminalTrigger
                                                | terminalEngine
                                                | terminalId;
            /* **************************** */
            /* START o STOP */
            nt_START_STOP.Rule = terminalStart + nt_OBJECTS_TO_START_OR_STOP + terminalId
                                    | terminalStop + nt_OBJECTS_TO_START_OR_STOP + terminalId;

            nt_OBJECTS_TO_START_OR_STOP.Rule = terminalAdapter
                                                | terminalSource
                                                | terminalStream
                                                | terminalTrigger;
            /* **************************** */

            /* PERMISOS */
            nt_PERMISSIONS.Rule = terminalGrant + nt_SECURE_OBJECTS + terminalId + terminalTo + nt_USER_OR_ROLE
                                    | terminalRevoke + nt_SECURE_OBJECTS + terminalId + terminalTo + nt_USER_OR_ROLE
                                    | terminalDeny + nt_SECURE_OBJECTS + terminalId + terminalTo + nt_USER_OR_ROLE;

            nt_USER_OR_ROLE.Rule = terminalUser + terminalId 
                                        | terminalRole + terminalId;

            nt_SECURE_OBJECTS.Rule = terminalStream 
                                        | terminalRole 
                                        | terminalServer + terminalRole;
            /* **************************** */

            /* DROP */
            nt_DROP.Rule = terminalDrop + nt_USER_DEFINED_OBJECTS + terminalId;

            nt_USER_DEFINED_OBJECTS.Rule = terminalAssembly
                                            | terminalAdapter
                                            | terminalSource
                                            | terminalStream
                                            | terminalTrigger
                                            | terminalRole
                                            | terminalUser;
            /* **************************** */

            /* EXPRESIONES CREATE */

            /* CREAR ROL */
            nt_CREATE_ROLE.Rule = this.ToTerm("create") + terminalRole + terminalId;
            /* **************************** */
            /* CREAR USUARIO */
            nt_CREATE_USER.Rule = terminalCreateAlter + terminalUser + terminalId + terminalWith + terminalPassword + terminalIgual + terminalCadena + terminalComa + terminalStatus + terminalIgual + terminalUserStatus;
            /* **************************** */
            /* CREAR ASSEMBLY */
            nt_CREATE_ASSEMBLY.Rule = terminalCreateAlter + terminalAssembly + terminalId + terminalFrom + terminalCadena;
            /* **************************** */
            /* CREAR ADAPTADOR */
            nt_CREATE_ADAPTER.Rule = terminalCreateAlter + terminalAdapter + terminalId + terminalFor + terminalAdapterType + terminalAs + nt_PARAMETER_LIST + terminalReference + terminalId;
            /* **************************** */
            /* LISTA DE PARAMETROS */
            nt_PARAMETER_LIST.Rule = nt_PARAMETER_LIST + nt_PARAMETER
                                        | nt_PARAMETER;
            /* **************************** */
            /* PARAMETER */
            nt_PARAMETER.Rule = terminalId + terminalArroba + terminalId + terminalIgual + nt_PARAMETER_VALUES;
            /* **************************** */
            /* CREAR FUENTE */
            nt_CREATE_SOURCE.Rule = terminalCreateAlter + terminalSource + terminalId + terminalAs + nt_FROM + nt_WHERE;
            /* **************************** */
            /* CREAR FLUJO */
            nt_CREATE_STREAM.Rule = terminalCreateAlter + terminalStream + terminalId + terminalAs + nt_FROM + nt_WHERE + nt_SELECT
                                    | terminalCreateAlter + terminalStream + terminalId + terminalAs + nt_JOIN + nt_WITH + nt_ON + nt_APPLY_WINDOW + nt_WHERE + nt_SELECT;
            /* **************************** */

            /* **************************** */
            /* JOIN */
            nt_JOIN.Rule = terminalJoin + terminalId + terminalAs + terminalId;
            /* **************************** */
            /* WITH */
            nt_WITH.Rule = terminalWith + terminalId + terminalAs + terminalId;
            /* **************************** */
            /* ON */
            nt_ON.Rule = terminalOn + nt_LOGIC_EXPRESSION;
            /* **************************** */
            /* APPLY WINDOW */
            nt_APPLY_WINDOW.Rule = terminalApply + terminalWindow + terminalDateTimeValue;
            /* **************************** */
            /* FROM */
            nt_FROM.Rule = terminalFrom + terminalId;
            /* SELECT */
            nt_SELECT.Rule = terminalSelect + nt_LIST_OF_VALUES;
            /* **************************** */
            /* CREATE TRIGGER */
            nt_CREATE_TRIGGER.Rule = terminalCreateAlter + terminalTrigger + terminalId + terminalOn + terminalId + terminalAs + nt_SEND_LIST
                                        | terminalCreateAlter + terminalTrigger + terminalId + terminalOn + terminalId + nt_APPLY_WINDOW + terminalAs + nt_IF_SEND_LIST;
            /* **************************** */
            /* IF SEND */
            nt_IF_SEND_LIST.Rule = nt_IF_SEND_LIST + nt_IF
                                    | nt_IF_SEND_LIST + nt_SEND
                                    | nt_IF
                                    | nt_SEND;
            /* **************************** */
            /* IF */
            nt_IF.Rule = terminalIf + terminalArroba + terminalHasEvents + nt_SEND_LIST + terminalEndIf
                            | terminalIf + terminalNot + terminalArroba + terminalHasEvents + nt_SEND_LIST + terminalEndIf;
            /* **************************** */
            /* LISTA DE SEND */
            nt_SEND_LIST.Rule = nt_SEND_LIST + nt_SEND
                                | nt_SEND;
            /* **************************** */
            /* SEND */
            nt_SEND.Rule = terminalSend + nt_EVENT + terminalTo + terminalId;
            /* **************************** */
            /* LISTA DE VALORES */
            nt_LIST_OF_VALUES.Rule = nt_LIST_OF_VALUES + terminalComa + nt_VALUES_WITH_ALIAS
                                    | nt_VALUES_WITH_ALIAS;
            /* **************************** */
            /* VALORES CON ALIAS */
            nt_VALUES_WITH_ALIAS.Rule = nt_VALUES + terminalAs + terminalId;
            /* **************************** */
            /* WHERE */
            nt_WHERE.Rule = terminalWhere + nt_LOGIC_EXPRESSION;
            /* **************************** */
            /* EXPRESIONES LOGICAS */
            nt_LOGIC_EXPRESSION.Rule = nt_LOGIC_EXPRESSION + terminalAnd + nt_LOGIC_EXPRESSION
                                    | nt_LOGIC_EXPRESSION + terminalOr + nt_LOGIC_EXPRESSION
                                    | terminalParentesisIz + nt_LOGIC_EXPRESSION + terminalParentesisDer
                                    | terminalNot + terminalParentesisIz + nt_LOGIC_EXPRESSION + terminalParentesisDer
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

            /* COMANDOS */
            nt_COMMAND_NODE.Rule = nt_CREATE_ASSEMBLY
                                        | nt_CREATE_ADAPTER
                                        | nt_CREATE_SOURCE
                                        | nt_CREATE_STREAM
                                        | nt_CREATE_TRIGGER
                                        | nt_CREATE_ROLE
                                        | nt_CREATE_USER
                                        | nt_DROP
                                        | nt_PERMISSIONS
                                        | nt_START_STOP
                                        | nt_SET_TRACE
                                        | nt_SYSTEM_VIEW;
            /* **************************** */
            /* LISTA DE COMANDOS */
            nt_COMMAND_LIST.Rule = nt_COMMAND_LIST + nt_COMMAND_NODE
                                    | nt_COMMAND_NODE;
            /* **************************** */

            if (prueba)
            {
                this.Root = nt_VALUES;
            }
            else
            {
                this.Root = nt_COMMAND_LIST;
            }

            this.LanguageFlags = Irony.Parsing.LanguageFlags.CreateAst;
        }
    }
}
