Imports CrystalDecisions.CrystalReports.Engine
'Imports GMS.CACommon
Imports CrystalDecisions.Shared
'Imports Microsoft.Office.Interop
Imports FMovers.Ticketing.Entity.clsUser

Public Class cReportUtility

    Private Shared Sub SetDBLogonForReport(ByVal myConnectionInfo As ConnectionInfo, ByVal myReportDocument As ReportDocument)


        Dim myTables As Tables = myReportDocument.Database.Tables
        Dim myTable As CrystalDecisions.CrystalReports.Engine.Table
        For Each myTable In myTables
            Dim myTableLogonInfo As TableLogOnInfo = myTable.LogOnInfo
            myTableLogonInfo.ConnectionInfo = myConnectionInfo
            myTable.ApplyLogOnInfo(myTableLogonInfo)
        Next



    End Sub

    Private Shared Sub SetDBLogonForSubreports(ByVal myConnectionInfo As ConnectionInfo, ByVal myReportDocument As ReportDocument)
        Dim mySections As Sections = myReportDocument.ReportDefinition.Sections
        Dim mySection As Section
        For Each mySection In mySections
            Dim myReportObjects As ReportObjects = mySection.ReportObjects
            Dim myReportObject As ReportObject
            For Each myReportObject In myReportObjects
                If myReportObject.Kind = ReportObjectKind.SubreportObject Then
                    Dim mySubreportObject As SubreportObject = CType(myReportObject, SubreportObject)
                    Dim subReportDocument As ReportDocument = mySubreportObject.OpenSubreport(mySubreportObject.SubreportName)
                    SetDBLogonForReport(myConnectionInfo, subReportDocument)
                End If
            Next
        Next
    End Sub

    Public Shared Sub setConnectionInfo(ByRef rptsource As ReportDocument)

        Try
            Dim myConnectionInfo As ConnectionInfo = New ConnectionInfo
            myConnectionInfo.ServerName = FMovers.Ticketing.DAL.Crypto.Decrypt(System.Configuration.ConfigurationManager.AppSettings.Item("DatabaseServer"), "")
            myConnectionInfo.DatabaseName = FMovers.Ticketing.DAL.Crypto.Decrypt(System.Configuration.ConfigurationManager.AppSettings.Item("Database"), "")
            myConnectionInfo.UserID = FMovers.Ticketing.DAL.Crypto.Decrypt(System.Configuration.ConfigurationManager.AppSettings.Item("DatabaseUser"), "")
            myConnectionInfo.Password = FMovers.Ticketing.DAL.Crypto.Decrypt(System.Configuration.ConfigurationManager.AppSettings.Item("DatabasePassword"), "")

            SetDBLogonForReport(myConnectionInfo, rptsource)
            SetDBLogonForSubreports(myConnectionInfo, rptsource)

        Catch exp As System.Exception
            'cUtility.CreateLogForControledExceptions(exp)
        End Try


    End Sub

    Public Shared Sub PassParameter(ByVal fieldName As String, ByVal param_value As String, ByRef rptDoc As ReportDocument)
        Dim newValue As New CrystalDecisions.Shared.ParameterDiscreteValue
        newValue.Value = param_value
        Dim paramfieldValues As New CrystalDecisions.Shared.ParameterValues  '= rptDoc.DataDefinition.ParameterFields.Item(param_index).CurrentValues
        paramfieldValues.Add(newValue)

        rptDoc.DataDefinition.ParameterFields.Item(fieldName).ApplyCurrentValues(paramfieldValues)


    End Sub

    Public Shared Sub EportToPDF(ByRef crReportDocument As ReportDocument)
        If HttpContext.Current.Session("CurrentUser") Is Nothing Then
            Dim str As String = "<script langugae=javascript> alert('The Session has expired.  Please log into the system again.'); this.close(); </script>"
            System.Web.HttpContext.Current.Response.Write(str)
            Return

        End If


        Dim crExportOptions As ExportOptions
        Dim crDiskFileDestinationOptions As DiskFileDestinationOptions
        Dim Fname As String

        If (Not System.IO.Directory.Exists(System.Web.HttpContext.Current.Request.PhysicalApplicationPath & "\TempDocument")) Then
            System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Request.PhysicalApplicationPath & "\TempDocument")
        End If

        'Fname = Request.PhysicalApplicationPath & "\" & Session.SessionID.ToString & ".pdf"
        Fname = System.Web.HttpContext.Current.Request.PhysicalApplicationPath & "\TempDocument\nauman.pdf"
        crDiskFileDestinationOptions = New DiskFileDestinationOptions
        crDiskFileDestinationOptions.DiskFileName = Fname
        crExportOptions = crReportDocument.ExportOptions
        With crExportOptions
            .DestinationOptions = crDiskFileDestinationOptions
            .ExportDestinationType = ExportDestinationType.DiskFile
            .ExportFormatType = ExportFormatType.PortableDocFormat
        End With
        crReportDocument.Export()
        ' The following code writes the pdf file to the Client’s browser.
        System.Web.HttpContext.Current.Response.ClearContent()
        System.Web.HttpContext.Current.Response.ClearHeaders()
        System.Web.HttpContext.Current.Response.ContentType = "application/pdf"
        System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "Attachment; filename=" & System.Web.HttpContext.Current.Session.SessionID.ToString & ".pdf")
        System.Web.HttpContext.Current.Response.WriteFile(Fname)
        System.Web.HttpContext.Current.Response.Flush()
        System.Web.HttpContext.Current.Response.Close()
        'delete the exported file from disk
        System.IO.File.Delete(Fname)

    End Sub

    Public Shared Sub ExportToWord(ByRef crReportDocument As ReportDocument)
        If HttpContext.Current.Session("CurrentUser") Is Nothing Then
            Dim str As String = "<script langugae=javascript> alert('The Session has expired.  Please log into the system again.'); this.close(); </script>"
            System.Web.HttpContext.Current.Response.Write(str)
            Return

        End If


        Dim crExportOptions As ExportOptions
        Dim crDiskFileDestinationOptions As DiskFileDestinationOptions
        Dim Fname As String



        'Fname = Request.PhysicalApplicationPath & "\" & Session.SessionID.ToString & ".doc"
        If (Not System.IO.Directory.Exists(System.Web.HttpContext.Current.Request.PhysicalApplicationPath & "\TempDocument\" & System.Web.HttpContext.Current.Session("CurrentUser").UserName)) Then
            System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Request.PhysicalApplicationPath & "\TempDocument\" & System.Web.HttpContext.Current.Session("CurrentUser").UserName)
        End If
        Fname = System.Web.HttpContext.Current.Request.PhysicalApplicationPath & "\TempDocument\" & System.Web.HttpContext.Current.Session("CurrentUser").UserName & "\" & System.Web.HttpContext.Current.Session.SessionID.ToString & ".doc"
        crDiskFileDestinationOptions = New DiskFileDestinationOptions
        crDiskFileDestinationOptions.DiskFileName = Fname
        crExportOptions = crReportDocument.ExportOptions
        With crExportOptions
            .DestinationOptions = crDiskFileDestinationOptions
            .ExportDestinationType = ExportDestinationType.DiskFile
            .ExportFormatType = ExportFormatType.WordForWindows
        End With
        crReportDocument.Export()
        ' The following code writes the pdf file to the Client’s browser.
        System.Web.HttpContext.Current.Response.ClearContent()
        System.Web.HttpContext.Current.Response.ClearHeaders()
        System.Web.HttpContext.Current.Response.ContentType = "application/msword"
        System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "Attachment; filename=" & System.Web.HttpContext.Current.Session.SessionID.ToString & ".doc")
        System.Web.HttpContext.Current.Response.WriteFile(Fname)
        System.Web.HttpContext.Current.Response.Flush()
        System.Web.HttpContext.Current.Response.Close()
        'delete the exported file from disk
        System.IO.File.Delete(Fname)
    End Sub

    Public Shared Sub ExportToExcel(ByRef crReportDocument As ReportDocument, Optional ByVal Wrap As Boolean = False)

        'If HttpContext.Current.Session("CurrentUser") Is Nothing Then
        '    Dim str As String = "<script langugae=javascript> alert('The Session has expired.  Please log into the system again.'); this.close(); </script>"
        '    System.Web.HttpContext.Current.Response.Write(str)
        '    Return

        'End If


        Dim crExportOptions As ExportOptions
        Dim crDiskFileDestinationOptions As DiskFileDestinationOptions
        Dim Fname As String



        'Fname = Request.PhysicalApplicationPath & "\" & Session.SessionID.ToString & ".doc"
        If (Not System.IO.Directory.Exists(System.Web.HttpContext.Current.Request.PhysicalApplicationPath & "\TempDocument\nauman")) Then
            System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Request.PhysicalApplicationPath & "\TempDocument\nauman")
        End If
        Fname = System.Web.HttpContext.Current.Request.PhysicalApplicationPath & "\TempDocument\nauman\nauman.xls"
        crDiskFileDestinationOptions = New DiskFileDestinationOptions
        crDiskFileDestinationOptions.DiskFileName = Fname
        crExportOptions = crReportDocument.ExportOptions
        With crExportOptions
            .DestinationOptions = crDiskFileDestinationOptions
            .ExportDestinationType = ExportDestinationType.DiskFile
            .ExportFormatType = ExportFormatType.Excel
        End With
        crReportDocument.Export()
        'developed by Shakeel
        If Wrap Then
            ExcelSheetAdjust(Fname)
        End If
        'End shakeel

        ' The following code writes the xls file to the Client’s browser.
        System.Web.HttpContext.Current.Response.ClearContent()
        System.Web.HttpContext.Current.Response.ClearHeaders()
        System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"
        System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "Attachment; filename=" & System.Web.HttpContext.Current.Session.SessionID.ToString & ".xls")
        System.Web.HttpContext.Current.Response.WriteFile(Fname)
        System.Web.HttpContext.Current.Response.Flush()
        System.Web.HttpContext.Current.Response.Close()
        'delete the exported file from disk




        System.IO.File.Delete(Fname)

    End Sub

    Private Shared Sub ExcelSheetAdjust(ByVal Fname As String)
        'Dim ObjExcel As New Excel.Application
        'Dim ObjWS As Excel.Worksheet
        'ObjExcel.Visible = False
        'ObjExcel.Workbooks.Add()
        'ObjExcel.Workbooks.Open(Fname)
        'Dim ObjW = ObjExcel.ActiveWorkbook
        ''Dim ObjW As Excel.Workbook = ObjExcel.Workbooks.Open(Fname)
        'ObjWS = ObjExcel.Worksheets(1)
        'ObjWS.Columns.WrapText = True
        'ObjWS.Columns.AutoFit()
        'ObjWS.Application.ActiveWindow.DisplayGridlines = True
        ''If Not (FreezRow Is Nothing Or FreezwRow.Equals("")) Then
        ''    ObjWS.Range(FreezeRow).Select()
        ''    ObjWS.Application.ActiveWindow.FreezePanes = True
        ''End If


        'ObjW.Save()
        'ObjExcel.Quit()
        'System.Runtime.InteropServices.Marshal.ReleaseComObject(ObjWS)
        'System.Runtime.InteropServices.Marshal.ReleaseComObject(ObjW)
        'System.Runtime.InteropServices.Marshal.ReleaseComObject(ObjExcel)

        'ObjWS = Nothing
        'ObjW = Nothing
        'ObjExcel = Nothing
        'GC.Collect()
    End Sub 'End shakeel

    Public Shared Function isParentSelection(Optional ByVal boolselected As Boolean = False) As Boolean
        Dim introwcount As Integer = System.Web.HttpContext.Current.Request.QueryString("grec")
        Dim intparent As String = System.Web.HttpContext.Current.Request.QueryString("Parent").Trim()
        Dim strsri As Integer = System.Web.HttpContext.Current.Request.QueryString("sri")
        If boolselected Then
            System.Web.HttpContext.Current.Session("RptSelContractId") = strsri
        End If
        If (intparent = "3" AndAlso introwcount > 0) Or (boolselected AndAlso strsri > 0) Then

            Return True
        Else
            Return False

        End If


    End Function

    Public Shared Sub setEnableControls(Optional ByRef rdoAll As RadioButton = Nothing, Optional ByRef rdoSlected As RadioButton = Nothing, Optional ByRef btnShowReport As Button = Nothing)

        'If Not rdoAll Is Nothing Then
        '    rdoAll.Checked = cReportUtility.isParentSelection() AndAlso System.Web.HttpContext.Current.Session("contractids") <> "" AndAlso Not System.Web.HttpContext.Current.Session("contractids") Is Nothing
        '    rdoAll.Enabled = rdoAll.Checked
        '    'cReportUtility.isParentSelection() AndAlso System.Web.HttpContext.Current.Session("contractids") <> "" AndAlso Not System.Web.HttpContext.Current.Session("contractids") Is Nothing
        'End If
        'If Not rdoSlected Is Nothing Then
        '    If rdoAll.Checked Then
        '        'rdoSlected.Checked = cReportUtility.isParentSelection(True)
        '        rdoSlected.Enabled = cReportUtility.isParentSelection(True) 'rdoSlected.Checked
        '    Else

        '        rdoSlected.Checked = cReportUtility.isParentContract() AndAlso Not System.Web.HttpContext.Current.Session("ContractID") Is Nothing AndAlso CStr(System.Web.HttpContext.Current.Session("ContractID")) <> ""
        '        rdoSlected.Enabled = rdoSlected.Checked
        '    End If

        '    'cReportUtility.isParentContract() AndAlso System.Web.HttpContext.Current.Session("ContractID") <> "" AndAlso Not System.Web.HttpContext.Current.Session("ContractID") Is Nothing
        'End If
        'If Not btnShowReport Is Nothing Then
        '    btnShowReport.Enabled = (cReportUtility.isParentContract() AndAlso Not System.Web.HttpContext.Current.Session("ContractID") Is Nothing AndAlso CStr(System.Web.HttpContext.Current.Session("ContractID")) <> "") Or (cReportUtility.isParentSelection() AndAlso Not System.Web.HttpContext.Current.Session("contractids") Is Nothing AndAlso CStr(System.Web.HttpContext.Current.Session("contractids")) <> "")
        'End If

    End Sub

    Public Shared Sub applyRptPaperInfo(ByRef rptDoc As ReportDocument, ByRef CRV As CrystalDecisions.Web.CrystalReportViewer, Optional ByVal PapSize As PaperSize = PaperSize.PaperLetter, Optional ByVal PapOrientation As PaperOrientation = PaperOrientation.Landscape)

        rptDoc.PrintOptions.PaperSize = PapSize
        rptDoc.PrintOptions.PaperOrientation = PapOrientation

        CRV.ReportSource = rptDoc
        CRV.PrintMode = CrystalDecisions.Web.PrintMode.ActiveX

        CRV.DisplayGroupTree = False
        CRV.HasToggleGroupTreeButton = False

        CRV.HasExportButton = True
        CRV.HasExportButton = False
        CRV.HasRefreshButton = False
        CRV.HasSearchButton = False
        CRV.HasCrystalLogo = False
        CRV.HasToggleParameterPanelButton = False
        CRV.DataBind()

    End Sub

End Class
