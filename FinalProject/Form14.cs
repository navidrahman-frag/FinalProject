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
    public partial class Form14 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

        public Form14()
        {
            InitializeComponent();
        }

        private void Form14_Load(object sender, EventArgs e)
        {
            LoadPaymentData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void LoadPaymentData()
        {
            string query = @"
            SELECT 
                sp.PaymentID,
                sp.StudentID,
                s.FirstName + ' ' + s.LastName AS StudentName,
                sp.TotalAmount,
                sp.PaidAmount,
                sp.PaymentStatus
            FROM StudentPayment sp
            INNER JOIN Student s ON sp.StudentID = s.StudentID";

            SqlDataAdapter da = new SqlDataAdapter(query, con);

            DataTable dt = new DataTable();

            da.Fill(dt);

            dataGridView1.DataSource = dt;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
        }
    }
}
