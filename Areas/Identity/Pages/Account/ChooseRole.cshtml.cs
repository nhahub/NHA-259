using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HeavyGo_Project_Identity.Areas.Identity.Pages.Account
{
    public class ChooseRoleModel : PageModel
    {
        [BindProperty]
        public string roleName { get; set; }

        public void OnGet()
        {
            roleName = "Client"; // default selection
        }

        public IActionResult OnPost()
        {
            if (roleName == "Client")
                return RedirectToPage("/Account/Register", new { area = "Identity", role = "Client" });
            else if (roleName == "Driver")
                return RedirectToPage("/Account/Register", new { area = "Identity", role = "Driver" });

            return Page();
        }

    }
}
