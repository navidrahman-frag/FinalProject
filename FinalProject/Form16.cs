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
    public partial class Form16 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");
        public Form16()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form16_Load(object sender, EventArgs e)
        {
            LoadCourseTeacherData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public void LoadCourseTeacherData()
        {
            string query = @"
            SELECT 
            c.CourseName,
            c.CourseCode,
            c.CourseCredit,
            c.TeacherID AS AssignedTeacherID,
            t.FirstName + ' ' + t.LastName AS TeacherName
            FROM Course c
            INNER JOIN Teacher t
            ON c.TeacherID = t.TeacherID";

            SqlDataAdapter da = new SqlDataAdapter(query, con);

            DataTable dt = new DataTable();

            da.Fill(dt);

            dataGridView1.DataSource = dt;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
        }

    }
}
