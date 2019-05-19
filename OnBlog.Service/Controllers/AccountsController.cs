using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnBlog.Service.Data;
using OnBlog.Service.Models;
using OnBlog.Service.Models.ViewModels.Account;

namespace OnBlog.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly Configuration configuration;

        public AccountsController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, Configuration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.context = context;
            this.configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(login);
            }

            var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            try
            {
                var user = await context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == login.Email);

                if (user == null)
                {
                    return NotFound();
                }

                return await TokenHandler(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] InstallViewModel installation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(installation);
            }

            var user = new ApplicationUser { FirstName = installation.FirstName, LastName = installation.LastName, UserName = installation.Email, Email = installation.Email };
            var result = await userManager.CreateAsync(user, installation.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            try
            {
                await roleManager.CreateAsync(new IdentityRole("Administrator"));
                await userManager.AddToRoleAsync(user, "Administrator");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return NoContent();
        }

        private async Task<string> TokenHandler(ApplicationUser user)
        {
            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FullName()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Typ, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Key));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(signingCredentials: signingCredentials, issuer: configuration.Copyright, claims: claims, expires: DateTime.UtcNow.AddDays(1));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}