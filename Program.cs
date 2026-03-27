using System.Diagnostics;
using MathGame;

MathGameLogic mathGame = new MathGameLogic();
Random random = new Random();

int score = 0;
bool gameOver = false;
GameDifficulty gameDifficulty = GameDifficulty.Easy;

while (!gameOver)
{
    int userMenuSelection = GetUserMenuSelection(mathGame);

    // Math operation questions
    if (userMenuSelection >= 1 && userMenuSelection <= 5)
    {
        int numberOfQuestions = 5;

        while (!gameOver && numberOfQuestions > 0)
        {
            int firstNumber = random.Next(1, 101);
            int secondNumber = random.Next(1, 101);

            // Determine operation
            char op = userMenuSelection switch
            {
                1 => '+',
                2 => '-',
                3 => '*',
                4 => '/',
                5 => (random.Next(1, 5) switch { 1 => '+', 2 => '-', 3 => '*', _ => '/' }),
                _ => '+'
            };

            // Make sure division is exact
            if (op == '/')
            {
                while (secondNumber == 0 || firstNumber % secondNumber != 0)
                {
                    firstNumber = random.Next(1, 101);
                    secondNumber = random.Next(1, 101);
                }
            }

            // Ask the question
            var result = await PerformOperation(mathGame, firstNumber, secondNumber, op, score, gameDifficulty);

            // Update score and check if game over
            score = result.updatedScore;
            if (result.isGameOver)
            {
                gameOver = true;
                break;
            }

            numberOfQuestions--;
        }
    }
    else // Non-math menu options
    {
        switch (userMenuSelection)
        {
            case 6:
                DisplayGameHistory(mathGame);
                break;
            case 7:
                gameDifficulty = ChangeGameDifficulty();
                Console.WriteLine($"Your new difficulty level: {gameDifficulty}");
                break;
            case 8:
                gameOver = true;
                Console.WriteLine($"Your final score is: {score}");
                break;
        }
    }
}

// FUNCTIONS

static GameDifficulty ChangeGameDifficulty()
{
    Console.WriteLine("Please enter a difficulty level");
    Console.WriteLine("1. Easy");
    Console.WriteLine("2. Medium");
    Console.WriteLine("3. Hard");

    while (true)
    {
        string? input = Console.ReadLine();
        if (int.TryParse(input, out int selection) && selection >= 1 && selection <= 3)
        {
            return selection switch
            {
                1 => GameDifficulty.Easy,
                2 => GameDifficulty.Medium,
                3 => GameDifficulty.Hard,
                _ => GameDifficulty.Easy
            };
        }

        Console.WriteLine("Please enter a valid option between 1 and 3");
    }
}

static int GetUserMenuSelection(MathGameLogic mathGame)
{
    mathGame.ShowMenu();

    while (true)
    {
        string? input = Console.ReadLine();
        if (int.TryParse(input, out int selection) && selection >= 1 && selection <= 8)
            return selection;

        Console.WriteLine("Please enter a valid option between 1 and 8");
    }
}

static void DisplayGameQuestion(int firstNumber, int secondNumber, char operation)
{
    Console.WriteLine($"{firstNumber} {operation} {secondNumber} = ??");
}

static async Task<int?> GetUserResponse(GameDifficulty gameDifficulty)
{
    int timeLimit = (int)gameDifficulty;
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();

    var inputTask = Task.Run(() => Console.ReadLine());
    var delayTask = Task.Delay(timeLimit * 1000);

    var completedTask = await Task.WhenAny(inputTask, delayTask);
    stopwatch.Stop();

    if (completedTask == inputTask)
    {
        string? result = await inputTask;
        if (result != null && int.TryParse(result, out int response))
        {
            Console.WriteLine($"Time taken: {stopwatch.Elapsed:mm\\:ss\\.fff}");
            return response;
        }
    }

    Console.WriteLine("Time is up!");
    return null;
}

static async Task<(int updatedScore, bool isGameOver)> PerformOperation(
    MathGameLogic mathGame, int firstNumber, int secondNumber, char operation, int score, GameDifficulty gameDifficulty)
{
    DisplayGameQuestion(firstNumber, secondNumber, operation);
    int result = mathGame.MathOperations(firstNumber, secondNumber, operation);
    int? userResponse = await GetUserResponse(gameDifficulty);

    if (userResponse == null)
    {
        Console.WriteLine("Game Over due to timeout!");
        return (score, true);
    }

    if (userResponse != result)
    {
        Console.WriteLine("Wrong answer! Game Over!");
        Console.WriteLine($"Correct answer was: {result}");
        return (score, true);
    }

    Console.WriteLine("Correct answer! You earned 5 points.");
    score += 5;
    return (score, false);
}

static void DisplayGameHistory(MathGameLogic mathGame)
{
    foreach (string operation in mathGame.GameHistory)
    {
        Console.WriteLine(operation);
    }
}

public enum GameDifficulty
{
    Easy = 50,
    Medium = 30,
    Hard = 10
}