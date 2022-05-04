using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    public class UsersController : BaseApiController
    {
        public DataContext _context { get; }
        public UsersController(DataContext context)
        {
            _context = context;

        }
        
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<Appuser>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Appuser>> GetUser(int id)
        {
           return await _context.Users.FindAsync(id);
        }
    }
}