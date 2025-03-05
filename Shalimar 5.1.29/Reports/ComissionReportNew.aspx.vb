Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online
Imports System.Drawing.Printing
Imports System.Management

Partial Public Class ComissionReportNew
    Inherits System.Web.UI.Page

    Public DateFormat As String
    Private CurrencySymbol As String
    Private objConnection As Object

    Protected WithEvents btnPrint As System.Web.UI.WebControls.Button
    Dim rptDoc As ReportDocument
    Dim objSchedules As clsSchedules
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        'Response.Write(Request.QueryString("from"))
        'Response.Write(Request.QueryString("to"))
        Dim fileName As String = ""

        fileName = Server.MapPath("..\TempDocument\") & "Comission.pdf"
        rptDoc = CreateReport()
        If ((System.IO.File.Exists(fileName))) Then
            System.IO.File.Delete(fileName)

        End If

        rptDoc = CreateReport()


        rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, fileName)


        rptDoc.Close()
        rptDoc.Dispose()
        GC.Collect()

        Response.Redirect("loadPDF.aspx?Type=Comission", True)


        ''Response.Write(Request.QueryString("from"))
        ''Response.Write(Request.QueryString("to"))
        'If Not IsPostBack Then
        '    For Each strLocalPrinter As String In PrinterSettings.InstalledPrinters
        '        'List Printers on a Combobox 

        '        'cboPrints.Items.Add(strLocalPrinter)
        '    Next

        '    rptDoc = CreateReport()
        '    Cache.Insert("Commission Report", rptDoc, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(2))
        '    '
        'Else
        '    If Not Cache("Commission Report") Is Nothing Then
        '        rptDoc = Cache("Commission Report")
        '    Else
        '        rptDoc = CreateReport()
        '        Cache.Insert("Commission Report", rptDoc, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(2))
        '    End If
        'End If
        'cReportUtility.applyRptPaperInfo(rptDoc, CRV, , PaperOrientation.Landscape)

    End Sub


    Public Function CreateReport() As ReportDocument


        Dim rptsrc As New ReportDocument


        Dim WhereClase As String = ""
        Dim WhereClasePrint As String = ""
        Dim dtReturn As New DataTable

        objConnection = ConnectionManager.GetConnection()
        objSchedules = New clsSchedules(objConnection)
        WhereClase = "Where 1 = 1   "


        If Not Request.QueryString("dfrom") Is Nothing Then

            If Request.QueryString("Type") = "3" Then

                WhereClase = WhereClase + " and dateadd(dd,0, datediff(dd,0,TS_Date)) between '" + FormateDate(Request.QueryString("dfrom")) + "' and '" + FormateDate(Request.QueryString("to")) + "' "
                WhereClasePrint = WhereClasePrint + " Date Range " & FormateDate(Request.QueryString("dfrom")) + " To " + FormateDate(Request.QueryString("to")) + "' "
            Else
                WhereClase = WhereClase + " and dateadd(dd,0, datediff(dd,0,TS_Date)) between '" + FormateDate(Request.QueryString("dfrom")) + "' and '" + FormateDate(Request.QueryString("to")) + "' "
                WhereClasePrint = WhereClasePrint + " Date Range " & FormateDate(Request.QueryString("dfrom")) + " To " + FormateDate(Request.QueryString("to")) + "' "

            End If

        End If


        If Not Request.QueryString("Route") Is Nothing Then
            If Request.QueryString("Route") <> "0" Then
                WhereClase = WhereClase + " and Ticketing_Schedule.Schedule_Id = " + Request.QueryString("Route")
                WhereClasePrint = WhereClasePrint + " Route " & Request.QueryString("Route")

            End If
        End If


        If Not Request.QueryString("Users") Is Nothing Then
            If Request.QueryString("Users") <> "0" Then
                WhereClase = WhereClase + " and Ticketing_Schedule.Voucher_Closed_By = " + Request.QueryString("Users")
                WhereClasePrint = WhereClasePrint + " Route " & Request.QueryString("Users")

            End If
        End If

        If Not Request.QueryString("Vehicle") Is Nothing Then
            If Request.QueryString("Vehicle") <> "0" Then
                WhereClase = WhereClase + " and Ticketing_Schedule.Vehicle_ID = " + Request.QueryString("Vehicle")
                WhereClasePrint = WhereClasePrint + " Route " & Request.QueryString("Vehicle")

            End If
        End If

        If Request.QueryString("Type") = "1" Then
            rptsrc.Load(Request.PhysicalApplicationPath & "Reports/rptCashSheetReport.New.rpt")
        ElseIf Request.QueryString("Type") = "2" Then
            rptsrc.Load(Request.PhysicalApplicationPath & "Reports/rptCashSheetReport.NewMultan.rpt")
        Else
            rptsrc.Load(Request.PhysicalApplicationPath & "Reports/rpt_UserWiseClosingSummary.rpt")
        End If

        'rptsrc.SetParameterValue(0, "Where = 1")
        'rptsrc.SetParameterValue(1, "Where = 1")

        cReportUtility.setConnectionInfo(rptsrc)

        ''strDate = strDate.AddDays(1)

        '' cReportUtility.PassParameter("@Option", "0", rptsrc)

        'cReportUtility.PassParameter("@WhereClase", "Where 1 = 1", rptsrc)
        'cReportUtility.PassParameter("@WhereClaseprint", "", rptsrc)

        'Return rptsrc

      


        'dtReturn = objSchedules.GetComissionReport(WhereClase, WhereClase)


        'rptsrc.Load(Request.PhysicalApplicationPath & "Reports/rptCashSheetReport.Newrpt.rpt")
        'rptsrc.SetDataSource(dtReturn)


        cReportUtility.PassParameter("@WhereClase", WhereClase, rptsrc)
        cReportUtility.PassParameter("@WhereClaseprint", WhereClasePrint, rptsrc)
        Return rptsrc

    End Function

    Public Function FormateDate(ByVal str_Date As String) As String
        Dim sdate As String() = str_Date.Split("/")
        Return sdate(2) & "-" & sdate(1) & "-" & sdate(0)
    End Function
End Class