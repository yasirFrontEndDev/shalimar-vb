Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Drawing.Printing
Imports System.Printing
Imports System.Management
Imports System.Management.Instrumentation

Public Class CNIC
    Inherits Web.UI.Page
    Public DateFormat As String
    Private CurrencySymbol As String
    Private objConnection As Object

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

        Dim fileName As String = ""


        If Not IsPostBack Then

            PrinterList()
            'cboPrints.Items.Add(System.Configuration.ConfigurationManager.AppSettings.Item("PrinterName"))
            'cboPrints.Items.Add(System.Configuration.ConfigurationManager.AppSettings.Item("SharedPrinterName"))
            'For Each pkInstalledPrinters In PrinterSettings.InstalledPrinters
            '    cboPrints.Items.Add(pkInstalledPrinters)
            'Next pkInstalledPrinters


            'For Each strLocalPrinter As String In PrinterSettings.InstalledPrinters
            '    'List Printers on a Combobox 

            '    cboPrints.Items.Add(strLocalPrinter)
            'Next


            fileName = Server.MapPath("..\TempDocument\") + "CINC.pdf"
            rptDoc = CreateReport()
            If ((System.IO.File.Exists(fileName))) Then
                System.IO.File.Delete(fileName)

            End If

            rptDoc = CreateReport()


            rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, fileName)


            rptDoc.Close()
            rptDoc.Dispose()
            GC.Collect()

            Response.Redirect("loadPDF.aspx?Type=CINC", True)


            Cache.Insert("Print Voucher Report", rptDoc, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(2))
            '    '
        Else
            If Not Cache("Print Voucher Report") Is Nothing Then
                rptDoc = Cache("Print Voucher Report")
            Else
                rptDoc = CreateReport()
                Cache.Insert("Print Voucher Report", rptDoc, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(2))
            End If
        End If
        cReportUtility.applyRptPaperInfo(rptDoc, CRV, , PaperOrientation.Portrait)



        If Request.QueryString("status") = "1" Then
            CRV.HasPrintButton = False
            btnPrint.Visible = False
        Else
            CRV.HasPrintButton = True
            btnPrint.Visible = True
        End If


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

        rptsrc.Load(Request.PhysicalApplicationPath & "Reports/rptCNICSheet.rpt")
        cReportUtility.setConnectionInfo(rptsrc)
        cReportUtility.PassParameter("@Ticketing_Schedule_ID", Request.QueryString("TSID"), rptsrc)
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

        Dim printServer As New System.Printing.LocalPrintServer

        'Dim printQueuesOnLocalServer As PrintQueueCollection = printServer.GetPrintQueues(New () {EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections})

        'For Each printer As PrintQueue In printQueuesOnLocalServer



        '    Debug.WriteLine(vbTab & "The shared printer : " + printer.Name)
        'Next

        'Dim QueryObject As ManagementObject
        'Try
        '    Dim SearchObject As New ManagementObjectSearcher("root\CIMV2", _
        '    "SELECT DeviceID FROM Win32_Printer")
        '    For Each QueryObject In SearchObject.Get()
        '        Response.Write(QueryObject("DeviceID").ToString())

        '    Next
        'Catch err As ManagementException
        '    Response.Write(err.Message)
        'End Try


        'Dim pkInstalledPrinters As String

        '' Find all printers installed
        'For Each pkInstalledPrinters In _
        '    PrinterSettings.InstalledPrinters

        '    Response.Write((pkInstalledPrinters))

        'Next pkInstalledPrinters

        rptDoc.PrintOptions.PrinterName = cboPrints.SelectedItem.Text
        rptDoc.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4

        rptDoc.PrintToPrinter(1, True, 0, 0)

    End Sub
End Class
