using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MidtermProjects
{
    public static class AtmMachineInterface
    {
        public static void StartAtmMachine()
        {
            string userChoice = Wellcome();
            BankAccount account;

            if (userChoice == "1")
                account = UserLogIn();
            else
                account = UserRegister();

            if (account == null)
                return;

            while (true)
            {
                userChoice = UserMenu();

                if (userChoice == "1")
                    CheckBalance(account);
                else if (userChoice == "2")
                    DepositMoney(account);
                else if (userChoice == "3")
                    Transfer(account);
                else
                    break;
            }

        }

        private static string Wellcome()
        {
            Console.WriteLine("Wellcome To LTD Gigi's Bank\n1. Login\n2. Register");
            return RegexForInput.GetCheckedUserInput("^[12]$", "1. Login\n2. Register");
        }

        private static string UserMenu()
        {
            Console.WriteLine("What Would You Like To Do?\n1. Check Balance \n2. Deposit Money\n3. Transfer\n4. Log Out");

            return RegexForInput.GetCheckedUserInput("^[1-4]$", "1. Check Balance \n2. Deposit Money\n3. Transfer\n4. Log Out");
        }

        // For now user logs in with their name and personal number
        private static BankAccount UserLogIn()
        {

            Console.WriteLine("Please enter your name\n Name: ");

            string name = RegexForInput.GetCheckedUserInput("^[a-zA-Z]+$", "Name: ");

            Console.WriteLine("Please enter your personal number: ");

            string personalN = RegexForInput.GetCheckedUserInput("^\\d{11}$", "Personal number: ");
            
            return Recorder.GetBankAccount(Recorder.GetPerson(personalN)); // if user doesn't exist i have to handle it!!!!
        }

        private static BankAccount UserRegister()
        {
            Console.WriteLine("Please enter your name\nName: ");

            string name = RegexForInput.GetCheckedUserInput("^[a-zA-Z]+$", "Name: ");

            Console.WriteLine("Enter Your Second Name");

            string surName = RegexForInput.GetCheckedUserInput("^[a-zA-Z]+$", "Second Name: ");

            Console.WriteLine("Enter Your Personal Number");

            string personalN = RegexForInput.GetCheckedUserInput("^\\d{11}$", "Personal number: ");

            Person person = new Person(name, surName, personalN);

            return AtmMachine.RegisterAccountForPerson(person);
        }

        private static void CheckBalance(BankAccount account)
        {
            BankAccount bankAccount = Recorder.GetBankAccount(account.PersonInfo);
            foreach (AccountIBAN iban in bankAccount.AccountNumber)
            {
                foreach (KeyValuePair<Currency, decimal> curr in iban.Balance)
                {
                    Console.WriteLine($"{curr.Key} - {curr.Value}");
                }
            }
        }

        private static void DepositMoney(BankAccount account)
        {
            Console.WriteLine("Which IBAN?");
            
            for (int i = 1; i < account.AccountNumber.Count + 1; i++)
            {
                Console.WriteLine($"{i}. {account.AccountNumber[i - 1].AccNum}");
            }

            int ibanIndex = int.Parse(RegexForInput.GetCheckedUserInput($"^[1-{account.AccountNumber.Count}]$"));

            ibanIndex--;

            Console.WriteLine("Please enter the amount:");

            decimal amount = decimal.Parse(RegexForInput.GetCheckedUserInput("^-?\\d+(\\.\\d+)?$"));

            var keys = account.AccountNumber[ibanIndex].Balance.Keys.ToList();

            for (int i = 1;i < account.AccountNumber[ibanIndex].Balance.Keys.Count + 1; i++)
            {
                Console.WriteLine($"{i}. {account.AccountNumber[ibanIndex].Balance.Keys.ToList()[i - 1]}");
            }

            int currIndex = int.Parse(RegexForInput.GetCheckedUserInput($"^[1-{account.AccountNumber[ibanIndex].Balance.Keys.Count}]$"));

            AtmMachine.DepositFunds(account, account.AccountNumber[ibanIndex], amount, account.AccountNumber[ibanIndex].Balance.Keys.ToList()[currIndex - 1]);
            
            Console.WriteLine("Complete!");
        }

        private static void Transfer(BankAccount senderAccount)
        {
            Console.WriteLine("To whom would you like to transfer funds?\nPlease enter personal number: ");

            string personalN = RegexForInput.GetCheckedUserInput("^\\d{11}$", "Please enter personal number: ");

            BankAccount reciverAccount = Recorder.GetBankAccount(Recorder.GetPerson(personalN));

            if (reciverAccount == null)
            {
                Console.WriteLine("Person Not Found!");
                return;
            }

            Console.WriteLine("From Which IBAN?");

            for (int i = 1; i < senderAccount.AccountNumber.Count + 1; i++)
            {
                Console.WriteLine($"{i}. {senderAccount.AccountNumber[i - 1].AccNum}");
            }

            int senderIbanIndex = int.Parse(RegexForInput.GetCheckedUserInput($"^[1-{senderAccount.AccountNumber.Count}]$"));

            senderIbanIndex--;


            var keys = senderAccount.AccountNumber[senderIbanIndex].Balance.Keys.ToList();

            for (int i = 1; i < senderAccount.AccountNumber[senderIbanIndex].Balance.Keys.Count + 1; i++)
            {
                Console.WriteLine($"{i}. {senderAccount.AccountNumber[senderIbanIndex].Balance.Keys.ToList()[i - 1]}");
            }

            int currIndex = int.Parse(RegexForInput.GetCheckedUserInput($"^[1-{senderAccount.AccountNumber[senderIbanIndex].Balance.Keys.Count}]$"));


            Console.WriteLine("Please enter the amount:");

            decimal amount = decimal.Parse(RegexForInput.GetCheckedUserInput("^-?\\d+(\\.\\d+)?$"));


            Console.WriteLine("To Which IBAN?");

            for (int i = 1; i < reciverAccount.AccountNumber.Count + 1; i++)
            {
                Console.WriteLine($"{i}. {reciverAccount.AccountNumber[i - 1].AccNum}");
            }

            int reciverIbanIndex = int.Parse(RegexForInput.GetCheckedUserInput($"^[1-{reciverAccount.AccountNumber.Count}]$"));

            reciverIbanIndex--;



            AtmMachine.CreateTransaction(senderAccount, reciverAccount,amount, senderAccount.AccountNumber[senderIbanIndex].Balance.Keys.ToList()[currIndex - 1], senderIbanIndex, reciverIbanIndex).ExecuteTransaction();
            Console.WriteLine("Complete!");
        }
    }

    internal static class RegexForInput
    {
        internal static string GetCheckedUserInput(string regex, string userChoicesForInvalidInput = "")
        {
            Regex r = new Regex(regex);

            string userInput;

            while (true)
            {
                userInput = Console.ReadLine();
                if (r.IsMatch(userInput))
                {
                    return userInput;
                }
                Console.WriteLine("Invalid Input\nTry Again: " + "\n" + userChoicesForInvalidInput);
            }

        }
    }
}
