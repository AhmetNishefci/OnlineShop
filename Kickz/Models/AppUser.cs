using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kickz.Models
{
    public class AppUser : IdentityUser
    {
        internal static AppUser appUser;

        public string Occupation { get; set; }


    }
}
