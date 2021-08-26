using Superstore_Management_System.Entitites;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Superstore_Management_System.Data_Access_Layer
{
    class CategoryDataAccess: DataAccess
    {
        public bool CreateCategory(Category category)
        {
            string sql = "INSERT INTO categories(categoryName,totalProducts) VALUES('" + category.CategoryName + "'," + category.NoOfProducts + ")";
            int result = this.ExecuteQuery(sql);
            if (result > 0)
                return true;
            else
                return false;
        }

        public bool ValidateCategory(string categoryName)
        {
            string sql = "SELECT * FROM categories WHERE categoryName= '" + categoryName +"'";
            SqlDataReader reader = this.GetData(sql);
            if (reader.HasRows)
                return true;
            else
                return false;
        }

        public bool ValidateCategoryID(string categoryID)
        {
            string sql = "SELECT * FROM categories WHERE categoryID= '" + categoryID + "'";
            SqlDataReader reader = this.GetData(sql);
            if (reader.HasRows)
                return true;
            else
                return false;
        }

        public bool ValidateCategoryName(string categoryName)
        {
            string sql = "SELECT * FROM categories WHERE categoryName= '" + categoryName + "'";
            SqlDataReader reader = this.GetData(sql);
            if (reader.HasRows)
                return true;
            else
                return false;
        }

        public Category GetCategoryByID(string categoryID)
        {
            string sql = "SELECT * FROM categories WHERE categoryID= '" + categoryID + "'";
            SqlDataReader reader = this.GetData(sql);
            Category category = new Category();
            if (reader.HasRows)
            {
                reader.Read();
                category.CategoryID = (int)reader["categoryID"];
                category.CategoryName = reader["categoryName"].ToString();
                category.NoOfProducts = (int)reader["totalProducts"];
            }

            return category;
                
        }

        public List<Category> GetAllCategory()
        {
            string sql = "SELECT * FROM categories";
            SqlDataReader reader = this.GetData(sql);
            List<Category> categories = new List<Category>();
            while (reader.Read())
            {
                Category category = new Category();
                category.CategoryID = (int)reader["categoryID"];
                category.CategoryName = reader["categoryName"].ToString();
                category.NoOfProducts = (int)reader["totalProducts"];
                categories.Add(category);
            }
            return categories;
        }

        public List<Category> SearchCategory(string value)
        {
            string sql = "SELECT * FROM categories WHERE categoryName LIKE '" + value + "%'";
            List<Category> categories = new List<Category>();
            SqlDataReader reader = this.GetData(sql);
            while (reader.Read())
            {
                Category category = new Category();
                category.CategoryID = (int)reader["categoryID"];
                category.CategoryName = reader["categoryName"].ToString();
                category.NoOfProducts = (int)reader["totalProducts"];
                categories.Add(category);
            }
            return categories;
        }

        public bool UpdateCategory(Category category)
        {
            string sql = "UPDATE categories SET categoryName='" + category.CategoryName + "' WHERE categoryID=" + category.CategoryID + "";
            int res = this.ExecuteQuery(sql);
            if (res == 1)
                return true;
            else
                return false;
        }

        public bool DeleteCategory(string categoryName)
        {
            string sql = "DELETE FROM categories WHERE categoryName='" + categoryName + "'";
            int res = this.ExecuteQuery(sql);
            if (res == 1)
                return true;
            else
                return false;
        }

        public int GetCategoryProductsQuantity(string name)
        {
            string sql = "SELECT SUM(quantity) FROM products WHERE categoryName= '" + name + "'";
            Int32 reader = (Int32)this.GetScaler(sql);
            return reader;
        }

        public void UpdateCategoryTotalProducts(string name, int products)
        {
            string sql = "UPDATE categories SET totalProducts=" + products + " WHERE categoryName='" + name + "'";
            this.ExecuteQuery(sql);
        }

        public void AlterCategories()
        {
            string sql = "DBCC CHECKIDENT ('categories', RESEED, 0)";
            SqlDataReader reader = this.GetData(sql);
        }

        public bool IsEmpty()
        {
            string sql = "SELECT * FROM categories";
            SqlDataReader reader = this.GetData(sql);
            if (reader.HasRows)
                return true;
            else
                return false;
        }
    }
}
