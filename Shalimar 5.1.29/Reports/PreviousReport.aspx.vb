Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online

Partial Public Class PreviousReport
    Inherits System.Web.UI.Page

    Public DateFormat As String
    Private CurrencySymbol As String
    Private objConnection As Object

    Dim rptDoc As ReportDocument


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        
           
        End If


        Dim fileName As String = ""
      

        fileName = Server.MapPath("..\TempDocument\") + "Log.pdf"
        rptDoc = CreateReport()
        If ((System.IO.File.Exists(fileName))) Then
            System.IO.File.Delete(fileName)

        End If

        rptDoc = CreateReport()


        rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, fileName)


        rptDoc.Close()
        rptDoc.Dispose()
        GC.Collect()

        Response.Redirect("loadPDF.aspx?Type=Log", True)

    End Sub
    Public Function CreateReport() As ReportDocument


        Dim rptsrc As New ReportDocument
        Dim objUser As clsUser

        objUser = CType(Session("CurrentUser"), clsUser)

        Dim shid = Request.QueryString("SID")

        rptsrc.Load(Request.PhysicalApplicationPath & "Reports/MissedCashLogReport.rpt")

        cReportUtility.setConnectionInfo(rptsrc)
        cReportUtility.PassParameter("@UserId", objUser.Id, rptsrc)
        cReportUtility.PassParameter("@DateFrom", FormateDate(Request.QueryString("from")), rptsrc)
        cReportUtility.PassParameter("@DateTo", FormateDate(Request.QueryString("to")), rptsrc)
        '    cReportUtility.PassParameter("@Schedule_Id", shid, rptsrc)


        If Not Request.QueryString("Type") Is Nothing Then

            If Request.QueryString("Type").ToString().Trim() = "Log" Then
                cReportUtility.PassParameter("@Type", "Log", rptsrc)
            ElseIf Request.QueryString("Type").ToString().Trim() = "4" Then
                cReportUtility.PassParameter("@Type", "Missed", rptsrc)
            ElseIf Request.QueryString("Type").ToString().Trim() = "5" Then
                cReportUtility.PassParameter("@Type", "TicketRefund", rptsrc)
            ElseIf Request.QueryString("Type").ToString().Trim() = "6" Then
                cReportUtility.PassParameter("@Type", "TicketChange", rptsrc)
            ElseIf Request.QueryString("Type").ToString().Trim() = "7" Then
                cReportUtility.PassParameter("@Type", "NextDeparture", rptsrc)
            Else
                cReportUtility.PassParameter("@Type", "", rptsrc)
            End If
        Else
            cReportUtility.PassParameter("@Type", "", rptsrc)
        End If




        Return rptsrc
    End Function
    Public Function FormateDate(ByVal str_Date As String) As String

        Dim sdate As String() = str_Date.Split("/")
        Return sdate(2) & "-" & sdate(1) & "-" & sdate(0)

    End Function
End Class