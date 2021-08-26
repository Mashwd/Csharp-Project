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
using Superstore_Management_System.Entitites;
using Superstore_Management_System.Data_Access_Layer;
using Superstore_Management_System.Presntation_Layer;

namespace Superstore_Management_System.Presentation_Layer.Clerk
{
    public partial class ClerkPanelBrandCUD : Form
    {
        private bool mouseDown;
        private Point offset;
        Thread thread;

        [Obsolete]
        public ClerkPanelBrandCUD()
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

        private void ClerkPanelBrandCUD_Load(object sender, EventArgs e)
        {
            brandBunifuButton.ApplyState(brandBunifuButton.OnPressedState);
            brandListBunifuDataGridView.DataSource = new BrandDataAccess().GetAllBrand();
            brandWiseBunifuDataGridView.DataSource = new ProductDataAccess().GetAllProduct();
        }

        [Obsolete]
        private void productBunifuButton_Click(object sender, EventArgs e)
        {
            brandBunifuButton.ApplyState(brandBunifuButton.OnIdleState);
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

        [Obsolete]
        private void categoryBunifuButton_Click(object sender, EventArgs e)
        {
            brandBunifuButton.ApplyState(brandBunifuButton.OnIdleState);
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

        private void createBrandResetBunifuButton_Click(object sender, EventArgs e)
        {
            createBrandBunifuTextBox.Text = "";
            brandDiscountBunifuTextBox.Text = "";
            surityBunifuCheckBox.Checked = false;
        }

        private void createBrandBunifuButton_Click(object sender, EventArgs e)
        {
            if(createBrandBunifuTextBox.Text == "" && brandDiscountBunifuTextBox.Text == "" &&
                surityBunifuCheckBox.Checked == false)
            {
                MessageBox.Show("Required fields are empty.");
            }
            else
            {
                bool flag = false;
                Brand brand = new Brand();
                if(createBrandBunifuTextBox.Text == "")
                {
                    flag = true;
                    MessageBox.Show("Name can't be empty.");
                }
                else
                {
                    if(new BrandDataAccess().ValidateBrand(createBrandBunifuTextBox.Text) == false)
                        brand.BrandName = createBrandBunifuTextBox.Text;
                    else if(!flag)
                    {
                        flag = true;
                        MessageBox.Show("Brand already exists.");
                    }
                }

                if(brandDiscountBunifuTextBox.Text == "" && flag == false)
                {
                    flag = true;
                    MessageBox.Show("Discount amount can't be empty.");
                }
                else if(!flag)
                {
                    if (IsDouble(brandDiscountBunifuTextBox.Text) == true)
                    {
                        brand.Discount = Convert.ToDouble(brandDiscountBunifuTextBox.Text);
                    }
                    else
                    {
                        flag = true;
                        MessageBox.Show("Invalid discount amount.");
                    }
                }

                if(surityBunifuCheckBox.Checked == false && flag == false)
                {
                    flag = true;
                    MessageBox.Show("Checked the checkbox.");
                }

                if(!flag)
                {
                    brand.NoOfProducts = 0;
                    brand.BrandStatus = "Unblock";
                    bool res = new BrandDataAccess().CreateBrand(brand);
                    if(res)
                    {
                        surityBunifuCheckBox.Checked = false;
                        createBrandBunifuTextBox.Text = "";
                        brandDiscountBunifuTextBox.Text = "";
                        brandListBunifuDataGridView.DataSource = new BrandDataAccess().GetAllBrand();
                        MessageBox.Show("Brand created successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Error while creating.");
                    }
                }
            }
        }

        private void updateResetBunifuButton_Click(object sender, EventArgs e)
        {
            brandIDBunifuTextBox.Text = "";
            updateBrandDiscountBunifuTextBox.Text = "";
            updateBrandNameBunifuTextBox.Text = "";
        }

        private void LoadBrandBunifuButton_Click(object sender, EventArgs e)
        {
            if (brandIDBunifuTextBox.Text == "")
            {
                MessageBox.Show("Provide brand ID to load details.");
            }
            else
            {
                if (brandIDBunifuTextBox.Text.All(Char.IsNumber) == true)
                {
                    bool res = new BrandDataAccess().ValidateBrandID(brandIDBunifuTextBox.Text);

                    if (res)
                    {
                        Brand brand = new Brand();
                        brand = new BrandDataAccess().GetBrandByID(brandIDBunifuTextBox.Text);
                        updateBrandNameBunifuTextBox.Text = brand.BrandName;
                        updateBrandDiscountBunifuTextBox.Text = brand.Discount.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Brand not found.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid brand ID.");
                }
            }
        }

        private void updateBunifuButton_Click(object sender, EventArgs e)
        {
            if (brandIDBunifuTextBox.Text == "")
            {
                MessageBox.Show("Provide brand ID to update details.");
            }
            else
            {
                if (brandIDBunifuTextBox.Text.All(Char.IsNumber) == true)
                {
                    if (updateBrandDiscountBunifuTextBox.Text == "")
                    {
                        MessageBox.Show("Provide brand discount amount.");
                    }
                    else
                    {
                        Brand brand = new Brand();
                        if (IsDouble(updateBrandDiscountBunifuTextBox.Text) == true)
                        {
                            brand.Discount = Convert.ToDouble(updateBrandDiscountBunifuTextBox.Text);
                            brand.BrandID = Convert.ToInt32(brandIDBunifuTextBox.Text);

                            bool res = new BrandDataAccess().UpdateBrand(brand);

                            if (res)
                            {
                                brandIDBunifuTextBox.Text = "";
                                updateBrandDiscountBunifuTextBox.Text = "";
                                updateBrandNameBunifuTextBox.Text = "";
                                brandListBunifuDataGridView.DataSource = new BrandDataAccess().GetAllBrand();
                                MessageBox.Show("Brand updated successfully.");
                            }
                            else
                            {
                                MessageBox.Show("Error while saving.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid discount amount.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid brand ID.");
                }
            }
        }

        private void deleteResetBunifuButton_Click(object sender, EventArgs e)
        {
            deleteBrandNameBunifuTextBox.Text = "";
        }

        private void deleteBunifuButton_Click(object sender, EventArgs e)
        {
            if (deleteBrandNameBunifuTextBox.Text == "")
            {
                MessageBox.Show("Please! provide brand name to delete.");
            }
            else
            {
                if (new BrandDataAccess().ValidateBrandName(deleteBrandNameBunifuTextBox.Text) == true)
                {
                    DialogResult dr = MessageBox.Show("It will delete All the products under this brand. Are you sure?", "Delete Brand", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            bool res = new BrandDataAccess().DeleteBrand(deleteBrandNameBunifuTextBox.Text);
                            if (res)
                            {
                                brandListBunifuDataGridView.DataSource = new BrandDataAccess().GetAllBrand();
                                new ProductDataAccess().DeleteProductByBrandName(deleteBrandNameBunifuTextBox.Text);
                                deleteBrandNameBunifuTextBox.Text = "";
                                if(!new BrandDataAccess().IsEmpty())
                                {
                                    new BrandDataAccess().AlterBrands();
                                }
                                MessageBox.Show("Brand deleted successfully.");
                                brandWiseBunifuDataGridView.DataSource = new ProductDataAccess().GetAllProduct();
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
                    MessageBox.Show("Brand not found.");
                }
            }
        }

        private void searchBrandListBunifuButton_Click(object sender, EventArgs e)
        {
            if (searchBrandBunifuTextBox.Text == "")
            {
                brandListBunifuDataGridView.DataSource = new BrandDataAccess().GetAllBrand();
                MessageBox.Show("Provide brand name to search.");
            }
            else
            {
                brandListBunifuDataGridView.DataSource = new BrandDataAccess().SearchBrand(searchBrandBunifuTextBox.Text);
            }
        }

        private void refreshBrandListBunifuButton_Click(object sender, EventArgs e)
        {
            searchBrandBunifuTextBox.Text = "";
            brandListBunifuDataGridView.DataSource = new BrandDataAccess().GetAllBrand();
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

        private void brandListBunifuDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                deleteBrandNameBunifuTextBox.Text = brandListBunifuDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
            }
        }

        private void refreshBrandWiseProductListBunifuButton_Click(object sender, EventArgs e)
        {
            brandWiseBunifuDataGridView.DataSource = new ProductDataAccess().GetAllProduct();
        }

        private void searchBrandWiseProductListBunifuButton_Click(object sender, EventArgs e)
        {
            if(searchBrandWisebunifuTextBox.Text == "")
            {
                brandWiseBunifuDataGridView.DataSource = new ProductDataAccess().GetAllProduct();
            }
            else
            {
                brandWiseBunifuDataGridView.DataSource = new ProductDataAccess().GetProductByBrandName(searchBrandWisebunifuTextBox.Text);
            }
        }

        private void searchBrandWisebunifuTextBox_TextChange(object sender, EventArgs e)
        {
            if (searchBrandWisebunifuTextBox.Text == "")
            {
                brandWiseBunifuDataGridView.DataSource = new ProductDataAccess().GetAllProduct();
            }
            else
            {
                brandWiseBunifuDataGridView.DataSource = new ProductDataAccess().GetProductByBrandName(searchBrandWisebunifuTextBox.Text);
            }
        }

        private void searchBrandBunifuTextBox_TextChanged(object sender, EventArgs e)
        {
            if (searchBrandBunifuTextBox.Text == "")
            {
                brandListBunifuDataGridView.DataSource = new BrandDataAccess().GetAllBrand();
            }
            else
            {
                brandListBunifuDataGridView.DataSource = new BrandDataAccess().SearchBrand(searchBrandBunifuTextBox.Text);
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
