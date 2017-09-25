using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TevenStudiosBudgetTracker.Models
{
    public class Transaction
    {
        public TransactionContext context;

        public int ID { get; set; }

        public int UserId { get; set; }

        public string Description { get; set; }

        public string Date { get; set; }

        public Double Amount { get; set; }

        public int StatusId { get; set; }
    }

    public class TransactionContext
    {
        public string ConnectionString { get; set; }

        public TransactionContext(string connectionString)
        {
            this.ConnectionString = connectionString; 
        }
        public MySqlConnection getConnection()
        {
            return new MySqlConnection(ConnectionString); 
        }

        // Return all transactions for a user that were either approved or denied (not pending)
        public List<Transaction> GetAllPastRequests(int UserID)
        {
            List<Transaction> list = new List<Transaction>();

            using (MySqlConnection conn = getConnection())
            {
                conn.Open();
                // Find all transactions that are approved or denied
                MySqlCommand cmd = new MySqlCommand("select * from Transactions where UserId = " + UserID + " and StatusId != 0", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    // Loop through all past requests
                    while (reader.Read())
                    {
                        // Add all data to do with this past transaction
                        list.Add(new Transaction()
                        {
                            Date = reader["Date"].ToString(),
                            Amount = Convert.ToDouble(reader["Amount"]),
                            Description = reader["Description"].ToString(),
                        });
                    }
                }
            }
            return list;
        }

        // Returns all transactions approved or pending to the user specified
        private double getTotalTransactionAmount(int UserId)
        {
            double value = 0;
            using (MySqlConnection conn = getConnection())

            {
                conn.Open();
                // find all transactions of the user that are not denied (either pending or approved)
                MySqlCommand cmd = new MySqlCommand("select * from Transactions where UserId="+ UserId+" and not StatusId=2", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) // loops through all the values
                    {
                        value += Convert.ToDouble(reader["Amount"]); // adds each expenditure to the value
                    }
                }
            }
            return value; // returns the value of all expenditures added
        }

        // returns the value of the current budget using the current user's ID, Start Date and Start Budget
        public double getCurrentBudget(int userId, DateTime startDate, double startBudget, double maxBudget)
        {
            // calculate number of days between today and the start date
            DateTime today = DateTime.Now; // Today's date
            int numberOfDaysDifferent = (int) (today - startDate).TotalDays; // total days is cast down from their start date
            Console.WriteLine("Number of Days between " + today + " and " + startDate + " = " + numberOfDaysDifferent);

            // 1. calculate accrued budget = $3000 / number of days in year * the number of days in the year that have passed
            double currentBudget = numberOfDaysDifferent * (maxBudget / 365);
            Console.WriteLine("Current Budget = " + currentBudget);
            
            // Get current amount spent by looping through the transactions
            double userTransactions = getTotalTransactionAmount(userId);
            Console.WriteLine("User has spent = " + userTransactions);

            // Calculate remaining value = the start budget + the current budget - all approved or pending spendings
            double remainingValue = startBudget + currentBudget - userTransactions;

            if (remainingValue >= maxBudget) // capped at the maximum budget
            {
                remainingValue = maxBudget;
            }

            Console.WriteLine("User has remaining budget of " + remainingValue);
            return Math.Round(remainingValue, 2); ; // return value rounded to 2dp
        }

    }
}
