//-----------------------------------------------------------------------
// <copyright file="ArgumentOperations.cs" company="Integra.Vision.Engine.Host">
//     Copyright (c) Integra.Vision.Engine.Host. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Host
{
    using System;
    using System.Configuration.Install;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security.Principal;
    using System.ServiceProcess;
    using Integra.Vision.Diagnostics;
    
    /// <summary>
    /// This class implements internal helper for common argument operations
    /// </summary>
    internal static class ArgumentOperations
    {
        /// <summary>
        /// Timeout for wait service start
        /// </summary>
        private static readonly TimeSpan StartServiceTimeout = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Application exit code
        /// </summary>
        private static ApplicationExitCode exitCode = ApplicationExitCode.Success;

        /// <summary>
        /// Gets the Application Exit Code
        /// </summary>
        public static ApplicationExitCode ExitCode
        {
            get
            {
                return exitCode;
            }
            
            private set
            {
                exitCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the Base path argument
        /// </summary>
        public static string BasePathArgument
        {
            get;
            set;
        }

        /// <summary>
        /// Execute the install service routine
        /// </summary>
        public static void Install()
        {
            // Check for admin rights
            if (false == IsAuthorized())
            {
                Integra.Vision.Engine.Host.Diagnostics.DiagnosticHelper.Logger.Error((int)ApplicationExitCode.SecurityError, Resources.SR.InsufficientAccessPermission);
                ExitCode = ApplicationExitCode.SecurityError;
                return;
            }

            if (IsServiceInstalled())
            {
                Integra.Vision.Engine.Host.Diagnostics.DiagnosticHelper.Logger.Info(0, Resources.SR.AlreadyInstalledService);
                return;
            }

            try
            {
                Integra.Vision.Engine.Host.Diagnostics.DiagnosticHelper.Logger.Info(0, Resources.SR.InstallingServiceMessage);
                ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                Integra.Vision.Engine.Host.Diagnostics.DiagnosticHelper.Logger.Info(0, Resources.SR.InstalledServiceMessage);
            }
            catch (Exception e)
            {
                Integra.Vision.Engine.Host.Diagnostics.DiagnosticHelper.Logger.Error((int)ApplicationExitCode.RuntimeError, e.Message);
                ExitCode = ApplicationExitCode.RuntimeError;
            }
        }

        /// <summary>
        /// Execute the uninstall service routine
        /// </summary>
        public static void Uninstall()
        {
            // Check for admin rights
            if (false == IsAuthorized())
            {
                Integra.Vision.Engine.Host.Diagnostics.DiagnosticHelper.Logger.Error((int)ApplicationExitCode.SecurityError, Resources.SR.InsufficientAccessPermission);
                ExitCode = ApplicationExitCode.SecurityError;
                return;
            }

            if (false == IsServiceInstalled())
            {
                Integra.Vision.Engine.Host.Diagnostics.DiagnosticHelper.Logger.Error((int)ApplicationExitCode.InputError, Resources.SR.NotInstalledService);
                ExitCode = ApplicationExitCode.InputError;
                return;
            }

            try
            {
                Integra.Vision.Engine.Host.Diagnostics.DiagnosticHelper.Logger.Info(0, Resources.SR.UninstallingServiceMessage);
                ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                Integra.Vision.Engine.Host.Diagnostics.DiagnosticHelper.Logger.Info(0, Resources.SR.UninstalledServiceMessage);
            }
            catch (Exception e)
            {
                Integra.Vision.Engine.Host.Diagnostics.DiagnosticHelper.Logger.Error((int)ApplicationExitCode.RuntimeError, e.Message);
                ExitCode = ApplicationExitCode.RuntimeError;
            }
        }

        /// <summary>
        /// Execute the start service routine and if the service is not installed, execute the install service routine.
        /// </summary>
        public static void Start()
        {
            if (false == IsServiceInstalled())
            {
                ExitCode = ApplicationExitCode.RuntimeError;
                return;
                
                // Install();
                // if (ExitCode != ApplicationExitCode.Success)
                // {
                //    return;
                // }
            }

            ServiceController controller = GetController();

            switch (controller.Status)
            {
                case ServiceControllerStatus.Running:
                    Integra.Vision.Engine.Host.Diagnostics.DiagnosticHelper.Logger.Info(0, Resources.SR.ServiceAlreadyStarted);
                    return;
                case ServiceControllerStatus.Stopped:
                    try
                    {
                        controller.Start();
                        controller.WaitForStatus(ServiceControllerStatus.Running, StartServiceTimeout);
                        Integra.Vision.Engine.Host.Diagnostics.DiagnosticHelper.Logger.Info(0, Resources.SR.ServiceStarted);
                        return;
                    }
                    catch (System.ServiceProcess.TimeoutException)
                    {
                        Integra.Vision.Engine.Host.Diagnostics.DiagnosticHelper.Logger.Warning((int)ApplicationExitCode.RuntimeError, Resources.SR.ServiceNotStartedTimeout);
                        break;
                    }
                    catch (Exception e)
                    {
                        Integra.Vision.Engine.Host.Diagnostics.DiagnosticHelper.Logger.Error((int)ApplicationExitCode.RuntimeError, e, string.Empty);
                        break;
                    }

                default:
                    Integra.Vision.Engine.Host.Diagnostics.DiagnosticHelper.Logger.Error((int)ApplicationExitCode.RuntimeError, Resources.SR.ServiceNotStarted(controller.Status));
                    break;
            }

            ExitCode = ApplicationExitCode.RuntimeError;
        }

        /// <summary>
        /// Execute the Console Mode routine
        /// </summary>
        /// <param name="args">Context execution arguments.</param>
        public static void ConsoleMode(string[] args)
        {
            try
            {
                System.Console.WindowWidth = (System.Console.LargestWindowWidth / 2) + 15;
                System.Console.SetBufferSize(System.Console.BufferWidth, 9999);
            }
            catch
            {
            }

            try
            {
                using (EngineHost service = new EngineHost(args))
                {
                    service.Start();
                    Console.WriteLine(Resources.SR.StopServiceMessage);
                    Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(Resources.SR.StopServiceMessage);
                Integra.Vision.Engine.Host.Diagnostics.DiagnosticHelper.Logger.Error(0, e, string.Empty);
                ExitCode = ApplicationExitCode.RuntimeError;
            }
            finally
            {
                Console.ReadLine();
            }
        }
        
        /// <summary>
        /// Show the arguments available in the console
        /// </summary>
        /// <param name="args">The arguments available</param>
        public static void ShowHelp(Arguments args)
        {
            foreach (var arg in args)
            {
                Console.WriteLine(arg.Description);
            }
        }
        
        /// <summary>
        /// Show the product information in the console
        /// </summary>
        public static void ShowHeader()
        {
            Console.WriteLine(Assembly.GetExecutingAssembly().GetProductString());
        }

        /// <summary>
        /// Check if the path is a valid directory
        /// </summary>
        /// <param name="path">The path to validate.</param>
        /// <returns>true if the path is a valid directory; otherwise false.</returns>
        public static bool IsValidDirectory(string path)
        {
            bool isValid = false;

            try
            {
                FileAttributes attributes = File.GetAttributes(path);
                if ((attributes & FileAttributes.Directory) != FileAttributes.Directory)
                {
                    isValid = false;
                }

                isValid = true;
            }
            catch
            {
            }

            return isValid;
        }

        /// <summary>
        /// Check if the file exists
        /// </summary>
        /// <param name="path">The path to validate.</param>
        /// <returns>true if the path is a valid directory; otherwise false.</returns>
        public static bool IsValidFile(string path)
        {
            bool isValid = false;

            try
            {
                isValid = File.Exists(path);
            }
            catch
            {
            }

            return isValid;
        }

        /// <summary>
        /// Check is the service is installed
        /// </summary>
        /// <returns>true if the service is installed; otherwise, false.</returns>
        private static bool IsServiceInstalled()
        {
            return GetController() != null;
        }
        
        /// <summary>
        /// Find and get the service controller associated to the service.
        /// </summary>
        /// <returns>If the service exists return the service controller associated; otherwise, null.</returns>
        private static ServiceController GetController()
        {
            return ServiceController.GetServices().FirstOrDefault(s => s.ServiceName.Equals(Resources.SR.ServiceName, StringComparison.OrdinalIgnoreCase));
        }
        
        /// <summary>
        /// Checks if the current identity or user is authorized to execute an administrator routine.
        /// </summary>
        /// <returns>true if the identity or user is authorized; otherwise, false.</returns>
        private static bool IsAuthorized()
        {
            var user = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(user);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
