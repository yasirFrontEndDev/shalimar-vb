Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid
Imports FMovers.Ticketing.Online
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Data.SqlClient


Partial Public Class UploadDataOnServer
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objScheduleList As clsSchedules
    Dim objTicketing As clsTicketing
    Dim objOnlineTicketing As eTicketing

    Private table As DataTable
    Dim dtVehicle As DataTable
    Dim mode As String

#Region " Form Events "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If

        If Page.IsPostBack = False Then

            '  UploadDataToServer()

        End If

        objConnection = ConnectionManager.GetConnection()
        objUser = CType(Session("CurrentUser"), clsUser)
        objScheduleList = New clsSchedules(objConnection)
        objTicketing = New clsTicketing(objConnection)
        dtVehicle = (New clsVehicle(objConnection)).GetAll()
        dtSchedule.CalendarLayout.Culture = clsUtil.GetDateChooserCulture()

        If "" & Request.QueryString("mode") <> "" Then
            mode = Request.QueryString("mode")
        Else
            mode = "1"
        End If

        If mode = "2" Then
            lblHeading.Text = "Booking"
        ElseIf mode = "1" Then
            lblHeading.Text = "Current Ticketing"
        End If

        If Not Me.IsPostBack Then
            Call loadCombos()
            'Call getMaxNumber()
            If cboRoute.Items.Count > 1 Then
                cboRoute.SelectedIndex = 1
                Call cboRoute_SelectedIndexChanged(Nothing, Nothing)
            End If
        End If

        Call loadTable()
        'Call RegisterClientEvents()

        If Not Me.IsPostBack Then
            If cboRoute.Text <> "" Then
                Call BindTicketingRoute()
            End If
        End If

    End Sub


    Private Sub UploadDataToServer()
        Try


            If ServerPing() Then

                Dim ConnString As String
                Dim ConnStringOnline As String
                Dim Connections As SqlClient.SqlConnection
                Dim ConnectionsOnline As SqlClient.SqlConnection



                ConnString = FMovers.Ticketing.DAL.Crypto.Decrypt(System.Configuration.ConfigurationManager.ConnectionStrings("FMoversLocal").ToString(), "")

                Connections = New SqlConnection(ConnString)



                ConnStringOnline = FMovers.Ticketing.DAL.Crypto.Decrypt(System.Configuration.ConfigurationManager.ConnectionStrings("FMoversCentral").ToString(), "")

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



                str_Query = " Select * from System_Info "

                Command_Offline.Connection = Connections

                Command_Offline.CommandType = CommandType.Text
                Command_Offline.CommandText = str_Query
                Adapter_Offline.SelectCommand = Command_Offline
                Adapter_Offline.Fill(dt_OfflineDataTaleSystem)


                If dt_OfflineDataTaleSystem.Rows.Count > 0 Then
                    Access_Terminal_Id = dt_OfflineDataTaleSystem.Rows(0)("Default_Terminal")
                End If



                ' ******************************** Start load which are not loaded before ******************************** 

                'str_Query = " Select top 50 * from Ticketing_Schedule where  " & _
                '            " Voucher_Status = 2 And ISNULL(Is_Pulled, 0) = 0 " & _
                '            " and Ticketing_Schedule_ID Not In ( " & _
                '            " select Ticketing_Schedule_ID from Ticketing_Schedule where Voucher_Closed_Date " & _
                '            " between dateadd(MINUTE, -25, getdate()) and dateadd(MINUTE, 25, getdate()) and" & _
                '            " Voucher_Status = 2" & _
                '            " ) "



                'Command_Offline.Connection = Connections

                'Command_Offline.CommandType = CommandType.Text
                'Command_Offline.CommandText = str_Query
                'Adapter_Offline.SelectCommand = Command_Offline
                'Adapter_Offline.Fill(dt_OfflineDataTale)

                If ServerPing() Then
                    If ConnectionsOnline.State = ConnectionState.Closed Then
                        ConnectionsOnline.Open()
                    End If
                    Command_Online.Connection = ConnectionsOnline
                End If

                For Each Offline_Dr As DataRow In table.Rows

                    Dim rtn_Value As Integer = 0



                    Command_Online.CommandType = CommandType.StoredProcedure

                    Command_Online.CommandText = "sp_GrandVoucherById"

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
                    rtn_Value = Convert.ToInt64(Command_Online.ExecuteScalar())


                    '****************** Get Seats Offline ************************

                    str_Query = " Select * From Ticketing_Seat Where Status = 4 and Ticketing_Schedule_ID = " & Offline_Dr("Ticketing_Schedule_ID") & " and Issue_Terminal =  " & Offline_Dr("Access_Terminal_Id") & " Order By Seat_No "
                    Command_Offline.CommandType = CommandType.Text
                    Command_Offline.CommandText = str_Query
                    dt_OfflineDataTale_DataList.Clear()
                    dt_OfflineDataTale_DataList.Dispose()

                    Adapter_Offline.SelectCommand = Command_Offline
                    Adapter_Offline.Fill(dt_OfflineDataTale_DataList)


                    '****************** Get Seats Online ************************

                    str_Query = " delete from Ticketing_Seat_ALL where  Ticketing_Schedule_ID =  " & Offline_Dr("Ticketing_Schedule_ID") & " and Issue_Terminal =  " & Offline_Dr("Access_Terminal_Id")


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
                        Command_Online.ExecuteNonQuery()

                        Command_Offline.CommandText = " update Ticketing_Schedule set Is_Pulled = 1 where Ticketing_Schedule_ID = " & _
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



                        '@CashPaidToDriver
                        Command_Online.ExecuteNonQuery()

                    End If

                Next




                ' ******************************** End load which are not loaded before ******************************** 

                'InProcessing = False
            End If

            lblHeading.Text = "Data have been posed on server sucessfully."

        Catch ex As Exception
            lblHeading.Text = ex.Message
        End Try

    End Sub
    Private Sub UserLogin_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        ConnectionManager.CloseConnection(objConnection)
    End Sub

#End Region

#Region " Control Events "

    Private Sub cboRoute_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboRoute.SelectedIndexChanged
        If cboRoute.SelectedValue <> "0" Then
            'LoadDataOnline()
            objTicketing.CreateTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, "")
            loadTable()
            Me.BindTicketingRoute()

        End If
    End Sub

    Private Sub LoadDataOnline()
        Try


            


        Catch ex As Exception

        End Try
    End Sub
    Private Sub dtSchedule_ValueChanged(ByVal sender As Object, ByVal e As Infragistics.WebUI.WebSchedule.WebDateChooser.WebDateChooserEventArgs) Handles dtSchedule.ValueChanged
        If cboRoute.SelectedValue <> "0" Then

            objTicketing.CreateTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, "")
            loadTable()

            Me.BindTicketingRoute()
        End If
    End Sub

    Private Sub grdVoucher_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles grdVoucher.InitializeLayout

        Try


            CustomizeControl.SetGridLayout(e.Layout.Grid)
            With e.Layout.Grid

                .Columns.FromKey("Book_Id").Hidden = True
                ' .Columns.FromKey("Book_Id1").Hidden = True
                .Columns.FromKey("Ticketing_Schedule_ID").Hidden = True
                .Columns.FromKey("TS_Date").Hidden = True
                .Columns.FromKey("Schedule_ID").Hidden = True
                .Columns.FromKey("Voucher_Opened_By").Hidden = True
                .Columns.FromKey("Voucher_Closed_By").Hidden = True
                .Columns.FromKey("Voucher_Status").Hidden = True

                .Columns.FromKey("User_ID").Hidden = True
                .Columns.FromKey("Access_DateTime").Hidden = True
                .Columns.FromKey("Access_Sys_Name").Hidden = True
                .Columns.FromKey("Access_Terminal_ID").Hidden = True
                .Columns.FromKey("Bus No").Hidden = True
                '   .Columns.FromKey("Voucher_No").Hidden = True


                .Columns.FromKey("Sr_No").Header.Caption = "Sr #"
                .Columns.FromKey("Departure_Time").Header.Caption = "Dep. Time"
                .Columns.FromKey("Actual_Departure_Time").Header.Caption = "A. Dep. Time"
                .Columns.FromKey("Vehicle_ID").Header.Caption = "Bus #"
                .Columns.FromKey("Driver_Name").Header.Caption = "Driver"
                .Columns.FromKey("Hostess_Name").Header.Caption = "Hostess"
                .Columns.FromKey("Voucher_No").Header.Caption = "Voucher #."
                '.Columns.FromKey("Veh_Code").Header.Caption = "Veh. Code"
                .Columns.FromKey("Voucher_Closed_Date").Header.Caption = "Closed On"
                .Columns.FromKey("Voucher_Status").Header.Caption = "Status"

                .Columns.FromKey("Departure_Time").AllowUpdate = AllowUpdate.No
                '.Columns.FromKey("Veh_Code").AllowUpdate = AllowUpdate.No
                ' .Columns.FromKey("Seats").AllowUpdate = AllowUpdate.No
                .Columns.FromKey("Voucher_No").AllowUpdate = AllowUpdate.No
                .Columns.FromKey("Voucher_Closed_Date").AllowUpdate = AllowUpdate.No
                .Columns.FromKey("Voucher_Status").AllowUpdate = AllowUpdate.No

                .Columns.FromKey("Sr_No").Width = 50
                .Columns.FromKey("Departure_Time").Width = 100
                .Columns.FromKey("Vehicle_ID").Width = 100
                .Columns.FromKey("Voucher_Closed_Date").Width = 150
                .Columns.FromKey("Driver_Name").Width = 100
                .Columns.FromKey("Hostess_Name").Width = 100
                .Columns.FromKey("Voucher_No").Width = 100

                'Dim vList As New ValueList

                'For Each drow As DataRow In dtVehicle.Rows
                '    vList.ValueListItems.Add(drow.Item("Vehicle_ID"), "" & drow.Item("Registration_No"))
                'Next

                '.Columns.FromKey("Vehicle_ID").Type = ColumnType.DropDownList
                '.Columns.FromKey("Vehicle_ID").ValueList = vList

            End With
        Catch ex As Exception
            Response.Write(ex.Message)

        End Try

    End Sub

    Private Sub grdVoucher_InitializeRow(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles grdVoucher.InitializeRow
        Dim TSDate As DateTime
        Dim DTime() As String
        'Dim hrs, mins As Integer
        With e.Row.Cells
            TSDate = .FromKey("TS_Date").Value
            DTime = .FromKey("Departure_Time").Value.ToString().Split(":")
            'hrs = CType(DTime(0), Integer)
            'hrs = hrs Mod 24
            If "" & .FromKey("Voucher_No").Value = "" Then '=================================TODO========================
                '.FromKey("Voucher_No").Value = "<a href='#' onclick='CreateVoucher(" & .FromKey("Ticketing_Schedule_Id").Value & ");'>Create Voucher</a>"
                If mode = "1" Then
                    .FromKey("Voucher_No").Value = "<a href='#' onclick='Ticketing(" & .FromKey("Ticketing_Schedule_Id").Value & ");'>Ticketing</a>"
                ElseIf mode = "2" Then
                    If DTime.Length > 0 Then
                        TSDate = New DateTime(TSDate.Year, TSDate.Month, TSDate.Day, CType(DTime(0), Integer) Mod 24, CType(DTime(1), Integer), 0)

                        Dim str = DateDiff(DateInterval.Day, DateTime.Now, TSDate)

                        If str >= 0 Then
                            '.FromKey("Voucher_No").Value = .FromKey("Voucher_No").Value & " | " & "<a href='#' onclick='TicketBooking(" & .FromKey("Ticketing_Schedule_Id").Value & ");'>Booking</a>"
                            .FromKey("Voucher_No").Value = "<a href='#' onclick='TicketBooking(" & .FromKey("Ticketing_Schedule_Id").Value & ");'>Booking</a>"
                        Else
                            .FromKey("Voucher_No").Value = ""
                        End If
                    End If
                End If
            Else
                If mode = "1" Then
                    .FromKey("Voucher_No").Value = "<a href='#' onclick='Ticketing(" & .FromKey("Ticketing_Schedule_Id").Value & ");'>" & .FromKey("Voucher_No").Value & "</a>"
                ElseIf mode = "2" Then
                    If DTime.Length > 0 Then
                        TSDate = New DateTime(TSDate.Year, TSDate.Month, TSDate.Day, CType(DTime(0), Integer) Mod 24, CType(DTime(1), Integer), 0)
                        If TSDate.ToString("dd/MM/yyyy") >= DateTime.Now.ToString("dd/MM/yyyy") Then
                            .FromKey("Voucher_No").Value = "<a href='#' onclick='TicketBooking(" & .FromKey("Ticketing_Schedule_Id").Value & ");'>" & .FromKey("Voucher_No").Value & "</a>"
                        Else
                            .FromKey("Voucher_No").Value = "<a href='#' onclick='TicketBooking(" & .FromKey("Ticketing_Schedule_Id").Value & ");'>" & .FromKey("Voucher_No").Value & "</a>"
                        End If
                    End If
                End If
            End If

            If "" & .FromKey("Voucher_Status").Value = "2" Then
                .FromKey("Voucher_Status").Value = "Closed"
            Else
                .FromKey("Voucher_Status").Value = "Open"
            End If
        End With
    End Sub

    Private Sub grdVoucher_UpdateGrid(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.UpdateEventArgs) Handles grdVoucher.UpdateGrid
        Dim row As Infragistics.WebUI.UltraWebGrid.UltraGridRow
        Dim pk = table.PrimaryKey(0).ColumnName

        'create and get the rows enumeration for the changed rows
        Dim updatedRows As Infragistics.WebUI.UltraWebGrid.UltraGridRowsEnumerator
        updatedRows = e.Grid.Bands(0).GetBatchUpdates()

        'for each row in the Updated rows check if the current row is an addedrow, if so create the row and add it to the dataset
        While updatedRows.MoveNext
            row = updatedRows.Current

            Select Case row.DataChanged
                Case DataChanged.Added

                    'Dim i As Integer
                    'Dim addedRow As DataRow
                    'addedRow = Table.NewRow()
                    'Dim isEmpty As Boolean = True

                    'For i = 0 To row.Cells.Count - 1
                    '    If (row.Cells(i).Column.Key <> pk) Then
                    '        If (Not row.Cells(i).Value Is Nothing) Then
                    '            isEmpty = False
                    '            addedRow(i) = Trim(row.Cells(i).Value)
                    '        Else
                    '            'If UCase(row.Cells(i).Column.Key) = "ROUTE_ID" Then
                    '            '    addedRow(i) = 0
                    '            'Else
                    '            '    addedRow(i) = ""
                    '            'End If
                    '        End If
                    '    End If
                    'Next

                    'If (Not isEmpty) Then
                    '    Table.Rows.Add(addedRow)
                    'End If

                Case DataChanged.Modified
                    Dim i As Integer
                    Dim ModifiedRow As DataRow
                    Dim key As Object = row.Cells.FromKey(pk).Value
                    ModifiedRow = table.Rows.Find(key)

                    If Not ModifiedRow Is Nothing Then
                        For i = 0 To row.Cells.Count - 1
                            If (row.Cells(i).Column.ToString <> pk And row.Cells(i).Column.Key <> "Voucher_No" And row.Cells(i).Column.Key <> "Voucher_Status") Then
                                If (Not row.Cells(i).Value Is Nothing) Then
                                    ModifiedRow(i) = Trim(row.Cells(i).Value)
                                Else
                                    Select Case UCase(row.Cells(i).Column.Key)
                                        Case "TS_DATE"
                                            ModifiedRow(i) = Date.Now
                                        Case "SCHEDULE_ID", "VOUCHER_STATUS", "VOUCHER_OPENED_BY", "VOUCHER_CLOSED_BY", "USER_ID", "ACCESS_TERMINAL_ID", "SEATS"
                                            ModifiedRow(i) = 0
                                        Case "VOUCHER_CLOSED_DATE"
                                            ModifiedRow(i) = DBNull.Value
                                        Case "BOOK_ID"
                                            ModifiedRow(i) = 0
                                        Case Else
                                            ModifiedRow(i) = ""
                                    End Select
                                End If
                            End If
                        Next
                    End If
            End Select

        End While
    End Sub

    

    

    

#End Region

#Region " Functions And Procedure  "

    Private Sub loadCombos()
        cboRoute.DataSource = objScheduleList.GetAll() '.get.GetRoute()
        cboRoute.DataValueField = "Schedule_Id"
        cboRoute.DataTextField = "Schedule_Title"
        cboRoute.DataBind()

        cboRoute.Items.Insert(0, New ListItem("Select", "0"))


    End Sub

    Private Sub loadTable()
        Dim count = 0
        Dim dtVoucher As New DataTable
        Dim dtVoucher_Online As New DataTable
        Dim dtVouchertemp As New DataTable
        objOnlineTicketing = New eTicketing



        dtVoucher = objTicketing.GetTicketingScheduleUpload(cboRoute.SelectedValue, dtSchedule.Value, txtTime.Text)

        Dim pk(0) As DataColumn
        pk(0) = dtVoucher.Columns("Ticketing_Schedule_ID")
        pk(0).AutoIncrement = True
        dtVoucher.PrimaryKey = pk
        dtVoucher.AcceptChanges()

        If (dtVoucher.Rows.Count > 0) Then

            pk(0).AutoIncrementSeed = dtVoucher.Rows(dtVoucher.Rows.Count - 1).Item("Ticketing_Schedule_ID") + 1
            totalvalue.Value = dtVoucher.Rows.Count

        Else

            pk(0).AutoIncrementSeed = 1

        End If

        'cboTime.DataSource = grdVoucher
        'cboTime.DataValueField = "Departure_Time"




        'For i As Integer = 0 To grdVoucher.Rows.Count - 1

        '    'If grdVoucher.Rows(i).Cells(9).Text <> "23:00" Then
        '    '    grdVoucher.Rows(i).Hidden = True
        '    'Else
        '    '    grdVoucher.Rows(i).Hidden = False
        '    'End If
        'Next


        table = dtVoucher


        'If txtTime.Text <> "" Then
        '    For i As Integer = 0 To grdVoucher.Rows.Count - 1
        '        If grdVoucher.Rows(i).Cells(9).Text <> txtTime.Text Then
        '            grdVoucher.Rows(i).Hidden = True
        '        Else
        '            grdVoucher.Rows(i).Hidden = False
        '        End If
        '    Next
        'Else
        '    For i As Integer = 0 To grdVoucher.Rows.Count - 1
        '        grdVoucher.Rows(i).Hidden = False
        '    Next
        'End If



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



    Private Sub BindTicketingRoute()

        grdVoucher.DataSource = table
        grdVoucher.DataBind()

        'For i As Integer = 5 To grdVoucher.Columns.Count - 1

        '    grdVoucher.Columns(i).Width  = 0


        'Next

    End Sub

    Private Sub RegisterClientEvents()
        'btnSave.Attributes.Add("onclick", "return validation();")
        'btnSave.Style.Add("display", "none")
    End Sub

    'Private Sub getMaxNumber()
    '    Dim sVoucherNo As String = objTicketing.GetMaxVoucherNumber()
    '    Select Case sVoucherNo.Length
    '        Case 1
    '            hidVoucherNo.Value = Format(Now, "ddMMyy") & "00" & sVoucherNo
    '        Case 2
    '            hidVoucherNo.Value = Format(Now, "ddMMyy") & "0" & sVoucherNo
    '        Case Else
    '            hidVoucherNo.Value = Format(Now, "ddMMyy") & sVoucherNo
    '    End Select

    'End Sub

    'Private Sub DateChooserFormat()
    '    dtSchedule.Format = Infragistics.WebUI.WebSchedule.DateFormat.Short
    '    Dim cInfo As New Globalization.CultureInfo("en-US")
    '    cInfo.DateTimeFormat.DateSeparator = "/"
    '    cInfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy"
    '    dtSchedule.CalendarLayout.Culture = cInfo
    'End Sub
#End Region

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpload.Click
        Try

            UploadDataToServer()

        Catch ex As Exception

            lblMessage.Text = ex.Message

        End Try
    End Sub

    Protected Sub btnLoad_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLoad.Click
        loadTable()
        Me.BindTicketingRoute()
    End Sub
End Class