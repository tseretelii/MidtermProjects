using Microsoft.Win32.SafeHandles;

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
            Hangman.StartGame(Hangman.Difficulty.Hard);

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

        private static Dictionary<string, string> easyWords = new Dictionary<string, string>
        {
            { "apple", "A sweet fruit often red or green in color." },
            { "house", "A building where people live." },
            { "chair", "A piece of furniture for sitting." },
            { "table", "A flat surface supported by legs, used for various purposes." },
            { "river", "A large stream of flowing water." },
            { "music", "Sounds arranged in a pleasing or rhythmic way." },
            { "tiger", "A large wild cat with orange fur and black stripes." },
            { "cloud", "A visible mass of water vapor in the sky." },
            { "green", "The color of grass and many plants." },
            { "happy", "A feeling of joy or contentment." }
        };

        private static Dictionary<string, string> mediumWords = new Dictionary<string, string>
        {
            { "garden", "An area for growing plants or flowers." },
            { "puzzle", "A game or problem that tests ingenuity." },
            { "forest", "A large area covered with trees." },
            { "mirror", "A surface that reflects images." },
            { "ocean", "A vast body of salt water covering much of the Earth." },
            { "pencil", "A tool for writing or drawing, typically made of wood." },
            { "basket", "A container made of woven materials." },
            { "planet", "A celestial body orbiting a star." },
            { "secret", "Something kept hidden or unknown." },
            { "circus", "A traveling show with performers and animals." }
        };

        private static Dictionary<string, string> hardWords = new Dictionary<string, string>
        {
            { "mountain", "A large natural elevation of the Earth's surface." },
            { "diamond", "A precious gemstone made of carbon." },
            { "thunder", "The sound caused by lightning." },
            { "jewelry", "Decorative items made of precious metals and stones." },
            { "cactus", "A spiny desert plant that stores water." },
            { "villain", "A character who opposes the hero in a story." },
            { "mystery", "Something that is difficult to understand or explain." },
            { "squirrel", "A small rodent with a bushy tail." },
            { "glacier", "A large, slow-moving mass of ice." },
            { "ancient", "Something very old, from a distant past." }
        };

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

        private static int numberOfTries = 9;
        public static void StartGame(Difficulty difficulty)
        {
            // Get the word from predefined list
            var wordAndDescription = GetWordByDifficulty(difficulty);
            var wordToGuess = wordAndDescription.Key;
            var description = wordAndDescription.Value;

            // Wellcome the usr and explain the word to them
            Console.WriteLine("Wellcome To The Hangman Game!");
            Console.WriteLine($"Here Is The Explanation Of The Word You Have To Guess {description}\nGood Luck!");

            // determine the number of tries and the length of the dashboard
            int tries = numberOfTries;
            char[] guessedCharByUser = Enumerable.Repeat('_', wordToGuess.Length).ToArray();


            while (tries > 0 && string.Join("", guessedCharByUser) != wordToGuess)
            {
                Console.WriteLine("\nEnter a letter:");
                string input = Console.ReadLine();

                if (!ValidateUserInput(input)) { Console.WriteLine("Invalid Input"); continue; }

                bool letterGuessed = false;

                for (int i = 0; i < wordToGuess.Length; i++)
                {
                    if (wordToGuess[i] == input[0])
                    {
                        letterGuessed = true;
                        guessedCharByUser[i] = input[0];
                    }
                }
                if (!letterGuessed)
                {
                    Console.WriteLine("Wrong!");
                    Console.WriteLine(drawing[drawing.Count - tries]);
                    tries--;
                }
                else
                    Console.WriteLine("\nCorrect!\n");
                guessedCharByUser.ToList().ForEach(Console.Write);
            }

            if (tries == 0)
                Console.WriteLine($"\nYou lost the word was {wordToGuess}");
            else
                Console.WriteLine("\nYou Won!");
        }

        private static KeyValuePair<string, string> GetWordByDifficulty(Difficulty difficulty)
        {
            if (difficulty == Difficulty.Easy)
            {
                return easyWords.ElementAt(random.Next(easyWords.Count));
            }
            else if (difficulty == Difficulty.Medium)
            {
                return mediumWords.ElementAt(random.Next(mediumWords.Count));
            }

            return hardWords.ElementAt(random.Next(hardWords.Count));
        }

        private static bool ValidateUserInput(string input)
        {
            return !string.IsNullOrEmpty(input) && input.Length == 1 && char.IsLetter(input[0]);
        }

        public enum Difficulty
        {
            Easy,
            Medium,
            Hard
        }
    }
    #endregion


    // OOP
    #region წიგნების სია
    public class Book
    {
        public Book(string title, string author, DateOnly releaseYear)
        {
            _title = title;
            _author = author;
            _releaseYear = releaseYear;
        }
        private string _title;
        private string _author;
        private DateOnly _releaseYear;
    }

    public class BookManager
    {

        public List<Book> Books { get; private set; }
        private static string _path = "C:\\Users\\Gigi\\Desktop\\BookFolder";
        private static DirectoryInfo _directory = new DirectoryInfo(_path);

        public void AddBook(string title, string author, DateOnly releaseYear)
        {
            Books.Add(new Book(title, author, releaseYear));
            // SaveToFile(Books[Books.Count-1])
        }

        private void SaveToFile(Book book)
        {
            if (!_directory.Exists)
            {
                _directory.Create();
            }

        }
    }
    #endregion
}
