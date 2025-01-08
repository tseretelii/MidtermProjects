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
                return Recorder.GetBankAccount(Recorder.GetPerson(person.PersonalN));
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
            
            List<BankAccount> accounts = JsonSerializer.Deserialize<List<BankAccount>>(File.ReadAllText(AtmMachineFileManagement.BankAccountPath));
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


            using (FileStream fileStream = new FileStream(AtmMachineFileManagement.BankAccountPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
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
                    throw new ArgumentException("Personal number must be 11 numbers");
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
        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                Regex regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,}$");
                if (regex.IsMatch(value))
                    _password = value;
                else
                    throw new ArgumentException("Password must be:\n- Must be at least 8 characters long\n- Must include at least 1 lowercase letter (a-z)\n- Must include at least 1 uppercase letter (A-Z)\n- Must include at least 1 number (0-9)");
            }
        }
        public List<AccountIBAN> AccountNumber { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsVerified { get; set; }
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
        public static void CreateRecord(Transaction transaction)
        {

            List<Transaction> transactions = JsonSerializer.Deserialize<List<Transaction>>(File.ReadAllText(AtmMachineFileManagement.TransactionsPath), new JsonSerializerOptions { IncludeFields = true, WriteIndented = true }) ?? new List<Transaction>();

            transactions.Add(transaction);

            using (FileStream fileStream = new FileStream(AtmMachineFileManagement.TransactionsPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true }));
                }
            }
        }

        public static void CreateRecord(Person person)
        {
            List<Person> persons = JsonSerializer.Deserialize<List<Person>>(File.ReadAllText(AtmMachineFileManagement.PersonsPath)) ?? new List<Person>();

            foreach (Person p in persons)
            {
                if (p.PersonalN == person.PersonalN)
                    throw new ArgumentException("This person allready exists");
            }

            
            person.RegisterDate = DateTime.Now;

            persons.Add(person);

            using (FileStream fileStream = new FileStream(AtmMachineFileManagement.PersonsPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(JsonSerializer.Serialize(persons, new JsonSerializerOptions {WriteIndented = true }));
                }
            }
        }

        public static void CreateRecord(BankAccount bankAccount)
        {

            List<BankAccount> bankAccounts = JsonSerializer.Deserialize<List<BankAccount>>(File.ReadAllText(AtmMachineFileManagement.BankAccountPath), new JsonSerializerOptions { WriteIndented = true }) ?? new List<BankAccount>();

            bankAccount.RegisterDate = DateTime.Now;

            bankAccounts.Add(bankAccount);

            using (FileStream fileStream = new FileStream(AtmMachineFileManagement.BankAccountPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(JsonSerializer.Serialize(bankAccounts, new JsonSerializerOptions { WriteIndented = true }));
                }
            }
        }

        public static Person GetPerson(string personalN)
        {
            var persons = JsonSerializer.Deserialize<List<Person>>(File.ReadAllText(AtmMachineFileManagement.PersonsPath)) ?? new List<Person>();
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

            if (person == null)
                return default;

            var accounts = JsonSerializer.Deserialize<List<BankAccount>>(File.ReadAllText(AtmMachineFileManagement.BankAccountPath)) ?? new List<BankAccount>();

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
            List<BankAccount> accounts = JsonSerializer.Deserialize<List<BankAccount>>(File.ReadAllText(AtmMachineFileManagement.BankAccountPath));

            if (accounts == null || accounts.Count == 0) { return; }

            
            for (int i = 0; i < accounts.Count; i++)
            {
                for (int j = 0; j < accounts[i].AccountNumber.Count; j++)
                {
                    if (accounts[i].AccountNumber[j].AccNum == transaction.SenderAccount.AccNum)
                    {
                        accounts[i].AccountNumber[j].Balance[transaction.Curr] -= transaction.Amount;
                        continue;
                    }
                    else if (accounts[i].AccountNumber[j].AccNum == transaction.ReciverAccount.AccNum)
                    {
                        accounts[i].AccountNumber[j].Balance[transaction.Curr] += transaction.Amount;
                    }
                }
            }


            using (FileStream fileStream = new FileStream(AtmMachineFileManagement.BankAccountPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(JsonSerializer.Serialize(accounts, new JsonSerializerOptions { WriteIndented = true }));
                }
            }
        }
    }
}
