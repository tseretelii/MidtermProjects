using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidtermProjects
{
    public class Hangman
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
            Console.WriteLine($"Here Is The Explanation Of The Word You Have To Guess: {description}\nGood Luck!");

            // determine the number of tries and the length of the dashboard
            int tries = numberOfTries;
            char[] guessedCharByUser = Enumerable.Repeat('_', wordToGuess.Length).ToArray();

            guessedCharByUser.ToList().ForEach(Console.Write);

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
}
