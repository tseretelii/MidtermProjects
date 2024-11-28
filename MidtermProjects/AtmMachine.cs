using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidtermProjects
{
    public static class AtmMachine
    {
        public static void RegisterPerson(string name, string secondName, string userName, string password)
        {
            Person person = new Person(name.ToLower(), secondName.ToLower(), userName.ToLower(), password);
        }
    }

    internal class Person
    {
        public string Name { get; private set; }
        public string SecondName { get; private set; }
        public string UserName { get; private set; }
        private string Password { get; set; }
        public string AccountNumber { get; private set; }
        public Person(string name, string secondName, string userName, string password)
        {
            Name = name;
            SecondName = secondName;
            UserName = userName;
            Password = password;
            AccountNumber = SetAccountNumber();
        }

        private string SetAccountNumber()
        {
            Random random = new Random();
            string accountNumber = "";
            for (int i = 0; i < 16; i++)
            {
                accountNumber += random.Next(0, 10).ToString();
            }
            return accountNumber;
        }
    }
}
