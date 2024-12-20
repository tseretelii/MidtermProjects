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
            //Calculator.UserInterface();
            #endregion

            // N2
            #region GuessingGame
            //GuessingGame.StartGame(11);
            #endregion

            // N3
            #region Hangman
            //Hangman.StartGame(Hangman.Difficulty.Hard);
            #endregion

            // N4 OOP
            #region წიგნების სია
            //BookManager.UserInterface();
            #endregion

            #region ATM
            var bank2 = AtmMachine.RegisterAccountForPerson("elene", "tsereteli", "12345678911");
            var bank1 = AtmMachine.RegisterAccountForPerson("gigi", "tsereteli", "12345678910");

            bank1.AccountNumber[0].Balance[Currency.GEL] = 100;

            AtmMachine.CreateTransaction(bank1, bank2, 50, Currency.GEL).ExecuteTransaction(0,0);
            #endregion
        }
    }
}
