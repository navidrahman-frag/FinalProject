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
    
    public partial class Form7 : Form
    {
        public int LoggedInStudentID = -1;

        public Form7()
        {
            InitializeComponent();
        }

        public Form7(int studentID)
        {
            InitializeComponent();
            LoggedInStudentID = studentID;
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            LoadPayments();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dataGridView1.RowTemplate.Height = 40;

            dataGridView1.AllowUserToResizeColumns = true;
            dataGridView1.AllowUserToResizeRows = true;
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
        private void LoadPayments()
        {
            SqlConnection con = new SqlConnection(
        "Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

            string query = @"
        SELECT 
            s.StudentID,
            s.FirstName + ' ' + s.LastName AS StudentName,
            STRING_AGG(c.CourseName, ', ') AS Courses,
            SUM(c.CourseFee) AS TotalAmount,
            ISNULL(sp.PaidAmount, 0) AS PaidAmount,
            ISNULL(sp.PaymentStatus, 'Pending') AS PaymentStatus
        FROM Student s
        INNER JOIN StudentCourse sc ON sc.StudentID = s.StudentID
        INNER JOIN Course c ON c.CourseID = sc.CourseID
        LEFT JOIN StudentPayment sp ON sp.StudentID = s.StudentID
        WHERE s.StudentID = @StudentID
        GROUP BY 
            s.StudentID,
            s.FirstName,
            s.LastName,
            sp.PaidAmount,
            sp.PaymentStatus";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@StudentID", LoggedInStudentID);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridView1.DataSource = dt;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Enter payment amount!");
                return;
            }

            
            decimal paidAmount;

            if (!decimal.TryParse(textBox1.Text, out paidAmount))
            {
                MessageBox.Show("Enter valid amount!");
                return;
            }

            SqlConnection con = new SqlConnection(
                "Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

            con.Open();

            
            string checkQuery = @"
        SELECT PaymentStatus
        FROM StudentPayment
        WHERE StudentID = @StudentID";

            SqlCommand checkCmd = new SqlCommand(checkQuery, con);
            checkCmd.Parameters.AddWithValue("@StudentID", LoggedInStudentID);

            object checkResult = checkCmd.ExecuteScalar();

            
            if (checkResult != null &&
                checkResult.ToString() == "Paid")
            {
                MessageBox.Show("Payment already completed!");
                con.Close();
                return;
            }

            
            string totalQuery = @"
        SELECT SUM(c.CourseFee)
        FROM StudentCourse sc
        INNER JOIN Course c
            ON sc.CourseID = c.CourseID
        WHERE sc.StudentID = @StudentID";

            SqlCommand cmdTotal = new SqlCommand(totalQuery, con);

            cmdTotal.Parameters.AddWithValue("@StudentID", LoggedInStudentID);

            object result = cmdTotal.ExecuteScalar();

            decimal totalAmount =
                result == DBNull.Value ? 0 : Convert.ToDecimal(result);

            
            if (totalAmount == 0)
            {
                MessageBox.Show("No courses selected!");
                con.Close();
                return;
            }

            
            string status;

            if (paidAmount < totalAmount)
            {
                decimal remaining = totalAmount - paidAmount;

                MessageBox.Show(
                    "Full payment required!\nRemaining Amount: " + remaining);

                status = "Pending";
            }
            else if (paidAmount == totalAmount)
            {
                status = "Paid";
            }
            else
            {
                MessageBox.Show(
                    "Paid amount cannot be greater than total amount!");

                con.Close();
                return;
            }

            
            string query = @"
        IF EXISTS
        (
            SELECT 1
            FROM StudentPayment
            WHERE StudentID = @StudentID
        )

        BEGIN

            UPDATE StudentPayment
            SET
                TotalAmount = @TotalAmount,
                PaidAmount = @PaidAmount,
                PaymentStatus = @Status
            WHERE StudentID = @StudentID

        END

        ELSE

        BEGIN

            INSERT INTO StudentPayment
            (
                StudentID,
                TotalAmount,
                PaidAmount,
                PaymentStatus
            )

            VALUES
            (
                @StudentID,
                @TotalAmount,
                @PaidAmount,
                @Status
            )

        END";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@StudentID", LoggedInStudentID);

            cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);

            cmd.Parameters.AddWithValue("@PaidAmount", paidAmount);

            cmd.Parameters.AddWithValue("@Status", status);

            cmd.ExecuteNonQuery();

            con.Close();

            if (status == "Paid")
            {
                MessageBox.Show("Payment completed successfully!");
            }

            textBox1.Clear();

            LoadPayments();
        }
    }
}
