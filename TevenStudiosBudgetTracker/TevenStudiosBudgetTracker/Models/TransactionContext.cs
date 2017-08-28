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

        public DateTime StartDate { get; set; }

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

        public List<Transaction> GetAllTransactions()
        {
            List<Transaction> list = new List<Transaction>();

            using (MySqlConnection conn = getConnection())

            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Transactions", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        
                        list.Add(new Transaction()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            UserId = Convert.ToInt32(reader["UserId"]),
                            StatusId = Convert.ToInt32(reader["StatusId"]),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            Description = reader["Description"].ToString(),
                            Amount = Convert.ToDouble(reader["Amount"])
                        });
                    }
                }
            }
            return list;
        }

        private double getTotalTransactionAmount(int UserId)
        {
            double value = 0;
            using (MySqlConnection conn = getConnection())

            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Transactions where UserId="+ UserId+" and StatusId=1", conn); // TODO: add pending, therefore change to and not StatusId=denied

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        value += Convert.ToDouble(reader["Amount"]);
                    }
                }
            }
            return value;
        }

        public double getCurrentBudget(DateTime startDate, double startBudget)
        {
            // calculate time from start_date
            // 1. Get Start Date
            // 2. set datetime to startdate

            // calculate number of days
            // 1. Get today's date
            DateTime today = DateTime.Now;
            // 2. find number of days between start_date & today

            double numberOfDaysDifferent = (today - startDate).TotalDays //StartDate - today

            // add to value
            // 1. calculate accrued budget = number of days in year
            double currentBudget = numberOfDaysDifferent * (3000 / 365);
                // NOTE: might have been added over a year ago?
                // NOTE: leap year?
            
            // 2. get current amount spent
            double userTransactions = getTotalTransactionAmount(userId);
            // 3. get start budget

            // Calculate remaining value
            double remainingValue = startBudget + currentBudget - userTransactions;

            double totalSpent = userTransactions;

            return totalSpent;
        }

    }
}
