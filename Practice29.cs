using System;
using System.IO;

namespace FileSystemPractice
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            string folderName = "OOP_Practice";

            Console.WriteLine("=== 1. ПЕРЕВІРКА ТА СТВОРЕННЯ ДИРЕКТОРІЇ (Домашнє завдання) ===");

            // Перевіряємо, чи існує папка на диску
            if (Directory.Exists(folderName))
            {
                Console.WriteLine("Інформація: папка '" + folderName + "' вже існує на диску. Повторне створення не потрібне.");
            }
            else
            {
                Directory.CreateDirectory(folderName);
                Console.WriteLine("Успішно: створено нову папку '" + folderName + "'.");
            }

            Console.WriteLine("\n=== 2. ГЕНЕРАЦІЯ ТРЬОХ ПОРОЖНІХ ТЕКСТОВИХ ФАЙЛІВ ===");

            string[] fileNames = new string[] { "file1.txt", "file2.txt", "file3.txt" };

            foreach (string name in fileNames)
            {
                // Безпечно комбінуємо шлях до папки та ім'я файлу
                string fullPath = Path.Combine(folderName, name);

                // Створюємо порожній файл та одразу закриваємо файловий потік (using)
                using (FileStream fs = File.Create(fullPath))
                {
                }

                Console.WriteLine("Згенеровано порожній файл: " + fullPath);
            }

            Console.WriteLine("\n=== 3. ЗЧИТУВАННЯ СПИСКУ ФАЙЛІВ ТА ЧАСУ ЇХ СТВОРЕННЯ ===");

            // Отримуємо масив шляхів до всіх файлів у директорії OOP_Practice
            string[] filesInDirectory = Directory.GetFiles(folderName);

            Console.WriteLine("Знайдено файлів у папці '" + folderName + "': " + filesInDirectory.Length + "\n");

            foreach (string filePath in filesInDirectory)
            {
                string onlyName = Path.GetFileName(filePath);
                DateTime creationTime = File.GetCreationTime(filePath);

                Console.WriteLine("Назва файлу: " + onlyName + " | Час створення: " + creationTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }
    }
}
