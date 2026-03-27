namespace MathGame
{
    public class MathGameLogic
    {
        public List<string> GameHistory { get; set; } = new List<string>();

        public void ShowMenu()
        {
            Console.WriteLine("Welcome to the Simple Maths Game");
            Console.WriteLine("Please choose an option to continue");
            Console.WriteLine("1. Summation Game Mode");
            Console.WriteLine("2. Subtraction Game Mode");
            Console.WriteLine("3. Multiplication Game Mode");
            Console.WriteLine("4. Division Game Mode");
            Console.WriteLine("5. Random Game Mode");
            Console.WriteLine("6. Show Game History");
            Console.WriteLine("7. Change Game Difficulty");
            Console.WriteLine("8. Exit");
        }

        public int MathOperations(int firstNumber,  int secondNumber, char operation)
        {
            switch (operation)
            {
                case '+':
                    GameHistory.Add($"{firstNumber} + {secondNumber} = {firstNumber + secondNumber}");
                    return firstNumber + secondNumber;
                case '-':
                    GameHistory.Add($"{firstNumber} - {secondNumber} = {firstNumber - secondNumber}");
                    return firstNumber - secondNumber;
                case '*':
                    GameHistory.Add($"{firstNumber} * {secondNumber} = {firstNumber * secondNumber}");
                    return firstNumber * secondNumber;
                case '/':
                    while (firstNumber < 0 || firstNumber > 100)
                    {
                        Console.WriteLine("Please enter a number between 0 and 100");
                        firstNumber = Convert.ToInt32(Console.ReadLine());
                    }
                    GameHistory.Add($"{firstNumber} / {secondNumber} = {firstNumber / secondNumber}");
                    return firstNumber / secondNumber;
            }
            return 0;
        } 
    }
}