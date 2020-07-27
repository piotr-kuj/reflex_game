using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KIK.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KIK.Controllers
{
    public class GameInvitationController : Controller
    {
        private IUserService _userService;
        private IStringLocalizer<GameInvitationController> _stringLocalizer;
        public GameInvitationController(IUserService userService, IStringLocalizer<GameInvitationController> stringLocalizer)
        { 
            _userService = userService;
            _stringLocalizer = stringLocalizer;
        }

        


        [HttpGet]
        public async Task<IActionResult> Index(string email)
        {
            var gameInvitationModel = new GameInvitationModel
            {
                InvitedBy = email
            };
            HttpContext.Session.SetString("email", email);
            return View(gameInvitationModel);
        }

        public IActionResult Index(GameInvitationModel gameInvitationModel)
        {
            return Content(_stringLocalizer["GameInvitationConfirmationMessage", gameInvitationModel.EmailTo]);

        }

    }
}
