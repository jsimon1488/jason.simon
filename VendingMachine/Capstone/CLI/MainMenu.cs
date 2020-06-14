using Capstone.Models;
using System;
using System.Collections.Generic;

namespace Capstone.CLI
{
    public class MainMenu : CLIMenu
    {
        private VendingMachine VendingMachine;

        public MainMenu(VendingMachine vendingMachine) : base("Main Menu")
        {
            this.VendingMachine = vendingMachine;
        }

        protected override void SetMenuOptions()
        {
            this.menuOptions.Add("1", "Display Vending Machine Items");
            this.menuOptions.Add("2", "Purchase");
            this.menuOptions.Add("Q", "Quit program");
        }
        protected override bool ExecuteSelection(string choice)
        {
            switch (choice)
            {
                case "1":
                    Item[] items = VendingMachine.GetInventory();
                    PrintInventory(items);
                    Pause("");
                    return true;
                case "2":
                    Transaction transaction = VendingMachine.CreateTransaction();
                    PurchaseMenu sm = new PurchaseMenu(transaction, VendingMachine);
                    sm.Run();
                    return true;
            }
            return true;
        }
        

        protected override void BeforeDisplayMenu()
        {
            PrintHeader();
        }


        private void PrintHeader()
        {
            SetColor(ConsoleColor.Yellow);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Vending Machine"));
            ResetColor();
        }
    }
}
