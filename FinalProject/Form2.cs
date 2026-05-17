using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            textBox7.UseSystemPasswordChar = true;
            textBox8.UseSystemPasswordChar = true;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Male");
            comboBox1.Items.Add("Female");
            comboBox2.Items.Add("Married");
            comboBox2.Items.Add("Unmarried");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True";

            string firstName = textBox1.Text.Trim();
            string lastName = textBox2.Text.Trim();
            string phone = textBox3.Text.Trim();
            string address = textBox4.Text.Trim();
            string email = textBox5.Text.Trim();
            string password = textBox7.Text.Trim();
            string confirmPassword = textBox8.Text.Trim();
            string collegeName = textBox9.Text.Trim();
            string universityName = textBox10.Text.Trim();
            string alternativeNo = textBox11.Text.Trim();
            string coreSubject = textBox12.Text.Trim();

            string gender = comboBox1.Text;
            string maritalStatus = comboBox2.Text;

            if (firstName == "" || lastName == "" || phone == "" || address == "" ||
                email == "" || password == "" || confirmPassword == "")
            {
                MessageBox.Show("All fields are required!");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match!");
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    string checkQuery = "SELECT COUNT(*) FROM [User] WHERE Email=@Email";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, con, transaction);
                    checkCmd.Parameters.AddWithValue("@Email", email);

                    int exists = (int)checkCmd.ExecuteScalar();

                    if (exists > 0)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Email already exists!");
                        return;
                    }

                    string insertUser = @"INSERT INTO [User] (Email, Password, UserType)VALUES (@Email, @Password, 'Student');SELECT SCOPE_IDENTITY();";

                    SqlCommand cmdUser = new SqlCommand(insertUser, con, transaction);
                    cmdUser.Parameters.AddWithValue("@Email", email);
                    cmdUser.Parameters.AddWithValue("@Password", password);

                    int userId = Convert.ToInt32(cmdUser.ExecuteScalar());

                    string insertStudent = @"INSERT INTO Student(FirstName, LastName, Phone, Adress, UserID, Gender, MaritalStatus, CollegeName, UniversityName, AlternativeNo, CoreSubject)VALUES(@FirstName, @LastName, @Phone, @Adress, @UserID, @Gender, @MaritalStatus, @CollegeName, @UniversityName, @AlternativeNo, @CoreSubject)";

                    SqlCommand cmdStudent = new SqlCommand(insertStudent, con, transaction);

                    cmdStudent.Parameters.AddWithValue("@FirstName", firstName);
                    cmdStudent.Parameters.AddWithValue("@LastName", lastName);
                    cmdStudent.Parameters.AddWithValue("@Phone", phone);
                    cmdStudent.Parameters.AddWithValue("@Adress", address);
                    cmdStudent.Parameters.AddWithValue("@UserID", userId);
                    cmdStudent.Parameters.AddWithValue("@Gender", gender);
                    cmdStudent.Parameters.AddWithValue("@MaritalStatus", maritalStatus);
                    cmdStudent.Parameters.AddWithValue("@CollegeName", collegeName);
                    cmdStudent.Parameters.AddWithValue("@UniversityName", universityName);
                    cmdStudent.Parameters.AddWithValue("@AlternativeNo", alternativeNo);
                    cmdStudent.Parameters.AddWithValue("@CoreSubject", coreSubject);

                    cmdStudent.ExecuteNonQuery();

                    transaction.Commit();

                    MessageBox.Show("Student Registered Successfully!");
                    ClearFields();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Close();
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool isHidden = textBox7.UseSystemPasswordChar;
            textBox7.UseSystemPasswordChar = !isHidden;
            textBox8.UseSystemPasswordChar = !isHidden;

            if (isHidden)
                button2.Text = "Hide Password";
            else
                button2.Text = "Show Password";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void ClearFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox6.Clear();

            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
        }
    }
}
