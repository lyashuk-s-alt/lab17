using System;
using System.Collections.Generic;
using System.IO;

namespace FleetManagementSystem
{
    // =========================================================================
    // 1. ІНТЕРФЕЙС ОБСЛУГОВУВАННЯ (IMaintainable)
    // =========================================================================
    interface IMaintainable
    {
        bool NeedsMaintenance();
        void PerformMaintenance();
    }

    // =========================================================================
    // 2. АБСТРАКТНИЙ БАЗОВИЙ КЛАС ТРАНСПОРТНИЙ ЗАСІБ (Vehicle)
    // =========================================================================
    abstract class Vehicle : IMaintainable
    {
        private int _year;
        private double _mileage;

        public string Brand { get; private set; }
        public string Model { get; private set; }

        // Валідація року випуску (Домашнє завдання: граничні значення)
        public int Year
        {
            get { return _year; }
            private set
            {
                if (value < 1900 || value > DateTime.Now.Year + 1)
                {
                    throw new ArgumentOutOfRangeException("Year", "Рік випуску має бути в межах від 1900 до " + (DateTime.Now.Year + 1) + ".");
                }
                _year = value;
            }
        }

        // Валідація пробігу (не може бути від'ємним)
        public double Mileage
        {
            get { return _mileage; }
            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Mileage", "Пробіг не може бути від'ємним числом.");
                }
                _mileage = value;
            }
        }

        public double LastServiceMileage { get; protected set; }

        protected Vehicle(string brand, string model, int year, double mileage)
        {
            if (string.IsNullOrWhiteSpace(brand) || string.IsNullOrWhiteSpace(model))
            {
                throw new ArgumentException("Марка та модель транспорту не можуть бути порожніми.");
            }

            Brand = brand;
            Model = model;
            Year = year;
            Mileage = mileage;
            LastServiceMileage = mileage;
        }

        public void Drive(double km)
        {
            if (km <= 0)
            {
                throw new ArgumentException("Дистанція поїздки має бути більшою за 0 км.");
            }
            Mileage += km;
        }

        // Абстрактні методи для поліморфної реалізації у нащадках
        public abstract string GetVehicleType();
        public abstract decimal CalculateServiceCost();
        public abstract bool NeedsMaintenance();

        public virtual void PerformMaintenance()
        {
            decimal cost = CalculateServiceCost();
            LastServiceMileage = Mileage;
            Console.WriteLine("   [ТО ВИКОНАНО] " + GetVehicleType() + " " + Brand + " " + Model + " | Вартість ТО: " + cost + " грн.");
        }

        public override string ToString()
        {
            string status = NeedsMaintenance() ? "ПОТРЕБУЄ ТО!" : "Справний";
            return string.Format("[{0,-10}] {1} {2} ({3} р.) | Пробіг: {4,7:F0} км | Статус: {5}",
                GetVehicleType(), Brand, Model, Year, Mileage, status);
        }
    }

    // =========================================================================
    // 3. ПОХІДНИЙ КЛАС: ЛЕГКОВИЙ АВТОМОБІЛЬ (Car)
    // =========================================================================
    class Car : Vehicle
    {
        public string BodyType { get; private set; }

        public Car(string brand, string model, int year, double mileage, string bodyType)
            : base(brand, model, year, mileage)
        {
            BodyType = string.IsNullOrWhiteSpace(bodyType) ? "Седан" : bodyType;
        }

        public override string GetVehicleType()
        {
            return "Легковик";
        }

        // Легковик потребує ТО кожні 15 000 км
        public override bool NeedsMaintenance()
        {
            return (Mileage - LastServiceMileage) >= 15000;
        }

        public override decimal CalculateServiceCost()
        {
            return 3500m;
        }

        public override string ToString()
        {
            return base.ToString() + " | Кузов: " + BodyType;
        }
    }

    // =========================================================================
    // 4. ПОХІДНИЙ КЛАС: ВАНТАЖІВКА (Truck)
    // =========================================================================
    class Truck : Vehicle
    {
        public double LoadCapacityTons { get; private set; }

        public Truck(string brand, string model, int year, double mileage, double loadCapacityTons)
            : base(brand, model, year, mileage)
        {
            if (loadCapacityTons <= 0 || loadCapacityTons > 100)
            {
                throw new ArgumentOutOfRangeException("loadCapacityTons", "Вантажопідйомність має бути в межах від 0.1 до 100 тонн.");
            }
            LoadCapacityTons = loadCapacityTons;
        }

        public override string GetVehicleType()
        {
            return "Вантажівка";
        }

        // Вантажівка потребує ТО кожні 10 000 км
        public override bool NeedsMaintenance()
        {
            return (Mileage - LastServiceMileage) >= 10000;
        }

        public override decimal CalculateServiceCost()
        {
            return 8000m + (decimal)LoadCapacityTons * 400m;
        }

        public override string ToString()
        {
            return base.ToString() + " | Вантажопідйомність: " + LoadCapacityTons + " т";
        }
    }

    // =========================================================================
    // 5. МЕНЕДЖЕР АВТОПАРКУ (Бізнес-логіка, події, колекція та експорт у файл)
    // =========================================================================
    class FleetManager
    {
        private List<Vehicle> _vehicles = new List<Vehicle>();

        // Подія додавання нового транспорту в автопарк
        public event Action<Vehicle> VehicleAdded;

        public void AddVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }
            _vehicles.Add(vehicle);

            if (VehicleAdded != null)
            {
                VehicleAdded(vehicle);
            }
        }

        public List<Vehicle> GetAllVehicles()
        {
            return new List<Vehicle>(_vehicles);
        }

        // Фільтрація транспорту за допомогою вбудованого делегата Predicate<Vehicle>
        public List<Vehicle> FilterVehicles(Predicate<Vehicle> condition)
        {
            return _vehicles.FindAll(condition);
        }

        // Проведення планового ТО для всіх машин, які цього потребують
        public void ServiceAllNeeded()
        {
            int servicedCount = 0;
            foreach (Vehicle v in _vehicles)
            {
                if (v.NeedsMaintenance())
                {
                    v.PerformMaintenance();
                    servicedCount++;
                }
            }
            if (servicedCount == 0)
            {
                Console.WriteLine("   Усі транспортні засоби у нормі, ТО не потрібне.");
            }
        }

        // Збереження фінального звіту у текстовий файл на диску
        public void SaveReportToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                writer.WriteLine("=========================================================================");
                writer.WriteLine("ЗВІТ СИСТЕМИ «УПРАВЛІННЯ АВТОПАРКОМ» | Дата: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                writer.WriteLine("Загальна кількість одиниць транспорту: " + _vehicles.Count);
                writer.WriteLine("=========================================================================");

                for (int i = 0; i < _vehicles.Count; i++)
                {
                    writer.WriteLine((i + 1) + ". " + _vehicles[i].ToString());
                }

                writer.WriteLine("=========================================================================");
            }
        }
    }

    // =========================================================================
    // 6. ГОЛОВНИЙ КЛАС ПРОГРАМИ (Демонстрація роботи та тестування граничних значень)
    // =========================================================================
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            FleetManager fleet = new FleetManager();

            // Підписка на подію додавання транспорту (лямбда-вираз)
            fleet.VehicleAdded += v => Console.WriteLine("[ПОДІЯ] До автопарку додано: " + v.Brand + " " + v.Model);

            Console.WriteLine("=== 1. НАПОВНЕННЯ АВТОПАРКУ (List<Vehicle>) ===");
            Car car1 = new Car("Toyota", "Camry", 2021, 45000, "Седан");
            Car car2 = new Car("Skoda", "Octavia", 2018, 112000, "Універсал");
            Truck truck1 = new Truck("Volvo", "FH16", 2020, 230000, 20.0);
            Truck truck2 = new Truck("MAN", "TGX", 2023, 35000, 18.5);

            fleet.AddVehicle(car1);
            fleet.AddVehicle(car2);
            fleet.AddVehicle(truck1);
            fleet.AddVehicle(truck2);

            // Імітуємо експлуатацію (пробіг після останнього ТО)
            car2.Drive(16000);   // Понад 15 000 км -> потребує ТО
            truck1.Drive(12000); // Понад 10 000 км -> потребує ТО

            Console.WriteLine("\n=== 2. ПЕРЕГЛЯД УСЬОГО ТРАНСПОРТУ В АВТОПАРКУ ===");
            PrintList(fleet.GetAllVehicles());

            Console.WriteLine("\n=== 3. ФІЛЬТРАЦІЯ: Транспорт новіший за 2019 рік ===");
