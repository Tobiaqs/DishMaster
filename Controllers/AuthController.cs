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
using DishMaster.Models;
using DishMaster.ViewModels;
using MimeKit;
using MailKit.Net.Smtp;
using DishMaster.Annotations;
using System.Web;
using DishMaster.Data;

namespace DishMaster.Controllers
{
    public class AuthController : BaseController
    {
        private readonly SignInManager<Person> signInManager;
        private readonly UserManager<Person> userManager;
        private readonly IConfiguration configuration;

        public AuthController(SignInManager<Person> signInManager, UserManager<Person> userManager, IConfiguration configuration, DMContext dMContext) : base(dMContext)
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
                return UnauthorizedJson();
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

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel forgotPasswordViewModel)
        {
            // check if smtp is actually enabled. if not, fail
            if (this.configuration["Smtp:Enabled"] != "True") {
                return FailedJson();
            }

            var person = dMContext.Persons.SingleOrDefault((p) => p.Email.ToLower() == forgotPasswordViewModel.Email.ToLower());

            if (person == null) {
                return NotFoundJson();
            }

            var resetToken = await userManager.GeneratePasswordResetTokenAsync(person);

            // construct message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("DishMaster", "dishmaster@tobiass.nl"));
            message.To.Add(new MailboxAddress(person.FullName, person.Email));
            message.Subject = "Forgot password";

            // construct body
            var bodyBuilder = new BodyBuilder();
            var link = this.configuration["Domain"] + "/login/reset?token=" + HttpUtility.UrlEncode(resetToken, Encoding.UTF8) + "&email=" + HttpUtility.UrlEncode(person.Email, Encoding.UTF8);
            bodyBuilder.HtmlBody = "Hello,<br><br>You've requested a new password on WhoDoesTheDishes.today.<br><br><a href=\"" + link + "\">Click here to create your new password.</a><br><br>The link is valid for 24 hours.<br><br>Best regards,<br>WhoDoesTheDishes.today<br>";
            bodyBuilder.TextBody = "Hello,\n\nYou've requested a new password on WhoDoesTheDishes.today.\n\nClick here to create your new password:\n\n" + link + "\n\nThe link is valid for 24 hours.\n\nBest regards,\nWhoDoesTheDishes.today\n";
            message.Body = bodyBuilder.ToMessageBody();

            // send message
            var client = new SmtpClient();
            await client.ConnectAsync(this.configuration["Smtp:Server"], int.Parse(this.configuration["Smtp:Port"]), this.configuration["Smtp:SSL"] == "True");
            await client.AuthenticateAsync(this.configuration["Smtp:Username"], this.configuration["Smtp:Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            client.Dispose();

            // store reset token in person
            person.ResetExpiration = System.DateTime.UtcNow.AddDays(1);

            // save
            await dMContext.SaveChangesAsync();

            return SucceededJson();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel resetPasswordViewModel)
        {
            var user = await userManager.FindByEmailAsync(resetPasswordViewModel.Email);

            if (user == null) {
                return NotFoundJson();
            }

            if (user.ResetExpiration > System.DateTime.UtcNow) {
                var result = await userManager.ResetPasswordAsync(user, resetPasswordViewModel.ResetToken, resetPasswordViewModel.Password);
                return result.Succeeded ? SucceededJson() : FailedJson();
            }

            return FailedJson();
        }

        private async Task<Person> Authenticate(LoginViewModel loginViewModel)
        {
            var result = await signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, true, true);

            if (result.Succeeded) {
                var person = dMContext.Persons.Single((p) => loginViewModel.Email == p.UserName);
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
