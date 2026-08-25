using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityModel;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Identity.API.Model;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
           var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var subjectId = subject.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (subjectId == null)
            {
                throw new ArgumentException("No sub claim present", nameof(context.Subject));
            }

            var user = await _userManager.FindByIdAsync(subjectId);
            if (user == null)
            {
                throw new ArgumentException("Invalid subject identifier", nameof(context.Subject));
            }
            var claims = GetClaimsFromUser(user);
            context.IssuedClaims = claims.ToList();



        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var subjectId = subject.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var user = await _userManager.FindByIdAsync(subjectId);
            context.IsActive = false;
            if (user != null)
            {
                if(_userManager.SupportsUserSecurityStamp)
                {
                    var security_stamp = subject.Claims.Where(c => c.Type == "security_stamp").Select(c => c.Value).SingleOrDefault();
                    if (security_stamp != null)
                    {
                        var db_security_stamp =await _userManager.GetSecurityStampAsync(user);
                        if (db_security_stamp != security_stamp)
                        {
                            return;
                        }
                    }
                }
                context.IsActive = !user.LockoutEnabled || !user.LockoutEnd.HasValue ||
                    user.LockoutEnd.Value.UtcDateTime <= DateTime.UtcNow;
            }

            
        }
        private IEnumerable<System.Security.Claims.Claim> GetClaimsFromUser(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
             new Claim(JwtClaimTypes.Subject, user.Id),
             new Claim(JwtClaimTypes.PreferredUserName, user.UserName), 
             new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtClaimTypes.Name, user.UserName),
            };
            if (!string.IsNullOrEmpty(user.FirstName))
            {
                claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
            }
            if (!string.IsNullOrEmpty(user.LastName))
            {
                claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
            }
            if(_userManager.SupportsUserEmail)
            {
                claims.AddRange(new[]
                {
                    new Claim(JwtClaimTypes.Email, user.Email),
                    new Claim(JwtClaimTypes.EmailVerified, user.EmailConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
                });
            }
            if(_userManager.SupportsUserPhoneNumber && !string.IsNullOrEmpty(user.PhoneNumber))
            {
                claims.AddRange(new[]
                {
                    new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
                    new Claim(JwtClaimTypes.PhoneNumberVerified, user.PhoneNumberConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
                });
            }
            return claims;
        }
    }
}