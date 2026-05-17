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
    public partial class Form11 : Form
    {
        int selectedCourseID = -1;
        SqlConnection con = new SqlConnection(@"Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");
        public Form11()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form11_Load(object sender, EventArgs e)
        {
            LoadCourseData();
        }
        public void LoadCourseData()
        {
            string query = "SELECT CourseID, CourseName, CourseCode, CourseCredit, CourseFee, CourseTiming, TeacherID FROM Course";

            SqlDataAdapter da = new SqlDataAdapter(query, con);

            DataTable dt = new DataTable();

            da.Fill(dt);

            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedCourseID == -1)
            {
                MessageBox.Show("Please select a course first");
                return;
            }

            string query = @"UPDATE Course SET
    CourseName=@CourseName,
    CourseCode=@CourseCode,
    CourseCredit=@CourseCredit,
    CourseFee=@CourseFee,
    CourseTiming=@CourseTiming
    WHERE CourseID=@CourseID";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@CourseName", textBox1.Text);
            cmd.Parameters.AddWithValue("@CourseCode", textBox2.Text);
            cmd.Parameters.AddWithValue("@CourseCredit", textBox3.Text);
            cmd.Parameters.AddWithValue("@CourseFee", textBox4.Text);
            cmd.Parameters.AddWithValue("@CourseTiming", textBox5.Text);
            cmd.Parameters.AddWithValue("@CourseID", selectedCourseID);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Course Updated Successfully");

            LoadCourseData();
            ClearFields();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string query = @"INSERT INTO Course
    (CourseName, CourseCode, CourseCredit, CourseFee, CourseTiming)
    VALUES
    (@CourseName, @CourseCode, @CourseCredit, @CourseFee, @CourseTiming)";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@CourseName", textBox1.Text);
            cmd.Parameters.AddWithValue("@CourseCode", textBox2.Text);
            cmd.Parameters.AddWithValue("@CourseCredit", textBox3.Text);
            cmd.Parameters.AddWithValue("@CourseFee", textBox4.Text);
            cmd.Parameters.AddWithValue("@CourseTiming", textBox5.Text);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Course Added Successfully");

            LoadCourseData();
            ClearFields();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (selectedCourseID == -1)
            {
                MessageBox.Show("Please select a course first");
                return;
            }

            string checkQuery = "SELECT TeacherID FROM Course WHERE CourseID=@CourseID";

            SqlCommand checkCmd = new SqlCommand(checkQuery, con);

            checkCmd.Parameters.AddWithValue("@CourseID", selectedCourseID);

            con.Open();

            object teacherID = checkCmd.ExecuteScalar();

            if (teacherID != DBNull.Value && teacherID != null)
            {
                con.Close();

                MessageBox.Show("Cannot delete course because a teacher is assigned.");

                return;
            }

            string deleteQuery = "DELETE FROM Course WHERE CourseID=@CourseID";

            SqlCommand deleteCmd = new SqlCommand(deleteQuery, con);

            deleteCmd.Parameters.AddWithValue("@CourseID", selectedCourseID);

            deleteCmd.ExecuteNonQuery();

            con.Close();

            MessageBox.Show("Course Deleted Successfully");

            LoadCourseData();
            ClearFields();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                selectedCourseID = Convert.ToInt32(row.Cells["CourseID"].Value);

                textBox1.Text = row.Cells["CourseName"].Value.ToString();
                textBox2.Text = row.Cells["CourseCode"].Value.ToString();
                textBox3.Text = row.Cells["CourseCredit"].Value.ToString();
                textBox4.Text = row.Cells["CourseFee"].Value.ToString();
                textBox5.Text = row.Cells["CourseTiming"].Value.ToString();
            }
        }
        public void ClearFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();

            selectedCourseID = -1;
        }
    }
}
