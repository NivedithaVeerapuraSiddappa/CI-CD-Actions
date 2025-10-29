using System;
using BankAccountApp;

class Program
{
    static void Main(string[] args)
    {
        BankAccount account = BankAccount.init_account(1000); // Initialize account with $1000

        Console.WriteLine($"Account ID: {account.AccountId}");
        Console.WriteLine($"Initial Balance: {account.Balance}");

        // Test deposit
        Console.WriteLine("\nDepositing $500...");
        account.deposit(500);
        Console.WriteLine($"Balance after deposit: {account.Balance}");

        // Test withdraw (valid)
        Console.WriteLine("\nWithdrawing $200...");
        bool validWithdraw = account.withdraw(200);
        Console.WriteLine(validWithdraw
            ? $"Balance after withdrawal: {account.Balance}"
            : $"Withdrawal of $200 failed. Balance: {account.Balance}");

        // Test withdraw (invalid - less than min)
        Console.WriteLine("\nAttempting to withdraw $50 (should fail)...");
        bool minWithdraw = account.withdraw(50);
        Console.WriteLine(minWithdraw
            ? $"Balance after withdrawal: {account.Balance}"
            : $"Withdrawal of $50 failed. Balance: {account.Balance}");

        // Test withdraw (invalid - more than max)
        Console.WriteLine("\nAttempting to withdraw $600 (should fail)...");
        bool maxWithdraw = account.withdraw(600);
        Console.WriteLine(maxWithdraw
            ? $"Balance after withdrawal: {account.Balance}"
            : $"Withdrawal of $600 failed. Balance: {account.Balance}");

        // Test withdraw (invalid - more than balance)
        Console.WriteLine("\nAttempting to withdraw $2000 (should fail)...");
        bool overWithdraw = account.withdraw(2000);
        Console.WriteLine(overWithdraw
            ? $"Balance after withdrawal: {account.Balance}"
            : $"Withdrawal of $2000 failed. Balance: {account.Balance}");

        // Test deposit (invalid - negative)
        Console.WriteLine("\nAttempting to deposit -$100 (should fail)...");
        account.deposit(-100);
        Console.WriteLine($"Balance after failed deposit: {account.Balance}");
    }
}
