using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Model
{
    public class ApplicationUser : IdentityUser
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
    }
}