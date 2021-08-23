using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Superstore_Management_System.Data_Access_Layer;
using Superstore_Management_System.Entitites;
using Superstore_Management_System.Presntation_Layer;

namespace Superstore_Management_System.Presentation_Layer.Clerk
{
    public partial class ClerkPanelProductCUD : Form
    {
        private bool mouseDown;
        private Point offset;
        Thread thread;

        [Obsolete]
        public ClerkPanelProductCUD()
        {
            InitializeComponent();
        }

        private void ClerkPanelCategoriesCUD_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void topBarbunifuGradientPanel_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void topBarbunifuGradientPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown == true)
            {
                Point currentScreenPosition = PointToScreen(e.Location);
                Location = new Point(currentScreenPosition.X - offset.X, currentScreenPosition.Y - offset.Y);
            }
        }

        private void topBarbunifuGradientPanel_MouseDown(object sender, MouseEventArgs e)
        {
            offset.X = e.X;
            offset.Y = e.Y;

            mouseDown = true;
        }

        private void minimizeBunifuButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void closeBunifuButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ClerkPanelProductCUD_Load(object sender, EventArgs e)
        {
            productBunifuButton.ApplyState(productBunifuButton.OnPressedState);
            productBrandNameComboBox.DataSource = new ProductDataAccess().GetBrandNames();
            productBrandNameComboBox.SelectedItem = "None";
            productCategoryNameComboBox.DataSource = new ProductDataAccess().GetCategoryNames();
            productCategoryNameComboBox.SelectedItem = "None";
            productListBunifuDataGridView.DataSource = new ProductDataAccess().GetAllProduct();
            updateProductBrandNameComboBox.DataSource = new ProductDataAccess().GetBrandNames();
            updateProductBrandNameComboBox.SelectedItem = "None";
            updateProductCategoryNameComboBox.DataSource = new ProductDataAccess().GetCategoryNames();
            updateProductCategoryNameComboBox.SelectedItem = "None";
        }

        public void UpdateComboBoxs()
        {
            productBrandNameComboBox.DataSource = new ProductDataAccess().GetBrandNames();
            productCategoryNameComboBox.DataSource = new ProductDataAccess().GetCategoryNames();
        }

        [Obsolete]
        private void categoryBunifuButton_Click(object sender, EventArgs e)
        {
            productBunifuButton.ApplyState(productBunifuButton.OnIdleState);
            this.Close();
            thread = new Thread(openCategory);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        [Obsolete]
        private void openCategory()
        {
            Application.Run(new ClerkPanelCategoriesCUD());
        }

        [Obsolete]
        private void brandBunifuButton_Click_1(object sender, EventArgs e)
        {
            productBunifuButton.ApplyState(productBunifuButton.OnIdleState);
            this.Close();
            thread = new Thread(openbrand);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        [Obsolete]
        private void openbrand()
        {
            Application.Run(new ClerkPanelBrandCUD());
        }

        private void createProductResetBunifuButton_Click(object sender, EventArgs e)
        {
            ClearCreateFields();
        }

        public void ClearCreateFields()
        {
            createProductBunifuTextBox.Text = "";
            productPriceBunifuTextBox.Text = "";
            productQuantityBunifuTextBox.Text = "";
            productBrandNameComboBox.DataSource = new ProductDataAccess().GetBrandNames();
            productCategoryNameComboBox.DataSource = new ProductDataAccess().GetCategoryNames();
            surityBunifuCheckBox.Checked = false;
        }

        private void createProductBunifuButton_Click(object sender, EventArgs e)
        {
            if (createProductBunifuTextBox.Text == "" && productPriceBunifuTextBox.Text == "" &&
               productQuantityBunifuTextBox.Text == "" && productBrandNameComboBox.Text == "" &&
               productCategoryNameComboBox.Text == "")
            {
                MessageBox.Show("Required fields are empty.");
            }
            else
            {
                bool flag = false;
                Product product = new Product();
                if (createProductBunifuTextBox.Text == "")
                {
                    flag = true;
                    MessageBox.Show("Provide product name.");
                }
                else if (!flag)
                {
                    bool response = new ProductDataAccess().ValidateProductName(createProductBunifuTextBox.Text);
                    if (response)
                    {
                        flag = true;
                        MessageBox.Show("Product name already exists. You can update it from update section.");
                    }
                    else
                        product.ProductName = createProductBunifuTextBox.Text;
                }

                if (productPriceBunifuTextBox.Text == "" && flag == false)
                {
                    flag = true;
                    MessageBox.Show("Provide product price.");
                }
                else if (!flag)
                {
                    if (IsDouble(productPriceBunifuTextBox.Text) == true)
                    {
                        product.Price = Convert.ToDouble(productPriceBunifuTextBox.Text);
                    }
                    else
                    {
                        flag = true;
                        MessageBox.Show("Invalid product price.");
                    }
                }

                if (productQuantityBunifuTextBox.Text == "" && flag == false)
                {
                    flag = true;
                    MessageBox.Show("Provide product quantity.");
                }
                else if (!flag)
                {
                    if (productQuantityBunifuTextBox.Text.All(Char.IsNumber) == true)
                    {
                        product.Quantity = Convert.ToInt32(productQuantityBunifuTextBox.Text);
                    }
                    else
                    {
                        flag = true;
                        MessageBox.Show("Invalid product quantity.");
                    }
                }

                if ((productBrandNameComboBox.Text == "" || productBrandNameComboBox.Text == "None") && flag == false)
                {
                    flag = true;
                    MessageBox.Show("Select product brand name.");
                }
                else if (!flag)
                {
                    bool res = new BrandDataAccess().ValidateBrand(productBrandNameComboBox.Text);

                    if (res)
                    {
                        product.ProductBrand = productBrandNameComboBox.Text;
                    }
                    else
                    {
                        DialogResult dr = MessageBox.Show("Are you sure?", "Create new brand", MessageBoxButtons.YesNo);
                        switch (dr)
                        {
                            case DialogResult.Yes:
                                Brand brand = new Brand();
                                brand.BrandName = productBrandNameComboBox.Text;
                                brand.NoOfProducts = 0;
                                brand.BrandStatus = "Unblock";
                                brand.Discount = 0;
                                res = new BrandDataAccess().CreateBrand(brand);
                                if (res)
                                {
                                    product.ProductBrand = productBrandNameComboBox.Text;
                                    MessageBox.Show("Brand created successfully.");
                                }
                                else
                                    MessageBox.Show("Error while deleting.");
                                break;
                            case DialogResult.No:
                                break;
                        }
                    }
                }

                if ((productCategoryNameComboBox.Text == "" || productCategoryNameComboBox.Text == "None") && flag == false)
                {
                    flag = true;
                    MessageBox.Show("Select product category name.");
                }
                else if (!flag)
                {
                    bool res = new CategoryDataAccess().ValidateCategory(productCategoryNameComboBox.Text);

                    if (res)
                    {
                        product.ProductCategory = productCategoryNameComboBox.Text;
                    }
                    else
                    {
                        DialogResult dr = MessageBox.Show("Are you sure?", "Create new Category", MessageBoxButtons.YesNo);
                        switch (dr)
                        {
                            case DialogResult.Yes:
                                Category category = new Category();
                                category.CategoryName = productCategoryNameComboBox.Text;
                                category.NoOfProducts = 0;
                                res = new CategoryDataAccess().CreateCategory(category);
                                if (res)
                                {
                                    product.ProductCategory = productCategoryNameComboBox.Text;
                                    MessageBox.Show("Category created successfully.");
                                }
                                else
                                    MessageBox.Show("Error while deleting.");
                                break;
                            case DialogResult.No:
                                break;
                        }
                    }
                }

                if (!flag)
                {
                    product.ProductStatus = "Unblock";
                    product.Discount = 0;
                    bool res = new ProductDataAccess().CreateProduct(product);

                    if (res)
                    {
                        productListBunifuDataGridView.DataSource = new ProductDataAccess().GetAllProduct();
                        int temp = new BrandDataAccess().GetBrandProductsQuantity(product.ProductBrand);
                        new BrandDataAccess().UpdateBrandTotalProducts(product.ProductBrand, temp);
                        temp = new CategoryDataAccess().GetCategoryProductsQuantity(product.ProductCategory);
                        new CategoryDataAccess().UpdateCategoryTotalProducts(product.ProductCategory,temp);

                        UpdateComboBoxs();
                        ClearCreateFields();
                        MessageBox.Show("Product created successfully.");
                    }
                    else
                        MessageBox.Show("Error while creating.");
                }
            }
        }

        public bool IsDouble(string text)
        {
            Double num = 0;
            bool isDouble = false;

            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            isDouble = Double.TryParse(text, out num);

            return isDouble;
        }

        private void searchProductListBunifuButton_Click(object sender, EventArgs e)
        {
            if (searchProductListBunifuTextBox.Text == "")
            {
                MessageBox.Show("Please! provide product name for search");
                productListBunifuDataGridView.DataSource = new ProductDataAccess().GetAllProduct();
            }
            else
            {
                productListBunifuDataGridView.DataSource = new ProductDataAccess().SearchProduct(searchProductListBunifuTextBox.Text);
            }
        }

        private void refreshProductListBunifuButton_Click(object sender, EventArgs e)
        {
            productListBunifuDataGridView.DataSource = new ProductDataAccess().GetAllProduct();
        }

        private void LoadProductBunifuButton_Click(object sender, EventArgs e)
        {
            if (productIDBunifuTextBox.Text == "")
            {
                MessageBox.Show("Please! Provide a product ID for loading detials.");
            }
            else
            {
                if (productIDBunifuTextBox.Text.All(Char.IsNumber) == true)
                {
                    Product product = new Product();
                    product = new ProductDataAccess().GetProductByID(productIDBunifuTextBox.Text);
                    if (product != null)
                    {
                        updateProductNameBunifuTextBox.Text = product.ProductName;
                        updateProductPriceBunifuTextBox.Text = product.Price.ToString();
                        updateProductQuantityBunifuTextBox.Text = product.Quantity.ToString();
                        updateProductBrandNameComboBox.DataSource = new ProductDataAccess().GetBrandNames();
                        updateProductBrandNameComboBox.SelectedItem = product.ProductBrand;
                        updateProductCategoryNameComboBox.DataSource = new ProductDataAccess().GetCategoryNames();
                        updateProductCategoryNameComboBox.SelectedItem = product.ProductCategory;
                    }
                    else
                    {
                        MessageBox.Show("Product not found.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid product ID.");
                }
            }
        }

        private void updateResetBunifuButton_Click(object sender, EventArgs e)
        {
            productIDBunifuTextBox.Text = "";
            updateProductNameBunifuTextBox.Text = "";
            updateProductPriceBunifuTextBox.Text = "";
            updateProductQuantityBunifuTextBox.Text = "";
            updateProductBrandNameComboBox.DataSource = new ProductDataAccess().GetBrandNames();
            updateProductBrandNameComboBox.SelectedItem = "None";
            updateProductCategoryNameComboBox.DataSource = new ProductDataAccess().GetCategoryNames();
            updateProductCategoryNameComboBox.SelectedItem = "None";
        }

        private void updateBunifuButton_Click(object sender, EventArgs e)
        {
            if (productIDBunifuTextBox.Text == "")
            {
                MessageBox.Show("Please! Provide a product ID for updating detials.");
            }
            else
            {

                if (productIDBunifuTextBox.Text.All(Char.IsNumber) == true)
                {
                    Product product = new Product();
                    product = new ProductDataAccess().GetProductByID(productIDBunifuTextBox.Text);

                    if (product != null)
                    {
                        bool flag = false;

                        if (updateProductPriceBunifuTextBox.Text == "" && flag == false)
                        {
                            flag = true;
                            MessageBox.Show("Provide product price.");
                        }
                        else if (!flag)
                        {
                            if (IsDouble(updateProductPriceBunifuTextBox.Text) == true)
                            {
                                product.Price = Convert.ToDouble(updateProductPriceBunifuTextBox.Text);
                            }
                            else
                            {
                                flag = true;
                                MessageBox.Show("Invalid product price.");
                            }
                        }

                        if (updateProductQuantityBunifuTextBox.Text == "" && flag == false)
                        {
                            flag = true;
                            MessageBox.Show("Provide product quantity.");
                        }
                        else if (!flag)
                        {
                            if (updateProductQuantityBunifuTextBox.Text.All(Char.IsNumber) == true)
                            {
                                product.Quantity = Convert.ToInt32(updateProductQuantityBunifuTextBox.Text);
                            }
                            else
                            {
                                flag = true;
                                MessageBox.Show("Invalid product quantity.");
                            }
                        }

                        if ((updateProductBrandNameComboBox.Text == "" || updateProductBrandNameComboBox.Text == "None") && flag == false)
                        {
                            flag = true;
                            MessageBox.Show("Select product brand name.");
                        }
                        else if (!flag)
                        {
                            bool res = new BrandDataAccess().ValidateBrand(updateProductBrandNameComboBox.Text);

                            if (res)
                            {
                                product.ProductBrand = updateProductBrandNameComboBox.Text;
                            }
                            else
                            {
                                DialogResult dr = MessageBox.Show("Are you sure?", "Create new brand", MessageBoxButtons.YesNo);
                                switch (dr)
                                {
                                    case DialogResult.Yes:
                                        Brand brand = new Brand();
                                        brand.BrandName = updateProductBrandNameComboBox.Text;
                                        brand.NoOfProducts = 0;
                                        brand.BrandStatus = "Unblock";
                                        brand.Discount = 0;
                                        res = new BrandDataAccess().CreateBrand(brand);
                                        if (res)
                                        {
                                            product.ProductBrand = updateProductBrandNameComboBox.Text;
                                            MessageBox.Show("Brand created successfully.");
                                        }
                                        else
                                            MessageBox.Show("Error while creating.");
                                        break;
                                    case DialogResult.No:
                                        updateProductBrandNameComboBox.SelectedItem = "None";
                                        break;
                                }
                            }
                        }

                        if ((updateProductCategoryNameComboBox.Text == "" || updateProductCategoryNameComboBox.Text == "None") && flag == false)
                        {
                            flag = true;
                            MessageBox.Show("Select product category name.");
                        }
                        else if (!flag)
                        {
                            bool res = new CategoryDataAccess().ValidateCategory(updateProductCategoryNameComboBox.Text);

                            if (res)
                            {
                                product.ProductCategory = updateProductCategoryNameComboBox.Text;
                            }
                            else
                            {
                                DialogResult dr = MessageBox.Show("Are you sure?", "Create new Category", MessageBoxButtons.YesNo);
                                switch (dr)
                                {
                                    case DialogResult.Yes:
                                        Category category = new Category();
                                        category.CategoryName = updateProductCategoryNameComboBox.Text;
                                        category.NoOfProducts = 0;
                                        res = new CategoryDataAccess().CreateCategory(category);
                                        if (res)
                                        {
                                            product.ProductCategory = updateProductCategoryNameComboBox.Text;
                                            MessageBox.Show("Category created successfully.");
                                        }
                                        else
                                            MessageBox.Show("Error while creating.");
                                        break;
                                    case DialogResult.No:
                                        updateProductCategoryNameComboBox.SelectedItem = "None";
                                        break;
                                }
                            }
                        }

                        if (!flag)
                        {
                            bool res = new ProductDataAccess().UpdateProduct(product);

                            if (res)
                            {
                                productListBunifuDataGridView.DataSource = new ProductDataAccess().GetAllProduct();
                                int temp = new BrandDataAccess().GetBrandProductsQuantity(product.ProductBrand);
                                new BrandDataAccess().UpdateBrandTotalProducts(product.ProductBrand, temp);
                                temp = new CategoryDataAccess().GetCategoryProductsQuantity(product.ProductCategory);
                                new CategoryDataAccess().UpdateCategoryTotalProducts(product.ProductCategory, temp);

                                UpdateComboBoxs();
                                ClearCreateFields();
                                MessageBox.Show("Product updated successfully.");
                            }
                            else
                                MessageBox.Show("Error while updating.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Product not found.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid product ID. Please provide a valid ID and click load to update.");
                }
            }
        }

        private void deleteResetBunifuButton_Click(object sender, EventArgs e)
        {
            deleteProductNameBunifuTextBox.Text = "";

        }

        private void deleteBunifuButton_Click(object sender, EventArgs e)
        {
            if (deleteProductNameBunifuTextBox.Text == "")
            {
                MessageBox.Show("Please! provide product name for deleting.");
            }
            else
            {
                Product product = new Product();
                product = new ProductDataAccess().GetProductByName(deleteProductNameBunifuTextBox.Text);
                if (product != null)
                {
                    int quantity = product.Quantity;

                    int temp = new BrandDataAccess().GetBrandProductsQuantity(product.ProductBrand);
                    new BrandDataAccess().UpdateBrandTotalProducts(product.ProductBrand, temp - quantity);
                    temp = new CategoryDataAccess().GetCategoryProductsQuantity(product.ProductCategory);
                    new CategoryDataAccess().UpdateCategoryTotalProducts(product.ProductCategory, temp - quantity);

                    DialogResult dr = MessageBox.Show("Are you sure?", "Delete Product", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            bool res = new ProductDataAccess().DeleteProduct(deleteProductNameBunifuTextBox.Text);
                            if (res)
                            {
                                deleteProductNameBunifuTextBox.Text = "";
                                productListBunifuDataGridView.DataSource = new ProductDataAccess().GetAllProduct();
                                if (!new ProductDataAccess().IsEmpty())
                                {
                                    new ProductDataAccess().AlterProducts();
                                }
                                MessageBox.Show("Product deleted successfully.");
                            }
                            else
                                MessageBox.Show("Error while creating.");
                            break;
                        case DialogResult.No:
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Product not found.");
                }


            }
        }

        private void productListBunifuDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                deleteProductNameBunifuTextBox.Text = productListBunifuDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
            }
        }

        [Obsolete]
        private void logoutBunifuButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure to log out?", "Log out", MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes:
                    new Login().Show();
                    this.Hide();
                    break;
                case DialogResult.No:
                    break;

            }
        }
    }
}
