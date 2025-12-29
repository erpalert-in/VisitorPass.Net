<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="VisitorCount.aspx.cs" Inherits="VisitorCount" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content" ContentPlaceHolderID="CPMain" runat="Server">

    <style>
        .RadInput_Default input[type="text"],
        .RadInput_Default textarea {
            background-color: #E6E6FA !important; /* Mild sandal color */
        }

        .form-section .RadInput_Default input[type="text"] {
            background-color: #E6E6FA !important;
            font-size: large;
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
            font-size: 20px;
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
            font-size: 20px;
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


        /* Optional: prevent pointer-style on disabled inputs but keep full visibility */
        input:disabled, textarea:disabled, .RadInput .riDisabled input {
            cursor: not-allowed;
        }

        .lblHeader {
            display: flex;
            align-items: center;
            justify-content: center;
            margin-top: 20px;
        }

            .lblHeader .icon {
                width: 40px;
                height: 40px;
                margin-right: 10px;
            }

        .visitor-label {
            font-size: 20px;
            font-weight: bold;
            color: #ffffff;
            background-color: #0078D7; /* Microsoft blue */
            padding: 8px 15px;
            border-radius: 8px;
            border: 2px solid #005A9E;
            box-shadow: 2px 2px 6px rgba(0,0,0,0.2);
        }
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

    <div class="lblHeader">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:Timer ID="Timer1" runat="server" Interval="10000" OnTick="Timer1_Tick"></asp:Timer>
    </div>


    <div class="container">

        <div class="form-section">
            <h2>Number of visitors present at the office right now.</h2>
        </div>
    </div>


    <div class="lblHeader" style="display: flex; align-items: center; justify-content: center;">
        <img src="Designer.png" alt="Designer" style="margin-right: 10px;" />
        <asp:Label ID="lblVisitorCount" runat="server" Font-Bold="true" Font-Size="200px"></asp:Label>
    </div>


    <div class="container">
        <div class="form-section">

            <telerik:RadGrid ID="RadGrid1" runat="server"
                AllowPaging="True" PageSize="50"
                AllowSorting="True" AutoGenerateColumns="False"
                AllowFilteringByColumn="false"
                RenderMode="Lightweight" Skin="Default"
                OnNeedDataSource="RadGrid1_NeedDataSource"
                OnItemCommand="RadGrid1_ItemCommand"
                OnItemDataBound="RadGrid1_ItemDataBound"
                ShowGroupPanel="false">

                <ClientSettings AllowDragToGroup="false">
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing AllowColumnResize="true" EnableRealTimeResize="true" />
                </ClientSettings>

                <ExportSettings ExportOnlyData="True" />

                <MasterTableView
                    DataKeyNames="FormID"
                    TableLayout="Fixed"
                    CommandItemDisplay="None">

                    <Columns>
                        <telerik:GridBoundColumn DataField="FormID" HeaderText="FormID" UniqueName="FormID"
                            HeaderStyle-Width="160px" ItemStyle-Width="160px" ItemStyle-Wrap="false" />
                        <telerik:GridBoundColumn DataField="VisitorName" HeaderText="Visitor Name" UniqueName="VisitorName"
                            HeaderStyle-Width="160px" ItemStyle-Width="160px" ItemStyle-Wrap="false" />
                        <telerik:GridBoundColumn DataField="VisitorCompany" HeaderText="Company" UniqueName="VisitorCompany"
                            HeaderStyle-Width="180px" ItemStyle-Width="180px" ItemStyle-Wrap="false" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>




        </div>
    </div>

</asp:Content>
