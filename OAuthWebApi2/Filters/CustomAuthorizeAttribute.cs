using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Security.Claims;
using System.Web.Http;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;
using Microsoft.Owin.Security;

namespace OAuthWebApi2.Filters
{
    //public class AccessTokenProvider : OAuthAuthorizationServerProvider
    //{
    //    public override async Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
    //    {
    //        // Reject token: context.Rejected(); Or:

    //        // chance to change authentication ticket for refresh token requests
    //        var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
    //        var appUser = await userManager.FindByNameAsync(context.Ticket.Identity.Name);
    //        var oAuthIdentity = await appUser.GenerateUserIdentityAsync(userManager);
    //        var newTicket = new AuthenticationTicket(oAuthIdentity, context.Ticket.Properties);

    //        context.Validated(newTicket);
    //    }

    //    public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
    //    {
    //        var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
    //        var appUser = await userManager.FindAsync(context.UserName, context.Password);
    //        if (appUser == null)
    //        {
    //            context.SetError("invalid_grant", "The user name or password is incorrect.");
    //            return;
    //        }

    //        var propertyDictionary = new Dictionary<string, string> { { "userName", appUser.UserName } };
    //        var properties = new AuthenticationProperties(propertyDictionary);

    //        var oAuthIdentity = await appUser.GenerateUserIdentityAsync(userManager);
    //        var ticket = new AuthenticationTicket(oAuthIdentity, properties);

    //        // Token is validated.
    //        context.Validated(ticket);
    //    }
    //}

    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        //public override void OnAuthorization(HttpActionContext actionContext)
        //{
        //    var user = actionContext.ControllerContext.User as ClaimsPrincipal;
        //}

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            //pass refresh token in header and then 401, pass refresh token and try if yes, call granttype-refresh_token, (https://localhost:44319/token)
            var principal = filterContext.RequestContext.Principal as ClaimsPrincipal;

            try
            {
                string refreshtoken = "";
                if (!string.IsNullOrEmpty(principal.Identity.Name))
                {
                }
                //else
                //{
                //    IEnumerable<string> headerValues = filterContext.Request.Headers.GetValues("refresh_token");
                //    refreshtoken = headerValues.FirstOrDefault();
                //    setRefreshToken(filterContext, refreshtoken);
                //}
            }
            catch (Exception ex)
            {
                filterContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                return;
            }

            base.OnAuthorization(filterContext);
        }

        public void setRefreshToken(HttpActionContext filterContext, string refreshtoken)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44319/token");
            webRequest.Method = "POST";
            webRequest.AllowAutoRedirect = true;
            webRequest.Timeout = 20 * 1000;
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.99 Safari/537.36";


            //write the data to post request
            //Postdata : a=1&b=2&c=3
            string data = $"refresh_token={refreshtoken}&grant_type=refresh_token";
            byte[] buffer = Encoding.Default.GetBytes(data);
            if (buffer != null)
            {
                webRequest.ContentLength = buffer.Length;
                webRequest.GetRequestStream().Write(buffer, 0, buffer.Length);
            }

            WebResponse wr = webRequest.GetResponse();

            var dataStream = wr.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            var result = JsonConvert.DeserializeObject<Token>(responseFromServer);

            filterContext.Request.Headers.Clear();
            filterContext.Request.Headers.Add("Authorization", "Bearer " + result.access_token);
            this.OnAuthorization(filterContext);
        }
    }

    public class Token
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
    }
}