Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class ValueAddedServiceReport
    Inherits System.Web.UI.Page
    Dim rptDoc As ReportDocument

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        Dim fileName As String = ""


        fileName = Server.MapPath("..\TempDocument\") + "ValueAddedServices.pdf"
        rptDoc = CreateReport()
        If ((System.IO.File.Exists(fileName))) Then
            System.IO.File.Delete(fileName)

        End If

        rptDoc = CreateReport()


        rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, fileName)


        rptDoc.Close()
        rptDoc.Dispose()
        GC.Collect()

        Response.Redirect("loadPDF.aspx?Type=ValueAddedServices", True)


    End Sub

    Private Function CreateReport() As Object
        Dim rptsrc As New ReportDocument

        rptsrc.Load(Request.PhysicalApplicationPath & "Reports/rptSmartValueAddedReport.rpt")
        cReportUtility.setConnectionInfo(rptsrc)
        cReportUtility.PassParameter("@Ticketing_Schedule_ID", Request.QueryString("TSID"), rptsrc)
        Return rptsrc
    End Function
End Class