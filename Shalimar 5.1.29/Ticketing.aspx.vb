
Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online
Imports Infragistics.WebUI.UltraWebGrid
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Web
Imports System.Text
Imports RestSharp

Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports MessagingToolkit.QRCode.Codec
Imports GsmComm.PduConverter
Imports GsmComm.GsmCommunication
Imports System.Drawing.Printing
Imports System.Management
Imports SQLServerMerg
Imports System.Reflection
Imports System.Drawing
Imports System.IO
Imports System.IO.Ports
Imports System.Data.SqlClient



Partial Public Class Ticketing
    Inherits System.Web.UI.Page

    'Public Shared Comm_Port As Integer = 46
    'Public Shared Comm_BaudRate As Int64 = 9600
    'Public Shared Comm_TimeOut As Int64 = 300

    'Private CommSetting As New comm_settings()

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objTicketingSeat As clsSeatTicketing
    Dim objTicketingSeatList As clsSeatTicketingList
    Dim objFare As clsFare
    Dim objOtherFare As clcRefreshment
    Dim objOtherChargeslit As OtherServiceCharges
    Dim objMiscellaneous As OtherServiceCharges

    Dim objeConnection As SqlConnection

    Dim Veicle_id As Integer
    Dim Schedule_ID As String
    Dim objVehicle As clsVehicle
    Dim objVehicle_Controler As clsVehicleControler

    Dim objTicketing As clsTicketing
    Dim TicketingScheduleId As String
    Dim objOnlineTicketing As eTicketing
    Public UserName As String
    Public CurrentUserID As String
    Public TerminalId As String
    Public dtRouteList As DataTable
    Public dtRouteListAll As DataTable
    Public TotalTime As TimeSpan
    Public CustomerPIN As String
    Public ServiceTypeName As String
    Public MYKM As Integer


#Region " Form Events "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        btnpospayment.Visible = False

        Try

            If Session("CurrentUser") Is Nothing Then
                Response.Redirect("UserLogin.aspx")
            Else
                objUser = CType(Session("CurrentUser"), clsUser)
                CurrentUserID = "" & objUser.Id
                UserName = objUser.LoginName.ToLower()

                lblSleeperTitles.Visible = False
                lblupper.Visible = False

                Label74.Visible = False
                Label75.Visible = False

                lblSleeperbusUP1.Visible = False
                lblSleeperbusUP2.Visible = False
                lblSleeperbusUP3.Visible = False
                lblSleeperbusDown1.Visible = False
                lblSleeperbusDown2.Visible = False
                lblSleeperbusDown3.Visible = False


            End If

            lblErr.Text = ""
            'Response.Write(GetDefaultPrinter() & " Print Name")


            lnkCreateOnline.Visible = False



            objConnection = ConnectionManager.GetConnection()

            objUser = CType(Session("CurrentUser"), clsUser)

            objTicketingSeat = New clsSeatTicketing(objConnection)
            objTicketing = New clsTicketing(objConnection)


            If "" & Request.QueryString("mode") <> "" Then
                hidMode.Value = Request.QueryString("mode")
            End If

            Select Case hidMode.Value
                Case 1
                    lblTitles.Text = "Select Seat for Current Ticketing"
                    BookingDropPoint.Visible = True
                    ActualDepartureTime.Visible = True
                    TicketCollectionPoint.Visible = False

                Case 2
                    lblTitles.Text = "Select Seat for Booking"
                    BookingDropPoint.Visible = False
                    ActualDepartureTime.Visible = False
                    TicketCollectionPoint.Visible = True



                Case 3
                    lblTitles.Text = "Select Seat for Advance Ticketing"
                Case 4
                    lblTitles.Text = "Select Seat for Return Ticketing"

            End Select

            If "" & Request.QueryString("TSID") <> "" Then
                TicketingScheduleId = Request.QueryString("TSID")
                hidTSID.Value = TicketingScheduleId

            Else
                TicketingScheduleId = 0
            End If

            'btnVoucherReport.Attributes.Add("onclick", "btnVoucherReport_onclick();")

            ShowHideWithMode()



            If Not Me.IsPostBack Then
                Dim dt As DataTable
                'objTicketing.UpdateVeichle(TicketingScheduleId)
                Form.DefaultButton = "btnSave"

                dt = clsUtil.GetDefaultSource(objConnection)
                Dim dtCurrentDate As DataTable = clsUtil.GetDefaultSource(objConnection)
                dtRouteList = New DataTable



                hidTerminal.Value = "" & dt.Rows(0)("Default_Terminal")

                'hidSMSDataPort.Value = ConfigurationManager.AppSettings("SMSPort").ToString()

                'BookingSMS.Value = "" & dt.Rows(0)("BookingSMS")

                'hidSMSData.Value = "" & dt.Rows(0)("SMSText")

                CloseSMS.Value = "" & dt.Rows(0)("CloseSMS")

                CloseNo.Value = "" & dt.Rows(0)("CloseNo")

                Call loadCombos()

                'If cboVoucherNo_1.Rows(0).Cells.FromKey("Vechile_ID").Value = 81 Or cboVoucherNo_1.Rows(0).Cells.FromKey("Vechile_ID").Value = "" Then

                '    lblErrVechile.Text = "Please update Vechile before closing voucher"

                'End If

                Call RegisterClientEvents()
                objTicketing.Id = Me.TicketingScheduleId

                Dim dt_GetScheduleData As DataTable = objTicketing.GetScheduleData()
                dtRouteList = objTicketing.GetRouteList()

                lblheader.Text = " Schedule : " & dt_GetScheduleData.Rows(0)(1) & " Date : " & dt_GetScheduleData.Rows(0)(3) & " Time : " & dt_GetScheduleData.Rows(0)(0)
                hidReoutName.Value = dt_GetScheduleData.Rows(0)(1)
                hndServiceType.Value = dt_GetScheduleData.Rows(0)("ServiceType_Id")
                hndDisbaleCount.Value = dt_GetScheduleData.Rows(0)("DisableCount")
                ServiceTypeName = dt_GetScheduleData.Rows(0)("ServiceType_Name")
                If CBool(dt_GetScheduleData.Rows(0)("IsDropped")) = True Then
                    hndDisable.Value = "1"
                    hndTimDropped.Value = "1"

                    MakeDisableTime()

                End If

                'If CDate(dt_GetScheduleData.Rows(0)(2)) < Now.Date Then
                '    tblTickets.Visible = False
                'End If


                ' To be clear 
                hndPrintDateTime.Value = dt_GetScheduleData.Rows(0)("PrintDateTime")

                If Not Request.QueryString("SCI") Is Nothing Then
                    hidSource.Value = Request.QueryString("SCI")
                Else
                    If hidMode.Value = "1" Then
                        hidSource.Value = "" & dt.Rows(0)("Source_City_ID")
                        trSource.Visible = False
                    Else
                        hidSource.Value = "" & dt_GetScheduleData.Rows(0)("Default_City_Id")
                        trSource.Visible = True
                    End If
                End If

                Schedule_ID = dt_GetScheduleData.Rows(0)(3)
                Desct.Value = dt_GetScheduleData.Rows(0)(1)
                bkDate.Value = dt_GetScheduleData.Rows(0)(2)

                '  LoadTimes(dt_GetScheduleData.Rows(0)(3), dt_GetScheduleData.Rows(0)(2), dt_GetScheduleData.Rows(0)(0))

                If cboVoucherNo_1.Rows.Count > 0 Then
                    PouplateForm()
                End If
            End If

            If chkOnline.Checked Then
                lnkMapping.Style.Add("DISPLAY", "")
            Else
                lnkMapping.Style.Add("DISPLAY", "none")
            End If

            If Page.IsPostBack = False Then

                cmbDestination.SelectedIndex = cmbDestination.Items.Count - 1
                loadFare()
                loadComboVechile(Veicle_id)

                'If Not objTicketing.ValidateVoucherDate(txtDepartureDate.Text) Then
                '    lblErr.Text = " Can not enter in fast date "
                'End If


                If (ServerPing()) Then
                    loadOnlineVoucher()
                End If


                'Else

                '    lblErr.Text = " Server is not online please contact IT Team. "
                '    lblErr.Visible = True
                '    chkOnline.Checked = False
                '    lnkMapping.Visible = False

                'End If



                'If Not (objOnlineTicketing.IsOnlineTicketingSchedule(objTicketing)) Then
                '    lblErr.Text = " Schedule is not online . "
                '    lblErr.Visible = True

                'End If

            End If

            If objUser.CanChangeFare = True Then
                txtFare.Enabled = True
                txtFare.ReadOnly = False

            End If
            If Not Page.IsPostBack Then getCCP_No()

          
            Response.Write("<script> disable(); </script>")

        Catch ex As Exception


            Response.Write(ex.Message())

        End Try


    End Sub


    Private Function loadOnlineVoucherForChange(ByVal Dep_Time As String, ByVal TS_date As String, ByVal Schedule_Id As Integer) As Integer

        Try


            objTicketing = New clsTicketing(objConnection)
            objOnlineTicketing = New eTicketing


            objTicketing.DepartureTime = "" & Dep_Time
            objTicketing.TSDate = CDate(TS_date)
            objTicketing.ScheduleID = "" & Schedule_Id
            Dim OnlineSchedule_ForChange_Id As Integer = objOnlineTicketing.IsOnlineTicketingScheduleOnLoad(objTicketing, Val("" & hndServiceType.Value), objUser.Vendor_Id)

            Return OnlineSchedule_ForChange_Id

        Catch ex As Exception

            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & line

        End Try

    End Function

    Private Sub MakeDisableTime()
        Try

            'Dim StudentName As String

            'StudentName = hndOnlineTSNo.Value

            'Dim tbDroppedTime As New DataTable
            'tbDroppedTime = GetDroppedByUser()

            'Dim row As DataRow = tbDroppedTime.Rows(0)

            hndDisable.Value = "1"
            hndTimDropped.Value = "1"
            DiableTable()
            lblDouple.Visible = True
            lblDouple.Text = "The departure time has been dropped. - اس بس کا وقت رک گیا ہے"
            btnDropTime.Enabled = False
            txtVehicle.Text = "Time Dropped"

        Catch ex As Exception

        End Try
    End Sub

    Private Function GetDroppedByUser() As DataTable
        Dim objDbManager As IDBManager
        Dim objDataSet As DataSet


        'objTicketing = New clsTicketing(objConnection)
        'objOnlineTicketing = New eTicketing

        objDbManager = DBManager.GetDatabaseManager()
        objDbManager.SetDBConnection(objConnection)
        Dim objDBParameters As New clsDBParameters
        'Session("TicketingScheduleId") = TicketingScheduleId


        objDBParameters.Parameters.Add(New clsDBParameter("@Ticketing_Schedule_ID", hndOnlineTSNo.Value, "bigint"))
        objDataSet = objDbManager.GetData("sp_getDropTimeInfo", objDBParameters)
        If Not objDataSet Is Nothing Then
            Return objDataSet.Tables(0)
        Else
            Return Nothing
        End If

    End Function

    Private Sub checkVoucherForDropTime()

        Try

            objTicketing = New clsTicketing(objConnection)
            objOnlineTicketing = New eTicketing
            hndTimeDrop.Value = 0
            Dim TimeDropped As Integer = objOnlineTicketing.IsTimeDropped(hndOnlineTSNo.Value)

            If TimeDropped = 1 Then
                hndTimeDrop.Value = 1
                MakeDisableTime()
            End If

        Catch ex As Exception

            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & line

        End Try


    End Sub



    Private Sub loadOnlineVoucher()

        Try

            objTicketing = New clsTicketing(objConnection)
            objOnlineTicketing = New eTicketing

            objTicketing.DepartureTime = "" & cboVoucherNo_1.Rows(0).Cells.FromKey("Departure_Time").Text
            objTicketing.TSDate = CDate(cboVoucherNo_1.Rows(0).Cells.FromKey("TS_Date").Value)
            objTicketing.ScheduleID = "" & cboVoucherNo_1.Rows(0).Cells.FromKey("Schedule_ID").Text
            Dim OnlineSchedule_Id As Integer = objOnlineTicketing.IsOnlineTicketingScheduleOnLoad(objTicketing, Val("" & hndServiceType.Value), objUser.Vendor_Id)

            hndOnlineTSNo.Value = OnlineSchedule_Id
            lblheader.Text = "Schedule # : " & hndOnlineTSNo.Value & " " & lblheader.Text

            If OnlineSchedule_Id = 0 Then

            Else
                Dim tblDropinfo As New DataTable
                tblDropinfo = GetDropTimeUserInfo()
                If tblDropinfo.Rows.Count = 0 Then

                Else
                    Dim row As DataRow = tblDropinfo.Rows(0)
                    'objTicketing.IS_Drop = row("User_Id").ToString()

                    lblDouple.Visible = True
                    lblDouple.Text = "The departure time has been dropped By. " + row("User_Name").ToString() + " - اس بس کا وقت رک گیا ہے"
                    btnDropTime.Enabled = False
                    txtVehicle.Text = "Time Dropped"

                    Dim tblupdateDropinfo As New DataTable
                    tblupdateDropinfo = UpdateDroptimestatus()



                End If



            End If






        Catch ex As Exception

            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & line

        End Try

    End Sub

    Private Function UpdateDroptimestatus() As DataTable
        Dim objeConnections = eConnectionManager.GetConnection()
        Dim objDbManager As IDBManager
        Dim objDataSet As DataSet
        objDbManager = DBManager.GetDatabaseManager()
        objDbManager.SetDBConnection(objeConnections)
        Dim objDBParameters As New clsDBParameters


        objDBParameters.Parameters.Add(New clsDBParameter("@Ticketing_Schedule_ID", hidTSID.Value, "bigint"))
        objDataSet = objDbManager.GetData("UpdateDropTimeinformation", objDBParameters)
        If Not objDataSet Is Nothing Then
            Return objDataSet.Tables(0)
        Else
            Return Nothing
        End If
    End Function

    Private Function GetDropTimeUserInfo() As DataTable

        Dim objeConnections = eConnectionManager.GetConnection()
        Dim objDbManager As IDBManager
        Dim objDataSet As DataSet
        objDbManager = DBManager.GetDatabaseManager()
        objDbManager.SetDBConnection(objeConnections)
        Dim objDBParameters As New clsDBParameters


        objDBParameters.Parameters.Add(New clsDBParameter("@Ticketing_Schedule_ID", hndOnlineTSNo.Value, "bigint"))
        objDataSet = objDbManager.GetData("sp_getDropTimeInfo", objDBParameters)
        If Not objDataSet Is Nothing Then
            Return objDataSet.Tables(0)
        Else
            Return Nothing
        End If


    End Function

    Private Sub UpdatesOnlineVoucher()

        Try


            objTicketing = New clsTicketing(objConnection)
            objOnlineTicketing = New eTicketing

            objTicketing.Id = Val("" & hndOnlineTSNo.Value)

            objTicketing.DriverName = "" & txtDriverName.Text
            objTicketing.HostessName = "" & txtHostessName.Text
            objTicketing.VehicleID = Val("" & cmbVehicle.SelectedValue)
            objTicketing.AccessTerminalId = Val("" & objUser.TerminalId)
            objTicketing.UserId = Val("" & objUser.Id)
            objTicketing.Manual_Vehicle_No = txtVehicle.Text


            Dim OnlineSchedule_Id As Integer = objOnlineTicketing.UpdateOnlineTicketingSchedule(objTicketing)
            hndOnlineTSNo.Value = OnlineSchedule_Id

            lblErr.Text = hndOnlineTSNo.Value
            lblErr.Visible = True


        Catch ex As Exception
            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & line

        End Try

    End Sub


    Private Function ServerPing() As Boolean

        Try
            Dim ping As New Ping
            Dim pingreply As PingReply = ping.Send(FMovers.Ticketing.DAL.Crypto.Decrypt(System.Configuration.ConfigurationManager.AppSettings("ServerIPAddress").ToString, ""))



            If pingreply.Status = IPStatus.Success Then
                Return True
            Else

                Return False
            End If

        Catch ex As Exception
            Return False

        End Try

    End Function
    'Private Sub LoadTimes(ByVal Schedule_Id As String, ByVal TS_Date As String, ByVal TS_Time As String)
    '    Try


    '        'Nauman
    '        Dim table As New DataTable

    '        cboTime.Items.Clear()
    '        objTicketing.ScheduleID = Schedule_Id
    '        objTicketing.TSDate = TS_Date
    '        table = objTicketing.GetDeparture_TimeId()

    '        cboTime.DataSource = table
    '        cboTime.DataValueField = "Ticketing_Schedule_Id"
    '        cboTime.DataTextField = "Departure_Time"
    '        cboTime.DataBind()


    '        'If TS_Time.Trim() <> "" Then
    '        '    If Not cboTime.Items.FindByText(TS_Time) Is Nothing Then
    '        '        cboTime.SelectedItem.Text = TS_Time
    '        '    End If
    '        'ElseIf cboTime.Items.Count > 0 Then
    '        '    cboTime.SelectedIndex = 0
    '        'End If

    '    Catch ex As Exception

    '        lblErr.Text = ex.Message

    '    End Try

    'End Sub
    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        ConnectionManager.CloseConnection(objConnection)

    End Sub

#End Region

#Region " Control Events "

    'Private Sub cboSource_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles cboSource.InitializeLayout, cboDestination.InitializeLayout

    '    CustomizeControl.SetGridLayout(e.Layout.Grid)
    '    With e.Layout.Grid
    '        .Columns.FromKey("City_ID").Hidden = True

    '        .Columns.FromKey("City_Name").Header.Caption = "City Name"
    '        .Columns.FromKey("City_Abbr").Header.Caption = "Abbreviation"
    '    End With
    'End Sub

    Private Sub cboVoucherNo_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs)

        CustomizeControl.SetGridLayout(e.Layout.Grid)
        With e.Layout.Grid
            '.Columns.FromKey("Ticketing_Schedule_ID").Hidden = True
            '.Columns.FromKey("TS_Date").Hidden = True
            '.Columns.FromKey("Schedule_ID").Hidden = True
            ''.Columns.FromKey("Voucher_Status").Hidden = True

            '.Columns.FromKey("Voucher_No").Header.Caption = "Voucher #"
            '.Columns.FromKey("Departure_Time").Header.Caption = "Dep. Time"
            '.Columns.FromKey("Veh_Code").Header.Caption = "Vehicle Code"
            '.Columns.FromKey("Registration_No").Header.Caption = "Reg. #"
            '.Columns.FromKey("Driver_Name").Header.Caption = "Driver"
            '.Columns.FromKey("Hostess_Name").Header.Caption = "Hostess"
            '.Columns.FromKey("Voucher_Status").Header.Caption = "Voucher"
        End With

    End Sub

    Private Sub cboVoucherNo_InitializeRow(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) 'Handles cboVoucherNo.InitializeRow
        'With e.Row.Cells
        '    If .FromKey("Voucher_Status").Value = "2" Then
        '        .FromKey("Voucher_Status").Text = "Closed"
        '    Else
        '        .FromKey("Voucher_Status").Text = "Open"
        '    End If
        'End With
    End Sub

    Private Sub loadComboVechile(ByVal VId As Integer)

        Dim tbVechile As New DataTable
        Dim tbCompany As New DataTable
        Dim tbController As New DataTable


        objVehicle = New clsVehicle(objConnection)
        objVehicle_Controler = New clsVehicleControler(objConnection)

        tbVechile = objVehicle.GetVehicleAdvance()
        tbCompany = objVehicle.GetCompany(cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text)

        tbController = objVehicle_Controler.GetVehicleControlers()

        cmbVehicle.DataSource = tbVechile
        cmbVehicle.DataValueField = "Vehicle_ID"
        cmbVehicle.DataTextField = "Registration_No"
        cmbVehicle.DataBind()


        cmbCompany.DataSource = tbCompany
        cmbCompany.DataValueField = "Vendor_Id"
        cmbCompany.DataTextField = "Vendor_Name"
        cmbCompany.DataBind()

        cmbControlerNumber.DataSource = tbController
        cmbControlerNumber.DataValueField = "ID"
        cmbControlerNumber.DataTextField = "ControlerNo"
        cmbControlerNumber.DataBind()


        'cmbVehicleTransit.DataSource = tbVechile
        'cmbVehicleTransit.DataValueField = "Vehicle_ID"
        'cmbVehicleTransit.DataTextField = "Registration_No"
        'cmbVehicleTransit.DataBind()

        cmbVehicle.Items.Insert(0, New ListItem("Select", "0"))
        For i As Integer = 0 To cmbVehicle.Items.Count - 1
            If cmbVehicle.Items(i).Value = VId Then
                cmbVehicle.Items(i).Selected = True
                hndVechileNo.Value = VId
                Exit For
            End If

        Next



    End Sub

    Private Function GetManualVehicleNo() As DataTable

        Dim objDbManager As IDBManager
        Dim objDataSet As DataSet
        objDbManager = DBManager.GetDatabaseManager()
        objDbManager.SetDBConnection(objConnection)
        Dim objDBParameters As New clsDBParameters
        'Session("TicketingScheduleId") = TicketingScheduleId

        objDBParameters.Parameters.Add(New clsDBParameter("@Ticketing_Schedule_ID", TicketingScheduleId, "bigint"))
        objDataSet = objDbManager.GetData("GetmanualVehicleNodata", objDBParameters)
        If Not objDataSet Is Nothing Then
            Return objDataSet.Tables(0)
        Else
            Return Nothing
        End If




    End Function

    Private Function getCurrentDate() As Boolean

        Dim FinalDate As DateTime
        FinalDate = Convert.ToDateTime(CDate(cboVoucherNo_1.Rows(0).Cells.FromKey("TS_Date").Value.ToString()).Date & " " & cboVoucherNo_1.Rows(0).Cells.FromKey("Departure_Time").Value.ToString() & ":00.000").AddHours(TotalTime.Hours)
        FinalDate = FinalDate.AddMinutes(TotalTime.Minutes)
        If ServerPing() Then

            objOnlineTicketing = New eTicketing
            Dim str_Date As String = ""
            str_Date = objOnlineTicketing.GetCurrentDate()
            If str_Date <> "" Then
                If FinalDate.Date < CDate(str_Date).Date Then
                    DiableTable()
                End If
            End If
        Else

            ' ************************ Updated Date ************************
            If FinalDate.Date < Now.Date Then
                DiableTable()
            End If

        End If
        Return True

    End Function


    Private Sub DiableTable()
        Dim i As Integer
        Dim j As Integer
        hndDisable.Value = "1"
        '' Iterate through the rows of the table.
        For i = 0 To tblTickets.Rows.Count - 1

            ' Iterate through the cells of a row.       
            For j = 0 To tblTickets.Rows(i).Cells.Count - 1

                ' Change the inner HTML of the cell.
                tblTickets.Rows(i).Cells(j).Style.Add("opacity", " 0.5 !important")
            Next j

        Next i

    End Sub
    Private Sub PouplateForm()
        Dim tbmanualbusno As New DataTable
        tbmanualbusno = GetManualVehicleNo()

        Dim row As DataRow = tbmanualbusno.Rows(0)


        txtVoucherNo.Text = "" & cboVoucherNo_1.Rows(0).Cells.FromKey("Voucher_No").Value
        txtVehicleNo.Text = "" & row("Manual_Vehicle_No").ToString()
        ' txtVehicleNo.Text = "" & cboVoucherNo_1.Rows(0).Cells.FromKey("Registration_No").Text
        txtVehicle.Text = "" & row("Manual_Vehicle_No").ToString()
        Veicle_id = "" & cboVoucherNo_1.Rows(0).Cells.FromKey("Vehicle_ID").Text
        txtDriverName.Text = "" & cboVoucherNo_1.Rows(0).Cells.FromKey("Driver_Name").Text
        txtHostessName.Text = "" & cboVoucherNo_1.Rows(0).Cells.FromKey("Hostess_Name").Text
        'txtDepartureDate.Text = "" & cboVoucherNo.Rows(0).Cells.FromKey("TS_Date").Text
        txtDepartureDate.Text = clsUtil.getDateForDisplay(cboVoucherNo_1.Rows(0).Cells.FromKey("TS_Date").Value)
        'txtDepartureDate.Text = Format(CDate(cboVoucherNo.SelectedRow.Cells.FromKey("TS_Date").Text), "dd-mm-yyyy")
        txtDepartureTime.Text = "" & cboVoucherNo_1.Rows(0).Cells.FromKey("Departure_Time").Text

        If "" & cboVoucherNo_1.Rows(0).Cells.FromKey("Actual_Departure_Time").Text <> "" Then
            txtActualDepartureTime.Text = "" & cboVoucherNo_1.Rows(0).Cells.FromKey("Actual_Departure_Time").Text
        Else
            txtActualDepartureTime.Text = txtDepartureTime.Text
        End If

        Call loadCities()
        Call clearValues()
        Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)

        TicketingWapper.Visible = False

        If cboVoucherNo_1.Rows(0).Cells.FromKey("Voucher_Status").Value = "2" Or cboVoucherNo_1.Rows(0).Cells.FromKey("Voucher_Status").Text = "Closed" Then
            tblDeductions.Style.Add("display", "")
            'btnOK.Style.Add("display", "none")
            btnCancel.Style.Add("display", "none")
            btnCloseVoucher.Style.Add("display", "none")
            btnRefresh.Style.Add("display", "none")
            btnSave.Style.Add("display", "none")
            tblTickets.Style.Add("disabled", "disabled")
            cmbCompany.Enabled = False
            btnSaveVehicle.Style.Add("display", "none")

            DiableTable()
            'tblTickets.Visible = False
            '  TicketingWapper.Visible = True
            ' TicketingWapper.InnerHtml = "This vouhcer is closed."

            btnSaveVehicle.Visible = False
            btnOK.Visible = False
            VoucherStatus.Value = "2"
            'Populate the Voucher Deductions

            Call PopulateVoucherDeductions()

        Else
            'btnOK.Style.Add("display", "")

            getCurrentDate()


            btnCancel.Style.Add("display", "")
            tblDeductions.Style.Add("display", "none")
            btnCloseVoucher.Style.Add("display", "")
            btnRefresh.Style.Add("display", "")
            btnSave.Style.Add("display", "")



        End If





    End Sub

    Private Sub cmbDestination_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDestination.SelectedIndexChanged
        Call loadFare()
        Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)
        Call loadDropAt()

        Dim ArrCount As Array = txtSeatNo.Text.Split(",")
        txtCount.Text = ArrCount.Length
        txtTotals.Text = CInt(Val(txtCount.Text)) * CInt(Val(txtFare.Text))

    End Sub

    'Private Sub UpdateV()
    '    Try
    '        'Nauman 
    '        objTicketing.UserId = objUser.Id
    '        objTicketing.ComputerName = "System"
    '        objTicketing.AccessTerminalId = 1

    '        Dim strTime As String = cboTime.SelectedValue
    '        Dim arrTime As Array

    '        arrTime = strTime.Split("~")

    '        With objTicketing
    '            .Id = Request.QueryString("TSID")
    '            If (Not IsDBNull(.Id)) AndAlso .Id <> 0 Then
    '                .GetById()
    '            End If

    '            .ScheduleID = Schedule_ID
    '            '.SerialNo = dRow.Item("Sr_No")
    '            .DepartureTime = cboTime.SelectedItem.Text


    '            If "" & arrTime(1) = "0" Then
    '                .VehicleID = 5
    '            End If
    '            '.DriverName = "" & dRow.Item("Driver_Name")
    '            '.HostessName = "" & dRow.Item("Hostess_Name")
    '        End With

    '        'If objTicketing.VehicleID = 0 Then
    '        '    Response.Write("<script>alert('Please select vehicle first!');</script>")

    '        '    Exit Sub
    '        'End If

    '        objTicketing.Save(False)

    '    Catch ex As Exception

    '    End Try

    'End Sub

    Private Function RandomString(ByVal size As Integer, ByVal lowerCase As Boolean) As String
        Dim builder As New StringBuilder()
        Dim random As New Random()
        Dim ch As Char
        For i As Integer = 0 To size - 1
            ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)))
            builder.Append(ch)
        Next
        If lowerCase Then
            Return builder.ToString().ToLower()
        End If
        Return builder.ToString()
    End Function

    Private Sub validateCNICAndMobileNo(ByVal CNIC As String, ByVal MobileNo As String)
        Try


            Dim dtReturn As New DataSet
            Dim dtReturnOnline As New DataSet

            objOnlineTicketing = New eTicketing

            Dim objWatchList As New clsWatchList(objConnection)

            'objTicketingSeat.CustomerCode = txtCustomerNumber.Text

            dtReturn = objTicketingSeat.validateWatchList(txtCNIC2.Text, txtContactNo.Text)

            If Not dtReturn Is Nothing And dtReturn.Tables(0).Rows.Count > 0 Or dtReturn.Tables(1).Rows.Count > 0 Then

                If dtReturn.Tables.Count > 0 Then



                    If dtReturn.Tables(0).Rows.Count > 0 Then
                        lblWatchList.Text = "Passenger having " & txtCNIC2.Text & " is on watch list. Please contact to terminal manager."
                    End If

                    If dtReturn.Tables(1).Rows.Count > 0 Then
                        lblWatchList.Text = "Passenger having " & txtContactNo.Text & " is on watch list. Please contact to terminal manager."
                    End If

                Else
                    If ServerPing() Then
                        dtReturnOnline = objOnlineTicketing.ValidatewatchList(txtCNIC2.Text, txtContactNo.Text)

                        If Not dtReturnOnline Is Nothing Then
                            If dtReturnOnline.Tables.Count > 0 Then

                                If dtReturn.Tables(0).Rows.Count > 0 Then
                                    lblWatchList.Text = "Passenger having " & txtCNIC2.Text & " is on watch list. Please contact to terminal manager."
                                    objWatchList.ImportAllCities(dtReturnOnline.Tables(0))
                                End If

                                If dtReturn.Tables(1).Rows.Count > 0 Then
                                    lblWatchList.Text = "Passenger having " & txtContactNo.Text & " is on watch list. Please contact to terminal manager."
                                    objWatchList.ImportAllCities(dtReturnOnline.Tables(1))
                                End If

                            End If
                        Else
                            'lblErr.Text = "Customer Not Found !"
                            'hndCustID.Value = 0
                            'txtPassengerName.Text = ""
                            'txtCNIC2.Text = ""
                            'txtContactNo.Text = ""
                        End If


                    Else
                        'lblErr.Text = "Customer Not Found !"
                        'hndCustID.Value = 0
                        'txtPassengerName.Text = ""
                        'txtCNIC2.Text = ""
                        'txtContactNo.Text = ""

                    End If

                End If
            Else

                If ServerPing() Then
                    dtReturnOnline = objOnlineTicketing.ValidatewatchList(txtCNIC2.Text, txtContactNo.Text)
                    If Not dtReturnOnline Is Nothing Then
                        If dtReturnOnline.Tables.Count > 0 Then

                            If dtReturnOnline.Tables(0).Rows.Count > 0 Then
                                lblWatchList.Text = "Passenger having " & txtCNIC2.Text & " is on watch list. Please contact to terminal manager."
                                objWatchList.ImportAllCities(dtReturnOnline.Tables(0))
                            End If

                            If dtReturnOnline.Tables(1).Rows.Count > 0 Then
                                lblWatchList.Text = "Passenger having " & txtContactNo.Text & " is on watch list. Please contact to terminal manager."
                                objWatchList.ImportAllCities(dtReturnOnline.Tables(1))
                            End If

                        End If
                    End If
                End If
            End If



            Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)


        Catch ex As Exception
            lblErr.Text = ex.Message

        End Try




    End Sub
    Private Function validateDiableCount() As Boolean

        objTicketing.Id = Me.TicketingScheduleId

        Dim DiableCount As Integer = objTicketing.DisbaleCount()

        If (DiableCount + 1) > 2 Then
            Return True
        Else
            Return False
        End If

    End Function


    Private Sub btnpospayment_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnpospayment.Click

        'Dim POSPort As String = System.Configuration.ConfigurationManager.AppSettings.Item("POSPort")

        'Dim mySerialPort As SerialPort = New SerialPort(POSPort)

        'mySerialPort.BaudRate = 9600
        'mySerialPort.Parity = Parity.None
        'mySerialPort.StopBits = StopBits.One
        'mySerialPort.DataBits = 8
        'mySerialPort.Handshake = Handshake.None

        'AddHandler mySerialPort.DataReceived, AddressOf DataReceivedHandler

        'mySerialPort.Open()
        'mySerialPort.Write("0200000000000100")
        'Console.WriteLine()
        ''   Dim msg As String = mySerialPort.ReadExisting()

        '' lblDouple.Text = msg
        'mySerialPort.Close()

    End Sub
    Private Sub DataReceivedHandler(sender As Object, e As SerialDataReceivedEventArgs)

        Dim sp As SerialPort = CType(sender, SerialPort)
        Dim indata As String = sp.ReadExisting()
        callmyfunction(indata)
    End Sub
    Private Shared Sub callmyfunction(ByVal data As String)
    End Sub



    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Try



            validateCNICAndMobileNo(txtCNIC2.Text, txtContactNo.Text)

            Dim versionNumber As Version
            Dim Droppoint As Integer = 0

            If cmbDropAt.Items.Count > 1 Then
                Droppoint = cmbDropAt.SelectedItem.Value
            End If


            versionNumber = Assembly.GetExecutingAssembly().GetName().Version




            Dim ArrSeat() As String
            ArrSeat = txtSeatNo.Text.Trim.Split(",")
            ' objFare = New clsFare(objConnection)

            If objUser.CanChangeFare = False Then
                loadFare()

            End If



            If Val(txtFare.Text) = 0 Then
                lblErr.Text = "Please select city !"
                Exit Sub
            End If

            Dim confirmedseats As String = ""
            Dim NotconfirmedSeats As String = ""
            'Dim SeatTicketing As New clsSeatTicketing(

            objTicketingSeat.TicketingScheduleID = cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text

            If txtVoucherNo.Text.Trim() = "" Then

                txtVoucherNo.Text = getNewVoucher(objTicketingSeat.TicketingScheduleID)
                objTicketing.Id = objTicketingSeat.TicketingScheduleID
                objTicketing.VoucherNo = txtVoucherNo.Text.Trim()
                objTicketing.VoucherOpenedBy = objUser.Id
                objTicketing.VoucherStatus = eVoucherStatus.Open
                objTicketing.CreateVoucher()

            End If

            Select Case hidMode.Value
                Case 2
                    objTicketingSeat.Status = eTicketStatus.Booked
                Case Else
                    objTicketingSeat.Status = eTicketStatus.Confirmed
            End Select

            objTicketingSeat.IssueDate = Now
            objTicketingSeat.IssueTerminalID = objUser.TerminalId
            objTicketingSeat.IssuedBy = objUser.Id
            objTicketingSeat.SourceCity = cmbSource.SelectedValue '.SelectedRow.Cells.FromKey("City_ID").Text
            objTicketingSeat.DestinationCity = cmbDestination.SelectedValue '.SelectedRow.Cells.FromKey("City_ID").Text
            objTicketingSeat.PassengerName = txtPassengerName.Text
            objTicketingSeat.PassengerContact = txtContactNo.Text
            objTicketingSeat.CNIC = txtCNIC2.Text

            If rdoGender.SelectedItem.Value <> "Disable" Then
                objTicketingSeat.Gender = rdoGender.SelectedItem.Value.ToString()
            Else
                objTicketingSeat.Gender = "Male"
            End If

            objTicketingSeat.CollectionPoint = cmbTicketCollectPoint.Text
            objTicketingSeat.Customer_Id = Val("" & hndCustID.Value)
            objTicketingSeat.VersionNo = versionNumber.ToString()
            objTicketingSeat.ComputerName = Environment.MachineName
            objTicketingSeat.DiscountAmount = DiscountFare.Text

            objTicketingSeat.ServiceType_Id = hndServiceType.Value
            objTicketingSeat.Is_Online = "0"
            'objTicketingSeat.CollectionPoint = "0"
            objTicketingSeat.Company_Id = objUser.Vendor_Id

            objTicketingSeat.IsDiscounted = IIf(rdoGender.SelectedItem.Value = "Disable", 1, 0)



            objTicketingSeat.PNR_No = DateTime.Now.Date.Day.ToString("00") & DateTime.Now.Date.Month.ToString("00") & RandomString(4, False)

            Dim Seat_No_For_Print As String = ""
            'PrintMultipleInSingle

            Dim PrintMultipleInSingle As String = System.Configuration.ConfigurationManager.AppSettings.Item("PrintMultipleInSingle")
            Dim SeatAlreadyPrinted As Boolean = False
            Dim TotalFare As Integer = 0
            If PrintMultipleInSingle = "Yes" Then
                objFare = New clsFare(objConnection)

                For i As Integer = 0 To ArrSeat.Count - 1

                    If ArrSeat(i) <> "" Then
                        Seat_No_For_Print = Seat_No_For_Print & "," + ArrSeat(i)
                        Dim dtFare As DataTable
                        dtFare = objFare.GetFare(0, cmbSource.SelectedValue, cmbDestination.SelectedValue, Val(cboVoucherNo_1.Rows(0).Cells.FromKey("Schedule_ID").Text), hndServiceType.Value, ArrSeat(i))

                        If Not dtFare Is Nothing Then
                            If dtFare.Rows.Count > 0 Then
                                TotalFare = TotalFare + dtFare.Rows(0)("Fare")
                            End If
                        End If


                        If objTicketing.ValidateTicket(objTicketingSeat.TicketingScheduleID, ArrSeat(i)) <> "0" Then
                            SeatAlreadyPrinted = True
                        End If
                    End If

                Next
                If SeatAlreadyPrinted = False Then
                    If objTicketingSeat.Status <> eTicketStatus.Booked Then
                        PrintReport(Seat_No_For_Print.Substring(1), TotalFare)

                    End If
                Else
                    'lblErr.Text = " Alread sold. "
                    'Exit Sub

                End If

            End If






            For i As Integer = 0 To ArrSeat.Count - 1
                If ArrSeat(i) <> "" Then


                    If rdoGender.SelectedItem.Value = "Disable" Then
                        If validateDiableCount() Then
                            Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)
                            lblErr.Text = "Disable persons can not be more then 2."
                            rdoGender.Items(2).Enabled = False

                            Exit Sub

                        End If
                    End If


                    objTicketingSeat.objConnection = Me.objConnection
                    objTicketingSeat.TicketSrNo = txtTicketNo.Text
                    objTicketingSeat.SeatNo = ArrSeat(i)
                    objTicketingSeat.IssueTerminalID = hidTerminal.Value
                    objTicketingSeat.DropPoint = Droppoint

                    If Request.QueryString("DestinationId") Is Nothing Then
                        Dim KMAmount As Integer


                        Dim dtFare As New DataTable

                        If (objUser.CanTFService = True) Then

                            If objUser.CanChangeFare = False Then
                                dtFare = objFare.GetFare(0, cmbSource.SelectedValue, cmbDestination.SelectedValue, Val(cboVoucherNo_1.Rows(0).Cells.FromKey("Schedule_ID").Text), hndServiceType.Value, ArrSeat(i))
                                Dim row As DataRow = dtFare.Rows(0)
                                KMAmount = row("KM")

                                MYKM = KMAmount
                                objTicketingSeat.KM = KMAmount

                                If objTicketingSeat.IsDiscounted = 1 Then
                                    objTicketingSeat.Fare = (dtFare.Rows(0)("Fare") / 2)
                                Else


                                    objTicketingSeat.Fare = dtFare.Rows(0)("Fare") - KMAmount
                                End If
                            Else
                                objTicketingSeat.Fare = Val(txtFare.Text)
                            End If

                        Else
                            If objUser.CanChangeFare = False Then
                                dtFare = objFare.GetFare(0, cmbSource.SelectedValue, cmbDestination.SelectedValue, Val(cboVoucherNo_1.Rows(0).Cells.FromKey("Schedule_ID").Text), hndServiceType.Value, ArrSeat(i))
                                Dim row As DataRow = dtFare.Rows(0)
                                KMAmount = row("KM")

                                MYKM = KMAmount
                                objTicketingSeat.KM = KMAmount

                                If objTicketingSeat.IsDiscounted = 1 Then
                                    objTicketingSeat.Fare = (dtFare.Rows(0)("Fare") / 2)
                                Else


                                    objTicketingSeat.Fare = dtFare.Rows(0)("Fare")
                                End If
                            Else
                                objTicketingSeat.Fare = Val(txtFare.Text)
                            End If
                        End If




                        If lblCustomerApproved.Text <> "" Then
                            objTicketingSeat.Fare = 0
                        End If

                        Dim StudentName As String
                        StudentName = Request.QueryString("TicketNumber")

                        If Not Request.QueryString("TicketNumber") Is Nothing Then
                            objTicketingSeat.ChangeType = "TicketChange"
                            '  objTicketingSeat.ChangeType = ""
                        Else

                            objTicketingSeat.ChangeType = ""
                            ' objTicketingSeat.ChangeType = "NextDeparture"
                        End If

                    Else
                        objTicketingSeat.ChangeType = "NextDeparture"
                        '  objTicketingSeat.ChangeType = ""

                        objTicketingSeat.Fare = Request.QueryString("Fare")

                    End If

                    If objTicketingSeat.IsReservedOrBooked() Then
                        If chkOnline.Checked Then

                            objTicketing = New clsTicketing(objConnection)
                            objTicketing.Id = TicketingScheduleId

                            objTicketing.GetById()

                            objOnlineTicketing = New eTicketing

                            If hndOnlineTSNo.Value <> 0 Then


                                If Not objOnlineTicketing.CheckReservationFinal(objTicketingSeat, hndOnlineTSNo.Value) Then
                                    lblErr.Text = "This seat is not available"
                                    Exit Sub
                                End If

                                If Not objOnlineTicketing.CheckReservationBooking_New(objTicketingSeat, hndOnlineTSNo.Value) Then
                                    lblErr.Text = "This seat is not available"
                                    Exit Sub

                                Else
                                    If Request.QueryString("DestinationId") Is Nothing Then
                                        ' Ticket Change
                                        If Not Request.QueryString("TicketNumber") Is Nothing Then
                                            If "" & Request.QueryString("TicketNumber") <> "" Then
                                                Dim ChangeTicketNumber As String = Request.QueryString("TicketNumber")
                                                objTicketingSeat.ChangeTicketNumber = Request.QueryString("oldTicketing_SeatId")
                                            End If
                                        End If
                                    ElseIf Not Request.QueryString("DestinationId") Is Nothing Then
                                        If Not Request.QueryString("TicketNumber") Is Nothing Then
                                            If "" & Request.QueryString("TicketNumber") <> "" Then
                                                Dim ChangeTicketNumber As String = Request.QueryString("oldTicketing_SeatId")
                                                objTicketingSeat.ChangeTicketNumber = ChangeTicketNumber
                                            End If
                                        End If
                                    End If

                                    If objTicketingSeat.Status <> eTicketStatus.Booked Then

                                        If objTicketing.ValidateTicket(objTicketingSeat.TicketingScheduleID, objTicketingSeat.SeatNo) = "0" Then

                                            'If objOnlineTicketing.CheckReservation_New(hndOnlineTSNo.Value, objTicketingSeat.SeatNo, cmbSource.SelectedItem.Value, objUser.Vendor_Id) <> eTicketStatus.Available Then
                                            '    lblErr.Text = "This seat is not available"
                                            '    Exit Sub
                                            'End If

                                            If Val("" & hndOnlineTSNo.Value) = 0 Then
                                                lblErr.Text = "Your server connection is very slow. Seats maybe double. Your seat is not avaiable on main server. Please contact IT team."
                                                Exit Sub
                                            End If

                                            If objOnlineTicketing.ValidateSeat(CInt(hndOnlineTSNo.Value), objTicketingSeat.SeatNo, objTicketingSeat.SourceCity, objTicketingSeat.DestinationCity, objTicketingSeat) > 0 Then
                                                lblErr.Text = "Seat number " & objTicketingSeat.SeatNo & " is not avaiable for your desire destination please select right destination to take print."
                                                Exit Sub

                                            End If

                                            If objOnlineTicketing.UpdateTicketingSeatInfo_new(CInt(hndOnlineTSNo.Value), objTicketingSeat, objTicketing.TSDate, objTicketing.DepartureTime, objTicketing.ScheduleID, objUser.Vendor_Id) = True Then

                                                If objOnlineTicketing.ValidateSeatAfterPrint(CInt(hndOnlineTSNo.Value), objTicketingSeat.SeatNo, objTicketingSeat.SourceCity, objTicketingSeat.DestinationCity, objTicketingSeat) = 0 Then
                                                    lblErr.Text = "Your seat was not updated on server please try again ."
                                                    Exit Sub

                                                End If
                                                If hidCustomerPIN.Value <> "" Then objOnlineTicketing.CustomerLog(txtCustomerNumber.Text)

                                                If PrintMultipleInSingle = "No" Then
                                                    If PrintReport(objTicketingSeat.SeatNo, objTicketingSeat.Fare) Then
                                                        objConnection = ConnectionManager.GetConnection()
                                                        objTicketingSeat.objConnection = objConnection
                                                        objTicketingSeat.Is_Online = "1"
                                                        objTicketingSeat.Save(False)
                                                    Else

                                                        Exit Sub

                                                    End If

                                                Else

                                                    objConnection = ConnectionManager.GetConnection()
                                                    objTicketingSeat.objConnection = objConnection
                                                    objTicketingSeat.Is_Online = "1"

                                                    objTicketingSeat.Save(False)

                                                End If

                                            Else
                                                lblErr.Text = "Your server connection is very slow. Seats maybe double. Your seat is not avaiable on main server. Please contact IT team."
                                                Exit Sub

                                            End If
                                        End If

                                    Else


                                        If Val("" & hndOnlineTSNo.Value) = 0 Then
                                            lblErr.Text = "Your server connection is very slow. Seats maybe double. Your seat is not avaiable on main server. Please contact IT team."
                                            Exit Sub
                                        End If

                                        If objOnlineTicketing.ValidateSeatBooking(CInt(hndOnlineTSNo.Value), objTicketingSeat.SeatNo, objTicketingSeat.SourceCity, objTicketingSeat.DestinationCity, objTicketingSeat) > 0 Then
                                            lblErr.Text = "This seat is not avaiable for booking."
                                            Exit Sub

                                        End If

                                        objOnlineTicketing.UpdateTicketingSeatInfo_new(CInt(hndOnlineTSNo.Value), objTicketingSeat, objTicketing.TSDate, objTicketing.DepartureTime, objTicketing.ScheduleID, objUser.Vendor_Id)

                                        objConnection = ConnectionManager.GetConnection()
                                        objTicketingSeat.objConnection = objConnection

                                        objTicketingSeat.Save(False)

                                    End If

                                    ' objTicketingSeat.Save(False)

                                    If confirmedseats = "" Then
                                        confirmedseats = objTicketingSeat.SeatNo
                                    Else
                                        confirmedseats = confirmedseats & "," & objTicketingSeat.SeatNo
                                    End If



                                End If
                            Else
                                'TODO:////////////////////////
                                lblErr.Text = "This Schedule is not Online!"
                                lblErr.Visible = True
                                lnkCreateOnline.Visible = True
                                Exit Sub
                            End If
                        Else
                            If objTicketingSeat.Status <> eTicketStatus.Booked Then

                                If objTicketing.ValidateTicket(objTicketingSeat.TicketingScheduleID, objTicketingSeat.SeatNo) = "0" Then

                                    If PrintMultipleInSingle = "No" Then
                                        If PrintReport(objTicketingSeat.SeatNo, objTicketingSeat.Fare) Then
                                            objTicketingSeat.Save(False)
                                        End If
                                    Else
                                        objTicketingSeat.Save(False)
                                    End If

                                Else
                                    lblErr.Text = "This seat number is already sold."
                                    Exit Sub

                                End If

                                '    ClientScript.RegisterStartupScript([GetType](), "hwa", " printWindows(); ", True)

                            Else
                                objTicketingSeat.Save(False)
                            End If


                            If confirmedseats = "" Then
                                confirmedseats = objTicketingSeat.SeatNo
                            Else
                                confirmedseats = confirmedseats & "," & objTicketingSeat.SeatNo
                            End If
                        End If

                        'If objTicketingSeat.Status = eTicketStatus.Booked Then

                        '    objOnlineTicketing.UpdateTicketingSeatInfo_PNRNO(CInt(hndOnlineTSNo.Value), objTicketingSeat, objTicketingSeat.SeatNo, objTicketingSeat.PNR_No)

                        'End If


                    Else
                        If NotconfirmedSeats = "" Then
                            NotconfirmedSeats = objTicketingSeat.SeatNo
                        Else
                            NotconfirmedSeats = NotconfirmedSeats & "," & objTicketingSeat.SeatNo
                        End If
                    End If
                End If

            Next

            If chkOnline.Checked Then

                'lnkMapping_Click(sender, e)


            End If
            hidRoute.Value = cmbSource.SelectedItem.Text & " To " & cmbDestination.SelectedItem.Text '.SelectedRow.Cells.FromKey("City_Abbr").Text
            'hidPrint.Value = "1"

            If hidMode.Value = "2" Then

                'sendSMS_NoBooking(BookingSMS.Value, confirmedseats, "03/04/2012", txtActualDepartureTime.Text, cmbSource.SelectedItem.Text, cmbDestination.SelectedItem.Text)

            End If

            Select Case hidMode.Value
                Case 1
                    ' sendSMS_No()
                    'objTicketingSeat.Status = eTicketStatus.Booked
                Case Else
                    'objTicketingSeat.Status = eTicketStatus.Confirmed
            End Select


            If objTicketingSeat.Status = eTicketStatus.Confirmed Or objTicketingSeat.Status = eTicketStatus.Booked Then
                If confirmedseats <> "" Then
                    If objTicketingSeat.Status = eTicketStatus.Confirmed Then
                        tdSeatNo.InnerHtml = "Seat #(s):<b>" & confirmedseats & "</b> CONFIRMED."
                        hdnPrint.Value = "1"
                        Response.Write("<script language='javascript'>alert();</script>")
                    ElseIf objTicketingSeat.Status = eTicketStatus.Booked Then
                        tdSeatNo.InnerHtml = "Seat #(s):<b>" & confirmedseats & "</b> BOOKED."
                    End If
                End If

                If NotconfirmedSeats.Trim() <> "" Then
                    If objTicketingSeat.Status = eTicketStatus.Confirmed Then
                        tdExtraComm.InnerHtml = "Seat #(s):<b>" & NotconfirmedSeats & "</b> already Confirmed/ Or not Reserved."
                    ElseIf objTicketingSeat.Status = eTicketStatus.Booked Then
                        tdExtraComm.InnerHtml = "Seat #(s):<b>" & NotconfirmedSeats & "</b> already Booked/ Or not Reserved."
                    End If
                End If

                If txtContactNo.Text = "" Then
                    tdPassenger.InnerText = "Passenger: " & txtPassengerName.Text
                Else
                    tdPassenger.InnerText = "Passenger: " & txtPassengerName.Text & " - (" & txtContactNo.Text & ") " & " PNR Number is  " & objTicketingSeat.PNR_No
                End If

                txtContactNo.Text = ""
                txtPassengerName.Text = ""
                txtSeatNo.Text = ""
                txtTicketNo.Text = ""
                txtCount.Text = ""
                txtCNIC2.Text = ""
                txtTotals.Text = ""
                txtCustomerNumber.Text = ""
                hndCustID.Value = 0
                txtFare.Text = ""
                btnCancelTicket.Visible = False

            End If

            ValidateCustomerPIN.Visible = False
            lblCustomerApproved.Text = ""
            hidCustomerPIN.Value = ""

            txtCNIC2.Enabled = True
            txtContactNo.Enabled = True
            txtCustomerNumber.Enabled = True
            btnValidateCustomers.Enabled = True
            txtPassengerName.Enabled = True
            tblTickets.Disabled = False
            btnReprint.Visible = False
            'btnCancelTicket.Visible = False '.Style.Add("display", "none")
            btnMissed.Visible = False
            hidCustomerPIN.Value = ""
            cmbDestination.SelectedIndex = cmbDestination.Items.Count - 1
            rdoGender.SelectedIndex = 0
            ChangeTicketUdpate()

            Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)

            If hidMode.Value = "1" Then
                btnSave.Text = "Print"
            Else
                btnSave.Text = "Book"
                ' lblDouple.Text = txtSeatNo.Text + " " + "This seat is already reserved and cannot be changed."

            End If

        Catch ex As Exception

            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & line

        End Try
    End Sub

    Private Sub ChangeTicketUdpate()
        Try

            If Not Request.QueryString("TicketNumber") Is Nothing Then
                If "" & Request.QueryString("TicketNumber") <> "" Then

                    Dim ChangeTicketNumber As String = Request.QueryString("TicketNumber")


                    objConnection = ConnectionManager.GetConnection()

                    objTicketingSeat = New clsSeatTicketing(objConnection)

                    objUser = CType(Session("CurrentUser"), clsUser)

                    Dim dt As New DataTable

                    dt = objTicketingSeat.getSeatRecord(ChangeTicketNumber)
                    If Not dt Is Nothing Then


                        If dt.Rows.Count > 0 Then
                            If dt.Rows(0)("Status") <> "4" Then

                                lblErr.Text = "No Record Found."
                            Else


                                Dim OnlineChangeTicketScheId As Integer = loadOnlineVoucherForChange(dt.Rows(0)("Departure_Time"), dt.Rows(0)("TS_Date"), dt.Rows(0)("Schedule_Id"))




                                objTicketingSeat.Ticketing_Seat_ID = dt.Rows(0)("Ticketing_Seat_ID")
                                objTicketingSeat.SeatNo = dt.Rows(0)("Seat_No")
                                objTicketingSeat.IssuedBy = objUser.Id

                                If Request.QueryString("DestinationId") Is Nothing Then
                                    objOnlineTicketing.MakeAvailable_New(CLng(OnlineChangeTicketScheId), dt.Rows(0)("Seat_No"), eTicketStatus.Available, objUser.Id, objUser.TerminalId)
                                    objTicketingSeat.AddMissed("TicketChange")
                                Else
                                    objTicketingSeat.AddMissed("NextDeparture")
                                End If




                                '***************** Clear data from server ******************


                                If Not Request.QueryString("DestinationId") Is Nothing Then
                                    lblErr.Text = "Next Departure Have been done sucessfully. Please close this plan now "
                                Else
                                    lblErr.Text = "Ticket have been changed sucessfully. Please close this plan now "

                                End If
                                'lblErr.Text = "Ticket have been changed sucessfully. Please close this plan now "
                                tblTickets.Visible = False


                            End If
                        Else
                            lblErr.Text = "No Record Found."
                        End If
                    Else
                        lblErr.Text = "No Record Found."

                    End If

                End If
            End If

        Catch ex As Exception
            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & line
        End Try
    End Sub

    Private Sub btnCancelTicket_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelTicket.Click
        Try

            If chkOnline.Checked = False Then
                lblErr.Text = "Booking can not be cancelled offline"
                Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)
                Exit Sub
            End If


            Dim ScheduleID_New As Integer = cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text
            Dim dtSeatDetail As New DataTable
            dtSeatDetail = objTicketingSeat.GetSeatDetail(ScheduleID_New, txtSeatNo.Text)


            If Not Val("" & txtSeatNo.Text) = 0 Then

                If ValidateStatus(Val("" & txtSeatNo.Text)) Then

                    objTicketingSeat.TicketingScheduleID = cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text

                    objTicketingSeat.TicketSrNo = ""
                    objTicketingSeat.SeatNo = txtSeatNo.Text
                    objTicketingSeat.Status = eTicketStatus.Available
                    objTicketingSeat.IssueDate = Now
                    objTicketingSeat.IssueTerminalID = objUser.TerminalId
                    objTicketingSeat.IssuedBy = objUser.Id
                    objTicketingSeat.SourceCity = 0
                    objTicketingSeat.DestinationCity = 0
                    objTicketingSeat.PassengerName = ""
                    objTicketingSeat.PassengerContact = ""
                    objTicketingSeat.Fare = 0


                    'Ticketing Deduction

                    objTicketingSeat.VoucherNo = cboVoucherNo_1.Rows(0).Cells.FromKey("Voucher_No").Text
                    objTicketingSeat.SeatNo = txtSeatNo.Text
                    objTicketingSeat.Deduction = Val(txtDeduction.Text)
                    objTicketingSeat.InsertTicketingDeduction(Convert.ToInt32(cmbDestination.SelectedValue))

                    objTicketingSeat.Save(False)

                    If chkOnline.Checked Then
                        '   Me.lnkMapping_Click(Nothing, Nothing)
                        objOnlineTicketing = New eTicketing
                        Dim ScheduleID As Integer = cboVoucherNo_1.Rows(0).Cells.FromKey("Schedule_Id").Text

                        objOnlineTicketing.BookingCancel(hndOnlineTSNo.Value, txtSeatNo.Text, eTicketStatus.Available, objUser.Id, hidTerminal.Value)

                        Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)

                    Else
                        Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)
                    End If

                Else

                    lblErr.Text = "Booking can not cancelled before 30 mints of your deparutre time."
                End If

            End If

            Call clearValues()
            btnCancelTicket.Visible = False
            btnReprint.Visible = False

            btnMissed.Visible = False
            cmbDestination.SelectedValue = "Please select"
            txtFare.Text = ""
            txtSeatNo.Text = ""
            clearValues()

            btnSkip.Visible = True
            btnSave.Style.Add("Display", "")
            tblTickets.Disabled = False
            btnCancelTicket.Visible = False


        Catch ex As Exception
            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & line

        End Try

    End Sub

    Private Sub lnkSeatDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkSeatDetail.Click



        If hndDisable.Value = "1" Then
            clearValues()
            lblErr.Text = "No action can be taken."
            Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)
            Exit Sub
        End If


        tblTickets.Disabled = False
        tblTickets.Attributes.Add("disabled", "true")

        Dim ScheduleID As Integer = cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text
        Dim dtSeatDetail As New DataTable
        dtSeatDetail = objTicketingSeat.GetSeatDetail(ScheduleID, txtSeatNo.Text)
        btnCancelTicket.Visible = False

        If dtSeatDetail.Rows(0).Item("Status") = 3 Then
            btnCancelTicket.Visible = True
            btnMissed.Visible = False


            txtContactNo.Enabled = False
            txtCustomerNumber.Enabled = False
            btnValidateCustomers.Enabled = False
            txtPassengerName.Enabled = False

        Else
            btnCancelTicket.Visible = False
            btnMissed.Visible = True
        End If


        If Not Request.QueryString("TicketNumber") Is Nothing Then
            Dim strquerySeatNo As String = Request.QueryString("TicketNumber")
            Dim arryquery As Array = strquerySeatNo.Split("-")
            If arryquery(0) = cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text Then
                If arryquery(1) = txtSeatNo.Text Then
                    lblErr.Text = "Same Seat Can not be selected ."
                    txtSeatNo.Text = ""
                    Exit Sub

                End If

            End If

        End If


        'If "" & dtSeatDetail.Rows(0).Item("Status") = "3" Then
        If chkOnline.Checked = True Then

            objTicketing = New clsTicketing(objConnection)
            objTicketing.Id = TicketingScheduleId
            objTicketing.GetById()

            objOnlineTicketing = New eTicketing

            If dtSeatDetail.Rows(0).Item("Status") = "4" And dtSeatDetail.Rows(0).Item("Telenor") = "1" Then

                btnReprint.Visible = True
                btnSave.Visible = False
                lblDouple.Text = " "
                lblboardingpoint.Text = " "
                Label2912555.Visible = False
                txtCustomerNumber.Visible = False
                btnValidateCustomers.Visible = False

                With dtSeatDetail.Rows(0)
                    If IsDBNull(.Item("Ticket_Sr_No")) Then
                        txtTicketNo.Text = ""
                    Else
                        txtTicketNo.Text = "" & .Item("Ticket_Sr_No")
                    End If

                    If IsDBNull(.Item("Passenger_Name")) Then
                        txtPassengerName.Text = ""
                    Else
                        txtPassengerName.Text = "" & .Item("Passenger_Name")
                    End If

                    'If IsDBNull(.Item("CNIC")) Then
                    '    txtCNIC2.Text = ""
                    'Else
                    '    txtCNIC2.Text = "" & .Item("CNIC")
                    'End If
                    txtPassengerName.Enabled = False
                    txtContactNo.Enabled = False
                    If IsDBNull(.Item("Contact_No")) Then
                        txtContactNo.Text = ""
                    Else
                        txtContactNo.Text = "" & .Item("Contact_No")
                    End If

                    If Not IsDBNull(.Item("Destination_ID")) Then
                        cmbDestination.SelectedValue = .Item("Destination_ID")

                    End If

                    If Not IsDBNull(.Item("PNR_No")) Then
                        txtPNR.Text = .Item("PNR_No")
                    End If

                    If (.Item("Genders") = "Female") Then
                        'cmbDestination.SelectedValue = .Item("Destination_ID")
                        rdoGender.SelectedIndex = 1
                    Else
                        rdoGender.SelectedIndex = 0
                    End If

                    If IsDBNull(.Item("Fare")) Then
                        txtFare.Text = ""
                    Else
                        txtFare.Text = Math.Round(.Item("Fare"))

                        Dim ArrCount As Array = txtSeatNo.Text.Split(",")
                        txtCount.Text = ArrCount.Length
                        txtTotals.Text = CInt(Val(txtCount.Text)) * CInt(Val(txtFare.Text))

                    End If

                    objUser = CType(Session("CurrentUser"), clsUser)
                    If objUser.Is_Missed = False Then
                        btnMissed.Visible = False

                    Else

                    End If

                    objUser = CType(Session("CurrentUser"), clsUser)
                    If objUser.Is_Skip = False Then
                        btnSkip.Visible = False

                    Else

                    End If
                    'Genders

                    Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)
                    Exit Sub

                End With



            End If

            '' i am here 
            Dim strStatus As String = objOnlineTicketing.CheckReservation_Very_New(hndOnlineTSNo.Value, txtSeatNo.Text, cmbSource.SelectedItem.Value, objUser.Vendor_Id)

            If (strStatus = 5) Or (strStatus = 0) Then
                lblErr.Text = "Server is offline please make it contact IT team."
                Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)
                Exit Sub
            End If



            'If dtSeatDetail.Rows(0)("issued_by") <> objUser.Id Then

            '    lblErr.Text = "You do not have rights to edit."
            '    Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)
            '    Exit Sub


            'End If


            If dtSeatDetail.Rows(0).Item("Status") <> "4" Then
                If strStatus = "4" Then
                    lblErr.Text = "This seat is not available"
                    Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)
                    Exit Sub
                Else
                    objTicketingSeat.GetById()
                    objTicketingSeat.Status = eTicketStatus.Reserved
                    objTicketingSeat.IssuedBy = objUser.Id
                    objTicketingSeat.IssueTerminalID = objUser.TerminalId
                    objTicketingSeat.UpdateSeatStatus()
                    txtTicketNo.Text = ScheduleID & hidSeatNo.Value
                    'txtTicketNo.Text = objTicketingSeat.GetNewSeatSerialNumber()

                    objOnlineTicketing.UpdateTicketingSeatInfo(objTicketingSeat, objTicketing.TSDate, objTicketing.DepartureTime, objTicketing.ScheduleID)
                End If
            End If
        End If


        'End If

        With dtSeatDetail.Rows(0)
            If IsDBNull(.Item("Ticket_Sr_No")) Then
                txtTicketNo.Text = ""
            Else
                txtTicketNo.Text = "" & .Item("Ticket_Sr_No")
            End If

            If IsDBNull(.Item("Passenger_Name")) Then
                txtPassengerName.Text = ""
            Else
                txtPassengerName.Text = "" & .Item("Passenger_Name")
            End If

            If IsDBNull(.Item("CNIC")) Then
                txtCNIC2.Text = ""
            Else
                txtCNIC2.Text = "" & .Item("CNIC")
            End If


            If IsDBNull(.Item("Contact_No")) Then
                txtContactNo.Text = ""
            Else
                txtContactNo.Text = "" & .Item("Contact_No")
            End If

            If IsDBNull(.Item("Fare")) Then
                txtFare.Text = ""
            Else
                txtFare.Text = Math.Round(.Item("Fare"))

                Dim ArrCount As Array = txtSeatNo.Text.Split(",")
                txtCount.Text = ArrCount.Length
                txtTotals.Text = CInt(Val(txtCount.Text)) * CInt(Val(txtFare.Text))

            End If


            If IsDBNull(.Item("CNIC")) Then
                txtCNIC2.Text = ""
            Else
                txtCNIC2.Text = "" & .Item("CNIC")
            End If

            If Not IsDBNull(.Item("Destination_ID")) Then
                cmbDestination.SelectedValue = .Item("Destination_ID")
                Call loadDropAt()

            End If

            If Not IsDBNull(.Item("PNR_No")) Then
                txtPNR.Text = .Item("PNR_No")
            End If

            If (.Item("Genders") = "Female") Then
                'cmbDestination.SelectedValue = .Item("Destination_ID")
                rdoGender.SelectedIndex = 1
            Else
                rdoGender.SelectedIndex = 0
            End If




            objUser = CType(Session("CurrentUser"), clsUser)
            If objUser.Is_Missed = False Then
                btnMissed.Visible = False

            Else

            End If

            objUser = CType(Session("CurrentUser"), clsUser)
            If objUser.Is_Skip = False Then
                btnSkip.Visible = False

            Else

            End If

            'Genders

        End With

        txtAmount.Text = txtFare.Text

        Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)

        If "" & dtSeatDetail.Rows(0).Item("Status") = "3" Then

            btnCancelTicket.Visible = True
            btnReprint.Visible = False
            btnMissed.Visible = False

            'btnCancelTicket.Text = "Cancel Booking"
            trDeduction.Style.Add("Display", "none")
            btnSave.Style.Add("Display", "")

            '        Call loadDropAt()

        ElseIf "" & dtSeatDetail.Rows(0).Item("Status") = "4" Then

            If (dtSeatDetail.Rows(0)("Issue_Terminal") <> objUser.TerminalId) Then


                If dtSeatDetail.Rows(0)("issued_by") <> objUser.Id Then

                    'btnCancelTicket.Visible = False
                    btnReprint.Visible = False

                    objUser = CType(Session("CurrentUser"), clsUser)
                    If objUser.Is_Missed = False Then
                        btnMissed.Visible = False

                    Else

                    End If

                    objUser = CType(Session("CurrentUser"), clsUser)
                    If objUser.Is_Skip = False Then
                        btnSkip.Visible = False

                    Else

                    End If


                    'btnMissed.Visible = True
                    ' btnSkip.Visible = True



                    trDeduction.Style.Add("Display", "")
                    'btnSave.Style.Add("Display", "none")
                    btnSave.Visible = True

                    'Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)
                    'Exit Sub


                End If


                'lblErr.Text = "You do not have rights to edit."
                'Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)
                'Exit Sub
            Else
                'btnCancelTicket.Visible = True
                btnReprint.Visible = False
                btnMissed.Visible = True
                btnMissed.Visible = True



                btnCancelTicket.Text = "Cancel Ticket"
                btnSkip.Visible = True
                trDeduction.Style.Add("Display", "")
                ' btnSave.Style.Add("Display", "none")

                objUser = CType(Session("CurrentUser"), clsUser)
                If objUser.Is_Missed = False Then
                    btnMissed.Visible = False

                Else

                End If

                objUser = CType(Session("CurrentUser"), clsUser)
                If objUser.Is_Skip = False Then
                    btnSkip.Visible = False

                Else

                End If

            End If

            btnSave.Visible = True
        End If
        tblTickets.Disabled = True

        Dim dtCanRefundMiss As Boolean = clsUtil.GetCanRefundMiss(objConnection)
        If dtCanRefundMiss = True Then btnMissed.Visible = False


    End Sub

    Private Sub btnCloseVoucher_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCloseVoucher.Click
        Try


            objTicketing.Id = hidTSID.Value()
            Dim dt_GetScheduleData As DataTable = objTicketing.GetVoucherTotal(cmbCompany.SelectedValue)

            'Here Nomi
            tblDeductions.Style.Add("Display", "")
            btnCloseVoucher.Style.Add("display", "")
            btnRefresh.Style.Add("display", "")
            btnSave.Style.Add("display", "")

            If hndTimeDrop.Value = 0 Then
                txtcommission.Text = dt_GetScheduleData.Rows(0)("commision").ToString()
                txtComPer.Text = dt_GetScheduleData.Rows(0)("Commission_Rate").ToString()
                txtBKComm.Text = dt_GetScheduleData.Rows(0)("BKCom").ToString()
                txtBKCommPer.Text = dt_GetScheduleData.Rows(0)("BKComPer").ToString()

            Else
                txtcommission.Text = 0
                txtComPer.Text = 0
                txtBKComm.Text = 0
                txtBKCommPer.Text = 0

            End If


            hidTotal.Value = dt_GetScheduleData.Rows(0)("total").ToString()


            txtRefreshment.Focus()



        Catch ex As Exception

        End Try


    End Sub

    Private Sub UploadCloseVoucher()
        Try

            If ServerPing() Then

                Dim ConnString As String
                Dim ConnStringOnline As String
                Dim Connections As SqlClient.SqlConnection
                Dim ConnectionsOnline As SqlClient.SqlConnection





                ConnString = FMovers.Ticketing.DAL.Crypto.Decrypt(ConfigurationManager.ConnectionStrings("FMoversLocal").ConnectionString, "")

                Connections = New SqlConnection(ConnString)

                ConnStringOnline = FMovers.Ticketing.DAL.Crypto.Decrypt(ConfigurationManager.ConnectionStrings("FMoversCentral").ConnectionString, "")

                ConnectionsOnline = New SqlConnection(ConnStringOnline)

                If Connections.State = ConnectionState.Closed Then
                    Connections.Open()
                End If

                Dim Command_Offline As New SqlCommand
                Dim Command_Online As New SqlCommand

                Dim Adapter_Offline As New SqlClient.SqlDataAdapter
                Dim Adapter_Online As New SqlClient.SqlDataAdapter
                Dim dt_OfflineDataTale As New DataTable
                Dim dt_OfflineDataTaleSystem As New DataTable
                Dim dt_OfflineDataTale_DataList As New DataTable
                Dim dt_OnlineDataTale_DataList As New DataTable
                Dim str_Query As String = ""
                Dim Access_Terminal_Id As String = ""

                Dim dtValidate As New DataTable
                'sp_validateTicketingSeatByTS_Id



                str_Query = " Select * from System_Info "

                Command_Offline.Connection = Connections

                Command_Offline.CommandType = CommandType.Text
                Command_Offline.CommandText = str_Query
                Adapter_Offline.SelectCommand = Command_Offline
                Adapter_Offline.Fill(dt_OfflineDataTaleSystem)

                '"  and Is_Pulled IS NULL "
                If dt_OfflineDataTaleSystem.Rows.Count > 0 Then
                    Access_Terminal_Id = dt_OfflineDataTaleSystem.Rows(0)("Default_Terminal")
                End If

                ' ******************************** Start load which are not loaded before ******************************** 
                str_Query = " Select * from Ticketing_Schedule where  " &
                            " Voucher_Status = 2 " &
                            " and Ticketing_Schedule_ID In ( " &
                            " select Ticketing_Schedule_ID from Ticketing_Schedule where  " &
                            "   Ticketing_Schedule.Ticketing_Schedule_ID = '" & cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text & "' and   Ticketing_Schedule.ServiceType_Id = '" & hndServiceType.Value & "' and" &
                            " Ticketing_Schedule.Departure_Time =  '" & txtDepartureTime.Text &
                            "' ) "

                Command_Offline.Connection = Connections
                Command_Offline.CommandType = CommandType.Text
                Command_Offline.CommandText = str_Query
                Adapter_Offline.SelectCommand = Command_Offline


                Adapter_Offline.Fill(dt_OfflineDataTale)




                If ServerPing() Then
                    If ConnectionsOnline.State = ConnectionState.Closed Then
                        ConnectionsOnline.Open()
                    End If
                    Command_Online.Connection = ConnectionsOnline
                End If
                Dim Counter As Integer = 0

                For Each Offline_Dr As DataRow In dt_OfflineDataTale.Rows
                    Counter = Counter + 1


                    '******************** Validate Tckeing Seat ****************



                    str_Query = "sp_validateTicketingSeatByTS_Id"

                    Command_Online.Connection = ConnectionsOnline
                    Command_Online.CommandType = CommandType.StoredProcedure

                    Command_Online.CommandText = str_Query

                    Command_Online.Parameters.Clear()
                    Command_Online.Parameters.Add("@Ticketing_Schedule_ID", SqlDbType.BigInt).Value = Offline_Dr("Ticketing_Schedule_ID")
                    Command_Online.Parameters.Add("@Issue_Terminal", SqlDbType.BigInt).Value = Access_Terminal_Id

                    Adapter_Offline.SelectCommand = Command_Online
                    dtValidate.Clear()
                    Adapter_Offline.Fill(dtValidate)





                    Dim rtn_Value As Integer = 0

                    Command_Online.CommandType = CommandType.StoredProcedure

                    Command_Online.CommandText = "sp_GrandVoucherById"

                    Label1.Text = "uploading " & Offline_Dr("Ticketing_Schedule_ID")


                    Dim ControlerNo As String = Offline_Dr("ControlNumber")


                    Dim ComType As String = Offline_Dr("ComType")
                    Dim ManualBusNo As String = Offline_Dr("Manual_Vehicle_No")

                    Command_Online.Parameters.Clear()
                    Command_Online.Parameters.Add("@Ticketing_Schedule_ID", SqlDbType.BigInt).Value = Offline_Dr("Ticketing_Schedule_ID")
                    Command_Online.Parameters.Add("@TS_Date", SqlDbType.DateTime).Value = Offline_Dr("TS_Date")
                    Command_Online.Parameters.Add("@Schedule_id", SqlDbType.Int).Value = Offline_Dr("Schedule_id")
                    Command_Online.Parameters.Add("@Voucher_No", SqlDbType.NVarChar).Value = Offline_Dr("Voucher_No")
                    Command_Online.Parameters.Add("@Voucher_Status", SqlDbType.NVarChar).Value = Offline_Dr("Voucher_Status")
                    Command_Online.Parameters.Add("@Voucher_Opened_By", SqlDbType.NVarChar).Value = Offline_Dr("Voucher_Opened_By")
                    Command_Online.Parameters.Add("@Voucher_Closed_By", SqlDbType.NVarChar).Value = Offline_Dr("Voucher_Closed_By")
                    Command_Online.Parameters.Add("@Voucher_Closed_Date", SqlDbType.DateTime).Value = Offline_Dr("Voucher_Closed_Date")
                    Command_Online.Parameters.Add("@Depature_Time", SqlDbType.NVarChar).Value = Offline_Dr("Departure_Time")
                    Command_Online.Parameters.Add("@Vehicle_ID", SqlDbType.Int).Value = Offline_Dr("Vehicle_ID")
                    Command_Online.Parameters.Add("@Driver_Name", SqlDbType.NVarChar).Value = Offline_Dr("Driver_Name")
                    Command_Online.Parameters.Add("@Hostess_Name", SqlDbType.NVarChar).Value = Offline_Dr("Hostess_Name")
                    Command_Online.Parameters.Add("@User_Id", SqlDbType.Int).Value = Offline_Dr("User_Id")
                    Command_Online.Parameters.Add("@Access_DateTime", SqlDbType.DateTime).Value = Offline_Dr("Access_DateTime")
                    Command_Online.Parameters.Add("@Access_Sys_Name", SqlDbType.NVarChar).Value = Offline_Dr("Access_Sys_Name")
                    Command_Online.Parameters.Add("@Access_Terminal_Id", SqlDbType.Int).Value = Offline_Dr("Access_Terminal_Id")
                    Command_Online.Parameters.Add("@Actual_Departure_Time", SqlDbType.NVarChar).Value = Offline_Dr("Actual_Departure_Time")
                    Command_Online.Parameters.Add("@ServiceType_Id", SqlDbType.NVarChar).Value = Val("" & Offline_Dr("ServiceType_Id"))
                    Command_Online.Parameters.Add("@BookId", SqlDbType.NVarChar).Value = Val("" & Offline_Dr("Book_Id"))
                    Command_Online.Parameters.Add("@CCNumber", SqlDbType.NVarChar).Value = Val("" & Offline_Dr("CCNumber"))
                    Command_Online.Parameters.Add("@Vendor_Id", SqlDbType.Int).Value = Val("" & Offline_Dr("Vendor_Id"))
                    Command_Online.Parameters.Add("@Ve_Vendor_Id", SqlDbType.Int).Value = Val("" & Offline_Dr("Ve_Vendor_Id"))
                    Command_Online.Parameters.Add("@Online_TSId", SqlDbType.Int).Value = Val("" & Offline_Dr("Online_TSId"))
                    Command_Online.Parameters.Add("@BKCom", SqlDbType.Int).Value = Val("" & Offline_Dr("BKCom"))
                    Command_Online.Parameters.Add("@Manual_Vehicle_No", SqlDbType.NVarChar).Value = ManualBusNo
                    Command_Online.Parameters.Add("@IsDropped", SqlDbType.Bit).Value = Val("" & Offline_Dr("IsDropped"))
                    Command_Online.Parameters.Add("@ControlNumber", SqlDbType.NVarChar).Value = ControlerNo
                    'Command_Online.Parameters.Add("@ControlNumber", SqlDbType.NVarChar).Value = Val("" & Offline_Dr("ControlNumber"))
                    Command_Online.Parameters.Add("@Value", SqlDbType.Int).Value = Val("" & Offline_Dr("Value"))
                    Command_Online.Parameters.Add("@ComType", SqlDbType.NVarChar).Value = ComType
                    Command_Online.Parameters.Add("@TrayApp_No", SqlDbType.NVarChar).Value = "5.30"


                    rtn_Value = Convert.ToInt64(Command_Online.ExecuteScalar())

                    '****************** Get Seats Offline ************************

                    '   LogTextToFileLogFile(Offline_Dr("Ticketing_Schedule_ID"))

                    Dim TerminalId As String = Offline_Dr("Access_Terminal_Id")
                    Dim BookkariId As String = "86"



                    str_Query = " Select * From Ticketing_Seat Where Status = 4 and Ticketing_Schedule_ID = " & Offline_Dr("Ticketing_Schedule_ID") & " and Issue_Terminal in (  " & TerminalId & "," & BookkariId & ")  Order By Seat_No "
                    Command_Offline.CommandType = CommandType.Text
                    Command_Offline.CommandText = str_Query
                    dt_OfflineDataTale_DataList.Clear()
                    dt_OfflineDataTale_DataList.Dispose()

                    Adapter_Offline.SelectCommand = Command_Offline
                    Adapter_Offline.Fill(dt_OfflineDataTale_DataList)



                    '****************** Get Seats Online ************************

                    If dtValidate.Rows(0)(0).ToString() <> dt_OfflineDataTale_DataList.Rows.Count Then

                        str_Query = " delete from Ticketing_Seat_ALL where  Ticketing_Schedule_ID =  " & Offline_Dr("Ticketing_Schedule_ID") & " and Issue_Terminal in (" & TerminalId & "," & BookkariId & ") "

                        Command_Online.CommandType = CommandType.Text
                        Command_Online.CommandText = str_Query
                        Command_Online.ExecuteNonQuery()


                        For Each Offline_Detail_Dr As DataRow In dt_OfflineDataTale_DataList.Rows

                            Command_Online.CommandText = "SP_AddTicketingSeat_ALL"
                            Command_Online.CommandType = CommandType.StoredProcedure
                            Command_Online.Parameters.Clear()

                            Command_Online.Parameters.Add("@Ticketing_Schedule_ID", SqlDbType.BigInt).Value = Offline_Detail_Dr("Ticketing_Schedule_ID")
                            Command_Online.Parameters.Add("@Seat_No", SqlDbType.Int).Value = Offline_Detail_Dr("Seat_No")
                            Command_Online.Parameters.Add("@Status", SqlDbType.Int).Value = Offline_Detail_Dr("Status")
                            Command_Online.Parameters.Add("@Issue_Date", SqlDbType.DateTime).Value = Offline_Detail_Dr("Issue_Date")
                            Command_Online.Parameters.Add("@Issue_Terminal", SqlDbType.Int).Value = Offline_Detail_Dr("Issue_Terminal")
                            Command_Online.Parameters.Add("@Issued_By", SqlDbType.Int).Value = Offline_Detail_Dr("Issued_By")
                            Command_Online.Parameters.Add("@Source_ID", SqlDbType.Int).Value = Offline_Detail_Dr("Source_ID")
                            Command_Online.Parameters.Add("@Destination_ID", SqlDbType.Int).Value = Offline_Detail_Dr("Destination_ID")
                            Command_Online.Parameters.Add("@Passenger_Name", SqlDbType.NVarChar).Value = Offline_Detail_Dr("Passenger_Name")
                            Command_Online.Parameters.Add("@Contact_No", SqlDbType.NVarChar).Value = Offline_Detail_Dr("Contact_No")
                            Command_Online.Parameters.Add("@Fare", SqlDbType.Decimal).Value = Offline_Detail_Dr("Fare")
                            Command_Online.Parameters.Add("@PNR_No", SqlDbType.NVarChar).Value = Offline_Detail_Dr("PNR_No")
                            Command_Online.Parameters.Add("@CNIC", SqlDbType.NVarChar).Value = Offline_Detail_Dr("CNIC")
                            Command_Online.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = Offline_Detail_Dr("Gender")
                            Command_Online.Parameters.Add("@Telenor", SqlDbType.Bit).Value = Offline_Detail_Dr("Telenor")
                            Command_Online.Parameters.Add("@PaymentDate", SqlDbType.DateTime).Value = Offline_Detail_Dr("PaymentDate")
                            Command_Online.Parameters.Add("@Cust_Code", SqlDbType.NVarChar).Value = Offline_Detail_Dr("Customer_Id")
                            Command_Online.Parameters.Add("@Invoice_Id", SqlDbType.NVarChar).Value = Offline_Detail_Dr("Invoice_Id")
                            Command_Online.Parameters.Add("@Is_OnlinePrinted", SqlDbType.NVarChar).Value = Offline_Detail_Dr("Is_OnlinePrinted")
                            Command_Online.Parameters.Add("@OnlinePrint_Terminal_Id", SqlDbType.NVarChar).Value = Offline_Detail_Dr("OnlinePrint_Terminal_Id")
                            Command_Online.Parameters.Add("@OnlinePrint_Date", SqlDbType.NVarChar).Value = Offline_Detail_Dr("OnlinePrint_Date")


                            Command_Online.ExecuteNonQuery()

                            Command_Offline.CommandText = " update Ticketing_Schedule set Is_Pulled = 1 where Ticketing_Schedule_ID = " &
                            Offline_Dr("Ticketing_Schedule_ID")

                            'InputBox(Command_Offline.CommandText, Command_Offline.CommandText, Command_Offline.CommandText)

                            'MessageBox.Show(Command_Offline.CommandText.ToString())

                            Command_Offline.ExecuteNonQuery()

                        Next

                        str_Query = " select * from Bus_Charges where Ticketing_Schedule_Id = " & Offline_Dr("Ticketing_Schedule_ID")
                        Command_Offline.CommandType = CommandType.Text
                        Command_Offline.CommandText = str_Query
                        dt_OfflineDataTale_DataList.Clear()
                        dt_OfflineDataTale_DataList.Dispose()

                        Adapter_Offline.SelectCommand = Command_Offline
                        Adapter_Offline.Fill(dt_OfflineDataTale_DataList)

                        '****************** Get Seats Online ************************

                        If dt_OfflineDataTale_DataList.Rows.Count > 0 Then


                            str_Query = " delete from Bus_Charges_ALL where  Ticketing_Schedule_ID =  " & Offline_Dr("Ticketing_Schedule_ID")
                            Command_Online.CommandType = CommandType.Text
                            Command_Online.CommandText = str_Query
                            Command_Online.ExecuteNonQuery()

                            Command_Online.CommandText = "SP_UpdateBusCharges_ALL"
                            Command_Online.CommandType = CommandType.StoredProcedure
                            Command_Online.Parameters.Clear()

                            Command_Online.Parameters.Add("@Ticketing_Schedule_ID", SqlDbType.BigInt).Value = dt_OfflineDataTale_DataList.Rows(0)("Ticketing_Schedule_ID")
                            Command_Online.Parameters.Add("@Hostess_Salary", SqlDbType.Decimal).Value = dt_OfflineDataTale_DataList.Rows(0)("Hostess_Salary")
                            Command_Online.Parameters.Add("@Driver_Salary", SqlDbType.Decimal).Value = dt_OfflineDataTale_DataList.Rows(0)("Driver_Salary")
                            Command_Online.Parameters.Add("@Guard_Salary", SqlDbType.Decimal).Value = dt_OfflineDataTale_DataList.Rows(0)("Guard_Salary")
                            Command_Online.Parameters.Add("@Service_Charges", SqlDbType.Decimal).Value = dt_OfflineDataTale_DataList.Rows(0)("Service_Charges")
                            Command_Online.Parameters.Add("@Cleaning_Charges", SqlDbType.Decimal).Value = dt_OfflineDataTale_DataList.Rows(0)("Cleaning_Charges")
                            Command_Online.Parameters.Add("@Hook_Charges", SqlDbType.Decimal).Value = dt_OfflineDataTale_DataList.Rows(0)("Hook_Charges")
                            Command_Online.Parameters.Add("@Bus_Charges", SqlDbType.Decimal).Value = dt_OfflineDataTale_DataList.Rows(0)("Bus_Charges")
                            Command_Online.Parameters.Add("@Toll", SqlDbType.Decimal).Value = dt_OfflineDataTale_DataList.Rows(0)("Toll")
                            Command_Online.Parameters.Add("@CashPaidToDriver", SqlDbType.Bit).Value = dt_OfflineDataTale_DataList.Rows(0)("CashPaidToDriver")

                            Command_Online.Parameters.Add("@Access_Terminal_Id", SqlDbType.Decimal).Value = Access_Terminal_Id
                            Command_Online.Parameters.Add("@Refreshment", SqlDbType.Decimal).Value = dt_OfflineDataTale_DataList.Rows(0)("Refreshment")
                            Command_Online.Parameters.Add("@Terminal_Expense", SqlDbType.Decimal).Value = dt_OfflineDataTale_DataList.Rows(0)("Terminal_Expense")
                            Command_Online.Parameters.Add("@Reward", SqlDbType.Decimal).Value = dt_OfflineDataTale_DataList.Rows(0)("Reward")
                            Command_Online.Parameters.Add("@Misc", SqlDbType.Decimal).Value = dt_OfflineDataTale_DataList.Rows(0)("Misc")
                            Command_Online.Parameters.Add("@DriverPaidAmount", SqlDbType.Decimal).Value = Val("" & dt_OfflineDataTale_DataList.Rows(0)("DriverPaidAmount"))


                            '@CashPaidToDriver
                            Command_Online.ExecuteNonQuery()


                        End If
                    End If

                Next

                ' ******************************** End load which are not loaded before ******************************** 
                'InProcessing = False
            End If

            'sendSMS("Voucher Have been pulled sucessfully." + Now.Date.ToString("dd/MM/yyyy") + " " + Now.TimeOfDay.ToString())

        Catch ex As Exception

            Response.Write(ex.Message)


        Finally

        End Try
    End Sub

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click

        'lnkMapping_Click(sender, e)

        If String.IsNullOrEmpty(txtCCNumber.Text) Then

            lblCCP.Text = "Please Enter CCP Number."

        Else


            btnOK.Enabled = False

            objTicketingSeat.TicketingScheduleID = cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text
            objTicketingSeat.Actual_Departure_Time = txtActualDepartureTime.Text
            objTicketingSeat.Hostess_Name = txtHostessName.Text
            objTicketingSeat.Vehicle_ID = cmbVehicle.SelectedItem.Value
            objTicketingSeat.Driver_Name = txtDriverName.Text
            objTicketingSeat.Guard = txtGuard.Text

            objTicketingSeat.UpdateActualTime()


            Dim objBusCharges As New clsBusCharges(objConnection)

            objBusCharges.TicketingScheduleId = cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text
            objBusCharges.GetByTicketingScheduleId()

            With objBusCharges

                .TicketingScheduleId = cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text
                .HostessSalary = txtHostessSalary.Text
                .DriverSalary = "0" & txtDriverSalary.Text
                .GuardSalary = "0" & txtGuardSalary.Text
                .ServiceCharges = "0" & txtServiceCharges.Text
                .CleaningCharges = "0" & txtCleaningCharges.Text
                .HookCharges = "0" & txtHookCharges.Text
                .BusCharges = "0" & txtBusCharges.Text
                .Refreshment = "0" & txtRefreshment.Text
                .Toll = "0" & txtToll.Text
                .PaidToDriver = "0" & txtPaidToDriver.Text
                .Commision = "0" & txtcommission.Text

                .Terminal_Expense = "0" & txtTerminalExpense.Text
                .Reward = "0" & txtReward.Text
                .Misc = "0" & txtMisc.Text

                .CashPaidToDriver = IIf(chkDriverCash.Checked = True, 1, 0)

                If objBusCharges.Id Is Nothing Then
                    .Save(True)
                Else
                    .Save(False)
                End If
            End With

            Dim objTicketing As New clsTicketing(objConnection)

            objTicketingSeat.TicketingScheduleID = cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text

            If txtVoucherNo.Text.Trim() = "" Then

                txtVoucherNo.Text = getNewVoucher(objTicketingSeat.TicketingScheduleID)
                objTicketing.Id = objTicketingSeat.TicketingScheduleID
                objTicketing.VoucherNo = txtVoucherNo.Text.Trim()
                objTicketing.VoucherOpenedBy = objUser.Id
                objTicketing.VoucherStatus = eVoucherStatus.Open
                objTicketing.CreateVoucher()

            End If

            objTicketing.Id = cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text
            objTicketingSeat.VoucherNo = cboVoucherNo_1.Rows(0).Cells.FromKey("Voucher_No").Text
            objTicketing.VoucherStatus = eVoucherStatus.Closed
            objTicketing.VoucherClosedBy = objUser.Id
            objTicketing.ComputerName = "PC1"
            objTicketing.ActualDepartureTime = txtActualDepartureTime.Text.Trim()
            objTicketing.AccessTerminalId = objUser.TerminalId
            objTicketing.CCNumber = txtCCNumber.Text
            objTicketing.CargoCash = txtcargocash.Text
            objTicketing.CargoCommission = txtcargocommission.Text
            objTicketing.CargoIncome = txtcargoIncom.Text
            objTicketing.Manual_Vehicle_No = txtVehicle.Text
            objTicketing.ControlNumber = cmbControlerNumber.SelectedItem.Text


            objTicketing.C_Vendor_Id = cmbCompany.SelectedValue
            objTicketing.Online_TSId = hndOnlineTSNo.Value


            'BKCom
            objTicketing.CloseVoucher()
            VoucherStatus.Value = "2"

            'send sms closing Voucher
            'CloseNo.Value
            Dim Arr As Array
            Arr = CloseNo.Value.Split(",")

            'For i As Int64 = 0 To Arr.Length - 1

            '    sendSMS_VoucherClose(Arr(i))

            'Next

            'objTicketingSeat.VoucherNo = cboVoucherNo.Rows(0).Cells.FromKey("Voucher_No").Text
            'objTicketingSeat.VoucherStatus = eVoucherStatus.Closed
            'objTicketingSeat.VoucherClosedBy = objUser.Id
            'objTicketingSeat.CloseVoucher()

            ' Update Online 
            'If ConfigurationManager.AppSettings("UpdateServer").ToString() = "Yes" Then
            '    'UpdateOnline

            '    objTicketing.Id = TicketingScheduleId
            '    objTicketing.GetById()
            '    objOnlineTicketing = New eTicketing
            '    'objTicketingSeat.UpdateOnline(CDate(txtDepartureDate.Text), txtDepartureTime.Text, Schedule_ID)
            '    objOnlineTicketing.UpdateOnline(objTicketing.ScheduleID, objTicketing.TSDate, objTicketing.DepartureTime)
            'End If



            If (ServerPing()) Then
                UpdatesOnlineVoucher()
                lnkMapping_Click(sender, e)

            End If

            Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)

            tdOKCancel.Style.Add("Display", "none")
            btnOK.Enabled = True
            'sendSMS()
            UploadCloseVoucher()

        End If

    End Sub
    Private Sub sendSMS()
        Try

            Dim objSMS As New clsSeatTicketing(objConnection)
            Dim tbl_SMS As New DataTable

            Dim comm As New GsmCommMain(hidSMSDataPort.Value, 9600, 300)



            tbl_SMS = objSMS.GetRecordForSMS(TicketingScheduleId)

            Dim pdu As SmsSubmitPdu

            'SP_GetRecordsForSMS

            ' The extended version with dcs
            Dim dcs As Byte

            Dim alert As Boolean = False
            Dim unicode As Boolean = False


            For Each dr As DataRow In tbl_SMS.Rows

                If Not alert AndAlso Not unicode Then

                    ' The straightforward version
                    ' "" indicate SMSC No

                    pdu = New SmsSubmitPdu(hidSMSData.Value, dr("Contact_No"), "")

                    'pdu. = Len(hidSMSData.Value)
                Else
                    ' The extended version with dcs
                    If Not alert AndAlso unicode Then
                        dcs = DataCodingScheme.NoClass_16Bit
                    ElseIf alert AndAlso Not unicode Then
                        dcs = DataCodingScheme.Class0_7Bit
                    ElseIf alert AndAlso unicode Then
                        dcs = DataCodingScheme.Class0_16Bit
                    Else
                        dcs = DataCodingScheme.NoClass_7Bit
                    End If
                    ' should never occur here
                    pdu = New SmsSubmitPdu(hidSMSData.Value, dr("Contact_No"), "", dcs)
                End If

                comm.Open()

                comm.SendMessage(pdu)
                comm.Close()

            Next



            'GsmCommMain comm = new GsmCommMain(3, 9600, 300);
            'GsmComm.GsmCommunication.GsmCommMain sss =new GsmCommMain();





        Catch ex As Exception

            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & line

        End Try
    End Sub

    Private Sub sendSMS_No()
        Try

            If txtContactNo.Text = "" Then Exit Sub

            Dim objSMS As New clsSeatTicketing(objConnection)
            Dim tbl_SMS As New DataTable
            Dim comm As New GsmCommMain(hidSMSDataPort.Value, 9600, 300)

            tbl_SMS = objSMS.GetRecordForSMS(TicketingScheduleId)

            Dim pdu As SmsSubmitPdu

            'SP_GetRecordsForSMS

            ' The extended version with dcs
            Dim dcs As Byte

            Dim alert As Boolean = False
            Dim unicode As Boolean = False


            If Not alert AndAlso Not unicode Then

                ' The straightforward version
                ' "" indicate SMSC No

                pdu = New SmsSubmitPdu(hidSMSData.Value, txtContactNo.Text, "")

                'pdu. = Len(hidSMSData.Value)
            Else
                ' The extended version with dcs
                If Not alert AndAlso unicode Then
                    dcs = DataCodingScheme.NoClass_16Bit
                ElseIf alert AndAlso Not unicode Then
                    dcs = DataCodingScheme.Class0_7Bit
                ElseIf alert AndAlso unicode Then
                    dcs = DataCodingScheme.Class0_16Bit
                Else
                    dcs = DataCodingScheme.NoClass_7Bit
                End If
                ' should never occur here
                pdu = New SmsSubmitPdu(hidSMSData.Value, txtContactNo.Text, "", dcs)
            End If
            comm.Open()





            comm.SendMessage(pdu)
            comm.Close()

            'GsmCommMain comm = new GsmCommMain(3, 9600, 300);
            'GsmComm.GsmCommunication.GsmCommMain sss =new GsmCommMain();

        Catch ex As Exception

            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & line

        End Try
    End Sub

    Private Sub sendSMS_NoBooking(ByVal sendtext As String, ByVal seatNo As String, ByVal strdate As String, ByVal strTime As String, ByVal str_from As String, ByVal str_To As String)
        Try

            If txtContactNo.Text = "" Then Exit Sub

            Dim Final_text As String = ""
            Dim objSMS As New clsSeatTicketing(objConnection)
            Dim tbl_SMS As New DataTable



            Dim comm As New GsmCommMain(hidSMSDataPort.Value, 9600, 300)
            If comm.IsOpen Then
                comm.Close()
            End If

            Final_text = sendtext.Replace("#no", seatNo)
            Final_text = Final_text.Replace("#date", bkDate.Value)
            Final_text = Final_text.Replace("#time", strTime)
            Final_text = Final_text.Replace("#desc", str_from & " To " & str_To)


            tbl_SMS = objSMS.GetRecordForSMS(TicketingScheduleId)

            Dim pdu As SmsSubmitPdu

            'SP_GetRecordsForSMS

            ' The extended version with dcs
            Dim dcs As Byte

            Dim alert As Boolean = False
            Dim unicode As Boolean = False


            If Not alert AndAlso Not unicode Then

                ' The straightforward version
                ' "" indicate SMSC No

                pdu = New SmsSubmitPdu(Final_text, txtContactNo.Text, "")

                'pdu. = Len(hidSMSData.Value)
            Else
                ' The extended version with dcs
                If Not alert AndAlso unicode Then
                    dcs = DataCodingScheme.NoClass_16Bit
                ElseIf alert AndAlso Not unicode Then
                    dcs = DataCodingScheme.Class0_7Bit
                ElseIf alert AndAlso unicode Then
                    dcs = DataCodingScheme.Class0_16Bit
                Else
                    dcs = DataCodingScheme.NoClass_7Bit
                End If
                ' should never occur here
                pdu = New SmsSubmitPdu(Final_text, txtContactNo.Text, "", dcs)
            End If
            comm.Open()

            comm.SendMessage(pdu)
            comm.Close()

            'GsmCommMain comm = new GsmCommMain(3, 9600, 300);
            'GsmComm.GsmCommunication.GsmCommMain sss =new GsmCommMain();

        Catch ex As Exception

            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & line

        End Try
    End Sub

    Private Sub sendSMS_ClosingVouvher()
        Try

            Dim Final_text As String = ""
            Dim objSMS As New clsSeatTicketing(objConnection)
            Dim tbl_SMS As New DataTable
            Dim comm As New GsmCommMain(hidSMSDataPort.Value, 9600, 300)

            tbl_SMS = objSMS.GetRecordForCloseVoucher(TicketingScheduleId)

            Dim pdu As SmsSubmitPdu

            'SP_GetRecordsForSMS

            ' The extended version with dcs
            Dim dcs As Byte

            Dim alert As Boolean = False
            Dim unicode As Boolean = False


            If Not alert AndAlso Not unicode Then

                ' The straightforward version
                ' "" indicate SMSC No

                pdu = New SmsSubmitPdu(Final_text, txtContactNo.Text, "")

                'pdu. = Len(hidSMSData.Value)
            Else
                ' The extended version with dcs
                If Not alert AndAlso unicode Then
                    dcs = DataCodingScheme.NoClass_16Bit
                ElseIf alert AndAlso Not unicode Then
                    dcs = DataCodingScheme.Class0_7Bit
                ElseIf alert AndAlso unicode Then
                    dcs = DataCodingScheme.Class0_16Bit
                Else
                    dcs = DataCodingScheme.NoClass_7Bit
                End If
                ' should never occur here
                pdu = New SmsSubmitPdu(Final_text, txtContactNo.Text, "", dcs)
            End If
            comm.Open()

            comm.SendMessage(pdu)
            comm.Close()

            'GsmCommMain comm = new GsmCommMain(3, 9600, 300);
            'GsmComm.GsmCommunication.GsmCommMain sss =new GsmCommMain();

        Catch ex As Exception

            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & line

        End Try
    End Sub

    Private Sub sendSMS_VoucherClose(ByVal Number As String)
        Try


            Dim Final_text As String = ""
            Dim objSMS As New clsSeatTicketing(objConnection)

            Dim tbl_SMS As New DataTable

            Dim comm As New GsmCommMain(hidSMSDataPort.Value, 9600, 300)
            If comm.IsOpen Then
                comm.Close()
            End If


            tbl_SMS = objSMS.GetRecordForCloseVoucher(TicketingScheduleId)
            Final_text = ""
            'Final_text = CloseSMS.Value.Replace("#Time", tbl_SMS.Rows(0)(0))

            Final_text = Final_text & Desct.Value & vbCrLf
            Final_text = Final_text & "Bus No . " & cmbVehicle.SelectedItem.Text & vbCrLf
            Final_text = Final_text & "Date . " & txtDepartureDate.Text

            Final_text = Final_text & "Time . " & txtActualDepartureTime.Text & vbCrLf
            'Final_text = Final_text & tbl_SMS.Rows(0)(0)

            For Each dr As DataRow In tbl_SMS.Rows

                Final_text = Final_text & dr("Terminal_Name") & "  " & dr("Seat_No") & vbCrLf

            Next

            Final_text = Final_text & " Total " & tbl_SMS.Rows(0)("Total") & vbCrLf

            '            Final_text = CloseSMS.Value.Replace("#seat", tbl_SMS.Rows(0)(0))

            '           Final_text = Final_text.Replace("#bsno", cmbVehicle.SelectedItem.Text)

            'Final_text = Final_text & " Route  " & Desct.Value & " at time " & txtActualDepartureTime.Text


            Dim pdu As SmsSubmitPdu

            'SP_GetRecordsForSMS

            ' The extended version with dcs
            Dim dcs As Byte

            Dim alert As Boolean = False
            Dim unicode As Boolean = False

            If Not alert AndAlso Not unicode Then

                ' The straightforward version
                ' "" indicate SMSC No

                pdu = New SmsSubmitPdu(Final_text, Number, "")

                'pdu. = Len(hidSMSData.Value)
            Else
                ' The extended version with dcs
                If Not alert AndAlso unicode Then
                    dcs = DataCodingScheme.NoClass_16Bit
                ElseIf alert AndAlso Not unicode Then
                    dcs = DataCodingScheme.Class0_7Bit
                ElseIf alert AndAlso unicode Then
                    dcs = DataCodingScheme.Class0_16Bit
                Else
                    dcs = DataCodingScheme.NoClass_7Bit
                End If
                ' should never occur here
                pdu = New SmsSubmitPdu(Final_text, Number, "", dcs)

            End If
            comm.Open()

            comm.SendMessage(pdu)
            comm.Close()

            'GsmCommMain comm = new GsmCommMain(3, 9600, 300);
            'GsmComm.GsmCommunication.GsmCommMain sss =new GsmCommMain();

        Catch ex As Exception

            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & line

        End Try
    End Sub

    Private Sub getCCP_No()



        Dim hwr As HttpWebRequest

        Dim IPAddress As String
        IPAddress = FMovers.Ticketing.DAL.Crypto.Decrypt(System.Configuration.ConfigurationManager.AppSettings("ServerIPAddress").ToString, "")

        Dim myUri As New Uri("http://" + IPAddress + ":7500/api/GetTicketingScheduleCCP?Schedule_Id=" & cboVoucherNo_1.Rows(0).Cells.FromKey("Schedule_ID").Value & "&Servicetype_Id=" & hndServiceType.Value & "&Company_Id=" & objUser.Vendor_Id & "&Sch_Time=" & txtDepartureTime.Text & "")

        'hwr = WebRequest.Create("http://203.130.22.170:7500/api/GetTicketingScheduleCCP?Schedule_Id=" & cboVoucherNo_1.Rows(0).Cells.FromKey("Schedule_ID").Value & "&Servicetype_Id=" & hndServiceType.Value & "&Company_Id=" & objUser.Vendor_Id & "&Sch_Time=" & txtDepartureTime.Text & "")
        hwr = WebRequest.Create(myUri)
        hwr.ReadWriteTimeout = 300

        Try
            Dim wr As WebResponse
            wr = hwr.GetResponse()

            If CType(wr, HttpWebResponse).StatusCode = HttpStatusCode.OK Then
                Dim st As Stream
                st = wr.GetResponseStream()
                Dim sr As StreamReader
                sr = New StreamReader(st)
                Dim res As String
                res = sr.ReadToEnd()
                If Not res Is Nothing Then
                    txtCCNumber.Text = res.Replace("CCP_NO", "")
                    txtCCNumber.Text = txtCCNumber.Text.Replace("""", "")
                    txtCCNumber.Text = txtCCNumber.Text.Replace("[{:", "")
                    txtCCNumber.Text = txtCCNumber.Text.Replace("}]", "")


                    lblheader.Text = "CCP NO : " & txtCCNumber.Text & " " & lblheader.Text
                End If
            End If
        Catch ex As Exception
            '...handle error...
        End Try


        'Dim SMS_Text As String
        'SMS_Text = ""

        'Dim username = "03158675222"
        'Dim password = "fm11234"
        'Dim fromM = "BOOKKARU"

        ''Dim toM = "923214057541"


        'Dim message = SMS_Text
        'Dim urlString As String = String.Format("https://rg.faisalmovers.com:85/ords/wms/sch/TicketingSchedules?Schedule_Id=360&Servicetype_Id=5&Company_Id=1&Sch_Time=23:30")
        'Dim URI = New Uri(urlString)
        'Dim wc As New WebClient()
        'Dim res = wc.DownloadString(URI)



    End Sub

    Private Sub sendSMS_CustomerPin(ByVal Number As String, ByVal PIN As String)
        Try

            Dim SMS_Text As String
            SMS_Text = "Your Pin is " & PIN

            Dim username = "03158675222"
            Dim password = "fm11234"
            Dim fromM = "BOOKKARU"

            'Dim toM = "923214057541"
            Dim toM = Number

            Number = "92" + Number.Substring(1)

            Dim message = SMS_Text
            Dim urlString As String = String.Format("http://weblogin.premiumsms.pk/sendsms_url.html?Username=03158675222&Password=fm11234&From=FAISALMOVER&To=" & Number & "&Message=" & SMS_Text)
            Dim URI = New Uri(urlString)
            Dim wc As New WebClient()
            Dim res = wc.DownloadString(URI)


        Catch ex As Exception

            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & line

        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles btnCancel.Click
        tblDeductions.Style.Add("Display", "none")
    End Sub

    Private Sub lnkReserve_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkReserve.Click
        Try

            'cmbDestination.SelectedIndex = cmbDestination.Items.Count - 1

            Response.Cache.SetCacheability(HttpCacheability.NoCache)



            If hndDisable.Value = "1" Then
                clearValues()
                lblErr.Text = "No action can be taken."
                Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)
                Exit Sub
            End If



            Dim ScheduleID As Integer = cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text
            Dim dtSeatDetail As New DataTable
            Dim res As Integer

            objTicketingSeat.TicketingScheduleID = ScheduleID
            objTicketingSeat.SeatNo = hidSeatNo.Value
            dtSeatDetail = objTicketingSeat.GetSeatStatus()

            'Dim ArrSeats() As String = txtSeatNo.Text.Trim().Split(",")
            'txtSeatNo.Text = ""

            If dtSeatDetail.Rows.Count = 0 Then
                lblErr.Text = "This seat is already RESERVED"
            Else
                If "" & dtSeatDetail.Rows(0)("Route_Sr_No") <> "" Then

                    dtRouteList = objTicketingSeat.GetCitiesTransit(cboVoucherNo_1.Rows(0).Cells.FromKey("Schedule_Id").Text, hidSource.Value, 1, dtSeatDetail.Rows(0)("Route_Sr_No"))

                    cmbDestination.DataSource = dtRouteList
                    cmbDestination.DataBind()
                    cmbDestination.Items.Add("Please select")
                    loadDropAt()
                    'btnRefresh.Enabled = False 
                End If



                ''Online checking
                If chkOnline.Checked = True Then

                    objTicketing = New clsTicketing(objConnection)
                    objTicketing.Id = TicketingScheduleId

                    objTicketing.GetById()

                    objOnlineTicketing = New eTicketing
                    'Verify Online Ticketing Schedule existance
                    If "" & dtSeatDetail.Rows(0)("Route_Sr_No") = "" Then
                        res = objOnlineTicketing.IsOnlineTicketingSchedule(objTicketing, Val("" & hndServiceType.Value), objUser.Vendor_Id)
                    Else
                        res = 1
                    End If
                    ''
                    If res = 1 Then
                        If 1 = 2 Then
                            '    If hndOnlineTSNo.Value = "" Or hndOnlineTSNo.Value = "0" Then

                            lblErr.Text = "This Schedule is not Online!"
                            lblErr.Visible = True
                            lnkCreateOnline.Visible = True
                            txtSeatNo.Text = ""

                        Else
                            If Val(hndOnlineTSNo.Value) = 0 Then
                                loadOnlineVoucher()
                            End If


                            If objOnlineTicketing.CheckReservation_New(hndOnlineTSNo.Value, objTicketingSeat.SeatNo, cmbSource.SelectedItem.Value, objUser.Vendor_Id) <> eTicketStatus.Available And "" & dtSeatDetail.Rows(0)("Route_Sr_No") = "" Then

                                lblErr.Text = "This seat is not available"
                            Else

                                objTicketingSeat.Id = TicketingScheduleId
                                objTicketingSeat.GetById()
                                objTicketingSeat.Status = eTicketStatus.Reserved
                                objTicketingSeat.IssuedBy = objUser.Id
                                objTicketingSeat.IssueTerminalID = objUser.TerminalId
                                objTicketingSeat.UpdateSeatStatus()

                                Dim OnlineUpdateReturn As Boolean = objTicketingSeat.UpdateSeatStatus()

                                If OnlineUpdateReturn Then
                                    If Val(hndOnlineTSNo.Value) = 0 Then
                                        loadOnlineVoucher()
                                    End If

                                    objTicketingSeat.SourceCity = cmbSource.SelectedValue
                                    objTicketingSeat.IssueTerminalID = objUser.TerminalId

                                    objOnlineTicketing.UpdateTicketingSeatInfo_Reservse(CInt(Val(hndOnlineTSNo.Value)), objTicketingSeat, objTicketing.TSDate, objTicketing.DepartureTime, objTicketing.ScheduleID, objUser.Vendor_Id)
                                    txtTicketNo.Text = ScheduleID & hidSeatNo.Value

                                Else
                                    lblErr.Text = "Server is offline please make it contact IT team"
                                End If
                            End If
                        End If


                    ElseIf res = 2 Then

                        'TODO:////////////////////////
                        lblErr.Text = "This Schedule is not Online!"
                        lblErr.Visible = True
                        lnkCreateOnline.Visible = True
                        txtSeatNo.Text = ""
                    Else
                        '' Response.Write("<script language='javascript'>alert('Server is not online please contact to IT team');</script>");

                        'Page.ClientScript.RegisterStartupScript(Me.[GetType](), "click", "alert('Informations');", True)
                        lblErr.Text = "Server is not online please contact to IT team."
                        lblErr.Visible = True

                    End If

                    chkOnline.Checked = True
                    lnkMapping.Style.Add("DISPLAY", "")
                Else
                    objTicketingSeat.Status = eTicketStatus.Reserved
                    objTicketingSeat.IssuedBy = objUser.Id
                    objTicketingSeat.UpdateSeatStatus()

                    txtTicketNo.Text = ScheduleID & hidSeatNo.Value

                    Dim ArrCount As Array = txtSeatNo.Text.Split(",")
                    txtCount.Text = ArrCount.Length
                    txtTotals.Text = CInt(Val(txtCount.Text)) * CInt(Val(txtFare.Text))

                    'txtTicketNo.Text = objTicketingSeat.GetNewSeatSerialNumber()
                End If
            End If

            If Not Request.QueryString("DestinationId") Is Nothing Then

                cmbDestination.SelectedValue = Request.QueryString("DestinationId")
                cmbDestination.Enabled = False
                Dim NDFare As Integer = Request.QueryString("Fare")
                txtFare.Text = Math.Round(NDFare)


                Dim ArrCount As Array = txtSeatNo.Text.Split(",")
                txtCount.Text = ArrCount.Length
                txtTotals.Text = CInt(Val(txtCount.Text)) * CInt(Val(txtFare.Text))


            End If

            Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)

            Call CalculateTotals()



        Catch ex As Exception
            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & line
            lblErr.Visible = True
        End Try
    End Sub

    Protected Sub lnkMakeAvailable_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkMakeAvailable.Click

        If hndDisable.Value = "1" Then
            clearValues()
            lblErr.Text = "No action can be taken."
            Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)
            Exit Sub
        End If


        Dim ScheduleID As Integer = cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text
        Dim dtSeatDetail As New DataTable

        objTicketingSeat.TicketingScheduleID = ScheduleID
        objTicketingSeat.SeatNo = hidSeatNo.Value
        objTicketingSeat.GetSeatDetail(ScheduleID, hidSeatNo.Value.Trim, False)

        If objTicketingSeat.IssuedBy = objUser.Id And objTicketingSeat.Status = eTicketStatus.Reserved Then

            objTicketingSeat.Status = eTicketStatus.Available
            objTicketingSeat.UpdateSeatStatus()

            If chkOnline.Checked Then

                objTicketing = New clsTicketing(objConnection)
                objTicketing.Id = TicketingScheduleId
                objTicketing.GetById()
                objOnlineTicketing = New eTicketing
                '' Nauman Here
                If Val(hndOnlineTSNo.Value) = 0 Then
                    loadOnlineVoucher()
                End If
                objOnlineTicketing.MakeAvailable(CLng(hndOnlineTSNo.Value), objTicketingSeat.SeatNo, eTicketStatus.Available, objUser.Id, objUser.TerminalId)

                'If objOnlineTicketing.IsOnlineTicketingSchedule(objTicketing) Then
                'Else
                '    'lblErr.Text = "This Schedule is not Online! Check it as off line"
                '    'lblErr.Visible = True
                '    'lnkCreateOnline.Visible = True
                'End If
            End If

            Dim ArrSeats() As String = txtSeatNo.Text.Trim().Split(",")
            txtSeatNo.Text = ""
            For i As Integer = 0 To ArrSeats.Count - 1
                If ArrSeats(i).Trim() <> hidSeatNo.Value.Trim() Then
                    If txtSeatNo.Text.Trim() = "" Then
                        txtSeatNo.Text = ArrSeats(i)
                    Else
                        txtSeatNo.Text = txtSeatNo.Text & "," & ArrSeats(i)
                    End If
                End If
            Next
            'txtSeatNo.Text = ""
            txtPassengerName.Text = ""
            txtContactNo.Text = ""
            hidSeatNo.Value = 0


            Dim ArrCount As Array = txtSeatNo.Text.Split(",")
            txtCount.Text = ArrCount.Length
            txtTotals.Text = CInt(Val(txtCount.Text)) * CInt(Val(txtFare.Text))

        ElseIf (objTicketingSeat.IssuedBy <> objUser.Id And objTicketingSeat.Status = eTicketStatus.Reserved) Then

            objTicketingSeat.Status = eTicketStatus.Available
            objTicketingSeat.UpdateSeatStatus()

            If chkOnline.Checked Then

                objTicketing = New clsTicketing(objConnection)
                objTicketing.Id = TicketingScheduleId
                objTicketing.GetById()
                objOnlineTicketing = New eTicketing
                '' Nauman Here
                If Val(hndOnlineTSNo.Value) = 0 Then
                    loadOnlineVoucher()
                End If
                objOnlineTicketing.MakeAvailable(CLng(hndOnlineTSNo.Value), objTicketingSeat.SeatNo, eTicketStatus.Available, objUser.Id, objUser.TerminalId)

                'If objOnlineTicketing.IsOnlineTicketingSchedule(objTicketing) Then
                'Else
                '    'lblErr.Text = "This Schedule is not Online! Check it as off line"
                '    'lblErr.Visible = True
                '    'lnkCreateOnline.Visible = True
                'End If
            End If

            Dim ArrSeats() As String = txtSeatNo.Text.Trim().Split(",")
            txtSeatNo.Text = ""
            For i As Integer = 0 To ArrSeats.Count - 1
                If ArrSeats(i).Trim() <> hidSeatNo.Value.Trim() Then
                    If txtSeatNo.Text.Trim() = "" Then
                        txtSeatNo.Text = ArrSeats(i)
                    Else
                        txtSeatNo.Text = txtSeatNo.Text & "," & ArrSeats(i)
                    End If
                End If
            Next
            'txtSeatNo.Text = ""
            txtPassengerName.Text = ""
            txtContactNo.Text = ""
            hidSeatNo.Value = 0


            Dim ArrCount As Array = txtSeatNo.Text.Split(",")
            txtCount.Text = ArrCount.Length
            txtTotals.Text = CInt(Val(txtCount.Text)) * CInt(Val(txtFare.Text))

        Else
            lblErr.Text = "This seat is Reserved by some other user."
        End If

        'noman end
        Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)
        Call CalculateTotals()
    End Sub

    Private Sub btnSkip_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSkip.Click

        btnCancelTicket.Visible = False
        btnReprint.Visible = False
        btnMissed.Visible = False
        btnSkip.Visible = False
        btnSave.Style.Add("DISPLAY", "")
        txtSeatNo.Text = ""
        txtPassengerName.Text = ""
        txtContactNo.Text = ""
        txtAmount.Text = ""
        tblTickets.Disabled = False
        Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)

        cmbDestination.DataSource = dtRouteListAll
        cmbDestination.DataBind()
        cmbDestination.Items.Add("Please select")
    End Sub

    Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefresh.Click
        Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)
        btnCancelTicket.Visible = False
        btnReprint.Visible = False
        btnMissed.Visible = False
        btnSave.Visible = True
        lblDouple.Text = ""
        clearValues()

        If Request.QueryString("mode") = "1" Then
            dtRouteListAll = objTicketingSeat.GetCities(cboVoucherNo_1.Rows(0).Cells.FromKey("Schedule_ID").Value, hidSource.Value, 1)
            cmbDestination.DataSource = dtRouteListAll
            cmbDestination.DataBind()
            cmbDestination.Items.Add("Please select")

        Else
            dtRouteListAll = objTicketingSeat.GetCities(cboVoucherNo_1.Rows(0).Cells.FromKey("Schedule_ID").Value, hidSource.Value, 2)
            cmbDestination.DataSource = dtRouteListAll
            cmbDestination.DataBind()
            cmbDestination.Items.Add("Please select")

        End If
        Label2912555.Visible = False
        txtCustomerNumber.Visible = False
        btnValidateCustomers.Visible = False


    End Sub

#End Region

#Region " Functions And Procedure  "

    Private Sub RegisterClientEvents()
        hidPrint.Value = 0

        btnSave.Attributes.Add("onclick", "return validation();")
        trDeduction.Style.Add("Display", "none")
        txtAmount.Attributes.Add("onblur", "txtAmount_onblur();")

        btnCloseVoucher.Attributes.Add("onclick", "return closeVoucher();")
        tblDeductions.Style.Add("Display", "none")
        txtHostessSalary.Attributes.Add("onblur", "updateTotal();")
        txtDriverSalary.Attributes.Add("onblur", "updateTotal();")
        txtGuardSalary.Attributes.Add("onblur", "updateTotal();")
        txtServiceCharges.Attributes.Add("onblur", "updateTotal();")
        txtCleaningCharges.Attributes.Add("onblur", "updateTotal();")
        txtHookCharges.Attributes.Add("onblur", "updateTotal();")
        txtBusCharges.Attributes.Add("onblur", "updateTotal();")
        txtRefreshment.Attributes.Add("onblur", "updateTotal();")
        txtToll.Attributes.Add("onblur", "updateTotal();")
        txtTotalDeductions.Attributes.Add("readonly", "true")

        chkOnline.Attributes.Add("onclick", "chkOnline_onclick();")

        If hidMode.Value.Trim() <> "2" Then
            txtSeatNo.Attributes.Add("onblur", "txtSeatNo_onblur();")
        End If
    End Sub

    Private Sub loadCombos()

        objTicketing.Id = Me.TicketingScheduleId
        Dim dt As DataTable = objTicketing.GetVouchersByTDId()
        Dim objScheduleList As clsSchedules

        objConnection = ConnectionManager.GetConnection()
        objScheduleList = New clsSchedules(objConnection)

        'Dim pk(0) As DataColumn
        'pk(0) = dt.Columns("Ticketing_Schedule_ID")
        'pk(0).AutoIncrement = True
        'dt.PrimaryKey = pk
        'dt.AcceptChanges()

        'cboRoute.DataSource = objScheduleList.GetAll() '.get.GetRoute()

        'cboRoute.DataValueField = "Schedule_Id"
        'cboRoute.DataTextField = "Schedule_Title"
        'cboRoute.DataBind()

        'cboRoute.Items.Insert(0, New ListItem("Select", "0"))


        cboVoucherNo_1.DataSource = dt
        cboVoucherNo_1.DataBind()
    End Sub

    Private Sub loadCities()
        Dim ScheduleID As Integer = cboVoucherNo_1.Rows(0).Cells.FromKey("Schedule_ID").Text
        Dim dt_AcTable As New DataTable
        Dim Source_Id As Integer = 56
        dt_AcTable = objTicketingSeat.GetCities_Actual(ScheduleID)

        Dim ts1 As TimeSpan = TimeSpan.Parse(txtActualDepartureTime.Text)


        'Response.Write(cboVoucherNo.Rows(0).Cells.FromKey("Seats").Value)

        If dt_AcTable.Rows.Count > 0 Then

            For Each dr_acc As DataRow In dt_AcTable.Rows

                Dim temptime As TimeSpan = TimeSpan.Parse(dr_acc("Travel_Time"))
                Dim temptime1 As TimeSpan = TimeSpan.Parse(dr_acc("Stay_Time"))

                If dr_acc("City_Id") = hidSource.Value.Trim() Then
                    TotalTime = ts1 + temptime
                    If TotalTime.Hours = 0 Then
                        TotalTime = temptime
                    End If
                    ts1 = ts1 + temptime
                    Exit For
                Else
                    If dr_acc("Travel_Time") <> "00:00" Then
                        ts1 = ts1 + temptime
                        ts1 = ts1 + temptime1
                    End If
                End If

            Next

        End If

        txtActualDepartureTime.Text = ts1.Hours.ToString("00") & ":" & ts1.Minutes.ToString("00")
        Dim dr As DataRow

        cmbSource.DataSource = objTicketingSeat.GetCities(ScheduleID)

        cmbSource.DataValueField = "City_ID"
        cmbSource.DataTextField = "City_Name"
        cmbSource.DataBind()



        If hidSource.Value.Trim() <> "" Then
            If Not cmbSource.Items.FindByValue(hidSource.Value.Trim()) Is Nothing Then
                cmbSource.SelectedValue = hidSource.Value.Trim()
            End If
        ElseIf cmbSource.Items.Count > 0 Then
            cmbSource.SelectedIndex = 0
        End If

        If Request.QueryString("mode") = "1" Then

            dtRouteListAll = objTicketingSeat.GetCities(ScheduleID, hidSource.Value, 1)
            cmbDestination.DataSource = dtRouteListAll
            cmbSource.Enabled = False

        Else

            dtRouteListAll = objTicketingSeat.GetCities(ScheduleID, hidSource.Value, 2)
            cmbDestination.DataSource = dtRouteListAll
            cmbSource.Enabled = True

        End If

        cmbDestination.DataValueField = "City_ID"
        cmbDestination.DataTextField = "City_Name"
        cmbDestination.DataBind()
        cmbDestination.Items.Add("Please select")
        'cmbDestination.Visible = False

        If cmbDestination.Items.Count > 0 Then
            'cmbDestination.SelectedIndex = 0
            Call loadFare()
        End If


    End Sub

    Private Sub UpdateVoucher()

        'objTicketing.UserId = objUser.Id
        'objTicketing.ComputerName = "System"
        'objTicketing.AccessTerminalId = 1

        'For Each dRow As DataRow In Table.Rows
        '    If "" & dRow.Item("Ticketing_Schedule_ID") = hidTSID.Value.Trim() Then
        '        With objTicketing
        '            .Id = dRow.Item("Ticketing_Schedule_ID")
        '            If (Not IsDBNull(.Id)) AndAlso .Id <> 0 Then
        '                .GetById()
        '            End If

        '            .ScheduleID = cboRoute.SelectedValue
        '            .SerialNo = dRow.Item("Sr_No")
        '            .DepartureTime = dRow.Item("Departure_Time")

        '            If "" & dRow.Item("Vehicle_ID") <> "" Then
        '                .VehicleID = dRow.Item("Vehicle_ID")
        '            Else
        '                .VehicleID = 0
        '            End If

        '            .DriverName = "" & dRow.Item("Driver_Name")
        '            .HostessName = "" & dRow.Item("Hostess_Name")

        '        End With

        '        If objTicketing.VehicleID = 0 Then
        '            Response.Write("<script>alert('Please select vehicle first!');</script>")
        '            Call loadTable()
        '            Call BindTicketingRoute()
        '            Exit Sub
        '        End If

        '        objTicketing.Save(False)
        '        Exit For
        '    End If
        'Next

        ''///////////////////////////////////////////////////////////////////////////////////////
        'If mode = "2" Then
        '    Response.Redirect("Ticketing.aspx?mode=2&TSID=" & hidTSID.Value.Trim())
        'ElseIf mode = "1" Then
        '    Response.Redirect("Ticketing.aspx?mode=1&TSID=" & hidTSID.Value.Trim())
        'Else
        '    Response.Redirect("Ticketing.aspx?TSID=" & hidTSID.Value.Trim())
        'End If

    End Sub


    Private Sub BookKaroAPIPrintUpdate(ByVal PNR_No As String, ByVal UserName As String, ByVal TerminalName As String, ByVal PrintDate As String)

        Dim IPAddress As String
        IPAddress = FMovers.Ticketing.DAL.Crypto.Decrypt(System.Configuration.ConfigurationManager.AppSettings("ServerIPAddress").ToString, "")


        Dim message = "This is testing"

        'Dim urlString As String = String.Format("http://203.130.22.170:7800/api/BookkaruTicketPrint?InvoiveNo=" + PNR_No + "&OnlinePrint_Terminal_Id=" + TerminalName + "&OnlinePrinter_UserId=" + UserName + "&OnlinePrint_Date=" + PrintDate + "")

        Dim myUri As New Uri("http://" + IPAddress + ":7800/api/BookkaruTicketPrint?InvoiveNo=" + PNR_No + "&OnlinePrint_Terminal_Id=" + TerminalName + "&OnlinePrinter_UserId=" + UserName + "&OnlinePrint_Date=" + PrintDate + "")


        'Dim URI = New Uri(myUri)
        Dim wc As New WebClient()

        Dim res = wc.DownloadString(myUri)

    End Sub

    Private Sub loadFare()

        objFare = New clsFare(objConnection)
        If cmbDestination.Items.Count > 0 Then
            Dim dtFare As DataTable

            If cmbDestination.SelectedValue Is Nothing Or cmbDestination.SelectedValue = "Please select" Then
                txtFare.Text = ""

                hidTicketChange.Value = "0"
                hidTicketRefund.Value = "0"
            Else
                Dim ServiceFare As String
                Dim txtRefrehment As String
                Dim txtTerminalIss As String
                Dim OnBoardStatus As Integer



                Dim tblOtherFare As New DataTable
                tblOtherFare = GetTicketingOtherFare()

                Dim row As DataRow = tblOtherFare.Rows(0)
                ServiceFare = row("ItemPrice").ToString()
                OnBoardStatus = row("OnBoardService").ToString()

                If (objUser.CanTFService = True) Then

                    Dim objMiscellaneous As New DataTable
                    objMiscellaneous = GetTicketingExpDetails()

                    If objMiscellaneous IsNot Nothing AndAlso objMiscellaneous.Rows.Count > 0 Then


                        Dim roow As DataRow = objMiscellaneous.Rows(0)
                        Dim roows As DataRow = objMiscellaneous.Rows(1)
                        txtRefrehment = roow("ItemPrice").ToString()
                        txtTerminalIss = roows("ItemPrice").ToString()



                    Else

                        txtRefrehment = "0.00"
                        txtTerminalIss = "0.00"
                    End If

                Else
                    txtRefrehment = "0.00"
                    txtTerminalIss = "0.00"
                End If








                If (objUser.CanTFService = True) Then
                    objTicketingSeat.ServiceFare = ServiceFare
                    objTicketingSeat.OnBoardService = objUser.CanTFService
                    dtFare = objFare.GetFare(0, cmbSource.SelectedValue, cmbDestination.SelectedValue, Val(cboVoucherNo_1.Rows(0).Cells.FromKey("Schedule_ID").Text), hndServiceType.Value)


                    If Not Request.QueryString("DestinationId") Is Nothing Then
                        txtFare.Text = Request.QueryString("Fare")
                        DiscountFare.Text = Request.QueryString("Column1")
                    Else
                        txtFare.Text = dtFare.Rows(0)("Fare")
                        txtRefreshmentiteam.Text = txtRefrehment
                        txtTerminalIssuance.Text = txtTerminalIss
                        DiscountFare.Text = dtFare.Rows(0)("Column1")
                    End If
                    txtFare.Text = dtFare.Rows(0)("Fare")
                    hidTicketRefund.Value = dtFare.Rows(0)("TicketRefund")
                    hidTicketChange.Value = dtFare.Rows(0)("TicketChange")
                Else

                    dtFare = objFare.GetFare(0, cmbSource.SelectedValue, cmbDestination.SelectedValue, Val(cboVoucherNo_1.Rows(0).Cells.FromKey("Schedule_ID").Text), hndServiceType.Value)
                    objTicketingSeat.OnBoardService = 0

                    If Not Request.QueryString("DestinationId") Is Nothing Then
                        txtFare.Text = Request.QueryString("Fare")
                        DiscountFare.Text = Request.QueryString("Column1")
                    Else
                        txtFare.Text = dtFare.Rows(0)("Fare")
                        DiscountFare.Text = dtFare.Rows(0)("Column1")
                    End If
                    txtFare.Text = dtFare.Rows(0)("Fare")
                    hidTicketRefund.Value = dtFare.Rows(0)("TicketRefund")
                    hidTicketChange.Value = dtFare.Rows(0)("TicketChange")
                End If

            End If
        End If
    End Sub

    Private Function GetTicketingExpDetails() As DataTable
        Dim objDbManager As IDBManager
        Dim objDataSet As DataSet
        objDbManager = DBManager.GetDatabaseManager()
        objDbManager.SetDBConnection(objConnection)
        Dim objDBParameters As New clsDBParameters
        'Session("TicketingScheduleId") = TicketingScheduleId

        objDBParameters.Parameters.Add(New clsDBParameter("@CompanyId", objUser.Vendor_Id, "bigint"))
        objDBParameters.Parameters.Add(New clsDBParameter("@ServiceId", hndServiceType.Value, "bigint"))
        objDBParameters.Parameters.Add(New clsDBParameter("@FromCityId", objUser.CityId, "bigint"))
        objDBParameters.Parameters.Add(New clsDBParameter("@ToCityId", cmbDestination.SelectedValue, "bigint"))
        objDataSet = objDbManager.GetData("GetAllOnboardServiceChrgesList", objDBParameters)
        If Not objDataSet Is Nothing Then
            Return objDataSet.Tables(0)
        Else
            Return Nothing
        End If
    End Function

    Private Function GetTicketingOtherFare() As DataTable
        Dim objDbManager As IDBManager
        Dim objDataSet As DataSet
        objDbManager = DBManager.GetDatabaseManager()
        objDbManager.SetDBConnection(objConnection)
        Dim objDBParameters As New clsDBParameters
        'Session("TicketingScheduleId") = TicketingScheduleId

        objDBParameters.Parameters.Add(New clsDBParameter("@CompanyId", objUser.Vendor_Id, "bigint"))
        objDBParameters.Parameters.Add(New clsDBParameter("@ServiceId", hndServiceType.Value, "bigint"))
        objDBParameters.Parameters.Add(New clsDBParameter("@FromCityId", objUser.CityId, "int"))
        objDBParameters.Parameters.Add(New clsDBParameter("@ToCityId", cmbDestination.SelectedValue, "int"))
        objDataSet = objDbManager.GetData("GetOtherServiceFareNew", objDBParameters)
        If Not objDataSet Is Nothing Then
            Return objDataSet.Tables(0)
        Else
            Return Nothing
        End If
    End Function

    Private Sub loadDropAt()
        Dim DTDropAt As New DataTable
        Dim DTCollectionPoint As New DataTable

        objFare = New clsFare(objConnection)
        If cmbDestination.Items.Count > 0 Then
            If cmbDestination.SelectedValue Is Nothing Or cmbDestination.SelectedValue = "Please select" Then
                txtFare.Text = ""
            Else

                DTDropAt = objFare.GetDropAt(cmbDestination.SelectedValue)
                DTCollectionPoint = objFare.GetDropAt(cmbSource.SelectedValue)

                cmbDropAt.DataSource = DTDropAt
                cmbDropAt.DataTextField = "Terminal_Name"
                cmbDropAt.DataValueField = "Terminal_id"
                cmbDropAt.DataBind()

                cmbTicketCollectPoint.DataSource = DTCollectionPoint
                cmbTicketCollectPoint.DataTextField = "Terminal_Name"
                cmbTicketCollectPoint.DataBind()
                'cmbDropAt.

            End If
        End If

    End Sub


    Public Sub PopulateTicketList(Optional ByVal SeatCount As Integer = 45)


        Dim dtseatsInfo As DataTable = Me.getSeatsInfo(cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Value)

        Dim dtCityInfo As DataTable = Me.getCityInfo(cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Value)

        Dim dtCityInfoColor As DataTable = Me.getCityInfoColor(cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Value)


        Dim dtCompanyInfoColor As DataTable = Me.getCompanyInfoColor(cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Value)


        Dim dvSeatsInfo As New DataView(dtseatsInfo)

        Dim RowIndex As Short = 0
        Dim IsODDRows As Boolean = False
        Dim Counter_Row As Int16 = 4
        Dim Counter_Loop As Int16 = 5
        Dim Counter_Last = SeatCount - 1
        Dim FirstTime As Boolean = True

        grdCityInfo.DataSource = dtCityInfo
        grdCityInfo.DataBind()
        grdCityInfo.Columns(0).Width = 50
        grdCityInfo.Columns(1).Width = 350

        divCityList.InnerHtml = ""
        divCompany.InnerHtml = ""

        For Each dr As DataRow In dtCompanyInfoColor.Rows
            divCompany.InnerHtml = divCompany.InnerHtml & "  <span class='dot'  style='background-color:" + dr("Seat_Color") + ";' > </span> &nbsp;&nbsp; " + dr("Company_Name")
        Next

        For Each dr As DataRow In dtCityInfoColor.Rows
            divCityList.InnerHtml = divCityList.InnerHtml & " <input  onclick=javascript:changecolors('" & dr("City_Name").ToString.Replace(" ", "_") & "','" & dr("Destination_Color") & "') style='height: 50px;min-width: 120px;font-size: 15px;font-weight: bold;border: 0px;cursor: pointer;background-color:#" & dr("Destination_Color") & "'  type='button' value='" & dr("City_Name") & " - " & dr("Total") & "' /> "
        Next
        'Dim tblRowToBeRemoved As HtmlTableRowCollection = New HtmlTableRowCollection()


        'For Each tblRowToBeRemoved In tblTickets.Rows
        '    tblTickets.Rows.Remove(tblRowToBeRemoved)
        'Next
        Dim tbRowCounter As Integer = 0
        tbRowCounter = tblTickets.Rows.Count

        For j As Integer = 0 To tbRowCounter - 1
            If tblTickets.Rows.Count > j Then
                tblTickets.Rows.RemoveAt(j)
            End If
        Next

        tbRowCounter = tblTickets.Rows.Count

        For j As Integer = 0 To tbRowCounter - 1
            If tblTickets.Rows.Count > j Then
                tblTickets.Rows.RemoveAt(j)
            End If
        Next


        tbRowCounter = tblTickets.Rows.Count

        For j As Integer = 0 To tbRowCounter - 1
            If tblTickets.Rows.Count > j Then
                tblTickets.Rows.RemoveAt(j)
            End If
        Next


        tbRowCounter = tblTickets.Rows.Count

        For j As Integer = 0 To tbRowCounter - 1
            If tblTickets.Rows.Count > j Then
                tblTickets.Rows.RemoveAt(j)
            End If
        Next

        tbRowCounter = tblTickets.Rows.Count

        For j As Integer = 0 To tbRowCounter - 1
            If tblTickets.Rows.Count > j Then
                tblTickets.Rows.RemoveAt(j)
            End If
        Next

        For j As Integer = 0 To tbRowCounter - 1
            If tblTickets.Rows.Count = j Then
                tblTickets.Rows.RemoveAt(j)
            End If
        Next



        Dim int_spancer As Integer = 0

        Dim int_Seat_Counter As Integer = 0



        If SeatCount = 34 And hndServiceType.Value = 15 Then

            'lblSleeperTitles.Visible = True
            lblSleeperbusUP1.Visible = True
            lblSleeperbusUP2.Visible = True

            lblSleeperbusUP1.Visible = True
            lblSleeperbusUP2.Visible = True
            lblSleeperbusUP3.Visible = True
            lblSleeperbusDown1.Visible = True
            lblSleeperbusDown2.Visible = True
            lblSleeperbusDown3.Visible = True

            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0

                If i = 32 Then



                    For j = 0 To 4



                        Dim htmlCell As New HtmlTableCell()
                        htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                        htmlCell.InnerText = i + j
                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                        tblRow.Cells.Insert(0, htmlCell)
                        tblRow.Cells.Add(New HtmlTableCell())



                    Next

                    Dim htmlCell_spancer As New HtmlTableCell()
                    dvSeatsInfo.RowFilter = "Seat_No=" & 99
                    htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & 99 & "Spacer"
                    htmlCell_spancer.InnerText = ""
                    htmlCell_spancer.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer)



                    Dim htmlCell_spancer_2 As New HtmlTableCell()
                    dvSeatsInfo.RowFilter = "Seat_No=" & 100
                    htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & 100 & "Spacer"
                    htmlCell_spancer.InnerText = ""
                    htmlCell_spancer.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer)


                    Dim htmlCell_spancer_3 As New HtmlTableCell()
                    dvSeatsInfo.RowFilter = "Seat_No=" & 101
                    htmlCell_spancer_3.ID = "cell_" & RowIndex & "_0_" & 101 & "Spacer"
                    htmlCell_spancer_3.InnerText = ""
                    htmlCell_spancer_3.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer_3)



                    Dim htmlCell_spancer_4 As New HtmlTableCell()
                    dvSeatsInfo.RowFilter = "Seat_No=" & 102
                    htmlCell_spancer_4.ID = "cell_" & RowIndex & "_0_" & 102 & "Spacer"
                    htmlCell_spancer_4.InnerText = ""
                    htmlCell_spancer_4.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer_4)



                ElseIf i > 12 And 1 <= 30 Then
                    int_spancer = 0
                    For j = 0 To 5


                        int_spancer = int_spancer + 1

                        Dim htmlCell As New HtmlTableCell()
                        htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                        htmlCell.InnerText = i + j


                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                        tblRow.Cells.Insert(0, htmlCell)
                        'tblRow.Cells.Add(New HtmlTableCell())


                        If int_spancer = 2 Then

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 56 + j
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + 56 + j & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)



                        End If

                        If int_spancer = 4 Then

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 57 + j
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + 57 + j & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)


                            Dim htmlCell_spancer_2 As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 58 + j
                            htmlCell_spancer_2.ID = "cell_" & RowIndex & "_0_" & i + 58 + j & "Spacer"
                            htmlCell_spancer_2.InnerText = ""
                            htmlCell_spancer_2.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer_2)


                        End If






                    Next



                ElseIf i <= 12 Then
                    int_spancer = 0

                    




                    'tblRow.Cells.Add(New HtmlTableCell())


                    For j = 0 To 5


                        int_spancer = int_spancer + 1

                        Dim htmlCell As New HtmlTableCell()
                        htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                        htmlCell.InnerText = i + j
                        htmlCell.Style.Add("height", "80px")

                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                        tblRow.Cells.Insert(0, htmlCell)

                        'tblRow.Cells.Add(New HtmlTableCell())


                        If int_spancer = 2 Then

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 56 + j
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + 56 + j & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)



                        End If

                        If int_spancer = 4 Then

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 57 + j
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + 57 + j & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)


                            Dim htmlCell_spancer_2 As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 58 + j
                            htmlCell_spancer_2.ID = "cell_" & RowIndex & "_0_" & i + 58 + j & "Spacer"
                            htmlCell_spancer_2.InnerText = ""
                            htmlCell_spancer_2.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer_2)


                        End If






                    Next

                End If


                tblTickets.Rows.Add(tblRow)
                i = i + 5
                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If



        If SeatCount = 45 Then

            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0

                If i = 41 Then
                    For j = 0 To 4
                        Dim htmlCell As New HtmlTableCell()
                        htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                        htmlCell.InnerText = i + j
                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                        tblRow.Cells.Insert(0, htmlCell)
                        ' tblRow.Cells.Add(New HtmlTableCell())

                    Next
                ElseIf i < 41 Then
                    For j = 0 To 3

                        int_spancer = int_spancer + 1
                        If int_spancer = 2 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            htmlCell.InnerText = i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            'tblRow.Cells.Add(New HtmlTableCell())

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)
                            ' tblRow.Cells.Add(New HtmlTableCell())

                        Else
                            Dim htmlCell As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j

                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            htmlCell.InnerText = i + j
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            ' tblRow.Cells.Add(New HtmlTableCell())
                        End If
                    Next

                End If
                tblTickets.Rows.Add(tblRow)
                i = i + 3
                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If




        If SeatCount = 49 Then

            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0

                If i = 45 Then
                    For j = 0 To 4
                        Dim htmlCell As New HtmlTableCell()
                        htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                        htmlCell.InnerText = i + j
                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                        tblRow.Cells.Insert(0, htmlCell)
                        ' tblRow.Cells.Add(New HtmlTableCell())

                    Next
                ElseIf i <= 44 Then
                    For j = 0 To 3

                        int_spancer = int_spancer + 1
                        If int_spancer = 2 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            htmlCell.InnerText = i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            'tblRow.Cells.Add(New HtmlTableCell())

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)
                            ' tblRow.Cells.Add(New HtmlTableCell())

                        Else
                            Dim htmlCell As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j

                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            htmlCell.InnerText = i + j
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            ' tblRow.Cells.Add(New HtmlTableCell())
                        End If
                    Next

                End If
                tblTickets.Rows.Add(tblRow)
                i = i + 3
                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If

        If SeatCount = 37 Then

            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0

                If i = 33 Then


                    For j = 0 To 4
                        Dim htmlCell As New HtmlTableCell()
                        htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                        htmlCell.InnerText = i + j
                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                        tblRow.Cells.Insert(0, htmlCell)
                        ' tblRow.Cells.Add(New HtmlTableCell())

                    Next

                ElseIf i < 32 Then

                    For j = 0 To 3

                        int_spancer = int_spancer + 1
                        If int_spancer = 2 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            htmlCell.InnerText = i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            'tblRow.Cells.Add(New HtmlTableCell())

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)
                            ' tblRow.Cells.Add(New HtmlTableCell())

                        Else
                            Dim htmlCell As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j

                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            htmlCell.InnerText = i + j
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            ' tblRow.Cells.Add(New HtmlTableCell())
                        End If
                    Next

                End If
                tblTickets.Rows.Add(tblRow)
                i = i + 3
                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If


 

        If SeatCount = 44 Then

            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0

                For j = 0 To 3

                    int_spancer = int_spancer + 1
                    If int_spancer = 2 Then

                        Dim htmlCell As New HtmlTableCell()
                        htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                        htmlCell.InnerText = i + j
                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                        tblRow.Cells.Insert(0, htmlCell)
                        'tblRow.Cells.Add(New HtmlTableCell())

                        Dim htmlCell_spancer As New HtmlTableCell()
                        dvSeatsInfo.RowFilter = "Seat_No=" & i + j
                        htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer"
                        htmlCell_spancer.InnerText = ""
                        htmlCell_spancer.Attributes.Add("class", "Spacer")
                        tblRow.Cells.Insert(0, htmlCell_spancer)
                        ' tblRow.Cells.Add(New HtmlTableCell())

                    Else
                        Dim htmlCell As New HtmlTableCell()
                        dvSeatsInfo.RowFilter = "Seat_No=" & i + j

                        htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                        htmlCell.InnerText = i + j
                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                        tblRow.Cells.Insert(0, htmlCell)
                        ' tblRow.Cells.Add(New HtmlTableCell())
                    End If
                Next



                tblTickets.Rows.Add(tblRow)
                i = i + 3
                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If


        If SeatCount = 29 Then

            For i As Integer = 1 To 22

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0

                If i = 1 Then


                    Dim htmlCell_spancer As New HtmlTableCell()
                    dvSeatsInfo.RowFilter = "Seat_No=" & i + 70
                    htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + 70 & "Spacer"
                    htmlCell_spancer.InnerText = ""
                    htmlCell_spancer.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer)


                    Dim htmlCell_spancer_744 As New HtmlTableCell()
                    dvSeatsInfo.RowFilter = "Seat_No=" & i + 744
                    htmlCell_spancer_744.ID = "cell_" & RowIndex & "_0_" & i + 744 & "Spacer"
                    htmlCell_spancer_744.InnerText = ""
                    htmlCell_spancer_744.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer_744)


                    Dim htmlCell_spancer_7444 As New HtmlTableCell()
                    dvSeatsInfo.RowFilter = "Seat_No=" & i + 7444
                    htmlCell_spancer_7444.ID = "cell_" & RowIndex & "_0_" & i + 7444 & "Spacer"
                    htmlCell_spancer_7444.InnerText = ""
                    htmlCell_spancer_7444.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer_7444)




                    Dim htmlCell_1 As New HtmlTableCell()
                    htmlCell_1.ID = "cell_" & RowIndex & "_0_" & 23
                    htmlCell_1.InnerText = "1F"
                    dvSeatsInfo.RowFilter = "Seat_No=" & (23)
                    Me.ApplyCellSettings(htmlCell_1, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_1)


                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)



                End If

                If i = 2 Then


                    Dim htmlCell_3 As New HtmlTableCell()
                    htmlCell_3.ID = "cell_" & RowIndex & "_0_" & i + 2
                    htmlCell_3.InnerText = i + 2
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell_3, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_3)


                    Dim htmlCell_2 As New HtmlTableCell()
                    htmlCell_2.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_2.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_2, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_2)


                    Dim htmlCell_spancer_7444 As New HtmlTableCell()
                    dvSeatsInfo.RowFilter = "Seat_No=" & i + 7444
                    htmlCell_spancer_7444.ID = "cell_" & RowIndex & "_0_" & i + 7444 & "Spacer"
                    htmlCell_spancer_7444.InnerText = ""
                    htmlCell_spancer_7444.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer_7444)




                    Dim htmlCell_1 As New HtmlTableCell()
                    htmlCell_1.ID = "cell_" & RowIndex & "_0_" & 24
                    htmlCell_1.InnerText = "2F"
                    dvSeatsInfo.RowFilter = "Seat_No=" & (24)
                    Me.ApplyCellSettings(htmlCell_1, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_1)


                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)



                End If


                If i = 4 Then


                    Dim htmlCell_3 As New HtmlTableCell()
                    htmlCell_3.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_3.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_3, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_3)


                    Dim htmlCell_2 As New HtmlTableCell()
                    htmlCell_2.ID = "cell_" & RowIndex & "_0_" & i + 2
                    htmlCell_2.InnerText = i + 2
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell_2, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_2)


                    Dim htmlCell_spancer_74446 As New HtmlTableCell()
                    dvSeatsInfo.RowFilter = "Seat_No=" & i + 74446
                    htmlCell_spancer_74446.ID = "cell_" & RowIndex & "_0_" & i + 74446 & "Spacer"
                    htmlCell_spancer_74446.InnerText = ""
                    htmlCell_spancer_74446.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer_74446)




                    Dim htmlCell_1 As New HtmlTableCell()
                    htmlCell_1.ID = "cell_" & RowIndex & "_0_" & 25
                    htmlCell_1.InnerText = "3F"
                    dvSeatsInfo.RowFilter = "Seat_No=" & (25)
                    Me.ApplyCellSettings(htmlCell_1, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_1)


                    Dim htmlCell_spancer_74445 As New HtmlTableCell()
                    dvSeatsInfo.RowFilter = "Seat_No=" & i + 74445
                    htmlCell_spancer_74445.ID = "cell_" & RowIndex & "_0_" & i + 74445 & "Spacer"
                    htmlCell_spancer_74445.InnerText = ""
                    htmlCell_spancer_74445.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer_74445)




                End If


                If i = 7 Then


                    Dim htmlCell_3 As New HtmlTableCell()
                    htmlCell_3.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell_3.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell_3, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_3)


                    Dim htmlCell_2 As New HtmlTableCell()
                    htmlCell_2.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_2.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_2, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_2)


                    Dim htmlCell_spancer_74446 As New HtmlTableCell()
                    dvSeatsInfo.RowFilter = "Seat_No=" & i + 74446
                    htmlCell_spancer_74446.ID = "cell_" & RowIndex & "_0_" & i + 74446 & "Spacer"
                    htmlCell_spancer_74446.InnerText = ""
                    htmlCell_spancer_74446.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer_74446)




                    Dim htmlCell_1 As New HtmlTableCell()
                    htmlCell_1.ID = "cell_" & RowIndex & "_0_" & 26
                    htmlCell_1.InnerText = "4F"
                    dvSeatsInfo.RowFilter = "Seat_No=" & (26)
                    Me.ApplyCellSettings(htmlCell_1, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_1)


                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                    htmlCell.InnerText = i + 2
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)




                End If


                If i = 10 Then


                    Dim htmlCell_3 As New HtmlTableCell()
                    htmlCell_3.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell_3.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell_3, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_3)


                    Dim htmlCell_2 As New HtmlTableCell()
                    htmlCell_2.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_2.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_2, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_2)


                    Dim htmlCell_spancer_74446 As New HtmlTableCell()
                    dvSeatsInfo.RowFilter = "Seat_No=" & i + 74446
                    htmlCell_spancer_74446.ID = "cell_" & RowIndex & "_0_" & i + 74446 & "Spacer"
                    htmlCell_spancer_74446.InnerText = ""
                    htmlCell_spancer_74446.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer_74446)




                    Dim htmlCell_1 As New HtmlTableCell()
                    htmlCell_1.ID = "cell_" & RowIndex & "_0_" & 27
                    htmlCell_1.InnerText = "5F"
                    dvSeatsInfo.RowFilter = "Seat_No=" & (27)
                    Me.ApplyCellSettings(htmlCell_1, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_1)


                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                    htmlCell.InnerText = i + 2
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)




                End If


                If i = 13 Then


                    Dim htmlCell_3 As New HtmlTableCell()
                    htmlCell_3.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell_3.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell_3, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_3)


                    Dim htmlCell_2 As New HtmlTableCell()
                    htmlCell_2.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_2.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_2, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_2)


                    Dim htmlCell_spancer_74446 As New HtmlTableCell()
                    dvSeatsInfo.RowFilter = "Seat_No=" & i + 74446
                    htmlCell_spancer_74446.ID = "cell_" & RowIndex & "_0_" & i + 74446 & "Spacer"
                    htmlCell_spancer_74446.InnerText = ""
                    htmlCell_spancer_74446.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer_74446)




                    Dim htmlCell_1 As New HtmlTableCell()
                    htmlCell_1.ID = "cell_" & RowIndex & "_0_" & 28
                    htmlCell_1.InnerText = "6F"
                    dvSeatsInfo.RowFilter = "Seat_No=" & (28)
                    Me.ApplyCellSettings(htmlCell_1, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_1)


                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                    htmlCell.InnerText = i + 2
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)




                End If


                If i = 16 Then


                    Dim htmlCell_3 As New HtmlTableCell()
                    htmlCell_3.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell_3.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell_3, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_3)


                    Dim htmlCell_2 As New HtmlTableCell()
                    htmlCell_2.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_2.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_2, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_2)


                    Dim htmlCell_spancer_74446 As New HtmlTableCell()
                    dvSeatsInfo.RowFilter = "Seat_No=" & i + 74446
                    htmlCell_spancer_74446.ID = "cell_" & RowIndex & "_0_" & i + 74446 & "Spacer"
                    htmlCell_spancer_74446.InnerText = ""
                    htmlCell_spancer_74446.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer_74446)




                    Dim htmlCell_1 As New HtmlTableCell()
                    htmlCell_1.ID = "cell_" & RowIndex & "_0_" & 29
                    htmlCell_1.InnerText = "7F"
                    dvSeatsInfo.RowFilter = "Seat_No=" & (29)
                    Me.ApplyCellSettings(htmlCell_1, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_1)


                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                    htmlCell.InnerText = i + 2
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)




                End If

                If i = 19 Then


                    Dim htmlCell_3 As New HtmlTableCell()
                    htmlCell_3.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell_3.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell_3, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_3)


                    Dim htmlCell_2 As New HtmlTableCell()
                    htmlCell_2.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_2.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_2, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_2)


                    Dim htmlCell_spancer_74446 As New HtmlTableCell()
                    dvSeatsInfo.RowFilter = "Seat_No=" & i + 74446
                    htmlCell_spancer_74446.ID = "cell_" & RowIndex & "_0_" & i + 74446 & "Spacer"
                    htmlCell_spancer_74446.InnerText = ""
                    htmlCell_spancer_74446.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer_74446)




                    Dim htmlCell_1 As New HtmlTableCell()
                    htmlCell_1.ID = "cell_" & RowIndex & "_0_" & 30
                    htmlCell_1.InnerText = "21"
                    dvSeatsInfo.RowFilter = "Seat_No=" & (21)
                    Me.ApplyCellSettings(htmlCell_1, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_1)


                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 3
                    htmlCell.InnerText = i + 3
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 3)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)




                End If


                tblTickets.Rows.Add(tblRow)

                If i = 1 Then
                    i = i + 0
                ElseIf i = 2 Then
                    i = i + 1

                ElseIf i = 4 Then
                    i = i + 2
                ElseIf i = 7 Then
                    i = i + 2
                ElseIf i = 10 Then
                    i = i + 2
                ElseIf i = 13 Then
                    i = i + 2
                ElseIf i = 16 Then
                    i = i + 2

                Else
                    i = i + 3
                End If
                int_Seat_Counter = int_Seat_Counter + 1

            Next
           

        End If

        If SeatCount = 26 Then

            lblSleeperTitles.Visible = True
            lblupper.Visible = True
            Label74.Visible = True
            Label75.Visible = True

            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0

                If i = 45 Then
                    For j = 0 To 4
                        Dim htmlCell As New HtmlTableCell()
                        htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                        htmlCell.InnerText = i + j
                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                        tblRow.Cells.Insert(0, htmlCell)
                        tblRow.Cells.Add(htmlCell)
                        ' tblRow.Cells.Add(New HtmlTableCell())

                    Next
                ElseIf i <= 44 Then
                    For j = 0 To 3

                        int_spancer = int_spancer + 1
                        If int_spancer = 2 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            htmlCell.InnerText = i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            'tblRow.Cells.Add(New HtmlTableCell())

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)
                            tblRow.Cells.Add(htmlCell)
                            ' tblRow.Cells.Add(New HtmlTableCell())

                        Else
                            Dim htmlCell As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j

                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            htmlCell.InnerText = i + j
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            tblRow.Cells.Add(htmlCell)
                            ' tblRow.Cells.Add(New HtmlTableCell())
                        End If
                    Next

                End If
                tblTickets.Rows.Add(tblRow)
                i = i + 3
                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If

        If SeatCount = 33 Then

            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0

                If i = 29 Then
                    For j = 0 To 4
                        Dim htmlCell As New HtmlTableCell()
                        htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                        htmlCell.InnerText = i + j
                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                        tblRow.Cells.Insert(0, htmlCell)
                        ' tblRow.Cells.Add(New HtmlTableCell())

                    Next
                ElseIf i <= 28 Then
                    For j = 0 To 3

                        int_spancer = int_spancer + 1
                        If int_spancer = 2 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            htmlCell.InnerText = i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            'tblRow.Cells.Add(New HtmlTableCell())

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)
                            ' tblRow.Cells.Add(New HtmlTableCell())

                        Else
                            Dim htmlCell As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j

                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            htmlCell.InnerText = i + j
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            ' tblRow.Cells.Add(New HtmlTableCell())
                        End If
                    Next

                End If
                tblTickets.Rows.Add(tblRow)
                i = i + 3
                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If





        If (SeatCount = 34 And hndServiceType.Value <> 15) Then

            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0


                If i = 31 Then
                    For j = 0 To 3
                        Dim htmlCell As New HtmlTableCell()
                        htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                        htmlCell.InnerText = i + j
                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                        tblRow.Cells.Insert(0, htmlCell)
                        ' tblRow.Cells.Add(New HtmlTableCell())

                    Next
                ElseIf i < 30 Then


                    For j = 0 To 2

                        int_spancer = int_spancer + 1

                        If int_spancer = 2 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            htmlCell.InnerText = i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            'tblRow.Cells.Add(New HtmlTableCell())

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)
                            ' tblRow.Cells.Add(New HtmlTableCell())

                        Else
                            Dim htmlCell As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j

                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            htmlCell.InnerText = i + j
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            ' tblRow.Cells.Add(New HtmlTableCell())
                        End If
                    Next
                End If

                tblTickets.Rows.Add(tblRow)
                i = i + 2
                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If


 

        If (SeatCount = 16) Then

            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0


                If i = 13 Then

                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 3
                    htmlCell.InnerText = i + 3
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 3)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)
                    'tblRow.Cells.Add(New HtmlTableCell())

                    Dim htmlCell_Second As New HtmlTableCell()
                    htmlCell_Second.ID = "cell_" & RowIndex & "_0_" & i + 2
                    htmlCell_Second.InnerText = i + 2
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell_Second, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Second)


                    Dim htmlCell_Third As New HtmlTableCell()
                    htmlCell_Third.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_Third.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_Third, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Third)


                    Dim htmlCell_Forth As New HtmlTableCell()
                    htmlCell_Forth.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell_Forth.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell_Forth, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Forth)



                ElseIf i < 13 Then

                    Dim htmlCell_Third As New HtmlTableCell()
                    htmlCell_Third.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell_Third.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell_Third, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Third)



                    'tblRow.Cells.Add(New HtmlTableCell())

                    Dim htmlCell_Second As New HtmlTableCell()
                    htmlCell_Second.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_Second.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_Second, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Second)


                    Dim htmlCell_spancer As New HtmlTableCell()
                    htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + 55 & "Spacer"
                    htmlCell_spancer.InnerText = ""
                    htmlCell_spancer.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer)





                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                    htmlCell.InnerText = i + 2
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)

                End If

                tblTickets.Rows.Add(tblRow)
                i = i + 2
                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If


        If (SeatCount = 31) Or (SeatCount = 30) Then

            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0


                If i = 28 Then

                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 3
                    htmlCell.InnerText = i + 3
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 3)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)
                    'tblRow.Cells.Add(New HtmlTableCell())

                    Dim htmlCell_Second As New HtmlTableCell()
                    htmlCell_Second.ID = "cell_" & RowIndex & "_0_" & i + 2
                    htmlCell_Second.InnerText = i + 2
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell_Second, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Second)


                    Dim htmlCell_Third As New HtmlTableCell()
                    htmlCell_Third.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_Third.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_Third, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Third)


                    Dim htmlCell_Forth As New HtmlTableCell()
                    htmlCell_Forth.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell_Forth.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell_Forth, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Forth)



                ElseIf i < 28 Then

                    Dim htmlCell_Third As New HtmlTableCell()
                    htmlCell_Third.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell_Third.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell_Third, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Third)



                    'tblRow.Cells.Add(New HtmlTableCell())

                    Dim htmlCell_Second As New HtmlTableCell()
                    htmlCell_Second.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_Second.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_Second, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Second)


                    Dim htmlCell_spancer As New HtmlTableCell()
                    htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + 55 & "Spacer"
                    htmlCell_spancer.InnerText = ""
                    htmlCell_spancer.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer)





                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                    htmlCell.InnerText = i + 2
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)
                    'For j = 0 To 2

                    '    int_spancer = int_spancer + 1
                    '    If int_spancer = 2 Then

                    '        Dim htmlCell As New HtmlTableCell()
                    '        htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                    '        htmlCell.InnerText = i + j
                    '        dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                    '        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    '        tblRow.Cells.Insert(0, htmlCell)
                    '        'tblRow.Cells.Add(New HtmlTableCell())

                    '        Dim htmlCell_spancer As New HtmlTableCell()
                    '        dvSeatsInfo.RowFilter = "Seat_No=" & i + j
                    '        htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer"
                    '        htmlCell_spancer.InnerText = ""
                    '        htmlCell_spancer.Attributes.Add("class", "Spacer")
                    '        tblRow.Cells.Insert(0, htmlCell_spancer)
                    '        ' tblRow.Cells.Add(New HtmlTableCell())

                    '    Else
                    '        Dim htmlCell As New HtmlTableCell()
                    '        dvSeatsInfo.RowFilter = "Seat_No=" & i + j

                    '        htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                    '        dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                    '        htmlCell.InnerText = i + j
                    '        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    '        tblRow.Cells.Insert(0, htmlCell)
                    '        ' tblRow.Cells.Add(New HtmlTableCell())
                    '    End If
                    'Next
                End If

                tblTickets.Rows.Add(tblRow)
                i = i + 2
                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If

        If SeatCount = 32 Then

            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0


                If i = 28 Then

                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 3
                    htmlCell.InnerText = i + 3
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 3)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)
                    'tblRow.Cells.Add(New HtmlTableCell())

                    Dim htmlCell_Second As New HtmlTableCell()
                    htmlCell_Second.ID = "cell_" & RowIndex & "_0_" & i + 2
                    htmlCell_Second.InnerText = i + 2
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell_Second, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Second)

                    Dim htmlCell_Third As New HtmlTableCell()
                    htmlCell_Third.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_Third.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_Third, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Third)

                    Dim htmlCell_Forth As New HtmlTableCell()
                    htmlCell_Forth.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell_Forth.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell_Forth, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Forth)







                ElseIf i < 28 Then


                    Dim htmlCell_Third As New HtmlTableCell()
                    htmlCell_Third.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell_Third.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell_Third, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Third)
                    '     tblRow.Cells.Add(htmlCell_Third)


                    'tblRow.Cells.Add(New HtmlTableCell())

                    Dim htmlCell_spancer As New HtmlTableCell()
                    htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + 55 & "Spacer"
                    htmlCell_spancer.InnerText = ""
                    htmlCell_spancer.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer)
                    tblRow.Cells.Add(htmlCell_spancer)


                    Dim htmlCell_Second As New HtmlTableCell()

                    htmlCell_Second.ID = "cell_" & RowIndex & "_0_" & (i + 1)
                    htmlCell_Second.InnerText = (i + 1).ToString()
                    htmlCell_Second.Style("position") = "relative"
                    htmlCell_Second.Style("top") = "38px"
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_Second, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Second)
                    tblRow.Cells.Add(htmlCell_Second)

                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & (i + 2)
                    htmlCell.InnerText = (i + 2).ToString()
                    htmlCell.Style("position") = "relative"
                    htmlCell.Style("top") = "38px"
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)
                    tblRow.Cells.Add(htmlCell)


                End If

                tblTickets.Rows.Add(tblRow)
                i = i + 2
                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If

        If SeatCount = 35 Then

            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0


                If i = 31 Then

                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 3
                    htmlCell.InnerText = i + 3
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 3)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)
                    'tblRow.Cells.Add(New HtmlTableCell())

                    Dim htmlCell_Second As New HtmlTableCell()
                    htmlCell_Second.ID = "cell_" & RowIndex & "_0_" & i + 2
                    htmlCell_Second.InnerText = i + 2
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell_Second, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Second)

                    Dim htmlCell_Third As New HtmlTableCell()
                    htmlCell_Third.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_Third.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_Third, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Third)

                    Dim htmlCell_Forth As New HtmlTableCell()
                    htmlCell_Forth.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell_Forth.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell_Forth, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Forth)







                ElseIf i < 29 Then

                    Dim htmlCell_Third As New HtmlTableCell()
                    htmlCell_Third.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell_Third.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell_Third, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Third)
                    '     tblRow.Cells.Add(htmlCell_Third)


                    'tblRow.Cells.Add(New HtmlTableCell())

                    Dim htmlCell_spancer As New HtmlTableCell()
                    htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + 55 & "Spacer"
                    htmlCell_spancer.InnerText = ""
                    htmlCell_spancer.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer)
                    tblRow.Cells.Add(htmlCell_spancer)



                    Dim htmlCell_Second As New HtmlTableCell()
                    htmlCell_Second.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_Second.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_Second, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Second)
                    tblRow.Cells.Add(htmlCell_Second)








                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                    htmlCell.InnerText = i + 2
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)
                    tblRow.Cells.Add(htmlCell)

                End If

                tblTickets.Rows.Add(tblRow)
                i = i + 2
                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If

        If (SeatCount = 24) Then

            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0

                Dim htmlCell_Third As New HtmlTableCell()
                htmlCell_Third.ID = "cell_" & RowIndex & "_0_" & i
                htmlCell_Third.InnerText = i
                dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                Me.ApplyCellSettings(htmlCell_Third, dvSeatsInfo)
                tblRow.Cells.Insert(0, htmlCell_Third)



                'tblRow.Cells.Add(New HtmlTableCell())

                Dim htmlCell_Second As New HtmlTableCell()
                htmlCell_Second.ID = "cell_" & RowIndex & "_0_" & i + 1
                htmlCell_Second.InnerText = i + 1
                dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                Me.ApplyCellSettings(htmlCell_Second, dvSeatsInfo)
                tblRow.Cells.Insert(0, htmlCell_Second)


                Dim htmlCell_spancer As New HtmlTableCell()
                htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + 55 & "Spacer"
                htmlCell_spancer.InnerText = ""
                htmlCell_spancer.Attributes.Add("class", "Spacer")
                tblRow.Cells.Insert(0, htmlCell_spancer)





                Dim htmlCell As New HtmlTableCell()
                htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                htmlCell.InnerText = i + 2
                dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                tblRow.Cells.Insert(0, htmlCell)


                tblTickets.Rows.Add(tblRow)
                i = i + 2
                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If




        If (SeatCount = 27) Then

            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0

                Dim htmlCell_Third As New HtmlTableCell()
                htmlCell_Third.ID = "cell_" & RowIndex & "_0_" & i
                htmlCell_Third.InnerText = i
                dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                Me.ApplyCellSettings(htmlCell_Third, dvSeatsInfo)
                tblRow.Cells.Insert(0, htmlCell_Third)



                'tblRow.Cells.Add(New HtmlTableCell())

                Dim htmlCell_Second As New HtmlTableCell()
                htmlCell_Second.ID = "cell_" & RowIndex & "_0_" & i + 1
                htmlCell_Second.InnerText = i + 1
                dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                Me.ApplyCellSettings(htmlCell_Second, dvSeatsInfo)
                tblRow.Cells.Insert(0, htmlCell_Second)


                Dim htmlCell_spancer As New HtmlTableCell()
                htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + 55 & "Spacer"
                htmlCell_spancer.InnerText = ""
                htmlCell_spancer.Attributes.Add("class", "Spacer")
                tblRow.Cells.Insert(0, htmlCell_spancer)





                Dim htmlCell As New HtmlTableCell()
                htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                htmlCell.InnerText = i + 2
                dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                tblRow.Cells.Insert(0, htmlCell)
         

                tblTickets.Rows.Add(tblRow)
                i = i + 2
                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If


        If (SeatCount = 43) And cboVoucherNo_1.Rows(0).Cells.FromKey("ServiceType_Id").Value = 12 Then

            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0


                If i > 13 And i < 39 Then

                    For j = 0 To 3

                        int_spancer = int_spancer + 1
                        If int_spancer = 2 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            htmlCell.InnerText = i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            'tblRow.Cells.Add(New HtmlTableCell())

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)
                            ' tblRow.Cells.Add(New HtmlTableCell())

                        Else
                            Dim htmlCell As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j

                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            htmlCell.InnerText = i + j
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            ' tblRow.Cells.Add(New HtmlTableCell())
                        End If
                    Next

                ElseIf i > 12 And i <= 14 Then

                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)
                    'tblRow.Cells.Add(New HtmlTableCell())

                    Dim htmlCell_Second As New HtmlTableCell()
                    htmlCell_Second.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_Second.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_Second, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Second)


                    Dim htmlCell_Third As New HtmlTableCell()
                    htmlCell_Third.ID = "cell_" & RowIndex & "_0_" & i + 53
                    htmlCell_Third.InnerText = ""
                    htmlCell_Third.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_Third)


                    Dim htmlCell_Forth As New HtmlTableCell()
                    htmlCell_Forth.ID = "cell_" & RowIndex & "_0_" & i + 54
                    htmlCell_Forth.InnerText = ""
                    htmlCell_Forth.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_Forth)



                    Dim htmlCell_Fifth As New HtmlTableCell()
                    htmlCell_Fifth.ID = "cell_" & RowIndex & "_0_" & i + 55
                    htmlCell_Fifth.InnerText = ""
                    htmlCell_Fifth.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_Fifth)


                ElseIf i > 38 And i <= 43 Then

                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)
                    'tblRow.Cells.Add(New HtmlTableCell())

                    Dim htmlCell_Second As New HtmlTableCell()
                    htmlCell_Second.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_Second.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_Second, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Second)


                    Dim htmlCell_Third As New HtmlTableCell()
                    htmlCell_Third.ID = "cell_" & RowIndex & "_0_" & i + 2
                    htmlCell_Third.InnerText = i + 2
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell_Third, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Third)


                    Dim htmlCell_Forth As New HtmlTableCell()
                    htmlCell_Forth.ID = "cell_" & RowIndex & "_0_" & i + 3
                    htmlCell_Forth.InnerText = i + 3
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 3)
                    Me.ApplyCellSettings(htmlCell_Forth, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Forth)



                    Dim htmlCell_Fifth As New HtmlTableCell()
                    htmlCell_Fifth.ID = "cell_" & RowIndex & "_0_" & i + 4
                    htmlCell_Fifth.InnerText = i + 4
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 4)
                    Me.ApplyCellSettings(htmlCell_Fifth, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Fifth)
                    i = i + 4
                ElseIf i <= 12 Then

                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)
                    'tblRow.Cells.Add(New HtmlTableCell())

                    Dim htmlCell_spancer As New HtmlTableCell()
                    htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + 55 & "Spacer"
                    htmlCell_spancer.InnerText = ""
                    htmlCell_spancer.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer)

                    Dim htmlCell_spancer_2 As New HtmlTableCell()
                    htmlCell_spancer_2.ID = "cell_" & RowIndex & "_0_" & i + 56 & "Spacer"
                    htmlCell_spancer_2.InnerText = ""
                    htmlCell_spancer_2.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer_2)


                    Dim htmlCell_Second As New HtmlTableCell()
                    htmlCell_Second.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_Second.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_Second, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Second)

                    Dim htmlCell_Third As New HtmlTableCell()
                    htmlCell_Third.ID = "cell_" & RowIndex & "_0_" & i + 2
                    htmlCell_Third.InnerText = i + 2
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell_Third, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Third)


                End If

                tblTickets.Rows.Add(tblRow)

                If i <= 12 Then
                    i = i + 2
                ElseIf i >= 13 And i <= 14 Then
                    i = i + 1
                ElseIf i > 15 And i < 39 Then
                    i = i + 3
                Else
                    i = i + 4
                End If


                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If



        If (SeatCount = 42) Then

            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0


                If i > 9 And i < 36 Then

                    For j = 0 To 3

                        int_spancer = int_spancer + 1
                        If int_spancer = 2 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            htmlCell.InnerText = i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            'tblRow.Cells.Add(New HtmlTableCell())

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)
                            ' tblRow.Cells.Add(New HtmlTableCell())

                        Else
                            Dim htmlCell As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j

                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            htmlCell.InnerText = i + j
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            ' tblRow.Cells.Add(New HtmlTableCell())
                        End If
                    Next

                ElseIf i > 36 Then

                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)
                    'tblRow.Cells.Add(New HtmlTableCell())

                    Dim htmlCell_Second As New HtmlTableCell()
                    htmlCell_Second.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_Second.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_Second, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Second)


                    Dim htmlCell_Third As New HtmlTableCell()
                    htmlCell_Third.ID = "cell_" & RowIndex & "_0_" & i + 2
                    htmlCell_Third.InnerText = i + 2
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell_Third, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Third)


                    Dim htmlCell_Forth As New HtmlTableCell()
                    htmlCell_Forth.ID = "cell_" & RowIndex & "_0_" & i + 3
                    htmlCell_Forth.InnerText = i + 3
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 3)
                    Me.ApplyCellSettings(htmlCell_Forth, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Forth)



                    Dim htmlCell_Fifth As New HtmlTableCell()
                    htmlCell_Fifth.ID = "cell_" & RowIndex & "_0_" & i + 4
                    htmlCell_Fifth.InnerText = i + 4
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 4)
                    Me.ApplyCellSettings(htmlCell_Fifth, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Fifth)

                ElseIf i <= 9 Then

                    Dim htmlCell_Third As New HtmlTableCell()
                    htmlCell_Third.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell_Third.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell_Third, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Third)



                    Dim htmlCell_Second As New HtmlTableCell()
                    htmlCell_Second.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_Second.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_Second, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Second)


                    Dim htmlCell_spancer As New HtmlTableCell()
                    htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + 55 & "Spacer"
                    htmlCell_spancer.InnerText = ""
                    htmlCell_spancer.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer)



                    Dim htmlCell_spancer_2 As New HtmlTableCell()
                    htmlCell_spancer_2.ID = "cell_" & RowIndex & "_0_" & i + 56 & "Spacer"
                    htmlCell_spancer_2.InnerText = ""
                    htmlCell_spancer_2.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer_2)




                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                    htmlCell.InnerText = i + 2
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)

                End If

                tblTickets.Rows.Add(tblRow)

                If i <= 9 Then
                    i = i + 2
                ElseIf i > 9 And i < 36 Then
                    i = i + 3
                Else
                    i = i + 4
                End If


                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If


        If (SeatCount = 40) Then

            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0


                If i > 10 And i < 35 Then

                    For j = 0 To 3

                        int_spancer = int_spancer + 1
                        If int_spancer = 2 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            htmlCell.InnerText = i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            'tblRow.Cells.Add(New HtmlTableCell())

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)
                            ' tblRow.Cells.Add(New HtmlTableCell())

                        Else
                            Dim htmlCell As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j

                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            htmlCell.InnerText = i + j
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            ' tblRow.Cells.Add(New HtmlTableCell())
                        End If
                    Next

                ElseIf i > 36 Then

                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)
                    'tblRow.Cells.Add(New HtmlTableCell())

                    Dim htmlCell_Second As New HtmlTableCell()
                    htmlCell_Second.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_Second.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_Second, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Second)


                    Dim htmlCell_Third As New HtmlTableCell()
                    htmlCell_Third.ID = "cell_" & RowIndex & "_0_" & i + 2
                    htmlCell_Third.InnerText = i + 2
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell_Third, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Third)


                    Dim htmlCell_Forth As New HtmlTableCell()
                    htmlCell_Forth.ID = "cell_" & RowIndex & "_0_" & i + 3
                    htmlCell_Forth.InnerText = i + 3
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 3)
                    Me.ApplyCellSettings(htmlCell_Forth, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Forth)



                    Dim htmlCell_Fifth As New HtmlTableCell()
                    htmlCell_Fifth.ID = "cell_" & RowIndex & "_0_" & i + 4
                    htmlCell_Fifth.InnerText = i + 4
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 4)
                    Me.ApplyCellSettings(htmlCell_Fifth, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Fifth)

                ElseIf i <= 12 Then

                    Dim htmlCell_Third As New HtmlTableCell()
                    htmlCell_Third.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell_Third.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell_Third, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Third)



                    Dim htmlCell_Second As New HtmlTableCell()
                    htmlCell_Second.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_Second.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_Second, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Second)


                    Dim htmlCell_spancer As New HtmlTableCell()
                    htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + 55 & "Spacer"
                    htmlCell_spancer.InnerText = ""
                    htmlCell_spancer.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer)



                    Dim htmlCell_spancer_2 As New HtmlTableCell()
                    htmlCell_spancer_2.ID = "cell_" & RowIndex & "_0_" & i + 56 & "Spacer"
                    htmlCell_spancer_2.InnerText = ""
                    htmlCell_spancer_2.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer_2)




                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                    htmlCell.InnerText = i + 2
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)


                    tblRow.Cells.Insert(0, htmlCell)

                End If

                tblTickets.Rows.Add(tblRow)

                If i <= 11 Then
                    i = i + 2
                ElseIf i > 9 And i < 42 Then
                    i = i + 3
                Else
                    i = i + 4
                End If


                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If

        If (SeatCount = 41) And cboVoucherNo_1.Rows(0).Cells.FromKey("ServiceType_Id").Value = 21 Then

            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0


                If i > 10 And i < 40 Then

                    For j = 0 To 3

                        int_spancer = int_spancer + 1
                        If int_spancer = 2 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            htmlCell.InnerText = i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            'tblRow.Cells.Add(New HtmlTableCell())

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)
                            ' tblRow.Cells.Add(New HtmlTableCell())

                        Else
                            Dim htmlCell As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j

                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            htmlCell.InnerText = i + j
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            ' tblRow.Cells.Add(New HtmlTableCell())
                        End If
                    Next

                ElseIf i > 40 Then

                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)

















                ElseIf i <= 12 Then

                    Dim htmlCell_Third As New HtmlTableCell()
                    htmlCell_Third.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell_Third.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell_Third, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Third)



                    Dim htmlCell_Second As New HtmlTableCell()
                    htmlCell_Second.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_Second.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_Second, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Second)


                    Dim htmlCell_spancer As New HtmlTableCell()
                    htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + 55 & "Spacer"
                    htmlCell_spancer.InnerText = ""
                    htmlCell_spancer.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer)



                    Dim htmlCell_spancer_2 As New HtmlTableCell()
                    htmlCell_spancer_2.ID = "cell_" & RowIndex & "_0_" & i + 56 & "Spacer"
                    htmlCell_spancer_2.InnerText = ""
                    htmlCell_spancer_2.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer_2)




                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                    htmlCell.InnerText = i + 2
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)


                    tblRow.Cells.Insert(0, htmlCell)

                End If

                tblTickets.Rows.Add(tblRow)

                If i <= 11 Then
                    i = i + 2
                ElseIf i > 9 And i < 42 Then
                    i = i + 3
                Else
                    i = i + 4
                End If


                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If

        Dim bustypev = cboVoucherNo_1.Rows(0).Cells.FromKey("ServiceType_Id").Value

        If (SeatCount = 41) And bustypev = 25 Then

            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0


                If i > 15 And i < 38 Then

                    For j = 0 To 3

                        int_spancer = int_spancer + 1
                        If int_spancer = 2 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            htmlCell.InnerText = i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            'tblRow.Cells.Add(New HtmlTableCell())

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)
                            ' tblRow.Cells.Add(New HtmlTableCell())

                        Else
                            Dim htmlCell As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j

                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            htmlCell.InnerText = i + j
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            ' tblRow.Cells.Add(New HtmlTableCell())
                        End If
                    Next

                ElseIf i > 38 Then

                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)







                ElseIf i <= 15 Then

                    Dim htmlCell_Third As New HtmlTableCell()
                    htmlCell_Third.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell_Third.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell_Third, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Third)



                    Dim htmlCell_Second As New HtmlTableCell()
                    htmlCell_Second.ID = "cell_" & RowIndex & "_0_" & i + 1
                    htmlCell_Second.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_Second, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Second)


                    Dim htmlCell_spancer As New HtmlTableCell()
                    htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + 55 & "Spacer"
                    htmlCell_spancer.InnerText = ""
                    htmlCell_spancer.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer)



                    Dim htmlCell_spancer_2 As New HtmlTableCell()
                    htmlCell_spancer_2.ID = "cell_" & RowIndex & "_0_" & i + 56 & "Spacer"
                    htmlCell_spancer_2.InnerText = ""
                    htmlCell_spancer_2.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer_2)




                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                    htmlCell.InnerText = i + 2
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)


                    tblRow.Cells.Insert(0, htmlCell)

                End If

                tblTickets.Rows.Add(tblRow)

                If i <= 15 Then
                    i = i + 2
                ElseIf i > 15 And i < 38 Then
                    i = i + 3
                Else
                    i = i + 3
                End If


                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If

        If (SeatCount = 25) Then




            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0


                If i = 1 Then

                    For j As Integer = 0 To 1
                        Dim htmlCell_spancer As New HtmlTableCell()
                        dvSeatsInfo.RowFilter = "Seat_No=" & i + j
                        htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer"
                        htmlCell_spancer.InnerText = ""
                        htmlCell_spancer.Attributes.Add("class", "Spacer")
                        tblRow.Cells.Insert(0, htmlCell_spancer)
                    Next

                    Dim htmlCell_Filding As New HtmlTableCell()
                    htmlCell_Filding.ID = "cell_" & RowIndex & "_0_" & 20
                    htmlCell_Filding.InnerText = "F 20"
                    dvSeatsInfo.RowFilter = "Seat_No=" & (20)
                    Me.ApplyCellSettings(htmlCell_Filding, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_Filding)

                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)
                    'tblRow.Cells.Add(New HtmlTableCell())


                End If


                If i = 2 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & j + 2
                            htmlCell.InnerText = j + 2
                            dvSeatsInfo.RowFilter = "Seat_No=" & (j + 2)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & j + 2
                            htmlCell.InnerText = j + 2
                            dvSeatsInfo.RowFilter = "Seat_No=" & (j + 2)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then

                            Dim htmlCell_Filding As New HtmlTableCell()
                            htmlCell_Filding.ID = "cell_" & RowIndex & "_0_" & 21
                            htmlCell_Filding.InnerText = "F 21"
                            dvSeatsInfo.RowFilter = "Seat_No=" & (21)
                            Me.ApplyCellSettings(htmlCell_Filding, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell_Filding)

                        End If

                        If j = 4 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & j - 2
                            htmlCell.InnerText = j - 2
                            dvSeatsInfo.RowFilter = "Seat_No=" & (j - 2)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                    Next

                End If



                If i = 5 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                            htmlCell.InnerText = i
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1
                            htmlCell.InnerText = i + 1
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then


                            Dim htmlCell_Filding As New HtmlTableCell()
                            htmlCell_Filding.ID = "cell_" & RowIndex & "_0_" & 22
                            htmlCell_Filding.InnerText = "F 22"
                            dvSeatsInfo.RowFilter = "Seat_No=" & (22)
                            Me.ApplyCellSettings(htmlCell_Filding, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell_Filding)

                        End If

                        If j = 4 Then

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 2
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j + 3 & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)
                        End If

                    Next

                End If


                If i = 7 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                            htmlCell.InnerText = i
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1
                            htmlCell.InnerText = i + 1
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then

                            Dim htmlCell_Filding As New HtmlTableCell()
                            htmlCell_Filding.ID = "cell_" & RowIndex & "_0_" & 23
                            htmlCell_Filding.InnerText = "F 23"
                            dvSeatsInfo.RowFilter = "Seat_No=" & (23)
                            Me.ApplyCellSettings(htmlCell_Filding, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell_Filding)

                        End If

                        If j = 4 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                            htmlCell.InnerText = i + 2
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                    Next

                End If

                If i = 10 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                            htmlCell.InnerText = i
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1
                            htmlCell.InnerText = i + 1
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then

                            Dim htmlCell_Filding As New HtmlTableCell()
                            htmlCell_Filding.ID = "cell_" & RowIndex & "_0_" & 24
                            htmlCell_Filding.InnerText = "F 24"
                            dvSeatsInfo.RowFilter = "Seat_No=" & (24)
                            Me.ApplyCellSettings(htmlCell_Filding, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell_Filding)

                        End If

                        If j = 4 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                            htmlCell.InnerText = i + 2
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                    Next

                End If


                If i = 13 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                            htmlCell.InnerText = i
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)



                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1
                            htmlCell.InnerText = i + 1
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then

                            Dim htmlCell_Filding As New HtmlTableCell()
                            htmlCell_Filding.ID = "cell_" & RowIndex & "_0_" & 25
                            htmlCell_Filding.InnerText = "F 25"
                            dvSeatsInfo.RowFilter = "Seat_No=" & (25)
                            Me.ApplyCellSettings(htmlCell_Filding, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell_Filding)


                        End If

                        If j = 4 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                            htmlCell.InnerText = i + 2
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                    Next

                End If

                If i = 16 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                            htmlCell.InnerText = i
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1
                            htmlCell.InnerText = i + 1
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                            htmlCell.InnerText = i + 2
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)

                        End If

                        If j = 4 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 3
                            htmlCell.InnerText = i + 3
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 3)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                    Next

                End If





                If 1 = 2 Then i = i + 3
                If 1 = 5 Then i = i + 2
                If 1 = 7 Then i = i + 3
                If 1 = 10 Then i = i + 3
                If 1 = 15 Then i = i + 4



                tblTickets.Rows.Add(tblRow)

                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If


        If (SeatCount = 13) Then

            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0

                If i = 10 Then



                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & 10
                    dvSeatsInfo.RowFilter = "Seat_No=" & (10)
                    htmlCell.InnerText = 10
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)
                    ' tblRow.Cells.Add(New HtmlTableCell())


                    Dim htmlCell_spancer_5523 As New HtmlTableCell()
                    dvSeatsInfo.RowFilter = "Seat_No=" & i + 5523
                    htmlCell_spancer_5523.ID = "cell_" & RowIndex & "_0_" & i + 552 & "Spacer"
                    htmlCell_spancer_5523.InnerText = ""
                    htmlCell_spancer_5523.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer_5523)


                    Dim htmlCell_11 As New HtmlTableCell()
                    htmlCell_11.ID = "cell_" & RowIndex & "_0_" & 11
                    dvSeatsInfo.RowFilter = "Seat_No=" & (11)
                    htmlCell_11.InnerText = 11
                    Me.ApplyCellSettings(htmlCell_11, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_11)
                    ' tblRow.Cells.Add(New HtmlTableCell())


                    Dim htmlCell_spancer_55231 As New HtmlTableCell()
                    dvSeatsInfo.RowFilter = "Seat_No=" & i + 55231
                    htmlCell_spancer_55231.ID = "cell_" & RowIndex & "_0_" & i + 55231 & "Spacer"
                    htmlCell_spancer_55231.InnerText = ""
                    htmlCell_spancer_55231.Attributes.Add("class", "Spacer")
                    tblRow.Cells.Insert(0, htmlCell_spancer_55231)

                    Dim htmlCell_12 As New HtmlTableCell()
                    htmlCell_12.ID = "cell_" & RowIndex & "_0_" & 12
                    dvSeatsInfo.RowFilter = "Seat_No=" & (12)
                    htmlCell_12.InnerText = 12
                    Me.ApplyCellSettings(htmlCell_12, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_12)
                    ' tblRow.Cells.Add(New HtmlTableCell())



                    Dim htmlCell_13 As New HtmlTableCell()
                    htmlCell_13.ID = "cell_" & RowIndex & "_0_" & 13
                    dvSeatsInfo.RowFilter = "Seat_No=" & (13)
                    htmlCell_13.InnerText = 13
                    Me.ApplyCellSettings(htmlCell_13, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_13)




                ElseIf i <= 9 Then
                    For j = 0 To 2

                        int_spancer = int_spancer + 1
                        If int_spancer = 2 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            htmlCell.InnerText = i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            'tblRow.Cells.Add(New HtmlTableCell())

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)
                            ' tblRow.Cells.Add(New HtmlTableCell())

                        Else
                            Dim htmlCell As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j

                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
                            htmlCell.InnerText = i + j
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                            ' tblRow.Cells.Add(New HtmlTableCell())

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j + 552
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j + 552 & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)
                            ' tblRow.Cells.Add(New HtmlTableCell())

                        End If
                    Next

                End If
                tblTickets.Rows.Add(tblRow)
                i = i + 2
                int_Seat_Counter = int_Seat_Counter + 1

            Next




        End If

        If (SeatCount = 15) Then




            For i As Integer = 1 To 6
                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0
                Dim rnd As New Random

                If i = 1 Then
                    For j As Integer = 0 To 2
                        Dim htmlCell_spancer As New HtmlTableCell()
                        dvSeatsInfo.RowFilter = "Seat_No=" & i + j
                        htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer" & rnd.Next(100, 200).ToString()
                        htmlCell_spancer.InnerText = ""
                        htmlCell_spancer.Attributes.Add("class", "Spacer")
                        tblRow.Cells.Insert(0, htmlCell_spancer)
                    Next

                    'Dim htmlCell_Filding As New HtmlTableCell()
                    'htmlCell_Filding.ID = "cell_" & RowIndex & "_0_" & 20
                    'htmlCell_Filding.InnerText = "F 20"
                    'dvSeatsInfo.RowFilter = "Seat_No=" & (20)
                    'Me.ApplyCellSettings(htmlCell_Filding, dvSeatsInfo)
                    'tblRow.Cells.Insert(0, htmlCell_Filding)

                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i & rnd.Next(1000, 2000).ToString()
                    htmlCell.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)

                    Dim htmlCell_1 As New HtmlTableCell()
                    htmlCell_1.ID = "cell_" & RowIndex & "_0_" & i + 1 & rnd.Next(10000, 20000).ToString()
                    htmlCell_1.InnerText = i + 1
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell_1, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell_1)
                    'tblRow.Cells.Add(New HtmlTableCell())


                End If


                If i = 2 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_3" & rnd.Next(1000, 2000).ToString()
                            htmlCell.InnerText = 3
                            dvSeatsInfo.RowFilter = "Seat_No=3"
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_4" & rnd.Next(1000, 2000).ToString()
                            htmlCell.InnerText = 4
                            dvSeatsInfo.RowFilter = "Seat_No=4"
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If
                        If j = 3 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_5" & rnd.Next(1000, 2000).ToString()
                            htmlCell.InnerText = 5
                            dvSeatsInfo.RowFilter = "Seat_No=5"
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 4 Then
                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer" & rnd.Next(1000, 2000).ToString()
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)
                        End If



                    Next

                End If


                If i = 3 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_6" & rnd.Next(1000, 2000).ToString()
                            htmlCell.InnerText = 6
                            dvSeatsInfo.RowFilter = "Seat_No=6"
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_7" & rnd.Next(1000, 2000).ToString()
                            htmlCell.InnerText = 7
                            dvSeatsInfo.RowFilter = "Seat_No=7"
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If
                        If j = 3 Then
                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer" & rnd.Next(1000, 2000).ToString()
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)
                        End If
                        If j = 4 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_8" & rnd.Next(1000, 2000).ToString()
                            htmlCell.InnerText = 8
                            dvSeatsInfo.RowFilter = "Seat_No=8"
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If



                    Next

                End If

                If i = 4 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_9" & rnd.Next(1000, 2000).ToString()
                            htmlCell.InnerText = 9
                            dvSeatsInfo.RowFilter = "Seat_No=9"
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_10" & rnd.Next(1000, 2000).ToString()
                            htmlCell.InnerText = 10
                            dvSeatsInfo.RowFilter = "Seat_No=10"
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then
                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + j
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer" & rnd.Next(1000, 2000).ToString()
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)
                        End If
                        If j = 4 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_11" & rnd.Next(1000, 2000).ToString()
                            htmlCell.InnerText = 11
                            dvSeatsInfo.RowFilter = "Seat_No=11"
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If



                    Next

                End If


                If i = 5 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_12" & rnd.Next(1000, 2000).ToString()
                            htmlCell.InnerText = 12
                            dvSeatsInfo.RowFilter = "Seat_No=12"
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_13" & rnd.Next(1000, 2000).ToString()
                            htmlCell.InnerText = 13
                            dvSeatsInfo.RowFilter = "Seat_No=13"
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_14" & rnd.Next(1000, 2000).ToString()
                            htmlCell.InnerText = 14
                            dvSeatsInfo.RowFilter = "Seat_No=14"
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If
                        If j = 4 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_15" & rnd.Next(1000, 2000).ToString()
                            htmlCell.InnerText = 15
                            dvSeatsInfo.RowFilter = "Seat_No=15"
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If



                    Next

                End If



                ' i = i + 1

                'If 1 = 2 Then i = i + 3
                'If 1 = 5 Then i = i + 2
                'If 1 = 7 Then i = i + 3
                'If 1 = 10 Then i = i + 3
                'If 1 = 15 Then i = i + 4

                If i >= 1 And i <= 6 Then
                    tblTickets.Rows.Add(tblRow)

                    int_Seat_Counter = int_Seat_Counter + 1

                End If


            Next

        End If


        If (SeatCount = 21) Then




            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0


                If i = 1 Then

                    For j As Integer = 0 To 2
                        Dim htmlCell_spancer As New HtmlTableCell()
                        dvSeatsInfo.RowFilter = "Seat_No=" & i + j
                        htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer"
                        htmlCell_spancer.InnerText = ""
                        htmlCell_spancer.Attributes.Add("class", "Spacer")
                        tblRow.Cells.Insert(0, htmlCell_spancer)
                    Next
                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)
                    'tblRow.Cells.Add(New HtmlTableCell())


                End If


                'If i = 2 Then

                '    For j As Integer = 1 To 4

                '        If j = 1 Then
                '            Dim htmlCell As New HtmlTableCell()
                '            htmlCell.ID = "cell_" & RowIndex & "_0_" & j + 2
                '            htmlCell.InnerText = j + 2
                '            dvSeatsInfo.RowFilter = "Seat_No=" & (j + 2)
                '            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                '            tblRow.Cells.Insert(0, htmlCell)
                '        End If

                '        If j = 2 Then
                '            Dim htmlCell As New HtmlTableCell()
                '            htmlCell.ID = "cell_" & RowIndex & "_0_" & j + 2
                '            htmlCell.InnerText = j + 2
                '            dvSeatsInfo.RowFilter = "Seat_No=" & (j + 2)
                '            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                '            tblRow.Cells.Insert(0, htmlCell)
                '        End If

                '        If j = 3 Then

                '            Dim htmlCell_spancer As New HtmlTableCell()
                '            dvSeatsInfo.RowFilter = "Seat_No=" & i + 3
                '            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j + 3 & "Spacer"
                '            htmlCell_spancer.InnerText = ""
                '            htmlCell_spancer.Attributes.Add("class", "Spacer")
                '            tblRow.Cells.Insert(0, htmlCell_spancer)

                '        End If

                '        If j = 4 Then
                '            Dim htmlCell As New HtmlTableCell()
                '            htmlCell.ID = "cell_" & RowIndex & "_0_" & j - 2
                '            htmlCell.InnerText = j - 2
                '            dvSeatsInfo.RowFilter = "Seat_No=" & (j - 2)
                '            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                '            tblRow.Cells.Insert(0, htmlCell)
                '        End If

                '    Next

                'End If



                If i = 2 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                            htmlCell.InnerText = i
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1
                            htmlCell.InnerText = i + 1
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 2
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j + 3 & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)

                        End If

                        If j = 4 Then

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 2
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j + 3 & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)
                        End If

                    Next

                End If


                If i = 4 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                            htmlCell.InnerText = i
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1
                            htmlCell.InnerText = i + 1
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 2
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j + 3 & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)

                        End If

                        If j = 4 Then

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 2
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j + 3 & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)
                        End If

                    Next

                End If


                If i = 6 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                            htmlCell.InnerText = i
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1
                            htmlCell.InnerText = i + 1
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 3
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j + 3 & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)

                        End If

                        If j = 4 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                            htmlCell.InnerText = i + 2
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                    Next

                End If

                If i = 10 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                            htmlCell.InnerText = i
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1
                            htmlCell.InnerText = i + 1
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 3
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j + 3 & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)

                        End If

                        If j = 4 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                            htmlCell.InnerText = i + 2
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                    Next

                End If


                If i = 13 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                            htmlCell.InnerText = i
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1
                            htmlCell.InnerText = i + 1
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 3
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j + 3 & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)

                        End If

                        If j = 4 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                            htmlCell.InnerText = i + 2
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                    Next

                End If

                If i = 15 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1
                            htmlCell.InnerText = i + 1
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                            htmlCell.InnerText = i + 2
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 4
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j + 4 & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)

                        End If

                        If j = 4 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 3
                            htmlCell.InnerText = i + 3
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 3)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                    Next

                End If

                If i = 19 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                            htmlCell.InnerText = i
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1
                            htmlCell.InnerText = i + 1
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                            htmlCell.InnerText = i + 2
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)

                        End If

                        If j = 4 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 3
                            htmlCell.InnerText = i + 3
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 3)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                    Next

                End If

                If 1 = 2 Then i = i + 3
                If 1 = 5 Then i = i + 2
                If 1 = 8 Then i = i + 2
                If 1 = 10 Then i = i + 3
                If 1 = 15 Then i = i + 4


                tblTickets.Rows.Add(tblRow)

                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If


        If (SeatCount = 21) Then




            For i As Integer = 1 To SeatCount

                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()
                int_spancer = 0


                If i = 1 Then

                    For j As Integer = 0 To 2
                        Dim htmlCell_spancer As New HtmlTableCell()
                        dvSeatsInfo.RowFilter = "Seat_No=" & i + j
                        htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer"
                        htmlCell_spancer.InnerText = ""
                        htmlCell_spancer.Attributes.Add("class", "Spacer")
                        tblRow.Cells.Insert(0, htmlCell_spancer)
                    Next
                    Dim htmlCell As New HtmlTableCell()
                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                    htmlCell.InnerText = i
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    tblRow.Cells.Insert(0, htmlCell)
                    'tblRow.Cells.Add(New HtmlTableCell())


                End If





                If i = 2 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                            htmlCell.InnerText = i
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1
                            htmlCell.InnerText = i + 1
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 2
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j + 3 & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)

                        End If

                        If j = 4 Then

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 2
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j + 3 & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)
                        End If

                    Next

                End If


                If i = 4 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                            htmlCell.InnerText = i
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1
                            htmlCell.InnerText = i + 1
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 2
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j + 3 & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)

                        End If

                        If j = 4 Then

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 2
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j + 3 & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)
                        End If

                    Next

                End If


                If i = 6 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                            htmlCell.InnerText = i
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1
                            htmlCell.InnerText = i + 1
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 3
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j + 3 & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)

                        End If

                        If j = 4 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                            htmlCell.InnerText = i + 2
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                    Next

                End If

                If i = 10 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                            htmlCell.InnerText = i
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1
                            htmlCell.InnerText = i + 1
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 3
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j + 3 & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)

                        End If

                        If j = 4 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                            htmlCell.InnerText = i + 2
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                    Next

                End If


                If i = 13 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                            htmlCell.InnerText = i
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1
                            htmlCell.InnerText = i + 1
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 3
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j + 3 & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)

                        End If

                        If j = 4 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                            htmlCell.InnerText = i + 2
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                    Next

                End If

                If i = 15 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1
                            htmlCell.InnerText = i + 1
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                            htmlCell.InnerText = i + 2
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then

                            Dim htmlCell_spancer As New HtmlTableCell()
                            dvSeatsInfo.RowFilter = "Seat_No=" & i + 4
                            htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j + 4 & "Spacer"
                            htmlCell_spancer.InnerText = ""
                            htmlCell_spancer.Attributes.Add("class", "Spacer")
                            tblRow.Cells.Insert(0, htmlCell_spancer)

                        End If

                        If j = 4 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 3
                            htmlCell.InnerText = i + 3
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 3)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                    Next

                End If

                If i = 19 Then

                    For j As Integer = 1 To 4

                        If j = 1 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i
                            htmlCell.InnerText = i
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 2 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1
                            htmlCell.InnerText = i + 1
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                        If j = 3 Then

                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
                            htmlCell.InnerText = i + 2
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)

                        End If

                        If j = 4 Then
                            Dim htmlCell As New HtmlTableCell()
                            htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 3
                            htmlCell.InnerText = i + 3
                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 3)
                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                            tblRow.Cells.Insert(0, htmlCell)
                        End If

                    Next

                End If

                If 1 = 2 Then i = i + 3
                If 1 = 5 Then i = i + 2
                If 1 = 8 Then i = i + 2
                If 1 = 10 Then i = i + 3
                If 1 = 15 Then i = i + 4


                tblTickets.Rows.Add(tblRow)

                int_Seat_Counter = int_Seat_Counter + 1

            Next

        End If



        Try
            Call TicketStatusCount()
        Catch ex As Exception
            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & line
        End Try

    End Sub

    'Public Sub PopulateTicketList(Optional ByVal SeatCount As Integer = 45)

    '    tblTickets.Disabled = True

    '    Dim dtseatsInfo As DataTable = Me.getSeatsInfo(cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Value)

    '    Dim dtCityInfo As DataTable = Me.getCityInfo(cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Value)


    '    Dim dtCityInfoColor As DataTable = Me.getCityInfoColor(cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Value)


    '    Dim dvSeatsInfo As New DataView(dtseatsInfo)

    '    Dim RowIndex As Short = 0
    '    Dim tblRow As HtmlTableRow
    '    Dim IsODDRows As Boolean = False
    '    Dim Counter_Row As Int16 = 4
    '    Dim Counter_Loop As Int16 = 5
    '    Dim Counter_Last = SeatCount - 1
    '    Dim FirstTime As Boolean = True

    '    Dim int_spancer As Integer = 0

    '    Dim int_Seat_Counter As Integer = 0

    '    grdCityInfo.DataSource = dtCityInfo
    '    grdCityInfo.DataBind()
    '    grdCityInfo.Columns(0).Width = 50
    '    grdCityInfo.Columns(1).Width = 350


    '    divCityList.InnerHtml = ""

    '    For Each dr As DataRow In dtCityInfoColor.Rows
    '        divCityList.InnerHtml = divCityList.InnerHtml & " <input  onclick=javascript:changecolors('" & dr("City_Name") & "','" & dr("Destination_Color") & "') style='height: 50px;min-width: 120px;font-size: 15px;font-weight: bold;border: 0px;cursor: pointer;background-color:#" & dr("Destination_Color") & "'  type='button' value='" & dr("City_Name") & " - " & dr("Total") & "' /> "
    '    Next




    '    If SeatCount = 30 Then


    '        For i As Integer = 0 To SeatCount - 1


    '            If RowIndex = 0 Then

    '                tblRow = tblTickets.Rows(0)
    '                Counter_Row = Counter_Row
    '                Dim htmlCell As New HtmlTableCell()
    '                tblRow.Cells.Clear()

    '                'If Not i + 1 > SeatCount Then

    '                '    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 4)
    '                '    Me.ApplyCellSettings(tblRow.Cells(0), dvSeatsInfo)
    '                '    tblRow.Cells(0).InnerText = i + 4
    '                'End If

    '                htmlCell = New HtmlTableCell()
    '                htmlCell.ID = "cell"
    '                htmlCell.Attributes.Add("class", "TicketAvailable")
    '                htmlCell.Attributes.Add("title", "")
    '                htmlCell.InnerText = 3

    '                htmlCell.ID = "3"
    '                tblRow.Cells.Insert(0, htmlCell)
    '                dvSeatsInfo.RowFilter = "Seat_No=" & 3
    '                Me.ApplyCellSettings(htmlCell, dvSeatsInfo)

    '                htmlCell = New HtmlTableCell()
    '                htmlCell.ID = "cell"
    '                htmlCell.Attributes.Add("class", "nothing")
    '                htmlCell.InnerText = ""

    '                htmlCell.ID = "Space"
    '                tblRow.Cells.Insert(1, htmlCell)



    '                htmlCell = New HtmlTableCell()
    '                htmlCell.ID = "cell"
    '                htmlCell.Attributes.Add("class", "TicketAvailable")
    '                htmlCell.Attributes.Add("title", "")
    '                htmlCell.InnerText = 2

    '                htmlCell.ID = "2"
    '                tblRow.Cells.Insert(2, htmlCell)
    '                dvSeatsInfo.RowFilter = "Seat_No=" & 2
    '                Me.ApplyCellSettings(htmlCell, dvSeatsInfo)

    '                htmlCell = New HtmlTableCell()
    '                htmlCell.ID = "cell"
    '                htmlCell.Attributes.Add("class", "TicketAvailable")
    '                htmlCell.Attributes.Add("title", "")
    '                htmlCell.InnerText = 1

    '                htmlCell.ID = "1"
    '                tblRow.Cells.Insert(3, htmlCell)
    '                dvSeatsInfo.RowFilter = "Seat_No=" & 1
    '                Me.ApplyCellSettings(htmlCell, dvSeatsInfo)


    '            Else

    '                tblRow = New HtmlTableRow()
    '                Dim htmlCell As New HtmlTableCell()

    '                If FirstTime = True Then
    '                    i = i - 1
    '                    FirstTime = False
    '                End If

    '                If i = Counter_Last Then

    '                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1

    '                    If Not i + 1 > SeatCount Then
    '                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
    '                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
    '                        'htmlCell.Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 1 & ", 2);")
    '                        'htmlCell.Attributes.Add("onclick", "OperateOnSeat(this, " & i + 1 & ", 1);")
    '                        htmlCell.InnerText = i + 1
    '                    End If
    '                    tblRow.Cells.Insert(0, htmlCell)
    '                    tblRow.Cells.Add(New HtmlTableCell())
    '                    tblTickets.Rows.Add(tblRow)

    '                Else

    '                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 4

    '                    If Not i + 1 > SeatCount Then
    '                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + 4)
    '                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
    '                        'htmlCell.Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 1 & ", 2);")
    '                        'htmlCell.Attributes.Add("onclick", "OperateOnSeat(this, " & i + 1 & ", 1);")
    '                        htmlCell.InnerText = i + 4
    '                    End If
    '                    tblRow.Cells.Insert(0, htmlCell)

    '                    htmlCell = New HtmlTableCell()
    '                    htmlCell.ID = "Spancee" & RowIndex & "_2_" & i + 2

    '                    tblRow.Cells.Insert(1, htmlCell)

    '                    htmlCell = New HtmlTableCell()
    '                    htmlCell.ID = "cell_" & RowIndex & "_1_" & i + 3
    '                    htmlCell.Attributes.Add("class", "TicketAvailable")
    '                    htmlCell.Attributes.Add("title", "")
    '                    If Not i + 2 > SeatCount Then
    '                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + 3)
    '                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
    '                        'htmlCell.Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 2 & ", 2);")
    '                        'htmlCell.Attributes.Add("onclick", "OperateOnSeat(this, " & i + 2 & ", 1);")
    '                        htmlCell.InnerText = i + 3
    '                    End If
    '                    tblRow.Cells.Insert(2, htmlCell)

    '                    htmlCell = New HtmlTableCell()
    '                    htmlCell.ID = "cell_" & RowIndex & "_2_" & i + 2
    '                    htmlCell.Attributes.Add("class", "TicketAvailable")

    '                    htmlCell.Attributes.Add("title", "")




    '                    If Not i + 3 > SeatCount Then
    '                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
    '                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
    '                        'htmlCell.Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 3 & ", 2);")
    '                        'htmlCell.Attributes.Add("onclick", "OperateOnSeat(this, " & i + 3 & ", 1);")
    '                        htmlCell.InnerText = i + 2
    '                        'htmlCell.Attributes.Add("style", "margin-left:30px")

    '                    End If

    '                    tblRow.Cells.Insert(3, htmlCell)

    '                    htmlCell = New HtmlTableCell()
    '                    htmlCell.ID = "cell_" & RowIndex & "_3_" & i + 1
    '                    htmlCell.Attributes.Add("class", "TicketAvailable")

    '                    tblTickets.Rows.Add(tblRow)


    '                End If


    '            End If

    '            RowIndex = RowIndex + 1
    '            i = i + 2
    '        Next

    '    ElseIf SeatCount = 15 Then

    '        For i As Integer = 0 To SeatCount - 1

    '            If RowIndex = 0 Then

    '                tblRow = tblTickets.Rows(0)
    '                Counter_Row = Counter_Row
    '                tblRow.Cells(0).InnerText = i + 4

    '                If Not i + 1 > SeatCount Then

    '                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 4)
    '                    Me.ApplyCellSettings(tblRow.Cells(0), dvSeatsInfo)
    '                    tblRow.Cells(0).InnerText = i + 4
    '                End If

    '                tblRow.Cells(1).InnerText = i + 3
    '                If Not i + 2 > SeatCount Then
    '                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 3)
    '                    Me.ApplyCellSettings(tblRow.Cells(1), dvSeatsInfo)
    '                    'tblRow.Cells(1).Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 2 & ", 2);")
    '                    'tblRow.Cells(1).Attributes.Add("onclick", "OperateOnSeat(this, " & i + 2 & ", 1);")
    '                    tblRow.Cells(1).InnerText = i + 3
    '                End If

    '                tblRow.Cells(2).InnerText = i + 2
    '                If Not i + 3 > SeatCount Then
    '                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
    '                    Me.ApplyCellSettings(tblRow.Cells(2), dvSeatsInfo)
    '                    'tblRow.Cells(2).Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 3 & ", 2);")
    '                    'tblRow.Cells(2).Attributes.Add("onclick", "OperateOnSeat(this, " & i + 3 & ", 1);")
    '                    'tblRow.Cells(2).Attributes.Add("style", "margin-left:30px")
    '                    tblRow.Cells(2).InnerText = i + 2
    '                End If

    '                tblRow.Cells(3).InnerText = i + 1
    '                If Not i + 4 > SeatCount Then
    '                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
    '                    Me.ApplyCellSettings(tblRow.Cells(3), dvSeatsInfo)
    '                    'tblRow.Cells(3).Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 4 & ", 2);")
    '                    'tblRow.Cells(3).Attributes.Add("onclick", "OperateOnSeat(this, " & i + 4 & ", 1);")
    '                    tblRow.Cells(3).InnerText = i + 1
    '                End If
    '            Else

    '                tblRow = New HtmlTableRow()
    '                Dim htmlCell As New HtmlTableCell()

    '                If i = Counter_Last Then

    '                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1

    '                    If Not i + 1 > SeatCount Then
    '                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
    '                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
    '                        'htmlCell.Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 1 & ", 2);")
    '                        'htmlCell.Attributes.Add("onclick", "OperateOnSeat(this, " & i + 1 & ", 1);")
    '                        htmlCell.InnerText = i + 1
    '                    End If
    '                    tblRow.Cells.Insert(0, htmlCell)
    '                    tblRow.Cells.Add(New HtmlTableCell())
    '                    tblTickets.Rows.Add(tblRow)

    '                Else

    '                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 4

    '                    If i = 12 Then


    '                        htmlCell = New HtmlTableCell()
    '                        htmlCell.ID = "cell_" & RowIndex & "_1_" & i + 3
    '                        htmlCell.Attributes.Add("class", "TicketAvailable")
    '                        htmlCell.Attributes.Add("title", "")
    '                        If Not i + 2 > SeatCount Then
    '                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 3)
    '                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
    '                            'htmlCell.Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 2 & ", 2);")
    '                            'htmlCell.Attributes.Add("onclick", "OperateOnSeat(this, " & i + 2 & ", 1);")
    '                            htmlCell.InnerText = i + 3
    '                        End If
    '                        tblRow.Cells.Insert(0, htmlCell)


    '                        htmlCell = New HtmlTableCell()
    '                        htmlCell.ID = "cell_" & RowIndex & "_1_" & i + 2
    '                        htmlCell.Attributes.Add("class", "TicketAvailable")
    '                        htmlCell.Attributes.Add("title", "")
    '                        If Not i + 2 > SeatCount Then
    '                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
    '                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
    '                            'htmlCell.Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 2 & ", 2);")
    '                            'htmlCell.Attributes.Add("onclick", "OperateOnSeat(this, " & i + 2 & ", 1);")
    '                            htmlCell.InnerText = i + 2
    '                        End If
    '                        tblRow.Cells.Insert(1, htmlCell)


    '                        htmlCell = New HtmlTableCell()
    '                        htmlCell.ID = "cell_" & RowIndex & "_1_" & i + 1
    '                        htmlCell.Attributes.Add("class", "TicketAvailable")
    '                        htmlCell.Attributes.Add("title", "")
    '                        If Not i + 2 > SeatCount Then
    '                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
    '                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
    '                            'htmlCell.Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 2 & ", 2);")
    '                            'htmlCell.Attributes.Add("onclick", "OperateOnSeat(this, " & i + 2 & ", 1);")
    '                            htmlCell.InnerText = i + 1
    '                        End If
    '                        tblRow.Cells.Insert(2, htmlCell)


    '                        tblRow.Cells.Add(New HtmlTableCell())
    '                        tblTickets.Rows.Add(tblRow)

    '                    Else

    '                        If Not i + 1 > SeatCount Then
    '                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 4)
    '                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
    '                            'htmlCell.Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 1 & ", 2);")
    '                            'htmlCell.Attributes.Add("onclick", "OperateOnSeat(this, " & i + 1 & ", 1);")
    '                            htmlCell.InnerText = i + 4
    '                        End If
    '                        tblRow.Cells.Insert(0, htmlCell)

    '                        htmlCell = New HtmlTableCell()
    '                        htmlCell.ID = "cell_" & RowIndex & "_1_" & i + 3
    '                        htmlCell.Attributes.Add("class", "TicketAvailable")
    '                        htmlCell.Attributes.Add("title", "")
    '                        If Not i + 2 > SeatCount Then
    '                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 3)
    '                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
    '                            'htmlCell.Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 2 & ", 2);")
    '                            'htmlCell.Attributes.Add("onclick", "OperateOnSeat(this, " & i + 2 & ", 1);")
    '                            htmlCell.InnerText = i + 3
    '                        End If
    '                        tblRow.Cells.Insert(1, htmlCell)

    '                        htmlCell = New HtmlTableCell()
    '                        htmlCell.ID = "cell_" & RowIndex & "_2_" & i + 2
    '                        htmlCell.Attributes.Add("class", "TicketAvailable")

    '                        htmlCell.Attributes.Add("title", "")

    '                        If Not i + 3 > SeatCount Then
    '                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
    '                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
    '                            'htmlCell.Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 3 & ", 2);")
    '                            'htmlCell.Attributes.Add("onclick", "OperateOnSeat(this, " & i + 3 & ", 1);")
    '                            htmlCell.InnerText = i + 2
    '                            'htmlCell.Attributes.Add("style", "margin-left:30px")

    '                        End If

    '                        tblRow.Cells.Insert(2, htmlCell)

    '                        htmlCell = New HtmlTableCell()
    '                        htmlCell.ID = "cell_" & RowIndex & "_3_" & i + 1
    '                        htmlCell.Attributes.Add("class", "TicketAvailable")
    '                        htmlCell.Attributes.Add("title", "")
    '                        If Not i + 4 > SeatCount Then
    '                            dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
    '                            Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
    '                            htmlCell.InnerText = i + 1
    '                        End If
    '                        tblRow.Cells.Insert(3, htmlCell)

    '                        tblRow.Cells.Add(New HtmlTableCell())
    '                        tblTickets.Rows.Add(tblRow)
    '                    End If
    '                End If

    '            End If

    '            RowIndex = RowIndex + 1
    '            i = i + 3
    '        Next

    '    ElseIf (SeatCount = 42) Then

    '        For i As Integer = 1 To SeatCount

    '            tblRow = New HtmlTableRow()
    '            int_spancer = 0


    '            If i > 9 And i < 36 Then

    '                For j = 0 To 3

    '                    int_spancer = int_spancer + 1
    '                    If int_spancer = 2 Then

    '                        Dim htmlCell As New HtmlTableCell()
    '                        htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
    '                        htmlCell.InnerText = i + j
    '                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
    '                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
    '                        tblRow.Cells.Insert(0, htmlCell)
    '                        'tblRow.Cells.Add(New HtmlTableCell())

    '                        Dim htmlCell_spancer As New HtmlTableCell()
    '                        dvSeatsInfo.RowFilter = "Seat_No=" & i + j
    '                        htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + j & "Spacer"
    '                        htmlCell_spancer.InnerText = ""
    '                        htmlCell_spancer.Attributes.Add("class", "Spacer")
    '                        tblRow.Cells.Insert(0, htmlCell_spancer)
    '                        ' tblRow.Cells.Add(New HtmlTableCell())

    '                    Else
    '                        Dim htmlCell As New HtmlTableCell()
    '                        dvSeatsInfo.RowFilter = "Seat_No=" & i + j

    '                        htmlCell.ID = "cell_" & RowIndex & "_0_" & i + j
    '                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + j)
    '                        htmlCell.InnerText = i + j
    '                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
    '                        tblRow.Cells.Insert(0, htmlCell)
    '                        ' tblRow.Cells.Add(New HtmlTableCell())
    '                    End If
    '                Next

    '            ElseIf i > 36 Then

    '                Dim htmlCell As New HtmlTableCell()
    '                htmlCell.ID = "cell_" & RowIndex & "_0_" & i
    '                htmlCell.InnerText = i
    '                dvSeatsInfo.RowFilter = "Seat_No=" & (i)
    '                Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
    '                tblRow.Cells.Insert(0, htmlCell)
    '                'tblRow.Cells.Add(New HtmlTableCell())

    '                Dim htmlCell_Second As New HtmlTableCell()
    '                htmlCell_Second.ID = "cell_" & RowIndex & "_0_" & i + 1
    '                htmlCell_Second.InnerText = i + 1
    '                dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
    '                Me.ApplyCellSettings(htmlCell_Second, dvSeatsInfo)
    '                tblRow.Cells.Insert(0, htmlCell_Second)


    '                Dim htmlCell_Third As New HtmlTableCell()
    '                htmlCell_Third.ID = "cell_" & RowIndex & "_0_" & i + 2
    '                htmlCell_Third.InnerText = i + 2
    '                dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
    '                Me.ApplyCellSettings(htmlCell_Third, dvSeatsInfo)
    '                tblRow.Cells.Insert(0, htmlCell_Third)


    '                Dim htmlCell_Forth As New HtmlTableCell()
    '                htmlCell_Forth.ID = "cell_" & RowIndex & "_0_" & i + 3
    '                htmlCell_Forth.InnerText = i + 3
    '                dvSeatsInfo.RowFilter = "Seat_No=" & (i + 3)
    '                Me.ApplyCellSettings(htmlCell_Forth, dvSeatsInfo)
    '                tblRow.Cells.Insert(0, htmlCell_Forth)



    '                Dim htmlCell_Fifth As New HtmlTableCell()
    '                htmlCell_Fifth.ID = "cell_" & RowIndex & "_0_" & i + 4
    '                htmlCell_Fifth.InnerText = i + 4
    '                dvSeatsInfo.RowFilter = "Seat_No=" & (i + 4)
    '                Me.ApplyCellSettings(htmlCell_Fifth, dvSeatsInfo)
    '                tblRow.Cells.Insert(0, htmlCell_Fifth)

    '            ElseIf i <= 9 Then

    '                Dim htmlCell As New HtmlTableCell()
    '                htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 2
    '                htmlCell.InnerText = i + 2
    '                dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
    '                Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
    '                tblRow.Cells.Insert(0, htmlCell)
    '                'tblRow.Cells.Add(New HtmlTableCell())

    '                Dim htmlCell_Second As New HtmlTableCell()
    '                htmlCell_Second.ID = "cell_" & RowIndex & "_0_" & i + 1
    '                htmlCell_Second.InnerText = i + 1
    '                dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
    '                Me.ApplyCellSettings(htmlCell_Second, dvSeatsInfo)
    '                tblRow.Cells.Insert(0, htmlCell_Second)


    '                Dim htmlCell_spancer As New HtmlTableCell()
    '                htmlCell_spancer.ID = "cell_" & RowIndex & "_0_" & i + 55 & "Spacer"
    '                htmlCell_spancer.InnerText = ""
    '                htmlCell_spancer.Attributes.Add("class", "Spacer")
    '                tblRow.Cells.Insert(0, htmlCell_spancer)



    '                Dim htmlCell_spancer_2 As New HtmlTableCell()
    '                htmlCell_spancer_2.ID = "cell_" & RowIndex & "_0_" & i + 56 & "Spacer"
    '                htmlCell_spancer_2.InnerText = ""
    '                htmlCell_spancer_2.Attributes.Add("class", "Spacer")
    '                tblRow.Cells.Insert(0, htmlCell_spancer_2)



    '                Dim htmlCell_Third As New HtmlTableCell()
    '                htmlCell_Third.ID = "cell_" & RowIndex & "_0_" & i
    '                htmlCell_Third.InnerText = i
    '                dvSeatsInfo.RowFilter = "Seat_No=" & (i)
    '                Me.ApplyCellSettings(htmlCell_Third, dvSeatsInfo)
    '                tblRow.Cells.Insert(0, htmlCell_Third)


    '            End If

    '            tblTickets.Rows.Add(tblRow)

    '            If i <= 9 Then
    '                i = i + 2
    '            ElseIf i > 9 And i < 36 Then
    '                i = i + 3
    '            Else
    '                i = i + 4
    '            End If


    '            int_Seat_Counter = int_Seat_Counter + 1

    '        Next


    '    Else



    '        For i As Integer = 0 To SeatCount - 1


    '            If RowIndex = 0 Then

    '                tblRow = tblTickets.Rows(0)
    '                Counter_Row = Counter_Row
    '                tblRow.Cells(0).InnerText = i + 4

    '                If Not i + 1 > SeatCount Then

    '                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 4)
    '                    Me.ApplyCellSettings(tblRow.Cells(0), dvSeatsInfo)
    '                    tblRow.Cells(0).InnerText = i + 4
    '                End If

    '                tblRow.Cells(1).InnerText = i + 3
    '                If Not i + 2 > SeatCount Then
    '                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 3)
    '                    Me.ApplyCellSettings(tblRow.Cells(1), dvSeatsInfo)
    '                    'tblRow.Cells(1).Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 2 & ", 2);")
    '                    'tblRow.Cells(1).Attributes.Add("onclick", "OperateOnSeat(this, " & i + 2 & ", 1);")
    '                    tblRow.Cells(1).InnerText = i + 3
    '                End If

    '                tblRow.Cells(2).InnerText = i + 2
    '                If Not i + 3 > SeatCount Then
    '                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
    '                    Me.ApplyCellSettings(tblRow.Cells(2), dvSeatsInfo)
    '                    'tblRow.Cells(2).Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 3 & ", 2);")
    '                    'tblRow.Cells(2).Attributes.Add("onclick", "OperateOnSeat(this, " & i + 3 & ", 1);")
    '                    'tblRow.Cells(2).Attributes.Add("style", "margin-left:30px")
    '                    tblRow.Cells(2).InnerText = i + 2
    '                End If

    '                tblRow.Cells(3).InnerText = i + 1
    '                If Not i + 4 > SeatCount Then
    '                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
    '                    Me.ApplyCellSettings(tblRow.Cells(3), dvSeatsInfo)
    '                    'tblRow.Cells(3).Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 4 & ", 2);")
    '                    'tblRow.Cells(3).Attributes.Add("onclick", "OperateOnSeat(this, " & i + 4 & ", 1);")
    '                    tblRow.Cells(3).InnerText = i + 1
    '                End If
    '            Else

    '                tblRow = New HtmlTableRow()
    '                Dim htmlCell As New HtmlTableCell()

    '                If i = Counter_Last Then

    '                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1

    '                    If Not i + 1 > SeatCount Then
    '                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
    '                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
    '                        'htmlCell.Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 1 & ", 2);")
    '                        'htmlCell.Attributes.Add("onclick", "OperateOnSeat(this, " & i + 1 & ", 1);")
    '                        htmlCell.InnerText = i + 1
    '                    End If
    '                    tblRow.Cells.Insert(0, htmlCell)
    '                    tblRow.Cells.Add(New HtmlTableCell())
    '                    tblTickets.Rows.Add(tblRow)

    '                Else

    '                    htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 4

    '                    If Not i + 1 > SeatCount Then
    '                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + 4)
    '                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
    '                        'htmlCell.Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 1 & ", 2);")
    '                        'htmlCell.Attributes.Add("onclick", "OperateOnSeat(this, " & i + 1 & ", 1);")
    '                        htmlCell.InnerText = i + 4
    '                    End If
    '                    tblRow.Cells.Insert(0, htmlCell)

    '                    htmlCell = New HtmlTableCell()
    '                    htmlCell.ID = "cell_" & RowIndex & "_1_" & i + 3
    '                    htmlCell.Attributes.Add("class", "TicketAvailable")
    '                    htmlCell.Attributes.Add("title", "")
    '                    If Not i + 2 > SeatCount Then
    '                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + 3)
    '                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
    '                        'htmlCell.Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 2 & ", 2);")
    '                        'htmlCell.Attributes.Add("onclick", "OperateOnSeat(this, " & i + 2 & ", 1);")
    '                        htmlCell.InnerText = i + 3
    '                    End If
    '                    tblRow.Cells.Insert(1, htmlCell)

    '                    htmlCell = New HtmlTableCell()
    '                    htmlCell.ID = "cell_" & RowIndex & "_2_" & i + 2
    '                    htmlCell.Attributes.Add("class", "TicketAvailable")

    '                    htmlCell.Attributes.Add("title", "")

    '                    If Not i + 3 > SeatCount Then
    '                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
    '                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
    '                        'htmlCell.Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 3 & ", 2);")
    '                        'htmlCell.Attributes.Add("onclick", "OperateOnSeat(this, " & i + 3 & ", 1);")
    '                        htmlCell.InnerText = i + 2
    '                        'htmlCell.Attributes.Add("style", "margin-left:30px")

    '                    End If

    '                    tblRow.Cells.Insert(2, htmlCell)

    '                    htmlCell = New HtmlTableCell()
    '                    htmlCell.ID = "cell_" & RowIndex & "_3_" & i + 1
    '                    htmlCell.Attributes.Add("class", "TicketAvailable")
    '                    htmlCell.Attributes.Add("title", "")
    '                    If Not i + 4 > SeatCount Then
    '                        dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
    '                        Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
    '                        'htmlCell.Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 4 & ", 2);")
    '                        'htmlCell.Attributes.Add("onclick", "OperateOnSeat(this, " & i + 4 & ", 1);")
    '                        htmlCell.InnerText = i + 1
    '                    End If
    '                    tblRow.Cells.Insert(3, htmlCell)

    '                    tblRow.Cells.Add(New HtmlTableCell())
    '                    tblTickets.Rows.Add(tblRow)
    '                    'tblRow = tblTickets.Rows(0)


    '                End If


    '            End If

    '            RowIndex = RowIndex + 1
    '            i = i + 3
    '        Next
    '    End If


    '    Try
    '        Call TicketStatusCount()
    '    Catch ex As Exception
    '        Dim trace = New Diagnostics.StackTrace(ex, True)
    '        Dim line As String = Right(trace.ToString, 5)

    '        lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & line
    '    End Try


    '    'Try
    '    '    Dim seats As String = ""
    '    '    dvSeatsInfo.RowFilter = " Status=2 And Issued_By=" & objUser.Id
    '    '    For i As Integer = 0 To dvSeatsInfo.Count - 1
    '    '        If seats = "" Then
    '    '            seats = seats & dvSeatsInfo.Item(i)("Seat_No")
    '    '        Else
    '    '            seats = seats & "," & dvSeatsInfo.Item(i)("Seat_No")
    '    '        End If
    '    '    Next
    '    '    txtSeatNo.Text = seats
    '    '    If txtAmount.Text = "" Then txtAmount.Text = "0"
    '    '    If txtBalance.Text = "" Then txtBalance.Text = "0"
    '    '    If txtFare.Text = "" Then txtFare.Text = "0"

    '    '    If dvSeatsInfo.Count > 0 Then
    '    '        txtTotal.Text = CType(txtFare.Text, Decimal) * dvSeatsInfo.Count
    '    '        If (CType(txtAmount.Text, Decimal) - CType(txtTotal.Text, Decimal) > 0) Then
    '    '            txtBalance.Text = CType(txtAmount.Text, Decimal) - CType(txtTotal.Text, Decimal)
    '    '        Else
    '    '            txtBalance.Text = "0.00"
    '    '        End If
    '    '    End If
    '    'Catch ex As Exception
    '    'End Try
    '    tblTickets.Disabled = False
    'End Sub

    Public Sub ApplyCellSettings(ByRef cell As HtmlTableCell, ByRef dvSeatInfo As DataView)

        If dvSeatInfo.Count > 0 Then

            If dvSeatInfo.Item(0)("Status") = "1" Then

                If "" & dvSeatInfo.Item(0)("Route_Sr_No") = "" Then
                    cell.Attributes.Add("class", "TicketAvailable")
                    cell.Attributes.Add("title", "Available" & vbCrLf & "Cancelled History: " & dvSeatInfo.Item(0)("Ded_Name") & " - " & dvSeatInfo.Item(0)("CurrentDate"))
                    cell.Attributes.Add("onclick", "OperateOnSeat(this, " & dvSeatInfo.Item(0)("Seat_No") & ", 2, 1, " & IIf("" & dvSeatInfo.Item(0)("Issued_By") = "", 0, dvSeatInfo.Item(0)("Issued_By")) & ");")
                Else
                    cell.InnerHtml = cell.InnerHtml & "<br /> <div style = 'float: right;color:white;font-size:9px;width:20px;' > " + dvSeatInfo.Item(0)("TransitCityName") + " </div>"

                    If dvSeatInfo.Item(0)("Genders") = "Female" Then
                        cell.Attributes.Add("class", "TicketAvailableTransitlady")
                    Else
                        cell.Attributes.Add("class", "TicketAvailableTransit")
                    End If

                    cell.Attributes.Add("title", "Available" & vbCrLf & "Cancelled History: " & dvSeatInfo.Item(0)("Ded_Name") & " - " & dvSeatInfo.Item(0)("CurrentDate"))
                    cell.Attributes.Add("onclick", "OperateOnSeat(this, " & dvSeatInfo.Item(0)("Seat_No") & ", 2, 1, " & IIf("" & dvSeatInfo.Item(0)("Issued_By") = "", 0, dvSeatInfo.Item(0)("Issued_By")) & ");")
                End If


            ElseIf dvSeatInfo.Item(0)("Status") = "2" Then

                cell.Attributes.Add("class", "TicketReserved" & vbCrLf & "Passenger: " & dvSeatInfo.Item(0)("Passenger_Name") & vbCrLf & "Reserved By: " & dvSeatInfo.Item(0)("Issued_By_Name") & vbCrLf & "Reserved On: " & dvSeatInfo.Item(0)("Issue_Date"))
                cell.Attributes.Add("title", "Reserved" & vbCrLf & "Reserved By: " & dvSeatInfo.Item(0)("Issued_By_Name") & vbCrLf & "Issued On: " & dvSeatInfo.Item(0)("Issue_Date") & vbCrLf & "Cancelled History: " & dvSeatInfo.Item(0)("Ded_Name") & " - " & dvSeatInfo.Item(0)("CurrentDate"))
                cell.Attributes.Add("onclick", "OperateOnSeat(this, " & dvSeatInfo.Item(0)("Seat_No") & ", 2, 2, " & IIf("" & dvSeatInfo.Item(0)("Issued_By") = "", 0, dvSeatInfo.Item(0)("Issued_By")) & ");")

            ElseIf dvSeatInfo.Item(0)("Status") = "3" Then

                If Not IsDBNull(dvSeatInfo.Item(0)("Telenor")) Then
                    If dvSeatInfo.Item(0)("Telenor") = "1" Then
                        cell.Attributes.Add("class", "TicketBookedTelenor")

                    Else

                        cell.Attributes.Add("class", "TicketBooked" & vbCrLf & "Passenger: " & dvSeatInfo.Item(0)("Passenger_Name") & vbCrLf & "From City : " & dvSeatInfo.Item(0)("Source") & vbCrLf & "Destination: " & dvSeatInfo.Item(0)("Destination") _
                                             & "Booked By: " & dvSeatInfo.Item(0)("Issued_By_Name") & vbCrLf & "Booked On: " & dvSeatInfo.Item(0)("Issue_Date") _
                                             & vbCrLf & "Mobile No: " & dvSeatInfo.Item(0)("Contact_No") & vbCrLf & "CNIC : " & dvSeatInfo.Item(0)("CNIC") & vbCrLf & "CollectionPoint : " & dvSeatInfo.Item(0)("CollectionPoint"))

                        If dvSeatInfo.Item(0)("Genders") = "Female" Then
                            cell.Attributes.Add("class", "TicketBookedlady")
                        Else
                            cell.Attributes.Add("class", "TicketBooked")
                        End If

                    End If
                Else
                    cell.Attributes.Add("class", "TicketBooked" & vbCrLf & "Passenger: " & dvSeatInfo.Item(0)("Passenger_Name") & vbCrLf & "From City : " & dvSeatInfo.Item(0)("Source") & vbCrLf & "Destination: " & dvSeatInfo.Item(0)("Destination") & vbCrLf & "Booked By: " & dvSeatInfo.Item(0)("Issued_By_Name") & vbCrLf & "Booked On: " & dvSeatInfo.Item(0)("Issue_Date") & vbCrLf & "Mobile No: " & dvSeatInfo.Item(0)("Contact_No") & vbCrLf & "CNIC : " & dvSeatInfo.Item(0)("CNIC") & vbCrLf & "CollectionPoint : " & dvSeatInfo.Item(0)("CollectionPoint"))
                    If dvSeatInfo.Item(0)("Genders") = "Female" Then
                        cell.Attributes.Add("class", "TicketBookedlady")
                    Else
                        cell.Attributes.Add("class", "TicketBooked")
                    End If
                End If


                cell.Attributes.Add("title", "Booked" & vbCrLf & "Passenger: " & dvSeatInfo.Item(0)("Passenger_Name") & vbCrLf & "From City : " & dvSeatInfo.Item(0)("Source") & vbCrLf & "Destination: " & dvSeatInfo.Item(0)("Destination") & vbCrLf & "Booked By: " & dvSeatInfo.Item(0)("Issued_By_Name") & vbCrLf & "Issued On: " & dvSeatInfo.Item(0)("Issue_Date") & vbCrLf & "Cancelled History: " & dvSeatInfo.Item(0)("Ded_Name") & " - " & dvSeatInfo.Item(0)("CurrentDate") & vbCrLf & "Mobile No: " & dvSeatInfo.Item(0)("Contact_No") & vbCrLf & "CNIC : " & dvSeatInfo.Item(0)("CNIC") & vbCrLf & "CollectionPoint : " & dvSeatInfo.Item(0)("CollectionPoint"))
                cell.Attributes.Add("onclick", "OperateOnSeat(this, " & dvSeatInfo.Item(0)("Seat_No") & ", 2, 3, " & IIf("" & dvSeatInfo.Item(0)("Issued_By") = "", 0, dvSeatInfo.Item(0)("Issued_By")) & ");")




            ElseIf dvSeatInfo.Item(0)("Status") = "4" Then
                cell.InnerHtml = cell.InnerHtml & "<br /> <div style = 'float:right;font-size:11px' > " + dvSeatInfo.Item(0)("Terminal_Abbr") + " </div>"


                If Val(dvSeatInfo.Item(0)("Discounted")) = 1 Then
                    cell.InnerHtml = cell.InnerHtml & "<div style = 'float:left;font-size:10px' > Disable </div>"


                End If
                cell.InnerHtml = cell.InnerHtml & "  <span style = 'float:right;' >  <span class='dot'  style = 'background-color:" + dvSeatInfo.Item(0)("Seat_Color") + ";' >   </span> </div>"
                Dim IsOnline As Integer = 0
                If IsDBNull((dvSeatInfo.Item(0)("Telenor"))) Then
                    IsOnline = 0
                Else
                    If dvSeatInfo.Item(0)("Telenor") = False Then
                        IsOnline = 0
                    Else
                        IsOnline = 1

                    End If
                End If

                If IsOnline = 1 Then
                    If IsOnline = 1 Then
                        If dvSeatInfo.Item(0)("Genders") = "Female" Then
                            cell.Attributes.Add("class", "TicketConfirmedladyTelenor")
                        Else
                            cell.Attributes.Add("class", "TicketConfirmedTelenor")
                        End If


                    Else
                        If dvSeatInfo.Item(0)("Genders") = "Female" Then
                            If dvSeatInfo.Item(0)("Transit") = "1" Then
                                cell.Attributes.Add("class", "TicketConfirmedladyTrasit")
                            Else
                                cell.Attributes.Add("class", "TicketConfirmedlady")
                            End If
                        Else
                            If dvSeatInfo.Item(0)("Transit") = "1" Then
                                cell.Attributes.Add("class", "TicketConfirmedTransit")
                            Else
                                cell.Attributes.Add("class", "TicketConfirmed")
                            End If

                        End If
                    End If


                Else
                    If dvSeatInfo.Item(0)("Genders") = "Female" Then
                        If dvSeatInfo.Item(0)("Transit") = "1" Then
                            cell.Attributes.Add("class", "TicketConfirmedladyTrasit")
                        Else
                            cell.Attributes.Add("class", "TicketConfirmedlady")
                        End If
                    Else
                        If dvSeatInfo.Item(0)("Transit") = "1" Then
                            cell.Attributes.Add("class", "TicketConfirmedTransit")
                        Else
                            cell.Attributes.Add("class", "TicketConfirmed")
                        End If

                    End If

                End If



                cell.Attributes.Add("title", "Confirmed" & vbCrLf & "Passenger: " & dvSeatInfo.Item(0)("Passenger_Name") & vbCrLf & "From City : " & dvSeatInfo.Item(0)("Source") & vbCrLf & "Destination: " & dvSeatInfo.Item(0)("Destination") & vbCrLf & "Confirmed By: " & dvSeatInfo.Item(0)("Issued_By_Name") & vbCrLf & "PNR: " & dvSeatInfo.Item(0)("Invoice_Id") & vbCrLf & "Issued On: " & dvSeatInfo.Item(0)("Issue_Date") & vbCrLf & "Mobile No: " & dvSeatInfo.Item(0)("Contact_No") & vbCrLf & "CNIC : " & dvSeatInfo.Item(0)("CNIC") & vbCrLf & "Cancelled History: " & dvSeatInfo.Item(0)("Ded_Name") & " - " & dvSeatInfo.Item(0)("CurrentDate") & vbCrLf & "Company : " & dvSeatInfo.Item(0)("Company_Name"))
                cell.Attributes.Add("onclick", "OperateOnSeat(this, " & dvSeatInfo.Item(0)("Seat_No") & ", 2, 4, " & IIf("" & dvSeatInfo.Item(0)("Issued_By") = "", 0, dvSeatInfo.Item(0)("Issued_By")) & ");")
                'cell.Attributes.Add("onclick", "OperateOnSeat(this, " & dvSeatInfo.Item(0)("Seat_No") & ", 1, 4);")

            End If
        Else
            cell.Attributes.Add("class", "TicketAvailable")
            cell.Attributes.Add("title", "Available")

        End If

    End Sub

    Public Function getSeatsInfo(ByVal TicketingScheduleID As Integer)
        Dim objTicketingList As New clsSeatTicketingList(objConnection)
        Return objTicketingList.GetAll(TicketingScheduleID, True)
    End Function


    Public Function getCityInfo(ByVal TicketingScheduleID As Integer)
        Dim objTicketingList As New clsSeatTicketingList(objConnection)
        Return objTicketingList.GetAllCity(TicketingScheduleID, True)
    End Function

    Public Function getCityInfoColor(ByVal TicketingScheduleID As Integer)

        Dim objTicketingList As New clsSeatTicketingList(objConnection)
        Return objTicketingList.GetAllCityColorWise(TicketingScheduleID, True)

    End Function

    Public Function getCompanyInfoColor(ByVal TicketingScheduleID As Integer)

        Dim objTicketingList As New clsSeatTicketingList(objConnection)
        Return objTicketingList.GetAllCompanyColorWise(TicketingScheduleID, True)


    End Function


    Public Sub clearValues()

        txtTicketNo.Text = ""
        txtPassengerName.Text = ""
        txtContactNo.Text = ""
        txtSeatNo.Text = ""
        txtAmount.Text = ""
        btnCancelTicket.Visible = False
        txtPNR.Text = ""
        txtCNIC2.Text = ""
        hndCustID.Value = 0
        txtCustomerNumber.Text = ""

        txtCNIC2.Enabled = True
        txtContactNo.Enabled = True
        txtCustomerNumber.Enabled = True
        btnValidateCustomers.Enabled = True
        txtPassengerName.Enabled = True

        btnCancelTicket.Visible = False
        btnReprint.Visible = False
        btnMissed.Visible = False
        btnSave.Visible = True
        ValidateCustomerPIN.Visible = False
        lblCustomerApproved.Text = ""
        hidCustomerPIN.Value = ""
        If hidMode.Value = "1" Then
            btnSave.Text = "Print"
        Else
            btnSave.Text = "Book"
        End If


        If hndTimDropped.Value = "1" Then
            MakeDisableTime()


        End If

    End Sub

    'Private Sub loadTable()
    '    Dim count = 0
    '    Dim dtRouteDetail As DataTable

    '    If RouteID = "" Then
    '        RouteID = "0"
    '    End If


    '    dtRouteDetail = objRouteDetail.GetRouteDetail(CInt(RouteID))

    '    Dim pk(0) As DataColumn
    '    pk(0) = dtRouteDetail.Columns("Route_Detail_ID")
    '    pk(0).AutoIncrement = True
    '    dtRouteDetail.PrimaryKey = pk
    '    dtRouteDetail.AcceptChanges()

    '    If (dtRouteDetail.Rows.Count > 0) Then
    '        pk(0).AutoIncrementSeed = dtRouteDetail.Rows(dtRouteDetail.Rows.Count - 1).Item("Route_Detail_ID") + 1
    '    Else
    '        pk(0).AutoIncrementSeed = 1
    '    End If

    '    table = dtRouteDetail
    'End Sub

    'Private Sub BindRouteDetail()
    '    grdRoutes.DataSource = table
    '    grdRoutes.DataBind()
    'End Sub

    'Private Sub RegisterClientEvents()
    '    btnSave.Attributes.Add("onclick", "return validation();")
    'End Sub

    Private Sub TicketStatusCount()
        Dim objTicketingSeat As New clsSeatTicketing(objConnection)
        Dim dt As DataTable = objTicketingSeat.GetTicketStatusCount(cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text)
        If Not dt Is Nothing Then
            If dt.Rows.Count > 0 Then
                lblB.Text = dt.Rows(0)("Booked")
                lblC.Text = dt.Rows(0)("Soled")
                lblA.Text = dt.Rows(0)("Available")
            End If
        End If
    End Sub

    Private Sub PopulateVoucherDeductions()
        Dim objBusCharges As New clsBusCharges(objConnection)
        objBusCharges.TicketingScheduleId = cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text

        objBusCharges.GetByTicketingScheduleId()

        txtBusCharges.Text = objBusCharges.BusCharges
        txtHostessSalary.Text = objBusCharges.HostessSalary
        txtDriverSalary.Text = objBusCharges.DriverSalary
        txtGuardSalary.Text = objBusCharges.GuardSalary
        txtServiceCharges.Text = objBusCharges.ServiceCharges
        txtCleaningCharges.Text = objBusCharges.CleaningCharges
        txtHookCharges.Text = objBusCharges.HookCharges
        txtBusCharges.Text = objBusCharges.BusCharges
        txtRefreshment.Text = objBusCharges.Refreshment
        txtToll.Text = objBusCharges.Toll

        txtTerminalExpense.Text = objBusCharges.Terminal_Expense
        txtPaidToDriver.Text = objBusCharges.PaidToDriver
        txtReward.Text = objBusCharges.Reward
        txtMisc.Text = objBusCharges.Misc

        txtcommission.Text = objBusCharges.Commision

        txtTotalDeductions.Text = objBusCharges.GetTotalDeductions()

    End Sub

    Private Sub ShowHideWithMode()
        If hidMode.Value = "2" Then
            btnSave.Text = "Book"
            'btnCancelTicket.Text = "Cancel Booking"
        End If
    End Sub

    Private Sub CalculateTotals()

        If cmbDestination.SelectedItem.Text = "Please select" Then
            txtFare.Text = ""
            txtTotal.Text = ""
            txtBalance.Text = ""
        End If
        Dim Fare As Decimal
        If IsNumeric(txtFare.Text.Trim()) Then
            Fare = CType(txtFare.Text.Trim(), Decimal)
        Else
            Fare = 0
        End If

        Dim SeatCount As Integer = 0
        SeatCount = txtSeatNo.Text.Trim().Split(",").Count
        Dim Total As Decimal = Fare * SeatCount

        txtTotal.Text = Total

        Dim Amount As Decimal
        If IsNumeric(txtAmount.Text.Trim()) Then
            Amount = CType(txtAmount.Text.Trim(), Decimal)
        Else
            Amount = 0
        End If

        Dim Balance As Decimal

        If Amount - Total > 0 Then
            Balance = Amount - Total
        Else
            Balance = 0
        End If

        txtBalance.Text = Balance.ToString("0000.00")


    End Sub

    'Private Function GetUnigueSeatNumbers(ByVal seats As String) As String
    '    Dim ArrSeats() As String = txtSeatNo.Text.Trim().Split(",")
    '    Dim ArrNewSeats() As String
    '    For i As Integer = 0 To ArrSeats.Count - 1
    '        Dim index As Integer = ArrNewSeats.Length
    '        ReDim Preserve ArrNewSeats(ArrNewSeats.Length + 1)
    '        If ArrSeats(i).Trim() <> hidSeatNo.Value Then
    '            If txtSeatNo.Text.Trim() = "" Then
    '                txtSeatNo.Text = ArrSeats(i)
    '            Else
    '                txtSeatNo.Text = txtSeatNo.Text & "," & ArrSeats(i)
    '            End If
    '        End If
    '    Next
    '    Return seats
    'End Function

    Private Function getNewVoucher(ByVal TSID As Long) As String
        objTicketing.Id = TSID
        Dim NewVoucherNumber As String = objTicketing.GetMaxVoucherNumber()
        Return NewVoucherNumber
    End Function

#End Region

    Private Sub lnkCreateOnline_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCreateOnline.Click

        lnkCreateOnline.Visible = False

        objOnlineTicketing = New eTicketing

        objTicketingSeatList = New clsSeatTicketingList(objConnection)

        objTicketing.Id = TicketingScheduleId
        objTicketing.GetById()

        objTicketingSeatList.GetAll(objTicketing.Id, False)

        'Create @ online DB



        objOnlineTicketing.CreateTicketingSchedule(objTicketing, objUser.Id, Val("" & (hndServiceType.Value)), objUser.Vendor_Id)

        hndOnlineTSNo.Value = objOnlineTicketing.IsOnlineTicketingScheduleOnLoad(objTicketing, Val("" & hndServiceType.Value), objUser.Vendor_Id)

        lblheader.Text = lblheader.Text.Replace("Schedule # : 0", "")

        lblheader.Text = "Schedule # : " & hndOnlineTSNo.Value & " " & lblheader.Text

        lnkCreateOnline.Visible = False

        Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)



    End Sub

    Private Sub lnkMapping_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkMapping.Click

        Try
            'Nomi_Jan
            rdoIDNumber.SelectedValue = "CNIC"
            Label2912556.Text = "CNIC"
            lblDouple.Text = " "
            lblboardingpoint.Text = " "
            lblurdu.Text = " "
            Label2912555.Visible = False
            txtCustomerNumber.Visible = False
            btnValidateCustomers.Visible = False

            Dim dtOnlineTickets As DataTable
            Dim dtLocalTickets As DataTable
            Dim onlinebookingchecker As DataTable


            Dim dtOnlineTicketsCancel As DataTable
            Dim dtLocalTicketsCancel As DataTable

            objOnlineTicketing = New eTicketing
            objTicketingSeatList = New clsSeatTicketingList(objConnection)

            If hndOnlineTSNo.Value = "" Or hndOnlineTSNo.Value = "0" Then




                objTicketing = New clsTicketing(objConnection)
                objOnlineTicketing = New eTicketing

                objTicketing.DepartureTime = "" & cboVoucherNo_1.Rows(0).Cells.FromKey("Departure_Time").Text
                objTicketing.TSDate = CDate(cboVoucherNo_1.Rows(0).Cells.FromKey("TS_Date").Value)
                objTicketing.ScheduleID = "" & cboVoucherNo_1.Rows(0).Cells.FromKey("Schedule_ID").Text
                Dim OnlineSchedule_Id As Integer = objOnlineTicketing.IsOnlineTicketingScheduleOnLoad(objTicketing, Val("" & hndServiceType.Value), objUser.Vendor_Id)
                hndOnlineTSNo.Value = OnlineSchedule_Id
            Else
                ' objOnlineTicketing.SendResponseToServer(hndOnlineTSNo.Value, objUser.Id, objUser.TerminalId)

            End If

            checkVoucherForDropTime()

            dtOnlineTickets = objOnlineTicketing.GetOnlineTicketsList_New(hndOnlineTSNo.Value, objUser.Vendor_Id)
            dtLocalTickets = objTicketingSeatList.GetAll(TicketingScheduleId, True)
            'onlinebookingchecker = BookingCheckonline(hndOnlineTSNo.Value)




            dtOnlineTicketsCancel = objOnlineTicketing.GetOnlineTicketsList_NewCancel(hndOnlineTSNo.Value, objUser.Vendor_Id)
            dtLocalTicketsCancel = objTicketingSeatList.GetAllCancel(hndOnlineTSNo.Value, True)

            If dtOnlineTickets Is Nothing Then

                lblErr.Text = objOnlineTicketing.mError & " . Server is not connected. Please login again."
                Exit Sub

            End If
            objTicketingSeat = New clsSeatTicketing(objConnection)

            Dim dtSrNumbers As DataTable = objTicketingSeat.GetAllRouteCities(TicketingScheduleId)
            Dim SQLServeMerge As New SQLServerMerg.SQLServerMerge.SQLServerMerge

            SQLServeMerge.CompareAndUpdate(TicketingScheduleId, dtOnlineTickets, dtLocalTickets, cmbSource.SelectedValue, hidTerminal.Value, dtSrNumbers)


            For Each dr As DataRow In dtLocalTickets.Rows

                If dr.RowState = DataRowState.Modified Then
                    objTicketingSeat.PopulateFromDataRow(dr)


                    objTicketingSeat.Save(False)

                End If
            Next




            If dtOnlineTicketsCancel.Rows.Count <> dtLocalTicketsCancel.Rows.Count Then
                Dim drFinded() As System.Data.DataRow
                Dim drFindedOnline() As System.Data.DataRow
                For Each dr_Online As DataRow In dtOnlineTicketsCancel.Rows
                    drFinded = dtLocalTicketsCancel.Select("Ticketing_Seat_ID='" & dr_Online("Ticketing_Seat_ID") & "' and PNR_No = '" & dr_Online("PNR_No") & "' ")
                    If drFinded.Count <= 0 Then
                        drFindedOnline = dtOnlineTicketsCancel.Select("Ticketing_Seat_ID='" & dr_Online("Ticketing_Seat_ID") & "' and PNR_No = '" & dr_Online("PNR_No") & "' ")
                        For Each drUpdate In drFindedOnline

                            objTicketing.UpdateTicketingScheduleCancel(drUpdate)
                        Next

                    End If

                Next
            End If


            objOnlineTicketing.UpdateOnlineTicketList(dtOnlineTickets)


        Catch ex As Exception
            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & line
        End Try

        clearValues()

        Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)

    End Sub

    Private Function BookingCheckonline(ByVal OnlineTicketingScheduleID) As DataTable
        Dim objDbManager As IDBManager
        Dim objDataSet As DataSet
        objDbManager = DBManager.GetDatabaseManager()
        objConnection = eConnectionManager.GetConnection()
        objDbManager.SetDBConnection(objConnection)
        Dim objDBParameters As New clsDBParameters
        'Session("TicketingScheduleId") = TicketingScheduleId

        objDBParameters.Parameters.Add(New clsDBParameter("@Ticketing_Schedule_ID", OnlineTicketingScheduleID, "bigint"))
        objDBParameters.Parameters.Add(New clsDBParameter("@Destination_ID", objUser.CityId, "bigint"))
        objDataSet = objDbManager.GetData("bookingstatuscheck", objDBParameters)
        If Not objDataSet Is Nothing Then
            Return objDataSet.Tables(0)
        Else
            Return Nothing
        End If
    End Function

    Private Sub UpdateRow(ByRef drOld As DataRow, ByRef drLatest As DataRow)

        If Not IsDBNull(drLatest("Status")) Then
            drOld("status") = drLatest("Status")
        End If

        If Not IsDBNull(drLatest("Issue_Date")) Then
            drOld("Issue_Date") = drLatest("Issue_Date")
        End If

        If Not IsDBNull(drLatest("Issue_Terminal")) Then
            drOld("Issue_Terminal") = drLatest("Issue_Terminal")
        End If

        If Not IsDBNull(drLatest("Issued_By")) Then
            drOld("Issued_By") = drLatest("Issued_By")
        End If

        If Not IsDBNull(drLatest("Source_ID")) Then
            drOld("Source_ID") = drLatest("Source_ID")
        End If

        If Not IsDBNull(drLatest("Destination_ID")) Then
            drOld("Destination_ID") = drLatest("Destination_ID")
        End If

        If Not IsDBNull(drLatest("Passenger_Name")) Then
            drOld("Passenger_Name") = drLatest("Passenger_Name")
        End If

        If Not IsDBNull(drLatest("Contact_No")) Then
            drOld("Contact_No") = drLatest("Contact_No")
        End If

        If Not IsDBNull(drLatest("Fare")) Then
            drOld("Fare") = drLatest("Fare")
        End If

        If Not IsDBNull(drLatest("Ticket_Sr_No")) Then
            drOld("Ticket_Sr_No") = drLatest("Ticket_Sr_No")
        End If
        If Not IsDBNull(drLatest("Gender")) Then
            drOld("Gender") = drLatest("Gender")
        End If

        If Not IsDBNull(drLatest("CNIC")) Then
            drOld("CNIC") = drLatest("CNIC")
        End If

        If Not IsDBNull(drLatest("Telenor")) Then
            drOld("Telenor") = drLatest("Telenor")
        End If

        If Not IsDBNull(drLatest("PNR_No")) Then
            drOld("PNR_No") = drLatest("PNR_No")
        End If

        If Not IsDBNull(drLatest("PaymentDate")) Then
            drOld("PaymentDate") = drLatest("PaymentDate")
        End If


        If Not IsDBNull(drLatest("Invoice_Id")) Then
            drOld("Invoice_Id") = drLatest("Invoice_Id")
        End If

        objUser = CType(Session("CurrentUser"), clsUser)
        If objUser.Is_Missed = False Then
            btnMissed.Visible = False

        Else

        End If

        objUser = CType(Session("CurrentUser"), clsUser)
        If objUser.Is_Skip = False Then
            btnSkip.Visible = False

        Else

        End If

    End Sub

    Private Function GetCitySerialNumber(ByVal CityID As Integer, ByRef dtRouteDetail As DataTable) As Integer
        Dim dv As New DataView(dtRouteDetail)

        dv.RowFilter = "City_ID=" & CityID.ToString()

        If dv.Count > 0 Then
            Return CType(dv.Item(0).Item("Sr_No"), Integer)
        Else
            Return 0
        End If

    End Function


    Private Function PrintReportBoarding2() As Boolean

        Try



            Dim SourcePath As String = Request.PhysicalApplicationPath & "Reports\rptPrintTicket3.rpt" 'This is just an example string and could be anything, it maps to fileToCopy in your code.
            Dim SaveDirectory As String = "c:\DestinationFolder"


            If Not System.IO.File.Exists(SourcePath) Then
                Exit Function

            End If

            Dim ServiceFare As String

            Dim tblOtherFare As New DataTable
            tblOtherFare = GetTicketingOtherFare()
            Dim rows As DataRow = tblOtherFare.Rows(0)

            If (MYKM = 0) Then
                ServiceFare = rows("ItemPrice").ToString()
            Else
                ServiceFare = MYKM

            End If




            Dim oReport As CrystalDecisions.CrystalReports.Engine.ReportDocument
            oReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument
            oReport.Load(Request.PhysicalApplicationPath & "Reports\rptPrintTicket3.rpt")
            objUser = CType(Session("CurrentUser"), clsUser)

            CurrentUserID = "" & objUser.Id
            TerminalId = "" & objUser.TerminalId
            UserName = objUser.Id
            Dim strRePrint As String = objTicketing.GetCanceltedTickets(objTicketingSeat.SeatNo, objTicketingSeat.TicketingScheduleID)


            Dim objPrintInfo As New clsPrintInfo
            With objPrintInfo
                Dim Status As String = ""
                .TicketingScheduleID = objTicketingSeat.TicketingScheduleID
                .TicketNumber = objTicketingSeat.TicketSrNo
                .PassengerName = objTicketingSeat.PassengerName & ""
                .ContactNumber = objTicketingSeat.PassengerContact
                .SeatNumber = objTicketingSeat.SeatNo
                .Fare = objTicketingSeat.Fare
                ' .Fare = txtFare.Text
                .Route = hidRoute.Value.Trim
                .DepartureTime = txtDepartureDate.Text.Trim + " " + txtDepartureTime.Text.Trim
                .VehicleNumber = txtVehicleNo.Text

                cReportUtility.PassParameter("TicketNo", .TicketingScheduleID.ToString() + "-" + objTicketingSeat.SeatNo.ToString(), oReport)
                cReportUtility.PassParameter("Name", .PassengerName, oReport)
                cReportUtility.PassParameter("Source", cmbSource.SelectedItem.Text, oReport)
                cReportUtility.PassParameter("Destination", cmbDestination.SelectedItem.Text, oReport)
                cReportUtility.PassParameter("TDate", hndPrintDateTime.Value, oReport)
                cReportUtility.PassParameter("TTime", txtDepartureTime.Text.Trim, oReport)
                Dim PrintMultipleInSingle As String = System.Configuration.ConfigurationManager.AppSettings.Item("PrintMultipleInSingle")

                If (PrintMultipleInSingle = "Yes") Then
                    Dim pi As Double
                    pi = txtFare.Text * txtCount.Text
                    cReportUtility.PassParameter("Fare", pi, oReport)

                Else
                    .Fare = txtFare.Text
                    cReportUtility.PassParameter("Fare", .Fare, oReport)

                End If

                ' cReportUtility.PassParameter("Fare", .Fare, oReport)
                cReportUtility.PassParameter("CoachNo", cmbVehicle.SelectedItem.Text, oReport)
                cReportUtility.PassParameter("SeatNo", .SeatNumber, oReport)
                cReportUtility.PassParameter("ActualDepartureTime", txtActualDepartureTime.Text, oReport)
                cReportUtility.PassParameter("UserName", UserName, oReport)
                cReportUtility.PassParameter("OnlineScheduleId", hndOnlineTSNo.Value, oReport)
                cReportUtility.PassParameter("TFCharges", ServiceFare, oReport)

                If chkOnline.Checked = True Then
                    Status = "ON"
                Else
                    Status = "OFF"
                End If
                cReportUtility.PassParameter("Status", Status, oReport)

                cReportUtility.PassParameter("TerminalID", objUser.TerminalId, oReport)

                cReportUtility.PassParameter("PrintDate", Now.Date.Day & "/" & Format(Now.Date.Month, "00") & "/" & Now.Date.Year, oReport)

                If strRePrint <> "" Then
                    cReportUtility.PassParameter("RePrint", strRePrint, oReport)
                Else
                    cReportUtility.PassParameter("RePrint", "", oReport)
                End If

                'If cmbDropAt.Items.Count > 0 Then
                '    If cmbDropAt.SelectedItem.Text = "Please select" Then
                '        cReportUtility.PassParameter("DropAt", "", oReport)
                '    Else
                '        cReportUtility.PassParameter("DropAt", cmbDropAt.SelectedItem.Text, oReport)
                '    End If
                'Else
                '    cReportUtility.PassParameter("DropAt", "", oReport)
                'End If
                cReportUtility.PassParameter("DropAt", "", oReport)



                cReportUtility.PassParameter("TerminalName", objUser.TerminalName, oReport)

            End With

            Dim i As Integer

            Dim doctoprint As New System.Drawing.Printing.PrintDocument()


            'doctoprint.PrinterSettings.PrinterName = "EPSONLQ3002" 'System.Configuration.ConfigurationManager.AppSettings.Item("PrinterName") '"\\thokar\EpsonLQ-" '(ex. "Epson SQ-1170 ESC/P 2")

            Dim rawKind As Integer

            doctoprint.PrinterSettings.PrinterName = System.Configuration.ConfigurationManager.AppSettings.Item("PrinterName")

            For i = 0 To doctoprint.PrinterSettings.PaperSizes.Count - 1
                If doctoprint.PrinterSettings.PaperSizes(i).PaperName = "FMF TICKET Boarding" Then

                    'rawKind = CInt(doctoprint.PrinterSettings.PaperSizes(i).GetType().GetField("kind", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic).GetValue(doctoprint.PrinterSettings.PaperSizes(i)))

                    'oReport.PrintOptions.PaperSize = rawKind

                    Exit For
                End If
            Next

            If rawKind = 0 Then
                rawKind = 119

                oReport.PrintOptions.PaperSize = rawKind
            Else
                oReport.PrintOptions.PaperSize = rawKind
            End If

            oReport.PrintOptions.PrinterName = System.Configuration.ConfigurationManager.AppSettings.Item("PrinterName")
            Dim PrintWithoutPrinter As String = System.Configuration.ConfigurationManager.AppSettings.Item("PrintWithoutPrinter")
            If PrintWithoutPrinter = "No" Then
                oReport.PrintToPrinter(1, False, 0, 0)
            End If


            oReport.Close()
            oReport.Dispose()
            GC.Collect()

            Return True

        Catch ex As Exception
            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & trace.ToString

            Return False
        Finally

            GC.Collect()
        End Try



    End Function


    Private Function PrintReportBoarding(ByVal SeatNo As String, ByVal fare As String) As Boolean

        Try

            'PrintReportBoarding2()

            Dim oReport As CrystalDecisions.CrystalReports.Engine.ReportDocument
            oReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument
            oReport.Load(Request.PhysicalApplicationPath & "Reports\rptPrintTicket2.rpt")
            objUser = CType(Session("CurrentUser"), clsUser)

            CurrentUserID = "" & objUser.Id
            TerminalId = "" & objUser.TerminalId
            UserName = objUser.Id
            Dim strRePrint As String = objTicketing.GetCanceltedTickets(objTicketingSeat.SeatNo, objTicketingSeat.TicketingScheduleID)


            Dim objPrintInfo As New clsPrintInfo
            With objPrintInfo
                Dim Status As String = ""
                .TicketingScheduleID = objTicketingSeat.TicketingScheduleID
                .TicketNumber = objTicketingSeat.TicketSrNo
                .PassengerName = objTicketingSeat.PassengerName & ""
                .ContactNumber = objTicketingSeat.PassengerContact
                .SeatNumber = SeatNo
                Dim OtherServiceFare As Double

                OtherServiceFare = objTicketingSeat.ServiceFare

                If (MYKM = 0) Then
                    .Fare = fare - OtherServiceFare
                Else
                    .Fare = fare

                End If


                ' .Fare = txtFare.Text
                .Route = hidRoute.Value.Trim
                .DepartureTime = txtDepartureDate.Text.Trim + " " + txtDepartureTime.Text.Trim
                .VehicleNumber = txtVehicleNo.Text

                cReportUtility.PassParameter("TicketNo", .TicketingScheduleID.ToString() + "-" + objTicketingSeat.SeatNo.ToString(), oReport)
                cReportUtility.PassParameter("Name", .PassengerName, oReport)
                cReportUtility.PassParameter("Source", cmbSource.SelectedItem.Text, oReport)
                cReportUtility.PassParameter("Destination", cmbDestination.SelectedItem.Text, oReport)
                cReportUtility.PassParameter("TDate", hndPrintDateTime.Value, oReport)
                cReportUtility.PassParameter("TTime", txtDepartureTime.Text.Trim, oReport)
                Dim PrintMultipleInSingle As String = System.Configuration.ConfigurationManager.AppSettings.Item("PrintMultipleInSingle")

                If (PrintMultipleInSingle = "Yes") Then
                    Dim pi As Double
                    pi = txtFare.Text * txtCount.Text
                    ' pi = .Fare * txtCount.Text
                    cReportUtility.PassParameter("Fare", pi, oReport)

                Else
                    '.Fare = txtFare.Text
                    ' .Fare = fare
                    cReportUtility.PassParameter("Fare", .Fare, oReport)

                End If

                ' cReportUtility.PassParameter("Fare", .Fare, oReport)
                Dim MGCompany As String = System.Configuration.ConfigurationManager.AppSettings.Item("Operated_By")

                If (objUser.AllowOperatedCompany = True) Then
                    cReportUtility.PassParameter("OperatedCompanyName", MGCompany, oReport)

                Else
                    cReportUtility.PassParameter("OperatedCompanyName", "", oReport)

                End If
                cReportUtility.PassParameter("CoachNo", cmbVehicle.SelectedItem.Text, oReport)
                cReportUtility.PassParameter("SeatNo", .SeatNumber, oReport)
                cReportUtility.PassParameter("ActualDepartureTime", txtActualDepartureTime.Text, oReport)
                cReportUtility.PassParameter("UserName", UserName, oReport)
                cReportUtility.PassParameter("OnlineScheduleId", hndOnlineTSNo.Value, oReport)




                If (DiscountFare.Text = 0) Then
                    Dim pi As Double
                    pi = DiscountFare.Text
                    cReportUtility.PassParameter("DiscountAmount", pi, oReport)

                Else
                    Dim pi As Double
                    pi = .Fare - DiscountFare.Text
                    cReportUtility.PassParameter("DiscountAmount", pi, oReport)
                End If

                If chkOnline.Checked = True Then
                    Status = "ON"
                Else
                    Status = "OFF"
                End If
                cReportUtility.PassParameter("Status", Status, oReport)

                cReportUtility.PassParameter("TerminalID", objUser.TerminalId, oReport)

                cReportUtility.PassParameter("PrintDate", Now.Date.Day & "/" & Format(Now.Date.Month, "00") & "/" & Now.Date.Year, oReport)

                If strRePrint <> "" Then
                    cReportUtility.PassParameter("RePrint", strRePrint, oReport)
                Else
                    cReportUtility.PassParameter("RePrint", "", oReport)
                End If

                'If cmbDropAt.Items.Count > 0 Then
                '    If cmbDropAt.SelectedItem.Text = "Please select" Then
                '        cReportUtility.PassParameter("DropAt", "", oReport)
                '    Else
                '        cReportUtility.PassParameter("DropAt", cmbDropAt.SelectedItem.Text, oReport)
                '    End If
                'Else
                '    cReportUtility.PassParameter("DropAt", "", oReport)
                'End If
                cReportUtility.PassParameter("DropAt", "", oReport)



                cReportUtility.PassParameter("TerminalName", objUser.TerminalName, oReport)

            End With

            Dim i As Integer

            Dim doctoprint As New System.Drawing.Printing.PrintDocument()


            'doctoprint.PrinterSettings.PrinterName = "EPSONLQ3002" 'System.Configuration.ConfigurationManager.AppSettings.Item("PrinterName") '"\\thokar\EpsonLQ-" '(ex. "Epson SQ-1170 ESC/P 2")

            Dim rawKind As Integer

            doctoprint.PrinterSettings.PrinterName = System.Configuration.ConfigurationManager.AppSettings.Item("PrinterName")

            For i = 0 To doctoprint.PrinterSettings.PaperSizes.Count - 1
                If doctoprint.PrinterSettings.PaperSizes(i).PaperName = "FMF TICKET Boarding" Then

                    'rawKind = CInt(doctoprint.PrinterSettings.PaperSizes(i).GetType().GetField("kind", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic).GetValue(doctoprint.PrinterSettings.PaperSizes(i)))

                    'oReport.PrintOptions.PaperSize = rawKind

                    Exit For
                End If
            Next

            If rawKind = 0 Then
                rawKind = 119

                oReport.PrintOptions.PaperSize = rawKind
            Else
                oReport.PrintOptions.PaperSize = rawKind
            End If

            oReport.PrintOptions.PrinterName = System.Configuration.ConfigurationManager.AppSettings.Item("PrinterName")
            Dim PrintWithoutPrinter As String = System.Configuration.ConfigurationManager.AppSettings.Item("PrintWithoutPrinter")
            If PrintWithoutPrinter = "No" Then
                oReport.PrintToPrinter(1, False, 0, 0)
            End If


            oReport.Close()
            oReport.Dispose()
            GC.Collect()

            If TFrdoprints.SelectedValue = "WithoutTF" Then

                objTicketingSeat.AllowTF = "WithoutTF"
            Else
                If (objUser.CanTFService = True) Then

                    PrintReportBoarding2()
                Else

                End If



            End If



            Return True

        Catch ex As Exception
            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & trace.ToString


            Return False
        Finally

            GC.Collect()
        End Try



    End Function

    Private Function PrintReport(ByVal seatnos As String, ByVal fares As Integer) As Boolean

        Try





            Dim PrintWithoutPrinter As String = System.Configuration.ConfigurationManager.AppSettings.Item("PrintWithoutPrinter")
            Dim versionNumber As Version

            versionNumber = Assembly.GetExecutingAssembly().GetName().Version
            Dim oReport As CrystalDecisions.CrystalReports.Engine.ReportDocument
            If PrintWithoutPrinter = "Yes" Then Return True

            If rdoprints.SelectedValue = "Business" Then


                oReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument
                oReport.Load(Request.PhysicalApplicationPath & "Reports\rptPrintTicket.rpt")

            Else


                oReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument
                oReport.Load(Request.PhysicalApplicationPath & "Reports\rptPrintTicketNormal.rpt")


            End If

            objUser = CType(Session("CurrentUser"), clsUser)

            CurrentUserID = "" & objUser.Id
            TerminalId = "" & objUser.TerminalId
            UserName = objUser.Id
            Dim strRePrint As String = objTicketing.GetCanceltedTickets(objTicketingSeat.SeatNo, objTicketingSeat.TicketingScheduleID)



            Dim objPrintInfo As New clsPrintInfo
            With objPrintInfo
                Dim Status As String = ""
                Dim value As String = ""
                value = objTicketingSeat.TicketingScheduleID.ToString() + "-" + objTicketingSeat.SeatNo.ToString()
                GetQR(value)
                .TicketingScheduleID = objTicketingSeat.TicketingScheduleID
                .TicketNumber = objTicketingSeat.TicketSrNo
                '.PassengerName = objTicketingSeat.PassengerName & ""
                .PassengerName = txtPassengerName.Text & ""
                .ContactNumber = objTicketingSeat.PassengerContact

                .SeatNumber = seatnos



                Dim ServiceFare As String
                Dim OnBoardService As String

                Dim tblOtherFare As New DataTable
                tblOtherFare = GetTicketingOtherFare()
                Dim rows As DataRow = tblOtherFare.Rows(0)
                ServiceFare = rows("ItemPrice").ToString()
                OnBoardService = rows("OnBoardService").ToString()

                If (objTicketingSeat.Fare = 0) Then
                    .Fare = objTicketingSeat.Fare
                Else

                    If (MYKM = 0) Then
                        .Fare = objTicketingSeat.Fare - ServiceFare
                    Else
                        .Fare = objTicketingSeat.Fare
                    End If



                End If





                'Dim othServiceCharges As String
                'Dim objOtherChargeslit As New DataTable
                'objOtherChargeslit = GetTicketingOthercharges()
                'Dim row1 As DataRow = objOtherChargeslit.Rows(0)


                'othServiceCharges = row1("OtherCharges").ToString()

                ' .Fare = txtFare.Text
                Dim PrintMultipleInSingle As String = System.Configuration.ConfigurationManager.AppSettings.Item("PrintMultipleInSingle")

                If (PrintMultipleInSingle = "Yes") Then



                    Dim pi As Double
                    pi = fares * txtCount.Text
                    cReportUtility.PassParameter("Fare", pi, oReport)

                Else


                    If (objUser.CanTFService = True) Then


                        If (objTicketingSeat.Fare = 0) Then
                            .Fare = objTicketingSeat.Fare
                        Else

                            If (MYKM = 0) Then
                                .Fare = objTicketingSeat.Fare - ServiceFare
                            Else
                                .Fare = objTicketingSeat.Fare
                            End If
                            '.Fare = objTicketingSeat.Fare - ServiceFare
                        End If


                    Else
                        .Fare = objTicketingSeat.Fare
                    End If




                    cReportUtility.PassParameter("Fare", .Fare, oReport)

                End If

                If objUser.CanChangeFare = True Then
                    Dim pii As Double

                    pii = txtFare.Text * txtCount.Text
                    cReportUtility.PassParameter("Fare", pii, oReport)

                Else
                    Dim pii As Double

                    pii = txtFare.Text * txtCount.Text
                    cReportUtility.PassParameter("Fare", .Fare, oReport)

                End If



                .Route = hidRoute.Value.Trim
                .DepartureTime = txtDepartureDate.Text.Trim + " " + txtDepartureTime.Text.Trim
                .VehicleNumber = txtVehicleNo.Text

                Dim dt_GetScheduleData As DataTable = objTicketing.GetScheduleData()

                If (dt_GetScheduleData.Rows.Count = 0) Then
                    Dim BusType As String
                    BusType = ""
                    cReportUtility.PassParameter("BusTypeName", BusType, oReport)
                Else
                    Dim BusType As String
                    BusType = dt_GetScheduleData.Rows(0)("ServiceType_Name")
                    cReportUtility.PassParameter("BusTypeName", BusType, oReport)
                End If



                Dim MGCompany As String = System.Configuration.ConfigurationManager.AppSettings.Item("Operated_By")

                cReportUtility.PassParameter("TicketNo", .TicketingScheduleID.ToString() + "-" + objTicketingSeat.SeatNo.ToString(), oReport)
                cReportUtility.PassParameter("Name", .PassengerName, oReport)
                If (objUser.AllowOperatedCompany = True) Then
                    cReportUtility.PassParameter("OperatedCompanyName", MGCompany, oReport)

                Else
                    cReportUtility.PassParameter("OperatedCompanyName", "", oReport)

                End If

                cReportUtility.PassParameter("Source", cmbSource.SelectedItem.Text, oReport)
                cReportUtility.PassParameter("Destination", cmbDestination.SelectedItem.Text, oReport)
                cReportUtility.PassParameter("TDate", hndPrintDateTime.Value, oReport)
                cReportUtility.PassParameter("SeatNo", .SeatNumber, oReport)
                cReportUtility.PassParameter("TTime", txtDepartureTime.Text.Trim, oReport)

                If objUser.CanChangeFare = True Then
                    Dim pii As Double

                    pii = txtFare.Text * txtCount.Text
                    cReportUtility.PassParameter("Fare", pii, oReport)

                Else
                    cReportUtility.PassParameter("Fare", .Fare, oReport)


                End If


                cReportUtility.PassParameter("CoachNo", txtVehicle.Text, oReport)
                cReportUtility.PassParameter("ActualDepartureTime", txtActualDepartureTime.Text, oReport)
                cReportUtility.PassParameter("UserName", UserName, oReport)
                cReportUtility.PassParameter("VerionNumber", versionNumber.ToString(), oReport)
                cReportUtility.PassParameter("ServiceType", hndServiceType.Value, oReport)
                cReportUtility.PassParameter("OnlineScheduleId", hndOnlineTSNo.Value, oReport)

                If (objUser.CanTFService = True) Then

                    cReportUtility.PassParameter("OtherServiceCharges", "0", oReport)
                    cReportUtility.PassParameter("OldFare", objTicketingSeat.Fare, oReport)
                    cReportUtility.PassParameter("txtRefreshment", txtRefreshmentiteam.Text, oReport)
                    cReportUtility.PassParameter("txtTerminalIssuance", txtTerminalIssuance.Text, oReport)
                Else
                    cReportUtility.PassParameter("OtherServiceCharges", "On Board Service Charges: 0", oReport)
                    cReportUtility.PassParameter("OldFare", objTicketingSeat.Fare, oReport)
                    cReportUtility.PassParameter("txtRefreshment", "0", oReport)
                    cReportUtility.PassParameter("txtTerminalIssuance", "0", oReport)

                End If

                ' cReportUtility.PassParameter("BookkaruFare", hndOnlineTSNo.Value, oReport)



                Dim tblinvoiceNo As New DataTable
                tblinvoiceNo = GetTicketInvoiceNo(objTicketingSeat.SeatNo.ToString())
                Dim row As DataRow = tblinvoiceNo.Rows(0)



                Dim bookkaruPRN = row("Invoice_Id").ToString()

                cReportUtility.PassParameter("InvoiceNo", bookkaruPRN, oReport)



                If String.IsNullOrEmpty(DiscountFare.Text) Then

                    Dim pi As Double
                    pi = 0
                    cReportUtility.PassParameter("DiscountAmount", pi, oReport)

                Else

                    If (DiscountFare.Text = 0) Then
                        Dim pi As Double
                        pi = DiscountFare.Text
                        cReportUtility.PassParameter("DiscountAmount", pi, oReport)

                    Else
                        Dim pi As Double
                        pi = .Fare - DiscountFare.Text
                        cReportUtility.PassParameter("DiscountAmount", pi, oReport)
                    End If

                End If







                cReportUtility.PassParameter("Dep_Time", txtDepartureTime.Text, oReport)


                If Not Request.QueryString("DestinationId") Is Nothing Then
                    cReportUtility.PassParameter("Title", hidReoutName.Value & " ND " & Request.QueryString("TicketNumber"), oReport)
                ElseIf Not Request.QueryString("TicketNumber") Is Nothing Then
                    cReportUtility.PassParameter("Title", hidReoutName.Value & " TC " & Request.QueryString("TicketNumber"), oReport)
                Else
                    cReportUtility.PassParameter("Title", hidReoutName.Value, oReport)
                End If

                cReportUtility.PassParameter("Refund", hidTicketRefund.Value, oReport)
                cReportUtility.PassParameter("Change", hidTicketChange.Value, oReport)

                If chkOnline.Checked = True Then
                    Status = "ON"
                Else
                    Status = "OFF"
                End If
                cReportUtility.PassParameter("Status", Status, oReport)

                cReportUtility.PassParameter("TerminalID", objUser.TerminalId, oReport)

                cReportUtility.PassParameter("PrintDate", Now.Date.Day & "/" & Format(Now.Date.Month, "00") & "/" & Now.Date.Year, oReport)

                If strRePrint <> "" Then
                    cReportUtility.PassParameter("RePrint", strRePrint, oReport)
                Else
                    cReportUtility.PassParameter("RePrint", "", oReport)
                End If

                If cmbDropAt.Items.Count > 0 Then
                    cReportUtility.PassParameter("DropAt", "" & cmbDropAt.SelectedItem.Text, oReport)
                Else
                    cReportUtility.PassParameter("DropAt", "" & "", oReport)

                End If

                cReportUtility.PassParameter("TerminalName", objUser.TerminalName, oReport)

            End With

            Dim i As Integer

            Dim doctoprint As New System.Drawing.Printing.PrintDocument()


            'doctoprint.PrinterSettings.PrinterName = "EPSONLQ3002" 'System.Configuration.ConfigurationManager.AppSettings.Item("PrinterName") '"\\thokar\EpsonLQ-" '(ex. "Epson SQ-1170 ESC/P 2")

            Dim rawKind As Integer
            Dim LaserPrinter As String = System.Configuration.ConfigurationManager.AppSettings.Item("LaserPrinter")



            If LaserPrinter = "No" Then


                doctoprint.PrinterSettings.PrinterName = System.Configuration.ConfigurationManager.AppSettings.Item("PrinterBusiness")

                For i = 0 To doctoprint.PrinterSettings.PaperSizes.Count - 1
                    If doctoprint.PrinterSettings.PaperSizes(i).PaperName = "fm" Then
                        'rawKind = CInt(doctoprint.PrinterSettings.PaperSizes(i).GetType().GetField("kind", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic).GetValue(doctoprint.PrinterSettings.PaperSizes(i)))
                        'oReport.PrintOptions.PaperSize = rawKind
                        Exit For
                    End If
                Next

                If rawKind = 0 Then
                    rawKind = 119
                    oReport.PrintOptions.PaperSize = rawKind
                Else
                    oReport.PrintOptions.PaperSize = rawKind
                End If

            End If

            If PrintWithoutPrinter = "No" Then


                If rdoprints.SelectedValue = "Business" Then
                    Dim bprinter = System.Configuration.ConfigurationManager.AppSettings.Item("PrinterBusiness")

                    Dim getprinterName As PrinterSettings = New PrinterSettings()
                    oReport.PrintOptions.PrinterName = bprinter

                    '  rptDoc.PrintOptions.PrinterName = getprinterName.PrinterName

                    oReport.PrintToPrinter(1, True, 1, 1)
                    oReport.Close()
                    oReport.Dispose()

                Else
                    oReport.PrintOptions.PrinterName = System.Configuration.ConfigurationManager.AppSettings.Item("PrinterName")
                    oReport.PrintToPrinter(1, False, 0, 0)
                    If LaserPrinter = "No" Then

                        If PrintReportBoarding(seatnos, fares) Then

                            oReport.Close()
                            oReport.Dispose()

                        Else
                            oReport.Close()
                            oReport.Dispose()

                        End If

                    End If
                End If


                oReport.Close()
                oReport.Dispose()
                GC.Collect()

                Return True

            End If



        Catch ex As Exception

            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & trace.ToString

            Return False

        End Try



    End Function

    Private Function GetTicketingOthercharges() As DataTable
        Dim objDbManager As IDBManager
        Dim objDataSet As DataSet
        objDbManager = DBManager.GetDatabaseManager()
        objDbManager.SetDBConnection(objConnection)
        Dim objDBParameters As New clsDBParameters
        'Session("TicketingScheduleId") = TicketingScheduleId

        objDBParameters.Parameters.Add(New clsDBParameter("@CompanyId", objUser.Vendor_Id, "bigint"))
        objDBParameters.Parameters.Add(New clsDBParameter("@ServiceId", hndServiceType.Value, "bigint"))
        objDataSet = objDbManager.GetData("GetOtherServiceChargesList", objDBParameters)
        If Not objDataSet Is Nothing Then
            Return objDataSet.Tables(0)
        Else
            Return Nothing
        End If
    End Function

    Private Function GetTicketInvoiceNo(ByVal seatNo) As DataTable
        Dim objDbManager As IDBManager
        Dim objDataSet As DataSet
        objDbManager = DBManager.GetDatabaseManager()
        objDbManager.SetDBConnection(objConnection)
        Dim objDBParameters As New clsDBParameters
        Session("TicketingScheduleId") = TicketingScheduleId

        objDBParameters.Parameters.Add(New clsDBParameter("@Ticketing_Schedule_ID", TicketingScheduleId, "bigint"))
        objDBParameters.Parameters.Add(New clsDBParameter("@SeatNo", seatNo, "bigint"))
        objDataSet = objDbManager.GetData("GetSeatInvoiceNo", objDBParameters)
        If Not objDataSet Is Nothing Then
            Return objDataSet.Tables(0)
        Else
            Return Nothing
        End If
    End Function

    Private Sub GetQR(ByVal value As String)
        Try
            'Dim  As String = Request("txtPassengerName")

            If value Is Nothing OrElse value = "" Then
                'ModelState.AddModelError("InvalidInput", "Input field is required.")
            Else
                Dim encoder As QRCodeEncoder = New QRCodeEncoder()
                Dim bi As Bitmap = encoder.Encode(value)
                bi.Save(Server.MapPath("~/images/qrcode.png"))
                Dim path = Server.MapPath("~/images/qrcode.png")
                Dim byteData As Byte() = System.IO.File.ReadAllBytes(path)
                'Image1.ImageUrl = byteData
                'rdoc As ReportDocument
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Function GetDefaultPrinter() As String

        Dim settings As New PrinterSettings()
        For Each printer As String In PrinterSettings.InstalledPrinters
            settings.PrinterName = printer



            If settings.IsValid Then
                Response.Write(settings.PrinterName)
                Response.Write("<br/>")

                'Return printer
            End If
        Next
        Return String.Empty
    End Function

    Public Function CreateReport() As ReportDocument
        Dim rptsrc As New ReportDocument
        rptsrc.Load(Request.PhysicalApplicationPath & "Reports/rptTest.rpt")
        'cReportUtility.setConnectionInfo(rptsrc)

        Return rptsrc
    End Function

    Private Sub btnSaveDep_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveDep.Click

        objTicketingSeat.TicketingScheduleID = cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text
        objTicketingSeat.Actual_Departure_Time = txtActualDepartureTime.Text
        objTicketingSeat.Hostess_Name = txtHostessName.Text
        objTicketingSeat.Vehicle_ID = cmbVehicle.SelectedItem.Value
        objTicketingSeat.Driver_Name = txtDriverName.Text
        objTicketingSeat.Guard = txtGuard.Text
        objTicketingSeat.ManualVehicle = txtVehicle.Text


        'If hndVechileNo.Value = cmbVehicle.SelectedItem.Value Then
        '    hndVechileNo.Value = cmbVehicle.SelectedItem.Value
        'Else


        'End If


        objTicketingSeat.UpdateActualTime()

        hndVechileNo.Value = cmbVehicle.SelectedItem.Value

        Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)


        objTicketing.Id = hidTSID.Value()

        Dim dt_GetScheduleData As DataTable = objTicketing.GetVoucherTotal(cmbCompany.SelectedValue)

        txtComPer.Text = dt_GetScheduleData.Rows(0)("Commission_Rate").ToString()
        txtcommission.Text = dt_GetScheduleData.Rows(0)("commision").ToString()
        hidTotal.Value = dt_GetScheduleData.Rows(0)("total").ToString()


    End Sub

    Private Function Validateonline() As Boolean
        'Try
        '    Dim ping As New Ping
        '    Dim pingreply As PingReply = ping.Send("58.27.235.174")

        '    If CInt(pingreply.RoundtripTime) > 0 Then
        '        chkOnline.Checked = True
        '        lnkMapping.Visible = True
        '        Return True
        '    Else
        '        chkOnline.Checked = False
        '        lnkMapping.Visible = False
        '        lblErr.Text = "Your server is offline please conctact admin"
        '        Return False
        '    End If

        'Catch ex As Exception
        '    lblErr.Text = "Your server is offline please conctact admin"
        '    chkOnline.Checked = False
        '    lnkMapping.Visible = False
        '    Return False
        'End Try
        Return True

    End Function

    Private Sub chkOnline_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkOnline.CheckedChanged
        'Try
        '    Dim ping As New Ping
        '    Dim pingreply As PingReply = ping.Send("58.27.235.174")

        '    If CInt(pingreply.RoundtripTime) > 0 Then
        '        chkOnline.Checked = True
        '        lnkMapping.Visible = True
        '    Else
        '        chkOnline.Checked = False
        '        lnkMapping.Visible = False
        '        lblErr.Text = "Your server is offline please conctact admin"
        '    End If

        'Catch ex As Exception
        '    lblErr.Text = "Your server is offline please conctact admin"
        '    chkOnline.Checked = False
        '    lnkMapping.Visible = False
        'End Try

        'Call PopulateTicketList(cboVoucherNo.Rows(0).Cells.FromKey("Seats").Value)
    End Sub

    'Private Sub cboTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboTime.TextChanged

    '    'Response.Write(cboTime.SelectedItem.Text & cboTime.SelectedItem.Value)
    '    UpdateV()
    '    'nomi
    '    Dim strTime As String = cboTime.SelectedValue
    '    Dim arrTime As Array

    '    arrTime = strTime.Split("~")

    '    Response.Redirect("Ticketing.aspx?mode=1&TSID=" & arrTime(0) & "")
    '    'Server.Transfer("Ticketing.aspx?mode=1&TSID=" & arrTime(0) & "")

    'End Sub

    Private Sub btnValidateBooking_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnValidateBooking.Click
        ' Validate Here 
        Dim Rtn As String = ""

        Rtn = objTicketingSeat.ValidateBooking(TicketingScheduleId, txtContactNo.Text)
        If (Rtn = "") Then
            tdPassenger.InnerHtml = "No Seat reserved for this phone no."
        Else
            tdPassenger.InnerHtml = "Seat No " & Rtn & " have been reserved from this user."
        End If


    End Sub

    Protected Sub warpTicketing_Load(ByVal sender As Object, ByVal e As EventArgs) Handles warpTicketing.Load

    End Sub

    Protected Sub warpTicketing_ContentRefresh(ByVal sender As Object, ByVal e As EventArgs) Handles warpTicketing.ContentRefresh
        '    ProgressBarwapper.Visible = True

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "disable();", True)

    End Sub

    Protected Sub btnTransit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTransit.Click

        getCCP_No()


    End Sub

    Private Sub loadTable()
        Try


            'Dim count = 0
            'Dim dtVoucher As New DataTable
            'Dim dtVouchertemp As New DataTable

            'dtVouchertemp = objTicketing.GetTicketingSchedule2(cboRoute.SelectedValue, txtDepartureDate.Text)

            'cboTime.Items.Clear()

            'cboTime.DataSource = dtVouchertemp

            'cboTime.DataValueField = "Ticketing_Schedule_ID"
            'cboTime.DataTextField = "Departure_Time"
            'cboTime.DataBind()

            'cboTime.Items.Insert(0, New ListItem("Select", "0"))

        Catch ex As Exception
            Response.Write(ex.Message)

        End Try


    End Sub


    Private Function ValidateStatus(ByVal Seat_No As Integer) As Boolean


        objTicketingSeat.TicketingScheduleID = cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text

        If objTicketingSeat.GetValidationForCancel(objTicketingSeat.TicketingScheduleID, Seat_No) = 0 Then

            Return True
        End If
        Return False


    End Function


    Private Function ValidateMissed() As Boolean


        objTicketingSeat.TicketingScheduleID = cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text

        If objTicketingSeat.GetValidationForMiss(objTicketingSeat.TicketingScheduleID) <= 30 Then

            Return True
        End If
        Return False


    End Function

    Protected Sub btnMissed_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMissed.Click

        Try

            If ValidateMissed() Then

                objTicketingSeat.TicketingScheduleID = cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text
                objTicketingSeat.TicketSrNo = ""
                objTicketingSeat.SeatNo = txtSeatNo.Text
                objTicketingSeat.Status = eTicketStatus.Available
                objTicketingSeat.IssueDate = Now
                objTicketingSeat.IssueTerminalID = objUser.TerminalId
                objTicketingSeat.IssuedBy = objUser.Id
                objTicketingSeat.SourceCity = 0
                objTicketingSeat.DestinationCity = 0
                objTicketingSeat.PassengerName = ""
                objTicketingSeat.PassengerContact = ""
                objTicketingSeat.Missed = 1
                objTicketingSeat.Fare = 0

                objTicketingSeat.Save(False)

                If chkOnline.Checked Then
                    '   Me.lnkMapping_Click(Nothing, Nothing)
                    objOnlineTicketing = New eTicketing
                    Dim ScheduleID As Integer = cboVoucherNo_1.Rows(0).Cells.FromKey("Schedule_Id").Text

                    objOnlineTicketing.MakeAvailable_New(hndOnlineTSNo.Value, txtSeatNo.Text, eTicketStatus.Available, objUser.Id, hidTerminal.Value, 1)

                    Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)

                Else
                    Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)
                End If

                Call clearValues()
                'btnCancelTicket.Visible = False
                btnReprint.Visible = False
                btnMissed.Visible = False
                cmbDestination.SelectedValue = "Please select"
                txtFare.Text = ""
                txtSeatNo.Text = ""
                clearValues()

                btnSkip.Visible = False
                btnSave.Style.Add("Display", "")
                tblTickets.Disabled = False
            Else

                lblErr.Text = "Missed passenger can not done before 30 mints. as per company policy."
                Call clearValues()
                'btnCancelTicket.Visible = False
                btnReprint.Visible = False
                btnMissed.Visible = False
                cmbDestination.SelectedValue = "Please select"
                txtFare.Text = ""
                txtSeatNo.Text = ""
                clearValues()

                btnSkip.Visible = False
                btnSave.Style.Add("Display", "")
                tblTickets.Disabled = False

            End If

        Catch ex As Exception
            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & line
        End Try

    End Sub

    Protected Sub btnReprint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReprint.Click

        Try

            txtPassengerName.Enabled = True
            txtContactNo.Enabled = True
            If String.IsNullOrEmpty(txtCNIC2.Text) Then
                lblDouple.ForeColor = Color.Red
                lblDouple.Text = "CNIC No is Required for Bookkaru Ticket print"

            Else

                If String.IsNullOrEmpty(txtPNR2.Text) Then
                    lblDouple.Text = " "
                    lblDouple.ForeColor = Color.Red
                    lblDouple.Text = "Please Enter The PNR # as Per bookkaru PNR."
                Else
                    'Here i am brother Noman Ali 04/01/2024

                    Dim dtbook As New DataTable
                    dtbook = GetBookkaruBordingTerminal(hndOnlineTSNo.Value, txtSeatNo.Text)
                    Dim rowws As DataRow = dtbook.Rows(0)

                    Dim OnlinePNR As String = "" & rowws("Invoice_Id").ToString()

                    If OnlinePNR <> txtPNR2.Text Then

                        lblDouple.Text = "PNR No is Invalid Please Enter The PNR # as Per bookkaru PNR."

                    Else

                        lblDouple.Text = " "

                        objTicketingSeat.TicketingScheduleID = cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text

                        Dim UserID = objUser.Id
                        objTicketingSeat.SeatNo = txtSeatNo.Text
                        Dim Is_OnlinePrinted As String = objTicketingSeat.GetInvoiceIdPrintDetail

                        If (Is_OnlinePrinted = "False") Then

                            lblDouple.Text = " "

                            If chkOnline.Checked = True Then

                                Dim dtbookarubordingpoint As New DataTable
                                dtbookarubordingpoint = GetBookkaruBordingTerminal(hndOnlineTSNo.Value, txtSeatNo.Text)
                                If dtbookarubordingpoint.Rows.Count = 0 Then
                                    lblDouple.ForeColor = Color.Red
                                    lblDouple.Text = "Please Try Again..."

                                Else
                                    Dim roww As DataRow = dtbookarubordingpoint.Rows(0)
                                    Dim BordingTerminal As String = "" & roww("Bording_Point").ToString()

                                    If BordingTerminal = objUser.TerminalId Then
                                        objTicketingSeat.Reprint(UserID)
                                        objTicketingSeat.SeatNo = txtSeatNo.Text

                                        '  Dim InvoiceNo As String = objTicketingSeat.GetInvoiceId

                                        Dim dtbookaruDetails As New DataTable
                                        dtbookaruDetails = GetBookkaruPNRData()
                                        Dim row As DataRow = dtbookaruDetails.Rows(0)


                                        Dim dtGetBoardingMessage As New DataTable
                                        dtGetBoardingMessage = GetBookkaruMessageData()
                                        Dim row3 As DataRow = dtGetBoardingMessage.Rows(0)

                                        Dim InvoiceNos As String = "" & row("Invoice_Id").ToString()
                                        Dim UserName As String = "" & row("UserId").ToString()
                                        Dim TerminalName As String = "" & row("TerminalName").ToString()
                                        Dim BOOKKARUCNIC As String = "" & row("CNIC").ToString()
                                        Dim PrintDate As String = "" & row("PrintDate").ToString()



                                        If Is_OnlinePrinted <> "" Then BookKaroAPIPrintUpdate(InvoiceNos, UserName, TerminalName, PrintDate)

                                        'If (BOOKKARUCNIC = txtCNIC2.Text) Then

                                        '    PrintReport(txtSeatNo.Text, 0)

                                        'Else
                                        '    lblDouple.Text = "Invalid CNIC number. Please enter a valid BOOKKARU CNIC."

                                        'End If

                                        lblDouple.Text = row3("BookkaruAlert_Message").ToString()
                                        lblDouple.ForeColor = Color.Blue
                                        PrintReport(txtSeatNo.Text, 0)
                                    Else

                                        'Checking Bookkaru Boarding PointPrinting Allow or not.

                                        Dim dtPrintingPoint As New DataTable
                                        dtPrintingPoint = CheckBordingTerminal(objUser.TerminalId)
                                        Dim ro As DataRow = dtPrintingPoint.Rows(0)
                                        Dim PrintAllow As String = "" & ro("AllowBoardingPoints").ToString()

                                        If (PrintAllow = "False") Then



                                            Dim dtgetTerminal As New DataTable
                                            dtgetTerminal = GetTerminalData(BordingTerminal)
                                            Dim row As DataRow = dtgetTerminal.Rows(0)


                                            Dim dtGetBoardingMessage As New DataTable
                                            dtGetBoardingMessage = GetBookkaruMessageData()
                                            Dim row2 As DataRow = dtGetBoardingMessage.Rows(0)


                                            Dim TerminalName As String = "" & row("Terminal_Name").ToString()
                                            lblDouple.ForeColor = Color.Red
                                            lblDouple.Text = objUser.TerminalName + " " + row2("Boarding_Point_Message").ToString()
                                            lblboardingpoint.Text = TerminalName
                                            lblurdu.Text = "ہے" + TerminalName + " " + row2("Boarding_Point_MessageUrdu").ToString()
                                            ' lblurdu.Text = "ہے" + TerminalName + " " + "نمبر کے مطابق آپ  کا بورڈنگ پوائنٹ یہ نہیں ہے آپ  کا بورڈنگ پوائنٹ " + "PNR  ے " + "Bookkaru"
                                            lblurdu.ForeColor = Color.Red
                                        Else
                                            objTicketingSeat.Reprint(UserID)
                                            objTicketingSeat.SeatNo = txtSeatNo.Text
                                            PrintReport(txtSeatNo.Text, 0)



                                        End If



                                    End If
                                End If




                            Else
                                objTicketingSeat.Reprint(UserID)
                                objTicketingSeat.SeatNo = txtSeatNo.Text

                                '  Dim InvoiceNo As String = objTicketingSeat.GetInvoiceId

                                Dim dtbookaruDetails As New DataTable
                                dtbookaruDetails = GetBookkaruPNRData()

                                Dim row As DataRow = dtbookaruDetails.Rows(0)


                                Dim dtGetBoardingMessage As New DataTable
                                dtGetBoardingMessage = GetBookkaruMessageData()
                                Dim row3 As DataRow = dtGetBoardingMessage.Rows(0)

                                Dim InvoiceNos As String = "" & row("Invoice_Id").ToString()
                                Dim UserName As String = "" & row("UserId").ToString()
                                Dim TerminalName As String = "" & row("TerminalName").ToString()
                                Dim BOOKKARUCNIC As String = "" & row("CNIC").ToString()
                                Dim PrintDate As String = "" & row("PrintDate").ToString()



                                If Is_OnlinePrinted <> "" Then BookKaroAPIPrintUpdate(InvoiceNos, UserName, TerminalName, PrintDate)

                                'If (BOOKKARUCNIC = txtCNIC2.Text) Then

                                '    PrintReport(txtSeatNo.Text, 0)

                                'Else
                                '    lblDouple.Text = "Invalid CNIC number. Please enter a valid BOOKKARU CNIC."

                                'End If

                                lblDouple.Text = row3("BookkaruAlert_Message").ToString()
                                lblDouple.ForeColor = Color.Blue
                                PrintReport(txtSeatNo.Text, 0)
                            End If




                        Else
                            lblDouple.ForeColor = Color.Red
                            lblDouple.Text = "Bookkaru Ticket No " + txtSeatNo.Text + " already printed"
                        End If
                    End If


                End If





            End If
        Catch ex As Exception
            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & line

        End Try





    End Sub

    Private Function CheckBordingTerminal(terminalId As Integer) As DataTable
        Dim objDbManager As IDBManager
        Dim objDataSet As DataSet
        Dim objeConnection As SqlConnection
        objeConnection = eConnectionManager.GetConnection()
        objDbManager = DBManager.GetDatabaseManager()
        objDbManager.SetDBConnection(objeConnection)
        Dim objDBParameters As New clsDBParameters

        objDBParameters.Parameters.Add(New clsDBParameter("@Terminal_Id", terminalId, "bigint"))




        objDataSet = objDbManager.GetData("CheckingBoardingPointAllow", objDBParameters)
        If Not objDataSet Is Nothing Then
            Return objDataSet.Tables(0)
        Else
            Return Nothing
        End If
    End Function

    Private Function GetBookkaruMessageData() As DataTable
        Dim objDbManager As IDBManager
        Dim objDataSet As DataSet
        objDbManager = DBManager.GetDatabaseManager()
        objDbManager.SetDBConnection(objConnection)
        Dim objDBParameters As New clsDBParameters



        objDataSet = objDbManager.GetData("sp_getbokkkaroAlertMessage", objDBParameters)
        If Not objDataSet Is Nothing Then
            Return objDataSet.Tables(0)
        Else
            Return Nothing
        End If
    End Function

    Private Function GetTerminalData(ByVal TerminalId) As DataTable
        Dim objDbManager As IDBManager
        Dim objDataSet As DataSet
        objDbManager = DBManager.GetDatabaseManager()
        objDbManager.SetDBConnection(objConnection)
        Dim objDBParameters As New clsDBParameters


        objDBParameters.Parameters.Add(New clsDBParameter("@Terminal_Id", TerminalId, "int"))

        objDataSet = objDbManager.GetData("Sp_GetTerminalforbookkaruBordingpoint", objDBParameters)
        If Not objDataSet Is Nothing Then
            Return objDataSet.Tables(0)
        Else
            Return Nothing
        End If

    End Function

    Private Function GetBookkaruBordingTerminal(ByVal ScheduleId, ByVal SeatNo) As DataTable
        Dim objDbManager As IDBManager
        Dim objDataSet As DataSet
        Dim objeConnection As SqlConnection
        objeConnection = eConnectionManager.GetConnection()
        objDbManager = DBManager.GetDatabaseManager()
        objDbManager.SetDBConnection(objeConnection)
        Dim objDBParameters As New clsDBParameters

        objDBParameters.Parameters.Add(New clsDBParameter("@Ticketing_Schedule_ID", ScheduleId, "bigint"))
        objDBParameters.Parameters.Add(New clsDBParameter("@SeatNo", SeatNo, "int"))



        objDataSet = objDbManager.GetData("BookkaruBordingPointStatus", objDBParameters)
        If Not objDataSet Is Nothing Then
            Return objDataSet.Tables(0)
        Else
            Return Nothing
        End If


    End Function

    Private Function GetBookkaruPNRData() As DataTable



        Dim objDbManager As IDBManager
        Dim objDataSet As DataSet
        objDbManager = DBManager.GetDatabaseManager()
        objDbManager.SetDBConnection(objConnection)
        Dim objDBParameters As New clsDBParameters

        objDBParameters.Parameters.Add(New clsDBParameter("@Ticketing_Schedule_ID", TicketingScheduleId, "bigint"))
        objDBParameters.Parameters.Add(New clsDBParameter("@seat_No", objTicketingSeat.SeatNo, "int"))



        objDataSet = objDbManager.GetData("sp_getInvoiceNo", objDBParameters)
        If Not objDataSet Is Nothing Then
            Return objDataSet.Tables(0)
        Else
            Return Nothing
        End If



    End Function

    Protected Sub Timer1_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles Timer1.Tick
        Try

            Label2912555.Visible = False
            txtCustomerNumber.Visible = False
            btnValidateCustomers.Visible = False

            lblboardingpoint.Text = " "
            'lblDouple.Text = " "
            lblurdu.Text = " "
            lblurdu.Text = " "
            If ServerPing() Then



                Dim dtOnlineTickets As DataTable
                Dim dtLocalTickets As DataTable
                objOnlineTicketing = New eTicketing
                Dim DS As New DataSet


                objTicketingSeatList = New clsSeatTicketingList(objConnection)

                DS = objTicketingSeatList.ClearGray(TicketingScheduleId)


                If Not DS Is Nothing Then
                    If DS.Tables(0).Rows(0)(0).ToString() <> "0" Then
                        ClearGrayOnline(hndOnlineTSNo.Value)
                        lnkMapping_Click(sender, e)
                    End If
                End If

                DS = objTicketingSeatList.ClearGrayBookKaru(TicketingScheduleId)


                If Not DS Is Nothing Then

                    For Each dr As DataRow In DS.Tables(0).Rows
                        objOnlineTicketing.MakeAvailable_New(hndOnlineTSNo.Value, dr("Seat_No"), eTicketStatus.Available, dr("Issued_By"), dr("Issue_Terminal"))
                    Next

                End If



            End If

            Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)

        Catch ex As Exception
            Dim trace = New Diagnostics.StackTrace(ex, True)
            Dim line As String = Right(trace.ToString, 5)

            lblErr.Text = "'" & ex.Message & "'" & " Error in- Line number: " & line
        End Try
    End Sub

    Private Sub ClearGrayOnline(ByVal hndOnlineTSNo As Integer)
        Try


            objeConnection = eConnectionManager.GetConnection()
            Dim objTicketingSeat As New clsSeatTicketing(objeConnection)
            Dim Updated As Boolean
            Updated = objTicketingSeat.UpdateHoldSeats(hndOnlineTSNo)
            eConnectionManager.CloseConnection(objeConnection)
            '  Return Updated

        Catch ex As Exception
            eConnectionManager.CloseConnection(objeConnection)
        End Try
        '  Return False

    End Sub

    Protected Sub btnValidateCustomers_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnValidateCustomers.Click

        Try

            If hidCustomerPIN.Value <> "" Then Exit Sub

            Dim dtReturn As New DataTable
            Dim dtReturnOnline As New DataTable
            objOnlineTicketing = New eTicketing

            Dim objCustomers As New clsCustoemr(objConnection)

            objTicketingSeat.CustomerCode = txtCustomerNumber.Text
            dtReturn = objTicketingSeat.ValidateCustomer()
            Dim sms_PNR As String = ""



            If Not dtReturn Is Nothing Then
                If dtReturn.Rows.Count > 0 Then

                    hndCustID.Value = dtReturn.Rows(0)("Customer_Id")
                    txtPassengerName.Text = dtReturn.Rows(0)("First_Name") & " " & dtReturn.Rows(0)("Last_Name")
                    txtCNIC2.Text = dtReturn.Rows(0)("CNIC")
                    txtContactNo.Text = dtReturn.Rows(0)("MobileNo")

                Else
                    If ServerPing() Then
                        dtReturnOnline = objOnlineTicketing.ValidateCustomer(txtCustomerNumber.Text)
                        If Not dtReturnOnline Is Nothing Then
                            If dtReturnOnline.Rows.Count > 0 Then
                                hndCustID.Value = dtReturnOnline.Rows(0)("Customer_Id")
                                txtPassengerName.Text = dtReturnOnline.Rows(0)("First_Name") & " " & dtReturnOnline.Rows(0)("Last_Name")
                                txtCNIC2.Text = dtReturnOnline.Rows(0)("CNIC")
                                txtContactNo.Text = dtReturnOnline.Rows(0)("MobileNo")
                                objCustomers.ImportAllCustomers(dtReturnOnline)

                            Else
                                If txtCustomerNumber.Text.Trim() <> "" Then
                                    lblErr.Text = "Customer Not Found !"
                                End If

                                hndCustID.Value = 0
                                txtPassengerName.Text = ""
                                txtCNIC2.Text = ""
                                txtContactNo.Text = ""

                            End If
                        Else
                            If txtCustomerNumber.Text.Trim() <> "" Then
                                lblErr.Text = "Customer Not Found !"
                            End If
                            hndCustID.Value = 0
                            txtPassengerName.Text = ""
                            txtCNIC2.Text = ""
                            txtContactNo.Text = ""
                        End If


                    Else
                        If txtCustomerNumber.Text.Trim() <> "" Then
                            lblErr.Text = "Customer Not Found !"
                        End If

                        hndCustID.Value = 0
                        txtPassengerName.Text = ""
                        txtCNIC2.Text = ""
                        txtContactNo.Text = ""

                    End If

                End If
            Else
                If txtCustomerNumber.Text.Trim() <> "" Then
                    lblErr.Text = "Customer Not Found !"
                End If

                hndCustID.Value = 0

                txtPassengerName.Text = ""
                txtCNIC2.Text = ""
                txtContactNo.Text = ""

            End If
            ValidateCustomerPIN.Visible = False
            If ServerPing() Then

                dtReturnOnline = objOnlineTicketing.GetCustomerLoalityCheck(txtCustomerNumber.Text)
                If CBool(dtReturnOnline.Rows(0)(0)) = True Then
                    'CustomerPIN = DateTime.Now.Date.Minute.ToString("00") & DateTime.Now.Date.Hour.ToString("00") & RandomString(4, False)
                    hidCustomerPIN.Value = GetRandom(100, 200).ToString() & RandomString(4, False)

                    sendSMS_CustomerPin(txtContactNo.Text, hidCustomerPIN.Value)
                    ValidateCustomerPIN.Visible = True

                End If


            End If


            Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)


        Catch ex As Exception
            lblErr.Text = ex.Message

        End Try

    End Sub

    Public Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
        Dim Generator As System.Random = New System.Random()
        Return Generator.Next(Min, Max)
    End Function


    Protected Sub btnSaveVehicle_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveVehicle.Click

        objTicketingSeat.TicketingScheduleID = cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text
        objTicketingSeat.Actual_Departure_Time = txtActualDepartureTime.Text
        objTicketingSeat.Hostess_Name = txtHostessName.Text
        objTicketingSeat.Vehicle_ID = cmbVehicle.SelectedItem.Value
        objTicketingSeat.Driver_Name = txtDriverName.Text
        objTicketingSeat.Guard = txtGuard.Text
        objTicketingSeat.CCNumber = txtCCNumber.Text
        objTicketingSeat.Ve_Vendor_Id = cmbCompany.SelectedValue



        'If hndVechileNo.Value = cmbVehicle.SelectedItem.Value Then
        '    hndVechileNo.Value = cmbVehicle.SelectedItem.Value
        'Else


        'End If


        objTicketingSeat.UpdateActualTime()

        hndVechileNo.Value = cmbVehicle.SelectedItem.Value

        Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)


        objTicketing.Id = hidTSID.Value()

        Dim dt_GetScheduleData As DataTable = objTicketing.GetVoucherTotal(cmbCompany.SelectedValue)

        txtComPer.Text = dt_GetScheduleData.Rows(0)("Commission_Rate").ToString()
        txtcommission.Text = dt_GetScheduleData.Rows(0)("commision").ToString()
        hidTotal.Value = dt_GetScheduleData.Rows(0)("total").ToString()
        btnSaveVehicle.Visible = False

    End Sub

    Protected Sub lkWatchList_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lkWatchList.Click
        Try

            'Dim dtReturn As New DataTable
            'Dim dtReturnOnline As New DataTable
            'objOnlineTicketing = New eTicketing

            'Dim objCustomers As New clsCustoemr(objConnection)

            'objTicketingSeat.CustomerCode = txtCustomerNumber.Text
            'dtReturn = objTicketingSeat.ValidateCustomer()

            'If Not dtReturn Is Nothing Then
            '    If dtReturn.Rows.Count > 0 Then

            '        hndCustID.Value = dtReturn.Rows(0)("Customer_Id")
            '        txtPassengerName.Text = dtReturn.Rows(0)("First_Name") & " " & dtReturn.Rows(0)("Last_Name")
            '        txtCNIC2.Text = dtReturn.Rows(0)("CNIC")
            '        txtContactNo.Text = dtReturn.Rows(0)("MobileNo")

            '    Else
            '        If ServerPing() Then
            '            dtReturnOnline = objOnlineTicketing.ValidateCustomer(txtCustomerNumber.Text)
            '            If Not dtReturnOnline Is Nothing Then
            '                If dtReturnOnline.Rows.Count > 0 Then
            '                    hndCustID.Value = dtReturnOnline.Rows(0)("Customer_Id")
            '                    txtPassengerName.Text = dtReturnOnline.Rows(0)("First_Name") & " " & dtReturnOnline.Rows(0)("Last_Name")
            '                    txtCNIC2.Text = dtReturnOnline.Rows(0)("CNIC")
            '                    txtContactNo.Text = dtReturnOnline.Rows(0)("MobileNo")
            '                    objCustomers.ImportAllCustomers(dtReturnOnline)

            '                Else
            '                    lblErr.Text = "Customer Not Found !"
            '                    hndCustID.Value = 0
            '                    txtPassengerName.Text = ""
            '                    txtCNIC2.Text = ""
            '                    txtContactNo.Text = ""

            '                End If
            '            Else
            '                lblErr.Text = "Customer Not Found !"
            '                hndCustID.Value = 0
            '                txtPassengerName.Text = ""
            '                txtCNIC2.Text = ""
            '                txtContactNo.Text = ""
            '            End If


            '        Else
            '            lblErr.Text = "Customer Not Found !"
            '            hndCustID.Value = 0
            '            txtPassengerName.Text = ""
            '            txtCNIC2.Text = ""
            '            txtContactNo.Text = ""

            '        End If

            '    End If
            'Else
            '    lblErr.Text = "Customer Not Found !"
            '    hndCustID.Value = 0
            '    txtPassengerName.Text = ""
            '    txtCNIC2.Text = ""
            '    txtContactNo.Text = ""

            'End If

            'Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)


        Catch ex As Exception
            lblErr.Text = ex.Message

        End Try
    End Sub

    Protected Sub cmbCompany_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbCompany.SelectedIndexChanged

        If cmbCompany.SelectedValue <> 0 Then

            Dim tbVechile As New DataTable

            objVehicle = New clsVehicle(objConnection)
            tbVechile = objVehicle.VehicleByCompany(cmbCompany.SelectedValue)

            cmbVehicle.Items.Clear()
            cmbVehicle.DataSource = tbVechile
            cmbVehicle.DataValueField = "Vehicle_ID"

            cmbVehicle.DataTextField = "Registration_No"


            cmbVehicle.DataBind()

            Try



                objTicketing.Id = hidTSID.Value()
                Dim dt_GetScheduleData As DataTable = objTicketing.GetVoucherTotal(cmbCompany.SelectedValue)

                'Here Nomi
                tblDeductions.Style.Add("Display", "")
                btnCloseVoucher.Style.Add("display", "")
                btnRefresh.Style.Add("display", "")
                btnSave.Style.Add("display", "")

                txtcommission.Text = dt_GetScheduleData.Rows(0)("commision").ToString()
                txtComPer.Text = dt_GetScheduleData.Rows(0)("Commission_Rate").ToString()
                hidTotal.Value = dt_GetScheduleData.Rows(0)("total").ToString()

                txtBKComm.Text = dt_GetScheduleData.Rows(0)("BKCom").ToString()
                txtBKCommPer.Text = dt_GetScheduleData.Rows(0)("BKComPer").ToString()

                txtRefreshment.Focus()

            Catch ex As Exception

            End Try

        End If
        Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)
    End Sub

    'Protected Sub rdoOption_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdoOption.SelectedIndexChanged
    '    If rdoOption.SelectedValue = "1" Then
    '        txtCNIC2.Visible = False
    '        txtContactNo.Visible = True
    '        divCusomerNumber.Visible = False
    '    ElseIf rdoOption.SelectedValue = "2" Then
    '        txtCNIC2.Visible = True
    '        txtContactNo.Visible = False
    '        divCusomerNumber.Visible = False
    '    ElseIf rdoOption.SelectedValue = "3" Then
    '        txtCNIC2.Visible = False
    '        txtContactNo.Visible = False
    '        divCusomerNumber.Visible = True

    '    End If

    '    Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)
    'End Sub

    'Protected Sub cmbOption_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbOption.SelectedIndexChanged

    '    If cmbOption.SelectedValue = "1" Then
    '        txtCNIC2.Visible = False
    '        txtContactNo.Visible = True
    '        divCusomerNumber.Visible = False
    '    ElseIf cmbOption.SelectedValue = "2" Then
    '        txtCNIC2.Visible = True
    '        txtContactNo.Visible = False
    '        divCusomerNumber.Visible = False
    '    ElseIf cmbOption.SelectedValue = "3" Then
    '        txtCNIC2.Visible = False
    '        txtContactNo.Visible = False
    '        divCusomerNumber.Visible = True

    '    End If

    '    Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)

    'End Sub


    Protected Sub btnCloseVoucher0_Click(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Protected Sub btnDropTime_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDropTime.Click

        Try

            Dim UserID As Integer
            UserID = objUser.Id

            'lnkMapping_Click(sender, e)

            If (ServerPing()) Then
                objOnlineTicketing = New eTicketing

                Dim OnlineSchedule_Id As Integer = objOnlineTicketing.DropTime(Val("" & hndOnlineTSNo.Value), Val("" & UserID))
                If Val("" & hndOnlineTSNo.Value) <> 0 Then
                    If OnlineSchedule_Id = 1 Then
                        objConnection = ConnectionManager.GetConnection()
                        Dim objTicketing As New clsTicketing(objConnection)
                        objTicketing.Id = cboVoucherNo_1.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text
                        objTicketing.UserId = objUser.Id
                        objTicketing.DropTime()
                    Else
                        lblErr.Text = "Server is not connected."

                    End If
                Else
                    lblErr.Text = "Server is not connected."

                End If

            End If
            MakeDisableTime()

            DiableTable()

            Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)

            tdOKCancel.Style.Add("Display", "none")
            btnOK.Enabled = True
            'sendSMS()
            BookkaruDropTime()

        Catch ex As Exception

        End Try


    End Sub

    Private Sub BookkaruDropTime()
        Dim hwr As HttpWebRequest

        Dim IPAddress As String
        IPAddress = FMovers.Ticketing.DAL.Crypto.Decrypt(System.Configuration.ConfigurationManager.AppSettings("ServerIPAddress").ToString, "")

        Dim myUri As New Uri("http://" + IPAddress + ":7600/api/DropTimeForBookkaru?Depart_Id=" & hndOnlineTSNo.Value & "")

        'hwr = WebRequest.Create("http://203.130.22.170:7600/api/DropTimeForBookkaru?Depart_Id=" & hndOnlineTSNo.Value & "")
        hwr = WebRequest.Create(myUri)
        hwr.ReadWriteTimeout = 300

        Try
            Dim wr As WebResponse
            wr = hwr.GetResponse()

            If CType(wr, HttpWebResponse).StatusCode = HttpStatusCode.OK Then
                Dim st As Stream
                st = wr.GetResponseStream()
                Dim sr As StreamReader
                sr = New StreamReader(st)
                Dim res As String
                res = sr.ReadToEnd()
                If Not res Is Nothing Then



                    lblErr.Text = "Bookkaru " & res.Replace("message", "")
                End If
            End If
        Catch ex As Exception
            '...handle error...
        End Try
    End Sub

    Protected Sub btnValidatePIN_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnValidatePIN.Click
        lblCustomerApproved.Text = ""
        If hidMode.Value = "1" Then
            btnSave.Text = "Print"
        Else
            btnSave.Text = "Book"
        End If


        If txtCustomerPIN.Text = hidCustomerPIN.Value Then
            lblCustomerApproved.ForeColor = Color.Green
            lblCustomerApproved.Text = "Approved."
            btnSave.Text = "Redeem"
        Else
            lblCustomerApproved.ForeColor = Color.Red
            lblCustomerApproved.Text = "Wrong Customer PIN ."
        End If

        Call PopulateTicketList(cboVoucherNo_1.Rows(0).Cells.FromKey("Seats").Value)

    End Sub
End Class