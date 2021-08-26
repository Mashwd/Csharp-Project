using Superstore_Management_System.Entitites;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Superstore_Management_System.Data_Access_Layer
{
    class BrandDataAccess : DataAccess
    {
        public bool CreateBrand(Brand brand)
        {
            string sql = "INSERT INTO brands(brandName,totalProducts,brandStatus,discount) VALUES('" + brand.BrandName + "'," + brand.NoOfProducts + ",'" + brand.BrandStatus + "', " + brand.Discount + ")";
            int result = this.ExecuteQuery(sql);
            if (result > 0)
                return true;
            else
                return false;
        }

        public bool ValidateBrand(string brandName)
        {
            string sql = "SELECT * FROM brands WHERE brandName= '" + brandName + "'";
            SqlDataReader reader = this.GetData(sql);
            if (reader.HasRows)
                return true;
            else
                return false;
        }

        public bool ValidateBrandID(string brandID)
        {
            string sql = "SELECT * FROM brands WHERE brandID= '" + brandID + "'";
            SqlDataReader reader = this.GetData(sql);
            if (reader.HasRows)
                return true;
            else
                return false;
        }

        public bool ValidateBrandName(string brandName)
        {
            string sql = "SELECT * FROM brands WHERE brandName= '" + brandName + "'";
            SqlDataReader reader = this.GetData(sql);
            if (reader.HasRows)
                return true;
            else
                return false;
        }

        public Brand GetBrandByID(string brandID)
        {
            string sql = "SELECT * FROM brands WHERE brandID= '" + brandID + "'";
            SqlDataReader reader = this.GetData(sql);
            Brand brand = new Brand();
            if (reader.HasRows)
            {
                reader.Read();
                brand.BrandID = (int)reader["brandID"];
                brand.BrandName = reader["brandName"].ToString();
                brand.BrandStatus = reader["brandStatus"].ToString();
                brand.NoOfProducts = (int)reader["totalProducts"];
                brand.Discount = (double)reader["discount"];
            }

            return brand;
        }

        public List<Brand> GetAllBrand()
        {
            string sql = "SELECT * FROM brands";
            SqlDataReader reader = this.GetData(sql);
            List<Brand> brands = new List<Brand>();
            while (reader.Read())
            {
                Brand brand = new Brand();
                brand.BrandID = (int)reader["brandID"];
                brand.BrandName = reader["brandName"].ToString();
                brand.BrandStatus = reader["brandStatus"].ToString();
                brand.NoOfProducts = (int)reader["totalProducts"];
                brand.Discount = (double)reader["discount"];
                brands.Add(brand);
            }
            return brands;
        }

        public List<Brand> SearchBrand(string value)
        {
            string sql = "SELECT * FROM brands WHERE brandName LIKE '" + value + "%'";
            List<Brand> brands = new List<Brand>();
            SqlDataReader reader = this.GetData(sql);
            while (reader.Read())
            {
                Brand brand = new Brand();
                brand.BrandID = (int)reader["brandID"];
                brand.BrandName = reader["brandName"].ToString();
                brand.BrandStatus = reader["brandStatus"].ToString();
                brand.NoOfProducts = (int)reader["totalProducts"];
                brand.Discount = (double)reader["discount"];
                brands.Add(brand);
            }
            return brands;
        }

        public bool UpdateBrand(Brand brand)
        {
            string sql = "UPDATE brands SET discount=" + brand.Discount + " WHERE brandID=" + brand.BrandID + "";
            int res = this.ExecuteQuery(sql);
            if (res == 1)
                return true;
            else
                return false;
        }

        public bool DeleteBrand(string brandName)
        {
            string sql = "DELETE FROM brands WHERE brandName='" + brandName + "'";
            int res = this.ExecuteQuery(sql);
            if (res == 1)
                return true;
            else
                return false;
        }

        public int GetBrandProductsQuantity(string name)
        {
            string sql = "SELECT SUM(quantity) FROM products WHERE brandName='" + name + "'";
            int reader = (int)this.GetScaler(sql);
            return reader;
        }

        public void UpdateBrandTotalProducts(string name, int products)
        {
            string sql = "UPDATE brands SET totalProducts=" + products + " WHERE brandName='" + name + "'";
            this.ExecuteQuery(sql);
        }

        public void AlterBrands()
        {
            string sql = "DBCC CHECKIDENT ('brands', RESEED, 0)";
            SqlDataReader reader = this.GetData(sql);
        }

        public bool IsEmpty()
        {
            string sql = "SELECT * FROM brands";
            SqlDataReader reader = this.GetData(sql);
            if (reader.HasRows)
                return true;
            else
                return false;
        }
        public List<BrandQuantity> GetAllBrandQuantity()
        {
            string sql = "SELECT brandID,brandName,totalProducts FROM brands";
            SqlDataReader reader = this.GetData(sql);
            List<BrandQuantity> brands = new List<BrandQuantity>();
            while (reader.Read())
            {
                BrandQuantity brand = new BrandQuantity();
                brand.BrandID = (int)reader["brandID"];
                brand.BrandName = reader["brandName"].ToString();
                brand.Quantity = (int)reader["totalProducts"];
                brands.Add(brand);
            }
            return brands;
        }

        public List<BrandQuantity> GetSpecificBrandQuantity(string brandName)
        {
            string sql = "SELECT brandID,brandName,totalProducts FROM brands WHERE brandName='" + brandName + "'";
            SqlDataReader reader = this.GetData(sql);
            List<BrandQuantity> brands = new List<BrandQuantity>();
            if (reader.Read())
            {
                BrandQuantity brand = new BrandQuantity();
                brand.BrandID = (int)reader["brandID"];
                brand.BrandName = reader["brandName"].ToString();
                brand.Quantity = (int)reader["totalProducts"];
                brands.Add(brand);
            }
            return brands;
        }

        public List<CategoryWiseBrand> GetCategoryWiseBrandListByCategoryName(string categoryName)
        {
            string sql = "SELECT brandName,categoryName FROM products WHERE categoryName='" + categoryName + "'";
            SqlDataReader reader = this.GetData(sql);
            List<CategoryWiseBrand> brands = new List<CategoryWiseBrand>();
            while (reader.Read())
            {
                CategoryWiseBrand brand = new CategoryWiseBrand();
                brand.BrandName = reader["brandName"].ToString();
                brand.CategoryName = reader["categoryName"].ToString();
                brands.Add(brand);
            }
            return brands;
        }

        public List<CategoryWiseBrand> GetCategoryWiseBrandList()
        {
            string sql = "SELECT brandName,categoryName FROM products";
            SqlDataReader reader = this.GetData(sql);
            List<CategoryWiseBrand> brands = new List<CategoryWiseBrand>();
            while (reader.Read())
            {
                CategoryWiseBrand brand = new CategoryWiseBrand();
                brand.BrandName = reader["brandName"].ToString();
                brand.CategoryName = reader["categoryName"].ToString();
                brands.Add(brand);
            }
            return brands;
        }

        public List<string> GetAllUnblockedBrandName()
        {
            string sql = "SELECT brandName FROM brands WHERE brandStatus= 'Unblock'";
            SqlDataReader reader = this.GetData(sql);
            List<string> brands = new List<string>();
            while (reader.Read())
            {
                string brand;
                brand = reader["brandName"].ToString();
                brands.Add(brand);
            }
            return brands;
        }

        public List<string> GetAllBlockedBrandName()
        {
            string sql = "SELECT brandName FROM brands WHERE brandStatus= 'Block'";
            SqlDataReader reader = this.GetData(sql);
            List<string> brands = new List<string>();
            while (reader.Read())
            {
                string brand;
                brand = reader["brandName"].ToString();
                brands.Add(brand);
            }
            return brands;
        }

        public bool BlockBrand(string brandName)
        {
            string status = "Block";
            string sql = "UPDATE brands SET brandStatus='" + status + "' WHERE brandName='" + brandName + "'";
            int res = this.ExecuteQuery(sql);
            if (res > 0)
                return true;
            else
                return false;
        }

        public bool UnblockBrand(string brandName)
        {
            string status = "Unblock";
            string sql = "UPDATE brands SET brandStatus='" + status + "' WHERE brandName='" + brandName + "'";
            int res = this.ExecuteQuery(sql);
            if (res > 0)
                return true;
            else
                return false;
        }

        public bool UpdateBrandDiscount(Brand brand)
        {
            string sql = "UPDATE brands SET discount=" + brand.Discount + " WHERE brandName='" + brand.BrandName + "'";
            int res = this.ExecuteQuery(sql);
            if (res == 1)
                return true;
            else
                return false;
        }

        public bool ValidateBrandAccess(string brandName)
        {
            string sql = "SELECT brandStatus FROM brands WHERE brandName= '" + brandName + "'";
            SqlDataReader reader = this.GetData(sql);
            reader.Read();
            if (reader["brandStatus"].ToString() == "Unblock")
                return true;
            else
                return false;
        }

    }
}
