using System;
using System.Collections.Generic;
using Capstone.Models;

namespace Capstone.CLI
{
    /// <summary>
    /// *CLIMenu* is an abstract class from which all other menu classes are derived.  
    /// To implement a menu, you need to:
    ///     * Derive from CLIMenu
    ///     * Override SetMenuOptions to build the *menuOptions* dictionary.
    ///     * Override ExecuteSelection to handle each of the user's selections.
    /// 
    /// </summary>
    public abstract class CLIMenu
    {
        /// <summary>
        /// This is where every sub-menu puts its options for display to the user.
        /// </summary>
        protected Dictionary<string, string> menuOptions;

        /// <summary>
        /// quitKey is the string the user types to quit this menu...Q by default.
        /// If you want a different quitKey, (B = Back, or E = Exit for example), then 
        /// simply set this value in the derived class's constructor
        /// </summary>
        protected string quitKey = "Q";

        static private ConsoleColor originalForegroundColor = Console.ForegroundColor;
        static private ConsoleColor originalBackgroundColor = Console.BackgroundColor;

        /// <summary>
        /// The Title of this menu
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Constructor - pass in model data here
        /// </summary>
        public CLIMenu(string title)
        {
            Title = title;
            this.menuOptions = new Dictionary<string, string>();
        }

        /// <summary>
        /// Run starts the menu loop. You cannot override Run. To change the behavior of CLIMenu,
        /// you override SetMenuOptions and ExecuteSelection
        /// </summary>
        public void Run()
        {
            SetMenuOptions();
            while (true)
            {
                Console.Clear();

                BeforeDisplayMenu();

                Console.WriteLine("\r\nPlease make a selection:");
                foreach (KeyValuePair<string, string> menuItem in menuOptions)
                {
                    Console.WriteLine($"{menuItem.Key} - {menuItem.Value}");
                }

                AfterDisplayMenu();

                string choice = GetString("Selection:").ToUpper();

                if (menuOptions.ContainsKey(choice))
                {
                    if (choice == quitKey)
                    {
                        break;
                    }
                    if (!ExecuteSelection(choice))
                    {
                        break;
                    }
                }

            }
        }
        protected void PrintInventory(Item[] items)
        {
            foreach (Item item in items)
            {
                string remaining = $"{item.CountRemaining} remaining";
                if (item.IsSoldOut == true)
                {
                    remaining = "SOLD OUT";
                }
                Console.WriteLine($"{item.SlotIdentifier, -4} {item.Name, -25} {item.Price, 6:C} {remaining, 15}");
            }
        }
        /// <summary>
        /// Override this to display messages, data or text prior to the menu listing
        /// </summary>
        virtual protected void BeforeDisplayMenu()
        {
            SetColor(ConsoleColor.Yellow);
            if (Title != null && Title.Length > 0)
            {
                Console.WriteLine(this.Title);
                Console.WriteLine(new string('=', this.Title.Length));
            }
            ResetColor();
        }

        /// <summary>
        /// Override this to display messages, data or text after to the menu listing
        /// </summary>
        virtual protected void AfterDisplayMenu()
        {
            return;
        }

        /// <summary>
        /// Given a valid menu selection, runs the approriate code to do what the user is asking for.
        /// </summary>
        /// <param name="choice">The menu option (key) selected by the user</param>
        /// <returns>True to keep executing the menu (loop), False to exit this menu (break)</returns>
        abstract protected bool ExecuteSelection(string choice);

        /// <summary>
        /// This method must be implemented to add options to the menuOptions dictionary.
        /// </summary>
        abstract protected void SetMenuOptions();

        #region User Input Helper Methods - NOTE: these are all static public so they can be used from anywhere, not just derived menus
        /// <summary>
        /// This continually prompts the user until they enter a valid integer.
        /// </summary>
        /// <param name="message">The string to prompt the user with</param>
        /// <returns>A valid integer entered by the user</returns>
        static public int GetInteger(string message)
        {
            int resultValue = 0;
            while (true)
            {
                Console.Write(message + " ");
                string userInput = Console.ReadLine().Trim();
                if (int.TryParse(userInput, out resultValue))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("!!! Invalid input. Please enter a valid whole number.");
                }
            }
            return resultValue;
        }

        /// <summary>
        /// This continually prompts the user until they enter a valid double.
        /// </summary>
        /// <param name="message">The string to prompt the user with</param>
        /// <returns>A valid double entered by the user</returns>
        static public double GetDouble(string message)
        {
            double resultValue = 0;
            while (true)
            {
                Console.Write(message + " ");
                string userInput = Console.ReadLine().Trim();
                if (double.TryParse(userInput, out resultValue))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("!!! Invalid input. Please enter a valid decimal number.");
                }
            }
            return resultValue;
        }

        /// <summary>
        /// This continually prompts the user until they enter a valid decimal.
        /// </summary>
        /// <param name="message">The string to prompt the user with</param>
        /// <returns>A valid decimal entered by the user</returns>

        // TODO 1: Write a test to ensure clean data input
        static public decimal GetDecimal(string message)
        {
            decimal resultValue = 0;
            while (true)
            {
                Console.Write(message + " ");
                string userInput = Console.ReadLine().Trim();
                if (decimal.TryParse(userInput, out resultValue))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("!!! Invalid input. Please enter a valid decimal number.");
                }
            }
            return resultValue;
        }

        static public decimal GetDecimalForFeedMoney(string message)
        {
            decimal resultValue = 0;
            while (true)
            {
                Console.Write(message + " ");
                string userInput = Console.ReadLine().Trim();
                if (decimal.TryParse(userInput, out resultValue))
                {
                    if (resultValue == 0 || resultValue == 1 || resultValue == 2 || resultValue == 5 || resultValue == 10)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("!!! Please enter a valid whole dollar amount.");
                    }
                }
                else
                {
                    Console.WriteLine("!!! Invalid input. Please enter a valid decimal number.");
                }
            }
            return resultValue;
        }

        /// <summary>
        /// This continually prompts the user until they enter a valid bool.
        /// </summary>
        /// <param name="message">The string to prompt the user with</param>
        /// <returns>True or false.  The user can type Y or true for true values, N or false for false values.</returns>
        static public bool GetBool(string message)
        {
            bool resultValue = false;
            while (true)
            {
                Console.Write(message + " ");
                string userInput = Console.ReadLine().Trim();
                if (userInput.ToUpper() == "Y")
                {
                    resultValue = true;
                    break;
                }
                else if (userInput.ToUpper() == "N")
                {
                    resultValue = false;
                    break;
                }
                else if (bool.TryParse(userInput, out resultValue))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("!!! Invalid input. Please enter [True, False, Y or N].");
                }
            }
            return resultValue;

        }

        /// <summary>
        /// This continually prompts the user until they enter a valid string (1 or more characters).
        /// </summary>
        /// <param name="message">The string to prompt the user with</param>
        /// <returns>String entered by the user</returns>
        static public string GetString(string message)
        {
            while (true)
            {
                Console.Write(message + " ");
                string userInput = Console.ReadLine().Trim();
                if (!String.IsNullOrEmpty(userInput))
                {
                    return userInput;
                }
                else
                {
                    Console.WriteLine("!!! Invalid input. Please enter a valid string.");
                }
            }
        }

        /// <summary>
        /// Shows a message to the user and waits for the user to hit return
        /// </summary>
        /// <param name="message">Displays a message to the user and then waits for them to hit Return.</param>
        static public void Pause(string message)
        {
            Console.Write(message + " Press Enter to continue.");
            Console.ReadLine();
        }

        static public void SetColor(ConsoleColor foregroundColor)
        {
            Console.ForegroundColor = foregroundColor;
        }

        static public void SetColor(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
        }

        static public void ResetColor()
        {
            Console.ForegroundColor = originalForegroundColor;
            Console.BackgroundColor = originalBackgroundColor;
        }

        static public void WriteError(string message)
        {
            SetColor(ConsoleColor.Red);
            Console.WriteLine(message);
            ResetColor();
        }

        #endregion

    }
}
