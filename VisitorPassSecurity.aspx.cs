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
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Globalization;

public partial class VisitorPassSecurity : System.Web.UI.Page
{


    public static DataTable dtTable;
    public static string connectionString;
    public SqlConnection SqlConnection = new SqlConnection();
    public SqlDataReader SqlDataReader;
    public SqlDataAdapter SqlDataAdapter = new SqlDataAdapter();
    public SqlCommand SqlCommand = new SqlCommand();
    public SqlDataReader SqlDataRdr;
    public int iInsertSlNo = 0;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["EmpCode"] == null && Session["EmpName"] == null)
        {
            Session["MainMsg"] = "Session Expired";
            Response.Redirect("~/Default.aspx");
            return;
        }

        //*********** CODE FOR MIGRATION *************
        string strCode = Request.QueryString["Code"];
        if (strCode != null)
        {
            if (MyUtilityCS.GetSessionDetails(strCode) == true)
            {
                Session["EmpCode"] = GlobalClassCS.session_empcode;
                Session["EmpName"] = GlobalClassCS.session_empname;
                Session["EmpLocation"] = GlobalClassCS.session_emplocation;
                Session["myConn"] = GlobalClassCS.DBConn;
            }
        }
        //***** END ****** CODE FOR MIGRATION **********

        GlobalClassCS.DBConn = (string)Session["MyConn"];
        connectionString = GlobalClassCS.DBConn;
        SqlConnection.ConnectionString = connectionString;

        if (!IsPostBack)
        {
        }
    }

    private DataTable GetVisitorData()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("FormID");
        dt.Columns.Add("VisitorName");
        dt.Columns.Add("Company");
        dt.Columns.Add("PersonToVisit");
        dt.Columns.Add("VisitDate", typeof(DateTime));
        dt.Columns.Add("Validity");
        dt.Columns.Add("ConfirmLogged", typeof(bool));
        dt.Columns.Add("ConfirmExit", typeof(bool));

        // Sample row
        dt.Rows.Add("VP-2025-00123", "John Doe", "ABC Corp", "Mr. Smith", DateTime.Today, "2 hours", true, true);

        return dt;
    }
    protected void btnCallHost_Click(object sender, EventArgs e)
    {
        // Trigger call logic or log the action
    }


    protected void rgVisitorLog_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "CapturePhoto")
        {
            // your logic
        }
        else if (e.CommandName == "UploadPhoto")
        {
            // your logic
        }
        else if (e.CommandName == "PrintBadge")
        {
            // your logic
        }
        else if (e.CommandName == "Capture")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "openCamera", "openCamera();", true);
        }
        else if (e.CommandName == "OpenForm")
        {
            GridDataItem item = e.Item as GridDataItem;
            if (item != null)
            {
                string formId = e.CommandArgument.ToString();
                string visitorName = item["VisitorName"].Text.Replace("&nbsp;", "").Trim();
                string visitorCompany = item["VisitorCompany"].Text.Replace("&nbsp;", "").Trim();
                string purpose = item["Purpose_of_Visit"].Text.Replace("&nbsp;", "").Trim();

                visitorIdText.InnerText = formId;
                visitorNameSpan.InnerText = visitorName;
                visitorCompanySpan.InnerText = visitorCompany;
                visitorPurpose.InnerText = purpose;

                // Generate barcode with C# 5 compatible string.Format
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "generateBarcode",
                    string.Format("generateBarcode('{0}');", formId),
                    true
                );

            }
        }
    }




    private string sRadGrid1_DataSource;

    protected void rgVisitorLog_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        string query = @"SELECT * FROM TblVisitorPass_Request WHERE status = @status AND Visit_ToTime >= @visitDate ORDER BY FormID DESC";

        rgVisitorLog.DataSource = GetTable(query, DateTime.Now.Date);
    }

    public DataTable GetTable(string query, DateTime visitDate)
    {
        using (SqlConnection sqlConn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(query, sqlConn))
        {
            cmd.Parameters.AddWithValue("@status", "A");
            cmd.Parameters.AddWithValue("@visitDate", visitDate);

            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataTable myTable = new DataTable();
                adapter.Fill(myTable);
                return myTable;
            }
        }
    }

    protected void rgVisitorLog_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = (GridDataItem)e.Item;
            DataRowView row = (DataRowView)e.Item.DataItem;
            bool Value = Convert.ToBoolean(row["Entry_Flag"]);
            CheckBox chkbox = (CheckBox)dataItem.FindControl("ConfirmLogged");
            chkbox.Checked = Value;
        }

        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = (GridDataItem)e.Item;
            LinkButton btn = dataItem.FindControl("lnkPrintBadge") as LinkButton;
            if (btn != null)
            {
                btn.ForeColor = System.Drawing.Color.Blue;
            }
        }
    }

    protected void ConfirmLogged_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridDataItem item = (GridDataItem)chk.NamingContainer;
        string formId = item.GetDataKeyValue("FormID").ToString();

        if (chk.Checked)
        {
            // Debug check
            System.Diagnostics.Debug.WriteLine("ConfirmLogged_CheckedChanged fired for FormID: " + formId);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE TblVisitorPass_Request SET Entry_Flag = 'True' WHERE FormID = @FormID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FormID", formId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


    }

    protected void ConfirmExit_CheckedChanged(object sender, EventArgs e)
    {

    }

}