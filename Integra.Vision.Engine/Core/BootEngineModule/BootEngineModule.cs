//-----------------------------------------------------------------------
// <copyright file="BootEngineModule.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Linq;
    using System.Threading;
    using Integra.Vision.Engine.Commands;
    using Integra.Vision.Engine.Database.Contexts;

    /// <summary>
    /// This class allow to implement a queue module for scheduling a request.
    /// </summary>
    internal sealed class BootEngineModule : Module, IBootEngineModule
    {
        /// <summary>
        /// The queue where the requests are enqueued for future processing.
        /// </summary>
        private IOperationSchedulerModule scheduler;

        /// <summary>
        /// Initializes a new instance of the <see cref="BootEngineModule"/> class.
        /// </summary>
        /// <param name="scheduler">The command queue where the requests are enqueued.</param>
        public BootEngineModule(IOperationSchedulerModule scheduler)
        {
            this.scheduler = scheduler;
        }

        /// <inheritdoc />
        public void BootEngine()
        {
            OperationContext bootContext = new OperationContextWrapper(Thread.CurrentPrincipal, new OperationRequest("boot engine", string.Empty));
            bootContext.Data.Add("IsPrivateCommand", true);
            this.scheduler.Schedule(bootContext); // Se agenda la ejecución del contexto.
            bootContext.WaitForCompletion().Wait(); // Se espera por la terminación de la operación.            

            BootCommandResult bootResult = bootContext.Response.Results[0] as BootCommandResult;

            // se corren los scripts en el siguiente orden: assemblies->adapters->sources->streams->triggers
            this.RunScripts(bootResult.Scripts.Where(x => x.Item2 == ObjectTypeEnum.Assembly).ToArray());
            this.RunScripts(bootResult.Scripts.Where(x => x.Item2 == ObjectTypeEnum.Adapter).ToArray());
            this.RunScripts(bootResult.Scripts.Where(x => x.Item2 == ObjectTypeEnum.Source).ToArray());
            this.RunScripts(bootResult.Scripts.Where(x => x.Item2 == ObjectTypeEnum.Stream).ToArray());
            this.RunScripts(bootResult.Scripts.Where(x => x.Item2 == ObjectTypeEnum.Trigger).ToArray());
        }

        /// <summary>
        /// Abort the module.
        /// </summary>
        protected override void OnAbort()
        {
        }

        /// <summary>
        /// Stop the module.
        /// </summary>
        protected override void OnStop()
        {
        }

        /// <summary>
        /// Start the module.
        /// </summary>
        protected override void OnStart()
        {
            using (ViewsContext context = new ViewsContext())
            {
                context.Database.Initialize(true);
                using (SystemViewsContext systemcontext = new SystemViewsContext())
                {
                    systemcontext.Database.Initialize(true);
                }
            }

            this.BootEngine();
        }

        /// <summary>
        /// Runs the scripts to boot the engine objects: assemblies, adapters, sources, streams and triggers
        /// </summary>
        /// <param name="objects">Objects to boot</param>
        private void RunScripts(Tuple<string, ObjectTypeEnum>[] objects)
        {
            // si no existen objetos no se hace nada
            if (objects.Count<Tuple<string, ObjectTypeEnum>>() == 0)
            {
                return;
            }

            // script de comandos
            string script = string.Empty;

            // indica si el objeto es un assembly o no
            bool isPrivateCommand = false;

            // se crea el script a ejecutar
            foreach (Tuple<string, ObjectTypeEnum> tuple in objects)
            {
                if (tuple.Item2.Equals(ObjectTypeEnum.Assembly))
                {
                    script += "load assembly " + tuple.Item1 + " ";
                    isPrivateCommand = true;
                }
                else
                {
                    script += "start " + tuple.Item2.ToString() + " " + tuple.Item1 + " ";
                }
            }

            // se crea el contexto
            OperationContext context = new OperationContextWrapper(Thread.CurrentPrincipal, new OperationRequest(script, string.Empty));

            // se establece si el comando es publico o privado, load assembly es un comando privado
            context.Data.Add("IsPrivateCommand", isPrivateCommand);

            // se calendariza la ejecución del comando
            this.scheduler.Schedule(context);
            context.WaitForCompletion().Wait();

            if (context.Response == null)
            {
                throw new BootException(Resources.SR.BootResponseNull);
            }

            if (context.Response.Results.Count == 0)
            {
                throw new BootException(Resources.SR.BootResponseEmpty);
            }

            if (context.Response.Results[0] is ErrorCommandResult)
            {
                throw new BootException((context.Response.Results[0] as ErrorCommandResult).Message);
            }

            if (!(context.Response.Results[0] is OkCommandResult))
            {
                throw new BootException(Resources.SR.BootResponseInvalid(context.Response.Results[0].GetType().Name));
            }
        }
    }
}
