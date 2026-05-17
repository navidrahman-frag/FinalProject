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
    public partial class Form10 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");
        public Form10()
        {
            InitializeComponent();
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            LoadUserData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Close();
        }
        public void LoadUserData()
        {
            string query = "SELECT UserID, Email, Password, UserType FROM [User]";

            SqlDataAdapter da = new SqlDataAdapter(query, con);

            DataTable dt = new DataTable();

            da.Fill(dt);

            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            dataGridView1.Columns["Password"].Visible = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form11 Courseinfo = new Form11();
            Courseinfo.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form12 Studentuser = new Form12();
            Studentuser.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form13 StudentGrade = new Form13();
            StudentGrade.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form14 StudentPayment = new Form14();
            StudentPayment.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form15 TeacherUserData = new Form15();
            TeacherUserData.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form16 TeacherAssignData = new Form16();
            TeacherAssignData.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form17 TeacherRatingData = new Form17();
            TeacherRatingData.Show();
        }
    }
}
