using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MidtermProjects
{
    public static class AtmMachineFileManagement
    {
        public static string DirPath { get; private set; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\BankLog";
        public static string PersonsPath { get; private set; } = DirPath + "\\Persons.json";
        public static string BankAccountPath { get; private set; } = DirPath + "\\BankAccount.json";
        public static string TransactionsPath { get; private set; } = DirPath + "\\Transactions.json";
        public static string PersonalInfoPath { get; set; } = DirPath + "\\PersonalInfo.json";
        public static void CreatingDirectoryForProject()
        {
            if (!Directory.Exists(DirPath))
                Directory.CreateDirectory(DirPath);
        }

        public static void CreatingFilesForProject()
        {
            if (!File.Exists(PersonsPath))
            {
                File.Create(PersonsPath);

                File.WriteAllText(PersonsPath, "[]");
            }

            if (!File.Exists(BankAccountPath))
            {
                File.Create(BankAccountPath);

                File.WriteAllText(BankAccountPath, "[]");
            }

            if (!File.Exists(TransactionsPath))
            {
                File.Create(TransactionsPath);

                File.WriteAllText(TransactionsPath, "[]");
            }

            if (!File.Exists(PersonalInfoPath))
            {
                File.Create(PersonalInfoPath);

                File.WriteAllText
                    (
                        PersonalInfoPath,
                        JsonSerializer.Serialize
                            (
                                new PersonalInfo("yourmail@mail.com", "your password")
                            )
                    );
            }
        }

        public static PersonalInfo GetPersonalInfo()
        {
            return JsonSerializer.Deserialize<PersonalInfo>(File.ReadAllText(PersonalInfoPath));
        }
    }

    public class PersonalInfo
    {
        public string EmailAddress { get; set; }
        public string EmailPassword { get; set; }
        public PersonalInfo()
        {
            
        }
        public PersonalInfo(string mail, string pass)
        {
            EmailAddress = mail;
            EmailPassword = pass;
        }
    }
}
