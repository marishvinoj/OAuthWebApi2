using Microsoft.Owin.Security.OAuth;
using OAuthWebApi2.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Repository;

namespace OauthApp.Provider
{
    public class OauthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            await Task.Run(() => context.Validated());
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (var db = new OauthApiEntities())
            {
                if (db != null)
                {
                    var user = db.ApiUsers.Where(o => o.UserName == context.UserName && o.UserPasswd == context.Password).FirstOrDefault();
                    if (user != null)
                    {
                       var identity = new ClaimsIdentity(new[] {
                                new Claim(ClaimTypes.Name, user.UserName),
                                new Claim("LoggedOn", DateTime.Now.ToString())
                                    }, context.Options.AuthenticationType);
                        foreach (var item in user.User_Roles)
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, item.Role.name));
                        }
                        await Task.Run(() => context.Validated(identity));
                    }
                    else
                    {
                        context.SetError("Wrong Crendtials", "Provided username and password is incorrect");
                    }
                }
                else
                {
                    context.SetError("Wrong Crendtials", "Provided username and password is incorrect");
                }
                return;
            }
        }

    }
}