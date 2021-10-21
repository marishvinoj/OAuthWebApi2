using Microsoft.Owin.Security.OAuth;
using OAuthWebApi2.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Repository;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security;

namespace OauthApp.Provider
{
    public class OauthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            await Task.Run(() => context.Validated());
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
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


    public class ApplicationRefreshTokenProvider : AuthenticationTokenProvider
    {
        //public override Task CreateAsync(AuthenticationTokenCreateContext context)
        //{
        //    var form = context.Request.ReadFormAsync().Result;
        //    var grantType = form.GetValues("grant_type");

        //    // If I remember correctly we arrive here for all implemented grant types.
        //    // But we don't want to add a refresh token to the refresh token itself.

        //    if (grantType[0] != "refresh_token")
        //    {
        //        int expire = 5 * 60;
        //        context.Ticket.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddSeconds(expire));
        //        context.SetToken(context.SerializeTicket());
        //    }
        //    base.Create(context);
        //    return Task.FromResult<object>(null);
        //}

        //public override Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        //{
        //    try
        //    {
        //        var refreshToken = TokenRepository.Instance.Get(context.Token);
        //        if (refreshToken != null)
        //        {
        //            if (TokenRepository.Instance.Delete(refreshToken))
        //            {
        //                context.DeserializeTicket(refreshToken.Ticket);
        //            }
        //        }
        //    }
        //    catch { }

        //    return Task.FromResult<object>(null);
        //}

        //public override void Create(AuthenticationTokenCreateContext context)
        //{
        //    var form = context.Request.ReadFormAsync().Result;
        //    var grantType = form.GetValues("grant_type");

        //    // If I remember correctly we arrive here for all implemented grant types.
        //    // But we don't want to add a refresh token to the refresh token itself.

        //    if (grantType[0] != "refresh_token")
        //    {
        //        int expire = 5 * 60;
        //        context.Ticket.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddSeconds(expire));
        //        context.SetToken(context.SerializeTicket());
        //    }
        //    base.Create(context);
        //}

        //public override void Receive(AuthenticationTokenReceiveContext context)
        //{
        //    context.DeserializeTicket(context.Token);
        //    base.Receive(context);
        //}


        public override void Create(AuthenticationTokenCreateContext context)
        {
            // Expiration time in seconds
            int expire = 5 * 60;
            //context.Ticket.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddSeconds(expire));

            context.Ticket.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddMinutes(5));
            context.SetToken(context.SerializeTicket());
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
            base.Receive(context);
        }
    }

    public class OAuthProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                //var account = AccountRepository.Instance.GetByUsername(context.UserName);
                //if (account != null && Global.VerifyHash(context.Password, account.Password))
                //{
                //    var claimsIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
                //    claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, account.Username));
                //    claimsIdentity.AddClaim(new Claim("DriverId", account.DriverId.ToString()));

                //    var newTicket = new AuthenticationTicket(claimsIdentity, null);
                //    context.Validated(newTicket);
                //}
            }
            catch { }

            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }
    }
}