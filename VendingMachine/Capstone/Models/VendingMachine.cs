using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Transactions;

namespace Capstone.Models
{
    public class VendingMachine
    {
        public List<Item> items = new List<Item>();
        public List<Transaction> Transactions = new List<Transaction>();
        public Dictionary<string, int> soldItems = new Dictionary<string, int>();
        private string TransactionLogPath { get; }
        private string SalesOutputPath { get; }
        public VendingMachine(string transactionLogPath, string salesOutputPath)
        {
            this.TransactionLogPath = transactionLogPath;
            this.SalesOutputPath = salesOutputPath;
        }
        public void Restock(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string lineOfText = "";
                while (!reader.EndOfStream)
                {
                    lineOfText = reader.ReadLine();
                    string[] itemData = lineOfText.Split("|");
                    Item item = new Item(itemData[0], itemData[1], decimal.Parse(itemData[2]), itemData[3]);
                    items.Add(item);
                }
            }
            LoadSoldItems();
        }
        public Item[] GetInventory()
        {
            return items.ToArray();
        }
        public Transaction CreateTransaction()
        {
            Transaction transaction = new Transaction(TransactionLogPath);
            Transactions.Add(transaction);
            return transaction;
        }
        public void Dispense(Item chosenItem)
        {
            chosenItem.CountRemaining--;
            AddSoldItemsToDictionary(chosenItem);
            using (StreamWriter writer = new StreamWriter(TransactionLogPath, true))
            {
                writer.WriteLine($"{DateTime.Now} {chosenItem.Name} {chosenItem.SlotIdentifier} {Transactions.Last().Balance:C} {(Transactions.Last().Balance - chosenItem.Price):C}");
            }
            Transactions.Last().Debit(chosenItem);
        }
        public void WriteSoldItemsToFile()
        {
            soldItems.Remove("TOTAL");
            using (StreamWriter writer = new StreamWriter(SalesOutputPath, false))
            {
                int totalItemsSold = 0;
                foreach (KeyValuePair<string, int> entry in soldItems)
                {
                    totalItemsSold += entry.Value;
                    writer.WriteLine($"{entry.Key}|{entry.Value}");
                }
                writer.WriteLine($"TOTAL|{totalItemsSold}");
            }
        }
        private void LoadSoldItems()
        {
            if (!File.Exists(SalesOutputPath))
            {
                using (FileStream fs = File.Create(SalesOutputPath))
                {

                }
            }
            using (StreamReader reader = new StreamReader(SalesOutputPath))
            {
                while (!reader.EndOfStream)
                {
                    string[] lineOfText = reader.ReadLine().Split('|');
                    soldItems.Add(lineOfText[0], int.Parse(lineOfText[1]));
                }
            }
        }
        private void AddSoldItemsToDictionary(Item chosenItem)
        {
            if (soldItems.ContainsKey(chosenItem.Name))
            {
                soldItems[chosenItem.Name]++;
            }
            else
            {
                soldItems.Add(chosenItem.Name, 1);
            }
        }
    }
}