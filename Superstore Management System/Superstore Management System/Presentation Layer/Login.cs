using Superstore_Management_System.Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Superstore_Management_System.Presentation_Layer;
using Superstore_Management_System.Entitites;
using Superstore_Management_System.Presentation_Layer.Clerk;
using Superstore_Management_System.Presentation_Layer.Cashier;

namespace Superstore_Management_System.Presntation_Layer
{
    public partial class Login : Form
    {
        private bool mouseDown;
        private Point offset;

        [Obsolete]
        public Login()
        {
            InitializeComponent();
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void topBarbunifuGradientPanel_MouseDown(object sender, MouseEventArgs e)
        {
            offset.X = e.X;
            offset.Y = e.Y;

            mouseDown = true;
        }

        private void topBarbunifuGradientPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if(mouseDown == true)
            {
                Point currentScreenPosition = PointToScreen(e.Location);
                Location = new Point(currentScreenPosition.X - offset.X, currentScreenPosition.Y - offset.Y);
            }
        }

        private void topBarbunifuGradientPanel_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void minimizeBunifuButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        [Obsolete]
        private void loginBunifuButton_Click(object sender, EventArgs e)
        {
            if (usernameBunifuTextBox.Text == "" || passwordBunifuTextBox.Text == "")
            {
                MessageBox.Show("Required Fields are empty.");
            }
            else
            {
                if (new UserDataAccess().ValidateLogin(usernameBunifuTextBox.Text, passwordBunifuTextBox.Text) == true)
                {
                    User user = new User();
                    user = new UserDataAccess().GetEmployeeByUsername(usernameBunifuTextBox.Text);
                    if(user.Status == "Proceed") 
                    { 
                        if (user.Position == "Manager")
                        {
                            new ManagerPanel().Show();
                            this.Hide();
                        }
                        else if(user.Position == "Clerk")
                        {
                            new ClerkPanelCategoriesCUD().Show();
                            this.Hide();
                        }
                        else if(user.Position == "Cashier")
                        {
                            new CashierHome().Show();
                            this.Hide();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Your account suspended. Try to contact your manager.");
                    }
                }
                else
                {
                    passwordBunifuTextBox.Text = "";
                    MessageBox.Show("Invalid username or incorrect password.");
                }
            }
        }
    }
}
