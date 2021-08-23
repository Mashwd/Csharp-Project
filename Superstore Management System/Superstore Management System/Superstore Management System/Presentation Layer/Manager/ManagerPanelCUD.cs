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
    public partial class ManagerPanelCUD: Form
    {
        private bool mouseDown;
        private Point offset;

        [Obsolete]
        public ManagerPanelCUD()
        {
            InitializeComponent();
        }

        private void ManagerPanelCUD_FormClosing(object sender, FormClosingEventArgs e)
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

        private void ManagerPanelCUD_Load(object sender, EventArgs e)
        {
            employeeBunifuButton.ApplyState(employeeBunifuButton.OnPressedState);
            deleteBunifuDataGridView.DataSource = new UserDataAccess().GetAllEmployee();
        }

        [Obsolete]
        private void dashBoardBunifuButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            new ManagerPanel().Show();

        }

        private void resetBunifuButton_Click(object sender, EventArgs e)
        {
            userNameBunifuTextBox.Text = string.Empty;
            passwordBunifuTextBox.Text = string.Empty;
            retypePasswordBunifuTextBox.Text = string.Empty;
            nameBunifuTextBox.Text = string.Empty;
            salaryBunifuTextBox.Text = string.Empty;
            positionComboBox.Text = string.Empty;
            proceedBunifuRadioButton.Checked = false;
            suspendBunifuRadioButton.Checked = false;
            surityBunifuCheckBox.Checked = false;
        }

        private void createBunifuButton_Click(object sender, EventArgs e)
        {
            bool flag = false; 
            if (userNameBunifuTextBox.Text == "" || passwordBunifuTextBox.Text == "" || retypePasswordBunifuTextBox.Text == ""||
                nameBunifuTextBox.Text == "" || salaryBunifuTextBox.Text == "" || positionComboBox.Text == "" || 
                (proceedBunifuRadioButton.Checked == false && suspendBunifuRadioButton.Checked == false) || 
                surityBunifuCheckBox.Checked == false)
            {
                flag = true;
                MessageBox.Show("Required Fields are empty.");
            }
            else
            {
                if(new UserDataAccess().ValidateUsername(userNameBunifuTextBox.Text) == false)
                {
                    User user = new User();
                    user.Username = userNameBunifuTextBox.Text;
                    if (passwordBunifuTextBox.Text == retypePasswordBunifuTextBox.Text)
                    {
                        user.Password = passwordBunifuTextBox.Text;
                    }
                    else
                    {
                        flag = true;
                        MessageBox.Show("Password didn't match.");
                    }
                    user.Name = nameBunifuTextBox.Text;
                    if((positionComboBox.Text == "None" || positionComboBox.Text == "None") && flag == false)
                    {
                        flag = true;
                        MessageBox.Show("Provide a position for employee.");
                    }
                    else
                        user.Position = positionComboBox.Text;

                    if(proceedBunifuRadioButton.Checked == false && suspendBunifuRadioButton.Checked == false && flag == false)
                    {
                        flag = true;
                        MessageBox.Show("Provide employee status.");
                    }
                    else
                    {
                        if (proceedBunifuRadioButton.Checked == true)
                            user.Status = "Proceed";
                        else
                            user.Status = "Suspend";
                    }

                    if (salaryBunifuTextBox.Text.All(Char.IsNumber) == true)
                        user.Salary = Convert.ToDouble(salaryBunifuTextBox.Text);
                    else if(flag == false)
                    {
                        flag = true;
                        MessageBox.Show("Invalid input in salary.");
                    }

                    user.JoiningDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

                    if (!flag)
                    {
                        if (new UserDataAccess().UserRegister(user) == true)
                        {
                            deleteBunifuDataGridView.DataSource = new UserDataAccess().GetAllEmployee();
                            MessageBox.Show("User registered successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Error while saving.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Username already exist. Try another one.");
                }

            }
        }

        private void updateEmployeeLoadbutton_Click(object sender, EventArgs e)
        {
            if(updateSearchBunifuTextBox.Text != "")
            {
                if (updateSearchBunifuTextBox.Text.All(Char.IsNumber) == true)
                {
                    User user = new User();
                    user = new UserDataAccess().GetEmployeeByID(updateSearchBunifuTextBox.Text);

                    updateUserNameBunifuTextBox.Text = user.Username;
                    updateOldPasswordBunifuTextBox.Text = user.Password;
                    updateNameBunifuTextBox.Text = user.Name;
                    updateSalaryBunifuTextBox.Text = user.Salary.ToString();
                    updatePositionBunifuComboBox.Text = user.Position;
                }
                else
                {
                    MessageBox.Show("Provide a valid user ID.");
                }
            }
            else
            {
                MessageBox.Show("Provide employee ID to load employee's details.");
            }
            
        }

        private void updateResetBunifuButton_Click(object sender, EventArgs e)
        {
            updateSearchBunifuTextBox.Text = "";
            updateUserNameBunifuTextBox.Text = "";
            updateOldPasswordBunifuTextBox.Text = "";
            updatePasswordBunifuTextBox.Text = "";
            updateNameBunifuTextBox.Text = "";
            updateSalaryBunifuTextBox.Text = "";
            updatePositionBunifuComboBox.Text = "";
        }

        private void updateBunifuButton_Click(object sender, EventArgs e)
        {
            bool flag = false;
            if (updateSearchBunifuTextBox.Text != "")
            {
                User user = new User();
                user = new UserDataAccess().GetEmployeeByID(updateSearchBunifuTextBox.Text);

                if (updatePasswordBunifuTextBox.Text != "")
                    user.Password = updatePasswordBunifuTextBox.Text;

                if (updateNameBunifuTextBox.Text == "")
                {
                    flag = true;
                    MessageBox.Show("Provide a name.");
                }
                else
                    user.Name = updateNameBunifuTextBox.Text;

                if (updateSalaryBunifuTextBox.Text == "")
                {
                    flag = true;
                    MessageBox.Show("Provide a name.");
                } 
                else if(updateSalaryBunifuTextBox.Text.All(Char.IsNumber) == true)
                    user.Salary = Convert.ToDouble(updateSalaryBunifuTextBox.Text);
                else
                {
                    flag = true;
                    MessageBox.Show("Provide number for salary.");
                }


                if (updatePositionBunifuComboBox.Text == "")
                {
                    flag = true;
                    MessageBox.Show("Position can't be empty.");
                }
                else
                {
                    if (user.Position != "Manager")
                    { 
                        if (updatePositionBunifuComboBox.Text == "Cashier" || updatePositionBunifuComboBox.Text == "Clerk")
                        {
                            user.Position = updatePositionBunifuComboBox.Text;
                        }
                        else
                        {
                            flag = true;
                            MessageBox.Show("Invalid Position.");
                        }
                    }
                    else
                    {
                        if(updatePositionBunifuComboBox.Text != "Manager" || updatePositionBunifuComboBox.Text == "")
                        {
                            flag = true;
                            updatePositionBunifuComboBox.Text = "Manager";
                            MessageBox.Show("Manager position should manager.");
                        }
                    }
                }

                if (!flag)
                {
                    bool res = new UserDataAccess().UpdateEmployee(user);

                    if (res)
                    {
                        deleteBunifuDataGridView.DataSource = new UserDataAccess().GetAllEmployee();
                        MessageBox.Show("User updated successfully.");
                    }
                    else
                        MessageBox.Show("Error while updating");
                }

            }
            else
                MessageBox.Show("Provide a employee ID to update details.");


        }

        private void deleteResetBunifuButton_Click(object sender, EventArgs e)
        {
            deleteSearchBunifuTextBox.Text = "";
        }

        private void deleteBunifuButton_Click(object sender, EventArgs e)
        {
            if(deleteSearchBunifuTextBox.Text == "")
            {
                MessageBox.Show("Provide employee ID to delete.");
            }
            else
            {
                User user = new User();
                user = new UserDataAccess().GetEmployeeByID(deleteSearchBunifuTextBox.Text);

                if(user.Position == "Manager")
                {
                    MessageBox.Show("Manager account can't be deleted.");
                }
                else
                {
                    DialogResult dr = MessageBox.Show("Are you sure?", "", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            bool res = new UserDataAccess().DeleteEmployee(deleteSearchBunifuTextBox.Text);
                            if (res)
                            {

                                deleteBunifuDataGridView.DataSource = new UserDataAccess().GetAllEmployee();
                                deleteSearchBunifuTextBox.Text = "";
                                MessageBox.Show("Employee deleted successfully.");
                            }   
                            else
                                MessageBox.Show("Error while deleteing");
                            break;
                        case DialogResult.No:
                            break;
                    }
                    
                }


            }
        }

        private void deleteBunifuDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                deleteSearchBunifuTextBox.Text = "";
                if (deleteBunifuDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString() != "Manager")
                {
                    deleteSearchBunifuTextBox.Text = deleteBunifuDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                }
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
