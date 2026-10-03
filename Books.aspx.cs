using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DigitalLibrary
{
    public partial class Books : System.Web.UI.Page
    {
        string connectionString =
            ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBooks();
            }
        }

        protected void btnAddBook_Click(object sender, EventArgs e)
        {
            string bookName = txtBookName.Text.Trim();
            string author = txtAuthor.Text.Trim();
            string category = txtCategory.Text.Trim();

            if (string.IsNullOrWhiteSpace(bookName) ||
                string.IsNullOrWhiteSpace(author) ||
                string.IsNullOrWhiteSpace(category))
            {
                lblMessage.Text = "Please fill all the fields.";
                return;
            }

            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO Books
                        (BookName, Author, Quantity, Category)
                        VALUES (@BookName, @Author, @Quantity, @Category)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@BookName", bookName);
                        cmd.Parameters.AddWithValue("@Author", author);
                        cmd.Parameters.AddWithValue("@Quantity", 1);
                        cmd.Parameters.AddWithValue("@Category", category);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                lblMessage.Text = "Book added successfully!";
                txtBookName.Text = "";
                txtAuthor.Text = "";
                txtCategory.Text = "";

                LoadBooks();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
            }
        }

        private void LoadBooks()
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(connectionString))
                {
                    string query = @"SELECT BookID, BookName, Author,
                                     Quantity, Category
                                     FROM Books ORDER BY BookID DESC";

                    using (SqlDataAdapter da =
                        new SqlDataAdapter(query, con))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        gvBooks.DataSource = dt;
                        gvBooks.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error loading books: " + ex.Message;
            }
        }
    }
}