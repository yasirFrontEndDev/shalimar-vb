Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online


Partial Public Class AD
    Inherits System.Web.UI.Page

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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        Dim fileName As String = ""



        fileName = Server.MapPath("..\TempDocument\") + "AD.pdf"
        rptDoc = CreateReport()
        If ((System.IO.File.Exists(fileName))) Then
            System.IO.File.Delete(fileName)

        End If

        rptDoc = CreateReport()


        rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, fileName)


        rptDoc.Close()
        rptDoc.Dispose()
        GC.Collect()

        Response.Redirect("loadPDF.aspx?Type=AD", True)

    End Sub

    Public Function CreateReport() As ReportDocument


        Dim rptsrc As New ReportDocument
        Dim objUser As clsUser

        objUser = CType(Session("CurrentUser"), clsUser)

        rptsrc.Load(Request.PhysicalApplicationPath & "Reports/AD.rpt")

        cReportUtility.setConnectionInfo(rptsrc)
        cReportUtility.PassParameter("@From_Date", FormateDate(Request.QueryString("from")), rptsrc)
        cReportUtility.PassParameter("@To_Date", FormateDate(Request.QueryString("to")), rptsrc)
        cReportUtility.PassParameter("@Status", Request.QueryString("Status"), rptsrc)

        If Not Request.QueryString("Type") Is Nothing Then

            If Request.QueryString("Type").ToString().Trim() = "Log" Then
                cReportUtility.PassParameter("@Type", "Log", rptsrc)
            ElseIf Request.QueryString("Type").ToString().Trim() = "P" Then
                cReportUtility.PassParameter("@Type", "P", rptsrc)
            Else
                cReportUtility.PassParameter("@Type", "", rptsrc)
            End If
        Else
            cReportUtility.PassParameter("@Type", "", rptsrc)
        End If
 
        cReportUtility.PassParameter("@User_ID", objUser.Id, rptsrc)


        cReportUtility.PassParameter("@Schedule_ID", Request.QueryString("SID"), rptsrc)

        Return rptsrc
    End Function

    Public Function FormateDate(ByVal str_Date As String) As String

        Dim sdate As String() = str_Date.Split("/")
        Return sdate(2) & "-" & sdate(1) & "-" & sdate(0)

    End Function
End Class