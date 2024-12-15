using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public static void Transaction(BankAccount bankAccount, decimal amount, Currency currency)
        {
            foreach (var acc in bankAccount.AccountNumber)
            {
                if (acc.Key.Value == currency)
                {
                    // continue from here!
                }
            }
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
        public Dictionary<KeyValuePair<string, Currency>, decimal> AccountNumber { get; private set; } = new Dictionary<KeyValuePair<string, Currency>, decimal>();

        public int MyProperty { get; set; }
        internal BankAccount(Person person)
        {
            Person1 = person;
            string accN = AccountNumberGenerator();
            AccountNumber.Add(new KeyValuePair<string, Currency>(accN, Currency.GEL), 0);
            AccountNumber.Add(new KeyValuePair<string, Currency>(accN, Currency.USD), 0);

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

    public enum Currency
    {
        GEL=1,
        USD,
        EUR
    }
}
