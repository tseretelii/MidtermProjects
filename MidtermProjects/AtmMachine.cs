using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MidtermProjects
{
    public static class AtmMachine
    {
        public static BankAccount RegisterAccountForPerson(string name, string secondName)
        {
            Person person = new Person(name, secondName);
            BankAccount bankAccount = new BankAccount(person);
            return bankAccount;
        }

        public static Transaction CreateTransaction(BankAccount senderBankAccount, BankAccount reciverBankAccount, decimal amount, Currency currency)
        {
            return new Transaction(senderBankAccount, reciverBankAccount, amount, currency);
        }
    }

    public class Person
    {
        public string Name { get; private set; }
        public string SecondName { get; private set; }
        internal Person(string name, string secondName)
        {
            Name = name;
            SecondName = secondName;
        }
    }
    public class BankAccount
    {
        public Person Person1 { get; set; }
        public List<AccountIBAN> AccountNumber { get; set; }
        internal BankAccount(Person person)
        {
            Person1 = person;
            AccountNumber = [new AccountIBAN()];
        }
    }

    public enum Currency
    {
        GEL=1,
        USD,
        EUR
    }

    public class Transaction
    {
        public BankAccount SenderAccount { get; set; }
        public BankAccount ReciverAccount { get; set; }
        public decimal Amount { get; set; }
        public Currency Curr { get; set; }
        public Transaction(BankAccount senderAccount, BankAccount reciverAccount, decimal amount, Currency curr)
        {
            SenderAccount = senderAccount;
            ReciverAccount = reciverAccount;
            Amount = amount;
            Curr = curr;
        }
        public void ExecuteTransaction(int senderIndex, int reciverIndex)
        {
            if (senderIndex > SenderAccount.AccountNumber.Count || reciverIndex > ReciverAccount.AccountNumber.Count)
                throw new ArgumentException("out of range");
            
            var senderAccAmount = from acc1 in SenderAccount.AccountNumber[senderIndex].Balance
                                      where acc1.Key == Curr
                                      select acc1.Value;

            if (senderAccAmount.ToList()[0] < Amount)
                throw new Exception("Insufficient funds");

            SenderAccount.AccountNumber[senderIndex].Balance[Curr] -= Amount;
            ReciverAccount.AccountNumber[reciverIndex].Balance[Curr] += Amount;

            Recorder.CreateRecord(this);
        }
    }

    public class AccountIBAN
    {
        public string AccNum { get; set; }
        public Dictionary<Currency, decimal> Balance { get; set; }
        public AccountIBAN()
        {
            AccNum = AccountNumberGenerator();
            Balance = new Dictionary<Currency, decimal>()
            {
                {Currency.GEL, 0 },
                {Currency.USD, 0 },
                {Currency.EUR, 0 },
            };
        }
        private string AccountNumberGenerator()
        {
            Random random = new Random();
            string accountNumber = "";
            for (int i = 0; i < 16; i++)
            {
                accountNumber += random.Next(1, 10).ToString();
            }
            return accountNumber;
        }
    }

    public static class Recorder
    {
        public static string DirPath { get; private set; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\BankLog";

        public static void CreateRecord(Transaction transaction)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(DirPath);
            if (!directoryInfo.Exists )
            {
                directoryInfo.Create();
            }

            using (FileStream fileStream = new FileStream(DirPath + "\\Transactions.json", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.WriteLine(JsonSerializer.Serialize(transaction));
                }
            }
            Console.WriteLine("Complete!");
        }
    }
}
