//-----------------------------------------------------------------------
// <copyright file="DiagnosticHelper.cs" company="Integra.Vision.Diagnostics">
//     Copyright (c) Integra.Vision.Diagnostics. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Diagnostics
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Xml;
    
    /// <summary>
    /// Utility internal diagnostic helper
    /// </summary>
    internal static class DiagnosticHelper
    {
        /// <summary>
        /// Initializes static members of the <see cref="DiagnosticHelper"/> class.
        /// </summary>
        static DiagnosticHelper()
        {
            AppDomainFriendlyName = AppDomain.CurrentDomain.FriendlyName;
        }

        /// <summary>
        /// Gets current AppDomain name
        /// </summary>
        public static string AppDomainFriendlyName
        {
            get;
            private set;
        }

        /// <summary>
        /// Check if a exception is fatal
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>True if the exception is fatal, otherwise false.</returns>
        public static bool IsFatal(Exception exception)
        {
            while (exception != null)
            {
                if (((exception is OutOfMemoryException) && !(exception is InsufficientMemoryException)) || exception is ThreadAbortException)
                {
                    return true;
                }
                
                if ((exception is TypeInitializationException) || (exception is TargetInvocationException))
                {
                    exception = exception.InnerException;
                }
                else
                {
                    if (exception is AggregateException)
                    {
                        foreach (Exception exception2 in ((AggregateException)exception).InnerExceptions)
                        {
                            if (IsFatal(exception2))
                            {
                                return true;
                            }
                        }
                    }
                    
                    break;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Write an xml of an exception.
        /// </summary>
        /// <param name="xml">The writer used for xml format.</param>
        /// <param name="exception">The exception.</param>
        public static void AddExceptionToTraceString(XmlWriter xml, Exception exception)
        {
            xml.WriteElementString("ExceptionType", XmlEncode(exception.GetType().AssemblyQualifiedName));
            xml.WriteElementString("Message", XmlEncode(exception.Message));
            xml.WriteElementString("StackTrace", XmlEncode(StackTraceString(exception)));
            xml.WriteElementString("ExceptionString", XmlEncode(exception.ToString()));
            Win32Exception exception2 = exception as Win32Exception;
            if (exception2 != null)
            {
                xml.WriteElementString("NativeErrorCode", exception2.NativeErrorCode.ToString("X", CultureInfo.InvariantCulture));
            }
            
            if ((exception.Data != null) && (exception.Data.Count > 0))
            {
                xml.WriteStartElement("DataItems");
                foreach (object obj2 in exception.Data.Keys)
                {
                    xml.WriteStartElement("Data");
                    xml.WriteElementString("Key", XmlEncode(obj2.ToString()));
                    xml.WriteElementString("Value", XmlEncode(exception.Data[obj2].ToString()));
                    xml.WriteEndElement();
                }
                
                xml.WriteEndElement();
            }
            
            if (exception.InnerException != null)
            {
                xml.WriteStartElement("InnerException");
                AddExceptionToTraceString(xml, exception.InnerException);
                xml.WriteEndElement();
            }
        }

        /// <summary>
        /// Builds a string with the contents of a stack
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>A formatted string with exception information.</returns>
        public static string StackTraceString(Exception exception)
        {
            string stackTrace = exception.StackTrace;
            if (!string.IsNullOrEmpty(stackTrace))
            {
                return stackTrace;
            }
            
            StackFrame[] frames = new StackTrace(false).GetFrames();
            int skipFrames = 0;
            bool flag = false;
            foreach (StackFrame frame in frames)
            {
                string name = frame.GetMethod().Name;
                switch (name)
                {
                    case "StackTraceString":
                    case "AddExceptionToTraceString":
                    case "BuildTrace":
                    case "TraceEvent":
                    case "TraceException":
                    case "GetAdditionalPayload":
                        skipFrames++;
                        break;

                    default:
                        if (name.StartsWith("ThrowHelper", StringComparison.Ordinal))
                        {
                            skipFrames++;
                        }
                        else
                        {
                            flag = true;
                        }
                        
                        break;
                }
                
                if (flag)
                {
                    break;
                }
            }
            
            StackTrace trace = new StackTrace(skipFrames, false);
            return trace.ToString();
        }

        /// <summary>
        /// Generates a name based on the type of data source, getting the type of data to information.
        /// </summary>
        /// <param name="source">The object</param>
        /// <returns>Formatted string with source type information.</returns>
        public static string CreateSourceString(object source)
        {
            return source.GetType().ToString() + "/" + source.GetHashCode().ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Look for the type of trace event description.
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>A description of trace event.</returns>
        public static string LookupSeverity(TraceEventType type)
        {
            switch (type)
            {
                case TraceEventType.Critical:
                    return "Critical";

                case TraceEventType.Error:
                    return "Error";

                case TraceEventType.Warning:
                    return "Warning";

                case TraceEventType.Information:
                    return "Information";

                case TraceEventType.Verbose:
                    return "Verbose";

                case TraceEventType.Suspend:
                    return "Suspend";

                case TraceEventType.Transfer:
                    return "Transfer";

                case TraceEventType.Start:
                    return "Start";

                case TraceEventType.Stop:
                    return "Stop";
            }
            
            return type.ToString();
        }

        /// <summary>
        /// Encodes a string for proper inclusion in a document xml.
        /// </summary>
        /// <param name="text">The string to be encoded.</param>
        /// <returns>The proper string.</returns>
        public static string XmlEncode(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            
            int length = text.Length;
            StringBuilder builder = new StringBuilder(length + 8);
            for (int i = 0; i < length; i++)
            {
                char ch = text[i];
                switch (ch)
                {
                    case '<':
                        {
                            builder.Append("&lt;");
                            continue;
                        }
                    
                    case '>':
                        {
                            builder.Append("&gt;");
                            continue;
                        }
                    
                    case '&':
                        {
                            builder.Append("&amp;");
                            continue;
                        }
                }

                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}
