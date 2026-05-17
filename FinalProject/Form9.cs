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
    public partial class Form9 : Form
    {
        public int LoggedInStudentID = -1;
        public Form9()
        {
            InitializeComponent();
        }
        public Form9(int studentID)
        {
            InitializeComponent();
            LoggedInStudentID = studentID;
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            LoadStudentCourses();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4(LoggedInStudentID);
            f4.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void LoadStudentCourses()
        {
            SqlConnection con = new SqlConnection(
                "Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

            string query = @"
SELECT 
    c.CourseName,
    c.CourseCode,
    c.CourseCredit,
    c.CourseTiming
FROM StudentCourse sc
INNER JOIN Course c 
    ON sc.CourseID = c.CourseID
WHERE sc.StudentID = @StudentID";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@StudentID", LoggedInStudentID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            da.Fill(dt);

            dataGridView1.DataSource = dt;

            dataGridView1.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
