using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Superstore_Management_System.Entitites;

namespace Superstore_Management_System.Data_Access_Layer
{
    class UserDataAccess: DataAccess
    {
        public bool UserRegister(User user)
        {
            string sql = "INSERT INTO users(userName,password,name,position,status,salary,joiningDate) VALUES('" + user.Username + "','" + user.Password + "','" + user.Name + "','" + user.Position + "','" + user.Status + "','" + user.Salary + "','" + user.JoiningDate + "')";
            int result = this.ExecuteQuery(sql);
            if (result > 0)
                return true;
            else
                return false;
        }

        public bool ValidateLogin(string username, string password)
        {
            string sql = "SELECT * FROM users WHERE userName='" + username + "' AND password='" + password + "'";
            SqlDataReader reader = this.GetData(sql);
            if (reader.HasRows)
                return true;
            else
                return false;
        }

        public User GetEmployeeByUsername(string username)
        {
            string sql = "SELECT * FROM users WHERE userName='" + username + "'";
            SqlDataReader reader = this.GetData(sql);
            User user = new User();
            if (reader.HasRows)
            {
                reader.Read();
                user.UserID = (int)reader["userID"];
                user.Username = reader["userName"].ToString();
                user.Password = reader["password"].ToString();
                user.Name = reader["name"].ToString();
                user.Position = reader["position"].ToString();
                user.Status = reader["status"].ToString();
                user.Salary = Convert.ToDouble(reader["salary"]);
                user.JoiningDate = reader["joiningDate"].ToString();
            }
            return user;
        }

        public User GetEmployeeByID(string userID)
        {
            string sql = "SELECT * FROM users WHERE userID='" + userID + "'";
            SqlDataReader reader = this.GetData(sql);
            User user = new User();
            if (reader.HasRows)
            {
                reader.Read();
                user.UserID = (int)reader["userID"];
                user.Username = reader["userName"].ToString();
                user.Password = reader["password"].ToString();
                user.Name = reader["name"].ToString();
                user.Position = reader["position"].ToString();
                user.Status = reader["status"].ToString();
                user.Salary = Convert.ToDouble(reader["salary"]);
                user.JoiningDate = reader["joiningDate"].ToString();
            }
            return user;
        }

        public List<User> GetAllEmployee()
        {
            string sql = "SELECT * FROM users";
            SqlDataReader reader = this.GetData(sql);
            List<User> users = new List<User>();
            while (reader.Read())
            {
                User user = new User();
                user.UserID = (int)reader["userID"];
                user.Username = reader["userName"].ToString();
                user.Password = reader["password"].ToString();
                user.Name = reader["name"].ToString();
                user.Position = reader["position"].ToString();
                user.Status = reader["status"].ToString();
                user.Salary = Convert.ToDouble(reader["salary"]);
                user.JoiningDate = reader["joiningDate"].ToString();
                users.Add(user);
            }
            return users;
        }

        public bool ValidateUsername(string username)
        {
            string sql = "SELECT * FROM users WHERE userName='" + username + "'";
            SqlDataReader reader = this.GetData(sql);
            if (reader.HasRows)
                return true;
            else
                return false;
        }

        public List<User> SearchEmployee(string value, bool flag)
        {
            if (flag == true)
            {
                string sql = "SELECT * FROM users WHERE userID LIKE '" + value + "%'";
                List<User> users = new List<User>();
                SqlDataReader reader = this.GetData(sql);
                while (reader.Read())
                {
                    User user = new User();
                    user.UserID = (int)reader["userID"];
                    user.Username = reader["userName"].ToString();
                    user.Password = reader["password"].ToString();
                    user.Name = reader["name"].ToString();
                    user.Position = reader["position"].ToString();
                    user.Status = reader["status"].ToString();
                    user.Salary = Convert.ToDouble(reader["salary"]);
                    user.JoiningDate = reader["joiningDate"].ToString();
                    users.Add(user);
                }
                return users;
            }
            else
            {
                string sql = "SELECT * FROM users WHERE position LIKE '" + value + "%'";
                List<User> users = new List<User>();
                SqlDataReader reader = this.GetData(sql);
                while (reader.Read())
                {
                    User user = new User();
                    user.UserID = (int)reader["userID"];
                    user.Username = reader["userName"].ToString();
                    user.Password = reader["password"].ToString();
                    user.Name = reader["name"].ToString();
                    user.Position = reader["position"].ToString();
                    user.Status = reader["status"].ToString();
                    user.Salary = Convert.ToDouble(reader["salary"]);
                    user.JoiningDate = reader["joiningDate"].ToString();
                    users.Add(user);
                }
                return users;
            }
        }

        public bool SuspendEmployee(string userID)
        {
            string status = "Suspend";
            string sql = "UPDATE users SET status='" + status + "' WHERE userID='" + userID + "'";
            int res = this.ExecuteQuery(sql);
            if (res == 1)
                return true;
            else
                return false;
        }

        public bool ProceedEmployee(string userID)
        {
            string status = "Proceed";            
            string sql = "UPDATE users SET status='"+status+"' WHERE userID='" + userID + "'";
            int res = this.ExecuteQuery(sql);
            if (res == 1)
                return true;
            else
                return false;
        }

        public List<string> ProceedEmployeeIDList()
        {
            string status = "Proceed";
            List<string> users = new List<string>();
            string sql = "SELECT userID FROM users WHERE status= '" + status + "' AND userID != 1";
            SqlDataReader reader = this.GetData(sql);
            while(reader.Read())
            {
                users.Add(reader["userID"].ToString());
            }
            return users;
        }

        public List<string> SuspendEmployeeIDList()
        {
            string status = "Suspend";
            List<string> users = new List<string>();
            string sql = "SELECT userID FROM users WHERE status= '" + status + "' AND userID != 1";
            SqlDataReader reader = this.GetData(sql);
            while (reader.Read())
            {
                users.Add(reader["userID"].ToString());
            }
            return users;
        }

        public bool UpdateEmployee(User user)
        {
            string sql = "UPDATE users SET password='" + user.Password + "',name='" + user.Name + "', salary=" + user.Salary + ", position='" + user.Position + "' WHERE userID=" + user.UserID + "";
            int res = this.ExecuteQuery(sql);
            if (res == 1)
                return true;
            else
                return false;
        }

        public bool DeleteEmployee(string userID)
        {
            string sql = "DELETE FROM users WHERE userID='" + userID + "'";
            int res = this.ExecuteQuery(sql);
            if (res == 1)
                return true;
            else
                return false;
        }
    }
}
