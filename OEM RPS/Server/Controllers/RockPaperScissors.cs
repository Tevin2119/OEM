using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OEM_RPS.Shared;
using OEM_RPS.Shared.DTO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OEMRPS.Server.Controllers
{
    public class RockPaperScissors : Controller
    {
        private readonly IRockPaperScissors gameService;
        public RockPaperScissors
        (
            IRockPaperScissors _gameService
        )
        {
            gameService = _gameService;
        }

        // GET: /<controller>/
        [HttpGet("startgame/{playerName}/{bestOf:int}")]
        public async Task<IActionResult> Index(string playerName, int bestOf)
        {
            RPSGame game = await gameService.StartGame(playerName, bestOf);

            if(game == null) return BadRequest();
            return Ok(game);
        }

        [HttpPost("playround/")]
        public async Task<RPSGame> PlayRound(RPSGameDTO rPSGame)
        {
            return await gameService.PlayRound(rPSGame);
        }
    }
}

