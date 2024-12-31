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
            //var bank2 = AtmMachine.RegisterAccountForPerson("elene", "tsereteli", "12345678911");
            //var bank1 = AtmMachine.RegisterAccountForPerson("gigi", "tsereteli", "12345678910");

            //bank1.AccountNumber[0].Balance[Currency.GEL] = 100;

            //AtmMachine.CreateTransaction(bank1, bank2, 50, Currency.GEL).ExecuteTransaction(0,0); // The indices 0 and 0 indicate the sender's and receiver's accounts involved in the transaction.

            
            Person person1 = new Person("Gigi", "Tsereteli", "12345678911");
            var acc1 = AtmMachine.RegisterAccountForPerson(person1);
            if (acc1 == null )
            {
                acc1 = Recorder.GetBankAccount(Recorder.GetPerson(person1.PersonalN));
            }
            Person person2 = new Person("Elene", "Tsereteli", "12345678910");
            var acc2 = AtmMachine.RegisterAccountForPerson(person2);
            if (acc2 == null)
            {
                acc2 = Recorder.GetBankAccount(Recorder.GetPerson(person2.PersonalN));
            }

            AtmMachine.DepositFunds(acc1, acc1.AccountNumber[0], 100, Currency.GEL);
            #endregion
        }
    }
}
