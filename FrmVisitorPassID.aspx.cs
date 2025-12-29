using System;
using System.Drawing;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Data.SqlClient;
using System.Data;
using Convert = System.Convert;
using System.Drawing.Imaging;

public partial class VisitorPass : System.Web.UI.Page
{
    public static DataTable dtTable;
    public static string connectionString;
    public SqlConnection SqlConnection = new SqlConnection();
    public SqlDataAdapter SqlDataAdapter = new SqlDataAdapter();
    public SqlCommand SqlCommand = new SqlCommand();
    public SqlDataReader SqlDataRdr;
    public string strHostName;

    protected void Page_Load(object sender, EventArgs e)
    {
        //*********** CODE FOR MIGRATION *************
        string strCode = Request.QueryString["Code"];
        strHostName = Request.QueryString["HostName"];


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

        if (Session["EmpCode"] == null && Session["EmpName"] == null)
        {
            Session["MainMsg"] = "Session Expired";
            Response.Redirect("~/Default.aspx");
            return;
        }
        //***** END ****** CODE FOR MIGRATION **********
        GlobalClassCS.DBConn = (string)Session["MyConn"];
        connectionString = GlobalClassCS.DBConn;
        SqlConnection.ConnectionString = connectionString;


        // Example dynamic data
        string visitorId = "VP25001";
        string name = "CHRISTHU RAJA";
        string purpose = "SUPPLIER VISIT";
        string validUntil = "03-Dec-2025";
        string barcode = "123456789012";

        // Resolve image inside IIS site folder
        string imagePath = Server.MapPath("~/ImagePath/CRS.png");

        string imageZPL = ConvertImageToZPL(imagePath);

        string zpl = "^XA" +
                     "^PW472^LL815" +
                     "^FO20,26^GB434,765,5,,0^FS" +
                     "^FT41,103^A0N,42,25^FD Mini-Circuits India-Urjita Pvt. Ltd ^FS" +
                     "^FT156,143^A0N,34,18^FD AS 9100 CERTIFIED ^FS" +
                     "^FT131,198^A0N,34,33^FD VISITOR PASS ^FS" +
                     "^FO131,227" + imageZPL + "^FS" +
                     "^FT63,529^A0N,25,25^FD Visitor ID : ^FS" +
                     "^FT192,529^A0N,25,25^FD " + visitorId + " ^FS" +
                     "^FT102,569^A0N,25,25^FD Name : ^FS" +
                     "^FT190,573^A0N,25,25^FD " + name + " ^FS" +
                     "^FT76,618^A0N,25,25^FD Purpose : ^FS" +
                     "^FT192,618^A0N,25,25^FD " + purpose + " ^FS" +
                     "^FT45,667^A0N,25,25^FD Valid Until : ^FS" +
                     "^FT190,667^A0N,25,25^FD " + validUntil + " ^FS" +
                     "^BY4,3,55^FT34,769^BCN,,N,N^FD>;" + barcode + "^FS" +
                     "^XZ";
        //// Validate the file exists
        //if (!File.Exists(imagePath))
        //{
        //    // Handle missing file gracefully
        //    throw new FileNotFoundException("Image not found at " + imagePath);
        //}

        //// Convert image to ZPL (^GFA with Z64), size based on 0.993 in × 1.362 in at 203 DPI
        //const int targetWidthDots = 202; // 0.993 * 203 ≈ 202
        //const int targetHeightDots = 276; // 1.362 * 203 ≈ 276

        //string z64ImageBlock = ConvertImageToZ64(imagePath, targetWidthDots, targetHeightDots);

        //// Build ZPL
        //string zpl = BuildZPL(visitorId, name, purpose, validUntil, barcode, z64ImageBlock);

        string FinalZplCode = zpl;

        //string printerName = "ZDesigner ZD230-203dpi ZPL (Copy 1)";

        //bool printResult = MyUtilityCS.RawPrinterHelper.SendStringToPrinter(printerName, zpl);
    }



    public string ConvertImageToZPL(string imagePath)
    {
        Bitmap bmp = new Bitmap(imagePath);
        bmp = new Bitmap(bmp, new Size(202, 276)); // Resize to label size

        // Convert to monochrome
        for (int y = 0; y < bmp.Height; y++)
        {
            for (int x = 0; x < bmp.Width; x++)
            {
                Color pixel = bmp.GetPixel(x, y);
                int gray = (pixel.R + pixel.G + pixel.B) / 3;
                bmp.SetPixel(x, y, gray > 128 ? Color.White : Color.Black);
            }
        }

        // Convert to ZPL GRF format
        MemoryStream ms = new MemoryStream();
        bmp.Save(ms, ImageFormat.Bmp);
        byte[] imageBytes = ms.ToArray();
        //string Data1 = "2189";
        //string Data2 = "7196";
        //string Date3 = "28";
        string zplImage = "^GFA,2189,7196,28," + BitConverter.ToString(imageBytes).Replace("-", "");
        return zplImage;
    }



    private static string BuildZPL(string visitorId, string name, string purpose, string validUntil, string barcode, string z64ImageBlock)
    {
        return @"^XA
                ^MMT
                ^PW472
                ^LL815
                ^LS0
                ^FO20,26^GB434,765,5,,0^FS
                ^FT41,103^A0N,42,25^FH\^CI28^FDMini-Circuits India-Urjita Pvt. Ltd^FS^CI27
                ^FT156,143^A0N,34,18^FH\^CI28^FDAS 9100 CERTIFIED^FS^CI27
                ^FT131,198^A0N,34,33^FH\^CI28^FDVISITOR PASS^FS^CI27
                ^FO131,227" + z64ImageBlock + @"
                ^FT63,529^A0N,25,25^FH\^CI28^FDVisitor ID :^FS^CI27
                ^FT192,529^A0N,25,25^FH\^CI28^FD" + visitorId + @"^FS^CI27
                ^FT102,569^A0N,25,25^FH\^CI28^FDName :^FS^CI27
                ^FT190,573^A0N,25,25^FH\^CI28^FD" + name + @"^FS^CI27
                ^FT76,618^A0N,25,25^FH\^CI28^FDPurpose :^FS^CI27
                ^FT192,618^A0N,25,25^FH\^CI28^FD" + purpose + @"^FS^CI27
                ^FT45,667^A0N,25,25^FH\^CI28^FDValid Until :^FS^CI27
                ^FT190,667^A0N,25,25^FH\^CI28^FD" + validUntil + @"^FS^CI27
                ^BY4,3,55^FT34,769^BCN,,N,N
                ^FH\^FD>;" + barcode + @"^FS
                ^PQ1,0,1,Y
                ^XZ";
    }

    private static string ConvertImageToZ64(string imagePath, int targetWidth, int targetHeight)
    {
        using (System.Drawing.Image img = System.Drawing.Image.FromFile(imagePath))
        using (Bitmap resizedBmp = new Bitmap(img, new Size(targetWidth, targetHeight)))
        {
            int bytesPerRow = (resizedBmp.Width + 7) / 8;
            int totalRows = resizedBmp.Height;
            byte[] rawData = new byte[bytesPerRow * totalRows];

            for (int y = 0; y < resizedBmp.Height; y++)
            {
                for (int x = 0; x < resizedBmp.Width; x++)
                {
                    Color pixel = resizedBmp.GetPixel(x, y);
                    // Convert to black or white based on brightness threshold
                    bool isBlack = pixel.GetBrightness() < 0.6; // Adjust threshold if needed
                    if (isBlack)
                    {
                        rawData[y * bytesPerRow + (x / 8)] |= (byte)(128 >> (x % 8));
                    }
                }
            }

            // Compress using Zlib
            byte[] compressed;
            using (MemoryStream ms = new MemoryStream())
            {
                using (DeflateStream ds = new DeflateStream(ms, CompressionMode.Compress))
                {
                    ds.Write(rawData, 0, rawData.Length);
                }
                compressed = ms.ToArray();
            }

            string Data1 = "2189";
            string Data2 = "7196";
            string Data3 = "28";
            //string Data4 = "5819";

            string z64Data = Convert.ToBase64String(compressed);
            return "^GFA," + Data1 + "," + Data2 + "," + Data3 + ",:Z64:" + z64Data + ":5819";
        }
    }


}