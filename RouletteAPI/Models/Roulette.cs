using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteAPI.Models
{
    public class Roulette
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public bool Status { get; set; } = false;
    }
}
