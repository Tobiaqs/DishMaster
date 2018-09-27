using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace wie_doet_de_afwas
{
    public class AuthController : Controller
    {
        private readonly SignInManager<User> signInManager;

        public AuthController(SignInManager<User> signInManager)
        {
            this.signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            var result = await signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe, false);
            
            return Json(new {
                Succeeded = result.Succeeded
            });
        }
    }
}