//-----------------------------------------------------------------------
// <copyright file="CreateStreamCommand.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Commands.Create.CreateStream
{
    using System;
    using Integra.Vision.Engine.Database.Repositories;

    /// <summary>
    /// Base class for create streams
    /// </summary>
    internal class CreateStreamCommand : CreateObjectCommandBase
    {
        /// <summary>
        /// Argument enumerator implementation for this command
        /// </summary>
        private IArgumentEnumerator argumentEnumerator = new CreateStreamArgumentEnumerator();

        /// <summary>
        /// Dependency enumerator implementation for this command
        /// </summary>
        private IDependencyEnumerator dependencyEnumerator = new CreateStreamDependencyEnumerator();

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateStreamCommand"/> class
        /// </summary>
        /// <param name="commandText">Text that must be interpreted as part of this command</param>
        /// <param name="securityContext">Context for security validation</param>
        public CreateStreamCommand(string commandText, ISecurityContext securityContext) : base(CommandTypeEnum.CreateStream, commandText, securityContext)
        {
        }

        /// <summary>
        /// Gets command dependency enumerator
        /// </summary>
        protected override IDependencyEnumerator DependencyEnumerator
        {
            get
            {
                return this.dependencyEnumerator;
            }
        }

        /// <summary>
        /// Gets command argument enumerator
        /// </summary>
        protected override IArgumentEnumerator ArgumentEnumerator
        {
            get
            {
                return this.argumentEnumerator;
            }
        }

        /// <summary>
        /// Save stream arguments
        /// </summary>
        public virtual void SaveArguments()
        {
            // Initialize the context
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            // initialize the repositories
            Database.Repositories.Repository<Database.Models.Stream> repoStream = new Database.Repositories.Repository<Database.Models.Stream>(vc);
            Database.Repositories.Repository<Database.Models.PList> repoPList = new Database.Repositories.Repository<Database.Models.PList>(vc);
            Database.Repositories.Repository<Database.Models.StreamCondition> repoStreamCondition = new Database.Repositories.Repository<Database.Models.StreamCondition>(vc);
            Database.Repositories.Repository<Database.Models.SourceAssignedToStream> repoAsig = new Database.Repositories.Repository<Database.Models.SourceAssignedToStream>(vc);
            Database.Repositories.Repository<Database.Models.Source> repoSource = new Database.Repositories.Repository<Database.Models.Source>(vc);

            Database.Models.Stream stream = null;
            int argumentsCount = this.Arguments.Count;

            if (argumentsCount == 4)
            {
                stream = new Database.Models.Stream() { Name = this.Arguments["Name"].Value.ToString(), Type = ObjectTypeEnum.Stream.ToString(), IsSystemObject = false, State = (int)UserDefinedObjectStateEnum.Stopped, CreationDate = DateTime.Now, UseJoin = false };
                repoStream.Create(stream);
                repoStream.Commit();

                string sourceName = this.Arguments["From"].Value.ToString();
                Database.Models.Source source = repoSource.Find(x => x.Name == sourceName);
                Database.Models.SourceAssignedToStream asig = new Database.Models.SourceAssignedToStream() { Alias = this.Arguments["From"].Value.ToString(), IsWithSource = false, StreamId = stream.Id, SourceId = source.Id };
                repoAsig.Create(asig);
                repoAsig.Commit();

                Database.Models.StreamCondition streamCondition = new Database.Models.StreamCondition() { StreamId = stream.Id, Expression = this.Arguments["Where"].Value.ToString(), IsOnCondition = false, Type = (int)ConditionTypeEnum.FilterCondition };
                repoStreamCondition.Create(streamCondition);
                repoStreamCondition.Commit();
            }
            else if (argumentsCount == 9)
            {
                TimeSpan d;
                TimeSpan.TryParse(this.Arguments["ApplyWindow"].Value.ToString(), out d);

                stream = new Database.Models.Stream() { Name = this.Arguments["Name"].Value.ToString(), Type = ObjectTypeEnum.Stream.ToString(), IsSystemObject = false, State = (int)UserDefinedObjectStateEnum.Stopped, CreationDate = DateTime.Now, UseJoin = true, DurationTime = d.TotalMilliseconds };
                repoStream.Create(stream);
                repoStream.Commit();

                Database.Models.StreamCondition streamOnCondition = new Database.Models.StreamCondition() { StreamId = stream.Id, Expression = this.Arguments["On"].Value.ToString(), IsOnCondition = true, Type = (int)ConditionTypeEnum.CombinationCondition };
                repoStreamCondition.Create(streamOnCondition);
                repoStreamCondition.Commit();

                string sourceName = this.Arguments["JoinSourceName"].Value.ToString();
                Database.Models.Source source = repoSource.Find(x => x.Name == sourceName);
                Database.Models.SourceAssignedToStream asig = new Database.Models.SourceAssignedToStream() { Alias = this.Arguments["JoinSourceAlias"].Value.ToString(), IsWithSource = false, StreamId = stream.Id, SourceId = source.Id };
                repoAsig.Create(asig);
                repoAsig.Commit();

                sourceName = this.Arguments["WithSourceName"].Value.ToString();
                source = repoSource.Find(x => x.Name == sourceName);
                asig = new Database.Models.SourceAssignedToStream() { Alias = this.Arguments["WithSourceAlias"].Value.ToString(), IsWithSource = true, StreamId = stream.Id, SourceId = source.Id };
                repoAsig.Create(asig);
                repoAsig.Commit();

                Database.Models.StreamCondition streamCondition = new Database.Models.StreamCondition() { StreamId = stream.Id, Expression = this.Arguments["Where"].Value.ToString(), IsOnCondition = false, Type = (int)ConditionTypeEnum.MergeResultCondition };
                repoStreamCondition.Create(streamCondition);
                repoStreamCondition.Commit();
            }

            int position = 0;
            System.Collections.Generic.List<System.Tuple<string, string>> projectionList = (System.Collections.Generic.List<System.Tuple<string, string>>)this.Arguments["Select"].Value;
            foreach (var a in projectionList)
            {
                Database.Models.PList projectionTuple = new Database.Models.PList() { Alias = a.Item1, StreamId = stream.Id, Expression = a.Item2, Order = position };
                repoPList.Create(projectionTuple);
                repoPList.Commit();
                position++;
            }

            // close connections
            repoStream.Dispose();
            repoPList.Dispose();
            repoStreamCondition.Dispose();
            repoAsig.Dispose();
            vc.Dispose();

            // save dependencies of the stream
            this.SaveDependencies(stream);
        }

        /// <summary>
        /// Contains create stream logic
        /// </summary>
        protected override void OnExecute()
        {
            base.OnExecute();

            // save arguments
            this.SaveArguments();
        }

        /// <summary>
        /// save the dependencies of the actual object
        /// </summary>
        /// <param name="stream">the actual adapter</param>
        private void SaveDependencies(Database.Models.Stream stream)
        {
            Integra.Vision.Engine.Database.Contexts.ViewsContext vc = new Integra.Vision.Engine.Database.Contexts.ViewsContext("EngineDatabase");

            Repository<Database.Models.Dependency> repoDependency = new Repository<Database.Models.Dependency>(vc);
            Repository<Database.Models.Source> repoSource = new Repository<Database.Models.Source>(vc);

            foreach (var streamDependency in this.Dependencies)
            {
                Database.Models.Source source = repoSource.Find(x => x.Name == streamDependency.Name);

                if (source == null)
                {
                    throw new Integra.Vision.Engine.Exceptions.NonExistentObjectException("The source '" + streamDependency.Name + "' does not exist");
                }

                Database.Models.Dependency dependency = new Database.Models.Dependency() { DependencyObjectId = source.Id, PrincipalObjectId = stream.Id };
                repoDependency.Create(dependency);
                repoDependency.Commit();
            }

            repoDependency.Dispose();
            repoSource.Dispose();
            vc.Dispose();
        }
    }
}
