using System.Drawing;
using OEM_RPS.Shared;
using OEM_RPS.Shared.DTO;
using OEM_RPS.Shared.Enums;
using OEM_RPS.Shared.Model;
using OEMRPS.Server.Repositories;

public interface IRockPaperScissors
{
    public Task<RPSGame> StartGame(string PlayerName, int bestOf);
    public Task<RPSGame> PlayRound(RPSGameDTO rPSGameDTO);
}
public class RockPaperScissorsService : IRockPaperScissors
{
    private readonly IGenericRepository<RPSGame> repo;

    public RockPaperScissorsService(IGenericRepository<RPSGame> _repo)
    {
        repo = _repo;
    }

    public async Task<RPSGame> StartGame(string playerName, int bestOf)
    {
        try
        {
            //check for an uncompleted instance of a game with the PlayerName and return this otherwise
            var hasGame = await repo.GetAsync(x => x.Player1 == playerName && !x.Closed);

            if (hasGame != null) return hasGame;

            //create a new entity for a game
            //initialise an instance of the game with defaults
            RPSGame game = new(playerName, bestOf);

            //default all choices to null
            //save Game and return obj
            var response = await repo.Add(game);

            //Verify Game is saved
            if (!response.IsSuccess) throw new Exception($"Failed to Save to db, \n Ex: {response.ErrorMessage}");

            return game;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to start a newGame with Player: {playerName} \n ex: {ex}");
            return null;
        }
    }


    public async Task<RPSGame> PlayRound(RPSGameDTO rPSGameDTO)
    {
        //verify ID afgainst db
        RPSGame rPSGame = await repo.GetById(rPSGameDTO.GameID);

        //verify current Game and rounds against db
        if(rPSGame.Closed || rPSGame.RoundsToWin == rPSGame.RoundResults.Count)
        {
            //game has concluded already return finished Game

            return rPSGame;
        }

        //add a new round
        RoundResult roundResult = new();

        roundResult.Player1Choice = rPSGameDTO.Choice;
        roundResult.Player2Choice = GetRandomPosition();

        //calculate winner

        rPSGame.RoundResults.Add(roundResult);

        //save to db
        var response = await repo.Update(rPSGame);

        //Verify Game is saved
        if (!response.IsSuccess) throw new Exception($"Failed to Updtae to db, \n Ex: {response.ErrorMessage}");


        return rPSGame;
    }

    private Position GetRandomPosition()
    {
        try
        {
            Array enumValues = Enum.GetValues(typeof(Position));

            // Create a random number generator
            Random rand = new Random();

            // Generate a random index within the valid range
            int randomIndex = rand.Next(enumValues.Length);

            // Cast the random index to the enum type to get the random enum value
            Position randomPosition = (Position)enumValues.GetValue(randomIndex);

            Console.WriteLine(randomPosition);
            return randomPosition;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to cast Position, \n Ex: {ex}");
        }         
    }
}