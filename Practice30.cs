using System;
using System.IO;

namespace StreamReaderWriterPractice
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            string filePath = "notes.txt";

            Console.WriteLine("=== ЗАГАЛЬНЕ ТА ДОМАШНЄ ЗАВДАННЯ: Запис 5 рядків у файл (режим додавання) ===");
            Console.WriteLine("Введіть 5 рядків тексту (після кожного рядка натискайте Enter):\n");

            // ДОМАШНЄ ЗАВДАННЯ: другий параметр 'true' у StreamWriter вмикає режим додавання (append)
            // Нові дані дописуються в кінець файлу, не перезаписуючи старий вміст.
            // Конструкція using гарантує автоматичне збереження буфера та закриття потоку.
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                for (int i = 1; i <= 5; i++)
                {
                    Console.Write("Рядок " + i + ": ");
                    string inputLine = Console.ReadLine();
                    writer.WriteLine(inputLine);
                }
            }

            Console.WriteLine("\nУспішно: 5 рядків додано у файл '" + filePath + "'.");

            Console.WriteLine("\n=== ЗЧИТУВАННЯ ВСЬОГО ВМІСТУ ФАЙЛУ З НУМЕРАЦІЄЮ РЯДКІВ ===");

            // Зчитуємо файл порядково за допомогою StreamReader до самого кінця потоку
            using (StreamReader reader = new StreamReader(filePath))
            {
                int lineNumber = 1;
                string line;

                // Читаємо рядки, поки ReadLine() не поверне null (або поки !reader.EndOfStream)
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(lineNumber + ". " + line);
                    lineNumber++;
                }
            }
        }
    }
}
