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
    public partial class Form19 : Form
    {
        public int LoggedInStudentID = -1;
        SqlConnection con = new SqlConnection(@"Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");
        public Form19(int studentID)
        {
            InitializeComponent();
            LoggedInStudentID = studentID;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form19_Load(object sender, EventArgs e)
        {
            LoadMyGrades();
        }
        private void LoadMyGrades()
        {
            con.Open();

            string query = @"
    SELECT 
        t.FirstName + ' ' + t.LastName AS TeacherName,
        c.CourseName,
        c.CourseCredit,
        sg.Marks,
        sg.GradeValue
    FROM StudentGrade sg
    INNER JOIN Course c 
        ON sg.CourseID = c.CourseID
    INNER JOIN Teacher t 
        ON c.TeacherID = t.TeacherID
    WHERE sg.StudentID = @StudentID";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@StudentID", LoggedInStudentID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridView1.DataSource = dt;

            con.Close();
        }
    }
}
