Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online

Partial Public Class BS
    Inherits System.Web.UI.Page

    Public DateFormat As String
    Private CurrencySymbol As String
    Private objConnection As Object

    Dim rptDoc As ReportDocument
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            rptDoc = CreateReport()
            Cache.Insert("Daily Departure Report", rptDoc, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(2))
            '
        Else
            If Not Cache("Daily Departure Report") Is Nothing Then
                rptDoc = Cache("Daily Departure Report")
            Else
                rptDoc = CreateReport()
                Cache.Insert("Daily Departure Report", rptDoc, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(2))
            End If
        End If
        ' cReportUtility.applyRptPaperInfo(rptDoc, CRV1, , PaperOrientation.Landscape)
    End Sub
    Public Function CreateReport() As ReportDocument


        Dim rptsrc As New ReportDocument
        Dim objUser As clsUser

        objUser = CType(Session("CurrentUser"), clsUser)

        rptsrc.Load(Request.PhysicalApplicationPath & "Reports/rptBlankSeats.rpt")

        cReportUtility.setConnectionInfo(rptsrc)
        cReportUtility.PassParameter("@From_Date", FormateDate(Request.QueryString("from")), rptsrc)
        cReportUtility.PassParameter("@To_Date", FormateDate(Request.QueryString("to")), rptsrc)



        Return rptsrc
    End Function

    Public Function FormateDate(ByVal str_Date As String) As String

        Dim sdate As String() = str_Date.Split("/")
        Return sdate(2) & "-" & sdate(1) & "-" & sdate(0)

    End Function
End Class