using Superstore_Management_System.Entitites;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Superstore_Management_System.Data_Access_Layer
{
    class ProductDataAccess: DataAccess
    {
        public bool CreateProduct(Product product)
        {
            string sql = "INSERT INTO products(productName,price,quantity,productStatus,discount,brandName,categoryName) VALUES('" + product.ProductName + "'," + product.Price + "," + product.Quantity + ", '" + product.ProductStatus + "', "+ product.Discount +", '" + product.ProductBrand + "', '" + product.ProductCategory + "')";
            int result = this.ExecuteQuery(sql);
            if (result > 0)
            {
                return true;
            }
            else
                return false;
        }

        public bool ValidateProductName(string name)
        {
            string sql = "SELECT * FROM products WHERE productName= '" + name + "'";
            SqlDataReader reader = this.GetData(sql);
            if (reader.HasRows)
                return true;
            else
                return false;
        }

        public List<string> GetBrandNames()
        {
            string sql = "SELECT brandName FROM brands";
            List<string> brandNames = new List<string>();
            SqlDataReader reader = this.GetData(sql);
            while(reader.Read())
            {
                string name = reader["brandName"].ToString();
                brandNames.Add(name);
            }
            brandNames.Add("None");
            return brandNames;
        }

        public List<string> GetCategoryNames()
        {
            string sql = "SELECT categoryName FROM categories";
            List<string> categoryNames = new List<string>();
            SqlDataReader reader = this.GetData(sql);
            while (reader.Read())
            {
                string name = reader["categoryName"].ToString();
                categoryNames.Add(name);
            }
            categoryNames.Add("None");
            return categoryNames;
        }

        public List<Product> GetAllProduct()
        {
            string sql = "SELECT * FROM products";
            List<Product> products = new List<Product>();
            SqlDataReader reader = this.GetData(sql);
            while (reader.Read())
            {
                Product product = new Product();
                product.ProductID = (int)reader["productID"];
                product.ProductName = reader["productName"].ToString();
                product.Price = (double)reader["price"];
                product.Quantity = (int)reader["quantity"];
                product.ProductStatus = reader["productStatus"].ToString();
                product.Discount = (double)reader["discount"];
                product.ProductBrand = reader["brandName"].ToString();
                product.ProductCategory = reader["categoryName"].ToString();
                products.Add(product);
            }
            return products;
        }

        public List<Product> SearchProduct(string value)
        {
            string sql = "SELECT * FROM products WHERE productName LIKE '" + value + "%'";
            List<Product> products = new List<Product>();
            SqlDataReader reader = this.GetData(sql);
            while (reader.Read())
            {
                Product product = new Product();
                product.ProductID = (int)reader["productID"];
                product.ProductName = reader["productName"].ToString();
                product.Price = (double)reader["price"];
                product.Quantity = (int)reader["quantity"];
                product.ProductStatus = reader["productStatus"].ToString();
                product.Discount = (double)reader["discount"];
                product.ProductBrand = reader["brandName"].ToString();
                product.ProductCategory = reader["categoryName"].ToString();
                products.Add(product);
            }
            return products;
        }

        public Product GetProductByID(string productID)
        {
            string sql = "SELECT * FROM products WHERE productID='" + productID + "'";
            Product product = new Product();
            SqlDataReader reader = this.GetData(sql);
            if (reader.HasRows)
            {
                reader.Read();
                product.ProductID = (int)reader["productID"];
                product.ProductName = reader["productName"].ToString();
                product.Price = (double)reader["price"];
                product.Quantity = (int)reader["quantity"];
                product.ProductStatus = reader["productStatus"].ToString();
                product.Discount = (double)reader["discount"];
                product.ProductBrand = reader["brandName"].ToString();
                product.ProductCategory = reader["categoryName"].ToString();
                return product;
            }
            else
            {
                return null;
            }
            
        }

        public Product GetProductByName(string productName)
        {
            string sql = "SELECT * FROM products WHERE productName='" + productName + "'";
            Product product = new Product();
            SqlDataReader reader = this.GetData(sql);
            if (reader.HasRows)
            {
                reader.Read();
                product.ProductID = (int)reader["productID"];
                product.ProductName = reader["productName"].ToString();
                product.Price = (double)reader["price"];
                product.Quantity = (int)reader["quantity"];
                product.ProductStatus = reader["productStatus"].ToString();
                product.Discount = (double)reader["discount"];
                product.ProductBrand = reader["brandName"].ToString();
                product.ProductCategory = reader["categoryName"].ToString();
                return product;
            }
            else
            {
                return null;
            }

        }

        public bool UpdateProduct(Product product)
        {
            string sql = "UPDATE products SET price=" + product.Price + ", Quantity=" + product.Quantity + ", brandName='" + product.ProductBrand + "',categoryName='" + product.ProductCategory + "' WHERE productID=" + product.ProductID + "";
            int res = this.ExecuteQuery(sql);
            if (res == 1)
                return true;
            else
                return false;
        }

        public bool DeleteProduct(string productName)
        {
            string sql = "DELETE FROM products WHERE productName='" + productName + "'";
            int res = this.ExecuteQuery(sql);
            if (res == 1)
                return true;
            else
                return false;
        }

        public bool DeleteProductByBrandName(string brandName)
        {
            string sql = "DELETE FROM products WHERE brandName='" + brandName + "'";
            int res = this.ExecuteQuery(sql);
            if (res == 1)
                return true;
            else
                return false;
        }

        public bool DeleteProductByCategoryName(string categoryName)
        {
            string sql = "DELETE FROM products WHERE categoryName='" + categoryName + "'";
            int res = this.ExecuteQuery(sql);
            if (res == 1)
                return true;
            else
                return false;
        }

        public void AlterProducts()
        {
            string sql = "DBCC CHECKIDENT ('products', RESEED, 0)";
            SqlDataReader reader = this.GetData(sql);
        }

        public bool IsEmpty()
        {
            string sql = "SELECT * FROM products";
            SqlDataReader reader = this.GetData(sql);
            if (reader.HasRows)
                return true;
            else
                return false;
        }

        public List<ProductQuantity> GetAllProductQuantity()
        {
            string sql = "SELECT * FROM products";
            List<ProductQuantity> products = new List<ProductQuantity>();
            SqlDataReader reader = this.GetData(sql);
            while (reader.Read())
            {
                ProductQuantity product = new ProductQuantity();
                product.ProductID = (int)reader["productID"];
                product.ProductName = reader["productName"].ToString();
                product.Quantity = (int)reader["quantity"];
                product.ProductBrand = reader["brandName"].ToString();
                products.Add(product);
            }
            return products;
        }

        public List<int> GetProductID()
        {
            string sql = "SELECT productID FROM products";
            List<int> productsID = new List<int>();
            SqlDataReader reader = this.GetData(sql);
            while (reader.Read())
            {
                int id = (int)reader["productID"];
                productsID.Add(id);
            }
            productsID.Add(0);
            return productsID;
        }

        public List<ProductQuantity> GetSpecificProductQuantity(string productID)
        {
            string sql = "SELECT * FROM products WHERE productID=" + productID + "";
            SqlDataReader reader = this.GetData(sql);
            List<ProductQuantity> products = new List<ProductQuantity>();
            if (reader.Read())
            {
                ProductQuantity product = new ProductQuantity();
                product.ProductID = (int)reader["productID"];
                product.ProductName = reader["productName"].ToString();
                product.Quantity = (int)reader["quantity"];
                product.ProductBrand = reader["brandName"].ToString();
                products.Add(product);
            }
            return products;
        }

        public List<Product> GetProductByBrandName(string brandName)
        {
            string sql = "SELECT * FROM products WHERE brandName LIKE '" + brandName + "%'";
            List<Product> products = new List<Product>();
            SqlDataReader reader = this.GetData(sql);
            while (reader.Read())
            {
                Product product = new Product();
                product.ProductID = (int)reader["productID"];
                product.ProductName = reader["productName"].ToString();
                product.Price = (double)reader["price"];
                product.Quantity = (int)reader["quantity"];
                product.ProductStatus = reader["productStatus"].ToString();
                product.Discount = (double)reader["discount"];
                product.ProductBrand = reader["brandName"].ToString();
                product.ProductCategory = reader["categoryName"].ToString();
                products.Add(product);
            }
            return products;

        }

        public List<CategoryWiseProduct> GetCategoryWiseProductListByCategoryName(string categoryName)
        {
            string sql = "SELECT productID,productName,categoryName FROM products WHERE categoryName='" + categoryName + "'";
            SqlDataReader reader = this.GetData(sql);
            List<CategoryWiseProduct> products = new List<CategoryWiseProduct>();
            while (reader.Read())
            {
                CategoryWiseProduct product = new CategoryWiseProduct();
                product.ProductID = (int)reader["productID"];
                product.ProductName = reader["productName"].ToString();
                product.Category = reader["categoryName"].ToString();
                products.Add(product);
            }
            return products;
        }

        public List<CategoryWiseProduct> GetCategoryWiseProductList()
        {
            string sql = "SELECT  productID,productName,categoryName FROM products";
            SqlDataReader reader = this.GetData(sql);
            List<CategoryWiseProduct> products = new List<CategoryWiseProduct>();
            while (reader.Read())
            {
                CategoryWiseProduct product = new CategoryWiseProduct();
                product.ProductID = (int)reader["productID"];
                product.ProductName = reader["productName"].ToString();
                product.Category = reader["categoryName"].ToString();
                products.Add(product);
            }
            return products;
        }

        public int ValidateQuantity(string productName)
        {
            string sql = "SELECT  quantity FROM products WHERE productName='" + productName + "'";
            SqlDataReader reader = this.GetData(sql);
            Product product = new Product();
            if (reader.Read())
            {
                product.ProductID = (int)reader["quantity"];
            }
            return product.ProductID;
        }

        public string ValidateProductAccess(string productName)
        {
            string sql = "SELECT  productStatus FROM products WHERE productName='" + productName + "'";
            SqlDataReader reader = this.GetData(sql);
            string productStatus = "";
            if (reader.Read())
            {
                productStatus = reader["productStatus"].ToString();
            }
            return productStatus;
        }

        public void UpdateProductQuantity(int quantity, string productName)
        {
            string sql = "UPDATE products SET quantity=" + quantity+ " WHERE productName='" + productName + "'";
            int res = this.ExecuteQuery(sql);
        }

        public double SumOfAllProduct()
        {
            string sql = "SELECT SUM(price) FROM products";
            var outputParam = this.GetScaler(sql);
            if (!(outputParam is DBNull))
                return Convert.ToInt64(outputParam);
            return 0;
        }

        public bool UpdateProductDiscount(int amount, string productID)
        {
            string sql = "UPDATE products SET discount=" + amount + " WHERE productID='" + productID + "'";
            int res = this.ExecuteQuery(sql);
            if (res > 0)
                return true;
            else
                return false;
        }
    }
}
