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

            // N3
            Hangman.StartGame(Hangman.Difficulty.Easy);

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
    // კოდს აქვს შემდეგი პრობლემები / ნიუანსი დასამატებელი
    // სადმე უნდა შევინახო რომელი ასო გამოიცნო უკვე იუზერმა, რათა იმუშაოს ისეთ სიტყვებზეც სადაც ერთი და იგივე ასო რამდენიმეჯერაა
    // დეშის დაბეჭდვის ლოგიკაა გასასწორებელი
    public static class Hangman
    {
        private static Random random = new Random();
        private static List<string> easyWords = ["apple", "house", "chair", "table", "river","music", "tiger", "cloud", "green", "happy"];
        private static List<string> mediumWords = ["garden", "puzzle", "forest", "mirror", "ocean", "pencil", "basket", "planet", "secret", "circus"];
        private static List<string> hardWords = ["mountain", "diamond", "thunder", "jewelry", "cactus", "villain", "mystery", "squirrel", "glacier", "ancient"];
        private static int numberOfTries = 9;
        private static List<string> drawing =
            [
                "|\n|\n|\n|\n",
                " ___\n|\n|\n|\n|\n",
                " ___\n|   |\n|\n|\n|",
                " ___\n|   |\n|   O\n|\n|",
                " ___\n|   |\n|   O\n|  /\n|",
                " ___\n|   |\n|   O\n|  /|\n|",
                " ___\n|   |\n|   O\n|  /|\\\n|",
                " ___\n|   |\n|   O\n|  /|\\\n|  /",
                " ___\n|   |\n|   O\n|  /|\\\n|  / \\"
            ];
        public static void StartGame(Difficulty difficulty)
        {
            Console.WriteLine("Wellcome To The Hangman Game!");
            int tries = numberOfTries;
            int points = 0;
            string wordToGuess = GetWordByDifficulty(difficulty);
            while (tries > 0)
            {
                string input = Console.ReadLine();

                if (!ValidateUserInput(input))
                {
                    Console.WriteLine("Invalid Input");
                    continue;
                }

                
            }
        }

        private static string GetWordByDifficulty(Difficulty difficulty)
        {
            if (difficulty == Difficulty.Easy)
            {
                return easyWords[random.Next(easyWords.Count())];
            }
            else if (difficulty == Difficulty.Medium)
            {
                return mediumWords[random.Next(mediumWords.Count())];
            }
            else
            {
                return hardWords[random.Next(hardWords.Count())];
            }
        }

        private static bool ValidateUserInput(string input)
        {
            return !string.IsNullOrEmpty(input) && input.Length == 1 && char.IsLetter(input[0]);
        }

        private static bool CheckIfUserInputInWord(char userInput, string wordToGuess)
        {
            for (int i = 0; i < wordToGuess.Length; i++)
            {
                if ( userInput == wordToGuess[i])
                    return true;
            }
            return false;
        }

        public enum Difficulty
        {
            Easy,
            Medium,
            Hard
        }
    }
    #endregion
}
