using System;
using OEM_RPS.Shared;

namespace OEMRPS.Server.Repositories
{
	public interface IGameRepository<RPSGame>
	{
		public Task<RPSGame?> GetById(int Id);
		public Task<RPSGame?> GetCurrentGame(string PlayerName);
		public Task<List<RPSGame>> GetAllPlayerGames(string PlayerName);


	}
	public class GameRepo: IGameRepository<RPSGame>
	{
        private readonly IGenericRepository<RPSGame> gameRepository;
		public GameRepo
        (
            IGenericRepository<RPSGame> _gameRepository
        )
		{
            gameRepository = _gameRepository;
		}

        public async Task<List<RPSGame>> GetAllPlayerGames(string PlayerName)
        {
            List<RPSGame> games = new();
            try
            {
                games = await gameRepository.GetAllAsync(x => x.Player1 == PlayerName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get games for Player: {PlayerName}, \n EX: {ex}");
            }

            return games;
        }

        public async Task<RPSGame?> GetById(int Id)
        {
            try
            {
                return await gameRepository.GetById(Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get game by Id: {Id}, \n EX: {ex}");

            }
            return null;
        }

        public async Task<RPSGame?> GetCurrentGame(string PlayerName)
        {
            try
            {
                return await gameRepository.GetAsync(x => x.Player1 == PlayerName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get current game by PlayerName: {PlayerName}, \n EX: {ex}");

            }
            return null;
        }
    }
}

