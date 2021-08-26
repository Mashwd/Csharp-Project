using Superstore_Management_System.Entitites;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Superstore_Management_System.Data_Access_Layer
{
    class ChartDataAccess:DataAccess
    {
        public bool InsertProductInChart(Chart chart)
        {
            string sql = "INSERT INTO chart(chartProduct,quantity, price, discount) VALUES('" + chart.Product + "'," + chart.Quantity + ", " + chart.Price + ", " + chart.Discount + ")";
            int result = this.ExecuteQuery(sql);
            if (result > 0)
                return true;
            else
                return false;
        }

        public List<Chart> GetAllProductFromChart()
        {
            string sql = "SELECT * FROM chart";
            List<Chart> charts = new List<Chart>();
            SqlDataReader reader = this.GetData(sql);
            while (reader.Read())
            {
                Chart chart = new Chart();
                chart.ChartID = (int)reader["chartID"];
                chart.Product = reader["chartProduct"].ToString();
                chart.Price = (double)reader["price"];
                chart.Discount = (double)reader["discount"];
                chart.Quantity = (int)reader["quantity"];
                charts.Add(chart);
            }
            return charts;
        }

        public bool RemoveProductFromChart(string chartID)
        {
            string sql = "DELETE FROM chart WHERE chartID='" + chartID + "'";
            int res = this.ExecuteQuery(sql);

            if (res > 0)
                return true;
            else
                return false;
        }

        public Chart GetProductFromChartByID(string chartID)
        {
            string sql = "SELECT * FROM chart WHERE chartID='" + chartID + "'";
            Chart chart = new Chart();
            SqlDataReader reader = this.GetData(sql);
            if (reader.Read())
            {
                chart.ChartID = (int)reader["chartID"];
                chart.Product = reader["chartProduct"].ToString();
                chart.Price = (double)reader["price"];
                chart.Discount = (double)reader["discount"];
                chart.Quantity = (int)reader["quantity"];
            }
            return chart;
        }

        public void AlterChart()
        {
            string sql = "DBCC CHECKIDENT ('chart', RESEED, 0)";
            SqlDataReader reader = this.GetData(sql);
        }

        public bool IsEmpty()
        {
            string sql = "SELECT * FROM chart";
            SqlDataReader reader = this.GetData(sql);
            if (reader.HasRows)
                return true;
            else
                return false;
        }

        public double SumOfChart()
        {
            string sql = "SELECT SUM(price) FROM chart";
            var outputParam = this.GetScaler(sql);
            if (!(outputParam is DBNull))
                return Convert.ToInt64(outputParam);
            return 0;
        }

        public double DiscountOfChart()
        {
            string sql = "SELECT SUM(discount) FROM chart";
            var outputParam = this.GetScaler(sql);
            if (!(outputParam is DBNull))
                return Convert.ToInt64(outputParam);
            return 0;
        }

        public void DeleteAll()
        {
            string sql = "DELETE FROM chart";
            int reader = this.ExecuteQuery(sql);
        }
    }
}
