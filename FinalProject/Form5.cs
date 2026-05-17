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
    public partial class Form5 : Form
    {
        int selectedCourseID;
        int selectedStudentID;
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            LoadCourses();

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            LoadStudents();

            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView2.ScrollBars = ScrollBars.Both;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AllowUserToResizeColumns = true;

            dataGridView2.AllowUserToResizeRows = true;

            dataGridView2.DefaultCellStyle.WrapMode =
                DataGridViewTriState.False;

            dataGridView2.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.None;

            foreach (DataGridViewColumn column in dataGridView2.Columns)
            {
                column.Width = 150;
            }

            comboBox1.Items.Add("Single");
            comboBox1.Items.Add("Married");

            comboBox2.Items.Add("Male");
            comboBox2.Items.Add("Female");

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedCourseID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);

            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            textBox16.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
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

       

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

            try
            {
                if (selectedCourseID == 0 ||string.IsNullOrWhiteSpace(textBox1.Text) ||string.IsNullOrWhiteSpace(textBox2.Text) ||string.IsNullOrWhiteSpace(textBox3.Text) ||string.IsNullOrWhiteSpace(textBox16.Text) ||string.IsNullOrWhiteSpace(textBox5.Text))
                {
                    MessageBox.Show("Please select a course to update!");
                    return;
                }
                con.Open();

                string query = @"UPDATE Course SET CourseName = @CourseName,CourseCode = @CourseCode,CourseCredit = @CourseCredit,CourseFee = @CourseFee,CourseTiming = @CourseTiming WHERE CourseID = @CourseID";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@CourseID", selectedCourseID);
                cmd.Parameters.AddWithValue("@CourseName", textBox1.Text);
                cmd.Parameters.AddWithValue("@CourseCode", textBox2.Text);
                cmd.Parameters.AddWithValue("@CourseCredit", textBox3.Text);
                cmd.Parameters.AddWithValue("@CourseFee", textBox16.Text);
                cmd.Parameters.AddWithValue("@CourseTiming", textBox5.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Course Updated Successfully!");

                LoadCourses();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text) ||string.IsNullOrWhiteSpace(textBox2.Text) ||string.IsNullOrWhiteSpace(textBox3.Text) ||string.IsNullOrWhiteSpace(textBox16.Text) ||string.IsNullOrWhiteSpace(textBox5.Text))
                {
                    MessageBox.Show("Please fill all text fields!");
                    return;
                }
                con.Open();
                string checkQuery = "SELECT COUNT(*) FROM Course WHERE CourseName = @CourseName";

                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@CourseName", textBox1.Text);

                int exists = (int)checkCmd.ExecuteScalar();

                if (exists > 0)
                {
                    MessageBox.Show("This course name already exists!");
                    return;
                }

                string query = @"INSERT INTO Course(CourseName, CourseCode, CourseCredit, CourseFee, CourseTiming)VALUES(@CourseName, @CourseCode, @CourseCredit, @CourseFee, @CourseTiming)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@CourseName", textBox1.Text);
                cmd.Parameters.AddWithValue("@CourseCode", textBox2.Text);
                cmd.Parameters.AddWithValue("@CourseCredit", textBox3.Text);
                cmd.Parameters.AddWithValue("@CourseFee", textBox16.Text);
                cmd.Parameters.AddWithValue("@CourseTiming", textBox5.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Course Saved Successfully!");

                LoadCourses();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (selectedCourseID == 0)
            {
                MessageBox.Show("Please select a course first!");
                return;
            }

            SqlConnection con = new SqlConnection("Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

            try
            {
                con.Open();

                DialogResult result = MessageBox.Show("Are you sure you want to delete this course?","Confirm Delete",MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM Course WHERE CourseID = @CourseID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@CourseID", selectedCourseID);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Course Deleted Successfully!");

                    LoadCourses();  
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

            try
            {
                con.Open();

                string query = @"SELECT CourseID, CourseName, CourseCode, CourseCredit, CourseFee, CourseTiming FROM Course WHERE CourseName LIKE @Search OR CourseCode LIKE @Search";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Search", "%" + textBox2.Text + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }
        private void LoadCourses()
        {
            SqlConnection con = new SqlConnection("Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

            try
            {
                con.Open();

                string query = @"SELECT CourseID, CourseName, CourseCode, CourseCredit, CourseFee, CourseTiming FROM Course";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }

        }
        private void ClearFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox5.Clear();
            textBox16.Clear();
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

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedStudentID =Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells[0].Value);
            textBox4.Text =dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();

            textBox6.Text =dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();

            textBox7.Text =dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();

            textBox8.Text =dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();

            textBox9.Text =dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString();

            textBox12.Text =dataGridView2.Rows[e.RowIndex].Cells[5].Value.ToString();

            textBox13.Text =dataGridView2.Rows[e.RowIndex].Cells[6].Value.ToString();

            textBox14.Text = dataGridView2.Rows[e.RowIndex].Cells[7].Value.ToString();

            textBox15.Text =dataGridView2.Rows[e.RowIndex].Cells[8].Value.ToString();

            comboBox1.Text =dataGridView2.Rows[e.RowIndex].Cells[9].Value.ToString();

            comboBox2.Text =dataGridView2.Rows[e.RowIndex].Cells[10].Value.ToString();
        }
       

        private void button6_Click(object sender, EventArgs e)
        {
            if (selectedStudentID == 0)
            {
                MessageBox.Show("Please select a student first!");
                return;
            }

            SqlConnection con = new SqlConnection(
                "Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

            try
            {
                con.Open();

                string query = @"UPDATE Student SET FirstName = @FirstName,LastName = @LastName,Phone = @Phone,Adress = @Adress,CollegeName = @CollegeName,UniversityName = @UniversityName,AlternativeNo = @AlternativeNo,CoreSubject = @CoreSubject,MaritalStatus = @MaritalStatus,Gender = @Gender WHERE StudentID = @StudentID";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@StudentID", selectedStudentID);

                cmd.Parameters.AddWithValue("@FirstName", textBox6.Text);
                cmd.Parameters.AddWithValue("@LastName", textBox7.Text);
                cmd.Parameters.AddWithValue("@Phone", textBox8.Text);
                cmd.Parameters.AddWithValue("@Adress", textBox9.Text);

                cmd.Parameters.AddWithValue("@CollegeName", textBox12.Text);
                cmd.Parameters.AddWithValue("@UniversityName", textBox13.Text);

                cmd.Parameters.AddWithValue("@AlternativeNo", textBox14.Text);
                cmd.Parameters.AddWithValue("@CoreSubject", textBox15.Text);

                cmd.Parameters.AddWithValue("@MaritalStatus", comboBox1.Text);
                cmd.Parameters.AddWithValue("@Gender", comboBox2.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Student Updated Successfully!");

                LoadStudents();

                textBox4.Clear();
                textBox6.Clear();
                textBox7.Clear();
                textBox8.Clear();
                textBox9.Clear();
                textBox12.Clear();
                textBox13.Clear();
                textBox14.Clear();
                textBox15.Clear();

                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;

                selectedStudentID = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

            try
            {
                if (string.IsNullOrWhiteSpace(textBox6.Text) ||
                    string.IsNullOrWhiteSpace(textBox7.Text) ||
                    string.IsNullOrWhiteSpace(textBox8.Text) ||
                    string.IsNullOrWhiteSpace(textBox9.Text))
                {
                    MessageBox.Show("Please fill all required fields!");
                    return;
                }

                con.Open();

                string query = @"INSERT INTO Student(FirstName, LastName, Phone, Adress, CollegeName, UniversityName, AlternativeNo, CoreSubject, MaritalStatus, Gender)VALUES(@FirstName, @LastName, @Phone, @Adress,@CollegeName, @UniversityName,@AlternativeNo, @CoreSubject,@MaritalStatus, @Gender)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@FirstName", textBox6.Text);
                cmd.Parameters.AddWithValue("@LastName", textBox7.Text);
                cmd.Parameters.AddWithValue("@Phone", textBox8.Text);
                cmd.Parameters.AddWithValue("@Adress", textBox9.Text);

                cmd.Parameters.AddWithValue("@CollegeName", textBox12.Text);
                cmd.Parameters.AddWithValue("@UniversityName", textBox13.Text);

                cmd.Parameters.AddWithValue("@AlternativeNo", textBox14.Text);
                cmd.Parameters.AddWithValue("@CoreSubject", textBox15.Text);

                cmd.Parameters.AddWithValue("@MaritalStatus", comboBox1.Text);
                cmd.Parameters.AddWithValue("@Gender", comboBox2.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Student Added Successfully!");

                LoadStudents();

                textBox4.Clear();
                textBox6.Clear();
                textBox7.Clear();
                textBox8.Clear();
                textBox9.Clear();
                textBox12.Clear();
                textBox13.Clear();
                textBox14.Clear();
                textBox15.Clear();

                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (selectedStudentID == 0)
            {
                MessageBox.Show("Please select a student first!");
                return;
            }

            SqlConnection con = new SqlConnection("Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

            try
            {
                con.Open();

                DialogResult result = MessageBox.Show("Are you sure you want to delete this student?","Delete Student",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    string query = "DELETE FROM Student WHERE StudentID = @StudentID";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@StudentID", selectedStudentID);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Student Deleted Successfully!");

                    LoadStudents();

                    textBox4.Clear();
                    textBox6.Clear();
                    textBox7.Clear();
                    textBox8.Clear();
                    textBox9.Clear();
                    textBox12.Clear();
                    textBox13.Clear();
                    textBox14.Clear();
                    textBox15.Clear();

                    comboBox1.SelectedIndex = -1;
                    comboBox2.SelectedIndex = -1;

                    selectedStudentID = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

            try
            {
                con.Open();

                string query = @"SELECT StudentID, FirstName, LastName, Phone, Adress,CollegeName,UniversityName,AlternativeNo,CoreSubject,MaritalStatus,Gender FROM Student WHERE StudentID = @ID OR FirstName LIKE @Search";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Search", "%" + textBox4.Text + "%");
                cmd.Parameters.AddWithValue("@ID", textBox4.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView2.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No Student Found!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
        private void LoadStudents()
        {
            SqlConnection con = new SqlConnection("Data Source=ASUS;Initial Catalog=OnlineCourse;Integrated Security=True");

            try
            {
                con.Open();

                string query = @"SELECT StudentID,FirstName,LastName,Phone,Adress,CollegeName,UniversityName,AlternativeNo,CoreSubject,MaritalStatus,Gender FROM Student";

                SqlDataAdapter da = new SqlDataAdapter(query, con);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView2.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
    }
}
