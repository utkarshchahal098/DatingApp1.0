using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Entities;

namespace API.Controllers
{
    public class BuggyController: BaseApiController
    {
        private readonly  DataContext _Context;
        public BuggyController(DataContext context)
        {
            _Context = context;
        }
        
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secretText";
        }

        [HttpGet("not-found")]
        public ActionResult<string> GetNotFound()
        {
            var thing = _Context.Users.Find(-1);

            if(thing == null){
                return NotFound();
            }
            return Ok(thing);
            
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = _Context.Users.Find(-1);

            var thingToReturn = thing.ToString();

            return thingToReturn;
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("bad request this is ");
        }

    }
}