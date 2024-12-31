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
            BankAccount bankAccount = new BankAccount(person);
            try
            {
                Recorder.CreateRecord(person);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default;
            }
            Recorder.CreateRecord(bankAccount);
            return bankAccount;
        }

        public static Transaction CreateTransaction(BankAccount senderBankAccount, BankAccount reciverBankAccount, decimal amount, Currency currency, int senderIndex, int reciverIndex)
        {
            if (senderIndex > senderBankAccount.AccountNumber.Count || reciverIndex > reciverBankAccount.AccountNumber.Count)
                throw new ArgumentException("out of range");
            return new Transaction(senderBankAccount.AccountNumber[senderIndex], reciverBankAccount.AccountNumber[reciverIndex], amount, currency);
        }

        public static void DepositFunds(BankAccount bankAccount, AccountIBAN iban, decimal amount, Currency currency)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\BankLog";
            List<BankAccount> accounts = JsonSerializer.Deserialize<List<BankAccount>>(File.ReadAllText(path + "\\BankAccount.json"));
            if (accounts == null || accounts.Count == 0) throw new Exception("error!");

            for (int i = 0; i < accounts.Count; i++)
            {
                for (int j = 0; accounts[i].AccountNumber.Count > j; j++)
                {
                    if (accounts[i].AccountNumber[j].AccNum == iban.AccNum)
                    {
                        accounts[i].AccountNumber[j].Balance[currency] += amount;
                    }
                }
            }

            string filePath = string.Concat(path, "\\BankAccount.json");

            DirectoryInfo dirInfo = new DirectoryInfo(path);

            if (!Directory.Exists(path))
                dirInfo.Create();

            if (File.ReadAllText(filePath) == "")
                File.WriteAllText(filePath, "[]");


            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(JsonSerializer.Serialize(accounts, new JsonSerializerOptions { WriteIndented = true }));
                }
            }
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
        public AccountIBAN SenderAccount { get; set; }
        public AccountIBAN ReciverAccount { get; set; }
        public decimal Amount { get; set; }
        public Currency Curr { get; set; }
        public Transaction() { }
        public Transaction(AccountIBAN senderAccount, AccountIBAN reciverAccount, decimal amount, Currency curr)
        {
            SenderAccount = senderAccount;
            ReciverAccount = reciverAccount;
            Amount = amount;
            Curr = curr;
        }
        public void ExecuteTransaction()
        {

            if (SenderAccount.Balance[Curr] < Amount)
                throw new Exception("insufficient funds!");


            Recorder.UpdateBankAccountRecord(this);
            
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

        public static Person GetPerson(string personalN)
        {
            var persons = JsonSerializer.Deserialize<List<Person>>(File.ReadAllText(DirPath + "\\Persons.json")) ?? new List<Person>();
            foreach (var item in persons)
            {
                if (item.PersonalN == personalN)
                {
                    return item;
                }
            }
            return default;
        }

        public static BankAccount GetBankAccount(Person person)
        {
            var accounts = JsonSerializer.Deserialize<List<BankAccount>>(File.ReadAllText(DirPath + "\\BankAccount.json")) ?? new List<BankAccount>();

            foreach (var item in accounts)
            {
                if (item.PersonInfo.PersonalN == person.PersonalN)
                {
                    return item;
                }
            }
            return default;
        }

        public static void UpdateBankAccountRecord(Transaction transaction)
        {
            List<BankAccount> accounts = JsonSerializer.Deserialize<List<BankAccount>>(File.ReadAllText(DirPath + "\\BankAccount.json"));

            if (accounts == null || accounts.Count == 0) { return; }

            

            foreach (BankAccount account in accounts)
            {
                if (transaction.SenderAccount.AccNum == account.AccountNumber[account.AccountNumber.IndexOf(transaction.SenderAccount)].AccNum)
                {
                    account.AccountNumber[account.AccountNumber.IndexOf(transaction.SenderAccount)].Balance[transaction.Curr] -= transaction.Amount;
                }
                else if (transaction.ReciverAccount.AccNum == account.AccountNumber[account.AccountNumber.IndexOf(transaction.ReciverAccount)].AccNum)
                {
                    account.AccountNumber[account.AccountNumber.IndexOf(transaction.ReciverAccount)].Balance[transaction.Curr] += transaction.Amount;
                }
            }

            string filePath = string.Concat(DirPath, "\\BankAccount.json");

            DirectoryInfo dirInfo = new DirectoryInfo(DirPath);

            if (!Directory.Exists(filePath))
                dirInfo.Create();

            if (File.ReadAllText(filePath) == "")
                File.WriteAllText(filePath, "[]");


            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(JsonSerializer.Serialize(accounts, new JsonSerializerOptions { WriteIndented = true }));
                }
            }
        }
    }
}
