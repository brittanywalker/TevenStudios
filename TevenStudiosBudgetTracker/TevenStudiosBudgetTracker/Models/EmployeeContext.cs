using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

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

        public double ChangeAnnualBudget { get; set; }

        public DateTime ChangeAnnualBudgetDate { get; set; }
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
                            ChangeAnnualBudget = Convert.ToDouble(reader["ChangeAnnualBudget"]),
                            ChangeAnnualBudgetDate = Convert.ToDateTime(reader["ChangeAnnualBudgetDate"]),
                        });
                    }
                }
            }
            return list;
        }

        public List<User> GetEmployeesForManager(int managerId)
        {
            List<User> list = new List<User>();

            using (MySqlConnection conn = getConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from User where ManagerId = @id", conn);
                cmd.Parameters.AddWithValue("@id", managerId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new User()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Name = reader["Name"].ToString(),
                            Email = reader["Email"].ToString(),
                            StartDate = Convert.ToDateTime(reader["StartDate"]),
                            ManagerId = Convert.ToInt32(reader["ManagerId"]),
                            RoleId = Convert.ToInt32(reader["RoleId"]),
                            StartBudget = Convert.ToDouble(reader["StartBudget"]),
                            AnnualBudget = Convert.ToDouble(reader["AnnualBudget"]),
                        });
                    }
                }
            }
            return list;
        }

        public User GetUserByEmail(string email)
        {
            User user = new User();

            using (MySqlConnection conn = getConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from User where Email='" + email + "'", conn);

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
                        user.ChangeAnnualBudget = Convert.ToDouble(reader["ChangeAnnualBudget"]);
                        user.ChangeAnnualBudgetDate = Convert.ToDateTime(reader["ChangeAnnualBudgetDate"]);
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

                MySqlCommand cmd = new MySqlCommand("select * from User where RoleId = 2 ", conn);

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
                            AnnualBudget = Convert.ToDouble(reader["AnnualBudget"]),
                        });
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
	
                 string query;		
                 if (user.ManagerId.Equals(-1)) // If no manager		
                 {		
                     query = "insert into User(Name, Email, StartDate, RoleId, StartBudget, AnnualBudget, ChangeAnnualBudget, ChangeAnnualBudgetDate) values('" + user.Name + "','" + user.Email + "','" + startDate +		
                        "','" + user.RoleId + "','" + user.StartBudget + "','" + user.AnnualBudget + "','0.00','" + startDate + "')";		
                 }		
                 else // If has a manager		
                 {		
                     query = "insert into User(Name, Email, StartDate, ManagerId, RoleId, StartBudget, AnnualBudget, ChangeAnnualBudget, ChangeAnnualBudgetDate) values('" + user.Name + "','" + user.Email + "','" + startDate +		
                        "','" + user.ManagerId + "','" + user.RoleId + "','" + user.StartBudget + "','" + user.AnnualBudget + "','0.00','" + startDate + "')";
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
                    StartDate = Convert.ToDateTime(reader["StartDate"]),
                    ManagerId = manager,
                    RoleId = Convert.ToInt32(reader["RoleId"]),
                    StartBudget = Convert.ToDouble(reader["StartBudget"]),
                    AnnualBudget = Convert.ToDouble(reader["AnnualBudget"]),
                    ChangeAnnualBudget = Convert.ToDouble(reader["ChangeAnnualBudget"]),
                    ChangeAnnualBudgetDate = Convert.ToDateTime(reader["ChangeAnnualBudgetDate"])
                };
                conn.Close();
            }

            return currentUser;
        }

        public int EditUserSQL(User user)
        {
            using (MySqlConnection conn = getConnection())
            {
                string query;

                DateTime today = DateTime.Now; // current time
                string todayString = today.ToString("yyyy-MM-dd HH:mm:ss");
                User userDetails = retrieveUserDetails(user.ID); // gets all the user's details (inc. Change Annual Budget & Date)
                int numberOfDaysDifferent = (int)(today - userDetails.ChangeAnnualBudgetDate).TotalDays; // days between today & last budget change
                double currentBudget = numberOfDaysDifferent * (userDetails.AnnualBudget / 365); // value of days * accrued budget
                double changeBudget = userDetails.ChangeAnnualBudget + currentBudget; // add this value to the current saved since last change

                if (user.ManagerId.Equals(-1)) // If no manager
                {
                    query = "UPDATE User SET Name = '" + user.Name + "', Email = '" + user.Email +
                        "', ManagerId = NULL" +
                        ", RoleId = '" + user.RoleId + "', StartBudget = '" + user.StartBudget +
                        "', AnnualBudget = '" + user.AnnualBudget + 
                        "', ChangeAnnualBudget = '" + changeBudget + "', ChangeAnnualBudgetDate = '" + todayString + 
                        "' WHERE ID = '" + user.ID + "'";
                }
                else // If has a manager
                {
                    query = "UPDATE User SET Name = '" + user.Name + "', Email = '" + user.Email +
                        "', ManagerId = '" + user.ManagerId +
                        "', RoleId = '" + user.RoleId + "', StartBudget = '" + user.StartBudget +
                        "', AnnualBudget = '" + user.AnnualBudget +
                        "', ChangeAnnualBudget = '" + changeBudget + "', ChangeAnnualBudgetDate = '" + todayString +
                        "' WHERE ID = '" + user.ID + "'";
                }

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
        public User CurrentUser { get; set; }
        public List<User> Users { get; set; }
        public int CurrentUserIndex;
        public List<User> Managers { get; set; }
        public User currentEditUser { get; set; }
    }

    public class ManagerViewData
    {
        public User CurrentUser { get; set; }
        public List<User> Employees { get; set; }
        public User SelectedEmployee { get; set; }
        public List<PendingRequest> PendingRequests { get; set; }
        public List<Transaction> PastRequests { get; set; }
    }

    public class PendingRequest
    {
        public PendingRequestsContext context;
        public string Date { get; set; }
        public string Cost { get; set; }
        public string Description { get; set; }
        public string ID { get; set; }
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
                            ID = reader["ID"].ToString(),
                        });
                    }
                }
            }
            return list;
        }

        public int SubmitPendingRequest(PendingRequest newRequest, int userId)
        {
            using (MySqlConnection conn = getConnection())
            {
                DateTime dateTimeNow = DateTime.Now;
                string startDate = dateTimeNow.ToString("yyyy-MM-dd HH:mm:ss");
	
                string query = "insert into Transactions(UserId, Date, Description, Amount, StatusId) values('" + userId + "','" + newRequest.Date + "','" + newRequest.Description +
                    "','" + newRequest.Cost + "','" + 0 + "')";  // this status of 0 is pending and should be refactored to be an global variable later 

                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                conn.Close();
                return i;
            }
        }

        public int ApprovePendingRequest(string ID)
        {
            using (MySqlConnection conn = getConnection())
            {
                string query = "Update Transactions Set StatusId = '1' Where ID = '" + ID + "'";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                conn.Close();
                return i;
            }
        }

        public int DeclinePendingRequest(string ID)
        {
            using (MySqlConnection conn = getConnection())
            {
                string query = "Update Transactions Set StatusId = '2' Where ID = '" + ID + "'";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                conn.Open();
                int i = cmd.ExecuteNonQuery();
                conn.Close();
                return i;
            }
        }
    }
}