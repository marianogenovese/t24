//-----------------------------------------------------------------------
// <copyright file="ConsoleTraceListener.cs" company="Integra.Vision.Engine.Host">
//     Copyright (c) Integra.Vision.Engine.Host. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Host.Diagnostics
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    
    /// <summary>
    /// Trace listener that outputs to the console in color.
    /// </summary>
    public sealed class ConsoleTraceListener : TextWriterTraceListener
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleTraceListener"/> class.
        /// </summary>
        public ConsoleTraceListener() : base(Console.Out)
        {
            this.UseColors = true;
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether the trace listener can change the text color of the console.
        /// </summary>
        public bool UseColors { get; set; }
        
        /// <summary>
        /// Writes trace information, a message, and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A System.Diagnostics.TraceEventCache object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the System.Diagnostics.TraceEventType values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="message">A message to write.</param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null))
            {
                this.SetColor(eventType);
                this.Write(string.Format("[{0}] ", eventType));
                string output = null;
                if (this.TryGetDescription(message, out output))
                {
                    this.WriteLine(output);
                }
                else if (this.TryFormatXml(message, out output))
                {
                    this.WriteLine(output);
                }
                else
                {
                    this.WriteLine(message);
                }
                
                Console.ResetColor();
            }
        }
        
        /// <summary>
        /// Writes trace information, a formatted array of objects and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A System.Diagnostics.TraceEventCache object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the System.Diagnostics.TraceEventType values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="format">A format string that contains zero or more format items, which correspond to objects in the args array.</param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, format, args, null, null))
            {
                this.SetColor(eventType);
                this.Write(string.Format("[{0}] ", eventType));
                string output = null;
                if (this.TryGetDescription(format, out output))
                {
                    this.WriteLine(output);
                }
                else if (this.TryFormatXml(format, out output))
                {
                    this.WriteLine(output);
                }
                else
                {
                    if (args != null)
                    {
                        this.WriteLine(string.Format(CultureInfo.InvariantCulture, format, args));
                    }
                    else
                    {
                        this.WriteLine(format);
                    }
                }
                
                Console.ResetColor();
            }
        }
        
        /// <summary>
        /// When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void WriteLine(string message)
        {
            base.WriteLine(message);
        }

        /// <summary>
        /// Change the color of the console depending on the type of event.
        /// </summary>
        /// <param name="eventType">The event type</param>
        /// <returns>The new console color.</returns>
        private ConsoleColor SetColor(TraceEventType eventType)
        {
            var oldColor = Console.ForegroundColor;
            if (this.UseColors)
            {
                switch (eventType)
                {
                    case TraceEventType.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    /*
                    case TraceEventType.Information:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    */
                    
                    case TraceEventType.Verbose:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case TraceEventType.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    default:
                        break;
                }
            }

            return oldColor;
        }

        /// <summary>
        /// Try to parse the xml fragment and format it.
        /// </summary>
        /// <param name="xml">Original xml fragment.</param>
        /// <param name="result">Formatted xml fragment.</param>
        /// <returns>True if no errors on formatting; otherwise false.</returns>
        private bool TryFormatXml(string xml, out string result)
        {
            bool noError = true;
            result = null;

            XElement element;

            try
            {
                element = XElement.Parse(xml);
            }
            catch
            {
                return false;
            }
            
            StringBuilder stringBuilder = new StringBuilder();

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = false;

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            result = stringBuilder.ToString();

            return noError;
        }

        /// <summary>
        /// Try to parse the xml fragment and get the trace message.
        /// </summary>
        /// <param name="xml">Original xml fragment.</param>
        /// <param name="message">The trace message.</param>
        /// <returns>True if no errors on formatting; otherwise false.</returns>
        private bool TryGetDescription(string xml, out string message)
        {
            message = null;
            XElement element;

            try
            {
                element = XElement.Parse(xml);
            }
            catch
            {
                return false;
            }
            
            if (element.IsEmpty)
            {
                return false;
            }

            XElement descriptionElement = element.Element(XName.Get("Description", "http://Integra.Vision/Diagnostics/Integra.Vision.Engine.Host/TraceRecord"));
            if (descriptionElement != null && !descriptionElement.IsEmpty)
            {
                message = descriptionElement.Value;
            }

            XElement exceptionElement = element.Element(XName.Get("Exception", "http://Integra.Vision/Diagnostics/Integra.Vision.Engine.Host/TraceRecord"));
            if (exceptionElement != null && !exceptionElement.IsEmpty)
            {
                if (string.IsNullOrEmpty(message))
                {
                    message = exceptionElement.Value;
                }
                else
                {
                    message += Environment.NewLine;
                    message += exceptionElement.Value;
                }
            }

            return true;
        }
    }
}
