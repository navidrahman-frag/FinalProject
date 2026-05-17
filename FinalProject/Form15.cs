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
    public partial class Form15 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");
        int selectedTeacherID = -1;
        public Form15()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form15_Load(object sender, EventArgs e)
        {
            LoadTeacherData();
            comboBox1.Items.Add("Male");
            comboBox1.Items.Add("Female");

            comboBox2.Items.Add("Bangladeshi");
            comboBox2.Items.Add("Indian");
            comboBox2.Items.Add("Pakistani");
            comboBox2.Items.Add("Nepali");
            comboBox2.Items.Add("American");
            comboBox2.Items.Add("Canadian");
            comboBox2.Items.Add("British");

            comboBox3.Items.Add("Married");
            comboBox3.Items.Add("Unmarried");
            comboBox3.Items.Add("Divorced");

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        public void LoadTeacherData()
        {
            string query = @"
            SELECT 
            TeacherID,
            FirstName,
            LastName,
            Gender,
            Nationality,
            PhoneNo,
            Adress,
            MaritalStatus,
            AlternativeNo,
            Department,
            YearsOfExperience,
            SubjectSpecialization,
            Designation,
            PreviuosInstitution,
            Education
            FROM Teacher";

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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedTeacherID == -1)
            {
                MessageBox.Show("Please select a teacher first");
                return;
            }

            string query = @"UPDATE Teacher SET
    FirstName=@FirstName,
    LastName=@LastName,
    Gender=@Gender,
    Nationality=@Nationality,
    PhoneNo=@PhoneNo,
    Adress=@Adress,
    MaritalStatus=@MaritalStatus,
    AlternativeNo=@AlternativeNo,
    Department=@Department,
    YearsOfExperience=@YearsOfExperience,
    SubjectSpecialization=@SubjectSpecialization,
    Designation=@Designation,
    PreviuosInstitution=@PreviuosInstitution,
    Education=@Education
    WHERE TeacherID=@TeacherID";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@FirstName", textBox1.Text);
            cmd.Parameters.AddWithValue("@LastName", textBox2.Text);
            cmd.Parameters.AddWithValue("@Gender", comboBox1.Text);
            cmd.Parameters.AddWithValue("@Nationality", comboBox2.Text);
            cmd.Parameters.AddWithValue("@PhoneNo", textBox5.Text);
            cmd.Parameters.AddWithValue("@Adress", textBox3.Text);
            cmd.Parameters.AddWithValue("@MaritalStatus", comboBox3.Text);
            cmd.Parameters.AddWithValue("@AlternativeNo", textBox8.Text);
            cmd.Parameters.AddWithValue("@Department", textBox7.Text);
            cmd.Parameters.AddWithValue("@YearsOfExperience", textBox6.Text);
            cmd.Parameters.AddWithValue("@SubjectSpecialization", textBox4.Text);
            cmd.Parameters.AddWithValue("@Designation", textBox9.Text);
            cmd.Parameters.AddWithValue("@PreviuosInstitution", textBox10.Text);
            cmd.Parameters.AddWithValue("@Education", textBox11.Text);

            cmd.Parameters.AddWithValue("@TeacherID", selectedTeacherID);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Teacher Updated Successfully");

            LoadTeacherData();
            ClearFields();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                selectedTeacherID = Convert.ToInt32(row.Cells["TeacherID"].Value);

                textBox1.Text = row.Cells["FirstName"].Value.ToString();
                textBox2.Text = row.Cells["LastName"].Value.ToString();
                comboBox1.Text = row.Cells["Gender"].Value.ToString();
                comboBox2.Text = row.Cells["Nationality"].Value.ToString();
                textBox5.Text = row.Cells["PhoneNo"].Value.ToString();
                textBox3.Text = row.Cells["Adress"].Value.ToString();
                comboBox3.Text = row.Cells["MaritalStatus"].Value.ToString();
                textBox8.Text = row.Cells["AlternativeNo"].Value.ToString();
                textBox7.Text = row.Cells["Department"].Value.ToString();
                textBox6.Text = row.Cells["YearsOfExperience"].Value.ToString();
                textBox4.Text = row.Cells["SubjectSpecialization"].Value.ToString();
                textBox9.Text = row.Cells["Designation"].Value.ToString();
                textBox10.Text = row.Cells["PreviuosInstitution"].Value.ToString();
                textBox11.Text = row.Cells["Education"].Value.ToString();
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
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();

            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;

            selectedTeacherID = -1;
        }
    }
}
