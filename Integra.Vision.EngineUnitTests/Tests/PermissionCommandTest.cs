using Integra.Vision.Engine.Commands;
using Integra.Vision.Engine.Commands.Permission.Deny;
using Integra.Vision.Engine.Commands.Permission.Grant;
using Integra.Vision.Engine.Commands.Permission.Revoke;
using Integra.Vision.Engine.Commands.SystemViews;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integra.Vision.EngineUnitTests
{
    [TestClass]
    public class PermissionCommandTest
    {
        [TestMethod]
        public void TestGrant()
        {
            string objectType = "role", objectName = "roleTest", toType = "user", userOrRoleName = "userTest";

            string sentencia1 = string.Format("grant {0} {1} to {2} {3}", objectType, objectName, toType, userOrRoleName);

            string sentenciaVista1 = string.Format("from System.UserDefinedObjects \n where Name=\"{0}\" \n select new(Id)", objectName);
            string sentenciaVista2 = string.Format("from System.UserDefinedObjects \n where Name=\"{0}\" \n select new(Id)", userOrRoleName);
            try
            {
                GrantPermissionCommand a = new GrantPermissionCommand(sentencia1, null);
                a.Execute();

                // obtengo el identificador del objeto a asignar
                SystemQueriesCommand b = new SystemQueriesCommand(sentenciaVista1, null);
                b.Execute();

                dynamic[] result = b.QueryResult;
                var valueToEvaluate = result.GetValue(0);

                var objectId = valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate);

                // obtengo el identificador del usuario o rol a asignar
                b = new SystemQueriesCommand(sentenciaVista2, null);
                b.Execute();

                result = b.QueryResult;
                valueToEvaluate = result.GetValue(0);

                var userOrRoleId = valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate);

                string sentenciaVista3 = "";
                if (objectType.Equals("role") && toType.Equals("user"))
                {
                    sentenciaVista3 = string.Format("from System.RoleMembers \n where Id.ToString()=\"{0}\" and Uid.ToString()=\"{1}\" \n select new(Id, Uid, CreationDate)", objectId, userOrRoleId);

                    // verifico lo almacenado en System.Permissions
                    b = new SystemQueriesCommand(sentenciaVista3, null);
                    b.Execute();

                    result = b.QueryResult;
                    valueToEvaluate = result.GetValue(0);

                    Assert.AreEqual(objectId, valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                    Assert.AreEqual(userOrRoleId, valueToEvaluate.GetType().GetProperty("Uid").GetValue(valueToEvaluate));
                    Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("CreationDate").GetValue(valueToEvaluate));

                    sentenciaVista3 = string.Format("from System.Permissions \n where Sid.ToString()=\"{0}\" and Oid.ToString()=\"{1}\" \n select new(Sid, Oid, Type, CreationDate)", userOrRoleId, objectId);

                    // verifico lo almacenado en System.Permissions
                    b = new SystemQueriesCommand(sentenciaVista3, null);
                    b.Execute();

                    result = b.QueryResult;
                    valueToEvaluate = result.GetValue(0);

                    Assert.AreEqual(objectId, valueToEvaluate.GetType().GetProperty("Oid").GetValue(valueToEvaluate));
                    Assert.AreEqual(userOrRoleId, valueToEvaluate.GetType().GetProperty("Sid").GetValue(valueToEvaluate));
                    Assert.IsTrue(bool.Parse(valueToEvaluate.GetType().GetProperty("Type").GetValue(valueToEvaluate).ToString()));
                    Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("CreationDate").GetValue(valueToEvaluate));
                }
                else
                {
                    sentenciaVista3 = string.Format("from System.Permissions \n where Sid.ToString()=\"{0}\" and Oid.ToString()=\"{1}\" \n select new(Sid, Oid, Type, CreationDate)", userOrRoleId, objectId);

                    // verifico lo almacenado en System.Permissions
                    b = new SystemQueriesCommand(sentenciaVista3, null);
                    b.Execute();

                    result = b.QueryResult;
                    valueToEvaluate = result.GetValue(0);

                    Assert.AreEqual(objectId, valueToEvaluate.GetType().GetProperty("Oid").GetValue(valueToEvaluate));
                    Assert.AreEqual(userOrRoleId, valueToEvaluate.GetType().GetProperty("Sid").GetValue(valueToEvaluate));
                    Assert.IsTrue(bool.Parse(valueToEvaluate.GetType().GetProperty("Type").GetValue(valueToEvaluate).ToString()));
                    Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("CreationDate").GetValue(valueToEvaluate));
                }                
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestDeny()
        {
            string objectType = "role", objectName = "roleTest", toType = "user", userOrRoleName = "userTest";

            string sentencia1 = string.Format("deny {0} {1} to {2} {3}", objectType, objectName, toType, userOrRoleName);

            string sentenciaVista1 = string.Format("from System.UserDefinedObjects \n where Name=\"{0}\" \n select new(Id)", objectName);
            string sentenciaVista2 = string.Format("from System.UserDefinedObjects \n where Name=\"{0}\" \n select new(Id)", userOrRoleName);
            try
            {
                DenyPermissionCommand a = new DenyPermissionCommand(sentencia1, null);
                a.Execute();

                // obtengo el identificador del objeto a asignar
                SystemQueriesCommand b = new SystemQueriesCommand(sentenciaVista1, null);
                b.Execute();

                dynamic[] result = b.QueryResult;
                var valueToEvaluate = result.GetValue(0);

                var objectId = valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate);

                // obtengo el identificador del usuario o rol a asignar
                b = new SystemQueriesCommand(sentenciaVista2, null);
                b.Execute();

                result = b.QueryResult;
                valueToEvaluate = result.GetValue(0);

                var userOrRoleId = valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate);

                string sentenciaVista3 = "";
                if (objectType.Equals("role") && toType.Equals("user"))
                {
                    sentenciaVista3 = string.Format("from System.RoleMembers \n where Id.ToString()=\"{0}\" and Uid.ToString()=\"{1}\" \n select new(Id, Uid, CreationDate)", objectId, userOrRoleId);

                    // verifico lo almacenado en System.Permissions
                    b = new SystemQueriesCommand(sentenciaVista3, null);
                    b.Execute();

                    result = b.QueryResult;
                    valueToEvaluate = result.GetValue(0);

                    Assert.AreEqual(objectId, valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate));
                    Assert.AreEqual(userOrRoleId, valueToEvaluate.GetType().GetProperty("Uid").GetValue(valueToEvaluate));
                    Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("CreationDate").GetValue(valueToEvaluate));

                    sentenciaVista3 = string.Format("from System.Permissions \n where Sid.ToString()=\"{0}\" and Oid.ToString()=\"{1}\" \n select new(Sid, Oid, Type, CreationDate)", userOrRoleId, objectId);

                    // verifico lo almacenado en System.Permissions
                    b = new SystemQueriesCommand(sentenciaVista3, null);
                    b.Execute();

                    result = b.QueryResult;
                    valueToEvaluate = result.GetValue(0);

                    Assert.AreEqual(objectId, valueToEvaluate.GetType().GetProperty("Oid").GetValue(valueToEvaluate));
                    Assert.AreEqual(userOrRoleId, valueToEvaluate.GetType().GetProperty("Sid").GetValue(valueToEvaluate));
                    Assert.IsFalse(bool.Parse(valueToEvaluate.GetType().GetProperty("Type").GetValue(valueToEvaluate).ToString()));
                    Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("CreationDate").GetValue(valueToEvaluate));
                }
                else
                {
                    sentenciaVista3 = string.Format("from System.Permissions \n where Sid.ToString()=\"{0}\" and Oid.ToString()=\"{1}\" \n select new(Sid, Oid, Type, CreationDate)", userOrRoleId, objectId);

                    // verifico lo almacenado en System.Permissions
                    b = new SystemQueriesCommand(sentenciaVista3, null);
                    b.Execute();

                    result = b.QueryResult;
                    valueToEvaluate = result.GetValue(0);

                    Assert.AreEqual(objectId, valueToEvaluate.GetType().GetProperty("Oid").GetValue(valueToEvaluate));
                    Assert.AreEqual(userOrRoleId, valueToEvaluate.GetType().GetProperty("Sid").GetValue(valueToEvaluate));
                    Assert.IsFalse(bool.Parse(valueToEvaluate.GetType().GetProperty("Type").GetValue(valueToEvaluate).ToString()));
                    Assert.IsNotNull(valueToEvaluate.GetType().GetProperty("CreationDate").GetValue(valueToEvaluate));
                }                
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [TestMethod]
        public void TestRevoke()
        {
            string objectType = "role", objectName = "roleTest", toType = "user", userOrRoleName = "userTest";

            string sentencia1 = string.Format("revoke {0} {1} to {2} {3}", objectType, objectName, toType, userOrRoleName);

            string sentenciaVista1 = string.Format("from System.UserDefinedObjects \n where Name=\"{0}\" \n select new(Id)", objectName);
            string sentenciaVista2 = string.Format("from System.UserDefinedObjects \n where Name=\"{0}\" \n select new(Id)", userOrRoleName);

            try
            {
                RevokePermissionCommand a = new RevokePermissionCommand(sentencia1, null);
                a.Execute();

                // obtengo el identificador del objeto a asignar
                SystemQueriesCommand b = new SystemQueriesCommand(sentenciaVista1, null);
                b.Execute();

                dynamic[] result = b.QueryResult;
                var valueToEvaluate = result.GetValue(0);

                var objectId = valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate);

                // obtengo el identificador del usuario o rol a asignar
                b = new SystemQueriesCommand(sentenciaVista2, null);
                b.Execute();

                result = b.QueryResult;
                valueToEvaluate = result.GetValue(0);

                var userOrRoleId = valueToEvaluate.GetType().GetProperty("Id").GetValue(valueToEvaluate);

                string sentenciaVista3 = "";
                if (objectType.Equals("role") && toType.Equals("user"))
                {
                    sentenciaVista3 = string.Format("from System.RoleMembers \n where Id.ToString()=\"{0}\" and Uid.ToString()=\"{1}\" \n select new(Id, Uid, CreationDate)", objectId, userOrRoleId);

                    // verifico si fue eliminado el objeto
                    if (!this.wasRemoved(sentenciaVista3))
                    {
                        Assert.Fail("No eliminó el objeto");
                    }

                    sentenciaVista3 = string.Format("from System.Permissions \n where Sid.ToString()=\"{0}\" and Oid.ToString()=\"{1}\" \n select new(Sid, Oid, Type, CreationDate)", userOrRoleId, objectId);

                    // verifico si fue eliminado el objeto
                    if (!this.wasRemoved(sentenciaVista3))
                    {
                        Assert.Fail("No eliminó el objeto");
                    }
                }
                else
                {
                    sentenciaVista3 = string.Format("from System.Permissions \n where Sid.ToString()=\"{0}\" and Oid.ToString()=\"{1}\" \n select new(Sid, Oid, Type, CreationDate)", userOrRoleId, objectId);

                    // verifico si fue eliminado el objeto
                    if (!this.wasRemoved(sentenciaVista3))
                    {
                        Assert.Fail("No eliminó el objeto");
                    }
                }                
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        /// <summary>
        /// Checks whether the object was removed
        /// </summary>
        /// <param name="sentencia">View query</param>
        /// <returns></returns>
        private bool wasRemoved(string sentencia)
        {
            // obtengo el objeto eliminado
            SystemQueriesCommand b = new SystemQueriesCommand(sentencia, null);
            b.Execute();

            dynamic[] result = b.QueryResult;
            int objectCount = result.Count<dynamic>();

            // se verifica si existe mas de un objeto
            if (objectCount != 0)
            {
                // si hay algun objeto con el nombre especificado, la prueba falla
                return false;
            }

            // si no existe algun objeto con el nombre especificado, la prueba es exitosa
            return true;
        }
    }
}
