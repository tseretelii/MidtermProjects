using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidtermProjects
{
    public static class AtmMachineFileManagement
    {
        public static string DirPath { get; private set; } = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\BankLog";
        public static string PersonsPath { get; private set; } = DirPath + "\\Persons.json";
        public static string BankAccountPath { get; private set; } = DirPath + "\\BankAccount.json";
        public static string TransactionsPath { get; private set; } = DirPath + "\\Transactions.json";
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
        }
    }
}
