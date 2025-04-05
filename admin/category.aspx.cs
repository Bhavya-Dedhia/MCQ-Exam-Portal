using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
public partial class admin_category : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            panel_categorylist.Visible = true;
            panel_addcategory.Visible = false;
            btn_panelcategorylist.BackColor = ColorTranslator.FromHtml("#343A40");
            btn_paneladdcategory.BackColor = ColorTranslator.FromHtml("#DC3545");
            categorylistmethod();
        }
    }
    //This is button for the enable list of category panel 
    protected void btn_panelcategorylist_Click(object sender, EventArgs e)
    {
        panel_categorylist.Visible = true;
        panel_addcategory.Visible = false;
        btn_panelcategorylist.BackColor = ColorTranslator.FromHtml("#343A40");
        btn_paneladdcategory.BackColor = ColorTranslator.FromHtml("#DC3545");
        categorylistmethod();

    }
    //This is button for enable the adding in category panel
    protected void btn_paneladdcategory_Click(object sender, EventArgs e)
    {
        txt_category.Focus();
        panel_categorylist.Visible = false;
        panel_addcategory.Visible = true;
        btn_panelcategorylist.BackColor = ColorTranslator.FromHtml("#DC3545");
        btn_paneladdcategory.BackColor = ColorTranslator.FromHtml("#343A40");
    }

    protected void btn_addcategory_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            using (SqlConnection con = new SqlConnection(s))
            {
                // SQL query to insert only the category name, assuming category_id is an identity field
                SqlCommand cmd = new SqlCommand("INSERT INTO category (category_name) VALUES (@category_name)", con);

                // Add the category_name parameter
                cmd.Parameters.AddWithValue("@category_name", txt_category.Text);

                try
                {
                    // Open the database connection
                    con.Open();

                    // Execute the query and check how many rows were affected
                    int i = cmd.ExecuteNonQuery();

                    if (i > 0)
                    {
                        // If the category was successfully added, clear the input field
                        txt_category.Text = string.Empty;
                        // Redirect the user to the category page
                        Response.Redirect("~/admin/category.aspx");
                        Response.Write("Added Successfully");
                    }
                    else
                    {
                        // If something went wrong, show an error message
                        txt_category.Focus();
                        panel_addcategory_warning.Visible = true;
                        lbl_categoryaddwarning.Text = "Something went wrong.";
                    }
                }
                catch (Exception ex)
                {
                    // Catch any exceptions and display a more detailed error message
                    txt_category.Focus();
                    panel_addcategory_warning.Visible = true;
                    lbl_categoryaddwarning.Text = "Something went wrong. Please try again later.<br/>Contact your developer for this issue: " + ex.Message;
                }
            }
        }
        else
        {
            // If validation fails (e.g., input is missing), show an error message
            txt_category.Focus();
            panel_addcategory_warning.Visible = true;
            lbl_categoryaddwarning.Text = "You must fill all the required fields.";
        }
    }

    // For row command argument
    protected void grdview_categorylist_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "delete_category")
        {
            // Ensure proper conversion and handling of the category ID
            int categoryId = Convert.ToInt32(e.CommandArgument);
            deletecategory(categoryId);  // Call delete method
            categorylistmethod();        // Rebind the GridView
        }
    }

    // From page index changing
    protected void grdview_categorylist_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdview_categorylist.PageIndex = e.NewPageIndex;  // Set the new page index
        categorylistmethod();  // Rebind the GridView to refresh data
    }

    string s = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
    // Method for retrieving category into list item 
    public void categorylistmethod()
    {
        using (SqlConnection con = new SqlConnection(s))
        {
            // SQL command to fetch all categories
            SqlCommand cmd = new SqlCommand("SELECT * FROM category", con);
            try
            {
                con.Open();  // Open the connection
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))  // Adapter to fill data
                {
                    DataTable dt = new DataTable();
                    sda.Fill(dt);  // Fill the DataTable with data
                    grdview_categorylist.DataSource = dt;  // Set GridView data source
                    grdview_categorylist.DataBind();  // Bind data to GridView
                }
            }
            catch (Exception ex)
            {
                // Display an error message if something goes wrong
                panel_categorylist_warning.Visible = true;
                lbl_categorylistwarning.Text = "Something went wrong. Please try again later.<br/>Contact your developer for this issue: " + ex.Message;
            }
        }
    }


    // Method for deleting category
    public void deletecategory(int id)
    {
        using (SqlConnection con = new SqlConnection(s))
        {
            // SQL command to delete the category by ID
            SqlCommand cmd = new SqlCommand("DELETE FROM category WHERE category_id = @categoryid", con);
            cmd.Parameters.AddWithValue("@categoryid", id);  // Add category ID parameter

            try
            {
                con.Open();  // Open the connection
                int rowsAffected = cmd.ExecuteNonQuery();  // Execute the delete query

                if (rowsAffected > 0)
                {
                    // Successfully deleted, rebind GridView
                    categorylistmethod();
                    lbl_categorylistwarning.Text = "Category deleted successfully.";  // Success message
                    panel_categorylist_warning.Visible = true;
                }
                else
                {
                    // If no rows were affected, show an error message
                    panel_categorylist_warning.Visible = true;
                    lbl_categorylistwarning.Text = "Something went wrong. The category could not be deleted.";
                }
            }
            catch (Exception ex)
            {
                // Handle any exception that occurs during deletion
                panel_categorylist_warning.Visible = true;
                lbl_categorylistwarning.Text = "Something went wrong. Please try again later.<br/>Contact your developer for this issue: " + ex.Message;
            }
        }
    }




}