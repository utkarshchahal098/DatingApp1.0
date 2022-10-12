using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        

        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO dto)
        {

            if(await UserExists(dto.username))
            {
              return BadRequest("User exists");
            }
           using var hmac = new HMACSHA512(); 

           var user = new Appuser
           {
              UserName =  dto.username.ToLower(),
              PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.password)),
              PasswordSalt = hmac.Key
           };

           _context.Users.Add(user);
           await _context.SaveChangesAsync();

           return new UserDTO{
               UserName = user.UserName,
               Token = _tokenService.CreateToken(user)
           };
        }

        [HttpPost("login")]

        public async Task<ActionResult<UserDTO>> Login(LoginDTO dto)
        {
           var user = await _context
                .Users.SingleOrDefaultAsync(x => x.UserName == dto.UserName);

            if( user is null)
            {
                return Unauthorized("User doesn't exist");
            }
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

            for(int i = 0; i< computedHash.Length;i++)
            {
               if(computedHash[i] != user.PasswordHash[i])
                   return Unauthorized("Wrong Password");
            }

           return new UserDTO{
               UserName = user.UserName,
               Token = _tokenService.CreateToken(user)
           };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(X => X.UserName == username.ToLower());
        }
    }
}