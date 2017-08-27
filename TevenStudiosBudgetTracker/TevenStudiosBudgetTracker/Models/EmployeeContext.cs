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

        public int SaveUserDetails(User user)
        {
            using (MySqlConnection conn = getConnection())
            {
                DateTime dateTimeNow = DateTime.Now;
                string startDate = dateTimeNow.ToString("yyyy-MM-dd HH:mm:ss");

                //string startDate = "2001-09-11 08:45:00";
                string query;
                if (user.ManagerId.Equals(-1)) // If no manager
                {
                    query = "insert into User(Name, Email, StartDate, RoleId, StartBudget) values('" + user.Name + "','" + user.Email + "','" + startDate +
                    "','" + user.RoleId + "','" + user.StartBudget + "')";
                }
                else // If has a manager
                {
                    query = "insert into User(Name, Email, StartDate, ManagerId, RoleId, StartBudget) values('" + user.Name + "','" + user.Email + "','" + startDate +
                    "','" + user.ManagerId + "','" + user.RoleId + "','" + user.StartBudget + "')";
                }

                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                conn.Close();
                return i;
            }

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
        public List<User> Managers { get; set; }
    }
}
