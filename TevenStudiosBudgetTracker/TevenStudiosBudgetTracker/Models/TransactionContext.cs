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

        //public string StartDate { get; set; }

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
                MySqlCommand cmd = new MySqlCommand("select * from Transactions where UserId="+ UserId+" and StatusId=1", conn);

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

        public List<Transaction> getCurrentBudget()
        {
            int userId = 1;
            double userTransactions = getTotalTransactionAmount(userId);
            // calculate time from start_date
            // calculate number of days
            // add to value
            // add startbudget            

            List<Transaction> newList = new List<Transaction>();
            Transaction obj = new Transaction();

            double totalSpent = userTransactions;

            obj.Amount = totalSpent;
            newList.Add(obj);
            return newList;
        }

    }
}
