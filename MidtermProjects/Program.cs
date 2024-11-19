namespace MidtermProjects
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Calculator
            // Calculator
            Console.WriteLine("Hello, World!");

            double us1;
            double us2;
            Console.WriteLine("Enter The First Number");
            while (true)
            {
                if (double.TryParse(Console.ReadLine(), out us1))
                    break;
            }
            Console.WriteLine("Enter The Second Number");
            while (true)
            {
                if (double.TryParse(Console.ReadLine(), out us2))
                    break;
            }

            Console.WriteLine(Calculator.Addition(us1, us2));
            #endregion
        }
    }

    #region Calculator
    // Calculator
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
}
