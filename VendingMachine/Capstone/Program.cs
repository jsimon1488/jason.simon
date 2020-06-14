using Capstone.CLI;
using Capstone.Models;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            string transactionLogPath = @"..\..\..\..\Log.txt";
            string salesOutputPath = @"..\..\..\..\SalesOutput.txt";
            string inventoryPath = @"..\..\..\..\vendingmachine.csv";

            VendingMachine vendingMachine = new VendingMachine(transactionLogPath, salesOutputPath);
            vendingMachine.Restock(inventoryPath);

            MainMenu mainMenu = new MainMenu(vendingMachine);
            mainMenu.Run();
        }
    }
}
