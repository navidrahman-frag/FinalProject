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
    public partial class Form8 : Form
    {
        public int LoggedInStudentID = -1;
        public Form8()
        {
            InitializeComponent();
        }
        public Form8(int studentID)
        {
            InitializeComponent();
            LoggedInStudentID = studentID;
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            LoadStudentProfile();
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
        private void LoadStudentProfile()
        {
            SqlConnection con = new SqlConnection(
                "Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

            string query = @"
    SELECT 
        s.StudentID,
        s.FirstName + ' ' + s.LastName AS StudentName,
        sp.PaymentStatus
    FROM Student s
    LEFT JOIN StudentPayment sp 
        ON s.StudentID = sp.StudentID
    WHERE s.StudentID = @StudentID";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@StudentID", LoggedInStudentID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            da.Fill(dt);

            dataGridView1.DataSource = dt;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
