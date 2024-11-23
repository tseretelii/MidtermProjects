namespace MidtermProjects
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // N1
            #region Calculator
            //Console.WriteLine(Calculator.Addition());
            //Console.WriteLine(Calculator.Subtraction());
            //Console.WriteLine(Calculator.Multiplication());
            //Console.WriteLine(Calculator.Division());
            #endregion

            // N2
            #region GuessingGame
            //GuessingGame.StartGame(11);
            #endregion
        }
    }

    // N1
    #region Calculator
    public static class Calculator
    {
        public static double Addition()
        {
            double us1;
            double us2;
            Console.WriteLine("Enter The First Number");
            while (true)
            {
                if (double.TryParse(Console.ReadLine(), out us1))
                    break;
            }
            Console.WriteLine("Enter The Second Number");
            while (true)
            {
                if (double.TryParse(Console.ReadLine(), out us2))
                    break;
            }
            return us1 + us2;
        }
        public static double Subtraction()
        {
            double us1;
            double us2;
            Console.WriteLine("Enter The First Number");
            while (true)
            {
                if (double.TryParse(Console.ReadLine(), out us1))
                    break;
            }
            Console.WriteLine("Enter The Second Number");
            while (true)
            {
                if (double.TryParse(Console.ReadLine(), out us2))
                    break;
            }
            return us1 - us2;
        }
        public static double Multiplication()
        {
            double us1;
            double us2;
            Console.WriteLine("Enter The First Number");
            while (true)
            {
                if (double.TryParse(Console.ReadLine(), out us1))
                    break;
            }
            Console.WriteLine("Enter The Second Number");
            while (true)
            {
                if (double.TryParse(Console.ReadLine(), out us2))
                    break;
            }
            return us1 * us2;
        }
        public static double Division()
        {
            double us1;
            double us2;
            Console.WriteLine("Enter The First Number");
            while (true)
            {
                if (double.TryParse(Console.ReadLine(), out us1))
                    break;
            }
            Console.WriteLine("Enter The Second Number");
            while (true)
            {
                if (double.TryParse(Console.ReadLine(), out us2))
                    break;
            }
            return us1 / us2;
        }
    }
    #endregion

    // N2
    #region GuessingGame
    public class GuessingGame
    {
        public static void StartGame(int range)
        {
            Console.WriteLine("Wellcome To My Guessing Game");
            Random random = new Random();
            int number = random.Next(0, range);

            int nOfGuesses = 0;
            while (true)
            {
                int guess;
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out guess))
                        break;
                    else
                        Console.WriteLine("Invalid Input!");
                }
                if (guess > number)
                {
                    Console.WriteLine("Lower!");
                }
                else if (guess < number)
                {
                    Console.WriteLine("Higher!");
                }
                else if (guess == number)
                {
                    if (nOfGuesses == 0)
                    {
                        Console.WriteLine("You Win, First Try! Incredible");
                    }
                    else
                    {
                        Console.WriteLine($"You Win, The Number Was: {number}\nIt Took You {nOfGuesses + 1 } Guesses");
                    }
                    break;
                }
                nOfGuesses++;
            }
        }
    }
    #endregion

    // N3
    #region Hangman
    public static class Hangman
    {
        private static Random random = new Random();
        private static List<string> easyWords = ["apple", "house", "chair", "table", "river","music", "tiger", "cloud", "green", "happy"];
        private static List<string> mediumWords = ["garden", "puzzle", "forest", "mirror", "ocean", "pencil", "basket", "planet", "secret", "circus"];
        private static List<string> hardWords = ["mountain", "diamond", "thunder", "jewelry", "cactus", "villain", "mystery", "squirrel", "glacier", "ancient"];
        public static void StartGame()
        {
            random.Next(easyWords.Count);
        }
    }
    #endregion
}
