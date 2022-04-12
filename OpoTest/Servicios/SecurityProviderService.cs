using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.ExpressApp.Xpo;

namespace OpoTest.Services
{
    public class SecurityProviderService : IDisposable
    {
        public SecurityStrategyComplex Security { get; private set; }
        public IObjectSpaceProvider ObjectSpaceProvider { get; private set; }
        XpoDataStoreProviderService xpoDataStoreProviderService;
        IHttpContextAccessor contextAccessor;
        public SecurityProviderService(SecurityStrategyComplex security, XpoDataStoreProviderService xpoDataStoreProviderService, IHttpContextAccessor contextAccessor)
        {
            Security = security;
            this.xpoDataStoreProviderService = xpoDataStoreProviderService;
            this.contextAccessor = contextAccessor;
            if (contextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true)
            {
                Initialize();
            }
        }
        public void Initialize()
        {
            ((AuthenticationMixed)Security.Authentication).SetupAuthenticationProvider(typeof(IdentityAuthenticationProvider).Name, contextAccessor.HttpContext.User.Identity);
            ObjectSpaceProvider = GetObjectSpaceProvider(Security);
            Login(Security, ObjectSpaceProvider);
        }
        private void SignIn(HttpContext httpContext, string userName)
        {
            List<Claim> claims = new List<Claim>{
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            ClaimsPrincipal principal = new ClaimsPrincipal(id);
            httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
        public bool InitConnection(string userName, string password)
        {
            AuthenticationStandardLogonParameters parameters = new AuthenticationStandardLogonParameters(userName, password);
            Security.Logoff();
            ((AuthenticationMixed)Security.Authentication).SetupAuthenticationProvider(typeof(AuthenticationStandardProvider).Name, parameters);
            IObjectSpaceProvider objectSpaceProvider = GetObjectSpaceProvider(Security);
            try
            {
                Login(Security, objectSpaceProvider);
                SignIn(contextAccessor.HttpContext, userName);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private IObjectSpaceProvider GetObjectSpaceProvider(SecurityStrategyComplex security)
        {
            SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, xpoDataStoreProviderService.GetDataStoreProvider(), true);
            RegisterEntities(objectSpaceProvider);
            return objectSpaceProvider;
        }
        private void Login(SecurityStrategyComplex security, IObjectSpaceProvider objectSpaceProvider)
        {
            IObjectSpace objectSpace = ((INonsecuredObjectSpaceProvider)objectSpaceProvider).CreateNonsecuredObjectSpace();
            security.Logon(objectSpace);
        }
        public static void RegisterEntities(IObjectSpaceProvider objectSpaceProvider)
        {
            objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyUser));
            objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyRole));
            objectSpaceProvider.TypesInfo.RegisterEntity(typeof(Examen));
            objectSpaceProvider.TypesInfo.RegisterEntity(typeof(ExamenPregunta));
            objectSpaceProvider.TypesInfo.RegisterEntity(typeof(ExamenRespuesta));
            objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PlantillaPregunta));
            objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PlantillaRespuesta));
            objectSpaceProvider.TypesInfo.RegisterEntity(typeof(Tema));
        }
        public void Dispose()
        {
            Security?.Dispose();
            ((SecuredObjectSpaceProvider)ObjectSpaceProvider)?.Dispose();
        }

        public XPObjectSpace GetObjectspace() => ObjectSpaceProvider.CreateObjectSpace() as XPObjectSpace;
    }
}
