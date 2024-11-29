using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidtermProjects
{
    public static class AtmMachine
    {
        public static void RegisterAccountForPerson(string name, string secondName)
        {
            Person person = new Person(name, secondName);
            BankAccount bankAccount = new BankAccount(person);
        }

        //public static void DepositMoney()
    }

    internal class Person
    {
        public string Name { get; private set; }
        public string SecondName { get; private set; }
        public Person(string name, string secondName)
        {
            Name = name;
            SecondName = secondName;
        }
    }
    internal class BankAccount
    {
        public Person Person1 { get; set; }
        public Dictionary<string, decimal> AccountNumber { get; private set; }
        public int MyProperty { get; set; }
        public BankAccount(Person person)
        {
            Person1 = person;
            string accN = AccountNumberGenerator();
            AccountNumber.Add(accN + "USD",0);
            AccountNumber.Add(accN + "EUR",0);
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
}
