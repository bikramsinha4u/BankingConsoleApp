using System;

namespace OnlineBanking
{
    class Program
    {
        static void Main(string[] args)
        {
            var myBank = new MyBank();
            myBank.AccountActions();

            while (true)
            {
                Console.Write("\nEnter Command: ");
                var action = Console.ReadLine();

                if (!string.Equals(action, "Exit", StringComparison.OrdinalIgnoreCase))
                {
                    myBank.ExecuteAction(action);
                }
                else if(string.Equals(action, "Exit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
            }
        }
    }
}
