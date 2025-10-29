using System;

namespace BankAccountApp
{
    public class BankAccount
    {
        public int AccountId { get; private set; }
        public decimal Balance { get; private set; }

        private BankAccount(int accountId, decimal initialBalance)
        {
            AccountId = accountId;
            Balance = initialBalance;
        }

        public static BankAccount init_account(decimal initialBalance)
        {
            var rand = new Random();
            int accountId = rand.Next(100000, 999999);
            return new BankAccount(accountId, initialBalance);
        }

        public void deposit(decimal amount)
        {
            if (amount > 0)
                Balance += amount;
        }

        public bool withdraw(decimal amount)
        {
            // Min withdrawal: $100, Max withdrawal: $500
            if (amount < 100 || amount > 500)
                return false;
            if (amount > 0 && amount <= Balance)
            {
                Balance -= amount;
                return true;
            }
            return false;
        }
    }
}
