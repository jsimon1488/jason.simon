using System;
using System.IO;

namespace Capstone.Models
{
    public class Transaction
    {
        public decimal Balance { get; private set; }
        public string TransactionLogPath { get; }
        public Transaction(string TransactionLogPath)
        {
            this.TransactionLogPath = TransactionLogPath;
        }
        public void FeedMoney(decimal amountIn)
        {
            Balance += amountIn;
            // Log amountIn first, then Balance
            using (StreamWriter writer = new StreamWriter(TransactionLogPath, true))
            {
                writer.WriteLine($"{DateTime.Now} FEED MONEY: {amountIn:C} {Balance:C}");
            }
        }
        public void Debit(Item item)
        {
            Balance -= item.Price;
        }
        public string MakeChange()
        {
            if (Balance == 0)
            {
                return $"No change given.";
            }
            decimal changeBalance = Balance;
            int quarters = (int)((double)Balance / 0.25);
            Balance %= 0.25M;
            int dimes = (int)((double)Balance / 0.10);
            Balance %= 0.10M;
            int nickels = (int)((double)Balance / 0.05);
            Balance %= 0.05M;
            // Log Balance first, then log $0.00;
            using (StreamWriter writer = new StreamWriter(TransactionLogPath, true))
            {
                writer.WriteLine($"{DateTime.Now} GIVE CHANGE: {changeBalance:C} {Balance:C}");
            }
            return $"Your change is {changeBalance:C}. You get {quarters} quarters, {dimes} dimes, and {nickels} nickels.";
        }
    }
}