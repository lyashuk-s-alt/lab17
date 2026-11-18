using System;
using System.IO;

namespace MulticastDelegatesPractice
{
    // Оголошення делегата типу void, який приймає рядок повідомлення
    delegate void LogHandler(string message);

    class Program
    {
        // =====================================================================
        // ТРИ ОКРЕМІ МЕТОДИ ЛОГУВАННЯ
        // =====================================================================

        // 1. Виведення повідомлення в консоль білим кольором
        static void LogWhiteConsole(string message)
        {
            ConsoleColor previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[БІЛИЙ ЛОГ]: " + message);
            Console.ForegroundColor = previousColor;
        }

        // 2. Виведення повідомлення в консоль червоним кольором (як помилка)
        static void LogRedErrorConsole(string message)
        {
            ConsoleColor previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ПОМИЛКА (ЧЕРВОНИЙ)]: " + message);
            Console.ForegroundColor = previousColor;
        }

        // 3. Запис повідомлення у текстовий файл
        static void LogToFile(string message)
        {
            string filePath = "log.txt";
            string logLine = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " | " + message + Environment.NewLine;
            File.AppendAllText(filePath, logLine);
            Console.WriteLine("[ФАЙЛ]: Повідомлення успішно записано у файл '" + filePath + "'.");
        }

        // =====================================================================
        // ГОЛОВНИЙ МЕТОД ПРОГРАМИ
        // =====================================================================

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== ЗАГАЛЬНЕ ЗАВДАННЯ: Об'єднання 3 методів у багатоадресний делегат ===\n");

            // Створюємо екземпляр делегата та об'єднуємо всі три методи за допомогою +=
            LogHandler logger = LogWhiteConsole;
            logger += LogRedErrorConsole;
            logger += LogToFile;

            // Один виклик делегата послідовно запускає всі три методи зі списку
            logger("Критичний збій з'єднання з сервером!");

            Console.WriteLine("\n=== ДОМАШНЄ ЗАВДАННЯ: Видалення методу з ланцюжка за допомогою -= ===\n");

            // Відключаємо виведення червоним кольором із ланцюжка викликів
            logger -= LogRedErrorConsole;

            // Безпечний виклик після видалення (перевірка на null на випадок, якщо ланцюжок спорожнів)
            if (logger != null)
            {
                logger("Планове збереження стану системи.");
            }
        }
    }
}
