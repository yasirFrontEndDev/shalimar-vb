Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online
Imports System.Drawing.Printing
Imports System.Management
Imports System.Net.NetworkInformation
Imports System.Reflection

Public Class PrintVoucherReportAllNew


    Inherits Web.UI.Page
    Public DateFormat As String
    Private CurrencySymbol As String
    Private objConnection As Object

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
        Dim fileName As String = ""

        rptDoc = CreateReport()

        fileName = Server.MapPath("..\TempDocument\") + Request.QueryString("TSID") + ".pdf"
        rptDoc = CreateReport()
        If ((System.IO.File.Exists(fileName))) Then
            System.IO.File.Delete(fileName)

        End If

        rptDoc = CreateReport()

        'Dim doctoprinttest = System.Configuration.ConfigurationManager.AppSettings.Item("PrinterLaser")

        'Dim getprinterName As PrinterSettings = New PrinterSettings()
        'rptDoc.PrintOptions.PrinterName = doctoprinttest

        'rptDoc.PrintOptions.PrinterName = getprinterName.PrinterName

        'rptDoc.PrintToPrinter(1, True, 0, 0)


        rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, fileName)


        rptDoc.Close()
        rptDoc.Dispose()
        GC.Collect()

        Response.Redirect("loadPDFVouher.aspx?TSID=" + Request.QueryString("TSID") + "", True)


        If Not IsPostBack Then
            For Each strLocalPrinter As String In PrinterSettings.InstalledPrinters
                'List Printers on a Combobox 


            Next

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
        Try


            Dim versionNumber As Version
            versionNumber = Assembly.GetExecutingAssembly().GetName().Version
            Dim rptsrc As New ReportDocument

            rptsrc.Load(Request.PhysicalApplicationPath & "Reports/rptPrintVoucher_All.rpt")
            cReportUtility.setConnectionInfo(rptsrc)
            cReportUtility.PassParameter("@Ticketing_Schedule_ID", Request.QueryString("TSID"), rptsrc)
            cReportUtility.PassParameter("@ShowALl", Request.QueryString("ShowALl"), rptsrc)
            cReportUtility.PassParameter("@VersionNumber", versionNumber.ToString(), rptsrc)
            cReportUtility.PassParameter("@Online_Ticketing_Schedule_ID", Request.QueryString("OnlineTSID"), rptsrc)
            Return rptsrc

        Catch ex As Exception

        End Try

    End Function

    Private Sub CRV_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles CRV.Load


    End Sub

    Private Sub CRV_Navigate(ByVal source As Object, ByVal e As CrystalDecisions.Web.NavigateEventArgs) Handles CRV.Navigate

    End Sub

    Private Sub CRV_Search(ByVal source As Object, ByVal e As CrystalDecisions.Web.SearchEventArgs) Handles CRV.Search

    End Sub




    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload


        rptDoc.Close()
        rptDoc.Dispose()
        GC.Collect()

    End Sub
End Class
