using System.Text.Json;

namespace TestingFeatures
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");

            //Person person1 = new Person("Gigi", "Tsereteli");
            //Person person2 = new Person("Elene", "Tsereteli");

            //JsonFileSaver.Save(person1);
            //JsonFileSaver.Save(person2);


            List<Dictionary<string, decimal>> accounts = new List<Dictionary<string, decimal>>()
            {
                new Dictionary<string, decimal>()
                {
                    { "123", 0 },
                    { "321", 0 }
                }
            };

            accounts[0]["123"] += 3;
            accounts[0]["321"] += 3;

            Console.WriteLine(JsonSerializer.Serialize<List<Dictionary<string, decimal>>>(accounts));
        }
    }
}
