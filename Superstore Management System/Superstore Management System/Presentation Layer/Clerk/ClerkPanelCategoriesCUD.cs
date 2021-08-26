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
    public partial class ClerkPanelCategoriesCUD : Form
    {
        private bool mouseDown;
        private Point offset;

        Thread thread;

        [Obsolete]
        public ClerkPanelCategoriesCUD()
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

        private void ClerkPanelCategoriesCUD_Load(object sender, EventArgs e)
        {
            categoryBunifuButton.ApplyState(categoryBunifuButton.OnPressedState);
            CategoryListBunifuDataGridView.DataSource = new CategoryDataAccess().GetAllCategory();
            categoryWiseComboBox.DataSource = new ProductDataAccess().GetCategoryNames();
            categoryWiseComboBox.SelectedItem = "None";
        }

        [Obsolete]
        private void brandBunifuButton_Click(object sender, EventArgs e)
        {
            categoryBunifuButton.ApplyState(categoryBunifuButton.OnIdleState);
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

        [Obsolete]
        private void productBunifuButton_Click(object sender, EventArgs e)
        {
            categoryBunifuButton.ApplyState(categoryBunifuButton.OnIdleState);
            this.Close();
            thread = new Thread(openProduct);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        [Obsolete]
        private void openProduct()
        {
            Application.Run(new ClerkPanelProductCUD());
        }

        private void createCategoryResetBunifuButton_Click(object sender, EventArgs e)
        {
            categoryNameBunifuTextBox.Text = "";
        }

        private void createCategoryBunifuButton_Click(object sender, EventArgs e)
        {
            if(categoryNameBunifuTextBox.Text == "" && surityBunifuCheckBox.Checked == false)
            {
                MessageBox.Show("Required fields are empty.");
            }
            else if(categoryNameBunifuTextBox.Text == "")
            {
                MessageBox.Show("Category name can't be empty.");
            }
            else if(surityBunifuCheckBox.Checked == false)
            {
                MessageBox.Show("Check the checkbox.");
            }
            else
            {
                bool res = new CategoryDataAccess().ValidateCategory(categoryNameBunifuTextBox.Text);
                if(!res)
                {
                    Category category = new Category();
                    category.CategoryName = categoryNameBunifuTextBox.Text;
                    category.NoOfProducts = 0;
                    res = new CategoryDataAccess().CreateCategory(category);
                    if(res)
                    {
                        surityBunifuCheckBox.Checked = false;
                        CategoryListBunifuDataGridView.DataSource = new CategoryDataAccess().GetAllCategory();
                        MessageBox.Show("Category created successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Error while saving.");
                    }
                }
                else
                {
                    MessageBox.Show("Category already exists.");
                }
            }
        }

        private void searchCategorybunifuButton_Click(object sender, EventArgs e)
        {
            if(SearchCategoryBunifuTextBox.Text == "")
            {
                CategoryListBunifuDataGridView.DataSource = new CategoryDataAccess().GetAllCategory();
                MessageBox.Show("Provide category name to serach.");
            }
            else
            {
                CategoryListBunifuDataGridView.DataSource = new CategoryDataAccess().SearchCategory(SearchCategoryBunifuTextBox.Text);
            }
        }

        private void refreshEmployeeListbunifuButton_Click(object sender, EventArgs e)
        {
            SearchCategoryBunifuTextBox.Text = "";
            CategoryListBunifuDataGridView.DataSource = new CategoryDataAccess().GetAllCategory();
        }

        private void LoadCategoryBunifuButton_Click(object sender, EventArgs e)
        {
            if (categoryIDBunifuTextBox.Text == "")
            {
                MessageBox.Show("Provide category ID to load details.");
            }
            else
            {
                if (categoryIDBunifuTextBox.Text.All(Char.IsNumber) == true)
                {
                    bool res = new CategoryDataAccess().ValidateCategoryID(categoryIDBunifuTextBox.Text);

                    if(res)
                    {
                        Category category = new Category();
                        category = new CategoryDataAccess().GetCategoryByID(categoryIDBunifuTextBox.Text);
                        updateCategoryNameBunifuTextBox.Text = category.CategoryName;
                    }
                    else
                    {
                        MessageBox.Show("Category not found.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid category ID.");
                }
            }
        }

        private void updateResetBunifuButton_Click(object sender, EventArgs e)
        {
            updateCategoryNameBunifuTextBox.Text = "";
            categoryIDBunifuTextBox.Text = "";
        }

        private void updateBunifuButton_Click(object sender, EventArgs e)
        {
            if (categoryIDBunifuTextBox.Text == "")
            {
                MessageBox.Show("Provide category ID to update details.");
            }
            else
            {
                if (categoryIDBunifuTextBox.Text.All(Char.IsNumber) == true)
                {
                    if (updateCategoryNameBunifuTextBox.Text == "")
                    {
                        MessageBox.Show("Provide category name.");
                    }
                    else
                    {
                        Category category = new Category();

                        category.CategoryID = Convert.ToInt32(categoryIDBunifuTextBox.Text);
                        category.CategoryName = updateCategoryNameBunifuTextBox.Text;

                        bool res = new CategoryDataAccess().UpdateCategory(category);

                        if (res)
                        {
                            CategoryListBunifuDataGridView.DataSource = new CategoryDataAccess().GetAllCategory();
                            MessageBox.Show("Category updated successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Error while saving.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid category ID.");
                }
            }
        }

        private void deleteResetBunifuButton_Click(object sender, EventArgs e)
        {
            deleteCategoryNameBunifuTextBox.Text = "";
        }

        private void deleteBunifuButton_Click(object sender, EventArgs e)
        {
            if(deleteCategoryNameBunifuTextBox.Text == "")
            {
                MessageBox.Show("Please! provide category name to delete.");
            }
            else
            {
                if (new CategoryDataAccess().ValidateCategoryName(deleteCategoryNameBunifuTextBox.Text) == true)
                {
                    DialogResult dr = MessageBox.Show("It will delete all the products under this category. Are you sure?", "Delete Category", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            bool res = new CategoryDataAccess().DeleteCategory(deleteCategoryNameBunifuTextBox.Text);
                            if (res)
                            {
                                CategoryListBunifuDataGridView.DataSource = new CategoryDataAccess().GetAllCategory();
                                new ProductDataAccess().DeleteProductByCategoryName(deleteCategoryNameBunifuTextBox.Text);
                                deleteCategoryNameBunifuTextBox.Text = "";
                                if (!new CategoryDataAccess().IsEmpty())
                                {
                                    new CategoryDataAccess().AlterCategories();
                                }
                                MessageBox.Show("Category deleted successfully.");
                            }
                            else
                                MessageBox.Show("Error while deleting.");
                            break;
                        case DialogResult.No:
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Category not found.");
                }
            }
        }

        private void CategoryListBunifuDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                deleteCategoryNameBunifuTextBox.Text = CategoryListBunifuDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
            }
        }

        private void SearchCategoryBunifuTextBox_TextChanged(object sender, EventArgs e)
        {
            if (SearchCategoryBunifuTextBox.Text == "")
            {
                CategoryListBunifuDataGridView.DataSource = new CategoryDataAccess().GetAllCategory();
            }
            else
            {
                CategoryListBunifuDataGridView.DataSource = new CategoryDataAccess().SearchCategory(SearchCategoryBunifuTextBox.Text);
            }
        }

        private void categoryWiseComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (categoryWiseComboBox.Text == "None" && BrandBunifuRadioButton.Checked == true)
            {
                categoryWiseBunifuDataGridView.DataSource = new BrandDataAccess().GetCategoryWiseBrandList();
            }
            else if(categoryWiseComboBox.Text == "None" && productBunifuRadioButton.Checked == true)
            {
                categoryWiseBunifuDataGridView.DataSource = new ProductDataAccess().GetCategoryWiseProductList();
            }
            else if (categoryWiseComboBox.Text != "")
            {
                if (BrandBunifuRadioButton.Checked == true)
                {
                    categoryWiseBunifuDataGridView.DataSource = new BrandDataAccess().GetCategoryWiseBrandListByCategoryName(categoryWiseComboBox.Text);
                }
                else if (productBunifuRadioButton.Checked == true)
                {
                    categoryWiseBunifuDataGridView.DataSource = new ProductDataAccess().GetCategoryWiseProductListByCategoryName(categoryWiseComboBox.Text);
                }
            }
        }

        private void BrandBunifuRadioButton_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {
            categoryWiseBunifuDataGridView.DataSource = new ProductDataAccess().GetCategoryWiseProductList();
            categoryWiseComboBox.SelectedItem = "None";
        }

        private void productBunifuRadioButton_CheckedChanged2(object sender, Bunifu.UI.WinForms.BunifuRadioButton.CheckedChangedEventArgs e)
        {
            categoryWiseBunifuDataGridView.DataSource = new BrandDataAccess().GetCategoryWiseBrandList();
            categoryWiseComboBox.SelectedItem = "None";
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
