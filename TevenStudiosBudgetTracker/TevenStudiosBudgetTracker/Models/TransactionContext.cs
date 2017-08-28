﻿using MySql.Data.MySqlClient;
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
                // find all transactions of the user that are not denied (either pending or approved)
                MySqlCommand cmd = new MySqlCommand("select * from Transactions where UserId="+ UserId+" and not StatusId=2", conn);
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

        public double getCurrentBudget(int userId, DateTime startDate, double startBudget)
        {
            // calculate number of days between today and the start date
            DateTime today = DateTime.Now; // Today's date
            double numberOfDaysDifferent = (today - startDate).TotalDays; // TODO: Cast up or down number of days
            Console.WriteLine("Number of Days between " + today + " and " + startDate + " = " + numberOfDaysDifferent);

            // 1. calculate accrued budget = $3000 / number of days in year * the number of days in the year that have passed
            double currentBudget = numberOfDaysDifferent * (3000 / 365);
            Console.WriteLine("Current Budget = " + currentBudget);
                // NOTE: might have been added over a year ago & leap year
            
            // Get current amount spent by looping through the transactions
            double userTransactions = getTotalTransactionAmount(userId);
            Console.WriteLine("User has spent = " + userTransactions);

            // Calculate remaining value = the start budget + the current budget - all approved or pending spendings
            double remainingValue = startBudget + currentBudget - userTransactions;
            Console.WriteLine("User has remaining budget of " + remainingValue);

            return remainingValue;
        }

    }
}
