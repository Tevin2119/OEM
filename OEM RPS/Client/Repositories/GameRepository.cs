using OEM_RPS.Shared;
using OEM_RPS.Shared.DTO;
using System.Net.Http.Json;
//using System.Text.Json;

namespace OEM_RPS.Client.Repositories
{
    public interface IGameRepository
    {
		Task<ApiResponse<RPSGame>> StartGameAsync(string playerName, int bestOf, bool randommode);
		Task<ApiResponse<RPSGame>> PlayRoundAsync(RPSGameDTO rPSGameDTO);
        Task<ApiResponse<List<RPSGame>>> GetAllGames();
    }
    public class GameRepository: IGameRepository
    {
        private readonly HttpClient httpClient;
        //private readonly JsonSerializerOptions _options;

        public GameRepository(HttpClient _httpClient)
        {
            httpClient = _httpClient;
            //_options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<ApiResponse<RPSGame>> StartGameAsync(string playerName, int bestOf, bool randDom)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<ApiResponse<RPSGame>>($"startgame/{playerName}/{bestOf}/{randDom}");

                if (response != null)
                {
                    return response;
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle exception if needed
                Console.WriteLine($"Error: {ex.Message}");
            }
            return new ApiResponse<RPSGame>(OEM_RPS.Shared.Enums.StatusCodeEnum.NotFound, "Failed to retirieve a game", null);
        }

        public async Task<ApiResponse<RPSGame>> PlayRoundAsync(RPSGameDTO rPSGame)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("playround", rPSGame);

                var res = await response.Content.ReadFromJsonAsync<ApiResponse<RPSGame>>();

                if (res != null) return res;
            }
            catch (HttpRequestException ex)
            {
                // Handle exception if needed
                Console.WriteLine($"Error: {ex.Message}");
            }
            return new ApiResponse<RPSGame>(OEM_RPS.Shared.Enums.StatusCodeEnum.BadRequest, "Failed to start a round", null);
        }

        public async Task<ApiResponse<List<RPSGame>>> GetAllGames()
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<ApiResponse<List<RPSGame>>>($"getall");
                if (response != null) return response;
            }
            catch (HttpRequestException ex)
            {
                // Handle exception if needed
                Console.WriteLine($"Error: {ex.Message}");
            }
            return new ApiResponse<List<RPSGame>>(OEM_RPS.Shared.Enums.StatusCodeEnum.BadRequest, "Failed to get all games", null);
        }
    }
}

