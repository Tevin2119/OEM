using Microsoft.AspNetCore.Mvc;
using OEM_RPS.Shared;
using OEM_RPS.Shared.DTO;
using OEM_RPS.Shared.Enums;

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
        [HttpGet("startgame/{playerName}/{bestOf:int}/{random:bool}")]
        public async Task<ActionResult<ApiResponse<RPSGame>>> StartGame(string playerName, int bestOf, bool random)
        {
            ApiResponse<RPSGame> apiResponse = new(StatusCodeEnum.BadRequest, "", null);
            try
            {
                RPSGame game = await gameService.StartGame(playerName, bestOf, random);

                if (game == null)
                {
                    apiResponse.Message = $"Failed to start game for player: {playerName}, please try again";
                    return apiResponse;
                }

                return Ok(ApiResponse<RPSGame>.Success(game));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<RPSGame>.BadRequest($"Failed to start a new game: {ex.Message}"));
            }
        }

        // GET: /<controller>/
        [HttpGet("getall")]
        public async Task<ActionResult<ApiResponse<List<RPSGame>>>> LeaderBoard()
        {
            ApiResponse<List<RPSGame>> apiResponse = new(StatusCodeEnum.Success, "", null);
            try
            {
                List<RPSGame> games = await gameService.LeaderBoard();

                if (games == null)
                {
                    apiResponse.Message = $"Failed to all games";
                    return apiResponse;
                }

                return Ok(ApiResponse<List<RPSGame>>.Success(games));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<RPSGame>.BadRequest($"Failed to start a new game: {ex.Message}"));
            }
        }

        //POST
        [HttpPost("playround/")]
        public async Task<ActionResult<ApiResponse<RPSGame>>> PlayRound([FromBody] RPSGameDTO rPSGame)
        {
            ApiResponse<RPSGame> apiResponse = new(StatusCodeEnum.BadRequest, "", null);
            try
            {
                RPSGame game = await gameService.PlayRound(rPSGame);
                if(game == null)
                {
                    apiResponse.Message = $"Failed to make a move in existing game, please try again";
                    return apiResponse;

                }
                return Ok(ApiResponse<RPSGame>.Success(game));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<RPSGame>.BadRequest($"Failed to continue an existing game: {ex.Message}"));
            }
        }
    }
}

