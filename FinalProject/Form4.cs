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

namespace FinalProject
{
    public partial class Form4 : Form
    {
        public int LoggedInStudentID= -1;
        

        int selectedCourseID;
        int selectedCredit;
        string selectedTiming;
        DataTable selectedCourses = new DataTable();
        bool isConfirmed = false;
        bool semesterConfirmed = false;

        public Form4()
        {
            InitializeComponent();
        }

        public Form4(int studentId)
        {
            InitializeComponent();
            LoggedInStudentID = studentId;
        }
        

        private void Form4_Load(object sender, EventArgs e)
        {
            
            LoadCourses();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.MultiSelect = false;
            dataGridView1.RowTemplate.Height = 30;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            LoadStudentCourses();
            if (selectedCourses.Columns.Count == 0)
            {
                selectedCourses.Columns.Add("CourseID", typeof(int));
                selectedCourses.Columns.Add("CourseCode", typeof(string));
                selectedCourses.Columns.Add("CourseCredit", typeof(int));
                selectedCourses.Columns.Add("CourseTiming", typeof(string));
            }
            isConfirmed = false;
            CheckStudentCourses();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["CourseCode"].Value.ToString();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string search = textBox1.Text.Trim();

            SqlConnection con = new SqlConnection("Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

            string query = @"SELECT CourseID, CourseName, CourseCode, CourseCredit, CourseFee, CourseTiming 
                     FROM Course
                     WHERE CourseCode LIKE @Search";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Search", "%" + search + "%");

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridView1.DataSource = dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (LoggedInStudentID <= 0)
            {
                MessageBox.Show("Student not logged in!");
                return;
            }

            if (selectedCourses.Rows.Count == 0)
            {
                MessageBox.Show("No courses selected to confirm!");
                return;
            }

            
            int totalCredit = 0;

            foreach (DataRow row in selectedCourses.Rows)
            {
                totalCredit += Convert.ToInt32(row["CourseCredit"]);
            }

            
            if (totalCredit < 12)
            {
                MessageBox.Show("Minimum 12 credits required! Current: " + totalCredit);
                return;
            }

            
            if (totalCredit > 18)
            {
                MessageBox.Show("Maximum 18 credits allowed! Current: " + totalCredit);
                return;
            }

            SqlConnection con = new SqlConnection("Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");
            con.Open();

            foreach (DataRow row in selectedCourses.Rows)
            {
                int courseID = Convert.ToInt32(row["CourseID"]);

                string checkQuery = @"SELECT COUNT(*) FROM StudentCourse 
                              WHERE StudentID = @StudentID AND CourseID = @CourseID";

                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@StudentID", LoggedInStudentID);
                checkCmd.Parameters.AddWithValue("@CourseID", courseID);

                int exists = (int)checkCmd.ExecuteScalar();

                if (exists == 0)
                {
                    string insert = @"INSERT INTO StudentCourse(StudentID, CourseID)
                              VALUES(@StudentID, @CourseID)";

                    SqlCommand cmd = new SqlCommand(insert, con);
                    cmd.Parameters.AddWithValue("@StudentID", LoggedInStudentID);
                    cmd.Parameters.AddWithValue("@CourseID", courseID);

                    cmd.ExecuteNonQuery();
                }
                
            }

            con.Close();

            MessageBox.Show("Courses confirmed successfully!");

            selectedCourses.Rows.Clear();
            selectedCourses.AcceptChanges();


            LoadStudentCourses();
            isConfirmed = true;

        }
        private void LoadCourses()
        {
            SqlConnection con = new SqlConnection("Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

            string query = @"SELECT CourseID, CourseName, CourseCode, CourseCredit, CourseFee, CourseTiming FROM Course";

            SqlDataAdapter da = new SqlDataAdapter(query, con);

            DataTable dt = new DataTable();

            da.Fill(dt);

            dataGridView1.DataSource = dt;
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void LoadStudentCourses()
        {
            SqlConnection con = new SqlConnection("Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

            string query = @"
    SELECT 
        sc.StudentCourseID,
        sc.StudentID,
        c.CourseName,
        c.CourseCode,
        c.CourseCredit,
        c.CourseTiming
    FROM StudentCourse sc
    INNER JOIN Course c ON sc.CourseID = c.CourseID
    WHERE sc.StudentID = @StudentID";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@StudentID", LoggedInStudentID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridView2.DataSource = dt;

            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (LoggedInStudentID <= 0)
            {
                MessageBox.Show("Student not logged in!");
                return;
            }
            if (isConfirmed)
            {
                MessageBox.Show("Cannot select courses. Delete the selected courses first to select again!");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please enter or select a Course Code!");
                return;
            }

            SqlConnection con = new SqlConnection("Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");
            con.Open();

            string findCourse = @"SELECT CourseID, CourseCredit, CourseTiming FROM Course WHERE CourseCode = @CourseCode";

            SqlCommand cmdFind = new SqlCommand(findCourse, con);
            cmdFind.Parameters.AddWithValue("@CourseCode", textBox1.Text);

            SqlDataReader reader = cmdFind.ExecuteReader();

            if (!reader.Read())
            {
                MessageBox.Show("Invalid Course Code!");
                con.Close();
                return;
            }

            int courseID = Convert.ToInt32(reader["CourseID"]);
            int credit = Convert.ToInt32(reader["CourseCredit"]);
            string timing = reader["CourseTiming"].ToString();

            reader.Close();
            con.Close();

            
            foreach (DataRow row in selectedCourses.Rows)
            {
                if ((int)row["CourseID"] == courseID)
                {
                    MessageBox.Show("Already selected!");
                    return;
                }
            }
            foreach (DataRow row in selectedCourses.Rows)
            {
                string existingTiming = row["CourseTiming"].ToString().Trim().Replace(" ", "");
                string newTiming = timing.ToString().Trim().Replace(" ", "");

                if (existingTiming.Equals(newTiming, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Timing clash detected with another selected course!");
                    return;
                }
            }

           
            selectedCourses.Rows.Add(courseID, textBox1.Text, credit, timing);

            MessageBox.Show("Course added to selection (not saved yet)");
            textBox1.Clear();
         
            dataGridView2.DataSource = selectedCourses;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (semesterConfirmed)
            {
                MessageBox.Show("Cannot delete course. Semester is already confirmed.");
                return;
            }
            if (LoggedInStudentID <= 0)
            {
                MessageBox.Show("Student not logged in!");
                return;

            }
            SqlConnection paymentCon = new SqlConnection("Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

            paymentCon.Open();

            string paymentQuery = @"SELECT PaymentStatus FROM StudentPayment WHERE StudentID = @StudentID";

            SqlCommand paymentCmd = new SqlCommand(paymentQuery, paymentCon);

            paymentCmd.Parameters.AddWithValue("@StudentID", LoggedInStudentID);

            object paymentResult = paymentCmd.ExecuteScalar();

            paymentCon.Close();

            
            if (paymentResult != null &&
                paymentResult.ToString() == "Paid")
            {
                MessageBox.Show(
                    "Cannot delete course because payment is already cleared!");

                return;
            }

            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a course to delete!");
                return;
            }
            if (!dataGridView2.Columns.Contains("StudentCourseID"))
            {
                MessageBox.Show("Can't do that operation now!");
                return;
            }

            int studentCourseID = Convert.ToInt32(
                dataGridView2.SelectedRows[0].Cells["StudentCourseID"].Value
            );

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this course?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.No)
                return;

            SqlConnection con = new SqlConnection("Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");
            con.Open();

            string query = @"DELETE FROM StudentCourse 
                     WHERE StudentCourseID = @ID AND StudentID = @StudentID";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@ID", studentCourseID);
            cmd.Parameters.AddWithValue("@StudentID", LoggedInStudentID);

            int rows = cmd.ExecuteNonQuery();

            con.Close();

            if (rows > 0)
            {
                MessageBox.Show("Course deleted successfully!");
            }
            else
            {
                MessageBox.Show("Delete failed!");
            }

            LoadStudentCourses();
            CheckStudentCourses();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (LoggedInStudentID <= 0)
            {
                MessageBox.Show("Student not logged in!");
                return;
            }

            if (IsSemesterPaid())
            {
                semesterConfirmed = true;

                MessageBox.Show("Semester Course Confirmed Successfully!");

                button10.Enabled = false;
                button13.Enabled = false;
            }
            else
            {
                MessageBox.Show("Payment not completed yet!");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7(LoggedInStudentID);
            f7.Show();
            this.Hide();
        }
        private void CheckStudentCourses()
        {
            SqlConnection con = new SqlConnection("Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

            string query = @"SELECT COUNT(*) 
                     FROM StudentCourse 
                     WHERE StudentID = @StudentID";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@StudentID", LoggedInStudentID);

            con.Open();

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            con.Close();

            
            if (count == 0)
            {
                isConfirmed = false;
            }
            else
            {
                isConfirmed = true;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (semesterConfirmed)
            {
                MessageBox.Show("Cannot remove course. Semester is already confirmed.");
                return;
            }
            if (!dataGridView2.Columns.Contains("CourseID"))
            {
                MessageBox.Show("Can't do that operation now!");
                return;
            }
            
            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a course to remove!");
                return;
            }

            
            int courseID = Convert.ToInt32(
                dataGridView2.SelectedRows[0].Cells["CourseID"].Value
            );

            
            foreach (DataRow row in selectedCourses.Rows)
            {
                if ((int)row["CourseID"] == courseID)
                {
                    row.Delete();
                    break;
                }
            }

            
            selectedCourses.AcceptChanges();

            
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = selectedCourses;

            MessageBox.Show("Course removed from selection!");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form8 f8 = new Form8(LoggedInStudentID);
            f8.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form9 f9 = new Form9(LoggedInStudentID);
            f9.Show();
            this.Hide();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Form19 f19 = new Form19(LoggedInStudentID);
            f19.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form20 f20 = new Form20(LoggedInStudentID);
            f20.Show();
        }
        private bool IsSemesterPaid()
        {
            SqlConnection con = new SqlConnection(
                "Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

            con.Open();

            string query = @"SELECT PaymentStatus 
                     FROM StudentPayment 
                     WHERE StudentID = @StudentID";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@StudentID", LoggedInStudentID);

            object result = cmd.ExecuteScalar();

            con.Close();

            return result != null && result.ToString() == "Paid";
        }
        
        
    }
}
