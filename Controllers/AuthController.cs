using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace wie_doet_de_afwas
{
    public class AuthController : Controller
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public AuthController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            var result = await signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, true);

            return Json(new {
                Succeeded = result.Succeeded
            });
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel registerViewModel)
        {
            var user = new User {
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email,
                FullName = registerViewModel.FullName
            };

            var result = await userManager.CreateAsync(user, registerViewModel.Password);

            return Json(result);
        }
    }
}