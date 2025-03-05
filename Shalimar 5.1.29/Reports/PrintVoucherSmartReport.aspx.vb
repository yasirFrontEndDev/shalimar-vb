Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Drawing.Printing
Imports System.Printing
Imports System.Management
Imports System.Management.Instrumentation
Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online


Public Class PrintVoucherSmartReport
    Inherits Web.UI.Page
    Public DateFormat As String
    Private CurrencySymbol As String
    Private objConnection As Object
    Dim objTicketing As clsTicketing

    Protected WithEvents btnPrint As System.Web.UI.WebControls.Button
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        objConnection = ConnectionManager.GetConnection()

        objTicketing = New clsTicketing(objConnection)



        Dim fileName As String = ""


        fileName = Server.MapPath("..\TempDocument\") + "SmartVoucher.pdf"
        rptDoc = CreateReport()
        If ((System.IO.File.Exists(fileName))) Then
            System.IO.File.Delete(fileName)

        End If

        rptDoc = CreateReport()

        ' Dim doctoprinttest = System.Configuration.ConfigurationManager.AppSettings.Item("PrinterLaser")
        Dim doctoprinttest = System.Configuration.ConfigurationManager.AppSettings.Item("PrinterName")

        Dim getprinterName As PrinterSettings = New PrinterSettings()
        rptDoc.PrintOptions.PrinterName = doctoprinttest

        '  rptDoc.PrintOptions.PrinterName = getprinterName.PrinterName

        '  rptDoc.PrintToPrinter(1, True, 0, 0)
        '  rptDoc.PrintToPrinter(1, True, 1, 1)

        rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, fileName)


        rptDoc.Close()
        rptDoc.Dispose()
        GC.Collect()

        objTicketing.PrintVoucher(Request.QueryString("TSID"))


        cboPrints.Visible = False

        Response.Redirect("loadPDF.aspx?Type=SmartVoucher", True)

        'If Not IsPostBack Then

        '    PrinterList()
        '    rptDoc = CreateReport()
        '    Cache.Insert("Print Voucher Report", rptDoc, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(2))
        '    '    '
        'Else
        '    If Not Cache("Print Voucher Report") Is Nothing Then
        '        rptDoc = Cache("Print Voucher Report")
        '    Else
        '        rptDoc = CreateReport()
        '        Cache.Insert("Print Voucher Report", rptDoc, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(2))
        '    End If
        'End If
        'cReportUtility.applyRptPaperInfo(rptDoc, CRV, , PaperOrientation.Portrait)




        'If Request.QueryString("status") = "1" Then
        '    CRV.HasPrintButton = False
        '    btnPrint.Visible = False
        'Else
        '    CRV.HasPrintButton = True
        '    btnPrint.Visible = True
        'End If


    End Sub

    Private Sub GetAllPrinterList()



        Dim objScope As New ManagementScope(ManagementPath.DefaultPath)
        'For the local Access
        objScope.Connect()

        Dim selectQuery As New SelectQuery()
        selectQuery.QueryString = "Select * from win32_Printer"
        Dim MOS As New ManagementObjectSearcher(objScope, selectQuery)
        Dim MOC As ManagementObjectCollection = MOS.[Get]()

        For Each mo As ManagementObject In MOC
            cboPrints.Items.Add(mo("Name").ToString())
        Next

    End Sub

    Private Sub PrinterList()
        ' USING WMI. (WINDOWS MANAGEMENT INSTRUMENTATION)
        cboPrints.Items.Clear()
        Dim objMS As System.Management.ManagementScope = _
            New System.Management.ManagementScope(ManagementPath.DefaultPath)
        objMS.Connect()

        Dim objQuery As SelectQuery = New SelectQuery("SELECT * FROM Win32_Printer")
        Dim objMOS As ManagementObjectSearcher = New ManagementObjectSearcher(objMS, objQuery)
        Dim objMOC As System.Management.ManagementObjectCollection = objMOS.Get()

        For Each Printers As ManagementObject In objMOC
            If CBool(Printers("Local")) Then                        ' LOCAL PRINTERS.
                cboPrints.Items.Add(Printers("Name"))
            End If
            If CBool(Printers("Network")) Then                      ' ALL NETWORK PRINTERS.
                cboPrints.Items.Add(Printers("Name"))
            End If
        Next Printers
    End Sub

    Public Function CreateReport() As ReportDocument

        Dim rptsrc As New ReportDocument

        rptsrc.Load(Request.PhysicalApplicationPath & "Reports/rptPrintSmartVoucher.rpt")
        cReportUtility.setConnectionInfo(rptsrc)
        cReportUtility.PassParameter("@Ticketing_Schedule_ID", Request.QueryString("TSID"), rptsrc)
        cReportUtility.PassParameter("@ShowALl", Request.QueryString("ShowALl"), rptsrc)
        cReportUtility.PassParameter("@Online_Ticketing_Schedule_ID", Request.QueryString("OnlineTSID"), rptsrc)
        cReportUtility.PassParameter("@VersionNumber", "", rptsrc)

        Return rptsrc

    End Function

    Private Function GetDefaultPrinter() As String



        Dim settings As New PrinterSettings()

        For Each printer As String In PrinterSettings.InstalledPrinters
            settings.PrinterName = printer
            If settings.IsDefaultPrinter Then
                Return printer
            End If
        Next

        Return String.Empty
    End Function


    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click
        Try



            Dim rawKind As Integer
            Dim doctoprint As New System.Drawing.Printing.PrintDocument()


            rptDoc.PrintOptions.PrinterName = System.Configuration.ConfigurationManager.AppSettings.Item("PrinterName")
            'rptDoc.PrintOptions.PrinterName = cboPrints.SelectedItem.Text
            'rptDoc.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.DefaultPaperSize

            rptDoc.PrintToPrinter(1, False, 0, 0)

        Catch ex As Exception
            Response.Write(ex.Message)

        End Try


    End Sub


    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

    End Sub
End Class
