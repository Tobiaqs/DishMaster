using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using wie_doet_de_afwas.Models;
using wie_doet_de_afwas.ViewModels;

namespace wie_doet_de_afwas.Controllers
{
    public class AuthController : BaseController
    {
        private readonly SignInManager<Person> signInManager;
        private readonly UserManager<Person> userManager;
        private readonly IConfiguration configuration;

        public AuthController(SignInManager<Person> signInManager, UserManager<Person> userManager, IConfiguration configuration, WDDAContext wDDAContext) : base(wDDAContext)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            var person = await Authenticate(loginViewModel);

            if (person != null) {
                return Json(new {
                    Token = BuildToken(person),
                    Succeeded = true
                });
            } else {
                return SucceededJson();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel registerViewModel)
        {
            var user = new Person {
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email,
                FullName = registerViewModel.FullName
            };

            var result = await userManager.CreateAsync(user, registerViewModel.Password);

            if (result.Succeeded)
            {
                return Json(new {
                    Token = BuildToken(user),
                    Succeeded = true
                });
            }

            return Json(result);
        }

        private async Task<Person> Authenticate(LoginViewModel loginViewModel)
        {
            var result = await signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, true, true);

            if (result.Succeeded) {
                var person = wDDAContext.Persons.Single((p) => loginViewModel.Email == p.UserName);
                return person;
            } else {
                return null;
            }
        }

        private string BuildToken(Person user)
        {
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Issuer"],
                claims: claims,
                expires: new DateTime(2038, 1, 1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
