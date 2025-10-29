using Microsoft.VisualStudio.TestTools.UnitTesting;
using BankAccountApp;

namespace BankAccountTests
{
    [TestClass]
    public class BankAccountUnitTests
    {
        [TestMethod]
        public void Deposit_PositiveAmount_IncreasesBalance()
        {
            var account = BankAccount.init_account(1000);
            account.deposit(200);
            Assert.AreEqual(1200, account.Balance);
        }

        [TestMethod]
        public void Deposit_NegativeAmount_DoesNotChangeBalance()
        {
            var account = BankAccount.init_account(1000);
            account.deposit(-100);
            Assert.AreEqual(1000, account.Balance);
        }

        [TestMethod]
        public void Withdraw_ValidAmount_DecreasesBalance()
        {
            var account = BankAccount.init_account(1000);
            bool result = account.withdraw(200);
            Assert.IsTrue(result);
            Assert.AreEqual(800, account.Balance);
        }

        [TestMethod]
        public void Withdraw_AmountLessThanMin_Fails()
        {
            var account = BankAccount.init_account(1000);
            bool result = account.withdraw(50);
            Assert.IsFalse(result);
            Assert.AreEqual(1000, account.Balance);
        }

        [TestMethod]
        public void Withdraw_AmountGreaterThanMax_Fails()
        {
            var account = BankAccount.init_account(1000);
            bool result = account.withdraw(600);
            Assert.IsFalse(result);
            Assert.AreEqual(1000, account.Balance);
        }

        [TestMethod]
        public void Withdraw_AmountGreaterThanBalance_Fails()
        {
            var account = BankAccount.init_account(400);
            bool result = account.withdraw(400);
            Assert.IsTrue(result);
            result = account.withdraw(200);
            Assert.IsFalse(result);
            Assert.AreEqual(0, account.Balance);
        }
    }
}
