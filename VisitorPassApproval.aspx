<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="VisitorPassApproval.aspx.cs" Inherits="VisitorPassApproval" MaintainScrollPositionOnPostback="true" %>

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

        body {
            font-family: Arial, sans-serif;
            margin: 20px;
            background-color: #f5f5f5;
        }

        .container {
            max-width: 700px;
            margin: auto;
            background: #fff;
            padding: 20px;
            border-radius: 40px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
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
                color: #0078d7;
            }

        label {
            display: block;
            margin: 8px 0 4px;
            font-weight: bold;
        }

        /* Telerik input styling override */
        input[type="text"],
        input[type="date"],
        input[type="time"],
        textarea,
        select,
        .radTextBox_Default,
        .RadInput_Default input {
            width: 100%;
            padding: 8px;
            margin-bottom: 10px;
            border: 1px solid #ccc !important;
            border-radius: 4px;
            font-size: 14px;
        }

        /* Radio buttons */
        .radio-group {
            display: flex;
            gap: 16px;
            align-items: center;
            margin-top: 10px;
        }

        .RadTreeView .rtTemplate input[type="radio"] + label {
            color: #000 !important; /* Black text */
            background-color: transparent; /* Keep background clean */
        }

        .RadTreeView .rtIn {
            color: #000 !important; /* Black text */
        }

        .radio-option {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            font-weight: normal;
        }

        /* Grid layout for signature/date */
        .form-grid-2 {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 16px;
        }

        /* Form row spacing */
        .form-row {
            margin-bottom: 12px;
        }

        .RadTreeView .rtTemplate label,
        .RadTreeView .rtIn {
            color: #000 !important;
        }

        .readonly-checkbox {
            pointer-events: none; /* disables mouse interaction */
            opacity: 1; /* keeps normal look (remove grayed-out effect) */
        }

        /* Button styling */
        button,
        .RadButton_Default {
            background: #0078d7;
            color: #fff;
            border: none;
            padding: 10px 20px;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
        }

            button:hover,
            .RadButton_Default:hover {
                background: #005a9e;
            }

        /* Form actions alignment */
        .form-actions {
            text-align: right;
            margin-top: 20px;
        }

        /* Responsive layout */
        @media (max-width: 768px) {
            .form-grid-2 {
                grid-template-columns: 1fr;
            }

            .radio-group {
                flex-direction: column;
                align-items: flex-start;
            }
        }


        /* Common style for read-only text fields (ASP.NET & Telerik) */
        .readonly-field,
        .readonly-field input {
            background-color: #eef0ff; /* pick your color */
            color: #333;
            opacity: 1; /* ensure full visibility */
            cursor: default;
        }

        /* ASP.NET TextBox read-only (HTML input[readonly]) */
        input[type="text"][readonly],
        textarea[readonly] {
            background-color: #eef0ff;
            color: #333;
            opacity: 1;
        }

        /* Prevent Telerik disabled inputs from appearing too dim */
        .RadInput .riDisabled input,
        .RadInput .riDisabled textarea {
            background-color: #eef0ff !important;
            color: #333 !important;
            opacity: 1 !important;
        }

        /* If tpRequestTime is a RadTimePicker or similar:
   remove default dimming for disabled state */
        .RadTimePicker,
        .RadDatePicker,
        .RadDateTimePicker {
            /* scope overrides inside disabled state */
        }

            .RadTimePicker .rcDisabled,
            .RadDatePicker .rcDisabled,
            .RadDateTimePicker .rcDisabled {
                opacity: 1 !important;
            }

            .RadTimePicker .riDisabled input,
            .RadDateTimePicker .riDisabled input {
                background-color: #eef0ff !important;
                color: #333 !important;
                opacity: 1 !important;
            }

        /* Optional: prevent pointer-style on disabled inputs but keep full visibility */
        input:disabled, textarea:disabled, .RadInput .riDisabled input {
            cursor: not-allowed;
        }


        /* Apply to all checkboxes you want read-only */
        .readonly-checkbox {
            pointer-events: none; /* disables click */
            cursor: default; /* normal cursor */
            opacity: 1; /* ensure full visibility */
        }

        
        /* Example CSS */
        .grid-header {
            background-color: lavender !important; /* Lavender color */
            color: #000; /* Optional: Adjust text color for better contrast */
        }


        /* For Telerik RadCheckBox specifically */
        .RadCheckBox.readonly-checkbox .rcText,
        .RadCheckBox.readonly-checkbox input[type="checkbox"] {
            pointer-events: none;
            opacity: 1 !important;
    </style>

    <script>
        function showList(status) {
            document.getElementById("pendingList").style.display = (status === "pending") ? "block" : "none";
            document.getElementById("approvedList").style.display = (status === "approved") ? "block" : "none";
            document.getElementById("rejectedList").style.display = (status === "rejected") ? "block" : "none";
        }
        function acbPONo_ShowAll_OnClientLoad(sender, args) {
            // fire a request for all records when focusing the input
            $telerik.$(sender.get_inputElement()).on("focus", function (e) {
                var showAllRecords = true;
                console.log("focused");

                if (!sender.__isAddingEntry) {
                    sender._requestItems("ShowAllRecords", showAllRecords)
                }
                sender.__isAddingEntry = false;
            });

            // prevent request fired when clicking an item of the dropdown
            sender.__isAddingEntry = false;
            $telerik.$(sender.get_dropDownElement()).find(".racList").on("mousedown", function () {
                sender.__isAddingEntry = true;
            });
        }
    </script>
    <telerik:RadWindowManager runat="server" ID="RadWindowManager2">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" runat="server" VisibleOnPageLoad="false" Modal="true" Behaviors="Close" Width="400px" Height="250px">
                <ContentTemplate>
                    <div>
                        <telerik:RadLabel runat="server" ID="LblEmpty" ForeColor="Red"></telerik:RadLabel>
                        <telerik:RadLabel runat="server" ID="TxtValidatePin" Font-Bold="true" Text="Enter Pin_No"></telerik:RadLabel>
                        <br />
                        <br />
                        <telerik:RadTextBox ID="TxtPassword" runat="server" EmptyMessage="PIN No." TextMode="Password"></telerik:RadTextBox>
                        <br />
                        <br />
                        <telerik:RadButton runat="server" ID="BtnValidatePin" Text="Verify" OnClick="BtnValidatePin_Click"></telerik:RadButton>
                    </div>
                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>



    <asp:HiddenField ID="hiddenSelected" runat="server" ClientIDMode="Static" />


    <%--  <asp:Label ID="Label1" runat="server" ClientIDMode="Static" Text="Selected: None" />--%>
    <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />


    <telerik:RadTreeView ID="rtvModules" runat="server">
        <Nodes>
            <telerik:RadTreeNode Text="Frontend" Value="frontend" Expanded="true">
                <Nodes>
                    <telerik:RadTreeNode Text="React" Value="react" />
                    <telerik:RadTreeNode Text="Vue" Value="vue" />
                </Nodes>
            </telerik:RadTreeNode>
            <telerik:RadTreeNode Text="Backend" CssClass="treestyle" Value="backend">
                <Nodes>
                    <telerik:RadTreeNode Text="Node.js" Value="node" />
                    <telerik:RadTreeNode Text="Python" Value="python" />
                </Nodes>
            </telerik:RadTreeNode>
        </Nodes>


        <NodeTemplate>
            <span style="display: flex; align-items: center; gap: .5rem;">
                <input type="radio" name="TreeRadio" id='<%# Eval("Id") %>' value='<%# Eval("Id") %>' />

                <label for='<%# Eval("Id") %>'>
                    <%# Eval("Text") + (string.IsNullOrEmpty(Eval("Status") as string) ? "" : " (" + Eval("Status") + ")") %>
                </label>

            </span>
        </NodeTemplate>

    </telerik:RadTreeView>


    <script>
        // Map status to colors
        const statusColor = {
            'Approved': 'green',
            'Dis-Approved': 'red',
            'Yet to Approve': 'goldenrod'
        };

        document.addEventListener('change', function (e) {
            if (e.target && e.target.name === 'TreeRadio') {
                const value = e.target.value;
                document.getElementById('lblSelected').textContent = 'Selected: ' + value;
                document.getElementById('hiddenSelected').value = value;

                // Change font color based on selected status
                const color = statusColor[value] || 'black';
                document.getElementById('lblSelected').style.color = color;
            }
        });
    </script>
    <div style="display: flex; justify-content: center;">

        <div style="left: -93px; top: -136px; position: relative">
            <div class="container" style="position: relative; left: 297px; top: -9px;">
                <!-- Header Section -->
                <header>
                    <div class="header-left">
                        <h1>Visitor Pass Approval Form</h1>
                    </div>
                    <div class="header-right">
                        <p id="approvalDatetime"></p>
                    </div>
                </header>
            </div>
            <!-- Request Metadata -->
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="Metro" EnableShadow="true">
            </telerik:RadWindowManager>
            <br />
            <div class="container">
                <h2>Request Metadata</h2>

                <label for="purpose">Form ID</label>


                <telerik:RadAutoCompleteBox RenderMode="Lightweight" ID="RadAutoCompleteBox1" runat="server" Width="190" DropDownHeight="150" Filter="StartsWith"
                    EmptyMessage="" InputType="Text" AllowCustomEntry="true" AutoPostBack="true" DataSourceID="SqlDataSourcePONo" DataTextField="formID" DataValueField="formID"
                    Font-Bold="true" OnTextChanged="acbSCD_TextChanged">
                    <TextSettings SelectionMode="Single" />
                </telerik:RadAutoCompleteBox>
                <asp:SqlDataSource ID="SqlDataSourcePONo" runat="server"></asp:SqlDataSource>
                <label for="purpose">VisitorName</label>
                <telerik:RadTextBox ID="txtFullName" runat="server" ReadOnly="true" Width="100%" />
                <label for="purpose">Mobile Number</label>
                <telerik:RadTextBox ID="txtMobileNumber" runat="server" ReadOnly="true" Width="100%" />
                <label for="purpose">VisitorCompany</label>
                <telerik:RadTextBox ID="txtCompany" runat="server" ReadOnly="true" Width="100%" />

                <label for="purpose">Date of Request</label>
                <telerik:RadTextBox ID="txtRequestDate" runat="server" ReadOnly="true" Width="100%" />
                <label for="purpose">Purpose of Visit</label>
                <telerik:RadTextBox ID="tpRequestTime" runat="server" ReadOnly="true" Width="100%" />
                <label for="purpose">Requested By</label>
                <telerik:RadTextBox ID="txtRequestedBy" runat="server" ReadOnly="true" Width="100%" />

                <label for="RadTextBox3">Meals Option</label>
                <telerik:RadCheckBox ID="chkBreakfast" runat="server" Text="Breakfast" CssClass="readonly-checkbox" />
                <telerik:RadCheckBox ID="chkDinner" runat="server" Text="Dinner" CssClass="readonly-checkbox" />
                <telerik:RadCheckBox ID="chkLunch" runat="server" Text="Lunch" CssClass="readonly-checkbox" />


                <label for="RadTextBox4">Cab Arrangements </label>
                <telerik:RadCheckBox ID="RadCheckBox1" runat="server" Text="Required" CssClass="readonly-checkbox" />
                <telerik:RadCheckBox ID="RadCheckBox2" runat="server" Text="Not Required" CssClass="readonly-checkbox" />

                <div>
                    <label for="txtDestination">Destination:</label>
                    <telerik:RadTextBox ID="txtDestination" runat="server" Width="100%" ReadOnly="true" />
                </div>

            </div>
        </div>

        <div style="left: 100px; position: relative">
            <div class="container">
                <!-- Approval Workflow -->
                <div>
                    <label for="purpose">No_of_Visitors</label>
                    <telerik:RadTextBox ID="numVisitors" runat="server" ReadOnly="true" Width="100%" />
                    <label for="purpose">From Date</label>
                    <telerik:RadTextBox ID="visitdatefrom" runat="server" ReadOnly="true" Width="100%" />
                    <label for="purpose">To Date</label>
                    <telerik:RadTextBox ID="visitdateto" runat="server" ReadOnly="true" Width="100%" />

                </div>

                <div class="form-section">
                    <h2>Approval Workflow</h2>
                    <!-- Status Indicator -->
                    <label for="purpose">Status Indicator</label>
                    <telerik:RadTextBox ID="txtStatus" runat="server" ReadOnly="true" Text="Waiting for approval" Width="100%" />
                    <!-- Admin Notes -->
                    <label for="purpose">Admin Notes (Optional)</label>
                    <telerik:RadTextBox ID="txtAdminNotes" runat="server" TextMode="MultiLine" Rows="2" Width="100%" />
                </div>
            </div>
            <br />
            <div class="container">
                <!-- Approval Section -->
                <div class="form-section">
                    <h2>Approval Section</h2>

                    <!-- Host Approval radios -->

                    <div class="form-row">
                        <label>Host Approval</label>
                        <div>
                            <telerik:RadRadioButtonList runat="server" ID="RadioOpen" Style="display: flex; justify-content: center; position: relative; gap: 20px">
                                <Items>
                                    <telerik:ButtonListItem Text="Approved" Selected="true" Value="1" />
                                    <telerik:ButtonListItem Text="Rejected" Value="2" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </div>

                    </div>

                    <div class="form-grid-2">
                        <div>
                            <label for="signature">Signature</label>
                            <telerik:RadTextBox runat="server" ID="signature" AutoPostBack="true" EmptyMessage="Approver Signature" OnTextChanged="signature_TextChanged"></telerik:RadTextBox>
                        </div>
                        <div>
                            <label for="approvalDate">Date</label>
                            <input type="date" id="approvalDate" name="approvalDate" disabled>
                        </div>
                        <script>
                            // Get today's date in YYYY-MM-DD format
                            const today = new Date().toISOString().split('T')[0];

                            // Bind it to the input field
                            document.getElementById("approvalDate").value = today;
                        </script>

                    </div>
                </div>
                <!-- Submit Button -->

            </div>
        </div>
    </div>
    <div class="form-section">
        <h4>Choose an Option</h4>
        <telerik:RadRadioButtonList runat="server" ID="rbStatusList" Style="display: flex; position: relative; gap: 20px" OnSelectedIndexChanged="rbStatusList_SelectedIndexChanged">
            <Items>
                <telerik:ButtonListItem Text="Yet to Approve" Value="N" />
                <telerik:ButtonListItem Text="Approved" Value="A" />
                <telerik:ButtonListItem Text="Dis-Approved" Value="D" />
            </Items>
        </telerik:RadRadioButtonList>
    </div>

    <div>
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
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </div>
    <telerik:RadWindowManager ID="RadWindowManager3" runat="server">
        <Windows>
            <telerik:RadWindow ID="RadWindow2" runat="server" Modal="false" Width="392px" Height="520px" Font-Bold="true" VisibleOnPageLoad="false" InitialBehaviors="Pin" Left="1500" Top="250px">

                <ContentTemplate>
                    <telerik:RadGrid ID="RadGrid2" runat="server" AllowPaging="True" PageSize="10"
                        AllowSorting="True" AutoGenerateColumns="False"
                        AllowFilteringByColumn="false"
                        OnNeedDataSource="RadGrid2_NeedDataSource"
                        OnItemDataBound="RadGrid2_ItemDataBound"
                        GroupingSettings-CaseSensitive="false">
                        <GroupingSettings CollapseAllTooltip="Collapse all groups" />
                        <ClientSettings AllowDragToGroup="true">
                        </ClientSettings>
                        <ExportSettings ExportOnlyData="True">
                        </ExportSettings>
                        <MasterTableView CommandItemDisplay="Top" EditMode="Batch" AllowMultiColumnSorting="true" DataKeyNames="slno">
                            <BatchEditingSettings EditType="Cell" HighlightDeletedRows="true" />
                            <CommandItemStyle Font-Bold="true" Font-Size="9px" HorizontalAlign="Left" />
                            <BatchEditingSettings EditType="Cell" HighlightDeletedRows="true" />
                            <CommandItemSettings ShowAddNewRecordButton="false" ShowSaveChangesButton="false" ShowCancelChangesButton="false" ShowRefreshButton="false" />
                            <CommandItemStyle Font-Bold="true" Font-Size="12px" />
                            <CommandItemTemplate>
                            </CommandItemTemplate>
                            <Columns>
                                <telerik:GridBoundColumn DataField="FormID" HeaderText="FormID" UniqueName="FormID" HeaderStyle-Width="80px" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VisitorName" HeaderText="VisitorName" UniqueName="VisitorName" HeaderStyle-Width="80px" ReadOnly="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="slno" HeaderText="slno" UniqueName="slno" HeaderStyle-Width="80px" ReadOnly="true" Display="false">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <br />
                    <br />
                </ContentTemplate>
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>



    <script>
        // Auto-fill current date
        document.getElementById("approvalDatetime").textContent =
            new Date().toLocaleDateString();
    </script>
</asp:Content>
