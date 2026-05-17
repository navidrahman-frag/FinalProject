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

            if (string.IsNullOrWhiteSpace(email) ||string.IsNullOrWhiteSpace(password) ||string.IsNullOrWhiteSpace(role))
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
                            int realStudentID = -1;
                            string studentQuery = "SELECT StudentID FROM Student WHERE UserID = @UserID";
                            using (SqlCommand studentCmd = new SqlCommand(studentQuery, con))
                            {
                                studentCmd.Parameters.AddWithValue("@UserID", userId);
                                object result = studentCmd.ExecuteScalar();
                                if (result != null)
                                {
                                    realStudentID = Convert.ToInt32(result);
                                }
                            }

                            if (realStudentID != -1)
                            {
                                Form4 studentDashboard = new Form4(realStudentID);
                                studentDashboard.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Student profile not found! The UserID (" + userId + ") is not linked to any StudentID in the Student table. Please register a new student account using the Registration Form to fix this.");
                            }
                        }
                        else if (userType == "Super Admin")
                        {
                            Form10 superAdminDashboard = new Form10();
                            superAdminDashboard.Show();
                            this.Hide();
                        }
                        else if (userType == "Admin")
                        {
                            Form5 adminDashboard = new Form5();
                            adminDashboard.Show();
                            this.Hide();
                        }
                        else if (userType == "Teacher")
                        {
                            int realTeacherID = -1;
                            string teacherQuery = "SELECT TeacherID FROM Teacher WHERE UserID = @UserID";
                            using (SqlCommand teacherCmd = new SqlCommand(teacherQuery, con))
                            {
                                teacherCmd.Parameters.AddWithValue("@UserID", userId);
                                object result = teacherCmd.ExecuteScalar();
                                if (result != null)
                                {
                                    realTeacherID = Convert.ToInt32(result);
                                }
                            }

                            if (realTeacherID != -1)
                            {
                                Form6 teacherDashboard = new Form6(realTeacherID);
                                teacherDashboard.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Teacher profile not found for this user account!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Users!");
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
                MessageBox.Show(ex.Message);
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

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
