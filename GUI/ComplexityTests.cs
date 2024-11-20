using System;

namespace JustLabel.ComplexityTests
{
    public class ComplexityTests
    {
        // Простой метод с цикломой сложностью 1
        public void SimpleMethod()
        {
            Console.WriteLine("This method is simple.");
        }

        // Метод с цикломой сложностью 2
        public void TwoBranchesMethod(bool condition)
        {
            if (condition)
            {
                Console.WriteLine("Condition is true.");
            }
            else
            {
                Console.WriteLine("Condition is false.");
            }
        }

        // Метод с цикломой сложностью 3
        public void ThreeBranchesMethod(int number)
        {
            if (number < 0)
            {
                Console.WriteLine("Negative number");
            }
            else if (number == 0)
            {
                Console.WriteLine("Zero");
            }
            else
            {
                Console.WriteLine("Positive number");
            }
        }

        // Метод с цикломой сложностью 4
        public void ComplexMethod(int a, int b)
        {
            if (a > b)
            {
                Console.WriteLine("A is greater than B");
            }
            else if (a == b)
            {
                Console.WriteLine("A is equal to B");
            }
            else
            {
                Console.WriteLine("B is greater than A");
            }

            if (a > 10)
            {
                Console.WriteLine("A is greater than 10");
            }
        }

        // Метод с цикломой сложностью 5
        public void VeryComplexMethod(int a, int b, int c)
        {
            if (a > b)
            {
                if (b > c)
                {
                    Console.WriteLine("A > B > C");
                }
                else
                {
                    Console.WriteLine("A > B, but not > C");
                }
            }
            else if (b < c)
            {
                Console.WriteLine("B < C");
            }
            else
            {
                Console.WriteLine("A <= B");
            }

            if (a < 0)
            {
                Console.WriteLine("A is negative");
            }
            else if (b < 0)
            {
                Console.WriteLine("B is negative");
            }
            else if (c < 0)
            {
                Console.WriteLine("C is negative");
            }
        }

        // Метод с цикломой сложностью 6
        public void ComplexityLevel6(int x)
        {
            if (x > 10)
            {
                Console.WriteLine("x is greater than 10");
            }
            else if (x > 5)
            {
                Console.WriteLine("x is greater than 5 but less than or equal to 10");
            }
            else if (x == 5)
            {
                Console.WriteLine("x is equal to 5");
            }
            else
            {
                Console.WriteLine("x is less than 5");
            }

            if (x % 2 == 0)
            {
                Console.WriteLine("x is even");
            }
            else
            {
                Console.WriteLine("x is odd");
            }
        }

        // Метод с цикломой сложностью 7
        public void ComplexityLevel7(int a, int b)
        {
            if (a > b)
            {
                Console.WriteLine("A is greater than B");
                if (b < 0)
                {
                    Console.WriteLine("B is negative");
                }
                else
                {
                    Console.WriteLine("B is non-negative");
                }
            }
            else if (a == b)
            {
                Console.WriteLine("A is equal to B");
            }
            else
            {
                Console.WriteLine("B is greater than A");
            }

            if (a % 2 == 0)
            {
                Console.WriteLine("A is even");
            }
            else
            {
                Console.WriteLine("A is odd");
            }
        }

        // Метод с цикломой сложностью 8
        public void ComplexityLevel8(int x, int y)
        {
            if (x > y)
            {
                Console.WriteLine("X is greater than Y");
            }
            else if (x < y)
            {
                Console.WriteLine("Y is greater than X");
            }
            else
            {
                Console.WriteLine("X is equal to Y");
            }

            if (x < 0)
            {
                Console.WriteLine("X is negative");
            }
            else if (x > 0)
            {
                Console.WriteLine("X is positive");
            }

            if (y < 0)
            {
                Console.WriteLine("Y is negative");
            }
            else if (y > 0)
            {
                Console.WriteLine("Y is positive");
            }
        }

        // Метод с цикломой сложностью 9
        public void ComplexityLevel9(int a, int b, int c)
        {
            if (a > b)
            {
                if (b > c)
                {
                    Console.WriteLine("A > B > C");
                }
                else
                {
                    Console.WriteLine("A > B, C is greater or equal");
                }

                if (a > 10)
                {
                    Console.WriteLine("A is greater than 10");
                }
            }
            else if (b > c)
            {
                Console.WriteLine("B > C");
            }
            else
            {
                Console.WriteLine("C is greatest");
            }

            if (a < 0)
            {
                Console.WriteLine("A is negative");
            }
            else if (b < 0)
            {
                Console.WriteLine("B is negative");
            }
            else if (c < 0)
            {
                Console.WriteLine("C is negative");
            }
            else
            {
                Console.WriteLine("All are non-negative");
            }
        }

        // Метод с цикломой сложностью 10
        public void ComplexityLevel10(int a, int b, int c, int d)
        {
            if (a > b)
            {
                if (b > c)
                {
                    if (c > d)
                    {
                        Console.WriteLine("A > B > C > D");
                    }
                    else
                    {
                        Console.WriteLine("A > B > C, but D is greater or equal");
                    }
                }
                else
                {
                    Console.WriteLine("A > B, B is not greater than C");
                }
            }
            else if (b > c)
            {
                if (c > d)
                {
                    Console.WriteLine("B > C > D");
                }
                else
                {
                    Console.WriteLine("B > C, but D is greater or equal");
                }
            }
            else
            {
                Console.WriteLine("C is greatest or A and B are equal to each other");
            }

            if (a % 2 == 0)
            {
                Console.WriteLine("A is even");
            }
            else
            {
                Console.WriteLine("A is odd");
            }

            if (b % 2 == 0)
            {
                Console.WriteLine("B is even");
            }
            else
            {
                Console.WriteLine("B is odd");
            }

            if (c % 2 == 0)
            {
                Console.WriteLine("C is even");
            }
            else
            {
                Console.WriteLine("C is odd");
            }

            if (d % 2 == 0)
            {
                Console.WriteLine("D is even");
            }
            else
            {
                Console.WriteLine("D is odd");
            }
        }
    }
}
