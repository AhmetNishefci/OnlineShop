﻿using Kickz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kickz.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class UsersController : Controller
    {

        private readonly UserManager<AppUser> userManager;
        

        public UsersController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
          
        }
        public IActionResult Index()
        {
            return View(userManager.Users);
        }
    }
}
