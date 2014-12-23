using Integra.Vision.Engine.Database.Contexts;
using Integra.Vision.Engine.Database.Models.SystemViews;
using Integra.Vision.Engine.Database.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Linq.Dynamic;
using Microsoft.CSharp.RuntimeBinder;

namespace Integra.Vision.Engine.DatabaseUnitTests
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void TestViewAssemblies()
        {
            try
            {
                SystemViewsContext vc = new SystemViewsContext("EngineDatabase");
                RepositoryForViews<SystemAssembly> repoSystemViews = new RepositoryForViews<SystemAssembly>(vc);
                IQueryable result = null;//repoSystemViews.Select("Name=\"assemblyTest\"", "Id");

               /* var iqd = repoSystemViews.Set.Where("Name=\"HOLA\"").Select("new (Id, Name, State, LocalPath)") as IQueryable<dynamic>;
                var x = iqd.ToArray();

                /*
                var resultCasted = result.Cast<System.Guid>();
                int resultCount = resultCasted.Count<System.Guid>();
                repoSystemViews.Dispose();

                if (resultCount != 1)
                {
                    Assert.Fail("La vista no devuelve la cantidad de datos esperada");
                }
                 * */
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestViewAdapters()
        {
            try
            {
                SystemViewsContext vc = new SystemViewsContext("EngineDatabase");
                RepositoryForViews<SystemAdapter> repoSystemViews = new RepositoryForViews<SystemAdapter>(vc);
                IQueryable result = repoSystemViews.Query("Name=\"adapterTest\"", "Id");
                var resultCasted = result.Cast<System.Guid>();
                int resultCount = resultCasted.Count<System.Guid>();
                repoSystemViews.Dispose();

                if (resultCount != 1)
                {
                    Assert.Fail("La vista no devuelve la cantidad de datos esperada");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestViewArgs()
        {
            try
            {
                System.Guid adapterId = new Guid("5C491A1E-377A-E411-B31D-005056C00008");

                SystemViewsContext vc = new SystemViewsContext("EngineDatabase");
                RepositoryForViews<SystemArg> repoSystemViews = new RepositoryForViews<SystemArg>(vc);
                IQueryable result = repoSystemViews.Query("Id=@0", "Name", adapterId);
                var resultCasted = result.Cast<string>();
                int resultCount = resultCasted.Count<string>();
                repoSystemViews.Dispose();

                if (resultCount < 1)
                {
                    Assert.Fail("La vista no devuelve la cantidad de datos esperada");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestViewConditions()
        {
            try
            {
                System.Guid objectId = new Guid("37E17CF5-397A-E411-B31D-005056C00008");

                SystemViewsContext vc = new SystemViewsContext("EngineDatabase");
                RepositoryForViews<SystemCondition> repoSystemViews = new RepositoryForViews<SystemCondition>(vc);
                IQueryable result = repoSystemViews.Query("Id=@0", "Expression", objectId);
                var resultCasted = result.Cast<string>();
                int resultCount = resultCasted.Count<string>();
                repoSystemViews.Dispose();

                if (resultCount < 1)
                {
                    Assert.Fail("La vista no devuelve la cantidad de datos esperada");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestViewDependencies()
        {
            try
            {
                SystemViewsContext vc = new SystemViewsContext("EngineDatabase");
                RepositoryForViews<SystemDependency> repoSystemViews = new RepositoryForViews<SystemDependency>(vc);
                IQueryable result = repoSystemViews.Query("CreationDate < DateTime.Now", "DependenceId");
                var resultCasted = result.Cast<System.Guid>();
                int resultCount = resultCasted.Count<System.Guid>();
                repoSystemViews.Dispose();

                if (resultCount < 1)
                {
                    Assert.Fail("La vista no devuelve la cantidad de datos esperada");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestViewPermissions()
        {
            try
            {
                SystemViewsContext vc = new SystemViewsContext("EngineDatabase");
                RepositoryForViews<SystemPermission> repoSystemViews = new RepositoryForViews<SystemPermission>(vc);
                IQueryable result = repoSystemViews.Query("CreationDate < DateTime.Now", "Sid");
                var resultCasted = result.Cast<System.Guid>();
                int resultCount = resultCasted.Count<System.Guid>();
                repoSystemViews.Dispose();

                if (resultCount < 1)
                {
                    Assert.Fail("La vista no devuelve la cantidad de datos esperada");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestViewPList()
        {
            try
            {
                System.Guid streamId = new System.Guid("36E17CF5-397A-E411-B31D-005056C00008");

                SystemViewsContext vc = new SystemViewsContext("EngineDatabase");
                RepositoryForViews<SystemPList> repoSystemViews = new RepositoryForViews<SystemPList>(vc);
                IQueryable result = repoSystemViews.Query("Id = @0", "Alias", streamId);
                var resultCasted = result.Cast<string>();
                int resultCount = resultCasted.Count<string>();
                repoSystemViews.Dispose();

                if (resultCount < 1)
                {
                    Assert.Fail("La vista no devuelve la cantidad de datos esperada");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestViewRoleMembers()
        {
            try
            {
                SystemViewsContext vc = new SystemViewsContext("EngineDatabase");
                RepositoryForViews<SystemRoleMember> repoSystemViews = new RepositoryForViews<SystemRoleMember>(vc);
                IQueryable result = repoSystemViews.Query("CreationDate < DateTime.Now", "Id");
                var resultCasted = result.Cast<System.Guid>();
                int resultCount = resultCasted.Count<System.Guid>();
                repoSystemViews.Dispose();

                if (resultCount < 1)
                {
                    Assert.Fail("La vista no devuelve la cantidad de datos esperada");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestViewRoles()
        {
            try
            {
                SystemViewsContext vc = new SystemViewsContext("EngineDatabase");
                RepositoryForViews<SystemRole> repoSystemViews = new RepositoryForViews<SystemRole>(vc);
                IQueryable result = repoSystemViews.Query("Name=\"roleTest\"", "Id");
                var resultCasted = result.Cast<System.Guid>();
                int resultCount = resultCasted.Count<System.Guid>();
                repoSystemViews.Dispose();

                if (resultCount != 1)
                {
                    Assert.Fail("La vista no devuelve la cantidad de datos esperada");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestViewSetTrace()
        {
            try
            {
                System.Guid objectId = new System.Guid("5C491A1E-377A-E411-B31D-005056C00008");

                SystemViewsContext vc = new SystemViewsContext("EngineDatabase");
                RepositoryForViews<SystemSetTrace> repoSystemViews = new RepositoryForViews<SystemSetTrace>(vc);
                IQueryable result = repoSystemViews.Query("Id=@0", "Level", objectId);
                var resultCasted = result.Cast<int>();
                int resultCount = resultCasted.Count<int>();
                repoSystemViews.Dispose();

                if (resultCount != 1)
                {
                    Assert.Fail("La vista no devuelve la cantidad de datos esperada");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestViewSources()
        {
            try
            {
                SystemViewsContext vc = new SystemViewsContext("EngineDatabase");
                RepositoryForViews<SystemSource> repoSystemViews = new RepositoryForViews<SystemSource>(vc);
                IQueryable result = repoSystemViews.Query("Name=\"sourceTest\"", "Id");
                var resultCasted = result.Cast<System.Guid>();
                int resultCount = resultCasted.Count<System.Guid>();
                repoSystemViews.Dispose();

                if (resultCount != 1)
                {
                    Assert.Fail("La vista no devuelve la cantidad de datos esperada");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestViewSourceAssignedToStreams()
        {
            try
            {
                System.Guid streamId = new System.Guid("37E17CF5-397A-E411-B31D-005056C00008");

                SystemViewsContext vc = new SystemViewsContext("EngineDatabase");
                RepositoryForViews<SystemSourceAssignedToStream> repoSystemViews = new RepositoryForViews<SystemSourceAssignedToStream>(vc);
                IQueryable result = repoSystemViews.Query("StreamId=@0", "Alias", streamId);
                var resultCasted = result.Cast<string>();
                int resultCount = resultCasted.Count<string>();
                repoSystemViews.Dispose();

                if (resultCount < 1)
                {
                    Assert.Fail("La vista no devuelve la cantidad de datos esperada");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestViewStmts()
        {
            try
            {
                System.Guid triggerId = new System.Guid("70E1CAE0-3A7A-E411-B31D-005056C00008");

                SystemViewsContext vc = new SystemViewsContext("EngineDatabase");
                RepositoryForViews<SystemStmt> repoSystemViews = new RepositoryForViews<SystemStmt>(vc);
                IQueryable result = repoSystemViews.Query("Id=@0", "AdapterId", triggerId);
                var resultCasted = result.Cast<System.Guid>();
                int resultCount = resultCasted.Count<System.Guid>();
                repoSystemViews.Dispose();

                if (resultCount < 1)
                {
                    Assert.Fail("La vista no devuelve la cantidad de datos esperada");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestViewStreams()
        {
            try
            {
                SystemViewsContext vc = new SystemViewsContext("EngineDatabase");
                RepositoryForViews<SystemStream> repoSystemViews = new RepositoryForViews<SystemStream>(vc);
                IQueryable result = repoSystemViews.Query("Name=\"streamJoinTest\"", "Id");
                var resultCasted = result.Cast<System.Guid>();
                int resultCount = resultCasted.Count<System.Guid>();
                repoSystemViews.Dispose();

                if (resultCount != 1)
                {
                    Assert.Fail("La vista no devuelve la cantidad de datos esperada");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestViewTriggers()
        {
            try
            {
                SystemViewsContext vc = new SystemViewsContext("EngineDatabase");
                RepositoryForViews<SystemTrigger> repoSystemViews = new RepositoryForViews<SystemTrigger>(vc);
                IQueryable result = repoSystemViews.Query("Name=\"triggerTestSimple\"", "Id");
                var resultCasted = result.Cast<System.Guid>();
                int resultCount = resultCasted.Count<System.Guid>();
                repoSystemViews.Dispose();

                if (resultCount != 1)
                {
                    Assert.Fail("La vista no devuelve la cantidad de datos esperada");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestViewUserDefinedObjects()
        {
            try
            {
                SystemViewsContext vc = new SystemViewsContext("EngineDatabase");
                RepositoryForViews<SystemUserDefinedObject> repoSystemViews = new RepositoryForViews<SystemUserDefinedObject>(vc);
                IQueryable result = repoSystemViews.Query("Id=Id", "Name");
                var resultCasted = result.Cast<string>();
                int resultCount = resultCasted.Count<string>();
                repoSystemViews.Dispose();

                if (resultCount < 1)
                {
                    Assert.Fail("La vista no devuelve la cantidad de datos esperada");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestViewUsers()
        {
            try
            {
                System.Guid userId = new System.Guid("F29420F5-3A7A-E411-B31D-005056C00008");
                SystemViewsContext vc = new SystemViewsContext("EngineDatabase");
                RepositoryForViews<SystemUser> repoSystemViews = new RepositoryForViews<SystemUser>(vc);
                IQueryable result = repoSystemViews.Query("Id=@0", "Sid", userId);
                var resultCasted = result.Cast<string>();
                int resultCount = resultCasted.Count<string>();
                repoSystemViews.Dispose();

                if (resultCount != 1)
                {
                    Assert.Fail("La vista no devuelve la cantidad de datos esperada");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }
    }
}
