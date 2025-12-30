using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Telerik.Web.UI;
public partial class VisitorCount : System.Web.UI.Page
{
    public static DataTable dtTable;
    public static string connectionString;
    public SqlConnection SqlConnection = new SqlConnection();
    public SqlDataReader SqlDataReader;
    public SqlDataAdapter SqlDataAdapter = new SqlDataAdapter();
    public SqlCommand SqlCommand = new SqlCommand();

    protected void Page_Load(object sender, EventArgs e)
    {
        // 1) Session check
        if (Session["EmpCode"] == null && Session["EmpName"] == null)
        {
            Session["MainMsg"] = "Session Expired";
            Response.Redirect("~/Default.aspx");
            return;
        }

        // *********** CODE FOR MIGRATION *************
        string strCode = Request.QueryString["Code"];
        if (strCode != null)
        {
            if (MyUtilityCS.GetSessionDetails(strCode))
            {
                Session["EmpCode"] = GlobalClassCS.session_empcode;
                Session["EmpName"] = GlobalClassCS.session_empname;
                Session["EmpLocation"] = GlobalClassCS.session_emplocation;
                Session["MyConn"] = GlobalClassCS.DBConn;
            }
        }
        // ***** END ****** CODE FOR MIGRATION **********

        var sessionConn = Session["MyConn"] != null ? Session["MyConn"].ToString() : GlobalClassCS.DBConn;
        GlobalClassCS.DBConn = sessionConn;
        connectionString = GlobalClassCS.DBConn;
        SqlConnection.ConnectionString = GlobalClassCS.DBConn;

        if (!IsPostBack)
        {
            LoadVisitorCount();
        }
    }

    private void LoadVisitorCount()
    {
        string query = "SELECT COUNT(*) AS Visitorcount FROM TblVisitorPass_Request WHERE Entry_Flag = 'True'";

        using (SqlConnection conn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            conn.Open();
            int visitorCount = (int)cmd.ExecuteScalar();
            lblVisitorCount.Text = visitorCount.ToString();
            
        }
    }

    // Optional: hook this into a Timer or AJAX UpdatePanel
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        LoadVisitorCount();
        Response.Redirect("~/VisitorPass/VisitorCount.aspx");

    }

    protected void RadGrid1_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        return;
    }

    protected void RadGrid1_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        return;
    }

    private string sRadGrid1_DataSource;
    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        sRadGrid1_DataSource = "SELECT * FROM TblVisitorPass_Request WHERE Entry_Flag = 'True'";
        RadGrid1.DataSource = GetTable(sRadGrid1_DataSource);
    }


    public DataTable GetTable(string query)
    {
        SqlConnection sqlConn = new SqlConnection(connectionString);
        sqlConn.Open();
        DataTable myTable = new DataTable();
        SqlDataAdapter.SelectCommand = new SqlCommand(query, sqlConn);
        SqlDataAdapter.Fill(myTable);
        sqlConn.Close();
        return myTable;
    }
}
