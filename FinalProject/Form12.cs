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
   // int selectedStudentID = -1;
    public partial class Form12 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");
        int selectedStudentID = -1;
        public Form12()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form12_Load(object sender, EventArgs e)
        {
            LoadStudentData();
            comboBox2.Items.Add("Male");
            comboBox2.Items.Add("Female");

            comboBox1.Items.Add("Married");
            comboBox1.Items.Add("Unmarried");
        }
        public void LoadStudentData()
        {
            string query = @"
            SELECT 
            s.StudentID,
            s.FirstName,
            s.LastName,
            s.Phone,
            s.Adress,
            s.CollegeName,
            s.UniversityName,
            s.AlternativeNo,
            s.CoreSubject,
            s.MaritalStatus,
            s.Gender,
            u.Email,
            u.UserType
            FROM Student s
            INNER JOIN [User] u
            ON s.UserID = u.UserID";

            SqlDataAdapter da = new SqlDataAdapter(query, con);

            DataTable dt = new DataTable();

            da.Fill(dt);

            dataGridView1.DataSource = dt;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedStudentID == -1)
            {
                MessageBox.Show("Select a student first");
                return;
            }

            string query = @"UPDATE Student SET
    FirstName=@FirstName,
    LastName=@LastName,
    Phone=@Phone,
    Adress=@Adress,
    CollegeName=@CollegeName,
    UniversityName=@UniversityName,
    AlternativeNo=@AlternativeNo,
    CoreSubject=@CoreSubject,
    MaritalStatus=@MaritalStatus,
    Gender=@Gender
    WHERE StudentID=@StudentID";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@FirstName", textBox1.Text);
            cmd.Parameters.AddWithValue("@LastName", textBox2.Text);
            cmd.Parameters.AddWithValue("@Phone", textBox3.Text);
            cmd.Parameters.AddWithValue("@Adress", textBox4.Text);
            cmd.Parameters.AddWithValue("@CollegeName", textBox5.Text);
            cmd.Parameters.AddWithValue("@UniversityName", textBox6.Text);
            cmd.Parameters.AddWithValue("@AlternativeNo", textBox7.Text);
            cmd.Parameters.AddWithValue("@CoreSubject", textBox8.Text);
            cmd.Parameters.AddWithValue("@MaritalStatus", comboBox1.Text);
            cmd.Parameters.AddWithValue("@Gender", comboBox2.Text);
            cmd.Parameters.AddWithValue("@StudentID", selectedStudentID);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Student Updated Successfully");

            LoadStudentData();
            ClearFields();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" ||
        textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" ||
        textBox7.Text == "" || textBox8.Text == "" ||
        comboBox1.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            string query = @"INSERT INTO Student
    (FirstName, LastName, Phone, Adress, CollegeName, UniversityName,
     AlternativeNo, CoreSubject, MaritalStatus, Gender)
    VALUES
    (@FirstName, @LastName, @Phone, @Adress, @CollegeName, @UniversityName,
     @AlternativeNo, @CoreSubject, @MaritalStatus, @Gender)";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@FirstName", textBox1.Text);
            cmd.Parameters.AddWithValue("@LastName", textBox2.Text);
            cmd.Parameters.AddWithValue("@Phone", textBox3.Text);
            cmd.Parameters.AddWithValue("@Adress", textBox4.Text);
            cmd.Parameters.AddWithValue("@CollegeName", textBox5.Text);
            cmd.Parameters.AddWithValue("@UniversityName", textBox6.Text);
            cmd.Parameters.AddWithValue("@AlternativeNo", textBox7.Text);
            cmd.Parameters.AddWithValue("@CoreSubject", textBox8.Text);
            cmd.Parameters.AddWithValue("@MaritalStatus", comboBox1.Text);
            cmd.Parameters.AddWithValue("@Gender", comboBox2.Text);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Student Added Successfully");

            LoadStudentData();
            ClearFields();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                selectedStudentID = Convert.ToInt32(row.Cells["StudentID"].Value);

                textBox1.Text = row.Cells["FirstName"].Value.ToString();
                textBox2.Text = row.Cells["LastName"].Value.ToString();
                textBox3.Text = row.Cells["Phone"].Value.ToString();
                textBox4.Text = row.Cells["Adress"].Value.ToString();
                textBox5.Text = row.Cells["CollegeName"].Value.ToString();
                textBox6.Text = row.Cells["UniversityName"].Value.ToString();
                textBox7.Text = row.Cells["AlternativeNo"].Value.ToString();
                textBox8.Text = row.Cells["CoreSubject"].Value.ToString();

                comboBox1.Text = row.Cells["MaritalStatus"].Value.ToString();
                comboBox2.Text = row.Cells["Gender"].Value.ToString();
            }
        }
        public void ClearFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();

            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;

            selectedStudentID = -1;
        }
    }
}
