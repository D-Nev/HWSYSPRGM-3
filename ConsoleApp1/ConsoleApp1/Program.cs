namespace ConsoleApp1
{
    internal class Program;

    class Bank
    {
        private int accountBalance = 1000;

        public void WithdrawMoney(int amount)
        {
            lock (this)
            {
                if (amount > accountBalance)
                {
                    Console.WriteLine("Insufficient funds.");
                }
                else
                {
                    Thread.Sleep(1000);
                    accountBalance -= amount;
                    Console.WriteLine($"Successfully withdrew ${amount}. New balance: ${accountBalance}");
                }
            }
        }
    }
    class ATM
    {
        private Bank bank;

        public ATM(Bank bank)
        {
            this.bank = bank;
        }

        public void WithdrawMoney(int amount)
        {
            bank.WithdrawMoney(amount);
        }
    }
    class ConsoleApp1
    {
        static void Main()
        {
            Bank bank = new Bank();
            ATM atm1 = new ATM(bank);
            ATM atm2 = new ATM(bank);

            new Thread(() => atm1.WithdrawMoney(500)).Start();
            new Thread(() => atm2.WithdrawMoney(750)).Start();

            Console.ReadLine();
        }
    }
}
