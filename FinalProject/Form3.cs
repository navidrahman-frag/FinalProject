using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Male");
            comboBox1.Items.Add("Female");
            comboBox2.Items.Add("Bangladeshi");
            comboBox2.Items.Add("Indian");
            comboBox2.Items.Add("Pakistani");
            comboBox2.Items.Add("American");
            comboBox2.Items.Add("British");
            comboBox2.Items.Add("Canadian");
            comboBox2.Items.Add("Australian");
            comboBox3.Items.Add("Single");
            comboBox3.Items.Add("Married");
            comboBox3.Items.Add("Divorced");
            textBox14.UseSystemPasswordChar = true;
            textBox15.UseSystemPasswordChar = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

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

        private void textBox8_TextChanged(object sender, EventArgs e)
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

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True";

            
            string firstName = textBox1.Text.Trim();
            string lastName = textBox2.Text.Trim();
            string email = textBox3.Text.Trim();
            string phone = textBox4.Text.Trim();
            string address = textBox5.Text.Trim();
            string altNo = textBox6.Text.Trim();
            string department = textBox7.Text.Trim();
            string experience = textBox8.Text.Trim();
            string subject = textBox9.Text.Trim();
            string designation = textBox10.Text.Trim();
            string institution = textBox11.Text.Trim();
            string education = textBox12.Text.Trim();
            string password = textBox14.Text.Trim();
            string confirmPassword = textBox15.Text.Trim();

            // ComboBoxes
            string gender = comboBox1.Text;
            string nationality = comboBox2.Text;
            string maritalStatus = comboBox3.Text;

            if (string.IsNullOrWhiteSpace(firstName) ||string.IsNullOrWhiteSpace(lastName) ||string.IsNullOrWhiteSpace(email) ||string.IsNullOrWhiteSpace(phone) ||string.IsNullOrWhiteSpace(address) ||string.IsNullOrWhiteSpace(password) ||string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please fill all required fields!");
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
                    string checkQuery = "SELECT COUNT(*) FROM [User] WHERE Email = @Email";

                    SqlCommand checkCmd = new SqlCommand(checkQuery, con, transaction);
                    checkCmd.Parameters.AddWithValue("@Email", email);

                    int exists = (int)checkCmd.ExecuteScalar();

                    if (exists > 0)
                    {
                        transaction.Rollback();
                        MessageBox.Show("This email is already registered!");
                        return;
                    }

                    string insertUser = @"INSERT INTO [User] (Email, Password, UserType)VALUES (@Email, @Password, 'Teacher');SELECT SCOPE_IDENTITY();";

                    SqlCommand cmdUser = new SqlCommand(insertUser, con, transaction);
                    cmdUser.Parameters.AddWithValue("@Email", email);
                    cmdUser.Parameters.AddWithValue("@Password", password);

                    object result = cmdUser.ExecuteScalar();

                    if (result == null)
                    {
                        throw new Exception("User creation failed.");
                    }

                    int userId = Convert.ToInt32(result);

             
                    string insertTeacher = @"INSERT INTO Teacher(FirstName, LastName, Gender, Nationality, PhoneNo, Adress, MaritalStatus,AlternativeNo, Department, YearsOfExperience, SubjectSpecialization,Designation, PreviuosInstitution, Education, UserID)VALUES(@FirstName, @LastName, @Gender, @Nationality, @PhoneNo, @Adress, @MaritalStatus,@AlternativeNo, @Department, @YearsOfExperience, @SubjectSpecialization,@Designation, @PreviuosInstitution, @Education, @UserID)";

                    SqlCommand cmdTeacher = new SqlCommand(insertTeacher, con, transaction);

                    cmdTeacher.Parameters.AddWithValue("@FirstName", firstName);
                    cmdTeacher.Parameters.AddWithValue("@LastName", lastName);
                    cmdTeacher.Parameters.AddWithValue("@Gender", gender);
                    cmdTeacher.Parameters.AddWithValue("@Nationality", nationality);
                    cmdTeacher.Parameters.AddWithValue("@PhoneNo", phone);
                    cmdTeacher.Parameters.AddWithValue("@Adress", address);
                    cmdTeacher.Parameters.AddWithValue("@MaritalStatus", maritalStatus);
                    cmdTeacher.Parameters.AddWithValue("@AlternativeNo", altNo);
                    cmdTeacher.Parameters.AddWithValue("@Department", department);
                    cmdTeacher.Parameters.AddWithValue("@YearsOfExperience", experience);
                    cmdTeacher.Parameters.AddWithValue("@SubjectSpecialization", subject);
                    cmdTeacher.Parameters.AddWithValue("@Designation", designation);
                    cmdTeacher.Parameters.AddWithValue("@PreviuosInstitution", institution);
                    cmdTeacher.Parameters.AddWithValue("@Education", education);
                    cmdTeacher.Parameters.AddWithValue("@UserID", userId);

                    cmdTeacher.ExecuteNonQuery();

                    transaction.Commit();

                    MessageBox.Show("Teacher Registered Successfully!");
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
            bool isHidden = textBox14.UseSystemPasswordChar;

            textBox14.UseSystemPasswordChar = !isHidden;
            textBox15.UseSystemPasswordChar = !isHidden;

            if (isHidden)
            {
                button3.Text = "Hide Password";
            }
            else
            {
                button3.Text = "Show Password";
            }
        }
    }
}
