using Microsoft.AspNetCore.Mvc;
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
        static ConcurrentBag<Roulette> roulettes = new ConcurrentBag<Roulette>();

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
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
