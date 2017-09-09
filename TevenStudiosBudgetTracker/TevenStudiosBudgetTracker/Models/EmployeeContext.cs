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

        public DateTime StartDate { get; set; }

        public int ManagerId { get; set; }

        public int RoleId { get; set; }

        public double StartBudget { get; set; }

        public double AnnualBudget { get; set; }
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
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            ManagerId = manager,
                            RoleId = Convert.ToInt32(reader["RoleId"]),
                            StartBudget = Convert.ToDouble(reader["StartBudget"]),
                            AnnualBudget = Convert.ToDouble(reader["AnnualBudget"]),
                        });
                    }
                }
            }
            return list;
        }

        public User GetUser(int id)
        {
            User user = new User();

            using (MySqlConnection conn = getConnection())

            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from User where ID=" + id, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user.ID = Convert.ToInt32(reader["ID"]);
                        user.Name = reader["Name"].ToString();
                        user.Email = reader["Email"].ToString();
                        user.StartDate = Convert.ToDateTime(reader["StartDate"]);
                        user.RoleId = Convert.ToInt32(reader["RoleId"]);
                        user.StartBudget = Convert.ToDouble(reader["StartBudget"]);
                        user.AnnualBudget = Convert.ToDouble(reader["AnnualBudget"]);
                    }
                }
            }
            return user;
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

        public User retrieveUserDetails(int UserID)
        {
            User currentUser;

            using (MySqlConnection conn = getConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from User where ID=" + UserID, conn);

                var reader = cmd.ExecuteReader();
                reader.Read();
                var manager = reader.GetOrdinal("ManagerId");
                if (reader.IsDBNull(manager))
                {
                    manager = 0;
                }
                else
                {
                    manager = Convert.ToInt32(reader["ManagerId"]);
                }

                currentUser = new User()
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString(),

                    ManagerId = manager,

                    RoleId = Convert.ToInt32(reader["RoleId"]),
                    StartBudget = Convert.ToDouble(reader["StartBudget"]),
                };

                conn.Close();
            }
            return currentUser;
        }
    }

   

    public class AdminViewData
    {
        public List<User> Users { get; set; }
        public int CurrentUserIndex;
        public List<User> Managers { get; set; }
        public User currentEditUser { get; set; }
   
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
                            Date = reader["Date"].ToString(),
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
