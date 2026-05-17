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
    public partial class Form18 : Form
    {
        public int LoggedInTeacherID = -1;
        int selectedCourseID = -1;
        int selectedStudentID = -1;
        SqlConnection con = new SqlConnection(@"Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");
       
        public Form18(int teacherID)
        {
            InitializeComponent();
            LoggedInTeacherID = teacherID;
        }

        private void Form18_Load(object sender, EventArgs e)
        {
            LoadTeacherClassView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedStudentID = Convert.ToInt32(
                    dataGridView1.Rows[e.RowIndex].Cells["StudentID"].Value);

                selectedCourseID = Convert.ToInt32(
                    dataGridView1.Rows[e.RowIndex].Cells["CourseID"].Value);
            }
        }
        private void LoadTeacherClassView()
        {
            con.Open();

            string query = @"
    SELECT 
        c.CourseID,
        c.CourseName,
        s.StudentID,
        s.FirstName + ' ' + s.LastName AS StudentName,
        sg.Marks,
        sg.GradeValue
    FROM Course c

    INNER JOIN StudentCourse sc 
        ON c.CourseID = sc.CourseID

    INNER JOIN Student s 
        ON sc.StudentID = s.StudentID

    LEFT JOIN StudentGrade sg 
        ON sg.StudentID = s.StudentID 
        AND sg.CourseID = c.CourseID

    WHERE c.TeacherID = @TeacherID
    ORDER BY c.CourseName, s.StudentID";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@TeacherID", LoggedInTeacherID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridView1.DataSource = dt;

            con.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (selectedStudentID == -1 || selectedCourseID == -1)
            {
                MessageBox.Show("Select a student first!");
                return;
            }

            con.Open();

            string query = @"
    UPDATE StudentGrade
    SET GradeValue = @GradeValue,
        Marks = @Marks
    WHERE StudentID = @StudentID
    AND CourseID = @CourseID";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@GradeValue", textBox1.Text);
            cmd.Parameters.AddWithValue("@Marks", textBox2.Text);
            cmd.Parameters.AddWithValue("@StudentID", selectedStudentID);
            cmd.Parameters.AddWithValue("@CourseID", selectedCourseID);

            cmd.ExecuteNonQuery();

            con.Close();

            MessageBox.Show("Grade updated successfully!");

            LoadTeacherClassView();
            textBox1.Clear();
            textBox2.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedStudentID == -1 || selectedCourseID == -1)
            {
                MessageBox.Show("Select a student first!");
                return;
            }

            con.Open();

            string query = @"
    INSERT INTO StudentGrade (StudentID, CourseID, GradeValue, Marks)
    VALUES (@StudentID, @CourseID, @GradeValue, @Marks)";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@StudentID", selectedStudentID);
            cmd.Parameters.AddWithValue("@CourseID", selectedCourseID);
            cmd.Parameters.AddWithValue("@GradeValue", textBox1.Text);
            cmd.Parameters.AddWithValue("@Marks", textBox2.Text);

            cmd.ExecuteNonQuery();

            con.Close();

            MessageBox.Show("Grade inserted successfully!");

            LoadTeacherClassView();
            textBox1.Clear();
            textBox2.Clear();
        }
    }
}
