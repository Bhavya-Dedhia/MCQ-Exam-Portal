﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

public partial class admin_addadmin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    //add admin
    protected void btn_addadmin_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string s = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
            using (SqlConnection con = new SqlConnection(s))
            {
                SqlCommand cmd = new SqlCommand("spAdminRegisterinsert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@admin_name", txt_adminame.Text);
                cmd.Parameters.AddWithValue("@admin_email", txt_admiemail.Text); // Correct parameter name
                cmd.Parameters.AddWithValue("@password", txt_adminpass.Text);

                try
                {
                    con.Open();
                    int value = (int)cmd.ExecuteScalar(); // Execute the stored procedure
                    if (value == 1)
                    {
                        Response.Redirect("~/admin/Index.aspx");  // Redirect to Admin Dashboard
                    }
                    else
                    {
                        txt_adminame.Focus();
                        panel_addamin_warning.Visible = true;
                        lbl_addaminwarning.Text = "Email is already in use";  // Show warning if email exists
                    }
                }
                catch (Exception ex)
                {
                    txt_admiemail.Focus();
                    panel_addamin_warning.Visible = true;
                    lbl_addaminwarning.Text = "Something went wrong! Contact your developer <br/>" + ex.Message;
                }
            }
        }
        else
        {
            txt_admiemail.Focus();
            panel_addamin_warning.Visible = true;
            lbl_addaminwarning.Text = "Please fill all the requirements";  // If page validation fails
        }
    }

}