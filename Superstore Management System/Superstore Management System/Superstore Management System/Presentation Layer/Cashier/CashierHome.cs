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

namespace Superstore_Management_System.Presentation_Layer.Cashier
{
    public partial class CashierHome : Form
    {
        private bool mouseDown;
        private Point offset;

        [Obsolete]
        public CashierHome()
        {
            InitializeComponent();
        }

        private void CashierHome_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void closeBunifuButton_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void minimizeBunifuButton_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void topBarbunifuGradientPanel_MouseUp_1(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void topBarbunifuGradientPanel_MouseDown_1(object sender, MouseEventArgs e)
        {
            offset.X = e.X;
            offset.Y = e.Y;

            mouseDown = true;
        }

        private void topBarbunifuGradientPanel_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (mouseDown == true)
            {
                Point currentScreenPosition = PointToScreen(e.Location);
                Location = new Point(currentScreenPosition.X - offset.X, currentScreenPosition.Y - offset.Y);
            }
        }

        private void CashierHome_Load(object sender, EventArgs e)
        {
            homeBunifuButton.ApplyState(homeBunifuButton.OnPressedState);
            productListBunifuDataGridView.DataSource = new ProductDataAccess().GetAllProduct();
            transitionHistoryBunifuDataGridView.DataSource = new TransitionDataAccess().GetAllTransitions();
            priceLabel.Text = "Total Price: 00.00 Tk.";
            changeLabel.Text = "Change: 00.00 Tk.";
        }

        private void resetBunifuButton3_Click(object sender, EventArgs e)
        {
            ClearMakePayment();
        }

        public void ClearMakePayment()
        {
            categoryNameTextBox.Text = "";
            productNameTextBox.Text = "";
            brandNameTextBox.Text = "";
            productPriceTextBox.Text = "";
            productQuantityTextBox.Text = "";
        }

        private void productListBunifuDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                categoryNameTextBox.Text = productListBunifuDataGridView.Rows[e.RowIndex].Cells[5].Value.ToString();
                brandNameTextBox.Text = productListBunifuDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString();
                productNameTextBox.Text = productListBunifuDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                productPriceTextBox.Text = productListBunifuDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
            }
        }

        private void productQuantityTextBox_TextChanged(object sender, EventArgs e)
        {
            if (productQuantityTextBox.Text != "")
            {
                if (productQuantityTextBox.Text.All(Char.IsNumber) == false)
                {
                    MessageBox.Show("Provide integer for Quantity.");
                }
                else
                {
                    if (productNameTextBox.Text != "")
                    {
                        int temp = new ProductDataAccess().ValidateQuantity(productNameTextBox.Text);
                        if (temp < Convert.ToInt32(productQuantityTextBox.Text))
                        {
                            MessageBox.Show("Only " + temp + "items left.");
                        }
                    }
                }
            }
        }

        private void addItemBunifuButton_Click(object sender, EventArgs e)
        {
            
            if (categoryNameTextBox.Text == "" || productNameTextBox.Text == "" ||
                brandNameTextBox.Text == "" || productPriceTextBox.Text == "")
            {
                MessageBox.Show("Click on the product that whats to add in the chart.");
            }
            else if (productQuantityTextBox.Text == "")
            {
                MessageBox.Show("Quantity required.");
            }
            else
            {
                Chart chart = new Chart();

                chart.Product = productNameTextBox.Text;
                chart.Quantity = Convert.ToInt32(productQuantityTextBox.Text);
                chart.Price = Convert.ToDouble(productPriceTextBox.Text);

                string res = new ProductDataAccess().ValidateProductAccess(productNameTextBox.Text);

                if (res == "Block")
                {
                    MessageBox.Show("Product is currently on a blocked state.");
                }
                else if (res == "Unblock")
                {
                    chart.Price = chart.Quantity * chart.Price;
                    new ChartDataAccess().InsertProductInChart(chart);

                    ClearMakePayment();

                    int quantity = new ProductDataAccess().ValidateQuantity(chart.Product);
                    quantity = quantity - chart.Quantity;
                    chartBunifuDataGridView.DataSource = new ChartDataAccess().GetAllProductFromChart();
                    new ProductDataAccess().UpdateProductQuantity(quantity, chart.Product);
                    productListBunifuDataGridView.DataSource = new ProductDataAccess().GetAllProduct();
                    if (new ChartDataAccess().IsEmpty() == true)
                        priceLabel.Text = "Total Price: " + new ChartDataAccess().SumOfChart() + ".00 Tk.";
                    else
                        priceLabel.Text = "Total Price: 00.00 TK.";
                }
            }
        }

        private void chartBunifuDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            removeTextBox.Text = chartBunifuDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
        }

        private void removeProductFromChartBunifuButton_Click(object sender, EventArgs e)
        {
            if (removeTextBox.Text == "")
            {
                MessageBox.Show("Select product from the chart or provide product ID from the chart.");
            }
            else
            {
                DialogResult dr = MessageBox.Show("It will delete this product from the chart. Are you sure?", "Remove Product", MessageBoxButtons.YesNo);
                switch (dr)
                {
                    case DialogResult.Yes:

                        Chart chart = new Chart();

                        chart = new ChartDataAccess().GetProductFromChartByID(removeTextBox.Text);
                        int quantity = new ProductDataAccess().ValidateQuantity(chart.Product);
                        quantity = quantity + chart.Quantity;

                        new ProductDataAccess().UpdateProductQuantity(quantity, chart.Product);
                        if(!new ChartDataAccess().RemoveProductFromChart(chart.ChartID.ToString()))
                        {
                            MessageBox.Show("Product not found in the chart.");
                        }
                            

                        productListBunifuDataGridView.DataSource = new ProductDataAccess().GetAllProduct();
                        chartBunifuDataGridView.DataSource = new ChartDataAccess().GetAllProductFromChart();

                        removeTextBox.Text = "";
                        if (new ChartDataAccess().IsEmpty() == true)
                            priceLabel.Text = "Total Price: " + new ChartDataAccess().SumOfChart() + ".00 Tk.";
                        else
                            priceLabel.Text = "Total Price: 00.00 TK.";

                        if (new ChartDataAccess().IsEmpty() == false)
                        {
                            new ChartDataAccess().AlterChart();
                        }

                        break;
                    case DialogResult.No:
                        break;
                }
            }
        }

        private void PaidButton_Click(object sender, EventArgs e)
        {
            if(givenAmountTextBox.Text == "")
            {
                MessageBox.Show("Provide given amount for making payment.");
            }
            else if (new ChartDataAccess().IsEmpty() == true)
            {
                List<Chart> charts = new List<Chart>();

                charts = new ChartDataAccess().GetAllProductFromChart();

                Transition transition = new Transition();

                for (int i = 0; i < charts.Count; i++)
                {
                    if (i != 0)
                        transition.Details = transition.Details + charts[i].Product + " " + charts[i].Quantity + "x, ";
                    else if(i == charts.Count-1)
                        transition.Details = charts[i].Product + " " + charts[i].Quantity + "x";
                    else
                        transition.Details = charts[i].Product + " " + charts[i].Quantity + "x, ";
                }

                transition.TotalPrice = new ChartDataAccess().SumOfChart();
                transition.Time = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");

                bool res = new TransitionDataAccess().InsertTransition(transition);

                if(res)
                {
                    transitionHistoryBunifuDataGridView.DataSource = new TransitionDataAccess().GetAllTransitions();
                    priceLabel.Text = "Total Price: 00.00 Tk.";
                    changeLabel.Text = "Change: 00.00 Tk.";
                    givenAmountTextBox.Text = "";
                    MessageBox.Show("Payment Successfully done");

                    new ChartDataAccess().DeleteAll();
                    chartBunifuDataGridView.DataSource = new ChartDataAccess().GetAllProductFromChart();

                    if (new ChartDataAccess().IsEmpty() == false)
                    {
                        new ChartDataAccess().AlterChart();
                    }
                }
                else
                {
                    MessageBox.Show("Error while making payment.");
                }
            }
            else
            {
                MessageBox.Show("Empty chart.");
            }
        }

        private void givenAmountTextBox_TextChanged(object sender, EventArgs e)
        {
            if(new ChartDataAccess().IsEmpty() == false)
            {
                MessageBox.Show("Empty chart. Add somthing for making payment.");
            }
            else if(givenAmountTextBox.Text != "")
            {
                if(givenAmountTextBox.Text.All(Char.IsNumber) == true)
                {
                    double temp = new ChartDataAccess().SumOfChart();
                    
                    if(new ChartDataAccess().SumOfChart() != 0)
                        changeLabel.Text = "Change: " + (Convert.ToDouble(givenAmountTextBox.Text) - temp) + ".00 Tk.";
                    
                }
                else
                {
                    MessageBox.Show("Please provide amount(Number).");
                }
            }

        }

        private void cancelBunifuButton_Click(object sender, EventArgs e)
        {
            if (new ChartDataAccess().IsEmpty() == true)
            {
                DialogResult dr = MessageBox.Show("Are you sure to cancel?", "Cancel payment", MessageBoxButtons.YesNo);
                switch (dr)
                {
                    case DialogResult.Yes:
                        List<Chart> charts = new List<Chart>();

                        charts = new ChartDataAccess().GetAllProductFromChart();

                        for (int i = 0; i < charts.Count; i++)
                        {
                            int quantity = new ProductDataAccess().ValidateQuantity(charts[0].Product);
                            quantity = quantity + charts[0].Quantity;
                            new ProductDataAccess().UpdateProductQuantity(quantity, charts[i].Product);
                        }

                        new ChartDataAccess().DeleteAll();
                        chartBunifuDataGridView.DataSource = new ChartDataAccess().GetAllProductFromChart();
                        productListBunifuDataGridView.DataSource = new ProductDataAccess().GetAllProduct();

                        priceLabel.Text = "Total Price: 00.00 Tk.";
                        changeLabel.Text = "Change: 00.00 Tk.";

                        ClearMakePayment();
                        break;
                    case DialogResult.No:
                        break;

                }
                
            }
        }

        private void searchProductListBunifuButton_Click(object sender, EventArgs e)
        {
            if(searchProductListBunifuTextBox.Text == "")
            {
                MessageBox.Show("Provide product name.");
                productListBunifuDataGridView.DataSource = new ProductDataAccess().GetAllProduct();
            }
            else
            {
                productListBunifuDataGridView.DataSource = new ProductDataAccess().SearchProduct(searchProductListBunifuTextBox.Text);
            }
        }

        private void searchProductListBunifuTextBox_TextChange(object sender, EventArgs e)
        {
            if(searchProductListBunifuTextBox.Text != "")
            {
                productListBunifuDataGridView.DataSource = new ProductDataAccess().SearchProduct(searchProductListBunifuTextBox.Text);
            }
            else if(searchProductListBunifuTextBox.Text == "")
            {
                productListBunifuDataGridView.DataSource = new ProductDataAccess().GetAllProduct();
            }
            
        }

        private void refreshProductListBunifuButton_Click(object sender, EventArgs e)
        {
            productListBunifuDataGridView.DataSource = new ProductDataAccess().GetAllProduct();
            searchProductListBunifuTextBox.Text = "";
        }

        private void transitionSearchBunifuTextBox_TextChange(object sender, EventArgs e)
        {
            if(transitionSearchBunifuTextBox.Text == "")
            {
                transitionHistoryBunifuDataGridView.DataSource = new TransitionDataAccess().GetAllTransitions();
            }
            else
            {
                transitionHistoryBunifuDataGridView.DataSource = new TransitionDataAccess().SearchTransitions(transitionSearchBunifuTextBox.Text);
            }
        }

        private void transitionSearchBunifuButton_Click(object sender, EventArgs e)
        {
            if (transitionSearchBunifuTextBox.Text == "")
            {
                MessageBox.Show("Provide transition ID for searching transiton.");
                transitionHistoryBunifuDataGridView.DataSource = new TransitionDataAccess().GetAllTransitions();
            }
            else
            {
                transitionHistoryBunifuDataGridView.DataSource = new TransitionDataAccess().SearchTransitions(transitionSearchBunifuTextBox.Text);
            }
        }

        private void transitionRefreshBunifuButton_Click(object sender, EventArgs e)
        {
            transitionSearchBunifuTextBox.Text = "";
            transitionHistoryBunifuDataGridView.DataSource = new TransitionDataAccess().GetAllTransitions();
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
