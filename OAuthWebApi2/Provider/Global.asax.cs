//using Microsoft.Owin;
//using Microsoft.Owin.Cors;
//using Microsoft.Owin.Security.OAuth;
//using Owin;
//using System;
//using System.Web.Http;
//using System.Web.Mvc;
//using System.Web.Optimization;
//using System.Web.Routing;
//using System.Threading.Tasks;
//using System.Security.Claims;
//using OAuthWebApi2.Models;
//using System.Linq;

//namespace OAuthWebApi2
//{
//    public class WebApiApplication : System.Web.HttpApplication
//    {
//        protected void Application_Start()
//        {
//            AreaRegistration.RegisterAllAreas();
//            GlobalConfiguration.Configure(WebApiConfig.Register);
//            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
//            RouteConfig.RegisterRoutes(RouteTable.Routes);
//            BundleConfig.RegisterBundles(BundleTable.Bundles);
//            Configuration(new AppBuilderUseExtensions());
//        }


//        public void Configuration(IAppBuilder app)
//        {
//            ConfigureAuth(app);

//            GlobalConfiguration.Configure(WebApiConfig.Register);
//            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
//            RouteConfig.RegisterRoutes(RouteTable.Routes);
//            BundleConfig.RegisterBundles(BundleTable.Bundles);
//        }


//        public void ConfigureAuth(IAppBuilder app)
//        {
//            //this is very important line cross orgin source(CORS)it is used to enable cross-site HTTP requests
//            //For security reasons, browsers restrict cross-origin HTTP requests
//            app.UseCors(CorsOptions.AllowAll);

//            var OAuthOptions = new OAuthAuthorizationServerOptions
//            {
//                AllowInsecureHttp = true,
//                TokenEndpointPath = new PathString("/token"),
//                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),//token expiration time
//                Provider = new OauthProvider()
//            };

//            //app.UseOAuthBearerTokens(OAuthOptions);
//            app.UseOAuthAuthorizationServer(OAuthOptions);
//            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

//            HttpConfiguration config = new HttpConfiguration();
//            WebApiConfig.Register(config);//register the request
//        }
//    }

//    public class OauthProvider : OAuthAuthorizationServerProvider
//    {
//        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
//        {
//            await Task.Run(() => context.Validated());
//        }

//        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
//        {
//            //var identity = new ClaimsIdentity(context.Options.AuthenticationType);

//            using (var db = new OauthApiEntities())
//            {
//                if (db != null)
//                {
//                    var user = db.ApiUsers.Where(o => o.UserName == context.UserName && o.UserPasswd == context.Password).FirstOrDefault();
//                    if (user != null)
//                    {
//                        var identity = new ClaimsIdentity(new[] {
//                                new Claim(ClaimTypes.Name, user.UserName),
//                                new Claim("LoggedOn", DateTime.Now.ToString())
//                                    }, context.Options.AuthenticationType);
//                        foreach (var item in user.User_Roles)
//                        {
//                            identity.AddClaim(new Claim(ClaimTypes.Role, item.Role.name));
//                        }
//                        await Task.Run(() => context.Validated(identity));
//                    }
//                    else
//                    {
//                        context.SetError("Wrong Crendtials", "Provided username and password is incorrect");
//                    }
//                }
//                else
//                {
//                    context.SetError("Wrong Crendtials", "Provided username and password is incorrect");
//                }
//                return;
//            }
//        }

//    }

//}

