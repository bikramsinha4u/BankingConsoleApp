using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineBanking
{
    internal class MyBank
    {
        private Dictionary<string, decimal> Accounts { get; set; }
        private Dictionary<string, short> Secrets { get; set; }
        public MyBank()
        {
            SeedAccountInfo();
        }

        private void SeedAccountInfo()
        {
            Accounts = new Dictionary<string, decimal>
            {
                { "Evan", 7.07m },
                { "Rong", 370.50m },
                { "J", 530.20m },
                { "David", 297.14m },
                { "Dan", 100.54m },
                { "Taylor", 135.45m }
            };

            Secrets = new Dictionary<string, short>
            {
                { "Evan", 5232 },
                { "Rong", 9999 },
                { "J", 1234 },
                { "David", 0000 },
                { "Dan", 4567 },
                { "Taylor", 9753 }
            };
        }

        private int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;

            Random _rdm = new Random();

            return _rdm.Next(_min, _max);
        }

        public void AddAccount()
        {
            Console.Write("Enter account name: ");
            var accountName = Console.ReadLine();
            Console.Write("Enter account opening balance: ");
            var openingBalanceInput = Console.ReadLine();
            decimal openingBalance;
            decimal.TryParse(openingBalanceInput, out openingBalance);

            if (!Accounts.ContainsKey(accountName))
            {
                Accounts.Add(accountName, openingBalance);
            }
            else
            {
                Console.WriteLine("Account with the same name already exists.");
                return;
            }            

            while (true)
            {
                var secret = (short)GenerateRandomNo();

                if (!Secrets.ContainsValue(secret))
                {
                    Secrets.Add(accountName, secret);
                    Console.WriteLine("New account added with following details:");
                    Console.WriteLine($"Account Name: {accountName}, Account Balance: {DisplayAmount(openingBalance)}, Account Secret: {secret}");
                    break;
                }
            }
        }

        public void ViewAccounts()
        {
            Console.WriteLine("Accounts");
            Console.WriteLine("--------");

            foreach (var item in Accounts)
            {
                Console.WriteLine(item.Key);
            }
        }

        public void ViewAccountBalances()
        {
            Console.WriteLine("Name \t\t Current Balance");
            Console.WriteLine("--------------------------------");

            foreach (var item in Accounts)
            {
                Console.WriteLine($"{item.Key} \t\t {DisplayAmount(item.Value)}");
            }
        }

        public void ViewTotalAccountBalance()
        {
            Console.Write("The bank's total account balance is ");
            Console.WriteLine(DisplayAmount(Accounts.Aggregate(0m, (current, item) => current + item.Value)));
        }

        public void DeductTax()
        {
            Console.Write("How much tax (in percent) should be subtracted from each account? ");
            var taxRateInput = Console.ReadLine();
            decimal taxRate;

            decimal.TryParse(taxRateInput, out taxRate);
            if (taxRate != 0)
            {
                var accounts = new List<string>(Accounts.Keys);

                foreach (string account in accounts)
                {
                    Accounts[account] = Accounts[account] * ((100 - taxRate) / 100);
                }

                Console.WriteLine($"Subtracted {taxRate}% tax from each account.\nNew account balances are:");
                ViewAccountBalances();
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }

        public bool AccessAccount(out string accountName)
        {
            short enteredPassword = 0;

            Console.Write("Please enter bank member's name: ");
            var enteredAccount = Console.ReadLine();
            accountName = enteredAccount;

            if (Accounts.ContainsKey(enteredAccount))
            {
                Console.Write("Please enter in the bank member's password: ");
                short.TryParse(Console.ReadLine(), out enteredPassword);
            }
            else
            {
                Console.WriteLine("Account does not exist.");
                return false;
            }

            if (Secrets[enteredAccount] == enteredPassword)
            {
                Console.WriteLine("Access Granted.");
                return true;
            }
            else
            {
                Console.WriteLine("Access denied.");
                return false;
            }
        }

        public void AccountWithdrawal()
        {
            decimal withdrawalAmount = 0.0m;
            string accountName;
            var hasAccountAccess = AccessAccount(out accountName);

            if (hasAccountAccess)
            {
                Console.Write("How much are they withdrawing? ");
                decimal.TryParse(Console.ReadLine(), out withdrawalAmount);
            }
            else
            {
                return;
            }

            if (withdrawalAmount == 0)
            {
                Console.WriteLine("Invalid withdrawal amount.");
            }
            else
            {
                var balance = Accounts[accountName] - withdrawalAmount;
                Accounts[accountName] = balance;

                Console.WriteLine($"Account balance of {accountName} after withdrawal is {DisplayAmount(balance)}.");
            }
        }

        public void AuditAccount()
        {
            var auditAmount = 0m;
            List<string> auditAccounts;

            Console.Write("Enter the amount for audit: ");
            decimal.TryParse(Console.ReadLine(), out auditAmount);

            if (auditAmount != 0m)
            {
                auditAccounts = Accounts.Where(x => x.Value <= auditAmount).Select(x => x.Key).ToList();
            }
            else
            {
                Console.WriteLine("Invalid audit amount.");
                return;
            }

            Console.WriteLine("Name \t\t Current Balance");
            Console.WriteLine("--------------------------------");
            foreach (var item in auditAccounts)
            {
                Console.WriteLine($"{item} \t\t {DisplayAmount(Accounts[item])}");
            }
        }

        public void ViewLowestAccountBalance()
        {
            var min = Accounts.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            Console.WriteLine($"Lowest balance is {DisplayAmount(Accounts[min])} in the account \'{min}\'.");
        }

        public void ExecuteAction(string action)
        {
            if (string.Equals(action, "Accounts", StringComparison.OrdinalIgnoreCase))
            {
                ViewAccounts();
            }
            else if (string.Equals(action, "Add", StringComparison.OrdinalIgnoreCase))
            {
                AddAccount();
            }
            else if (string.Equals(action, "Balances", StringComparison.OrdinalIgnoreCase))
            {
                ViewAccountBalances();
            }
            else if (string.Equals(action, "Total", StringComparison.OrdinalIgnoreCase))
            {
                ViewTotalAccountBalance();
            }
            else if (string.Equals(action, "Tax", StringComparison.OrdinalIgnoreCase))
            {
                DeductTax();
            }
            else if (string.Equals(action, "Open", StringComparison.OrdinalIgnoreCase))
            {
                AccessAccount(out _);
            }
            else if (string.Equals(action, "Withdraw", StringComparison.OrdinalIgnoreCase))
            {
                AccountWithdrawal();
            }
            else if (string.Equals(action, "Audit", StringComparison.OrdinalIgnoreCase))
            {
                AuditAccount();
            }
            else if (string.Equals(action, "Lowest", StringComparison.OrdinalIgnoreCase))
            {
                ViewLowestAccountBalance();
            }
            else if (string.Equals(action, "Clear", StringComparison.OrdinalIgnoreCase))
            {
                Console.Clear();
                AccountActions();
            }
            else
            {
                Console.WriteLine("Invalid operation.");
            }
        }

        private string DisplayAmount(decimal amount)
        {
            return amount.ToString("#,##0.00");
        }

        public void AccountActions()
        {
            Console.Title = "MyBank";
            Console.WriteLine("Use following commands for the bank operations:");
            Console.WriteLine("Accounts \t To view all accounts.");
            Console.WriteLine("Add      \t To view all accounts.");
            Console.WriteLine("Balances \t To view all accounts with balances.");
            Console.WriteLine("Total    \t To view sum of all account balances.");
            Console.WriteLine("Tax      \t To deduct tax on from all accounts.");
            Console.WriteLine("Open     \t To gain access to an account.");
            Console.WriteLine("Withdraw \t To withdraw from an account.");
            Console.WriteLine("Audit    \t To get accounts having maximum audit balance.");
            Console.WriteLine("Lowest   \t To get the account with lowest balance.");
            Console.WriteLine("Clear    \t To clear the console window.");
        }
    }
}