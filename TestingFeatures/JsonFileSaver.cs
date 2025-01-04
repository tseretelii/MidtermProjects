using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TestingFeatures
{
    public static class JsonFileSaver
    {
        private static DirectoryInfo Dir { get; set; } = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\BankLog\\TestFolder");

        public static void Save(Person person)
        {
            if (!Dir.Exists)
                Dir.Create();

            string filePath = Path.Combine(Dir.FullName, "persons.json");

            if (!File.Exists(filePath))
                File.WriteAllText(filePath, "[]");

            var persons = JsonSerializer.Deserialize<List<Person>>(File.ReadAllText(filePath)) ?? new List<Person>();
            persons.Add(person);
            File.WriteAllText(filePath, JsonSerializer.Serialize(persons, new JsonSerializerOptions { WriteIndented = true }));


        }
    }

    public class Person
    {
        public string Name { get; set; }
        public string SecondName { get; set; }
        public Person(string name, string secondName)
        {
            Name = name;
            SecondName = secondName;
        }
    }
}
