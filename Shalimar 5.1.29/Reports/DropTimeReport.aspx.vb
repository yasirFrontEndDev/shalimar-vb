Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine

Public Class DropTimeReport
    Inherits System.Web.UI.Page
    Dim rptDoc As ReportDocument
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        Dim fileName As String = ""


        fileName = Server.MapPath("..\TempDocument\") + "DropTimeReport.pdf"

        If ((System.IO.File.Exists(fileName))) Then
            System.IO.File.Delete(fileName)

        End If

        rptDoc = CreateReport()


        rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, fileName)


        rptDoc.Close()
        rptDoc.Dispose()
        GC.Collect()

        Response.Redirect("loadPDF.aspx?Type=DropTimeReport", True)
    End Sub
    Public Function FormateDate(ByVal str_Date As String) As String
        Dim sdate As String() = str_Date.Split("/")
        Return sdate(2) & "-" & sdate(0) & "-" & sdate(1)
    End Function
    Private Function CreateReport() As ReportDocument
        Dim rptsrc As New ReportDocument


        rptsrc.Load(Request.PhysicalApplicationPath & "Reports/DropTimeUserReport.rpt")
        cReportUtility.setConnectionInfo(rptsrc)

        cReportUtility.PassParameter("@FromDate", FormateDate(Request.QueryString("from")), rptsrc)
        cReportUtility.PassParameter("@ToDate", FormateDate(Request.QueryString("to")), rptsrc)



        Return rptsrc
    End Function
End Class