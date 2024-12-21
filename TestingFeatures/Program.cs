namespace TestingFeatures
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            Person person1 = new Person("Gigi", "Tsereteli");
            Person person2 = new Person("Elene", "Tsereteli");

            JsonFileSaver.Save(person1);
            JsonFileSaver.Save(person2);
        }
    }
}
