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
using System.Windows.Forms;

// Add aliases here:
using WinFormsTextBox = System.Windows.Forms.TextBox;
using WebTextBox = System.Web.UI.WebControls.TextBox;


public partial class VisitorPassEntry : System.Web.UI.Page
{
    public static DataTable dtTable;
    public static string connectionString;
    public SqlConnection SqlConnection = new SqlConnection();
    public SqlDataReader SqlDataReader;
    public SqlDataAdapter SqlDataAdapter = new SqlDataAdapter();
    public SqlCommand SqlCommand = new SqlCommand();
    public SqlDataReader SqlDataRdr;
    public int iInsertSlNo = 0;
    public string Visitorid;
    public DateTime IDDate = DateTime.Today;
    public string sRadGrid1_DataSource;
    public string sRadGrid2_DataSource;
    public string PID;
    public double dLPID;

    protected void Page_Load(object sender, EventArgs e)
    {
        // 1) Session check (single place)
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
            if (MyUtilityCS.GetSessionDetails(strCode) == true)
            {
                Session["EmpCode"] = GlobalClassCS.session_empcode;
                Session["EmpName"] = GlobalClassCS.session_empname;
                Session["EmpLocation"] = GlobalClassCS.session_emplocation;
                // Use a consistent key casing: choose "MyConn" or "myConn" and stick to it
                Session["MyConn"] = GlobalClassCS.DBConn;
            }
        }
        // ***** END ****** CODE FOR MIGRATION **********

        var sessionConn = Session["MyConn"] != null ? Session["MyConn"].ToString() : null;
        if (string.IsNullOrWhiteSpace(sessionConn))
        {
            // Fallback to global if needed
            sessionConn = GlobalClassCS.DBConn;
        }

        GlobalClassCS.DBConn = sessionConn;
        connectionString = GlobalClassCS.DBConn;
        SqlConnection.ConnectionString = GlobalClassCS.DBConn;
        
        if (IsPostBack)
        {
            
            lblDateTime.Text = DateTime.Now.ToString();
            //txtFormID.Text = PID;
            //rwVisitorGrid.VisibleOnPageLoad = false;
            //RadWindow2.VisibleOnPageLoad = false;
        }
        else
        {
            
            visitdatefrom.SelectedDate = DateTime.Today;
            visitdateto.SelectedDate = DateTime.Today;
            rbStatusList.SelectedValue = "N";
            Visitorid = FuncVisitorID(IDDate);
            txtPersonToMeet.Text = Session["EmpName"].ToString();
            txtValidity.Enabled = false;
            txtDestination.Enabled = false;
           
            rwVisitorGrid.VisibleOnPageLoad = false;
            numVisitors.Enabled = false;
            string query = "SELECT UnitNo FROM Unit_Master ORDER BY UnitNo DESC";
            SqlDataAdapter da = new SqlDataAdapter(query, SqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            txtresarea.DataSource = dt;
            txtresarea.DataBind();
            txtDepartment.Enabled = false;
            txtPersonToMeet.Enabled = false;
            txtValidity.Text = "1";
            //RadWindow2.VisibleOnPageLoad = false;
        }


        if (IsPostBack != true)
        {
            GETSPID();
            Session["dLPID"] = dLPID;
            txtFormID.Text = "PID" + PID;
        }


        ApplySandalColor(this.Page);
        ApplyLavenderColor(this.Page);


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
                    txtDepartment.Text = teamCode;
                }
            }

            // Later anywhere in the page or other pages:
            var tc = Session["TeamCode"] as string;

        }
    }

    public double GETSPID()
    {

        //SqlConnection.Close();
        SqlConnection.Open();
        SqlCommand.CommandText = "SELECT @@SPID AS 'ID', SYSTEM_USER AS 'Login Name', USER AS 'User Name'";
        SqlCommand.Connection = SqlConnection;
        SqlDataReader DR = SqlCommand.ExecuteReader();
        while (DR.Read())
        {
            PID = DR["ID"].ToString();
        }

        dLPID = Convert.ToDouble(PID);
        SqlConnection.Close();
        return dLPID;

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

    //protected void btnSubmit_Click(object sender, EventArgs e)
    //{
    //    rwVisitorGrid.VisibleOnPageLoad = false;
    //    Visitorid = FuncVisitorID(IDDate);
    //    // Collect values from form controls
    //    string visitorName = txtFullName.Text.Trim();
    //    string visitorCompany = txtCompany.Text.Trim();
    //    string formId = Visitorid; // fixed value, or use txtFormID.Text if needed

    //    string purpose = txtpurpose.Value.Trim();
    //    string type = txttype.Value.Trim();
    //    string personToMeet = txtPersonToMeet.Text.Trim();
    //    string department = txtDepartment.Text.Trim();
    //    DateTime visitFrom = visitdatefrom.SelectedDate.Value;
    //    DateTime visitTo = visitdateto.SelectedDate.Value;
    //    string accessLevel = RadTextBox3.Text.Trim();
    //    string restrictedArea = txtresarea.Text.Trim();
    //    string duration = txtDuration.Text.Trim();
    //    string accessories = txtaccessories.Text.Trim();
    //    string legalIdProof = txtproof.Text.Trim();
    //    string enterDateTime = lblDateTime.Text.Trim();
    //    string Destination = txtDestination.Text.Trim();
    //    int visitorsCount = Convert.ToInt32(numVisitors.Value ?? 0);

    //    // Null-coalescing (defaults to false when null)
    //    bool breakfast = chkBreakfast.Checked ?? false;
    //    bool lunch = chkLunch.Checked ?? false;
    //    bool dinner = chkDinner.Checked ?? false;

    //    bool cabRequired;
    //    bool cabNotRequired;
    //    if (RadioOpen.SelectedValue.ToString() == "1")
    //    {
    //        cabRequired = true;
    //        cabNotRequired = false;
    //    }
    //    else
    //    {
    //        cabNotRequired = true;
    //        cabRequired = false;
    //    }
    //    if (string.IsNullOrEmpty(visitorName))
    //    {

    //        RadWindowManager1.RadAlert("Visitor Name Should not be empty.", 500, 150, "Validation Error", null);
    //        return;
    //    }

    //    if (string.IsNullOrEmpty(visitorCompany))
    //    {
    //        RadWindowManager1.RadAlert("Visitor Company Should not be empty.", 500, 150, "Validation Error", null);
    //        return;
    //    }

    //    if (string.IsNullOrEmpty(purpose))
    //    {
    //        RadWindowManager1.RadAlert("Purpose of Visit Should not be empty.", 500, 150, "Validation Error", null);
    //        return;
    //    }

    //    if (string.IsNullOrEmpty(type))
    //    {
    //        RadWindowManager1.RadAlert("Type of Visit Should not be empty.", 500, 150, "Validation Error", null);
    //        return;
    //    }

    //    if (string.IsNullOrEmpty(personToMeet))
    //    {
    //        RadWindowManager1.RadAlert("Person to Meet Should not be empty.", 500, 150, "Validation Error", null);
    //        return;
    //    }

    //    if (string.IsNullOrEmpty(accessLevel))
    //    {
    //        RadWindowManager1.RadAlert("Access Level Should not be empty.", 500, 150, "Validation Error", null);
    //        return;
    //    }

    //    if (string.IsNullOrEmpty(duration))
    //    {
    //        RadWindowManager1.RadAlert("Expected Duration Should not be empty.", 500, 150, "Validation Error", null);
    //        return;
    //    }

    //    if (string.IsNullOrEmpty(accessories))
    //    {
    //        RadWindowManager1.RadAlert("Details of accessories Should not be empty.", 500, 150, "Validation Error", null);
    //        return;
    //    }

    //    if (string.IsNullOrEmpty(legalIdProof))
    //    {
    //        RadWindowManager1.RadAlert("legalIdProof Should not be empty.", 500, 150, "Validation Error", null);
    //        return;
    //    }

    //    if (visitdatefrom.SelectedDate != null && visitdateto.SelectedDate != null)
    //    {
    //        DateTime fromDate = visitdatefrom.SelectedDate.Value;
    //        DateTime toDate = visitdateto.SelectedDate.Value;

    //        if (toDate < fromDate)
    //        {
    //            RadWindowManager1.RadAlert("Visit To Date cannot be earlier than Visit From Date.",
    //                                       500, 150, "Validation Error", null);
    //            visitdateto.Clear(); // reset invalid date
    //            return;
    //        }

    //        // Calculate difference in days (inclusive)
    //        int days = (toDate - fromDate).Days + 1;
    //        txtValidity.Text = days.ToString();
    //    }


    //    using (SqlConnection conn = new SqlConnection(connectionString))
    //    {

    //        string query = @"INSERT INTO TblVisitorPass_Request
    //                 (VisitorName, VisitorCompany, formid,
    //                  Purpose_of_Visit, Type_for_Visit, Person_to_Meet, Teamcode,
    //                  Visit_FromTime, Visit_ToTime, Restricted_Area,
    //                  Expected_Duration, Details_of_accessories, Legal_ID_proof, No_of_Visitors, Entered_DateTime, Status, Breakfast, Lunch, Dinner, CabRequired, CabNotRequired,AccessLevel,Destination,Validity)
    //                 VALUES
    //                 (@VisitorName, @VisitorCompany, @FormId,
    //                  @Purpose, @Type, @PersonToMeet, @Department,
    //                  @VisitFrom, @VisitTo, @RestrictedArea,
    //                  @Duration, @Accessories, @LegalIdProof, @NumVisitors, @enterDateTime, @status, @breakfast, @lunch, @dinner, @cabRequired, @cabNotRequired,@accessLevel,@Destination,@Validity)";

    //        using (SqlCommand cmd = new SqlCommand(query, conn))
    //        {
    //            // Add parameters
    //            cmd.Parameters.AddWithValue("@VisitorName", visitorName);
    //            cmd.Parameters.AddWithValue("@VisitorCompany", visitorCompany);
    //            cmd.Parameters.AddWithValue("@FormId", Visitorid);
    //            cmd.Parameters.AddWithValue("@Purpose", purpose);
    //            cmd.Parameters.AddWithValue("@Type", type);
    //            cmd.Parameters.AddWithValue("@PersonToMeet", personToMeet);
    //            cmd.Parameters.AddWithValue("@Department", department);
    //            cmd.Parameters.AddWithValue("@VisitFrom", visitFrom);
    //            cmd.Parameters.AddWithValue("@VisitTo", visitTo);
    //            cmd.Parameters.AddWithValue("@AccessLevel", accessLevel);
    //            cmd.Parameters.AddWithValue("@RestrictedArea", restrictedArea);
    //            cmd.Parameters.AddWithValue("@Duration", duration);
    //            cmd.Parameters.AddWithValue("@Accessories", accessories);
    //            cmd.Parameters.AddWithValue("@LegalIdProof", legalIdProof);
    //            cmd.Parameters.AddWithValue("@NumVisitors", BindVisitorsCount());
    //            cmd.Parameters.AddWithValue("@enterDateTime", lblDateTime.Text);
    //            cmd.Parameters.AddWithValue("@status", "N");
    //            cmd.Parameters.AddWithValue("@Breakfast", breakfast);
    //            cmd.Parameters.AddWithValue("@Lunch", lunch);
    //            cmd.Parameters.AddWithValue("@Dinner", dinner);
    //            cmd.Parameters.AddWithValue("@cabRequired", cabRequired);
    //            cmd.Parameters.AddWithValue("@cabNotRequired", cabNotRequired);
    //            cmd.Parameters.AddWithValue("@Destination", Destination);
    //            cmd.Parameters.AddWithValue("@Validity", txtValidity.Text);

    //            conn.Open();
    //            cmd.ExecuteNonQuery();

    //            ShowMessage("Data Saved Successfully");
    //            txtFormID.Text = "";

    //            // Get last Msg_No
    //            DataTable dt = GetTable("SELECT TOP 1 Msg_No FROM EMails_Undelivered ORDER BY Msg_Slno DESC");

    //            int inc = 1; // Default if no rows
    //            if (dt != null && dt.Rows.Count > 0)
    //            {
    //                int lastMsgNo;
    //                if (int.TryParse(Convert.ToString(dt.Rows[0]["Msg_No"]), out lastMsgNo))
    //                {
    //                    inc = lastMsgNo + 1;
    //                }
    //            }

    //            // Get recipient email
    //            DataTable dtTable = GetTable("SELECT EMailID FROM VisitorPass_Approval_Config WHERE Teamcode = 'Soft'");
    //            if (dtTable != null && dtTable.Rows.Count > 0)
    //            {
    //                string recipientEmail = Convert.ToString(dtTable.Rows[0]["EMailID"]);
    //                InsertApprovalEmail(visitorName, visitorCompany, personToMeet, recipientEmail, Visitorid, inc, visitFrom);
    //            }

    //            Funclear();
    //            RadGrid1.Rebind();
    //        }
    //    }
    //}

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        rwVisitorGrid.VisibleOnPageLoad = false;
        Visitorid = FuncVisitorID(IDDate);

        // Collect values from form controls
        string visitorName = txtFullName.Text.Trim();
        string visitorCompany = txtCompany.Text.Trim();
        string formId = Visitorid; // or use txtFormID.Text if editing existing

        string purpose = txtpurpose.Value.Trim();
        string type = txttype.Value.Trim();
        string personToMeet = txtPersonToMeet.Text.Trim();
        string department = txtDepartment.Text.Trim();
        DateTime visitFrom = visitdatefrom.SelectedDate.Value;
        DateTime visitTo = visitdateto.SelectedDate.Value;
        string accessLevel = RadTextBox3.Text.Trim();
        string restrictedArea = txtresarea.Text.Trim();
        string duration = txtDuration.Text.Trim();
        string accessories = txtaccessories.Text.Trim();
        string legalIdProof = txtproof.Text.Trim();
        string enterDateTime = lblDateTime.Text.Trim();
        string Destination = txtDestination.Text.Trim();
        int visitorsCount = Convert.ToInt32(numVisitors.Value ?? 0);
        // Null-coalescing (defaults to false when null)
        bool breakfast = chkBreakfast.Checked ?? false;
        bool lunch = chkLunch.Checked ?? false;
        bool dinner = chkDinner.Checked ?? false;

        bool cabRequired;
        bool cabNotRequired;
        if (RadioOpen.SelectedValue.ToString() == "1")
        {
            cabRequired = true;
            cabNotRequired = false;
        }
        else
        {
            cabNotRequired = true;
            cabRequired = false;
        }
   

            // Validations
            if (string.IsNullOrEmpty(visitorName))
        {
            RadWindowManager1.RadAlert("Visitor Name Should not be empty.", 500, 150, "Validation Error", null);
            return;
        }
        if (string.IsNullOrEmpty(visitorCompany))
        {
            RadWindowManager1.RadAlert("Visitor Company Should not be empty.", 500, 150, "Validation Error", null);
            return;
        }
        if (string.IsNullOrEmpty(purpose))
        {
            RadWindowManager1.RadAlert("Purpose of Visit Should not be empty.", 500, 150, "Validation Error", null);
            return;
        }
        if (string.IsNullOrEmpty(type))
        {
            RadWindowManager1.RadAlert("Type of Visit Should not be empty.", 500, 150, "Validation Error", null);
            return;
        }
        if (string.IsNullOrEmpty(personToMeet))
        {
            RadWindowManager1.RadAlert("Person to Meet Should not be empty.", 500, 150, "Validation Error", null);
            return;
        }
        if (string.IsNullOrEmpty(accessLevel))
        {
            RadWindowManager1.RadAlert("Access Level Should not be empty.", 500, 150, "Validation Error", null);
            return;
        }
        if (string.IsNullOrEmpty(duration))
        {
            RadWindowManager1.RadAlert("Expected Duration Should not be empty.", 500, 150, "Validation Error", null);
            return;
        }
        if (string.IsNullOrEmpty(accessories))
        {
            RadWindowManager1.RadAlert("Details of accessories Should not be empty.", 500, 150, "Validation Error", null);
            return;
        }
        if (string.IsNullOrEmpty(legalIdProof))
        {
            RadWindowManager1.RadAlert("Legal ID Proof Should not be empty.", 500, 150, "Validation Error", null);
            return;
        }

        if (visitdatefrom.SelectedDate != null && visitdateto.SelectedDate != null)
        {
            DateTime fromDate = visitdatefrom.SelectedDate.Value;
            DateTime toDate = visitdateto.SelectedDate.Value;

            if (toDate < fromDate)
            {
                RadWindowManager1.RadAlert("Visit To Date cannot be earlier than Visit From Date.", 500, 150, "Validation Error", null);
                visitdateto.Clear();
                return;
            }

            int days = (toDate - fromDate).Days + 1;
            txtValidity.Text = days.ToString();
        }

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            // Check if record exists
            string checkQuery = "SELECT COUNT(*) FROM TblVisitorPass_Request WHERE formid = @FormId";
            using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
            {
                checkCmd.Parameters.AddWithValue("@FormId", txtFormID.Text);
                int count = (int)checkCmd.ExecuteScalar();

                string query;
                bool isUpdate = count > 0;
                if (isUpdate)
                {
                    string formmId = txtFormID.Text;
                    // UPDATE existing record
                    query = @"UPDATE TblVisitorPass_Request
                          SET VisitorName = @VisitorName,
                              VisitorCompany = @VisitorCompany,
                              Purpose_of_Visit = @Purpose,
                              Type_for_Visit = @Type,
                              Person_to_Meet = @PersonToMeet,
                              Teamcode = @Department,
                              Visit_FromTime = @VisitFrom,
                              Visit_ToTime = @VisitTo,
                              Restricted_Area = @RestrictedArea,
                              Expected_Duration = @Duration,
                              Details_of_accessories = @Accessories,
                              Legal_ID_proof = @LegalIdProof,
                              No_of_Visitors = @NumVisitors,
                              Entered_DateTime = @enterDateTime,
                              Status = @status,
                              Breakfast = @Breakfast,
                              Lunch = @Lunch,
                              Dinner = @Dinner,
                              CabRequired = @cabRequired,
                              CabNotRequired = @cabNotRequired,
                              AccessLevel = @AccessLevel,
                              Destination = @Destination,
                              Validity = @Validity
                          WHERE formid = @formmId";
                }
                else
                {
                    // INSERT new record
                    query = @"INSERT INTO TblVisitorPass_Request
                         (VisitorName, VisitorCompany, formid,
                          Purpose_of_Visit, Type_for_Visit, Person_to_Meet, Teamcode,
                          Visit_FromTime, Visit_ToTime, Restricted_Area,
                          Expected_Duration, Details_of_accessories, Legal_ID_proof, No_of_Visitors, Entered_DateTime, Status, Breakfast, Lunch, Dinner, CabRequired, CabNotRequired, AccessLevel, Destination, Validity)
                         VALUES
                         (@VisitorName, @VisitorCompany, @FormId,
                          @Purpose, @Type, @PersonToMeet, @Department,
                          @VisitFrom, @VisitTo, @RestrictedArea,
                          @Duration, @Accessories, @LegalIdProof, @NumVisitors, @enterDateTime, @status, @Breakfast, @Lunch, @Dinner, @cabRequired, @cabNotRequired, @AccessLevel, @Destination, @Validity)";
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Add parameters
                    cmd.Parameters.AddWithValue("@VisitorName", visitorName);
                    cmd.Parameters.AddWithValue("@VisitorCompany", visitorCompany);
                    cmd.Parameters.AddWithValue("@FormId", formId);
                    cmd.Parameters.AddWithValue("@Purpose", purpose);
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters.AddWithValue("@PersonToMeet", personToMeet);
                    cmd.Parameters.AddWithValue("@Department", department);
                    cmd.Parameters.AddWithValue("@VisitFrom", visitFrom);
                    cmd.Parameters.AddWithValue("@VisitTo", visitTo);
                    cmd.Parameters.AddWithValue("@AccessLevel", accessLevel);
                    cmd.Parameters.AddWithValue("@RestrictedArea", restrictedArea);
                    cmd.Parameters.AddWithValue("@Duration", duration);
                    cmd.Parameters.AddWithValue("@Accessories", accessories);
                    cmd.Parameters.AddWithValue("@LegalIdProof", legalIdProof);
                    cmd.Parameters.AddWithValue("@NumVisitors", BindVisitorsCount());
                    cmd.Parameters.AddWithValue("@enterDateTime", lblDateTime.Text);
                    cmd.Parameters.AddWithValue("@status", "N");
                    cmd.Parameters.AddWithValue("@Breakfast", breakfast);
                    cmd.Parameters.AddWithValue("@Lunch", lunch);
                    cmd.Parameters.AddWithValue("@Dinner", dinner);
                    cmd.Parameters.AddWithValue("@cabRequired", cabRequired);
                    cmd.Parameters.AddWithValue("@cabNotRequired", cabNotRequired);
                    cmd.Parameters.AddWithValue("@Destination", Destination);
                    cmd.Parameters.AddWithValue("@Validity", txtValidity.Text);

                    if (isUpdate)
                    {
                        // For UPDATE, use @formmId (textbox value)
                        cmd.Parameters.AddWithValue("@formmId", txtFormID.Text);
                    }
                    else
                    {
                        // For INSERT, use textbox value only
                        cmd.Parameters.AddWithValue("@formmId", txtFormID.Text);

                        // DO NOT re-add @FormId here, it’s already added above
                        // cmd.Parameters.AddWithValue("@FormId", formId);

                        // Query existing records using textbox value
                        string checkQueryInsert = "SELECT FormID FROM tbl_VisitorPass_member_temp WHERE FormID LIKE '%' + @formmId + '%'";
                        List<string> oldFormIds = new List<string>();

                        using (SqlCommand checkCmdInsert = new SqlCommand(checkQueryInsert, conn))
                        {
                            checkCmdInsert.Parameters.AddWithValue("@formmId", txtFormID.Text);

                            using (SqlDataReader reader = checkCmdInsert.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    oldFormIds.Add(reader["FormID"].ToString());
                                }
                            }
                        }

                        // Update each record after closing the reader
                        foreach (string oldFormId in oldFormIds)
                        {
                            string newFormId = oldFormId.Replace(txtFormID.Text, formId);

                            using (SqlCommand updateCmd = new SqlCommand(
                                "UPDATE tbl_VisitorPass_member_temp SET FormID = @newFormId WHERE FormID = @oldFormId", conn))
                            {
                                updateCmd.Parameters.AddWithValue("@newFormId", newFormId);
                                updateCmd.Parameters.AddWithValue("@oldFormId", oldFormId);

                                updateCmd.ExecuteNonQuery();
                            }
                        }
                    }


                    cmd.ExecuteNonQuery();
                }
            }

            ShowMessage("Data Saved Successfully");

            GETSPID();
            Session["dLPID"] = dLPID;
            txtFormID.Text = "PID" + PID;

            // Email logic
            DataTable dt = GetTable("SELECT TOP 1 Msg_No FROM EMails_Undelivered ORDER BY Msg_Slno DESC");
            int inc = 1;
            if (dt != null && dt.Rows.Count > 0)
            {
                int lastMsgNo;
                if (int.TryParse(Convert.ToString(dt.Rows[0]["Msg_No"]), out lastMsgNo))
                {
                    inc = lastMsgNo + 1;
                }
            }

            DataTable dtTable = GetTable("SELECT EMailID FROM VisitorPass_Approval_Config WHERE Teamcode = 'Soft'");
            if (dtTable != null && dtTable.Rows.Count > 0)
            {
                string recipientEmail = Convert.ToString(dtTable.Rows[0]["EMailID"]);
                InsertApprovalEmail(visitorName, visitorCompany, personToMeet, recipientEmail, formId, inc, visitFrom);
            }

            Funclear();
            RadGrid1.Rebind();
        }
    }


    private void InsertApprovalEmail(string visitorName, string visitorCompany, string personToMeet, string recipientEmail, string Visitorid, int inc, DateTime visitFrom)
    {
        string msgSubject = "Approval Required: Visitor Pass Request";
        string formattedDate = visitFrom.ToString("dd-MMM-yyyy");
        string msgText = "<HTML><BODY>"
                       + "Dear Approver,<br/><br/>"
                       + "A visitor pass request requires your approval.<br/><br/>"
                       + "<b>Visitor ID:</b> " + Visitorid + "<br/>"
                       + "<b>Visitor Name:</b> " + visitorName + "<br/>"
                       + "<b>Visitor Company:</b> " + visitorCompany + "<br/>"
                       + "<b>Person To Meet:</b> " + personToMeet + "<br/>"
                       + "<b>Visit Date:</b> " + formattedDate + "<br/>"
                       + "Please review and approve at your earliest convenience.<br/><br/>"
                       + "Thank you.<br/>"
                       + "</BODY></HTML>";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand(@"INSERT INTO EMails_Undelivered
            (Msg_No, Msg_FromName, Msg_Recipient, Msg_Subject, Msg_Text, Msg_RecDateTime, Msg_Status, Msg_Module, Msg_FormName, Msg_FuncName)
            VALUES (@Msg_No, @Msg_FromName, @Msg_Recipient, @Msg_Subject, @Msg_Text, @Msg_RecDateTime, @Msg_Status, @Msg_Module, @Msg_FormName, @Msg_FuncName)", conn)) // ✅ Pass connection here
            {
                cmd.Parameters.AddWithValue("@Msg_No", inc);
                cmd.Parameters.AddWithValue("@Msg_FromName", "Urjita ERP Alerter");
                cmd.Parameters.AddWithValue("@Msg_Recipient", recipientEmail);
                cmd.Parameters.AddWithValue("@Msg_Subject", msgSubject);
                cmd.Parameters.AddWithValue("@Msg_Text", msgText);
                cmd.Parameters.AddWithValue("@Msg_RecDateTime", DateTime.Now);
                cmd.Parameters.AddWithValue("@Msg_Status", 0);
                cmd.Parameters.AddWithValue("@Msg_Module", "VisitorPass.Net");
                cmd.Parameters.AddWithValue("@Msg_FormName", "VisitorPassEntry.aspx");
                cmd.Parameters.AddWithValue("@Msg_FuncName", "InsertApprovalEmail");

                cmd.ExecuteNonQuery();
            }
        }
    }

    private void Funclear()
    {

        txtFullName.Text = string.Empty;
        txtCompany.Text = string.Empty;
        txtpurpose.Value = string.Empty;
        txttype.Value = string.Empty;
        txtPersonToMeet.Text = string.Empty;
        txtDepartment.Text = string.Empty;

        // Other textboxes
        RadTextBox3.Text = string.Empty;
        txtresarea.Text = string.Empty;
        txtDuration.Text = string.Empty;
        txtaccessories.Text = string.Empty;
        txtproof.Text = string.Empty;
        txtValidity.Text = string.Empty;
        txtDestination.Text = string.Empty;
        //visitdateto.Text = string.Empty;
        //visitdatefrom.Text = string.Empty;

        // Labels
        lblDateTime.Text = string.Empty;

        // Numbers (Telerik RadNumericTextBox)
        numVisitors.Value = null;

        // Checkboxes (normalize bool? to false)
        chkBreakfast.Checked = false;
        chkLunch.Checked = false;
        chkDinner.Checked = false;

        RadioOpen.SelectedValue = "0";
    }


    private string FuncVisitorID(DateTime DtGRN)
    {
        string lastval = "001";
        string str = string.Empty;
        string Visitorid = string.Empty;

        //string we = FindWeekCode(DtGRN);
        string ye = FindYearCode(DtGRN);

        if (ye.Length >= 4)
            ye = ye.Substring(ye.Length - 2);


        string prefix = "VP" + ye;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "SELECT MAX(FormID) as FormID FROM TblVisitorPass_Request";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@prefix", prefix + "%");
                    object result = cmd.ExecuteScalar();

                    if (result != DBNull.Value && result != null && result.ToString().Trim() != "")
                    {
                        str = result.ToString().Trim();

                        // Ensure the GRN number is long enough to extract numeric part
                        if (str.Length >= 7)
                        {
                            string numericPart = str.Substring(str.Length - 3); // Get last 3 digits
                            int currentNum;
                            if (int.TryParse(numericPart, out currentNum))
                            {
                                int nextNum = currentNum + 1;
                                lastval = nextNum.ToString("D3"); // Pad to 3 digits
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating FormID: " + ex.Message);
            }
        }

        Visitorid = "VP" + ye + lastval;
        return Visitorid;
    }


    private string FuncSubVisitorID(DateTime DtGRN)
    {
        string lastval = "01"; // Default if no visitors exist
        string Visitorid = string.Empty;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();

                string FormID = txtFormID.Text;
                // Count visitors for the current FormID
                string query = @"SELECT ISNULL(MAX(CAST(SUBSTRING(FormID, LEN(@FormID)+2, 2) AS INT)), 0) FROM tbl_VisitorPass_member_temp WHERE FormID LIKE @FormID + '-%'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@FormID", SqlDbType.NVarChar).Value = txtFormID.Text;

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    // Next number = count + 1
                    int nextNumber = count + 1;
                    lastval = nextNumber.ToString("D2"); // Format as 01, 02, 03
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating VisitorID: " + ex.Message);
            }
        }

        string VP = txtFormID.Text; // Prefix from UI
        Visitorid = VP + "-" + lastval;
        return Visitorid;
    }


    private string FindWeekCode(DateTime date)
    {
        CultureInfo culture = CultureInfo.CurrentCulture;
        int weekNum = culture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        return weekNum.ToString();
    }

    private string FindYearCode(DateTime date)
    {
        return date.Year.ToString();
    }



    public void ShowMessage(string sMessage, string sTitle = "Alert", decimal dWidth = 500, decimal dHeight = 210)
    {
        string radalertscript = "function f(){var oAlert = radalert('" + sMessage + "', " + dWidth + ", " + dHeight + ",'" + sTitle + "'); setTimeout(function(){oAlert.center();},0);Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "radalert", radalertscript, true);
    }

    protected void btnAddVisitorDetails_Click1(object sender, EventArgs e)
    {

        RadWindowManager2.Windows[0].VisibleOnPageLoad = true;
        rwVisitorGrid.VisibleOnPageLoad = true;
        rgVisitorDetails.Rebind();
        BindVisitorsCount();
    }
    protected void rgVisitorDetails_InsertCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem item = (GridEditableItem)e.Item;

        string visitorName = (item["VisitorName"].Controls[0] as WebTextBox).Text;

        string formId = FuncSubVisitorID(IDDate);
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"INSERT INTO tbl_VisitorPass_member_temp (FormID,VisitorName)
                         VALUES (@FormID,@VisitorName)";
            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.Add("@FormID", SqlDbType.NVarChar, 50).Value = formId;
            cmd.Parameters.Add("@VisitorName", SqlDbType.NVarChar, 200).Value = (visitorName ?? string.Empty);

            conn.Open();
            cmd.ExecuteNonQuery();
            rgVisitorDetails.Rebind();
        }

    }
    protected void rgVisitorDetails_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        string formId = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FormID"].ToString();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "DELETE FROM tbl_VisitorPass_member_temp WHERE FormID = @FormID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FormID", formId);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

    }

    protected void btnTerminate_Click(object sender, EventArgs e)
    {
        rwVisitorGrid.VisibleOnPageLoad = false;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {

            string query = @"UPDATE TblVisitorPass_Request 
                     SET Status = @Status 
                     WHERE formid = @FormId";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Status", "T");
                cmd.Parameters.AddWithValue("@FormId", txtFormID.Text);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    ShowMessage("Visitor request terminated successfully.");
                    Funclear();
                    RadGrid1.Rebind();
                }
                else
                {
                    ShowMessage("No record found with the given FormID.");
                }
            }
        }
    }

    protected void rgVisitorDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = @"
        SELECT FormID, VisitorName, VisitorCompany
        FROM tbl_VisitorPass_member_temp
        WHERE FormID LIKE '%' + @FormID + '%'";

            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            da.SelectCommand.Parameters.Add("@FormID", SqlDbType.NVarChar).Value = txtFormID.Text;

            DataTable dt = new DataTable();
            da.Fill(dt);
            rgVisitorDetails.DataSource = dt;
        }
        BindVisitorsCount();
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        string teamcode = (string)Session["Teamcode"];
        //sRadGrid1_DataSource = "SELECT * FROM TblVisitorPass_Request where status = 'N' and teamcode ='" + Session["Teamcode"] + "'";
        //RadGrid1.DataSource = GetTable(sRadGrid1_DataSource);
        string selectQuery = "SELECT * FROM TblVisitorPass_Request where status = 'N' and teamcode ='" + Session["Teamcode"] + "' and Visit_ToTime >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' order by FormID desc";
        dtTable = GetTable(selectQuery);
        RadGrid1.DataSource = dtTable;

    }


    private int BindVisitorsCount()
    {
        string formId = txtFormID.Text;
        string likePattern = "%" + formId + "%";

        using (var conn = new SqlConnection(connectionString))
        using (var cmd = new SqlCommand("SELECT COUNT(*) FROM tbl_VisitorPass_member_temp WHERE FormID LIKE @FormID", conn))
        {
            cmd.Parameters.Add("@FormID", SqlDbType.NVarChar, 100).Value = likePattern;
            conn.Open();
            object result = cmd.ExecuteScalar();
            int visitorsCount = (result == null || result == DBNull.Value)
                ? 0
                : Convert.ToInt32(result);
            numVisitors.Value = visitorsCount;
            return visitorsCount; // ← crucial to satisfy the method's return type
        }
    }
        private DataTable GetTable(string sql)
    {
        DataTable ds = new DataTable();
        try
        {
            //SqlConnection conn = new SqlConnection(connectionString)
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["urjLive"].ToString();
                con.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, con);
                dataAdapter.Fill(ds);
            }
            catch (Exception ex) 
            {
                //Logger.get_error_log(ex, "", "", sql);
            }

            finally
            {
                con.Close();
            }
        }
        catch (Exception ex)
        {
            // Logger.get_error_log(ex, "", "", "");
        }

        return ds;
    }
    //public DataTable GetTable(string query)
    //{
    //    SqlConnection SqlConnection = new SqlConnection(connectionString);
    //    SqlConnection.Open();
    //    DataTable myTable = new DataTable();
    //    SqlDataAdapter.SelectCommand = new SqlCommand(query, SqlConnection);
    //    SqlDataAdapter.Fill(myTable);
    //    SqlConnection.Close();
    //    return myTable;
    //}

    protected void RadGrid1_InsertCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
    {

    }


    private static void EnsureHtmlSelectValue(System.Web.UI.HtmlControls.HtmlSelect sel, string value)
    {
        if (sel == null || string.IsNullOrWhiteSpace(value)) return;

        foreach (System.Web.UI.WebControls.ListItem li in sel.Items)
        {
            if (string.Equals(li.Value, value, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(li.Text, value, StringComparison.OrdinalIgnoreCase))
            {
                sel.Value = li.Value; // select it
                return;
            }
        }

        // If not found, add and select
        sel.Items.Add(new System.Web.UI.WebControls.ListItem(value, value));
        sel.Value = value;
    }


    private static DateTime? SafeNullableDate(object value)
    {
        if (value == null || value == DBNull.Value) return null;
        DateTime dt;
        return DateTime.TryParse(value.ToString(), out dt) ? (DateTime?)dt : null;
    }


    private static bool SafeBool(object value)
    {
        if (value == null || value == DBNull.Value) return false;
        if (value is bool) return (bool)value;
        var s = value.ToString().Trim().ToLowerInvariant();
        return s == "true" || s == "1" || s == "y" || s == "yes";
    }

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        LinkButton lnkFormId = e.Item.FindControl("lnkFormId") as LinkButton;
        ViewState["FormID"] = lnkFormId.Text;

        if (e.CommandName == "OpenForm")
        {
            //RadWindow2.VisibleOnPageLoad = true;
            //btnSubmit.Enabled = false;
            SqlConnection.Open();
            SqlCommand.CommandText = "SELECT FormID, VisitorName, VisitorCompany, Person_to_Meet, AccessLevel,Teamcode, Expected_Duration, Details_of_accessories, Legal_ID_proof, Restricted_Area, No_of_Visitors, Destination, AccessLevel, Purpose_of_Visit, Type_for_Visit,Status, Visit_FromTime,Visit_ToTime, Entered_DateTime, Breakfast, Lunch, Dinner, CabRequired, CabNotRequired,Validity FROM TblVisitorPass_Request where formid = '" + ViewState["FormID"] + "' and Visit_ToTime >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            SqlCommand.Connection = SqlConnection;
            SqlDataReader dr = SqlCommand.ExecuteReader();
            if (dr.HasRows && dr.Read())
            {
                txtFormID.Text = dr["FormID"].ToString();
                txtFullName.Text = dr["VisitorName"].ToString();
                txtCompany.Text = dr["VisitorCompany"].ToString();
                txtPersonToMeet.Text = dr["Person_to_Meet"].ToString();
                txtDepartment.Text = dr["Teamcode"].ToString();
                txtDuration.Text = dr["Expected_Duration"].ToString();
                txtaccessories.Text = dr["Details_of_accessories"].ToString();
                txtproof.Text = dr["Legal_ID_proof"].ToString();
                txtresarea.Text = dr["Restricted_Area"].ToString();
                numVisitors.Text = dr["No_of_Visitors"].ToString();
                txtDestination.Text = dr["Destination"].ToString();
                RadTextBox3.Text = dr["AccessLevel"].ToString();
                txtValidity.Text = dr["Validity"].ToString();
                EnsureHtmlSelectValue(txtpurpose, dr["Purpose_of_Visit"].ToString());
                EnsureHtmlSelectValue(txttype, dr["Type_for_Visit"].ToString());

                visitdatefrom.SelectedDate = SafeNullableDate(dr["Visit_FromTime"]);
                visitdateto.SelectedDate = SafeNullableDate(dr["Visit_ToTime"]);
                lblDateTime.Text = dr["Entered_DateTime"].ToString();

                chkBreakfast.Checked = SafeBool(dr["Breakfast"]);
                chkLunch.Checked = SafeBool(dr["Lunch"]);
                chkDinner.Checked = SafeBool(dr["Dinner"]);

                bool cabRequired = SafeBool(dr["CabRequired"]); if (cabRequired)
                {
                    RadioOpen.SelectedValue = "1";
                } 
                else
                { 
                    RadioOpen.SelectedValue = "2"; 
                }
            }
            if (dr["Status"].ToString() == "T")
            {
                DisableFormControls();
            }
            if (dr["Status"].ToString() == "A")
            {
                DisableFormControls();
                RadGrid1.Enabled = false;
            }
            if (dr["Status"].ToString() == "D")
            {
                DisableFormControls();
                RadGrid1.Enabled = false;
            }
        }
    }
    protected void rbStatusList_SelectedIndexChanged(object sender, EventArgs e)
    {
        Funclear();
        string status = rbStatusList.SelectedValue;
        string Teamcode = (string) Session["Teamcode"];
        BindGrid(status, Teamcode);
    }
    private void BindGrid(string status,string Teamcode)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand("SELECT * FROM TblVisitorPass_Request WHERE Status = '" + status + "' and Teamcode = '"+ Teamcode + "' and Visit_ToTime >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'", conn))
        {
            //cmd.Parameters.Add("@Status", SqlDbType.VarChar, 1).Value = status; // Use VarChar if column is VARCHAR
            //cmd.Parameters.Add("@Teamcode", SqlDbType.VarChar, 1).Value = Teamcode; // Use VarChar if column is VARCHAR

            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                RadGrid1.DataSource = dt;
                RadGrid1.DataBind();
            }
        }
    }
    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
    private void DisableFormControls()
    {
        txtFormID.Enabled = false;
        txtFullName.Enabled = false;
        txtCompany.Enabled = false;
        txtPersonToMeet.Enabled = false;
        txtDepartment.Enabled = false;
        txtDuration.Enabled = false;
        txtaccessories.Enabled = false;
        txtproof.Enabled = false;
        txtresarea.Enabled = false;
        numVisitors.Enabled = false;
        txtValidity.Enabled = false;
        RadTextBox3.Enabled = false;
        txtDestination.Enabled = false;
        btnSubmit.Enabled = false;
        btnTerminate.Enabled = false;
        if (txtpurpose != null) txtpurpose.Disabled = true;
        if (txttype != null) txttype.Disabled = true;

        visitdatefrom.Enabled = false;
        visitdateto.Enabled = false;

        chkBreakfast.Enabled = false;
        chkLunch.Enabled = false;
        chkDinner.Enabled = false;
        //RadCheckBox1.Enabled = false;
        //RadCheckBox2.Enabled = false;

        RadioOpen.Enabled = false;

        foreach (BaseValidator v in Page.Validators)
        {
            v.Enabled = false;
        }
    }
    protected void RadioOpen_SelectedIndexChanged(object sender, EventArgs e)
    {
        rwVisitorGrid.VisibleOnPageLoad = false;
        if (RadioOpen.SelectedValue == "1") 
        {
            txtDestination.Enabled = true; 
        } 
        else 
        {
            txtDestination.Enabled = false; 
        }
    }

    protected void MealCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        rwVisitorGrid.VisibleOnPageLoad = false;
    }



    protected void visitdateto_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        DateTime fromDate = visitdatefrom.SelectedDate.Value;
        DateTime toDate = visitdateto.SelectedDate.Value;

        if (toDate < fromDate)
        {
            RadWindowManager1.RadAlert("Visit To Date cannot be earlier than Visit From Date.",
                                       500, 150, "Validation Error", null);
            visitdateto.Clear(); // reset invalid date
            return;
        }

        // Calculate difference in days (inclusive)
        int days = (toDate - fromDate).Days + 1;
        txtValidity.Text = days.ToString();
    }
    protected void visitdateto_SelectedDateChanged1(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        DateTime fromDate = visitdatefrom.SelectedDate.Value;
        DateTime toDate = visitdateto.SelectedDate.Value;
        if (toDate < fromDate)
        {
            RadWindowManager1.RadAlert("Visit To Date cannot be earlier than Visit From Date.",500, 150, "Validation Error", null);
            visitdateto.Clear(); // reset invalid date
            return;
        }
        // Calculate difference in days (inclusive)
        int days = (toDate - fromDate).Days + 1;
        txtValidity.Text = days.ToString();
    }


}

