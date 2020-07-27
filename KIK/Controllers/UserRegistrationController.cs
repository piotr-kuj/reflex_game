using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using KIK.Model;
using KIK.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace KIK.Controllers
{
    public class UserRegistrationController : Controller
    {
        readonly IUserService _userService;
        readonly IEmailService _emailService;
        public UserRegistrationController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        HomeController homeCulture;
        HomeController swapHomeCulture;
        [HttpPost]
        public async Task<IActionResult> Index(UserModel userModel)
        {
            if(ModelState.IsValid)
            {
                await _userService.RegisterUser(userModel);
                //if(homeCulture.Accepted())
                //return Content($"Uzytkownik {userModel.FirstName} {userModel.LastName} zostal zarejestrowany pomyslnie.");
                return RedirectToAction(nameof(EmailConfirmation), new { userModel.Email });
            }
            else
            {

                return View(userModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EmailConfirmation(string email)
        {
            var user = await _userService.GetUserByEmail(email);

            var urlAction = new UrlActionContext
            {
                Action = "ConfirmEmail",
                Controller = "UserRegistration",
                Values = new { email },
                Protocol = Request.Scheme,
                Host = Request.Host.ToString()
            };

            /*
            if(CultureInfo.CurrentCulture.EnglishName=="English")
            {
            var messageEn = $"Thank You for registration. To confirm your account, please click here "
                + $"{Url.Action(urlAction)}";
            }
            */
            var message = $"Dziekujemy za rejestracje. Aby Potwierdzic email, prosze kilknać tutaj "
                + $"{Url.Action(urlAction)}";

            try
            {
                /*
                if(CultureInfo.CurrentCulture.EnglishName=="English")
                {
                    _emailService.SendEmail(email, "Potwierdzenie adresu email", messageEn).Wait();
                }
                */
                _emailService.SendEmail(email, "Potwierdzenie adresu email", message).Wait();
            }
            catch(Exception e)
            {

            }


            if(user?.IsEmailConfirmed == true )
            {
                return RedirectToAction("Index", "GameInvitation", new { email = email });
            }

            ViewBag.Email = email;

            return View();
        }
    }
}
