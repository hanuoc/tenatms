using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using TMS.Common;
using TMS.Common.Constants;
using TMS.Model.Models;
using TMS.Service;
using TMS.Web.Infrastructure.Core;
using TMS.Web.Models;
using System.DirectoryServices;

namespace TMS.Web.Providers
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public AuthorizationServerProvider()
        {
        }
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            await Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            UserManager<AppUser> userManager = context.OwinContext.GetUserManager<UserManager<AppUser>>();
            AppUser user;
            try
            {
                user = await userManager.FindAsync(context.UserName, context.Password);
            }
            catch
            {
                // Could not retrieve the user due to error.
                context.SetError(CommonConstants.ServerError, MessageSystem.ServerProcessingError);
                return;
            }
            if (user != null)
            {
                if (!user.Status.Value)
                {
                    context.SetError(CommonConstants.ServerError, MessageSystem.BlockAccount);
                    var b = context.Error;
                   // context.Rejected();
                    return;
                }
                var permissions = ServiceFactory.Get<IPermissionService>().GetByUserId(user.Id);
                var permissionViewModels = AutoMapper.Mapper.Map<ICollection<Permission>, ICollection<PermissionViewModel>>(permissions);
                var roles = userManager.GetRoles(user.Id);
                ClaimsIdentity identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ExternalBearer);
                string email = string.IsNullOrEmpty(user.Email) ? "" : user.Email;
                identity.AddClaim(new Claim("id", user.Id));
                identity.AddClaim(new Claim("fullName", user.FullName));
                identity.AddClaim(new Claim("email", email));
                identity.AddClaim(new Claim("username", user.UserName));
                identity.AddClaim(new Claim("roles", JsonConvert.SerializeObject(roles)));
                identity.AddClaim(new Claim("permissions", JsonConvert.SerializeObject(permissionViewModels)));
                identity.AddClaim(new Claim("groupId", user.GroupId.ToString()));
                var props = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        {"id", user.Id},
                        {"fullName", user.FullName},
                        {"email", email},
                        {"username", user.UserName},
                        {"permissions",JsonConvert.SerializeObject(permissionViewModels) },
                        {"roles",JsonConvert.SerializeObject(roles) },
                        {"groupId",user.GroupId.ToString() }
                    });
                context.Validated(new AuthenticationTicket(identity, props));
                MemoryCacheHelper.RemoveUserEditByAdmin(user.UserName);
            }
            else
            {
                context.SetError(CommonConstants.Invalid_Grant, MessageSystem.WorngUserNameAndPassWord);
            }
        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }

    }
}
