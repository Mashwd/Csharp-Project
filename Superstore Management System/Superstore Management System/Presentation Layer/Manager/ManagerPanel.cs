using Superstore_Management_System.Data_Access_Layer;
using Superstore_Management_System.Entitites;
using Superstore_Management_System.Presntation_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Superstore_Management_System.Presentation_Layer
{
    public partial class ManagerPanel : Form
    {
        private bool mouseDown;
        private Point offset;

        [Obsolete]
        public ManagerPanel()
        {
            InitializeComponent();
        }

        private void ManagerPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void closeBunifuButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void minimizeBunifuButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void topBarbunifuGradientPanel_MouseDown(object sender, MouseEventArgs e)
        {
            offset.X = e.X;
            offset.Y = e.Y;

            mouseDown = true;
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

        [Obsolete]
        private void employeeBunifuButton_Click(object sender, EventArgs e)
        { 
            this.Hide();
            new ManagerPanelCUD().Show();
            
        }

        private void ManagerPanel_Load(object sender, EventArgs e)
        {
            dashBoardBunifuButton.ApplyState(dashBoardBunifuButton.OnPressedState);
            List<string> productQuantitySearch = new List<string>();
            productQuantitySearch.Add("None");
            productQuantitySearch.Add("Brand Name");
            productQuantitySearch.Add("Product ID");
            selectProductQuantityComboBox.DataSource = productQuantitySearch;
            selectProductQuantityComboBox.SelectedItem = "None";
            stockPricelabel.Text = "***Stock Price: " + new ProductDataAccess().SumOfAllProduct().ToString() + ".00 Tk.";
            totalSalesLabel.Text = "***Total Sales: " + new TransitionDataAccess().SumOfSells().ToString() + ".00 Tk.";
            UpdateEmployeeList();
            UpdateEmployeeAccessList();
            UpdateBrands();
            selectDiscountComboBox.SelectedItem = "None";
        }

        private void UpdateEmployeeAccessList()
        {
            suspendComboBox.DataSource = new UserDataAccess().ProceedEmployeeIDList();
            proceedComboBox.DataSource = new UserDataAccess().SuspendEmployeeIDList();

        }

        private void UpdateBrands()
        {
            denyComboBox.DataSource = new BrandDataAccess().GetAllUnblockedBrandName();
            allowComboBox.DataSource = new BrandDataAccess().GetAllBlockedBrandName();
        }

        private void UpdateEmployeeList()
        {
            employeeListBunifuDataGridView.DataSource = new UserDataAccess().GetAllEmployee();
        }

        private void searchEmployeeBunifuButton_Click(object sender, EventArgs e)
        {
            if (employeeSearchComboBox.Text == "")
            {
                MessageBox.Show("Select search Category.");
            }
            if (searchEmployeeBunifuTextBox.Text == "")
            {
                MessageBox.Show("Please! provide value for search.");
                UpdateEmployeeList();
            }

            if (employeeSearchComboBox.Text == "Employee ID")
            {
                if (searchEmployeeBunifuTextBox.Text.All(Char.IsNumber) == true)
                {
                    employeeListBunifuDataGridView.DataSource = "";
                    employeeListBunifuDataGridView.DataSource = new UserDataAccess().SearchEmployee(searchEmployeeBunifuTextBox.Text, true);
                }
                else
                {
                    searchEmployeeBunifuTextBox.Text = "";
                    MessageBox.Show("Error! Invalid employee ID.");
                }
            }
            else if(employeeSearchComboBox.Text == "Position")
            {
                if (searchEmployeeBunifuTextBox.Text.All(Char.IsLetter) == true)
                {
                    employeeListBunifuDataGridView.DataSource = "";
                    employeeListBunifuDataGridView.DataSource = new UserDataAccess().SearchEmployee(searchEmployeeBunifuTextBox.Text, false);
                }
                else
                {
                    searchEmployeeBunifuTextBox.Text = "";
                    MessageBox.Show("Error! Invalid employee position.");
                }
            }
        }

        private void refreshBunifuButton_Click(object sender, EventArgs e)
        {
            UpdateEmployeeList();
            employeeSearchComboBox.SelectedItem = "Employee ID";
            searchEmployeeBunifuTextBox.Text = "";
        }

        private void employeeListBunifuDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                suspendComboBox.Text = "";
                proceedComboBox.Text = "";
                if (employeeListBunifuDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString() != "Manager")
                {
                    string status;
                    status = employeeListBunifuDataGridView.Rows[e.RowIndex].Cells[5].Value.ToString();
                    if (status == "Proceed")
                    {
                        suspendComboBox.Text = employeeListBunifuDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                    }
                    else
                    {
                        proceedComboBox.Text = employeeListBunifuDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                    }
                }
            }
        }

        private void employeeSuspendBunifuButton_Click(object sender, EventArgs e)
        {
            if(suspendComboBox.Text == "1")
            {
                MessageBox.Show("Manager account can't be suspended.");
            }
            else if(suspendComboBox.Text.All(Char.IsNumber) == true)
            {
                bool resp = new UserDataAccess().SuspendEmployee(suspendComboBox.Text);

                if (resp)
                {
                    suspendComboBox.Text = "";
                    UpdateEmployeeList();
                    UpdateEmployeeAccessList();
                    MessageBox.Show("User suspended.");
                }
                else
                {
                    suspendComboBox.Text = "";
                    MessageBox.Show("User not found.");
                }
            }
            else
            {
                suspendComboBox.Text = "";
                MessageBox.Show("Invalid user ID.");
            }
        }

        private void employeeProceedBunifuButton3_Click(object sender, EventArgs e)
        {
            if (proceedComboBox.Text.All(Char.IsNumber) == true)
            {
                bool resp = new UserDataAccess().ProceedEmployee(proceedComboBox.Text);

                if (resp)
                {
                    proceedComboBox.Text = "";
                    UpdateEmployeeList();
                    UpdateEmployeeAccessList();
                    MessageBox.Show("User proceeded.");

                }
                else
                {
                    proceedComboBox.Text = "";
                    MessageBox.Show("User not found.");
                }
            }
            else
            {
                proceedComboBox.Text = "";
                MessageBox.Show("Invalid user ID.");
            }
        }

        private void selectProductQuantityComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(selectProductQuantityComboBox.Text != "" && selectProductQuantityComboBox.Text != "None")
            {
                if (selectProductQuantityComboBox.Text == "Brand Name")
                {
                    productQuantityListBunifuDataGridView.DataSource = new BrandDataAccess().GetAllBrandQuantity();
                    suggestionsProductQuantityComboBox.DataSource = new ProductDataAccess().GetBrandNames();
                    suggestionsProductQuantityComboBox.SelectedItem = "None";
                }
                else if(selectProductQuantityComboBox.Text == "Product ID")
                {
                    productQuantityListBunifuDataGridView.DataSource = new ProductDataAccess().GetAllProductQuantity();
                    suggestionsProductQuantityComboBox.DataSource = new ProductDataAccess().GetProductID();
                    suggestionsProductQuantityComboBox.SelectedItem = 0;
                }
            }
        }

        private void suggestionsProductQuantityComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(suggestionsProductQuantityComboBox.Text == "None")
            {
                productQuantityListBunifuDataGridView.DataSource = new BrandDataAccess().GetAllBrandQuantity();
            }
            else if(suggestionsProductQuantityComboBox.Text == "0")
            {
                productQuantityListBunifuDataGridView.DataSource = new ProductDataAccess().GetAllProductQuantity();
            }
            else if(suggestionsProductQuantityComboBox.Text != "")
            {
                if(selectProductQuantityComboBox.SelectedItem.ToString() == "Brand Name")
                {
                    productQuantityListBunifuDataGridView.DataSource = new BrandDataAccess().GetSpecificBrandQuantity(suggestionsProductQuantityComboBox.Text);
                }
                else if(selectProductQuantityComboBox.Text == "Product ID")
                {
                    productQuantityListBunifuDataGridView.DataSource = new ProductDataAccess().GetSpecificProductQuantity(suggestionsProductQuantityComboBox.Text);
                }
            }
        }

        private void searchEmployeeBunifuTextBox_TextChanged(object sender, EventArgs e)
        {
            if (employeeSearchComboBox.Text == "Employee ID")
            {
                if (searchEmployeeBunifuTextBox.Text.All(Char.IsNumber) == true)
                {
                    employeeListBunifuDataGridView.DataSource = "";
                    employeeListBunifuDataGridView.DataSource = new UserDataAccess().SearchEmployee(searchEmployeeBunifuTextBox.Text, true);
                }
                else
                {
                    searchEmployeeBunifuTextBox.Text = "";
                    MessageBox.Show("Error! Invalid employee ID.");
                }
            }
            else if(employeeSearchComboBox.Text == "Position")
            {
                if (searchEmployeeBunifuTextBox.Text.All(Char.IsLetter) == true)
                {
                    employeeListBunifuDataGridView.DataSource = "";
                    employeeListBunifuDataGridView.DataSource = new UserDataAccess().SearchEmployee(searchEmployeeBunifuTextBox.Text, false);
                }
                else
                {
                    searchEmployeeBunifuTextBox.Text = "";
                    MessageBox.Show("Error! Invalid employee position.");
                }
            }
            else if(searchEmployeeBunifuTextBox.Text == "")
            {
                new UserDataAccess().GetAllEmployee();
            }
        }

        private void employeeSearchComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            searchEmployeeBunifuTextBox.Text = "";
            new UserDataAccess().GetAllEmployee();
        }

        private void brandProductBlockBunifuButton_Click(object sender, EventArgs e)
        {
            if (denyComboBox.Text != "")
            {
                bool resp = new BrandDataAccess().BlockBrand(denyComboBox.SelectedItem.ToString());

                if (resp)
                {
                    new ProductDataAccess().UpdateAllProductAccessToBlock(denyComboBox.Text);
                    denyComboBox.Text = "";
                    UpdateBrands();
                    MessageBox.Show("Brand Blocked.");
                }
                else
                {
                    denyComboBox.Text = "";
                    MessageBox.Show("Brand not found.");
                    UpdateBrands();

                }
            }
            else
            {
                suspendComboBox.Text = "";
                MessageBox.Show("Invalid brand name.");
                UpdateBrands();
            }
        }

        private void brandProductAllowBunifuButton_Click(object sender, EventArgs e)
        {
            if (allowComboBox.Text != "")
            {
                bool resp = new BrandDataAccess().UnblockBrand(allowComboBox.Text);

                if (resp)
                {
                    new ProductDataAccess().UpdateAllProductAccessToUnblock(allowComboBox.Text);
                    allowComboBox.Text = "";
                    UpdateBrands();
                    MessageBox.Show("Brand unblocked.");
                }
                else
                {
                    allowComboBox.Text = "";
                    MessageBox.Show("Brand not found.");
                    UpdateBrands();

                }
            }
            else
            {
                allowComboBox.Text = "";
                MessageBox.Show("Invalid brand name.");
                UpdateBrands();
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

        private void selectDiscountComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(selectDiscountComboBox.Text != "None" || selectDiscountComboBox.Text != "")
            {
                if(selectDiscountComboBox.Text == "Brand")
                {
                    suggestionsDiscountComboBox.DataSource = new ProductDataAccess().GetBrandNames();
                    suggestionsDiscountComboBox.SelectedItem = "None";
                }
                else if(selectDiscountComboBox.Text == "Product")
                {
                    suggestionsDiscountComboBox.DataSource = new ProductDataAccess().GetProductID();
                    suggestionsDiscountComboBox.SelectedItem = 0;
                }
            }
        }

        private void discountBunifuButton_Click(object sender, EventArgs e)
        {
            if(discountBunifuTextBox.Text == "")
            {
                MessageBox.Show("Discount amount can't be empty.");
            }
            else 
            {
                bool res = false;
                if (selectDiscountComboBox.Text == "Brand")
                {
                    Brand brand = new Brand();
                    brand.Discount = Convert.ToInt32(discountBunifuTextBox.Text);
                    brand.BrandName = suggestionsDiscountComboBox.SelectedItem.ToString();
                    res = new BrandDataAccess().UpdateBrandDiscount(brand);
                    new ProductDataAccess().UpdateSpecificBrandProductDiscount(brand.BrandName, brand.Discount);
                }
                else if (selectDiscountComboBox.Text == "Product")
                {
                    res = new ProductDataAccess().UpdateProductDiscount(Convert.ToInt32(discountBunifuTextBox.Text), suggestionsDiscountComboBox.SelectedItem.ToString());
                }
                

                if(res)
                {
                    MessageBox.Show("Discoount added.");
                }
            }
        }
    }
}
