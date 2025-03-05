Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine

Partial Public Class rpt_BookingReport
    Inherits Web.UI.Page
    Public DateFormat As String
    Private CurrencySymbol As String
    Private objConnection As Object

    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Dim rptDoc As ReportDocument


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

    Public Function FormateDate(ByVal str_Date As String) As String
        Dim sdate As String() = str_Date.Split("/")
        Return sdate(2) & "-" & sdate(1) & "-" & sdate(0)
    End Function
    Public Function CreateReport() As ReportDocument

        Dim rptsrc As New ReportDocument
        rptsrc.Load(Request.PhysicalApplicationPath & "Reports/rptBookingReport.rpt")
        cReportUtility.setConnectionInfo(rptsrc)

        cReportUtility.PassParameter("@From_Date", FormateDate(Request.QueryString("from")), rptsrc)
        cReportUtility.PassParameter("@To_Date", FormateDate(Request.QueryString("to")), rptsrc)
        cReportUtility.PassParameter("@Schedule_Id", CInt(Request.QueryString("SID")), rptsrc)

        Return rptsrc

    End Function

    Protected Sub ImageButton2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton2.Click
        cReportUtility.EportToPDF(rptDoc)
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        cReportUtility.ExportToExcel(rptDoc, True)
    End Sub

End Class