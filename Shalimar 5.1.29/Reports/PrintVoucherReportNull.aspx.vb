Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine

Public Class PrintVoucherReportNull
    Inherits Web.UI.Page
    Public DateFormat As String
    Private CurrencySymbol As String
    Private objConnection As Object

    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Dim rptDoc As ReportDocument

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnWord As System.Web.UI.WebControls.Button
    Protected WithEvents btnPDF As System.Web.UI.WebControls.Button
    'Protected WithEvents CRV As CrystalDecisions.Web.CrystalReportViewer
    'Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        'Response.Write(Request.QueryString("from"))
        'Response.Write(Request.QueryString("to"))
        Dim fileName As String = ""

        rptDoc = CreateReport()

        fileName = Server.MapPath("..\TempDocument\") + Request.QueryString("TSID") + ".pdf"
        rptDoc = CreateReport()
        If ((System.IO.File.Exists(fileName))) Then
            System.IO.File.Delete(fileName)

        End If

        rptDoc = CreateReport()


        rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, fileName)


        rptDoc.Close()
        rptDoc.Dispose()
        GC.Collect()

        Response.Redirect("loadPDFVouher.aspx?TSID=" + Request.QueryString("TSID") + "", True)



        If Not IsPostBack Then
            rptDoc = CreateReport()
            Cache.Insert("Print Voucher Report", rptDoc, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(2))
            '
        Else
            If Not Cache("Print Voucher Report") Is Nothing Then
                rptDoc = Cache("Print Voucher Report")
            Else
                rptDoc = CreateReport()
                Cache.Insert("Print Voucher Report", rptDoc, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(2))
            End If
        End If
        cReportUtility.applyRptPaperInfo(rptDoc, CRV, , PaperOrientation.Landscape)

        If Request.QueryString("status") = "1" Then
            CRV.HasPrintButton = False
        Else
            CRV.HasPrintButton = True
        End If


    End Sub

    Public Function CreateReport() As ReportDocument
        Dim rptsrc As New ReportDocument
        rptsrc.Load(Request.PhysicalApplicationPath & "Reports/rptPrintVoucherNull.rpt")
        cReportUtility.setConnectionInfo(rptsrc)

        cReportUtility.PassParameter("@Ticketing_Schedule_ID", Request.QueryString("TSID"), rptsrc)

        Return rptsrc
    End Function

End Class
