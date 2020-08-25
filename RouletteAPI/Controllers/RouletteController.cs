using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using RouletteAPI.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RouletteController : ControllerBase
    {
        private readonly ActionExecutedContext context;
        public RouletteController(ActionExecutedContext context)
        {
            this.context = context;
        }
        static List<Roulette> roulettes = new List<Roulette>();
        List<User> users = new List<User>(3){
            new User()
            {
                UserId = "1",
                UserName = "user1",
                Credit = 86.000
            },
            new User()
            {
                UserId = "2",
                UserName = "user2",
                Credit = 50.000
            },
            new User()
            {
                UserId = "3",
                UserName = "user3",
                Credit = 23.000
            }
        };
        [HttpPost]
        [Route("SetRoulette")]
        public int SetRoulette(Roulette roulette)
        {
            try
            {
                if (roulette.Name != null)
                {
                    roulette.Id = roulettes.Count() + 1;
                    roulettes.Add(roulette);
                }
                else
                {

                    return 0;
                }

                return roulette.Id;
            }
            catch
            {

                return 0;
            }      
        }
        [HttpPost]
        [Route("OpenRoulette")]
        public IActionResult OpenRoulette(int id)
        {
            try
            {
                if (roulettes.Any(i => i.Id == id))
                {
                    roulettes.Where(i => i.Id == id).FirstOrDefault(v => v.Status = true);
                }
                else
                {

                    return BadRequest($"La ruleta con el ID {id} no fue encontrada, verifique e intente nuevamente");
                }

                return StatusCode(200);
            }
            catch
            {

                return StatusCode(500);
            }
        }
        [HttpPost]
        [Route("StakeRoulette")]
        public IActionResult StakeRoulette(Stake stake)
        {
            try
            {
                var userId = context.HttpContext.Request.Headers["Basic"];
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();
                if (users.Any(u => u.UserId == userId))
                {
                    if (users.FirstOrDefault(u => u.UserId == userId).Credit > stake.Value && 
                        (stake.Color.Trim().ToLower().Contains("rojo") || 
                        stake.Color.Trim().ToLower().Contains("verde")) &&
                        stake.Number <= 36 &&
                        stake.Value < 10000)
                    {

                        return StatusCode(200);
                    }
                    else
                    {

                        return BadRequest();
                    }
                }
                else
                {

                    return Unauthorized();
                }
            }
            catch
            {

                return StatusCode(500);
            }
        }
    }
}
