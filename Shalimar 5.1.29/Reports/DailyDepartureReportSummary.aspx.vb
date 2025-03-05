Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine

Public Class DailyDepartureReportSummary
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
    Protected WithEvents CRV As CrystalDecisions.Web.CrystalReportViewer
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable

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



        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        Dim fileName As String = ""


        fileName = Server.MapPath("..\TempDocument\") + "DailyDepartureReportSummary.pdf"
        rptDoc = CreateReport()
        If ((System.IO.File.Exists(fileName))) Then
            System.IO.File.Delete(fileName)

        End If

        rptDoc = CreateReport()


        rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, fileName)


        rptDoc.Close()
        rptDoc.Dispose()
        GC.Collect()

        Response.Redirect("loadPDF.aspx?Type=DailyDepartureReportSummary", True)

        'If Not IsPostBack Then
        '    rptDoc = CreateReport()
        '    Cache.Insert("Daily Departure Report", rptDoc, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(2))
        '    '
        'Else
        '    If Not Cache("Daily Departure Report") Is Nothing Then
        '        rptDoc = Cache("Daily Departure Report")
        '    Else
        '        rptDoc = CreateReport()
        '        Cache.Insert("Daily Departure Report", rptDoc, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(2))
        '    End If
        'End If

        'cReportUtility.applyRptPaperInfo(rptDoc, CRV, , PaperOrientation.Landscape)
        'rptDoc.ExportToDisk(ExportFormatType.Excel, "c:\nauman.xls")
        cReportUtility.applyRptPaperInfo(rptDoc, CRV, , PaperOrientation.Portrait)
    End Sub
    Public Function FormateDate(ByVal str_Date As String) As String
        Dim sdate As String() = str_Date.Split("/")
        Return sdate(2) & "-" & sdate(1) & "-" & sdate(0)
    End Function
    Public Function CreateReport() As ReportDocument
        Dim rptsrc As New ReportDocument
        rptsrc.Load(Request.PhysicalApplicationPath & "Reports/rptDailyDepartureSummary.rpt")
        cReportUtility.setConnectionInfo(rptsrc)

        ' cReportUtility.PassParameter("@Option", CInt(Request.QueryString("Type")), rptsrc)
        cReportUtility.PassParameter("@From_Date", FormateDate(Request.QueryString("from")), rptsrc)
        cReportUtility.PassParameter("@To_Date", FormateDate(Request.QueryString("to")), rptsrc)
        cReportUtility.PassParameter("@Route", Request.QueryString("Route"), rptsrc)


        Return rptsrc
    End Function

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click



    End Sub

End Class
