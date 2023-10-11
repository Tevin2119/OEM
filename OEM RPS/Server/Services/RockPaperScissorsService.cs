using System.Drawing;
using Microsoft.EntityFrameworkCore;
using OEM_RPS.Shared;
using OEM_RPS.Shared.DTO;
using OEM_RPS.Shared.Enums;
using OEM_RPS.Shared.Model;
using OEMRPS.Server.Repositories;

public interface IRockPaperScissors
{
    public Task<RPSGame> StartGame(string PlayerName, int bestOf, bool randomMode);
    public Task<RPSGame> PlayRound(RPSGameDTO rPSGameDTO);
    public Task<List<RPSGame>> LeaderBoard();
}
public class RockPaperScissorsService : IRockPaperScissors
{
    private readonly IGenericRepository<RPSGame> repo;
    private readonly IGenericRepository<RoundResult> roundResultRepo;

    public RockPaperScissorsService(
        IGenericRepository<RPSGame> _repo,
        IGenericRepository<RoundResult> _roundResultRepo
    )
    {
        repo = _repo;
        roundResultRepo = _roundResultRepo;
    }

    public async Task<RPSGame> StartGame(string playerName, int bestOf, bool randomMode)
    {
        try
        {
            //check for an uncompleted instance of a game with the PlayerName and return this otherwise
            var hasGame = await repo.GetAsync(x => x.Player1 == playerName && !x.Closed);

            if (hasGame != null) return hasGame;

            //create a new entity for a game
            //initialise an instance of the game with defaults
            RPSGame game = new()
            {
                Player1 = playerName,
                //Default to CPU for single player mode
                Player2 = "CPU",
                RoundsToWin = bestOf,
                RandomMode = randomMode
            };

            //save Game and return obj
            var response = await repo.Add(game);

            //Verify Game is saved
            if (!response.IsSuccess || response.Entity == null) throw new Exception($"Failed to Save to db, \n Ex: {response.ErrorMessage}");

            return response.Entity;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to start a newGame with Player: {playerName} \n ex: {ex}");
        }
    }

    public async Task<List<RPSGame>> LeaderBoard()
    {
        try
        {
            return (List<RPSGame>)await repo.Queryable().AsQueryable().AsNoTracking().Include(x => x.RoundResults).ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"unexpected error occured, Ex: {ex}");
            throw new Exception($"Failed to retrieve all games");
        }
    }

    public async Task<RPSGame> PlayRound(RPSGameDTO rPSGameDTO)
    {
        if (rPSGameDTO == null || string.IsNullOrEmpty(rPSGameDTO.PlayerName))
        {
            // Handle the case where rPSGameDTO is null.
            throw new ArgumentNullException($"Not all properties are present in RPSGameDTO during PlayRound");
        }

        //verify ID afgainst db
        RPSGame rPSGame = await repo.Queryable().AsQueryable().Include(game => game.RoundResults).FirstOrDefaultAsync(game => game.Id == rPSGameDTO.GameID) ?? throw new ArgumentNullException($"Failed to retrieve RPSGame during PlayRound");

        //if(rPSGame == null) throw NULL
        //verify current Game and rounds against db
        if (rPSGame.Closed || (rPSGame.RoundResults != null && rPSGame.RoundsToWin == rPSGame.RoundResults.Count))
        {
            //game has concluded already return last finished Game
            //check that game is marked as closed
            if (!rPSGame.Closed)
            {
                rPSGame.Closed = true;

                var updateResponse = await repo.Update(rPSGame);

                if (!updateResponse.IsSuccess) throw new DbUpdateException($"Failed to close Game: {rPSGame.Id} on Update");
            }
            return rPSGame;
        }

        //Get Player2 pick 
        PositionEnum player2Position;

        try
        {
            //we only take a trip down memory lane when there are objects to query against 
            if (!rPSGame.RandomMode && rPSGame?.RoundResults?.Count > 0)
            {
                player2Position = rPSGame.RoundResults
                                    .OrderByDescending(x => x.createdAt)
                                    .FirstOrDefault() // Assuming CreatedAt is the correct property
                                    .Player1Choice;
            }
            else
            {
                player2Position = GetRandomPosition();
            }
            WinnerEnum winner = EvaluateRound(rPSGameDTO.Choice, player2Position);

            //add a new round
            RoundResult roundResult = new()
            {
                Player1Choice = rPSGameDTO.Choice,
                Player2Choice = player2Position,
                Winner = winner
            };

            //calculate winner
            //game logic to determine the winner of the round

            rPSGame.RoundResults ??= await roundResultRepo.Queryable().AsQueryable().Where(x => x.RPSGameId == rPSGame.Id).ToListAsync();
            rPSGame.RoundResults?.Add(roundResult);

            //verify current Game and rounds against gameObj
            if (rPSGame.RoundResults != null && rPSGame.RoundsToWin == rPSGame.RoundResults.Count)
            {
                //game has concluded update to close Game
                rPSGame.Closed = true;
            }

            //save to db
            var response = await repo.Update(rPSGame);

            //Verify Game is saved
            if (!response.IsSuccess) throw new Exception($"Failed to Updtae to db during PlayRound, \n Ex: {response.ErrorMessage}");


            return rPSGame;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"unexpected error occured, Ex: {ex}");
            throw new Exception($"Failed to play a round, please try again");
        }
    }

    private PositionEnum GetRandomPosition()
    {
        try
        {
            Array enumValues = Enum.GetValues(typeof(PositionEnum));

            // Create a random number generator
            Random rand = new Random();

            // Generate a random index within the valid range
            int randomIndex = rand.Next(enumValues.Length);

            // Cast the random index to the enum type to get the random enum value
            PositionEnum randomPosition = (PositionEnum)Enum.ToObject(typeof(PositionEnum), randomIndex);

            Console.WriteLine(randomPosition);
            return randomPosition;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured the getting position of player2, \n EX: {ex}");
            throw new Exception($"Failed to cast Position");
        }         
    }

    public static WinnerEnum EvaluateRound(PositionEnum player1Choice, PositionEnum player2Choice)
    {
        // Handle the logic to determine the winner
        if (player1Choice == player2Choice)
        {
            // It's a tie
            return WinnerEnum.Tie;
        }
        else if (
            (player1Choice == PositionEnum.Rock && (player2Choice == PositionEnum.Scissors || player2Choice == PositionEnum.Lizard)) ||
            (player1Choice == PositionEnum.Paper && (player2Choice == PositionEnum.Rock || player2Choice == PositionEnum.Spock)) ||
            (player1Choice == PositionEnum.Scissors && (player2Choice == PositionEnum.Paper || player2Choice == PositionEnum.Lizard)) ||
            (player1Choice == PositionEnum.Lizard && (player2Choice == PositionEnum.Spock || player2Choice == PositionEnum.Paper)) ||
            (player1Choice == PositionEnum.Spock && (player2Choice == PositionEnum.Scissors || player2Choice == PositionEnum.Rock))
        )
        {
            // Player1 wins
            return WinnerEnum.Player1;
        }
        else
        {
            // Player2 wins
            return WinnerEnum.Player2;
        }
    }
}