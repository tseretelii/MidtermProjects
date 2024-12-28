using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;

namespace MidtermProjects
{
    public static class AtmMachine
    {
        public static BankAccount RegisterAccountForPerson(Person person)
        {
            // Person person = new Person(name, secondName, personalN);
            BankAccount bankAccount = new BankAccount(person);
            try
            {
                Recorder.CreateRecord(person);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Recorder.CreateRecord(bankAccount);
            Console.WriteLine("Code Continues");
            return bankAccount;
        }

        public static Transaction CreateTransaction(BankAccount senderBankAccount, BankAccount reciverBankAccount, decimal amount, Currency currency)
        {
            return new Transaction(senderBankAccount, reciverBankAccount, amount, currency);
        }
    }

    public class Person
    {
        public string PersonalN
        {
            get 
            {
                return _personalN;
            }

            set
            {
                Regex regex = new Regex("^\\d{11}$");
                if (regex.IsMatch(value))
                    _personalN = value;
                else
                    throw new InvalidOperationException("Personal number must be 11 numbers");
            }
        }
        private string _personalN;
        public string Name { get; private set; }
        public string SecondName { get; private set; }
        public DateTime RegisterDate { get; set; }
        public Person(string name, string secondName, string personalN)
        {
            Name = name;
            SecondName = secondName;
            PersonalN = personalN;
        }
    }

    public class BankAccount
    {
        public Person PersonInfo { get; set; }
        public List<AccountIBAN> AccountNumber { get; set; }
        public DateTime RegisterDate { get; set; }
        public BankAccount()
        {
            AccountNumber = new List<AccountIBAN>();
        }
        public BankAccount(Person person)
        {
            PersonInfo = person;
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
        public Transaction() { }
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
            string filePath = string.Concat(DirPath, "\\Transactions.json");

            DirectoryInfo directoryInfo = new DirectoryInfo(DirPath);
            if (!directoryInfo.Exists )
                directoryInfo.Create();

            if (!File.Exists(filePath))
                File.Create(filePath);

            if (File.ReadAllText(filePath) == "")
                File.WriteAllText(filePath,"[]");

            List<Transaction> transactions = JsonSerializer.Deserialize<List<Transaction>>(File.ReadAllText(filePath), new JsonSerializerOptions { IncludeFields = true, WriteIndented = true }) ?? new List<Transaction>();

            transactions.Add(transaction);

            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true }));
                }
            }
        }

        public static void CreateRecord(Person person)
        {
            string filePath = string.Concat(DirPath, "\\Persons.json");

            DirectoryInfo directoryInfo = new DirectoryInfo(DirPath);
            if (!directoryInfo.Exists)
                directoryInfo.Create();

            if (!File.Exists(filePath))
                File.Create(filePath);

            if (File.ReadAllText(filePath) == "")
                File.WriteAllText(filePath, "[]");


            List<Person> persons = JsonSerializer.Deserialize<List<Person>>(File.ReadAllText(filePath)) ?? new List<Person>();

            foreach (Person p in persons)
            {
                if (p.PersonalN == person.PersonalN)
                    throw new ArgumentException("This person allready exists");
            }

            
            person.RegisterDate = DateTime.Now;

            persons.Add(person);

            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(JsonSerializer.Serialize(persons, new JsonSerializerOptions {WriteIndented = true }));
                }
            }
        }

        public static void CreateRecord(BankAccount bankAccount)
        {
            string filePath = string.Concat(DirPath, "\\BankAccount.json");

            DirectoryInfo dirInfo = new DirectoryInfo(DirPath);

            if (!Directory.Exists(filePath))
                dirInfo.Create();
            
            //if (!File.Exists(filePath))
            //    File.Create(filePath);

            if (File.ReadAllText(filePath) == "")
                File.WriteAllText(filePath, "[]");

            List<BankAccount> bankAccounts = JsonSerializer.Deserialize<List<BankAccount>>(File.ReadAllText(filePath), new JsonSerializerOptions { WriteIndented = true }) ?? new List<BankAccount>();

            bankAccount.RegisterDate = DateTime.Now;

            bankAccounts.Add(bankAccount);

            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(JsonSerializer.Serialize(bankAccounts, new JsonSerializerOptions { WriteIndented = true }));
                }
            }
        }
    }
}
