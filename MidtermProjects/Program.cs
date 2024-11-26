using Microsoft.Win32.SafeHandles;
using System.Text;
using System.Text.Json;

namespace MidtermProjects
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // N1
            #region Calculator
            Console.WriteLine(Calculator.Addition());
            Console.WriteLine(Calculator.Subtraction());
            Console.WriteLine(Calculator.Multiplication());
            Console.WriteLine(Calculator.Division());
            #endregion

            // N2
            #region GuessingGame
            GuessingGame.StartGame(11);
            #endregion

            // N3
            #region Hangman
            Hangman.StartGame(Hangman.Difficulty.Hard);
            #endregion

            // N4 OOP
            #region წიგნების სია

            BookManager.AddBook("lord of the rings", "j.r.r Tolkein", DateOnly.FromDateTime(DateTime.Now));
            BookManager.GetAllBooks();
            BookManager.GetBookByName("lord of the rings");

            #endregion

            
        }
    }
}
