using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidtermProjects
{
    public static class Calculator
    {
        private static double ValidatedNumber()
        {
            Console.WriteLine("Enter The Number");
            double value;
            while (true)
            {
                if (double.TryParse(Console.ReadLine(), out value))
                    return value;
                Console.WriteLine("Invalid Input");
            }
        }
        public static void UserInterface()
        {
            Console.WriteLine("Wellcome To the calculator\nDo you want to perform single or multiple operations?\n1 -- Single\n2 -- Multiple\n(enter the digit)");
            int singleOrMultiple;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out singleOrMultiple) && singleOrMultiple > 0 && singleOrMultiple < 3)
                    break;
                Console.WriteLine("Invalid Input");
            }
            if (singleOrMultiple == 1)
                SingleOperation();
            else
                MultipleOperations();
        }
        public static void MultipleOperations()
        {
            double total = 0;
            while (true)
            {
                Console.WriteLine($"Which operation do you want to perform?\n1 -- +\n2 -- -\n3 -- *\n4 -- /\n5 -- Exit\nTotal: {total}\n(enter the digit 1, 2, 3, 4 or 5)");
                int operation;
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out operation) && operation > 0 && operation < 6)
                        break;
                    Console.WriteLine("Invalid Input");
                }

                if (operation == 1)
                {
                    total += ValidatedNumber();
                }
                else if (operation == 2)
                {
                    total -= ValidatedNumber();
                }
                else if (operation == 3)
                {
                    total *= ValidatedNumber();
                }
                else if (operation == 4)
                {
                    total /= ValidatedNumber();
                }
                else
                {
                    return;
                }
            }
        }
        public static void SingleOperation()
        {
            Console.WriteLine($"Which operation do you want to perform?\n1 -- +\n2 -- -\n3 -- *\n4 -- /\n5 -- Exit\n(enter the digit 1, 2, 3, 4 or 5)");
            int operation;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out operation) && operation > 0 && operation < 6)
                    break;
                Console.WriteLine("Invalid Input");
            }
            if (operation == 1)
            {
                Console.WriteLine($"Answer: {ValidatedNumber() + ValidatedNumber()}");
            }
            else if (operation == 2)
            {
                Console.WriteLine($"Answer: {ValidatedNumber() - ValidatedNumber()}");
            }
            else if (operation == 3)
            {
                Console.WriteLine($"Answer: {ValidatedNumber() * ValidatedNumber()}");
            }
            else if (operation == 4)
            {
                double num1 = ValidatedNumber();
                double num2 = ValidatedNumber();
                while(num2 == 0)
                {
                    num2 = ValidatedNumber();
                    Console.WriteLine("Second Value Must Not Be 0");
                }
                Console.WriteLine($"Answer: {num1 / num2}");
            }
            else
            {
                return ;
            }
        }
    }
}
