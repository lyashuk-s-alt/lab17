using System;

namespace EventsPractice
{
    // =========================================================================
    // ДОМАШНЄ ЗАВДАННЯ: Клас аргументів події на базі стандартного EventArgs
    // =========================================================================
    class LowBalanceEventArgs : EventArgs
    {
        public string AccountNumber { get; private set; }
        public decimal CurrentBalance { get; private set; }

        public LowBalanceEventArgs(string accountNumber, decimal currentBalance)
        {
            AccountNumber = accountNumber;
            CurrentBalance = currentBalance;
        }
    }

    // =========================================================================
    // КЛАС РАХУНОК (Account)
    // =========================================================================
    class Account
    {
        public string AccountNumber { get; private set; }
        public decimal Balance { get; private set; }

        public Account(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public bool Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Помилка: сума зняття має бути більшою за 0.");
                return false;
            }

            if (amount > Balance)
            {
                Console.WriteLine("Помилка: недостатньо коштів на рахунку " + AccountNumber + ".");
                return false;
            }

            Balance -= amount;
            Console.WriteLine("З рахунку " + AccountNumber + " знято " + amount + " грн. Поточний баланс: " + Balance + " грн.");
            return true;
        }
    }

    // =========================================================================
    // КЛАС-ВИДАВЕЦЬ: БАНК (Bank) із подією LowBalance
    // =========================================================================
    class Bank
    {
        public string Name { get; private set; }

        // Оголошення події із використанням стандартного шаблону EventHandler<TEventArgs>
        public event EventHandler<LowBalanceEventArgs> LowBalance;

        public Bank(string name)
        {
            Name = name;
        }

        // Метод проведення операції зняття грошей з рахунку
        public void ProcessWithdrawal(Account account, decimal amount)
        {
            Console.WriteLine("\n[" + Name + "] Обробка запиту на зняття " + amount + " грн...");
            
            bool success = account.Withdraw(amount);

            // Якщо операція успішна і баланс став меншим за 100 одиниць — генеруємо подію
            if (success && account.Balance < 100m)
            {
                OnLowBalance(new LowBalanceEventArgs(account.AccountNumber, account.Balance));
            }
        }

        // Захищений метод для безпечного виклику події
        protected virtual void OnLowBalance(LowBalanceEventArgs e)
        {
            // Копіюємо посилання у локальну змінну для безпечної перевірки на null
            EventHandler<LowBalanceEventArgs> handler = LowBalance;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    // =========================================================================
    // КЛАС-ПІДПИСНИК (Subscriber): Служба сповіщень
    // =========================================================================
    class NotificationService
    {
        // Метод-обробник події (сигнатура відповідає EventHandler<LowBalanceEventArgs>)
        public void OnLowBalanceAlert(object sender, LowBalanceEventArgs e)
        {
            Bank bank = sender as Bank;
            string bankName = bank != null ? bank.Name : "Невідомий банк";

            ConsoleColor prevColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("   [СПОВІЩЕННЯ від " + bankName + "]: Увага! Низький баланс на рахунку " + e.AccountNumber + "!");
            Console.WriteLine("   -> Поточний залишок становить: " + e.CurrentBalance + " грн (менше 100 грн).");
            Console.ForegroundColor = prevColor;
        }
    }

    // =========================================================================
    // ГОЛОВНИЙ МЕТОД ПРОГРАМИ
    // =========================================================================
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Створюємо видавця (Банк), рахунок та підписника (Службу сповіщень)
            Bank bank = new Bank("ПриватКредитБанк");
            Account myAccount = new Account("UA-4500-8899", 250m);
            NotificationService smsService = new NotificationService();

            // Підписуємо обробник на подію LowBalance за допомогою оператора +=
            bank.LowBalance += smsService.OnLowBalanceAlert;

            Console.WriteLine("Початковий стан рахунку " + myAccount.AccountNumber + ": " + myAccount.Balance + " грн.");

            // 1. Знімаємо 100 грн -> залишок 150 грн (подія ще не спрацьовує, бо >= 100)
            bank.ProcessWithdrawal(myAccount, 100m);

            // 2. Знімаємо ще 80 грн -> залишок 70 грн (< 100) -> спрацьовує подія LowBalance!
            bank.ProcessWithdrawal(myAccount, 80m);

            // 3. Знімаємо ще 30 грн -> залишок 40 грн (< 100) -> подія знову спрацьовує!
            bank.ProcessWithdrawal(myAccount, 30m);
        }
    }
}
