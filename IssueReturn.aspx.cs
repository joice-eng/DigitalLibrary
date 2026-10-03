using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DigitalLibrary
{
    public partial class IssueReturn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string studentName = txtStudentName.Text.Trim();
            string bookName = txtBookName.Text.Trim();
            string action = ddlAction.SelectedValue;

            if (string.IsNullOrWhiteSpace(studentName) ||
                string.IsNullOrWhiteSpace(bookName))
            {
                lblMessage.Text = "Please fill all the fields.";
                return;
            }

            if (action != "Issue" && action != "Return")
            {
                lblMessage.Text = "Please select a valid action.";
                return;
            }

            string cs = ConfigurationManager
                .ConnectionStrings["MyDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        // Find the book and lock its row
                        string checkQuery = @"
                            SELECT Quantity
                            FROM dbo.Books WITH (UPDLOCK, HOLDLOCK)
                            WHERE BookName = @BookName";

                        int quantity;

                        using (SqlCommand checkCmd =
                            new SqlCommand(checkQuery, con, transaction))
                        {
                            checkCmd.Parameters.Add(
                                "@BookName", SqlDbType.VarChar, 150
                            ).Value = bookName;

                            object result = checkCmd.ExecuteScalar();

                            if (result == null || result == DBNull.Value)
                            {
                                transaction.Rollback();
                                lblMessage.Text = "Book not found.";
                                return;
                            }

                            quantity = Convert.ToInt32(result);
                        }

                        // Issue: decrease available quantity
                        if (action == "Issue")
                        {
                            if (quantity <= 0)
                            {
                                transaction.Rollback();
                                lblMessage.Text =
                                    "Book is not available.";
                                return;
                            }

                            string updateQuery = @"
                                UPDATE dbo.Books
                                SET Quantity = Quantity - 1
                                WHERE BookName = @BookName
                                  AND Quantity > 0";

                            using (SqlCommand updateCmd =
                                new SqlCommand(updateQuery, con, transaction))
                            {
                                updateCmd.Parameters.Add(
                                    "@BookName", SqlDbType.VarChar, 150
                                ).Value = bookName;

                                int rows = updateCmd.ExecuteNonQuery();

                                if (rows != 1)
                                {
                                    throw new Exception(
                                        "Could not update book quantity.");
                                }
                            }
                        }
                        else
                        {
                            // Return: increase available quantity
                            string updateQuery = @"
                                UPDATE dbo.Books
                                SET Quantity = Quantity + 1
                                WHERE BookName = @BookName";

                            using (SqlCommand updateCmd =
                                new SqlCommand(updateQuery, con, transaction))
                            {
                                updateCmd.Parameters.Add(
                                    "@BookName", SqlDbType.VarChar, 150
                                ).Value = bookName;

                                int rows = updateCmd.ExecuteNonQuery();

                                if (rows != 1)
                                {
                                    throw new Exception(
                                        "Could not update book quantity.");
                                }
                            }
                        }

                        // Save transaction history
                        string insertQuery = @"
                            INSERT INTO dbo.BookTransactions
                                (StudentName, BookName, ActionType)
                            VALUES
                                (@StudentName, @BookName, @ActionType)";

                        using (SqlCommand insertCmd =
                            new SqlCommand(insertQuery, con, transaction))
                        {
                            insertCmd.Parameters.Add(
                                "@StudentName", SqlDbType.VarChar, 100
                            ).Value = studentName;

                            insertCmd.Parameters.Add(
                                "@BookName", SqlDbType.VarChar, 150
                            ).Value = bookName;

                            insertCmd.Parameters.Add(
                                "@ActionType", SqlDbType.VarChar, 10
                            ).Value = action;

                            insertCmd.ExecuteNonQuery();
                        }

                        // Save both changes together
                        transaction.Commit();

                        lblMessage.Text =
                            "Transaction saved successfully!";
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch
                        {
                            // Rollback may fail if already completed.
                        }

                        lblMessage.Text =
                            "Database error: " + ex.Message;
                    }
                }
            }
        }
    }
}