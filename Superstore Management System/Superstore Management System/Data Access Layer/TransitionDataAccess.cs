using Superstore_Management_System.Entitites;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Superstore_Management_System.Data_Access_Layer
{
    class TransitionDataAccess:DataAccess
    {
        public bool InsertTransition(Transition transition)
        {
            string sql = "INSERT INTO transitions(details,totalPrice, time) VALUES('" + transition.Details + "'," + transition.TotalPrice + ", '" + transition.Time + "')";
            int result = this.ExecuteQuery(sql);
            if (result > 0)
                return true;
            else
                return false;
        }

        public List<Transition> GetAllTransitions()
        {
            string sql = "SELECT * FROM transitions";
            List<Transition> transitions = new List<Transition>();
            SqlDataReader reader = this.GetData(sql);
            while (reader.Read())
            {
                Transition transition = new Transition();
                transition.TransitionID = (int)reader["transitionID"];
                transition.Details = reader["details"].ToString();
                transition.TotalPrice = (double)reader["totalPrice"];
                transition.Time = reader["time"].ToString();
                transitions.Add(transition);
            }
            return transitions;
        }

        public List<Transition> SearchTransitions(string value)
        {
            string sql = "SELECT * FROM transitions WHERE transitionID LIKE '" + value + "%'";
            List<Transition> transitions = new List<Transition>();
            SqlDataReader reader = this.GetData(sql);
            while (reader.Read())
            {
                Transition transition = new Transition();
                transition.TransitionID = (int)reader["transitionID"];
                transition.Details = reader["details"].ToString();
                transition.TotalPrice = (double)reader["totalPrice"];
                transition.Time = reader["time"].ToString();
                transitions.Add(transition);
            }
            return transitions;
        }

        public double SumOfSells()
        {
            string sql = "SELECT SUM(totalPrice) FROM transitions";
            var outputParam = this.GetScaler(sql);
            if (!(outputParam is DBNull))
                return Convert.ToInt64(outputParam);
            return 0;
        }
    }
}
