using System;
using System.Collections.Generic;
using Capstone.Models;

namespace Capstone.CLI
{
    /// <summary>
    /// The top-level menu in our Market Application
    /// </summary>
    public class PurchaseMenu : CLIMenu
    {
        Transaction Transaction;
        VendingMachine VendingMachine;
        public PurchaseMenu(Transaction transaction, VendingMachine vendingMachine) :
            base("Sub-Menu1")
        {
            this.Transaction = transaction;
            this.VendingMachine = vendingMachine;
            // Store any values passed in....
        }

        protected override void SetMenuOptions()
        {
            this.menuOptions.Add("1", "Feed Money");
            this.menuOptions.Add("2", "Select Product");
            this.menuOptions.Add("3", "Finish Transaction");
            //this.quitKey = "3";
        }

        /// <summary>
        /// The override of ExecuteSelection handles whatever selection was made by the user.
        /// This is where any business logic is executed.
        /// </summary>
        /// <param name="choice">"Key" of the user's menu selection</param>
        /// <returns></returns>
        protected override bool ExecuteSelection(string choice)
        {
            switch (choice)
            {
                case "1":
                    while (true)
                    {
                        decimal input = GetDecimalForFeedMoney("Please enter your money. Machine accepts $1, $2, $5, $10: ");
                        Transaction.FeedMoney(input);
                        bool needsMoreMoney = GetBool("Do you wish to enter more money? (Y/N) ");
                        if (!needsMoreMoney)
                        {
                            break;
                        }
                    }
                    return true;
                case "2":
                    Item[] items = VendingMachine.GetInventory();
                    PrintInventory(items);
                    Item chosenItem = DoChecks();
                    if (chosenItem == null)
                    {
                        return true;
                    }
                    VendingMachine.Dispense(chosenItem);
                    Console.WriteLine(chosenItem);
                    Pause("");
                    return true;
                case "3":
                    Console.WriteLine(Transaction.MakeChange());
                    VendingMachine.WriteSoldItemsToFile();
                    Pause("");
                    return false;
            }
            return true;
        }
        public Item GetChosenItem(string code)
        {
            foreach (Item item in VendingMachine.items)
            {
                if (code.ToUpper() == item.SlotIdentifier)
                {
                    return item;
                }
            }
            return null;
        }
        private Item DoChecks()
        {
            while (true)
            {
                string codeSelection = GetString("Please enter your selection (\"Q\" to go back): ");
                if (codeSelection.ToUpper() == "Q")
                {
                    return null;
                }

                Item chosenItem = GetChosenItem(codeSelection);
                if (chosenItem == null)
                {
                    Console.WriteLine("Please select a valid code: ");
                    Pause("");
                }
                else if (chosenItem.IsSoldOut)
                {
                    Console.WriteLine("Item is sold out. Please choose another item: ");
                    Pause("");
                }
                else if (Transaction.Balance < chosenItem.Price)
                {
                    Console.WriteLine("Balance is not enough. Please enter more money. ");
                    Pause("");
                    return null;
                }
                else
                {
                    return chosenItem;
                }
            }
        }
        protected override void BeforeDisplayMenu()
        {
            Console.WriteLine($"Your current balance is: {Transaction.Balance:C}");
            PrintHeader();
        }

        protected override void AfterDisplayMenu()
        {
            base.AfterDisplayMenu();
            SetColor(ConsoleColor.Cyan);
            Console.WriteLine("");
            ResetColor();
        }

        private void PrintHeader()
        {
            SetColor(ConsoleColor.Magenta);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Purchase"));
            ResetColor();
        }

    }
}
