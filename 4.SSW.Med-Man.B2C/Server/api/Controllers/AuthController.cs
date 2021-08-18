﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SSW.Med_Man.MVC.Data;
using SSW.Med_Man.MVC.DTOs;

namespace SSW.Med_Man.MVC.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signinManager, IConfiguration configuration, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signinManager;
            _configuration = configuration;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<SuccesfulLoginResult>> Login([FromBody] LoginUserDTO loginUser)
        {
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginUser.email, loginUser.password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
                return Unauthorized();

            //IdentityUser user = await _userManager.FindByEmailAsync(loginUser.email);

            IdentityUser user = await _userManager.FindByNameAsync(loginUser.email); //.FindByEmailAsync(loginUser.email);
            JwtSecurityToken token = await GenerateTokenAsync(user);

            string serializedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new SuccesfulLoginResult() { Token = serializedToken });
        }

        private async Task<JwtSecurityToken> GenerateTokenAsync(IdentityUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.NormalizedUserName)
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var expirationDays = _configuration.GetValue<int>("JWTConfiguration:TokenExpirationDays");
            var signingKey = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWTConfiguration:SigningKey"));

            var token = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("JWTConfiguration:Issuer"),
                audience: _configuration.GetValue<string>("JWTConfiguration:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(expirationDays)),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}