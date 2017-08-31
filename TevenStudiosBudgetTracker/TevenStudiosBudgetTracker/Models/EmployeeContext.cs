using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TevenStudiosBudgetTracker.Models
{
    public class User
    {
        public UserContext context;

        public int ID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        //public string StartDate { get; set; }

        public int ManagerId { get; set; }

        public int RoleId { get; set; }

        public double StartBudget { get; set; }

    }

    public class RoleType
    {
        public int ID { get; set; }

        public string Type { get; set; }
    }

    public class UserContext
    {
        public string ConnectionString { get; set; }

        public UserContext(string connectionString)
        {
            this.ConnectionString = connectionString; 
        }
        public MySqlConnection getConnection()
        {
            return new MySqlConnection(ConnectionString); 
        }

        public List<User> GetAllUsers()
        {
            List<User> list = new List<User>();

            using (MySqlConnection conn = getConnection())

            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from User", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var manager = reader.GetOrdinal("ManagerId");
                        if (reader.IsDBNull(manager))
                        {
                            manager = 0; 
                        }
                        else
                        {
                            manager = Convert.ToInt32(reader["ManagerId"]);
                        }

                        list.Add(new User()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),

                            ManagerId = manager,

                            RoleId = Convert.ToInt32(reader["RoleId"]),
                            StartBudget = Convert.ToDouble(reader["StartBudget"]),
                        });
                        Console.WriteLine(Convert.ToInt32(reader["ID"]));
                    }
                }
            }
            return list;
        }

        public List<User> GetAllManagers()
        {
            List<User> list = new List<User>();

            using (MySqlConnection conn = getConnection())

            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from User where RoleId = 1 ", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var manager = reader.GetOrdinal("ManagerId");
                        if (reader.IsDBNull(manager))
                        {
                            manager = 0;
                        }
                        else
                        {
                            manager = Convert.ToInt32(reader["ManagerId"]);
                        }

                        list.Add(new User()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),

                            ManagerId = manager,

                            RoleId = Convert.ToInt32(reader["RoleId"]),
                            StartBudget = Convert.ToDouble(reader["StartBudget"]),
                        });
                        Console.WriteLine(Convert.ToInt32(reader["ID"]));
                    }
                }
            }
            return list;
        }
		
		public int DeleteUserSQL(int userID)
        {
            using (MySqlConnection conn = getConnection())
            {
                string query;
                
                query = "DELETE FROM User WHERE ID = '" + userID + "'";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                conn.Close();
                return i;
            }

        }
    }

    public class AdminViewData
    {
        public List<User> Users { get; set; }
        public int CurrentUserIndex;
        public List<User> Managers { get; set; }
    }

    public class PendingRequest
    {
        public PendingRequestsContext context;
        public string Date { get; set; }
        public string Cost { get; set; }
        public string Description { get; set; }
    }

    public class PendingRequestsContext
    {
        public string ConnectionString { get; set; }

        public PendingRequestsContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        public MySqlConnection getConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<PendingRequest> GetAllPendingRequests(int UserID)
        {
            List<PendingRequest> list = new List<PendingRequest>();

            using (MySqlConnection conn = getConnection())

            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Transactions where UserId = " + UserID + " and StatusId = 0", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new PendingRequest()
                        {
                            Date = reader["StartDate"].ToString(),
                            Cost = reader["Amount"].ToString(),
                            Description = reader["Description"].ToString(),
                        });
                    }
                }
            }
            return list;
        }

    }
}
