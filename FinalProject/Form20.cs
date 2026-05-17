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
    public partial class Form20 : Form
    {
        public int LoggedInStudentID = -1;
        int selectedCourseID = -1;
        int selectedTeacherID = -1;
        public Form20()
        {
            InitializeComponent();
        }
        public Form20(int studentID)
        {
            InitializeComponent();
            LoggedInStudentID = studentID;
        }

        private void Form20_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("1");
            comboBox1.Items.Add("2");
            comboBox1.Items.Add("3");
            comboBox1.Items.Add("4");
            comboBox1.Items.Add("5");
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBox2.Items.Clear();
            comboBox2.Items.Add("Very Good");
            comboBox2.Items.Add("Good");
            comboBox2.Items.Add("Moderate");
            comboBox2.Items.Add("Bad");
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;

            LoadMyRatings();
        }
        private void LoadMyRatings()
        {
            SqlConnection con = new SqlConnection(@"Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

            con.Open();

            string query = @"
    SELECT 
        c.CourseID,
        c.CourseName,
        t.TeacherID,
        t.FirstName + ' ' + t.LastName AS TeacherName,
        tr.RatingValue,
        tr.Comment
    FROM StudentCourse sc
    INNER JOIN Course c ON sc.CourseID = c.CourseID
    INNER JOIN Teacher t ON c.TeacherID = t.TeacherID
    LEFT JOIN TeacherRating tr 
        ON sc.StudentID = tr.StudentID AND c.CourseID = tr.CourseID AND t.TeacherID = tr.TeacherID
    WHERE sc.StudentID = @StudentID";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@StudentID", LoggedInStudentID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridView1.DataSource = dt;

            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns["CourseID"].Visible = false;
                dataGridView1.Columns["TeacherID"].Visible = false;
            }

            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dataGridView1.RowTemplate.Height = 30;
            dataGridView1.ColumnHeadersHeight = 35;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                if (row.Cells["CourseID"].Value != DBNull.Value)
                {
                    selectedCourseID = Convert.ToInt32(row.Cells["CourseID"].Value);
                }
                if (row.Cells["TeacherID"].Value != DBNull.Value)
                {
                    selectedTeacherID = Convert.ToInt32(row.Cells["TeacherID"].Value);
                }
                
                if (row.Cells["RatingValue"].Value != DBNull.Value)
                {
                    comboBox1.Text = row.Cells["RatingValue"].Value.ToString();
                }
                else
                {
                    comboBox1.Text = "";
                }
                
                if (row.Cells["Comment"].Value != DBNull.Value)
                {
                    comboBox2.Text = row.Cells["Comment"].Value.ToString();
                }
                else
                {
                    comboBox2.Text = "";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedCourseID == -1 || selectedTeacherID == -1)
            {
                MessageBox.Show("Please select a course/teacher from the list first.");
                return;
            }

            if (string.IsNullOrWhiteSpace(comboBox1.Text))
            {
                MessageBox.Show("Please enter a rating value.");
                return;
            }

            int ratingValue;
            if (!int.TryParse(comboBox1.Text, out ratingValue))
            {
                MessageBox.Show("Please enter a valid numeric rating.");
                return;
            }

            string comment = comboBox2.Text;

            using (SqlConnection con = new SqlConnection(@"Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True"))
            {
                con.Open();

                string checkQuery = "SELECT COUNT(*) FROM TeacherRating WHERE StudentID = @StudentID AND CourseID = @CourseID AND TeacherID = @TeacherID";
                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@StudentID", LoggedInStudentID);
                checkCmd.Parameters.AddWithValue("@CourseID", selectedCourseID);
                checkCmd.Parameters.AddWithValue("@TeacherID", selectedTeacherID);

                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    string updateQuery = "UPDATE TeacherRating SET RatingValue = @RatingValue, Comment = @Comment WHERE StudentID = @StudentID AND CourseID = @CourseID AND TeacherID = @TeacherID";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                    updateCmd.Parameters.AddWithValue("@RatingValue", ratingValue);
                    updateCmd.Parameters.AddWithValue("@Comment", comment);
                    updateCmd.Parameters.AddWithValue("@StudentID", LoggedInStudentID);
                    updateCmd.Parameters.AddWithValue("@CourseID", selectedCourseID);
                    updateCmd.Parameters.AddWithValue("@TeacherID", selectedTeacherID);
                    updateCmd.ExecuteNonQuery();
                    MessageBox.Show("Rating updated successfully!");
                }
                else
                {
                    string insertQuery = "INSERT INTO TeacherRating (StudentID, TeacherID, CourseID, RatingValue, Comment) VALUES (@StudentID, @TeacherID, @CourseID, @RatingValue, @Comment)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, con);
                    insertCmd.Parameters.AddWithValue("@StudentID", LoggedInStudentID);
                    insertCmd.Parameters.AddWithValue("@TeacherID", selectedTeacherID);
                    insertCmd.Parameters.AddWithValue("@CourseID", selectedCourseID);
                    insertCmd.Parameters.AddWithValue("@RatingValue", ratingValue);
                    insertCmd.Parameters.AddWithValue("@Comment", comment);
                    insertCmd.ExecuteNonQuery();
                    MessageBox.Show("Rating submitted successfully!");
                }
            }

            LoadMyRatings();
        }
    }
}
