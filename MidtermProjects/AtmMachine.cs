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
        public static BankAccount RegisterAccountForPerson(string name, string secondName, string personalN)
        {
            Person person = new Person(name, secondName, personalN);
            BankAccount bankAccount = new BankAccount(person);
            return bankAccount;
        }

        public static Transaction CreateTransaction(BankAccount senderBankAccount, BankAccount reciverBankAccount, decimal amount, Currency currency)
        {
            return new Transaction(senderBankAccount, reciverBankAccount, amount, currency);
        }

        //public static BankAccount GetBankAccount(UniqueId personId)
        //{
        //    return Recorder.GetPersonFromRecord(personId);
        //}
    }

    public class Person
    {
        public int Id { get; private set; }
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
        internal Person(string name, string secondName, string personalN)
        {
            //Id = IdGenerator();
            Name = name;
            SecondName = secondName;
            PersonalN = personalN;
            Recorder.CreateRecord(this);
        }
        private static int IdGenerator()
        {
            var persons = Recorder.GetPersonFromRecord();
            return persons.Count + 1;
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
        }

        public static void CreateRecord(Person person)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(DirPath);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            using (FileStream fileStream = new FileStream(DirPath + "\\Persons.json", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(JsonSerializer.Serialize(person, new JsonSerializerOptions {WriteIndented = true }));
                    //streamWriter.WriteLine(JsonSerializer.Serialize(person));
                }
            }
        }

        public static List<Person> GetPersonFromRecord()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(DirPath);
            if (!directoryInfo.Exists)
            {
                throw new ArgumentException("Directory doesn't exist");
            }

            FileInfo fileInfo = new FileInfo(DirPath + "\\Persons.json");
            if (!fileInfo.Exists)
            {
                return new List<Person>();
            }

            using (FileStream fileStream = new FileStream(DirPath + "\\Persons.json", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    var text = streamReader.ReadToEnd();
                    Console.WriteLine(text);
                    var list = JsonSerializer.Deserialize<List<Person>>(text);
                    return list;
                }
            }
        }
    }
}
