using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheXEffect.Data.Constants;
using TheXEffect.Data.Extensions;
using TheXEffect.Data.Models;

namespace TheXEffect.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Current goal")]
            [StringLength(30, MinimumLength = 2)]
            public string DefaultCalendarGoal { get; set; }
        }

        private void _load()
        {
            Input = new InputModel
            {
                DefaultCalendarGoal = User.GetDefaultCalendarGoal()
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            _load();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                _load();
                return Page();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            await _userManager.ReplaceClaimAsync(user, User.FindFirst(UserClaims.DefaultCalendarGoal), new Claim(UserClaims.DefaultCalendarGoal, Input.DefaultCalendarGoal ?? ""));

            await _signInManager.RefreshSignInAsync(user);

            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
