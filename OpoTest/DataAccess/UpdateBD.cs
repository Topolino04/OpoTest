using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Microsoft.AspNetCore.Builder;
using OpoTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpoTest.DataAccess
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UpdateBD(this IApplicationBuilder app, string connectionString)
        {
            using (var objectSpaceProvider = new XPObjectSpaceProvider(connectionString))
            {
                SecurityProviderService.RegisterEntities(objectSpaceProvider);
                using var objectSpace = objectSpaceProvider.CreateUpdatingObjectSpace(true);
                (objectSpace as XPObjectSpace).Session.UpdateSchema();
                new Updater(objectSpace).UpdateDatabase();
            }
            return app;
        }

        public class Updater
        {
            private const string AdministratorUserName = "Admin";
            private const string AdministratorRoleName = "Administrators";


            private IObjectSpace ObjectSpace { get; }

            public Updater(IObjectSpace objectSpace) { ObjectSpace = objectSpace; }

            public void UpdateDatabase()
            {
                CreateAdmin();
                CreateTipos();
            }
            private void CreateTipos()
            {
                if (ObjectSpace.GetObjectsCount(typeof(Tema), null) == 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        var tipo = ObjectSpace.CreateObject<Tema>();
                        tipo.Nombre = $"Tipo {i}";
                    }
                    ObjectSpace.CommitChanges();
                }
            }
            //private void CreateUser()
            //{
            //    PermissionPolicyUser defaultUser = ObjectSpace.FirstOrDefault<PermissionPolicyUser>(
            //        u => u.UserName == DefaultUserName);
            //    if (defaultUser == null)
            //    {
            //        defaultUser = ObjectSpace.CreateObject<PermissionPolicyUser>();
            //        defaultUser.UserName = DefaultUserName;
            //        defaultUser.SetPassword("");
            //        defaultUser.Roles.Add(GetUserRole());
            //        ObjectSpace.CommitChanges();
            //    }
            //}

            private void CreateAdmin()
            {
                PermissionPolicyUser adminUser = ObjectSpace.FirstOrDefault<PermissionPolicyUser>(
                    u => u.UserName == AdministratorUserName);
                if (adminUser == null)
                {
                    adminUser = ObjectSpace.CreateObject<PermissionPolicyUser>();
                    adminUser.UserName = AdministratorUserName;
                    adminUser.SetPassword("");
                    adminUser.Roles.Add(GetAdminRole());
                    ObjectSpace.CommitChanges();
                }
            }

            private PermissionPolicyRole GetAdminRole()
            {
                PermissionPolicyRole adminRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(
                    u => u.Name == AdministratorRoleName);
                if (adminRole == null)
                {
                    adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                    adminRole.Name = AdministratorRoleName;
                    adminRole.IsAdministrative = true;
                }
                return adminRole;
            }
        }
    }
}
