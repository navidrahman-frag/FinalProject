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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Student");
            comboBox1.Items.Add("Admin");
            comboBox1.Items.Add("Teacher");
            comboBox1.Items.Add("Super Admin");
            textBox5.PasswordChar = '*';
            textBox5.UseSystemPasswordChar = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True";

            string email = textBox4.Text.Trim();
            string password = textBox5.Text.Trim();
            string role = comboBox1.Text;

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Please fill all fields!");
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"SELECT UserID, UserType FROM [User] WHERE Email = @Email AND Password = @Password AND UserType = @UserType";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@UserType", role);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int userId = Convert.ToInt32(reader["UserID"]);
                        string userType = reader["UserType"].ToString();

                        reader.Close();

                        MessageBox.Show("Login Successful!");

                       
                        if (userType == "Student")
                        {
                            Form4 studentDashboard = new Form4();
                            studentDashboard.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Only Students can access this dashboard!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Email, Password or Role!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.Show();
            this.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
