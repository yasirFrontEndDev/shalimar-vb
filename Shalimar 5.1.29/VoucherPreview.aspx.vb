Imports System.Data.SqlClient
Imports System.Reflection
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports FMovers.Ticketing.DAL


Public Class VoucherPreview
    Inherits System.Web.UI.Page
    Dim objConnection As Object
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim VoucherDetails As New DataTable

        objConnection = ConnectionManager.GetConnection()


        VoucherDetails = GetVoucherPreviewDetails()


    End Sub

    Private Function GetVoucherPreviewDetails() As DataTable

        Dim objDbManager As IDBManager
        Dim objDataSet As DataSet
        Dim Schedule_Id As String

        objDbManager = DBManager.GetDatabaseManager()
        objDbManager.SetDBConnection(objConnection)
        Dim objDBParameters As New clsDBParameters

        Dim fileName As String = ""
        Dim rptDoc As ReportDocument
        '  Schedule_Id = Session("TicketingScheduleId")

        Dim TSID As String = Request.QueryString("TSID")

        '   objDBParameters.Parameters.Add(New clsDBParameter("@TicketingId", Schedule_Id, "bigint"))





        Dim cmd As SqlCommand = New SqlCommand("sp_GetVoucherForPreview", objConnection)

        cmd.Parameters.Add("@Ticketing_Schedule_ID", SqlDbType.NVarChar, 50).Value = TSID
        cmd.CommandType = CommandType.StoredProcedure
        Dim ds As DataSet = New DataSet()
        Dim ad As SqlDataAdapter = New SqlDataAdapter()
        ad.SelectCommand = cmd
        ad.Fill(ds)
        Dim cryRpt As ReportDocument = New ReportDocument()
        Dim strPath As String = Server.MapPath("~/Reports/Voucher.rpt")
        cryRpt.Load(strPath)

        fileName = Server.MapPath("~/TempDocument/") & "Voucher.pdf"


        If (System.IO.File.Exists(fileName)) Then
            System.IO.File.Delete(fileName)
        End If


        cryRpt.SetDataSource(ds.Tables(0))
        cryRpt.Refresh()
        cryRpt.ExportToDisk(ExportFormatType.PortableDocFormat, fileName)
        GC.Collect()

        fileName = Server.MapPath("~/TempDocument/") + "Voucher.pdf"

        Response.ContentType = "Application/pdf"
        Response.TransmitFile(fileName)

        '   rptDoc = CreateReport()
        ' Response.Redirect("loadPDFPreVouher.aspx?Type=Voucher", True)


    End Function

    Public Function CreateReport(ByVal Schedule_Id) As ReportDocument

        Try


            Dim versionNumber As Version
            versionNumber = Assembly.GetExecutingAssembly().GetName().Version
            Dim rptsrc As New ReportDocument

            rptsrc.Load(Request.PhysicalApplicationPath & "Reports/Voucher.rpt")
            cReportUtility.setConnectionInfo(rptsrc)
            cReportUtility.PassParameter("@Ticketing_Schedule_ID", Schedule_Id, rptsrc)

            Return rptsrc

        Catch ex As Exception

        End Try

    End Function


End Class
