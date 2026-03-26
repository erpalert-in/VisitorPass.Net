<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="VisitorPassEntry.aspx.cs" Inherits="VisitorPassEntry" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPMain" runat="Server">
    <style>
        .RadInput_Default input[type="text"],
        .RadInput_Default textarea {
            background-color: #E6E6FA !important; /* Mild sandal color */
        }

        .form-section .RadInput_Default input[type="text"] {
            background-color: #E6E6FA !important;
        }


        select {
            background-color: #E6E6FA !important;
        }


        /* Mild Lavender for RadTextBox inner input */
        .RadInput_Default input[type="text"],
        .RadInput_Default textarea {
            background-color: #E6E6FA !important; /* Mild Lavender */
        }


        /* RadTextBox */
        .RadInput_Default input[type="text"],
        .RadInput_Default textarea {
            background-color: #E6E6FA !important;
        }

        /* HTML select dropdowns */
        select {
            background-color: #E6E6FA !important;
        }

        /* RadCheckBox labels */
        .RadCheckBox_Default input[type="checkbox"] + label {
            background-color: #E6E6FA !important;
            padding: 5px;
            border-radius: 4px;
        }

        /* RadDatePicker input */
        .RadDatePicker .rcInput {
            background-color: #E6E6FA !important;
        }

        /* RadComboBox input */
        .RadComboBox .rcbInput {
            background-color: #E6E6FA !important;
        }


        .RadRadioButtonList .rbItem {
            margin-right: 15px;
        }


        /* Mild Lavender for Telerik RadRadioButton labels */
        .RadRadioButton_Default input[type="radio"] + label {
            background-color: #E6E6FA !important; /* Mild Lavender */
            padding: 6px 10px;
            border-radius: 4px;
            display: inline-block;
            margin-right: 8px;
        }

        body {
            font-family: Arial, sans-serif;
            margin: 20px;
            background-color: #f5f5f5;
        }

        .wrapper {
            display: flex;
            gap: 20px; /* space between divs */
            align-items: stretch; /* makes both divs equal height */
        }

        .container,
        .container1 {
            flex: 1; /* equal width */
            max-width: 700px;
            background: #fff;
            padding: 20px;
            border-radius: 49px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
            display: flex;
            flex-direction: column; /* keep internal content aligned */
        }


        header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            border-bottom: 2px solid #ccc;
            padding-bottom: 10px;
            margin-bottom: 20px;
        }

            header img {
                height: 50px;
            }

        h1 {
            margin: 0;
            font-size: 24px;
        }

        .form-section {
            margin-bottom: 20px;
        }

            .form-section h2 {
                font-size: 18px;
                margin-bottom: 10px;
                border-bottom: 1px solid #ddd;
                padding-bottom: 5px;
            }

        label {
            display: block;
            margin: 8px 0 4px;
            font-weight: bold;
        }

        input, select, textarea {
            width: 100%;
            padding: 8px;
            margin-bottom: 10px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        .form-actions {
            text-align: right;
        }

        button {
            background: #0078d7;
            color: #fff;
            border: none;
            padding: 10px 20px;
            border-radius: 4px;
            cursor: pointer;
        }

            button:hover {
                background: #005a9e;
            }

        header {
            display: flex;
            justify-content: space-between; /* left and right sides */
            align-items: center;
            border-bottom: 2px solid #ccc;
            padding-bottom: 10px;
            margin-bottom: 20px;
        }

        .header-left h1 {
            margin: 0;
            font-size: 24px;
        }

        .header-right p {
            margin: 0;
            font-weight: bold;
            font-size: 14px;
        }

        .page-layout {
            display: flex;
            align-items: flex-start;
        }

        .visitor-details {
            flex: 1; /* left column */
            margin-right: 20px;
            text-align: left;
        }

        .RadWindow_Metro .rwTitlebar {
            background-color: #0078D7; /* Blue header */
            color: white;
            font-weight: bold;
        }

        .RadWindow_Metro .rwContent {
            background-color: #f9f9f9; /* Light background */
            font-size: 14px;
            color: #333;
        }

        .RadWindow_Metro .rwIcon {
            color: #0078D7; /* Icon color */
        }

        .RadWindow_Metro .rwButton {
            background-color: #0078D7;
            color: white;
            border-radius: 4px;
            padding: 6px 12px;
        }

        .RadWindow_Metro .rwContent {
            font-size: 16px;
            padding: 20px;
        }


        .RadWindow_Metro {
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.2);
        }

            .RadWindow_Metro .container {
                /* Add your container styles here */
                padding: 16px;
                background-color: #fff;
            }


            .RadWindow_Metro .rwTitlebar .rwTitle {
                text-align: center !important;
                width: 100%;
                display: block;
            }

        {
            font-weight: bold;
            margin-bottom: 5px; /* spacing between label and textbox */
            display: block;
        }

        .RadWindow_Metro .rwTitlebar .rwIcon {
            display: none;
        }
    </style>



    <style>
        /* Wider filter inputs inside the filter row */
        .RadGrid .rgFilterRow input[type="text"],
        .RadGrid .rgFilterRow .riTextBox {
            width: 95%;
            box-sizing: border-box;
        }

        /* Avoid header text wrapping */
        .RadGrid th.rgHeader {
            white-space: nowrap;
        }

        /* Keep cells from wrapping into multiple lines */
        .RadGrid .rgRow td, .RadGrid .rgAltRow td {
            white-space: nowrap;
        }

        /* Change group panel background to grey */
        .RadGrid .rgGroupPanel {
            background-color: #e5e5e5 !important; /* light grey */
            color: #000 !important; /* black text */
            font-weight: bold;
            background-image: none !important; /* remove gradient */
        }
    </style>



    <style>
        .form-link {
            color: #000; /* black font */
            text-decoration: underline; /* underline */
            font-weight: normal; /* optional */
        }

            .form-link:hover {
                color: #000; /* keep black on hover */
                text-decoration: underline;
            }
    </style>


    <script>

</script>

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Metro" EnableShadow="true">
    </telerik:RadWindowManager>

    <div style="display: flex">
        <div class="container" style="position: relative; left: 100px">

            <header>
                <div class="header-left">
                    <h1>Visitor Pass Request Form</h1>
                </div>
                <div class="header-right">
                    <asp:Label ID="lblDateTime" runat="server" CssClass="datetime" />
                </div>
            </header>

            <div class="form-section">
                <h2>Visitor Information</h2>
                <label for="purpose">Form ID</label>

                <telerik:RadTextBox ID="txtFormID" runat="server" ReadOnly="true" Width="100%"
                    Text='<%# Eval("FormID") %>' />

                <label for="purpose">Full Name *</label>
                <telerik:RadTextBox ID="txtFullName" runat="server" Width="100%" />
                <label for="purpose">Mobile Number *</label>
                <telerik:RadNumericTextBox ID="txtMobileNumber" runat="server" Width="100%"
                    Type="Number" NumberFormat-DecimalDigits="0" NumberFormat-GroupSeparator=""
                    MinValue="0" MaxValue="9999999999" MaxLength="10"
                    EmptyMessage="Enter 10-digit mobile number" />
                <label for="purpose">Company/Organization *</label>
                <telerik:RadTextBox ID="txtCompany" runat="server" Width="100%" />
                <div class="form-section">
                    <!-- Visit Details -->
                    <div class="form-section">
                        <h2>Visit Details</h2>
                        <label for="purpose">Purpose of Visit *</label>
                        <select id="txtpurpose" runat="server" name="purpose">
                            <option value="">--Select--</option>
                            <option>Client Meeting</option>
                            <option>Interview</option>
                            <option>Training Session</option>
                            <option>Vendor Visit</option>
                            <option>Audit</option>
                            <option>Maintenance</option>
                            <option>Consultation</option>
                            <option>Delivery</option>
                            <option>Technical Support</option>
                            <option>Compliance Check</option>
                            <option>Demo / Product Showcase</option>
                            <option>Site Inspection</option>
                        </select>

                        <label for="type">Type of Visit *</label>
                        <select id="txttype" runat="server" name="type">
                            <option>Planned</option>
                            <option>Unplanned</option>
                        </select>
                        <label for="type">Person to Meet *</label>
                        <telerik:RadTextBox ID="txtPersonToMeet" runat="server" Width="100%" />
                        <label for="type">Teamcode *</label>
                        <telerik:RadTextBox ID="txtDepartment" runat="server" Width="100%" />
                        <label for="RadTextBox3">Meals Option</label>

                        <telerik:RadCheckBox ID="chkBreakfast" runat="server" Text="Breakfast"
                            AutoPostBack="true" OnCheckedChanged="MealCheckBox_CheckedChanged" />

                        <telerik:RadCheckBox ID="chkLunch" runat="server" Text="Lunch"
                            AutoPostBack="true" OnCheckedChanged="MealCheckBox_CheckedChanged" />

                        <telerik:RadCheckBox ID="chkDinner" runat="server" Text="Dinner"
                            AutoPostBack="true" OnCheckedChanged="MealCheckBox_CheckedChanged" />


                        <label for="RadTextBox4">Cab Arrangements</label>

                        <div>
                            <telerik:RadRadioButtonList runat="server" ID="RadioOpen" Style="display: flex; justify-content: center; position: relative; gap: 20px" OnSelectedIndexChanged="RadioOpen_SelectedIndexChanged">
                                <Items>
                                    <telerik:ButtonListItem Text="Required" Selected="true" Value="1" />
                                    <telerik:ButtonListItem Text="Not Required" Selected="true" Value="2" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </div>

                        <div>
                            <label for="txtDestination">Destination:</label>
                            <telerik:RadTextBox ID="txtDestination" runat="server" Width="100%" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div>

            <br />
            <br />

            <div class="container1" style="position: relative; right: -152px">
                <div class="form-section">
                    <h3>Security & Access</h3>
                    <label for="Access Level">Access Level *</label>
                    <telerik:RadTextBox ID="RadTextBox3" runat="server" Width="555px" />
                    <label for="Restricated Area">Restricated Area</label>


                    <telerik:RadComboBox ID="txtresarea" runat="server" Width="100%"
                        DataTextField="UnitNo"
                        DataValueField="UnitNo"
                        AllowCustomText="true"
                        EnableLoadOnDemand="true"
                        MarkFirstMatch="true">
                    </telerik:RadComboBox>



                    <telerik:RadDatePicker ID="visitdatefrom" runat="server" DateInput-Label="From Date *" Height="30px"
                        DateInput-EmptyMessage="Select a date"
                        DateInput-Required="true"
                        DateInput-ToolTip="Select the start date"
                        Width="250px" />

                    <%--<label for="visitdateto">To Date *</label>--%>
                    <telerik:RadDatePicker ID="visitdateto" runat="server" OnSelectedDateChanged="visitdateto_SelectedDateChanged1" AutoPostBack="true" DateInput-Label="To Date *" Height="30px"
                        DateInput-EmptyMessage="Select a date"
                        DateInput-Required="true"
                        DateInput-ToolTip="Select the end date"
                        Width="250px" />
                    <label for="Validity">Validity</label>
                    <telerik:RadTextBox ID="txtValidity" runat="server" Width="100%" />
                </div>

                <div class="form-section">
                    <h4>Visitor Details</h4>

                    <label for="txtDuration">Expected Duration *</label>
                    <telerik:RadTextBox
                        ID="txtDuration"
                        runat="server"
                        Width="100%" />

                    <label for="txtaccessories">Details of accessories *</label>
                    <telerik:RadTextBox
                        ID="txtaccessories"
                        runat="server"
                        Width="100%" />

                    <label for="txtproof">Legal ID proof *</label>
                    <telerik:RadTextBox
                        ID="txtproof"
                        runat="server"
                        Width="100%" />

                    <label for="numVisitors">Number of Visitors</label>
                    <telerik:RadNumericTextBox
                        ID="numVisitors"
                        DateInput-ToolTip="Enter the number of visitors (if more than one)."
                        runat="server"
                        Width="100%">
                    </telerik:RadNumericTextBox>

                    <div class="form-actions" style="display: flex; gap: 10px;">
                        <telerik:RadButton ID="btnAddVisitorDetails" runat="server" Text="Add Visitor Details"
                            ButtonType="StandardButton" Width="150px" OnClick="btnAddVisitorDetails_Click1" />

                        <telerik:RadButton ID="btnSubmit" runat="server" Text="Submit"
                            ButtonType="StandardButton" Width="150px" OnClick="btnSubmit_Click" />

                        <telerik:RadButton ID="btnTerminate" runat="server" Text="Terminate"
                            ButtonType="StandardButton" Width="150px" OnClick="btnTerminate_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="form-section">
        <h4>Choose an Option</h4>
        <telerik:RadRadioButtonList runat="server" ID="rbStatusList" Style="display: flex; position: relative; gap: 20px" OnSelectedIndexChanged="rbStatusList_SelectedIndexChanged">
            <Items>
                <telerik:ButtonListItem Text="New" Value="N" />
                <telerik:ButtonListItem Text="Approved" Value="A" />
                <telerik:ButtonListItem Text="Dis-Approved" Value="D" />
                <telerik:ButtonListItem Text="Terminated" Value="T" />
            </Items>
        </telerik:RadRadioButtonList>

        <telerik:RadGrid ID="RadGrid1" runat="server"
            AllowPaging="True" PageSize="50"
            AllowSorting="True" AutoGenerateColumns="False"
            AllowFilteringByColumn="True"
            RenderMode="Lightweight" Skin="Material"
            OnNeedDataSource="RadGrid1_NeedDataSource"
            OnItemCommand="RadGrid1_ItemCommand"
            OnItemDataBound="RadGrid1_ItemDataBound"
            ShowGroupPanel="true">

            <ClientSettings AllowDragToGroup="true">
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing AllowColumnResize="true" EnableRealTimeResize="true" />
            </ClientSettings>

            <ExportSettings ExportOnlyData="True" />

            <MasterTableView CommandItemDisplay="Top"
                DataKeyNames="FormID"
                TableLayout="Fixed">

                <CommandItemTemplate>
                    <telerik:RadButton ID="rbtnExcel" Text="Export Excel" runat="server" CommandName="Excel" CssClass="btn btn-success" />
                    <telerik:RadButton ID="rbtnPdf" Text="Export PDF" runat="server" CommandName="PDF" CssClass="btn btn-danger" />
                </CommandItemTemplate>

                <Columns>


                    <telerik:GridTemplateColumn HeaderText="Form ID" UniqueName="FormIDLink"
                        HeaderStyle-Width="120px" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkFormId"
                                runat="server"
                                Text='<%# Eval("FormID") %>'
                                CommandName="OpenForm"
                                CommandArgument='<%# Eval("FormID") %>'
                                CssClass="form-link" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="VisitorName" HeaderText="Visitor Name" UniqueName="VisitorName"
                        HeaderStyle-Width="160px" ItemStyle-Width="160px" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="MobileNumber" HeaderText="Mobile Number" UniqueName="MobileNumber"
                        HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="VisitorCompany" HeaderText="Company" UniqueName="VisitorCompany"
                        HeaderStyle-Width="180px" ItemStyle-Width="180px" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="Purpose_of_Visit" HeaderText="Purpose" UniqueName="Purpose_of_Visit"
                        HeaderStyle-Width="160px" ItemStyle-Width="160px" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="Type_for_Visit" HeaderText="Type" UniqueName="Type_for_Visit"
                        HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="Person_to_Meet" HeaderText="Person to Meet" UniqueName="Person_to_Meet"
                        HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="Teamcode" HeaderText="Team" UniqueName="Teamcode"
                        HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="Visit_FromTime" HeaderText="From" UniqueName="Visit_FromTime"
                        DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false"
                        HeaderStyle-Width="160px" ItemStyle-Width="160px" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="Visit_ToTime" HeaderText="To" UniqueName="Visit_ToTime"
                        DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false"
                        HeaderStyle-Width="160px" ItemStyle-Width="160px" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="Expected_Duration" HeaderText="Duration" UniqueName="Expected_Duration"
                        HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="Details_of_accessories" HeaderText="Accessories" UniqueName="Details_of_accessories"
                        HeaderStyle-Width="200px" ItemStyle-Width="200px" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="Legal_ID_proof" HeaderText="ID Proof" UniqueName="Legal_ID_proof"
                        HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="No_of_Visitors" HeaderText="Visitors" UniqueName="No_of_Visitors"
                        HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="Access_Level" HeaderText="Access" UniqueName="Access_Level"
                        HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="Restricted_Area" HeaderText="Restricted Area" UniqueName="Restricted_Area"
                        HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="Entered_DateTime" HeaderText="Entered On" UniqueName="Entered_DateTime"
                        DataFormatString="{0:dd-MMM-yyyy hh:mm tt}" HtmlEncode="false"
                        HeaderStyle-Width="170px" ItemStyle-Width="170px" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="Status" HeaderText="Status" UniqueName="Status"
                        HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="Breakfast" HeaderText="Breakfast" UniqueName="Breakfast"
                        HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="Lunch" HeaderText="Lunch" UniqueName="Lunch"
                        HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="Dinner" HeaderText="Dinner" UniqueName="Dinner"
                        HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="CabRequired" HeaderText="Cab Req." UniqueName="CabRequired"
                        HeaderStyle-Width="90px" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="CabNotRequired" HeaderText="Cab Not Req." UniqueName="CabNotRequired"
                        HeaderStyle-Width="110px" ItemStyle-Width="110px" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="Validity" HeaderText="Validity" UniqueName="Validity"
                        HeaderStyle-Width="140px" ItemStyle-Width="90px" ItemStyle-Wrap="false" />
                    <telerik:GridBoundColumn DataField="Destination" HeaderText="Destination" UniqueName="Destination"
                        HeaderStyle-Width="140px" ItemStyle-Width="90px" ItemStyle-Wrap="false" />
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </div>
    <telerik:RadWindowManager ID="RadWindowManager2" runat="server">
        <Windows>
            <telerik:RadWindow
                ID="rwVisitorGrid"
                runat="server"
                Title="Visitor Details"
                Width="800px"
                Height="500px"
                Modal="true"
                VisibleStatusbar="false"
                Behaviors="Close, Move, Resize" VisibleOnPageLoad="false">
                <ContentTemplate>
                    <telerik:RadGrid ID="rgVisitorDetails" runat="server" AutoGenerateColumns="false"
                        AllowPaging="true" AllowSorting="true"
                        OnNeedDataSource="rgVisitorDetails_NeedDataSource"
                        OnInsertCommand="rgVisitorDetails_InsertCommand"
                        OnDeleteCommand="rgVisitorDetails_DeleteCommand">
                        <MasterTableView CommandItemDisplay="Top" DataKeyNames="FormID">


                            <Columns>
                                <telerik:GridBoundColumn DataField="FormID" HeaderText="FormID" ReadOnly="true" />
                                <telerik:GridBoundColumn DataField="VisitorName" HeaderText="Name" />
                                <telerik:GridButtonColumn CommandName="Delete" Text="Delete" />
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
</asp:Content>





