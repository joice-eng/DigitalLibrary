using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace DigitalLibrary
{
    public partial class Students : System.Web.UI.Page
    {
        private string connectionString =
            ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStudents();
            }
        }

        private void LoadStudents()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"SELECT StudentID, StudentName, Age, Department
                                 FROM Students_New
                                 ORDER BY StudentID DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvStudents.DataSource = dt;
                    gvStudents.DataBind();
                }
            }
        }

        protected void btnAddStudent_Click(object sender, EventArgs e)
        {
            string name = txtStudentName.Text.Trim();
            string department = txtDepartment.Text.Trim();
            int age;

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(department) ||
                !int.TryParse(txtAge.Text.Trim(), out age) ||
                age < 1 || age > 120)
            {
                lblMessage.Text = "Enter valid student details and age.";
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO Students_New (StudentName, Age, Department)
                      VALUES (@Name, @Age, @Department)", con))
                {
                    cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = name;
                    cmd.Parameters.Add("@Age", SqlDbType.Int).Value = age;
                    cmd.Parameters.Add("@Department", SqlDbType.VarChar, 100).Value = department;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                txtStudentName.Text = "";
                txtDepartment.Text = "";
                txtAge.Text = "";
                lblMessage.Text = "Student added successfully!";
                LoadStudents();
            }
            catch (Exception)
            {
                lblMessage.Text = "Unable to add student. Please check database.";
            }
        }

        protected void gvStudents_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvStudents.EditIndex = e.NewEditIndex;
            LoadStudents();
        }

        protected void gvStudents_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvStudents.EditIndex = -1;
            LoadStudents();
        }

        protected void gvStudents_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int studentID = Convert.ToInt32(gvStudents.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvStudents.Rows[e.RowIndex];

            string name = ((TextBox)row.Cells[2].Controls[0]).Text.Trim();
            int age;
            string department = ((TextBox)row.Cells[4].Controls[0]).Text.Trim();

            if (!int.TryParse(((TextBox)row.Cells[3].Controls[0]).Text.Trim(), out age)
                || age < 1 || age > 120
                || string.IsNullOrWhiteSpace(name)
                || string.IsNullOrWhiteSpace(department))
            {
                lblMessage.Text = "Enter valid name, age and department.";
                return;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                @"UPDATE Students_New
                  SET StudentName=@Name, Age=@Age, Department=@Department
                  WHERE StudentID=@ID", con))
            {
                cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = name;
                cmd.Parameters.Add("@Age", SqlDbType.Int).Value = age;
                cmd.Parameters.Add("@Department", SqlDbType.VarChar, 100).Value = department;
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = studentID;

                con.Open();
                cmd.ExecuteNonQuery();
            }

            gvStudents.EditIndex = -1;
            lblMessage.Text = "Student updated successfully!";
            LoadStudents();
        }

        protected void gvStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int studentID = Convert.ToInt32(gvStudents.DataKeys[e.RowIndex].Value);

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "DELETE FROM Students_New WHERE StudentID=@ID", con))
            {
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = studentID;
                con.Open();
                cmd.ExecuteNonQuery();
            }

            lblMessage.Text = "Student deleted successfully!";
            LoadStudents();
        }
    }
}