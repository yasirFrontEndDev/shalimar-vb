Imports System.Configuration
Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online
Imports Infragistics.WebUI.UltraWebGrid
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Web
Imports System.Text
Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine

Imports GsmComm.PduConverter
Imports GsmComm.GsmCommunication
Imports System.Drawing.Printing

Imports System.Management


Partial Public Class UserCashClosingMulti
    Inherits System.Web.UI.Page

    Dim objBusCharges As clsBusCharges
    Dim objConnection As Object
    Dim objUser As clsUser
    Public UserName As String
    Public CurrentUserID As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        Else
            objUser = CType(Session("CurrentUser"), clsUser)
            CurrentUserID = "" & objUser.Id
            UserID.Value = "" & objUser.Id

            UserName = objUser.LoginName.ToLower()
        End If


        'If Not ServerPing() Then

        '    lblError.Style.Add("DISPLAY", "")
        '    lblError.Text = "Server is not online please contact IT team."
        '    btnShowReport.Visible = False

        'End If

        If Page.IsPostBack = False Then
            UploadDataToServer()
            loadCombes()
            loadbalance()
            InitizlizeGVWODetail()
            InitizlizeGVWODetail2()
        End If


    End Sub
    Protected Sub grdUsers_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdUsers.RowCommand
        Try

            If e.CommandName = "Delete" Then

                Dim drToRemove As DataRow
                Dim dt As DataTable
                dt = ViewState("dtRPTVWODetail")
                Dim isRemoved As Boolean = False

                'Dim Index As Integer = Convert.ToInt32(e.CommandArgument)
                Dim KeyID As Integer = Convert.ToInt32(e.CommandArgument)



                'Dim gvRow As GridViewRow = grdUsers.Rows(Index)

                For Each dr As DataRow In dt.Rows
                    'dr("SequenceNo") = Convert.ToInt32(dr("SequenceNo")) - 1

                    If KeyID = Convert.ToInt32(dr("SequenceNo").ToString()) Then
                        drToRemove = dr
                        dt.Rows.Remove(drToRemove)
                        dt.AcceptChanges()
                        If dt.Rows.Count = 0 Then
                            grdUsers.DeleteRow(KeyID)
                            grdUsers.DataSource = Nothing
                            ViewState("dtRPTVWODetail") = Nothing
                            grdUsers.DataBind()
                        Else
                            ViewState("dtRPTVWODetail") = dt
                            grdUsers.DeleteRow(KeyID)
                            grdUsers.DataSource = dt
                            grdUsers.DataBind()

                        End If

                        'isRemoved = True
                        'Exit For
                    End If


                Next
                'If (isRemoved = True) Then

                '    dt.Rows.Remove(drToRemove)
                '    If dt.Rows.Count = 0 Then
                '        grdUsers.DataSource = Nothing
                '        grdUsers.DataBind()

                '    Else

                '    End If

                'End If

            End If
        Catch ex As Exception
            lblError.Visible = True
            lblError.Text = ex.Message

        End Try
    End Sub
    Private Function ServerPing() As Boolean

        Try


            Dim ping As New Ping
            Dim pingreply As PingReply = ping.Send(FMovers.Ticketing.DAL.Crypto.Decrypt(System.Configuration.ConfigurationManager.AppSettings("ServerIPAddress").ToString.Trim, ""))

            If pingreply.Status = IPStatus.Success Then

                ' Send Heart Beat

                'MessageBox.Show("Server connected")

            Else
               
                Return False

            End If



            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function
    Protected Sub btnAddGrid_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddGrid.Click
        Try

            Dim dt As DataTable = New DataTable
            dt = ViewState("dtRPTVWODetail")
            Dim dr As DataRow = dt.NewRow()

            lblError.Text = ""

            For Each drCheck As DataRow In dt.Rows
                If Convert.ToInt32(drCheck("Schedule_Id")) = _
                Convert.ToInt32(cboSchedule.SelectedItem.Value) Then

                    lblError.Visible = True
                    lblError.Text = "Route already added."
                    Exit Sub


                End If

            Next

            Dim dc As New DataColumn
            Dim ArrAmount As Array
            Dim Amount As String

            ArrAmount = cboSchedule.SelectedItem.Text.Split("[")

            'Amount = ArrAmount(1).ToString().Replace("[")


            dr("Schedule") = cboSchedule.SelectedItem.Text
            dr("User") = cboUsers.SelectedItem.Text
            dr("Amount") = ArrAmount(1).ToString().Trim().Replace("]", "")
            dr("Schedule_Id") = cboSchedule.SelectedItem.Value
            dr("User_Id") = cboUsers.SelectedItem.Value

            If (dt.Rows.Count > 0) Then
                dr("SequenceNo") = dt.Rows.Count + 1

            Else
                dr("SequenceNo") = 1
            End If


            dt.Rows.Add(dr)
            ViewState("dtRPTVWODetail") = dt

            grdUsers.DataSource = dt
            grdUsers.DataBind()

            grdUsers.Columns(4).Visible = False
            grdUsers.Columns(5).Visible = False



        Catch ex As Exception
            lblError.Visible = True
            lblError.Text = ex.Message

        End Try
    End Sub
    Private Sub InitizlizeGVWODetail()

        Dim dt As DataTable = New DataTable
        Dim dc As DataColumn
        dc = New DataColumn("Schedule")
        dt.Columns.Add(dc)

        dc = New DataColumn("User")
        dt.Columns.Add(dc)

        dc = New DataColumn("Amount")
        dt.Columns.Add(dc)

        dc = New DataColumn("Schedule_Id")
        dt.Columns.Add(dc)

        dc = New DataColumn("User_Id")
        dt.Columns.Add(dc)

        dc = New DataColumn("SequenceNo")
        dt.Columns.Add(dc)

        Dim dr As DataRow = dt.NewRow()

        ViewState("dtRPTVWODetail") = dt
        grdUsers.DataSource = dt
        grdUsers.DataBind()
        grdUsers.Columns(4).Visible = False
        grdUsers.Columns(5).Visible = False




    End Sub

    Private Sub InitizlizeGVWODetail2()

        Dim dt As DataTable = New DataTable
        Dim dc As DataColumn
        dc = New DataColumn("User")
        dt.Columns.Add(dc)


        dc = New DataColumn("Amount")
        dt.Columns.Add(dc)

      

        dc = New DataColumn("User_Id")
        dt.Columns.Add(dc)

        dc = New DataColumn("SequenceNo")
        dt.Columns.Add(dc)

        Dim dr As DataRow = dt.NewRow()

        ViewState("dtRPTVWODetail2") = dt
        grdDec.DataSource = dt
        grdDec.DataBind()
        'grdDec.Columns(2).Visible = False
        grdDec.Columns(3).Visible = False




    End Sub

    Private Sub loadbalance()



        Dim strClosing As String = ""
        Dim Arr As Array

        objConnection = ConnectionManager.GetConnection()

        objBusCharges = New clsBusCharges(objConnection)
        objBusCharges.UserId = CurrentUserID

        Dim Openning As Integer = objBusCharges.GetOpeningBalance(CurrentUserID)

        strClosing = objBusCharges.GetByTicketingAdvanceById()
        Arr = strClosing.Split("~")

        If Arr.Length > 1 Then

            lblAdance.Text = Math.Round(CDbl(Val(Arr(0))))

            lblMissed.Text = Math.Round(CDbl(Val(Arr(3))))
            lblRefund.Text = Math.Round(CDbl(Val(Arr(5))))
            lblChange.Text = Math.Round(CDbl(Val(Arr(4))))

            lblCashCollection.Text = Math.Round(CDbl(Val(Arr(1))))
            lblDeduction.Text = Math.Round(CDbl(Val(Arr(2))), 0)
            lblTotal.Text = Math.Round(CDbl(Arr(1))) + Math.Round(CDbl(Arr(2)))

        Else

            lblAdance.Text = "0"
            lblCashCollection.Text = "0"
            lblDeduction.Text = "0"
            lblTotal.Text = "0"

        End If

        lblOpeniningBal.Text = Openning.ToString()

        lblTotal.Text = CDbl(Val(lblTotal.Text)) + CDbl(Val(Openning.ToString()))






    End Sub

    Private Sub loadCombes()

        Dim dtDataTable As New DataTable

        dtDataTable = objUser.GetAllForClosing()
        cboUsers.Items.Clear()
        cboUsers.DataSource = dtDataTable
        cboUsers.DataValueField = "User_Id"
        cboUsers.DataTextField = "Comeplete"
        cboUsers.DataBind()


        cboUsers0.Items.Clear()
        cboUsers0.DataSource = dtDataTable
        cboUsers0.DataValueField = "User_Id"
        cboUsers0.DataTextField = "Comeplete"
        cboUsers0.DataBind()

        cboSchedule.Items.Clear()
        dtDataTable = objUser.getUserClosingSchedules()
        cboSchedule.Items.Clear()
        cboSchedule.DataSource = dtDataTable
        cboSchedule.DataValueField = "Schedule_ID"
        cboSchedule.DataTextField = "Schedule_Title"
        cboSchedule.DataBind()


    End Sub


    Protected Sub btnShowReport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnShowReport.Click

        objConnection = ConnectionManager.GetConnection()

        objBusCharges = New clsBusCharges(objConnection)

        If Not ServerPing() Then
            UploadDataToServer()
        End If

        Dim Result As Integer = 0
        Dim BookId As Integer = 0, TotalAdvance As Decimal = 0, TotalDecuction As Decimal = 0


        Dim Total As Integer = 0, ShiftToCashier As Integer = 0
        If chkCashToCashier.Checked = True Then
            ShiftToCashier = 1
        End If

        Total = (CInt(lblOpeniningBal.Text)) + (CInt(lblCashCollection.Text)) + (CInt(lblAdance.Text)) + (CInt(lblDeduction.Text))


        If (Total = 0) Then

            lblError.Style.Add("DISPLAY", "")
            lblError.Text = "User Closing can not be proceeded. Not traction to proceed."
        Else

            Dim dt As DataTable
            dt = New DataTable
            dt = ViewState("dtRPTVWODetail")



            Dim dt2 As DataTable
            dt2 = New DataTable
            dt2 = ViewState("dtRPTVWODetail2")


            For Each dr As DataRow In dt.Rows
                TotalAdvance = TotalAdvance + CDbl(dr("Amount"))
            Next

            If lblAdance.Text <> "0" Then
                If TotalAdvance < CDbl(lblAdance.Text) Then
                    lblError.Style.Add("DISPLAY", "")
                    lblError.Text = "User Closing can not be proceeded. Please shift advance seats fully."
                    Exit Sub
                End If
            End If

            For Each dr As DataRow In dt2.Rows
                TotalDecuction = TotalDecuction + CDbl(dr("Amount"))
                ' objBusCharges.Comission_Shift_Insert(CInt(dr("User_Id")), CInt(dr("Amount")), BookId)
            Next

            If lblTotal.Text <> "0" Then
                If TotalDecuction < CDbl(lblTotal.Text) Then
                    lblError.Style.Add("DISPLAY", "")
                    lblError.Text = "User Closing can not be proceeded. Please shift deduction cash fully."
                    Exit Sub
                End If
            End If



            If CInt(Val(cboUsers0.SelectedValue)) = 0 Then
                lblError.Style.Add("DISPLAY", "")
                lblError.Text = "User Closing can not be proceeded. select user to shift cash fully."
                Exit Sub
            Else
                BookId = objBusCharges.UserCashClosingInsert(2, CurrentUserID, CInt(Val(cboUsers0.SelectedValue)), Total, ShiftToCashier)
                If ServerPing() Then
                    UploadBookOnServer(BookId, 2, CurrentUserID, CInt(Val(cboUsers0.SelectedValue)), Now.Date, 0, objUser.TerminalId, 0, 0)
                End If
            End If

            For Each dr As DataRow In dt.Rows
                objBusCharges.Advance_Multi(BookId, CurrentUserID, CInt(dr("User_Id")), CInt(dr("Schedule_Id")))
            Next

            For Each dr As DataRow In dt2.Rows
                objBusCharges.Comission_Shift_Insert(CInt(dr("User_Id")), CInt(dr("Amount")), BookId)
            Next


            lblOK.Visible = True
            grdUsers.DataSource = Nothing
            grdUsers.DataBind()
            grdUsers.Visible = False
            btnShowReport.Visible = False
            btnAdd.Visible = False



        End If

        loadbalance()

        If Not ServerPing() Then
            UploadDataToServerUserBook(BookId)
        End If

        ' Will uncommect at the end


    End Sub

    '    
    Private Sub UploadBookOnServer(ByVal Book_Id As Integer, ByVal Closing_Type_Id As Integer, ByVal Current_Id As Integer, ByVal Shift_User_Id As Integer, ByVal CurrentDate As Date, ByVal ShiftToCashier As Integer, ByVal Terminal_Id As Integer, ByVal CashReceived As Integer, ByVal CashReceived_Id As Integer)
        Try

            lblError.Text = ""

            Dim objOnlineTicketing As eTicketing
            objOnlineTicketing = New eTicketing
            objOnlineTicketing.AddBookIdOnServer(Book_Id, Closing_Type_Id, Current_Id, Shift_User_Id, CurrentDate, ShiftToCashier, Terminal_Id, 0, 0)

        Catch ex As Exception
            lblError.Text = ex.Message()
        End Try
    End Sub
    Protected Sub lnkDelete_Click(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Private Sub UploadDataToServer()
        Try
            Dim Connections As SqlClient.SqlConnection
            Dim ConnectionsOnline As SqlClient.SqlConnection

            If ServerPing() Then




                Dim ConnString As System.Configuration.ConnectionStringSettings
                ConnString = System.Configuration.ConfigurationManager.ConnectionStrings("FMoversLocal")

                Dim ConnStringOnline As System.Configuration.ConnectionStringSettings
                ConnStringOnline = System.Configuration.ConfigurationManager.ConnectionStrings("FMoversCentral")


                Connections = New SqlConnection()
                Connections.ConnectionString = FMovers.Ticketing.DAL.Crypto.Decrypt(ConnString.ConnectionString, "")


                ConnectionsOnline = New SqlConnection()
                ConnectionsOnline.ConnectionString = FMovers.Ticketing.DAL.Crypto.Decrypt(ConnStringOnline.ConnectionString, "")

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


                If dt_OfflineDataTaleSystem.Rows.Count > 0 Then
                    Access_Terminal_Id = dt_OfflineDataTaleSystem.Rows(0)("Default_Terminal")
                End If

                ' ******************************** Start load which are not loaded before ******************************** 

                str_Query = " Select * from Ticketing_Schedule Where Voucher_Closed_By = " & CurrentUserID & " and isnull(Book_Id,0) = 0 and Voucher_status = 2  "

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

                    rtn_Value = Convert.ToInt64(Command_Online.ExecuteScalar())

                    '****************** Get Seats Offline ************************

                    '   LogTextToFileLogFile(Offline_Dr("Ticketing_Schedule_ID"))

                    str_Query = " Select * From Ticketing_Seat Where Status = 4 and Ticketing_Schedule_ID = " & Offline_Dr("Ticketing_Schedule_ID") & " and Issue_Terminal =  " & Offline_Dr("Access_Terminal_Id") & " Order By Seat_No "
                    Command_Offline.CommandType = CommandType.Text
                    Command_Offline.CommandText = str_Query
                    dt_OfflineDataTale_DataList.Clear()
                    dt_OfflineDataTale_DataList.Dispose()

                    Adapter_Offline.SelectCommand = Command_Offline
                    Adapter_Offline.Fill(dt_OfflineDataTale_DataList)



                    '****************** Get Seats Online ************************

                    If dtValidate.Rows(0)(0).ToString() <> dt_OfflineDataTale_DataList.Rows.Count Then

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

                            Command_Offline.CommandText = " update Ticketing_Schedule set Is_Pulled = 1 where Ticketing_Schedule_ID = " & Offline_Dr("Ticketing_Schedule_ID")

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


            ConnectionsOnline.Close()
            ConnectionsOnline.Dispose()
            Connections.Close()
            Connections.Dispose()


        Catch ex As Exception

            lblError.Text = ex.Message + " " + " Please contact IT Team."

            '  Response.Write(ex.Message)

        Finally


        End Try
    End Sub


    Private Sub UploadDataToServerUserBook(ByVal pBook_Id As Integer)
        Try
            Dim Connections As SqlClient.SqlConnection
            Dim ConnectionsOnline As SqlClient.SqlConnection

            If ServerPing() Then

                Dim ConnString As System.Configuration.ConnectionStringSettings
                ConnString = System.Configuration.ConfigurationManager.ConnectionStrings("FMoversLocal")

                Dim ConnStringOnline As System.Configuration.ConnectionStringSettings
                ConnStringOnline = System.Configuration.ConfigurationManager.ConnectionStrings("FMoversCentral")


                Connections = New SqlConnection()
                Connections.ConnectionString = FMovers.Ticketing.DAL.Crypto.Decrypt(ConnString.ConnectionString, "")


                ConnectionsOnline = New SqlConnection()
                ConnectionsOnline.ConnectionString = FMovers.Ticketing.DAL.Crypto.Decrypt(ConnStringOnline.ConnectionString, "")

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


                If dt_OfflineDataTaleSystem.Rows.Count > 0 Then
                    Access_Terminal_Id = dt_OfflineDataTaleSystem.Rows(0)("Default_Terminal")
                End If

                ' ******************************** Start load which are not loaded before ******************************** 

                str_Query = " Select * from Ticketing_Schedule Where Voucher_Closed_By = " & CurrentUserID & " and isnull(Book_Id,0) = " & pBook_Id & "  and Voucher_status = 2  "

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

                    rtn_Value = Convert.ToInt64(Command_Online.ExecuteScalar())

                    '****************** Get Seats Offline ************************

                    '   LogTextToFileLogFile(Offline_Dr("Ticketing_Schedule_ID"))

                    str_Query = " Select * From Ticketing_Seat Where Status = 4 and Ticketing_Schedule_ID = " & Offline_Dr("Ticketing_Schedule_ID") & " and Issue_Terminal =  " & Offline_Dr("Access_Terminal_Id") & " Order By Seat_No "
                    Command_Offline.CommandType = CommandType.Text
                    Command_Offline.CommandText = str_Query
                    dt_OfflineDataTale_DataList.Clear()
                    dt_OfflineDataTale_DataList.Dispose()

                    Adapter_Offline.SelectCommand = Command_Offline
                    Adapter_Offline.Fill(dt_OfflineDataTale_DataList)



                    '****************** Get Seats Online ************************

                    If dtValidate.Rows(0)(0).ToString() <> dt_OfflineDataTale_DataList.Rows.Count Then

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

                            Command_Offline.CommandText = " update Ticketing_Schedule set Is_Pulled = 1 where Ticketing_Schedule_ID = " & Offline_Dr("Ticketing_Schedule_ID")

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


            ConnectionsOnline.Close()
            ConnectionsOnline.Dispose()
            Connections.Close()
            Connections.Dispose()


        Catch ex As Exception

            lblError.Text = ex.Message + " " + " Please contact IT Team."

            '  Response.Write(ex.Message)

        Finally


        End Try
    End Sub

    Protected Sub grdUsers_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles grdUsers.SelectedIndexChanged

        Response.Write(e)


    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReset.Click
        Response.Redirect("UserCashClosingMulti.aspx")
    End Sub

    Protected Sub btnAddGrid0_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddGrid0.Click
        Try

            Dim dt As DataTable = New DataTable
            dt = ViewState("dtRPTVWODetail2")
            Dim dr As DataRow = dt.NewRow()
            'lblDeduction
            Dim GrandTotal As Decimal = 0
            lblError.Text = ""

            For Each drCheck As DataRow In dt.Rows

                GrandTotal = GrandTotal + CDbl(drCheck("Amount"))

                'If Convert.ToInt32(drCheck("User_Id")) = _
                'Convert.ToInt32(cboSchedule.SelectedItem.Value) Then

                '    lblError.Visible = True
                '    lblError.Text = "Route already added."
                '    Exit Sub


                'End If

            Next


            If (GrandTotal + CDbl(txtAmounts.Text)) > CDbl(lblTotal.Text) Then

                lblError.Visible = True
                lblError.Text = "Given amount can not be greater then deduction amount."
                Exit Sub

            End If



            Dim dc As New DataColumn
            Dim ArrAmount As Array
            Dim Amount As String

            'ArrAmount = txtAmounts.Text

            'Amount = ArrAmount(1).ToString().Replace("[")


            dr("User") = cboUsers0.SelectedItem.Text
            dr("Amount") = txtAmounts.Text
            dr("User_Id") = cboUsers0.SelectedItem.Value

            If (dt.Rows.Count > 0) Then
                dr("SequenceNo") = dt.Rows.Count + 1

            Else
                dr("SequenceNo") = 1
            End If


            dt.Rows.Add(dr)
            ViewState("dtRPTVWODetail2") = dt

            grdDec.DataSource = dt
            grdDec.DataBind()

            ' grdDec.Columns(2).Visible = False
            grdDec.Columns(3).Visible = False



        Catch ex As Exception
            lblError.Visible = True
            lblError.Text = ex.Message

        End Try
    End Sub
End Class