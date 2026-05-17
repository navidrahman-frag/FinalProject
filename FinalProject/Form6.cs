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
    public partial class Form6 : Form
    {
        public int LoggedInTeacherID = -1;
        int selectedCourseID = -1;
        string selectedCourseTiming = "";
        SqlConnection con = new SqlConnection(@"Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");
        public Form6()
        {
            InitializeComponent();
        }

        public Form6(int teacherId)
        {
            InitializeComponent();
            LoggedInTeacherID = teacherId;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form6_Load(object sender, EventArgs e)
        {
            LoadCourseData();
            LoadPaidStudents();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                selectedCourseID = Convert.ToInt32(row.Cells["CourseID"].Value);

                selectedCourseTiming = row.Cells["CourseTiming"].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedCourseID == -1)
            {
                MessageBox.Show("Please select a course first!");
                return;
            }

            try
            {
                con.Open();

                // STEP 1: CHECK IF COURSE ALREADY ASSIGNED
                string checkQuery = "SELECT TeacherID FROM Course WHERE CourseID=@CourseID";

                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@CourseID", selectedCourseID);

                object result = checkCmd.ExecuteScalar();

                if (result != DBNull.Value && result != null)
                {
                    MessageBox.Show("This course is already assigned to another teacher!");
                    con.Close();
                    return;
                }

                // STEP 1.5: CHECK FOR TIMING CLASH
                string timingQuery = @"
        SELECT COUNT(*) FROM Course 
        WHERE TeacherID = @TeacherID 
        AND REPLACE(CourseTiming, ' ', '') = REPLACE(@CourseTiming, ' ', '')";

                SqlCommand timingCmd = new SqlCommand(timingQuery, con);
                timingCmd.Parameters.AddWithValue("@TeacherID", LoggedInTeacherID);
                timingCmd.Parameters.AddWithValue("@CourseTiming", selectedCourseTiming);

                int clashCount = (int)timingCmd.ExecuteScalar();

                if (clashCount > 0)
                {
                    MessageBox.Show("Timing clash! You are already teaching another course at this exact time.");
                    con.Close();
                    return;
                }

                // STEP 2: ASSIGN COURSE TO LOGGED IN TEACHER
                string updateQuery = @"
        UPDATE Course
        SET TeacherID = @TeacherID
        WHERE CourseID = @CourseID";

                SqlCommand cmd = new SqlCommand(updateQuery, con);

                cmd.Parameters.AddWithValue("@TeacherID", LoggedInTeacherID);
                cmd.Parameters.AddWithValue("@CourseID", selectedCourseID);

                cmd.ExecuteNonQuery();

                con.Close();

                MessageBox.Show("Course assigned successfully!");

                LoadCourseData();
                LoadPaidStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();

                string query = @"
        SELECT 
            c.CourseID,
            c.CourseName,
            c.CourseCode,
            c.CourseCredit,
            c.CourseTiming,
            t.TeacherID,
            t.FirstName + ' ' + t.LastName AS TeacherName
        FROM Course c
        LEFT JOIN Teacher t
        ON c.TeacherID = t.TeacherID
        WHERE c.CourseCode = @CourseCode";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@CourseCode", textBox1.Text.Trim());

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;

                dataGridView1.Columns["CourseID"].Visible = false;

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                con.Close();

                textBox1.Clear();
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No course found!");
                    LoadCourseData(); // reload all data
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoadCourseData()
        {
            try
            {
                con.Open();

                string query = @"
        SELECT 
            c.CourseID,
            c.CourseName,
            c.CourseCode,
            c.CourseCredit,
            c.CourseTiming,
            t.TeacherID,
            t.FirstName + ' ' + t.LastName AS TeacherName

        FROM Course c
        LEFT JOIN Teacher t
        ON c.TeacherID = t.TeacherID";

                SqlDataAdapter da = new SqlDataAdapter(query, con);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;


                dataGridView1.Columns["CourseID"].Visible = false;


                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


                dataGridView1.RowTemplate.Height = 35;


                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;


                dataGridView1.AllowUserToAddRows = false;


                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;


                dataGridView1.ReadOnly = true;

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            LoadCourseData();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void LoadPaidStudents()
        {
            try
            {
                con.Open();

                string query = @"
        SELECT 
            s.StudentID,
            s.FirstName + ' ' + s.LastName AS StudentName,
            c.CourseName,
            c.CourseCode,
            c.CourseCredit
        FROM StudentCourse sc
        INNER JOIN Student s ON sc.StudentID = s.StudentID
        INNER JOIN Course c ON sc.CourseID = c.CourseID
        INNER JOIN StudentPayment sp 
            ON sp.StudentID = sc.StudentID
        WHERE sp.PaymentStatus = 'Paid'
        AND c.TeacherID = @TeacherID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@TeacherID", LoggedInTeacherID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView2.DataSource = dt;

                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView2.AllowUserToAddRows = false;
                dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView2.ReadOnly = true;

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();

                string query = @"
        SELECT 
            s.StudentID,
            s.FirstName + ' ' + s.LastName AS StudentName,
            c.CourseName,
            c.CourseCode,
            c.CourseCredit
        FROM StudentCourse sc
        INNER JOIN Student s ON sc.StudentID = s.StudentID
        INNER JOIN Course c ON sc.CourseID = c.CourseID
        INNER JOIN StudentPayment sp 
            ON sp.StudentID = sc.StudentID
        WHERE sp.PaymentStatus = 'Paid'
        AND c.TeacherID = @TeacherID
        AND (s.FirstName LIKE @Search OR s.LastName LIKE @Search OR (s.FirstName + ' ' + s.LastName) LIKE @Search)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@TeacherID", LoggedInTeacherID);
                cmd.Parameters.AddWithValue("@Search", "%" + textBox2.Text.Trim() + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView2.DataSource = dt;

                con.Close();

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No student found with that name in your courses!");
                    textBox2.Clear();
                    LoadPaidStudents(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form18 f18 = new Form18(LoggedInTeacherID);
            f18.Show();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
