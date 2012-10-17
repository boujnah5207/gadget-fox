﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace GadgetFox
{
    public partial class ViewInventory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gdvInventory.DataSource = getInventoryProducts();
                gdvInventory.DataBind();
            }
        }

        private DataSet getInventoryProducts()
        {
            String myConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
            SqlConnection myConnection = new SqlConnection(myConnectionString);
            DataSet ds = new DataSet();
            try
            {
                myConnection.Open();
                SqlCommand cmd = new SqlCommand("Select * from [GadgetFox].[dbo].[Products]", myConnection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (SqlException ex)
            {
                Response.Write("<SCRIPT LANGUAGE='JavaScript'>alert('" + ex.Message + "')</SCRIPT>");
            }
            finally
            {
                myConnection.Close();
            }
            return ds;
        }

        protected void gdvInventory_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataSet ds = getInventoryProducts();
            if (ViewState[e.SortExpression] == null)
                ViewState[e.SortExpression] = "DESC";

            String strSortDirection, prevDirect = ViewState[e.SortExpression].ToString();

            ViewState[e.SortExpression] = strSortDirection = (prevDirect == "ASC") ? "DESC" : "ASC";

            if (ds != null)
            {
                DataView dataView = new DataView(ds.Tables[0]);
                dataView.Sort = e.SortExpression + " " + strSortDirection;

                gdvInventory.DataSource = dataView;
                gdvInventory.DataBind();
            }
        }



    }
}