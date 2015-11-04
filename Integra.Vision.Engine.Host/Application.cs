//-----------------------------------------------------------------------
// <copyright file="Application.cs" company="Integra.Vision.Engine.Host">
//     Copyright (c) Integra.Vision.Engine.Host. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Host
{
    using System;
    using System.Globalization;
    using System.ServiceProcess;
    using System.Threading;
    using Integra.Vision.Engine.Host.Diagnostics;

    /// <summary>
    /// Integra Vision Engine Service Host Entry point
    /// </summary>
    public static class Application
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">Context execution arguments</param>
        /// <returns>Returns an exit code</returns>
        internal static int Main(string[] args)
        {
            ThreadPool.SetMaxThreads(500, 500);
            ThreadPool.SetMinThreads(500, 500);

            // Cambio de lenguaje de cultura del thread actual
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            
            // Cambio de lenguaje de cultura por defecto para que cuando se creen nuevos thread se hagan con un lenguage neutral.
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            // Esta condición permite identificar si el servicio esta siendo ejecutado desde el manejador
            // de servicios de Windows o desde una consola del sistema, si esta siendo ejecutado desde el 
            // manejado de servicios, entonces se inicial el servicio directamente mediante ServiceBase
            if (!Environment.UserInteractive)
            {
                ServiceBase.Run(new EngineHost(args));
                return (int)ApplicationExitCode.Success;
            }

            byte actionCounter = 0; // Usado para contar cuantas acciones se estan ejecutando
            bool isInstallAction = false; // Usado para detectar si la acción ejecutada es instalar
            bool isUninstallAction = false; // Usado para detectar si la acción ejecutada es desinstalar
            bool isStartAction = false; // Usado para detectar si la acción ejecutada es iniciar el servicio
            bool isConsoleAction = false; // Usado para detectar si la acción ejecutada es iniciar la consola
            bool isHelpAction = false; // Usado para detectar si la acción ejecutada es mostrar la ayuda
            string basePath = null; // Usado para asignar la ruta bae

            // Por el contrario si el servicio esta siendo ejecutado desde una consola del sistema, se intenta
            // parsear los argumentos enviados y asi iniciar el servicio en modo consola
            var argumentSet = new Arguments()
             {
                { "i|install", Resources.SR.InstallArgDescription, (p, value) => { actionCounter++; isInstallAction = true; } },
                { "u|uninstall", Resources.SR.UninstallArgDescription, (p, value) => { actionCounter++; isUninstallAction = true; } },
                { "s|start", Resources.SR.StartArgDescription, (p, value) => { actionCounter++; isStartAction = true; } },
                { "c|console", Resources.SR.ConsoleArgDescription, (p, value) => { actionCounter++; isConsoleAction = true; } },
                { "p|path", Resources.SR.EntryPointPathArgDescription, (p, value) => { basePath = value; } },
                { "h|help|?", Resources.SR.HelpArgDescription, (p, value) => { actionCounter++; isHelpAction = true; } }
             };
            
            ArgumentOperations.ShowHeader();
            
            // Si hay error al parsear los argumentos o no hay ninguna acción a tomar muestra la ayuda.
            if (!argumentSet.Parse(args))
            {
                LogErrorMessage(Resources.SR.InvalidArguments);
                ArgumentOperations.ShowHelp(argumentSet);
                return (int)ApplicationExitCode.InputError;
            }

            // Si no hay acciones, muestra solo como información que no se tomarán acciones.
            if (actionCounter == 0)
            {
                LogInfoMessage(Resources.SR.NoActionTaken);
                return (int)ApplicationExitCode.InputError;
            }

            // Si hay más de una acción, error.
            if (actionCounter > 1)
            {
                LogErrorMessage(Resources.SR.OnlyOneActionAllowed);
                return (int)ApplicationExitCode.InputError;
            }

            // Si se esta instalando o ejecutando en modo consola, debe tener una ruta base
            if ((isInstallAction || isConsoleAction) && string.IsNullOrEmpty(basePath))
            {
                LogErrorMessage(Resources.SR.InvalidPathArgument);
                return (int)ApplicationExitCode.InputError;
            }

            if (isInstallAction || isConsoleAction)
            {
                // Validación del directorio
                try
                {
                    if (!ArgumentOperations.IsValidDirectory(basePath))
                    {
                        LogErrorMessage(Resources.SR.InvalidPathArgument);
                        return (int)ApplicationExitCode.InputError;
                    }

                    ArgumentOperations.BasePathArgument = basePath;
                }
                catch
                {
                    LogErrorMessage(Resources.SR.InvalidPathArgument);
                    return (int)ApplicationExitCode.InputError;
                }
            }
            
            // Si la acción es ayuda
            if (isHelpAction)
            {
                ArgumentOperations.ShowHelp(argumentSet);
                return (int)ApplicationExitCode.InputError;
            }
            
            // Si la acción es instalación
            if (isInstallAction)
            {
                ArgumentOperations.Install();
                return (int)ArgumentOperations.ExitCode;
            }

            // Si la acción es desinstalación
            if (isUninstallAction)
            {
                ArgumentOperations.Uninstall();
                return (int)ArgumentOperations.ExitCode;
            }

            // Si la acción es iniciar
            if (isStartAction)
            {
                ArgumentOperations.Start();
                return (int)ArgumentOperations.ExitCode;
            }

            // Si la acción es modo consola
            if (isConsoleAction)
            {
                ArgumentOperations.ConsoleMode(args);
                return (int)ArgumentOperations.ExitCode;
            }

            return (int)ArgumentOperations.ExitCode;
        }

        /// <summary>
        /// Display a error message in console
        /// </summary>
        /// <param name="message">Message to display</param>
        private static void LogErrorMessage(string message)
        {
            DiagnosticHelper.Logger.Error(0, message);
        }

        /// <summary>
        /// Display a info message in console
        /// </summary>
        /// <param name="message">Message to display</param>
        private static void LogInfoMessage(string message)
        {
            DiagnosticHelper.Logger.Info(0, message);
        }
    }
}
