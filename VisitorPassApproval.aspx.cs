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

public partial class VisitorPassApproval : System.Web.UI.Page
{
    public static DataTable dtTable;
    public static string connectionString;
    public SqlConnection SqlConnection = new SqlConnection();
    public SqlDataReader SqlDataReader;
    public SqlDataAdapter SqlDataAdapter = new SqlDataAdapter();
    public SqlCommand SqlCommand = new SqlCommand();
    public SqlDataReader SqlDataRdr;
    public int iInsertSlNo = 0;
    int countApproved;
    int countDisapproved;
    int countPending;
    string teamCode;
    public string sRadGrid2_DataSource;


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

        // ✅ Initialize controls only on first load
        if (!IsPostBack)
        {
            rbStatusList.SelectedValue = "N";
            RadWindowManager2.VisibleOnPageLoad = false;

            TreeViewBind();
            //acbFormID.Entries.Clear(); // Clear any existing selection
            //acbFormID.Entries.Add(new Telerik.Web.UI.AutoCompleteBoxEntry(formId, formId));
            RadWindow2.VisibleOnPageLoad = false;
        }

        if (IsPostBack)
        {
            RadWindowManager2.VisibleOnPageLoad = false;
            RadWindow2.VisibleOnPageLoad = false;
            TreeViewBind();   
        }
    }
    public class VisitorPassService
    {
        private readonly string connectionString;

        public VisitorPassService()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyDb"].ConnectionString;
        }

        // ✅ This method is at class scope, not inside another method
        private List<RequestItem> GetRequestsByStatus(string status, string teamcode)
        {
            var list = new List<RequestItem>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "SELECT FormID FROM TblVisitorPass_Request WHERE status = @status and Teamcode = @Teamcode and Visit_ToTime >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'", conn))
            {
                // Avoid AddWithValue if you know the actual type; use Add with SqlDbType for better performance/accuracy
                cmd.Parameters.Add("@status", System.Data.SqlDbType.NVarChar, 50).Value = status ?? (object)DBNull.Value;
                cmd.Parameters.Add("@status", System.Data.SqlDbType.NVarChar, 50).Value = status;
                //string teamcode = tc;
                cmd.Parameters.Add("@teamcode", SqlDbType.NVarChar, 50).Value = teamcode ?? (object)DBNull.Value;
                cmd.Parameters.Add("@teamcode", System.Data.SqlDbType.NVarChar, 50).Value = teamcode;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        list.Add(new RequestItem
                        {
                            Id = reader["FormID"] == DBNull.Value ? null : reader["FormID"].ToString()
                        });
                    }

                }
            }

            return list;
        }
    }

    public class RequestItem
    {
        public string Id { get; set; }
    }


    protected void rtvModules_NodeDataBound(object sender, RadTreeNodeEventArgs e)
    {
        var statusObj = DataBinder.Eval(e.Node.DataItem, "Status");
        var status = statusObj != null ? statusObj.ToString() : string.Empty;

        var rb = e.Node.FindControl("rbSelect") as RadioButton;

        if (rb != null)
        {
            string displayStatus;
            switch (status)
            {
                case "N":
                    displayStatus = "New";
                    break;
                case "A":
                    displayStatus = "Active";
                    break;
                case "D":
                    displayStatus = "Disabled";
                    break;
                case "T":
                    displayStatus = "Terminated";
                    break;
                default:
                    displayStatus = "Unknown";
                    break;
            }

            rb.Text = string.Format("{0} ({1})", e.Node.Text, displayStatus);

            // Disable radio if status is Disabled or Terminated
            if (status == "D" || status == "T")
                rb.Enabled = false;
        }
    }

    protected void rbSelect_CheckedChanged(object sender, EventArgs e)
    {
        var rb = (RadioButton)sender;
        RadTreeNode selectedNode = null;
        foreach (RadTreeNode n in rtvModules.GetAllNodes())
        {
            var radio = n.FindControl("rbSelect") as RadioButton;
            if (radio == rb) { selectedNode = n; break; }
        }
        if (selectedNode != null)
        {
            //lblSelected.Text = "Selected: " + selectedNode.Value;
        }
    }

    public class Node
    {
        public string Id { get; set; }
        public string ParentId { get; set; } // null or empty for root
        public string Text { get; set; }
        public string Status { get; set; } // N, A, D, T

    }

    private void ApplySandalColor(System.Web.UI.Control parent)
    {
        foreach (System.Web.UI.Control ctrl in parent.Controls)
        {
            if (ctrl is Telerik.Web.UI.RadTextBox)
            {
                Telerik.Web.UI.RadTextBox radTextBox = (Telerik.Web.UI.RadTextBox)ctrl;
                radTextBox.BackColor = System.Drawing.ColorTranslator.FromHtml("#E6E6FA");
            }

            // Recursively apply to child controls
            if (ctrl.HasControls())
            {
                ApplySandalColor(ctrl);
            }
        }
    }


    private void ApplyLavenderColor(System.Web.UI.Control parent)
    {
        foreach (System.Web.UI.Control ctrl in parent.Controls)
        {
            if (ctrl is Telerik.Web.UI.RadTextBox)
            {
                ((Telerik.Web.UI.RadTextBox)ctrl).BackColor = System.Drawing.ColorTranslator.FromHtml("#E6E6FA");
            }
            else if (ctrl is Telerik.Web.UI.RadCheckBox)
            {
                ((Telerik.Web.UI.RadCheckBox)ctrl).BackColor = System.Drawing.ColorTranslator.FromHtml("#E6E6FA");
            }
            else if (ctrl is Telerik.Web.UI.RadDatePicker)
            {
                ((Telerik.Web.UI.RadDatePicker)ctrl).DateInput.BackColor = System.Drawing.ColorTranslator.FromHtml("#E6E6FA");
            }
            else if (ctrl is Telerik.Web.UI.RadComboBox)
            {
                ((Telerik.Web.UI.RadComboBox)ctrl).BackColor = System.Drawing.ColorTranslator.FromHtml("#E6E6FA");
            }
            if (ctrl.HasControls())
            {
                ApplyLavenderColor(ctrl);
            }
        }
    }

    protected void signature_TextChanged(object sender, EventArgs e)
    {
        if ((approved.Checked != true) && (rejected.Checked != true))
        {
            RadWindowManager1.RadAlert("Please select Approved or Rejected.", 300, 200, "Validation", null);
            return;
        }

        LblEmpty.Text = "";
        RadWindow1.VisibleOnPageLoad = true;
    }

    protected void BtnValidatePin_Click(object sender, EventArgs e)
    {
        if (approved.Checked == true)
        {
            LblEmpty.Text = string.Empty;
            SqlConnection.Close();
            SqlConnection.Open();
            SqlCommand.CommandText = "select * from VisitorPass_Approval_Config WHERE EmpCode='" + signature.Text + "'";
            SqlCommand.Connection = SqlConnection;
            SqlDataRdr = SqlCommand.ExecuteReader();
            if (!SqlDataRdr.HasRows)
            {
                RadWindow1.VisibleOnPageLoad = true;
                LblEmpty.Text = "InValid Employee Id";
                return;
            }
            if (signature.Text == "")
            {
                RadWindow1.VisibleOnPageLoad = true;
                LblEmpty.Text = "InValid Employee Id";
                return;
            }
            if (signature.Text == "")
            {
                RadWindow1.VisibleOnPageLoad = true;
                LblEmpty.Text = "InValid valid Id";
                return;
            }

            var byEnPinNo = new byte[0];
            SqlConnection.Close();
            SqlConnection.Open();
            SqlCommand.CommandText = "SELECT CAST(HASHBYTES('MD5','" + TxtPassword.Text + "') AS BINARY(64)) As PinNo";
            SqlCommand.Connection = SqlConnection;
            SqlDataReader dr = SqlCommand.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    byEnPinNo = (byte[])dr["PinNo"];
                }
            }
            else
            {
                LblEmpty.Text = "Enter A Valid PIN No";
                RadWindow1.VisibleOnPageLoad = true;
                return;
            }
            SqlConnection.Close();
            SqlConnection.Open();
            SqlCommand.CommandText = "SELECT Encrypt_PinNo,Location FROM Qry_HTML_Login_Auth Where Empcode='" + signature.Text + "'";
            SqlCommand.Connection = SqlConnection;
            SqlDataReader dr1 = SqlCommand.ExecuteReader();
            bool bPinEq = false;

            if (dr1.HasRows)
            {
                while (dr1.Read())
                {
                    var byDbPinNo = (byte[])dr1["Encrypt_PinNo"];

                    if (byDbPinNo.SequenceEqual(byEnPinNo) == true)
                    {
                        bPinEq = true;
                    }
                    else
                    {
                        bPinEq = false;
                    }
                }
            }
            if (bPinEq == false)
            {
                LblEmpty.Text = "Enter A Valid PIN No";
                RadWindow1.VisibleOnPageLoad = true;
            }
            if (bPinEq == true)
            {
                SqlConnection.Close();
                SqlConnection.Open();
                SqlCommand.CommandText = "select * From EmpPersonnel_Master where EmpCode='" + signature.Text + "'";
                SqlCommand.Connection = SqlConnection;
                SqlDataReader drsss = SqlCommand.ExecuteReader();
                try
                {
                    if (drsss.HasRows)
                    {
                        drsss.Read();
                        signature.Text = drsss["EmpName"].ToString();
                        var TestConductBy = drsss["EmpName"].ToString();
                        SqlConnection.Close();
                        SqlConnection.Open();
                        string UpdateQuery = "Update  TblVisitorPass_Request set Approved_by = '" + TestConductBy + "', Status = 'A', Approver_Cmts = '" + txtAdminNotes.Text + "' where FormID='" + RadAutoCompleteBox1.Text + "'";
                        SqlCommand.CommandText = UpdateQuery;
                        SqlCommand.Connection = SqlConnection;
                        SqlCommand.ExecuteNonQuery();
                        signature.Enabled = false;
                        RadGrid1.Rebind();
                        DisableFormControls();
                        txtAdminNotes.Enabled = false;
                        Funclear();
                    }
                }
                catch (Exception ex) { }
                RadWindow1.VisibleOnPageLoad = false;
                RadWindow2.VisibleOnPageLoad = false;
            }
        }

        if (rejected.Checked == true)
        {
            LblEmpty.Text = string.Empty;
            SqlConnection.Close();
            SqlConnection.Open();
            SqlCommand.CommandText = "select * from VisitorPass_Approval_Config WHERE EmpCode='" + signature.Text + "'";
            SqlCommand.Connection = SqlConnection;
            SqlDataRdr = SqlCommand.ExecuteReader();
            if (!SqlDataRdr.HasRows)
            {
                RadWindow1.VisibleOnPageLoad = true;
                LblEmpty.Text = "InValid Employee Id";
                return;
            }
            if (signature.Text == "")
            {
                RadWindow1.VisibleOnPageLoad = true;
                LblEmpty.Text = "InValid Employee Id";
                return;
            }
            if (signature.Text == "")
            {
                RadWindow1.VisibleOnPageLoad = true;
                LblEmpty.Text = "InValid valid Id";
                return;
            }

            var byEnPinNo = new byte[0];
            SqlConnection.Close();
            SqlConnection.Open();
            SqlCommand.CommandText = "SELECT CAST(HASHBYTES('MD5','" + TxtPassword.Text + "') AS BINARY(64)) As PinNo";
            SqlCommand.Connection = SqlConnection;
            SqlDataReader dr = SqlCommand.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    byEnPinNo = (byte[])dr["PinNo"];
                }
            }
            else
            {
                LblEmpty.Text = "Enter A Valid PIN No";
                RadWindow1.VisibleOnPageLoad = true;
                return;
            }
            SqlConnection.Close();
            SqlConnection.Open();
            SqlCommand.CommandText = "SELECT Encrypt_PinNo,Location FROM Qry_HTML_Login_Auth Where Empcode='" + signature.Text + "'";
            SqlCommand.Connection = SqlConnection;
            SqlDataReader dr1 = SqlCommand.ExecuteReader();
            bool bPinEq = false;

            if (dr1.HasRows)
            {
                while (dr1.Read())
                {
                    var byDbPinNo = (byte[])dr1["Encrypt_PinNo"];

                    if (byDbPinNo.SequenceEqual(byEnPinNo) == true)
                    {
                        bPinEq = true;
                    }
                    else
                    {
                        bPinEq = false;
                    }
                }
            }
            if (bPinEq == false)
            {
                LblEmpty.Text = "Enter A Valid PIN No";
                RadWindow1.VisibleOnPageLoad = true;
            }
            if (bPinEq == true)
            {
                SqlConnection.Close();
                SqlConnection.Open();
                SqlCommand.CommandText = "select * From EmpPersonnel_Master where EmpCode='" + signature.Text + "'";
                SqlCommand.Connection = SqlConnection;
                SqlDataReader drsss = SqlCommand.ExecuteReader();
                try
                {
                    if (drsss.HasRows)
                    {
                        drsss.Read();
                        signature.Text = drsss["EmpName"].ToString();
                        var TestConductBy = drsss["EmpName"].ToString();
                        SqlConnection.Close();
                        SqlConnection.Open();
                        string UpdateQuery = "Update  TblVisitorPass_Request set Approved_by = '" + TestConductBy + "' , Status = 'D' where FormID='" + RadAutoCompleteBox1.Text + "'";
                        SqlCommand.CommandText = UpdateQuery;
                        SqlCommand.Connection = SqlConnection;
                        SqlCommand.ExecuteNonQuery();
                        signature.Enabled = false;
                        RadGrid1.Rebind();
                        DisableFormControls();
                        Funclear();
                    }
                }
                catch (Exception ex) { }
                RadWindow1.VisibleOnPageLoad = false;
            }
        }
    }

    protected void acbSCD_TextChanged(object sender, EventArgs e)
    {
        var autoCompleteBox = sender as Telerik.Web.UI.RadAutoCompleteBox;
        if (autoCompleteBox != null && autoCompleteBox.Entries.Count > 0)
        {
            string selectedFormId = autoCompleteBox.Entries[0].Value;
            BindFormDetails(selectedFormId);
        }
    }

    private void BindFormDetails(string formId)
    {
        // Get connection string from Web.config
        //string connStr = ConfigurationManager.ConnectionStrings["YourConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"Select Entered_DateTime, Purpose_of_Visit, Person_to_Meet,
                                Breakfast, Lunch, Dinner,
                                CabRequired, Destination
                         FROM TblVisitorPass_Request
                         WHERE FormID = @FormID";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FormID", RadAutoCompleteBox1.Text);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                // Bind TextBoxes

                txtRequestDate.Text = reader["Entered_DateTime"] is DBNull
                    ? string.Empty
                    : Convert.ToDateTime(reader["Entered_DateTime"]).ToString("dd/MM/yyyy");

                tpRequestTime.Text = reader["Purpose_of_Visit"] is DBNull
                    ? string.Empty
                    : reader["Purpose_of_Visit"].ToString();

                txtRequestedBy.Text = reader["Person_to_Meet"] is DBNull
                    ? string.Empty
                    : reader["Person_to_Meet"].ToString();

                // Bind CheckBoxes
                chkBreakfast.Checked = reader["Breakfast"] is DBNull ? false : Convert.ToBoolean(reader["Breakfast"]);
                chkLunch.Checked = reader["Lunch"] is DBNull ? false : Convert.ToBoolean(reader["Lunch"]);
                chkDinner.Checked = reader["Dinner"] is DBNull ? false : Convert.ToBoolean(reader["Dinner"]);

                bool cabRequired = reader["CabRequired"] is DBNull ? false : Convert.ToBoolean(reader["CabRequired"]);
                RadCheckBox1.Checked = cabRequired;
                RadCheckBox2.Checked = !cabRequired;
                RadTextBox6.Text = reader["Destination"] is DBNull
                ? string.Empty
                : reader["Destination"].ToString();
            }
            reader.Close();


            // Now lock the controls
            SetVisitorControlsReadOnlyConsistent();

        }
    }

    private void SetVisitorControlsReadOnlyConsistent()
    {
        // Text fields: keep enabled and set ReadOnly
        txtRequestDate.ReadOnly = true;
        txtRequestDate.Enabled = true;

        txtRequestedBy.ReadOnly = true;
        txtRequestedBy.Enabled = true;

        RadTextBox6.ReadOnly = true;
        RadTextBox6.Enabled = true;

        tpRequestTime.Enabled = false;

        chkBreakfast.Enabled = true;
        chkBreakfast.CssClass += " readonly-checkbox";

        chkLunch.Enabled = true;
        chkLunch.CssClass += " readonly-checkbox";

        chkDinner.Enabled = true;
        chkDinner.CssClass += " readonly-checkbox";

        RadCheckBox1.Enabled = true;
        RadCheckBox1.CssClass += " readonly-checkbox";

        RadCheckBox2.Enabled = true;
        RadCheckBox2.CssClass += " readonly-checkbox";

        // Add CSS class for text fields too
        txtRequestDate.CssClass += " readonly-field";
        txtRequestedBy.CssClass += " readonly-field";
        RadTextBox6.CssClass += " readonly-field";
    }

    protected void acbPONo_ShowAll_DataSourceSelect(object sender, AutoCompleteBoxDataSourceSelectEventArgs e)
    {

        dtTable = new DataTable();
        string sqlSelectCommand = "select Distinct FormID from TblVisitorPass_Request where status='N' and Visit_ToTime >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";

        SqlDataAdapter adapter = new SqlDataAdapter(sqlSelectCommand, connectionString);
        adapter.Fill(dtTable);
        var autocompleteBox = sender as RadAutoCompleteBox;
        if (e.FilterString == "ShowAllRecords")
        {
            //DataTable dtPONo = GetDataTable("Select Distinct Cust_PO from Production_Planning where UR_Order_Status='W' Order By Cust_PO");

            DataTable dtPONo = dtTable;
            autocompleteBox.DataSource = dtPONo;
        }
        else
        {
            //DataTable dtPONo = GetDataTable("Select Distinct Cust_PO from Production_Planning where UR_Order_Status='W' and Cust_PO like '" + e.FilterString.ToLower() + "%' Order By Cust_PO");
            DataTable dtPONo = new DataTable();
            var table = dtTable;
            var rows = table.AsEnumerable().Where(x => x.Field<string>("FormID").StartsWith(e.FilterString.ToUpper()));
            if (rows.Any())
            {
                dtPONo = rows.CopyToDataTable();
            }
            autocompleteBox.DataSource = dtPONo;
            //List<DataRow> lstPONo = dtPONo.AsEnumerable().Where(x => x["Cust_PO"].ToString().ToLower().StartsWith(e.FilterString.ToLower())).ToList();
            //if (lstPONo.Count() > 0)
            //{
            //    var query = from i in lstPONo
            //                select i;
            //    DataTable table = query.CopyToDataTable();
            //    autocompleteBox.DataSource = table;
            //}
            //else
            //{
            //    autocompleteBox.DataSource = "";
            //}
        }
    }

    //protected void OnLocationSelection(object sender, AutoCompleteTextEventArgs e)
    //{
    //    acbSCD.Enabled = false;
    // var   sRadGrid2_DataSource = "select *  from Qry_openreceipt_Balance_kanban where  LocationCode='" + FromLocation.Text + "'  and REC_PartCode_Trans='" + PartCode1.Text + "' and RecBalance!=0 and ReceivedQty!=0";
    //    acbSCD.DataSource = GetDataTable(sRadGrid2_DataSource);
    //    LocationParts.Rebind();   
    //}





    private string sRadGrid1_DataSource;

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        string teamcode = (string)Session["Teamcode"];
        sRadGrid1_DataSource = "SELECT * FROM TblVisitorPass_Request WHERE status = 'N' and teamcode ='" + Session["Teamcode"] + "' and Visit_ToTime >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' order by FormID desc";
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

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        LinkButton lnkFormId = e.Item.FindControl("lnkFormId") as LinkButton;
        ViewState["FormID"] = lnkFormId.Text;

        if (e.CommandName == "OpenForm")
        {
            SqlConnection.Close();
            SqlConnection.Open();
            SqlCommand.CommandText = "SELECT FormID, VisitorName, VisitorCompany, Person_to_Meet, AccessLevel,Teamcode, Expected_Duration, Details_of_accessories, Legal_ID_proof, Restricted_Area, No_of_Visitors, Destination, AccessLevel, Purpose_of_Visit, Type_for_Visit,Status, Visit_FromTime,Visit_ToTime, Entered_DateTime, Breakfast, Lunch, Dinner, CabRequired, CabNotRequired FROM TblVisitorPass_Request where formid = '" + ViewState["FormID"] + "' and Visit_ToTime >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            SqlCommand.Connection = SqlConnection;
            SqlDataReader dr = SqlCommand.ExecuteReader();
            if (dr.HasRows && dr.Read())

            {

                RadAutoCompleteBox1.Entries.Clear();
                RadAutoCompleteBox1.Entries.Add(new AutoCompleteBoxEntry(dr["FormID"].ToString()));

                txtFullName.Text = dr["VisitorName"].ToString();
                txtCompany.Text = dr["VisitorCompany"].ToString();
                txtRequestedBy.Text = dr["Person_to_Meet"].ToString();
                numVisitors.Text = dr["No_of_Visitors"].ToString();
                RadTextBox6.Text = dr["Destination"].ToString();
                tpRequestTime.Text = dr["Purpose_of_Visit"].ToString();
                txtRequestDate.Text = dr["Entered_DateTime"].ToString();
                visitdatefrom.Text = dr["Visit_FromTime"].ToString();
                visitdateto.Text = dr["Visit_ToTime"].ToString();

                chkBreakfast.Checked = SafeBool(dr["Breakfast"]);
                chkLunch.Checked = SafeBool(dr["Lunch"]);
                chkDinner.Checked = SafeBool(dr["Dinner"]);
                RadCheckBox1.Checked = SafeBool(dr["CabRequired"]);
                RadCheckBox2.Checked = SafeBool(dr["CabNotRequired"]);

                string status = dr["Status"].ToString();
                if (status == "A" || status == "D")
                {
                    DisableFormControls();
                    RadGrid1.Enabled = false;
                }
            }

            SqlConnection.Close();
            SqlConnection.Open();
            string formid = RadAutoCompleteBox1.Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_VisitorPass_member_temp WHERE FormID LIKE @FormID", conn))
            {
                // Add parameter with wildcard for LIKE
                cmd.Parameters.AddWithValue("@FormID", "%" + formid + "%");

                conn.Open(); // open connection here

                using (SqlDataReader drr = cmd.ExecuteReader())
                {
                    if (drr.HasRows && drr.Read())
                    {
                        RadWindow2.VisibleOnPageLoad = true;

                        string formID = RadAutoCompleteBox1.Text;
                        //string query = "SELECT FormID,VisitorName,slno FROM tbl_VisitorPass_member_temp WHERE FormID LIKE '" + formID + "%'";
                        //sRadGrid1_DataSource = query;
                        //RadGrid2.DataSource = GetTable(sRadGrid1_DataSource);

                        RadGrid2.Rebind();

                    }
                }
            }

        }
    }

    protected void RadGrid2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (RadWindow2.VisibleOnPageLoad == true)
        {

            string formID = RadAutoCompleteBox1.Text;
            sRadGrid2_DataSource = "SELECT FormID,VisitorName,slno FROM tbl_VisitorPass_member_temp WHERE FormID LIKE '" + formID + "%'";
            RadGrid2.DataSource = GetTable(sRadGrid2_DataSource);

        }
    }


    private void DisableFormControls()
    {

        RadAutoCompleteBox1.Enabled = false;
        txtFullName.Enabled = false;
        txtCompany.Enabled = false;
        txtRequestedBy.Enabled = false;
        numVisitors.Enabled = false;

        RadTextBox6.Enabled = false;


        tpRequestTime.Enabled = false; // HtmlSelect


        visitdatefrom.Enabled = false; // RadDatePicker
        visitdateto.Enabled = false;

        chkBreakfast.Enabled = false;
        chkLunch.Enabled = false;
        chkDinner.Enabled = false;
        RadCheckBox1.Enabled = false;
        RadCheckBox2.Enabled = false;
        

        foreach (BaseValidator v in Page.Validators)
        {
            v.Enabled = false;
        }
    }

    private void Funclear()
    {
        txtFullName.Text = string.Empty;
        txtCompany.Text = string.Empty;
        tpRequestTime.Text = string.Empty;

        txtRequestedBy.Text = string.Empty;
        RadTextBox6.Text = string.Empty;
        RadTextBox6.Text = string.Empty;
        visitdateto.Text = string.Empty;
        visitdatefrom.Text = string.Empty;
        txtRequestDate.Text = string.Empty;
        // Labels


        // Numbers (Telerik RadNumericTextBox)
        numVisitors.Text = string.Empty;

        // Checkboxes (normalize bool? to false)
        chkBreakfast.Checked = false;
        chkLunch.Checked = false;
        chkDinner.Checked = false;

        RadCheckBox1.Checked = false;
        RadCheckBox2.Checked = false;
    }


    private bool SafeBool(object value)
    {
        if (value == null || value == DBNull.Value)
            return false;

        bool result;
        if (bool.TryParse(value.ToString(), out result))
            return result;

        // If value is numeric (e.g., 1 or 0)
        int intValue;
        if (int.TryParse(value.ToString(), out intValue))
            return intValue != 0;

        return false;
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void rbStatusList_SelectedIndexChanged(object sender, EventArgs e)
    {

        string status = rbStatusList.SelectedValue;
        string Teamcode = (string)Session["Teamcode"];
        BindGrid(status, Teamcode);
    }

    private void BindGrid(string status, string Teamcode)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand("SELECT * FROM TblVisitorPass_Request WHERE Status = '" + status + "' and Teamcode = '" + Teamcode + "' and Visit_ToTime >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'", conn))
        {
            cmd.Parameters.Add("@Status", SqlDbType.VarChar, 1).Value = status; // Use VarChar if column is VARCHAR

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                RadGrid1.DataSource = dt;
                RadGrid1.DataBind();
            }
        }
    }


    public void TreeViewBind()
    {
        string empCode = Session["EmpCode"] != null ? Session["EmpCode"].ToString() : null;
        if (!string.IsNullOrEmpty(empCode))
        {
            empCode = Session["EmpCode"] != null ? Session["EmpCode"].ToString() : null;
            if (!string.IsNullOrEmpty(empCode))
            {
                const string query = "SELECT TOP 1 Teamcode FROM Qry_EmpMasterUnion WHERE Empcode = @Empcode";
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(query, conn))
                {
                    var p = cmd.Parameters.Add("@Empcode", SqlDbType.VarChar, 50); // adjust to your actual schema
                    p.Value = empCode;

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    var teamCode = (result != null && result != DBNull.Value) ? result.ToString() : null;

                    // Persist for use anywhere
                    Session["TeamCode"] = teamCode;
                }
            }
        }

        string teamcode = (string)Session["Teamcode"];
        SqlConnection.Close();
        SqlConnection.Open();
        SqlCommand.CommandText = "select Count(Status) as  count1 from TblVisitorPass_Request where status='N'  and Teamcode = '" + teamcode + "' and Visit_ToTime >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
        SqlCommand.Connection = SqlConnection;
        SqlDataRdr = SqlCommand.ExecuteReader();
        if (SqlDataRdr.HasRows)
        {
            SqlDataRdr.Read();
            countPending = int.Parse(SqlDataRdr["count1"].ToString());
        }

        SqlConnection.Close();
        SqlConnection.Open();
        SqlCommand.CommandText = "select Count(Status) as  count1 from TblVisitorPass_Request where status='A'  and Teamcode = '" + teamcode + "' and Visit_ToTime >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
        SqlCommand.Connection = SqlConnection;
        SqlDataRdr = SqlCommand.ExecuteReader();
        if (SqlDataRdr.HasRows)
        {
            SqlDataRdr.Read();
            countApproved = int.Parse(SqlDataRdr["count1"].ToString());
        }

        SqlConnection.Close();
        SqlConnection.Open();
        SqlCommand.CommandText = "select Count(Status) as  count1 from TblVisitorPass_Request where status='D'  and Teamcode = '" + teamcode + "' and Visit_ToTime >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
        SqlCommand.Connection = SqlConnection;
        SqlDataRdr = SqlCommand.ExecuteReader();
        if (SqlDataRdr.HasRows)
        {
            SqlDataRdr.Read();
            countDisapproved = int.Parse(SqlDataRdr["count1"].ToString());
        }

        var data = new List<Node>
        {
            new Node { Id = "frontend", ParentId = null, Text = "Yet to Approve - "+countPending+" " },
            new Node { Id = "react", ParentId = "frontend", Text = "Approved - "+countApproved+" " },
            new Node { Id = "vue", ParentId = "frontend", Text = "Dis-Approved - "+countDisapproved+"" },
        };

        rtvModules.DataSource = data;
        rtvModules.DataBind();

        // Expand some nodes initially
        var frontend = rtvModules.FindNodeByValue("frontend");
        if (frontend != null) frontend.Expanded = true;

        // Preselect
        var react = rtvModules.FindNodeByValue("react");
        if (react != null)
        {
            var rb = react.FindControl("rbSelect") as RadioButton;
            if (rb != null) rb.Checked = true;
        }

        ApplySandalColor(this.Page);
        ApplyLavenderColor(this.Page);


        SqlDataSourcePONo.ConnectionString = connectionString;
        SqlDataSourcePONo.SelectCommand = "select Distinct FormID from TblVisitorPass_Request where status='N' and Teamcode = '" + teamcode + "' and Visit_ToTime >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
    }

    public DataTable GetTable(string query, SqlParameter[] parameters)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
    protected void RadGrid2_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
}

