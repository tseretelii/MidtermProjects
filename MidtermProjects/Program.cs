namespace MidtermProjects
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // N1
            #region Calculator
            //Console.WriteLine("Hello, World!");

            //double us1;
            //double us2;
            //Console.WriteLine("Enter The First Number");
            //while (true)
            //{
            //    if (double.TryParse(Console.ReadLine(), out us1))
            //        break;
            //}
            //Console.WriteLine("Enter The Second Number");
            //while (true)
            //{
            //    if (double.TryParse(Console.ReadLine(), out us2))
            //        break;
            //}

            //Console.WriteLine(Calculator.Addition(us1, us2));
            #endregion

            // N2
            GuessingGame.StartGame(5);
        }
    }

    // N1
    #region Calculator
    public static class Calculator
    {
        public static double Addition(double x, double y)
        {
            return x + y;
        }
        public static double Subtraction(double x, double y)
        {
            return x - y;
        }
        public static double Multiplication(double x, double y)
        {
            return x * y;
        }
        public static double Division(double x, double y)
        {
            return x / y;
        }
    }
    #endregion

    // N2
    public class GuessingGame
    {
        public static void StartGame(int range)
        {
            Random random = new Random();
            int number = random.Next(0, range);

            int guess;
            while (!int.TryParse(Console.ReadLine(), out guess) || guess != number)
            {
                Console.WriteLine("Try Again");
            }
            Console.WriteLine($"You Win, The Number Was: {number}");
        }
    }
}
