using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using RegistrationForm.Connection;

namespace RegistrationForm
{
    public partial class RegisterForm : Form
    {

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            loadUserData();
            firstnameTextBox.Select();
        }

        private void loadUserData()
        {
            DataTable userData = ServerConnection.executeSQL("SELECT (First_Name + ' ' + Last_Name) AS FullName, Username from User_Registration_Form");
            dataGridView1.DataSource = userData;
            dataGridView1.Columns[0].HeaderText = "FullName";
            dataGridView1.Columns[1].HeaderText = "Username";
            dataGridView1.Columns[0].Width = 200;
            dataGridView1.Columns[1].Width = 200;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            clearControls();
            firstnameTextBox.Select();
        }

        private void clearControls()
        {
            foreach (TextBox tb in this.Controls.OfType<TextBox>())
            {
                tb.Text = string.Empty;
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you want to permanently delete the selected record?",
                    "Delete Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                {
                    ServerConnection.executeSQL("DELETE from User_Registration_Form WHERE Username = '" + dataGridView1.CurrentRow.Cells[1].Value + "'");
                    loadUserData();

                    MessageBox.Show("The record has been deleted.",
                        "Delete Data",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    return;
                }
            }
            catch (Exception)
            {
                // An error occured!
            }
        }
        private void submitButton_Click(object sender, EventArgs e)
        {
            MessageBoxButtons button = MessageBoxButtons.OK;
            MessageBoxIcon icon = MessageBoxIcon.Information;
            string caption = "Save Data : Completion";

            if (string.IsNullOrEmpty(firstnameTextBox.Text))
            {
                MessageBox.Show("Please Enter First Name.", caption, button, icon);
                firstnameTextBox.Select();
                return;
            }

            if (string.IsNullOrEmpty(lastnameTextBox.Text))
            {
                MessageBox.Show("Please Enter Last Name.", caption, button, icon);
                lastnameTextBox.Select();
                return;
            }

            if (string.IsNullOrEmpty(dobDateTimePicker.Text))
            {
                MessageBox.Show("Please enter Date of Birth.", caption, button, icon);
                dobDateTimePicker.Select();
                return;
            }

            if (string.IsNullOrEmpty(usernameTextBox.Text))
            {
                MessageBox.Show("Please Enter Username.", caption, button, icon);
                usernameTextBox.Select();
                return;
            }

            if (string.IsNullOrEmpty(passwordTextBox.Text))
            {
                MessageBox.Show("Please Enter Password.", caption, button, icon);
                passwordTextBox.Select();
                return;
            }

            if (string.IsNullOrEmpty(reenterTtextBox.Text))
            {
                MessageBox.Show("Please Re-enter Password.", caption, button, icon);
                reenterTtextBox.Select();
                return;
            }

            if (passwordTextBox.Text != reenterTtextBox.Text)
            {
                MessageBox.Show("Your Passowrd didn't match.", caption, button, icon);
                reenterTtextBox.SelectAll();
                return;
            }

            string yourSQL = "SELECT Username from User_Registration_Form WHERE Username = '" + usernameTextBox.Text + "'";
            DataTable checkDuplicates = RegistrationForm.Connection.ServerConnection.executeSQL(yourSQL);

            if(checkDuplicates.Rows.Count > 0)
            {
                MessageBox.Show("The username already exists. Please try another username.",
                    "Login Form", MessageBoxButtons.OK, MessageBoxIcon.Information);
                usernameTextBox.SelectAll();
                return;
            }

            DialogResult result;
            result = MessageBox.Show("Do you want to save the record?", "Save Data : Login Form",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string mySQL = string.Empty;

                mySQL += "INSERT into User_Registration_Form (First_Name, Last_Name, Gender, DOB, Email_ID, Contact_Number," +
                    "Alternate_Contact_Number, Address, State, Pincode, Maritial_Status, Account_Type, " +
                    "Account_Number, Occupation, Username, Password)";

                mySQL += "VALUES ('" + firstnameTextBox.Text + "','" + lastnameTextBox + "','" + genderComboBox + "','" + dobDateTimePicker + "','" + emailTextBox + "','"
                    + contactTextBox + "','" + altcontactTextBox + "','" + addressTextBox + "','" + stateTextBox + "','" + pincodeTextBox + "','" + maritialStatusComboBox + "','"
                    + accounttypeComboBox + "','" + accountNumberTextBox + "','" + occupationTextBox + "','" + usernameTextBox + "','" + passwordTextBox + " ')";

                RegistrationForm.Connection.ServerConnection.executeSQL(mySQL);

                MessageBox.Show("The record has been saved successfully.",
                    "Login Form", MessageBoxButtons.OK, MessageBoxIcon.Information);

                loadUserData();
                clearControls();
            }
        }      

        private void accounttypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            accounttypeComboBox.Items.Add("Savings Account");
            accounttypeComboBox.Items.Add("Current Acoount");
            accounttypeComboBox.Items.Add("Recuring Deposit");
            accounttypeComboBox.Items.Add("IRA Account");
            accounttypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void genderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            genderComboBox.Items.Add("Male");
            genderComboBox.Items.Add("Female");
            genderComboBox.Items.Add("Transgender");
            genderComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void maritialStatusComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            maritialStatusComboBox.Items.Add("Married");
            maritialStatusComboBox.Items.Add("Unmarried");
            maritialStatusComboBox.Items.Add("Divorced");
            maritialStatusComboBox.Items.Add("Widow");
            maritialStatusComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }
    }
}
//string dt = "05-05-2020";
//DateTime oDate = DateTime.Parse(dt);
