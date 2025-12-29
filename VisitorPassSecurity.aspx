<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="VisitorPassSecurity.aspx.cs" Inherits="VisitorPassSecurity" MaintainScrollPositionOnPostback="true" %>


<asp:Content ID="Content1" ContentPlaceHolderID="CPMain" runat="Server">
    <style>
        /* General layout */
        body {
            font-family: 'Segoe UI', Tahoma, sans-serif;
            background-color: #f9f9f9;
            margin: 0;
            padding: 20px;
            color: #333;
        }

        /* Section styling */
        .form-section {
            background-color: #fff;
            border-radius: 8px;
            padding: 20px;
            margin-bottom: 30px;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
        }

            .form-section h2 {
                font-size: 1.5em;
                margin-bottom: 20px;
                color: #005a9e;
            }

        /* Form rows and labels */
        .form-row,
        .form-grid-2 > div {
            margin-bottom: 15px;
        }


        label {
            display: block;
            font-weight: 500;
            margin-bottom: 6px;
            color: #444;
            font-size: 14px; /* Optional: increase label font size */
        }

        /* Grid layout for side-by-side fields */
        .form-grid-2 {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 20px;
        }
        /* Radio button group */
        .radio-group {
            display: flex;
            gap: 20px;
            margin-top: 10px;
        }
        /* Apply border to the entire RadGrid */
        .RadGrid_Material {
            border: 2px solid #4CAF50; /* green border */
            border-radius: 6px; /* optional rounded corners */
            padding: 5px; /* spacing inside the border */
        }

        .radio-option {
            display: flex;
            align-items: center;
            gap: 8px;
            font-weight: 500;
        }
        /* Textarea styling */
        textarea,
        input[type="text"],
        input[type="date"],
        input[type="time"] {
            width: 100%;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 6px;
            font-size: 1em;
        }
        /* Button styling */
        .form-actions {
            text-align: center;
            margin-top: 20px;
        }

        #photoBox img {
            width: 100px;
            height: 120px;
            object-fit: cover;
            border: 1px solid #333;
        }

        button,
        .telerik-button {
            background-color: #0078d4;
            color: white;
            border: none;
            padding: 12px 24px;
            font-size: 1em;
            border-radius: 6px;
            cursor: pointer;
            transition: background-color 0.3s ease;
        }

            button:hover,
            .telerik-button:hover {
                background-color: #005a9e;
            }

        @media print {
            @page {
                size: 4.094in 3in; /* Keep label size */
                margin: 0;
            }

            html, body {
                width: 4.094in;
                height: 3in;
                margin: 0;
                padding: 0;
            }

            /* Rotate content for landscape */
            body {
                transform: rotate(-90deg);
                transform-origin: top left;
                width: 3in;
                height: 4.094in;
                position: absolute;
                top: 4.094in; /* Adjust to fit */
                left: 0;
                font-size: 12px;
                line-height: 1.2;
            }

            .form-actions, button {
                display: none;
            }

            * {
                -webkit-print-color-adjust: exact;
                print-color-adjust: exact;
            }
        }

        .blue-link {
            color: blue !important;
            text-decoration: underline; /* optional */
        }

        .form-link {
            color: blue !important;
            text-decoration: underline; /* optional, makes it look like a hyperlink */
        }

        /* Entire grid background */
        #rgVisitorLog.RadGrid {
            background-color: #E6E6FA !important;
        }

        /* Header */
        #rgVisitorLog .rgHeader {
            background-color: #D8BFD8 !important;
            color: #333 !important;
            font-weight: bold;
        }

        .blue-value {
            color: blue;
        }

        #rgVisitorLog input[type="checkbox"] {
            accent-color: blue;
        }


        /* Rows */
        #rgVisitorLog .rgRow {
            background-color: #F3E8FF !important;
        }

        #rgVisitorLog .rgAltRow {
            background-color: #EDE7F6 !important;
        }

        /* Pager */
        #rgVisitorLog .rgPager {
            background-color: #E6E6FA !important;
            border-top: 1px solid #ccc !important;
        }
        /* Target header cells by their unique name */
        #rgVisitorLog th[uniqueName="ConfirmLogged"],
        #rgVisitorLog th[uniqueName="ConfirmExit"],
        #rgVisitorLog th[uniqueName="CapturePhoto"],
        #rgVisitorLog th[uniqueName="PrintBadge"] {
            color: blue !important;
        }

        /* Make link buttons inside grid blue */
        #rgVisitorLog .form-link,
        #rgVisitorLog .rgButton,
        #rgVisitorLog a {
            color: blue !important;
        }
    </style>
    <script>
        const visitorId = document.getElementById("visitorIdText").textContent;
        JsBarcode("#barcode", visitorId, {
            format: "CODE128",
            displayValue: false,
            width: 2,
            height: 40
        });

    </script>
    <script>
        function openCamera() {
            const video = document.createElement('video');
            video.setAttribute('autoplay', '');
            video.setAttribute('playsinline', '');
            video.style.width = '100%';
            video.style.maxHeight = '300px';

            navigator.mediaDevices.getUserMedia({ video: true })
                .then(stream => {
                    video.srcObject = stream;
                    document.body.appendChild(video); // ✅ fixed typo

                    // Optional: Add capture button
                    const captureBtn = document.createElement('button');
                    captureBtn.innerText = '📸 Take Snapshot';
                    captureBtn.onclick = () => {
                        const canvas = document.createElement('canvas');
                        canvas.width = video.videoWidth;
                        canvas.height = video.videoHeight;
                        canvas.getContext('2d').drawImage(video, 0, 0);
                        const photoData = canvas.toDataURL('image/jpeg');

                        // Stop camera
                        stream.getTracks().forEach(track => track.stop());
                        video.remove();
                        captureBtn.remove();

                        // Preview snapshot
                        const img = document.createElement('img');
                        img.src = photoData;
                        img.style.maxWidth = '100%';
                        document.body.appendChild(img);

                        // Or send to server
                        // fetch('/upload', { method: 'POST', body: JSON.stringify({ photo: photoData }) });
                    };
                    document.body.appendChild(captureBtn);
                })
                .catch(err => {
                    alert('Camera access denied or unavailable.');
                    console.error(err);
                });
        }

        function printDiv(divId, title) {
            let mywindow = window.open('', 'PRINT', 'height=650,width=900,top=100,left=150');

            mywindow.document.write('<html><head><title>' + title + '</title>');
            document.querySelectorAll('link[rel="stylesheet"], style').forEach(style => {
                mywindow.document.write(style.outerHTML);
            });
            mywindow.document.write('</head><body>');
            mywindow.document.write(document.getElementById(divId).innerHTML);
            mywindow.document.write('</body></html>');
            mywindow.document.close();

            mywindow.onload = function () {
                mywindow.focus();
                mywindow.print();
            };

            return true;
        }

        document.addEventListener("DOMContentLoaded", function () {
            document.querySelectorAll(".captureBtn").forEach(btn => {
                btn.addEventListener("click", function (e) {
                    e.preventDefault(); // prevent postback
                    openCamera();       // call your JS function
                });
            });
        });
    </script>




    <script>
        function printDiv(divId, title) {
            var content = document.getElementById(divId).innerHTML;
            var printWindow = window.open('', '', 'height=600,width=800');
            printWindow.document.write('<html><head><title>' + title + '</title>');
            printWindow.document.write('<style>');
            printWindow.document.write(`
                    body {font - family: Arial, sans-serif; text-align: center; }
                    .id-card {display: inline-block; text-align: center; }

 #photoBox { width: 120px; height: 150px; margin: 10px auto; }
    #photoBox img { width: 100%; height: 100%; object-fit: cover; display: block; margin: 0 auto; }

                    `);
            printWindow.document.write('</style></head><body>');
            printWindow.document.write(content);
            printWindow.document.write('</body></html>');

            printWindow.print();
        }
    </script>

    <style>
        .id-card {
            width: 350px;
            border: 2px solid #333;
            border-radius: 10px;
            padding: 15px;
            font-family: Arial, sans-serif;
            background: #f9f9f9;
            display: flex;
            flex-direction: column;
            align-items: center;
        }

        .header {
            font-size: 18px;
            font-weight: bold;
            color: #004080;
            text-align: center;
        }

        .sub-header {
            font-size: 12px;
            color: #666;
            margin-bottom: 10px;
            text-align: center;
        }

        .title {
            font-size: 16px;
            font-weight: bold;
            margin: 10px 0;
            text-transform: uppercase;
            text-align: center;
        }

        #photoBox {
            width: 200px;
            height: 250px;
            overflow: hidden;
            border: 1px solid #ccc;
            margin: 10px 0;
        }

            #photoBox img {
                width: 100%;
                height: 100%;
                object-fit: cover;
            }

        .details {
            width: 100%;
            margin: 15px 0;
            font-size: 14px;
            line-height: 1.5;
            text-align: center;
        }

        .barcode img {
            display: block;
            margin: 0 auto;
        }
    </style>




    <h1>Visitor Pass Security Screen </h1>
    <br />
    <telerik:RadRadioButtonList runat="server" ID="SATReceipt" Direction="Horizontal" Font-Bold="true" Font-Size="Medium">
        <Items>
            <telerik:ButtonListItem Text="Yet to Visit" Value="0" Selected="true" />
            <telerik:ButtonListItem Text="History" Value="1" />
        </Items>
    </telerik:RadRadioButtonList>
    <br />


    <telerik:RadGrid ID="rgVisitorLog" runat="server"
        AllowPaging="True" PageSize="50"
        AllowSorting="True" AutoGenerateColumns="False"
        AllowFilteringByColumn="True"
        RenderMode="Lightweight" Skin="Material"
        OnNeedDataSource="rgVisitorLog_NeedDataSource"
        OnItemCommand="rgVisitorLog_ItemCommand"
        OnItemDataBound="rgVisitorLog_ItemDataBound"
        AllowFiltering="true"
        ShowGroupPanel="false">

        <ClientSettings AllowDragToGroup="true">
            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
            <Resizing AllowColumnResize="true" EnableRealTimeResize="true" />
        </ClientSettings>

        <ExportSettings ExportOnlyData="True" />

        <MasterTableView CommandItemDisplay="None"
            DataKeyNames="FormID"
            TableLayout="Fixed">
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
                <telerik:GridBoundColumn DataField="VisitorCompany" HeaderText="Company" UniqueName="VisitorCompany"
                    HeaderStyle-Width="180px" ItemStyle-Width="180px" ItemStyle-Wrap="false" />
                <telerik:GridBoundColumn DataField="Purpose_of_Visit" HeaderText="Purpose" UniqueName="Purpose_of_Visit"
                    HeaderStyle-Width="160px" ItemStyle-Width="160px" ItemStyle-Wrap="false" />
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
                <telerik:GridBoundColumn DataField="Validity" HeaderText="Validity" UniqueName="Validity"
                    HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-Wrap="false" />

                <telerik:GridTemplateColumn DataField="Entry_Flag" HeaderText="ConfirmLogged" ReadOnly="True" UniqueName="ConfirmLogged">
                    <ItemTemplate>
                        <asp:CheckBox ID="ConfirmLogged" runat="server"
                            AutoPostBack="True"
                            OnCheckedChanged="ConfirmLogged_CheckedChanged" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn DataField="ConfirmExit" HeaderText="ConfirmExit" ReadOnly="True" UniqueName="ConfirmExit">
                    <ItemTemplate>
                        <asp:CheckBox ID="ConfirmExit" runat="server" OnCheckedChanged="ConfirmExit_CheckedChanged"
                            AutoPostBack="True" HeaderStyle-ForeColor="Blue" />
                    </ItemTemplate>
                </telerik:GridTemplateColumn>


                <telerik:GridTemplateColumn HeaderText="Capture Image" UniqueName="Capture"
                    HeaderStyle-Width="120px" ItemStyle-Width="120px">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkCapture"
                            runat="server"
                            Text="Capture Image"
                            CommandName="Capture"
                            CommandArgument='<%# Eval("FormID") %>'
                            ForeColor="Blue"
                            CssClass="form-link" />

                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridTemplateColumn HeaderText="Print Badge">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkPrintBadge" runat="server"
                            Text="Print Badge"
                            CommandName="PrintBadge"
                            ForeColor="Blue"
                            OnClientClick="printDiv('PrintID', 'My Print Title'); return false;">
                        </asp:LinkButton>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

            </Columns>
        </MasterTableView>

        <PagerStyle Mode="NextPrevAndNumeric" />
    </telerik:RadGrid>




    <div id="PrintID" class="id-card">
        <div class="header">Mini-Circuits India-Urjita Pvt. Ltd</div>
        <div class="sub-header">AS 9100 CERTIFIED</div>
        <div class="title">Visitor Pass</div>
        <div id="photoBox">Photo</div>
        <div class="details">
            Visitor ID : <span id="visitorIdText">VP25001</span><br>
            Name : ISHWARYA<br>
            Purpose : Vendor Visit<br>
            Valid Until : 24-Dec-2025
        </div>
        <svg id="barcode"></svg>
    </div>

    <input type="file" id="photoInput" accept="image/*" />

    <script src="https://cdn.jsdelivr.net/npm/jsbarcode@3.11.5/dist/JsBarcode.all.min.js"></script>
    <script>
        const photoInput = document.getElementById('photoInput');
        const photoBox = document.getElementById('photoBox');

        // Upload preview
        photoInput.addEventListener('change', function (event) {
            const file = event.target.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    photoBox.innerHTML = `<img src="${e.target.result}" alt="Visitor Photo">`;
                };
                reader.readAsDataURL(file);
            }
        });

        // Camera capture
        function openCamera() {
            const video = document.createElement('video');
            video.setAttribute('autoplay', '');
            video.setAttribute('playsinline', '');
            video.style.width = '100%';
            video.style.maxHeight = '300px';

            navigator.mediaDevices.getUserMedia({ video: true })
                .then(stream => {
                    video.srcObject = stream;
                    photoBox.innerHTML = ''; // clear old content
                    photoBox.appendChild(video);

                    const captureBtn = document.createElement('button');
                    captureBtn.innerText = '📸 Take Snapshot';
                    captureBtn.onclick = () => {
                        const canvas = document.createElement('canvas');
                        canvas.width = video.videoWidth;
                        canvas.height = video.videoHeight;
                        canvas.getContext('2d').drawImage(video, 0, 0);
                        const photoData = canvas.toDataURL('image/jpeg');

                        // Stop camera
                        stream.getTracks().forEach(track => track.stop());

                        // Replace video with captured image
                        photoBox.innerHTML = `<img src="${photoData}" alt="Visitor Photo">`;
                    };
                    photoBox.appendChild(captureBtn);
                })
                .catch(err => {
                    alert('Camera access denied or unavailable.');
                    console.error(err);
                });
        }

        // Barcode generation
        JsBarcode("#barcode", document.getElementById("visitorIdText").textContent.trim(), {
            format: "CODE128",
            displayValue: false,
            width: 2,
            height: 40
        });
    </script>

</asp:Content>
