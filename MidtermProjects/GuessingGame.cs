using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidtermProjects
{
    public static class GuessingGame
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
                if (guess >= range || guess < 0)
                {
                    Console.WriteLine("Out of range!");
                    continue;
                }
                else if (guess > number)
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
                        Console.WriteLine($"You Win, The Number Was: {number}\nIt Took You {nOfGuesses + 1} Guesses");
                    }
                    break;
                }
                nOfGuesses++;
            }
        }
    }
}
