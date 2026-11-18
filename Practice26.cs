using System;
using System.Collections.Generic;

namespace BuiltInDelegatesPractice
{
    class Program
    {
        // =====================================================================
        // ЗАГАЛЬНЕ ЗАВДАННЯ: Фільтрація списку через Predicate<int>
        // =====================================================================

        // Метод приймає список чисел та стандартний делегат Predicate<int>
        static List<int> FilterList(List<int> numbers, Predicate<int> condition)
        {
            List<int> result = new List<int>();
            foreach (int num in numbers)
            {
                if (condition(num))
                {
                    result.Add(num);
                }
            }
            return result;
        }

        // Умова 1: перевірка на парність
        static bool IsEven(int n)
        {
            return n % 2 == 0;
        }

        // Умова 2: перевірка на додатність (> 0)
        static bool IsPositive(int n)
        {
            return n > 0;
        }

        static void PrintIntList(string title, List<int> list)
        {
            Console.Write(title + ": ");
            for (int i = 0; i < list.Count; i++)
            {
                Console.Write(list[i] + (i < list.Count - 1 ? ", " : ""));
            }
            Console.WriteLine();
        }

        // =====================================================================
        // ДОМАШНЄ ЗАВДАННЯ: Переписане ПЗ №24 через узагальнений делегат Func
        // =====================================================================

        static double Add(double a, double b) { return a + b; }
        static double Subtract(double a, double b) { return a - b; }
        static double Multiply(double a, double b) { return a * b; }
        static double Divide(double a, double b)
        {
            if (b == 0)
            {
                Console.WriteLine("Помилка: ділення на нуль!");
                return double.NaN;
            }
            return a / b;
        }

        // Замість власного делегата використовуємо вбудований Func<double, double>
        static void ProcessArray(double[] array, Func<double, double> operation)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = operation(array[i]);
            }
        }

        static double Square(double x) { return x * x; }
        static double DoubleValue(double x) { return x * 2; }

        static void PrintDoubleArray(string title, double[] array)
        {
            Console.Write(title + ": ");
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + (i < array.Length - 1 ? ", " : ""));
            }
            Console.WriteLine();
        }

        // =====================================================================
        // ГОЛОВНИЙ МЕТОД ПРОГРАМИ
        // =====================================================================

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // --- ЧАСТИНА 1: ЗАГАЛЬНЕ ЗАВДАННЯ ---
            Console.WriteLine("=== ЗАГАЛЬНЕ ЗАВДАННЯ: Фільтрація через Predicate<int> ===");

            List<int> numbers = new List<int> { -10, -3, 0, 4, 7, 12, -5, 8, 15 };
            PrintIntList("Початковий список", numbers);

            // Передаємо метод IsEven як Predicate<int>
            List<int> evenNumbers = FilterList(numbers, IsEven);
            PrintIntList("Парні числа      ", evenNumbers);

            // Передаємо метод IsPositive як Predicate<int>
            List<int> positiveNumbers = FilterList(numbers, IsPositive);
            PrintIntList("Додатні числа    ", positiveNumbers);

            // --- ЧАСТИНА 2: ДОМАШНЄ ЗАВДАННЯ ---
            Console.WriteLine("\n=== ДОМАШНЄ ЗАВДАННЯ: ПЗ №24 через делегат Func ===");

            double a = 12.0;
            double b = 4.0;

            // Використовуємо стандартний Func<double, double, double> замість власного делегата
            Func<double, double, double> mathOp;

            mathOp = Add;
            Console.WriteLine(a + " + " + b + " = " + mathOp(a, b));

            mathOp = Subtract;
            Console.WriteLine(a + " - " + b + " = " + mathOp(a, b));

            mathOp = Multiply;
            Console.WriteLine(a + " * " + b + " = " + mathOp(a, b));

            mathOp = Divide;
            Console.WriteLine(a + " / " + b + " = " + mathOp(a, b));

            Console.WriteLine();
            double[] arr = new double[] { 2.0, 3.0, 4.0, 5.0 };
            PrintDoubleArray("Початковий масив       ", arr);

            ProcessArray(arr, DoubleValue);
            PrintDoubleArray("Після подвоєння (Func) ", arr);

            ProcessArray(arr, Square);
            PrintDoubleArray("Після квадрату (Func)  ", arr);
        }
    }
}
