﻿using Microsoft.AspNetCore.Mvc;
using MovieStore.Models.DTO;
using MovieStore.Repositories.Abstract;

namespace MovieStore.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _userAuthentication;

        public UserAuthenticationController(IUserAuthenticationService userAuthentication)
        {
            _userAuthentication = userAuthentication;
        }


        //public async Task<IActionResult> Register()
        //{
        //    var model = new RegistrationModel
        //    {
        //        Email = "admin@gmail.com",
        //        Username = "admin",
        //        Name = "Ravindra",
        //        Password = "Admin@123",
        //        PasswordConfirm = "Admin@123",
        //        Role = "Admin"
        //    };
        //    // if you want to register with user , Change Role="User"
        //    var result = await _userAuthentication.RegisterAsync(model);
        //    return Ok(result.Message);
        //}


        public async Task<IActionResult> Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            { 
                return View(model);
            }
            var result = await _userAuthentication.LoginAsync(model);
            if (result.StatusCode == 1) 
            { 
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["msg"] = "Could not logged in..";
                return RedirectToAction(nameof(Login));
            }
        }


        public async Task<IActionResult> Logout()
        {
            await _userAuthentication.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
