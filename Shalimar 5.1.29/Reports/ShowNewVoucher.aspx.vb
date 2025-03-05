Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online
Imports System.Drawing.Printing
Imports System.Management

Public Class ShowNewVoucher


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
        'Response.Write(Request.QueryString("from"))
        'Response.Write(Request.QueryString("to"))
        If Not IsPostBack Then
        

            rptDoc = CreateReport()
            Cache.Insert("Commission Report", rptDoc, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(2))
            '
        Else
            If Not Cache("Commission Report") Is Nothing Then
                rptDoc = Cache("Commission Report")
            Else
                rptDoc = CreateReport()
                Cache.Insert("Commission Report", rptDoc, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(2))
            End If
        End If
        cReportUtility.applyRptPaperInfo(rptDoc, CRV, , PaperOrientation.Landscape)



    End Sub
    Public Function FormateDate(ByVal str_Date As String) As String
        Dim sdate As String() = str_Date.Split("/")
        Return sdate(2) & "-" & sdate(1) & "-" & sdate(0)
    End Function

    Public Function CreateReport() As ReportDocument

        Dim rptsrc As New ReportDocument
        rptsrc.Load(Request.PhysicalApplicationPath & "Reports/rptPrintVoucher.rpt")
        cReportUtility.setConnectionInfo(rptsrc)
        cReportUtility.PassParameter("@Ticketing_Schedule_ID", Request.QueryString("TSID"), rptsrc)
        cReportUtility.PassParameter("@ShowALl", Request.QueryString("ShowALl"), rptsrc)


        Return rptsrc
    End Function

    Private Sub CRV_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles CRV.Load


    End Sub

    Private Sub CRV_Navigate(ByVal source As Object, ByVal e As CrystalDecisions.Web.NavigateEventArgs) Handles CRV.Navigate

    End Sub

    Private Sub CRV_Search(ByVal source As Object, ByVal e As CrystalDecisions.Web.SearchEventArgs) Handles CRV.Search

    End Sub

End Class
