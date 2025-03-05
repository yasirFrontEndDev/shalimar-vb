Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid
Imports FMovers.Ticketing.Online
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Data.SqlClient


Partial Public Class TicketingSchedule
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objScheduleList As clsSchedules

    Dim objServiceType As clsServiceType

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

        'If Page.IsPostBack = False Then

        '    UploadDataToServer()

        'End If

        objConnection = ConnectionManager.GetConnection()
        objUser = CType(Session("CurrentUser"), clsUser)
        objScheduleList = New clsSchedules(objConnection)
        objServiceType = New clsServiceType(objConnection)
        objTicketing = New clsTicketing(objConnection)
        dtVehicle = (New clsVehicle(objConnection)).GetAll()
        dtSchedule.CalendarLayout.Culture = clsUtil.GetDateChooserCulture()
        If "" & Request.QueryString("mode") <> "" Then
            mode = Request.QueryString("mode")
        Else
            mode = "1"
        End If

        If mode = "2" Then
            lblHeading.Text = "Advance Booking"
        ElseIf mode = "1" Then
            lblHeading.Text = "Advance Ticketing"
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


    
    Private Sub UserLogin_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        ConnectionManager.CloseConnection(objConnection)
    End Sub

#End Region

#Region " Control Events "

    Private Sub cboRoute_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboRoute.SelectedIndexChanged
        If cboRoute.SelectedValue <> "0" Then
            LoadDataOnline()
            objTicketing.CreateTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, "")
            loadTable()

            Me.BindTicketingRoute()
            loadOperatedByTable()


        End If
    End Sub

    Private Sub loadOperatedByTable()
        Try


            ' Session("ServiceType") = cboServiceType.SelectedItem.Text
            Dim SchId = cboRoute.SelectedValue
            Dim tbOperatorNames As New DataTable
            tbOperatorNames = GetOperatorNameDetails()

            OperatedDownList.Items.Clear()
            OperatedDownList.DataSource = tbOperatorNames
            OperatedDownList.DataValueField = "Operated_By"
            OperatedDownList.DataTextField = "Operated_By"
            OperatedDownList.DataBind()
            OperatedDownList.Items.Insert(0, New ListItem("Select", "0"))


        Catch ex As Exception
            Response.Write(ex.Message)

        End Try
    End Sub

    Private Sub OperatedDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles OperatedDownList.SelectedIndexChanged
        If OperatedDownList.SelectedValue <> "0" Then



            Dim ManageCompany = OperatedDownList.SelectedValue


            Dim settings = System.Configuration.ConfigurationManager.AppSettings

            settings.Set("Operated_By", ManageCompany)



        End If
    End Sub

    Private Function GetOperatorNameDetails() As DataTable
        Dim objDbManager As IDBManager
        Dim objDataSet As DataSet
        objDbManager = DBManager.GetDatabaseManager()
        objDbManager.SetDBConnection(objConnection)
        Dim objDBParameters As New clsDBParameters
        'Session("TicketingScheduleId") = TicketingScheduleId

        objDBParameters.Parameters.Add(New clsDBParameter("@Schedule_Id", cboRoute.SelectedValue, "bigint"))
        objDataSet = objDbManager.GetData("GetOperatorbyCompany", objDBParameters)
        If Not objDataSet Is Nothing Then
            Return objDataSet.Tables(0)
        Else
            Return Nothing
        End If

    End Function

    Private Sub LoadDataOnline()
        Try


            'Dim dtVoucher_Online As DataTable
            'objOnlineTicketing = New eTicketing

            'If ServerPing() Then

            '    dtVoucher_Online = objOnlineTicketing.GetTicketingSchedule_Online(cboRoute.SelectedValue, dtSchedule.Value)

            '    For Each dr As DataRow In dtVoucher_Online.Rows

            '        If "" & dr("Vehicle_ID") <> "" And "" & dr("Vehicle_ID") <> "81" Then

            '            objTicketing.ScheduleID = cboRoute.SelectedValue
            '            objTicketing.TSDate = dtSchedule.Value
            '            objTicketing.DepartureTime = "" & dr("Departure_Time")
            '            objTicketing.DriverName = "" & dr("Driver_Name")

            '            objTicketing.HostessName = "" & dr("Hostess_Name")
            '            objTicketing.VehicleID = dr("Vehicle_ID")

            '            objTicketing.UpdateOnlineTicketingSchedule_Online()


            '        End If

            '    Next

            'End If


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
                .Columns.FromKey("Is_Closed").Hidden = True
                .Columns.FromKey("Is_Pulled").Hidden = True
                .Columns.FromKey("ServiceType_Id").Hidden = True
                .Columns.FromKey("ServiceType_Id").Hidden = True

                .Columns.FromKey("User_ID").Hidden = True
                .Columns.FromKey("Access_DateTime").Hidden = True
                .Columns.FromKey("Access_Sys_Name").Hidden = True
                .Columns.FromKey("Access_Terminal_ID").Hidden = True
                .Columns.FromKey("Vehicle_ID").Hidden = True


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

                Dim vList As New ValueList

                For Each drow As DataRow In dtVehicle.Rows
                    vList.ValueListItems.Add(drow.Item("Vehicle_ID"), "" & drow.Item("Registration_No"))
                Next

                .Columns.FromKey("Vehicle_ID").Type = ColumnType.DropDownList
                .Columns.FromKey("Vehicle_ID").ValueList = vList

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
                    If DTime.Length > 0 And CType(DTime(1), Integer) <= 59 Then
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

                Case DataChanged.Modified
                    Dim i As Integer
                    Dim ModifiedRow As DataRow
                    Dim key As Object = row.Cells.FromKey(pk).Value
                    ModifiedRow = table.Rows.Find(key)
                    'ServiceType_Id1 

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

    

    Protected Sub lnkTicketing_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkTicketing.Click

        objTicketing.UserId = objUser.Id
        objTicketing.ComputerName = "System"
        objTicketing.AccessTerminalId = 1

        For Each dRow As DataRow In table.Rows

            If "" & dRow.Item("Ticketing_Schedule_ID") = hidTSID.Value.Trim() Then

                With objTicketing
                    .Id = dRow.Item("Ticketing_Schedule_ID")

                    If (Not IsDBNull(.Id)) AndAlso .Id <> 0 Then
                        .GetById()
                    End If

                    .ScheduleID = cboRoute.SelectedValue

                    .SerialNo = dRow.Item("Sr_No")
                    .DepartureTime = dRow.Item("Departure_Time")

                    If "" & dRow.Item("Vehicle_ID") <> "" Then
                        If objTicketing.VehicleID <> 0 Then
                            .VehicleID = dRow.Item("Vehicle_ID")
                        Else
                            .VehicleID = 81
                        End If
                    Else
                        .VehicleID = 81
                    End If

                    .DriverName = "" & dRow.Item("Driver_Name")
                    .HostessName = "" & dRow.Item("Hostess_Name")

                End With


                objTicketing.Save(False)
                Exit For
            End If
        Next

        '///////////////////////////////////////////////////////////////////////////////////////
        If mode = "2" Then
            Response.Redirect("Ticketing.aspx?mode=2&TSID=" & hidTSID.Value.Trim())
        ElseIf mode = "1" Then
            Response.Redirect("Ticketing.aspx?mode=1&TSID=" & hidTSID.Value.Trim())
        Else
            Response.Redirect("Ticketing.aspx?TSID=" & hidTSID.Value.Trim())
        End If
    End Sub



#End Region

#Region " Functions And Procedure  "

    Private Sub loadCombos()

        cboRoute.DataSource = objScheduleList.GetAll() '.get.GetRoute()
        cboRoute.DataValueField = "Schedule_Id"
        cboRoute.DataTextField = "Schedule_Title"
        cboRoute.DataBind()

        cboRoute.Items.Insert(0, New ListItem("Select", "0"))


        cboServiceType.DataSource = objServiceType.GetServiceTypes() '.get.GetRoute()
        cboServiceType.DataValueField = "ServiceType_Id"
        cboServiceType.DataTextField = "ServiceType_Name"
        cboServiceType.DataBind()

        cboServiceType.Items.Insert(0, New ListItem("Select", "0"))

    End Sub

    Private Sub loadTable()
        Dim count = 0
        Dim dtVoucher As New DataTable
        Dim dtVoucher_Online As New DataTable
        Dim dtVouchertemp As New DataTable
        objOnlineTicketing = New eTicketing

        Dim Merge As Integer = 0
        If rdoMerge.SelectedItem.Value = 1 Then
            Merge = 1
        End If

        dtVoucher = objTicketing.GetTicketingScheduleLoad(cboRoute.SelectedValue, dtSchedule.Value, cboServiceType.SelectedValue, Merge)



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

        table = dtVoucher


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


    Private Sub lnkReserve_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkReserve.Click


        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        Dim dtVoucher As New DataTable
        Dim dtVoucher_Online As New DataTable
        Dim dtVouchertemp As New DataTable
        objOnlineTicketing = New eTicketing

        dtVoucher = objTicketing.getTicketingSchById(hndTicketing_Schedule_ID.Value)

        objTicketing.UserId = objUser.Id
        objTicketing.ComputerName = "System"
        objTicketing.AccessTerminalId = 1



        With objTicketing
            .Id = hndTicketing_Schedule_ID.Value

            If (Not IsDBNull(.Id)) AndAlso .Id <> 0 Then
                .GetById()
            End If

            .ScheduleID = cboRoute.SelectedValue

            .SerialNo = hndSr_No.Value
            .DepartureTime = dtVoucher.Rows(0)("Departure_Time")

            If "" & hndVehicle_ID.Value <> "" Then
                If objTicketing.VehicleID <> 0 Then
                    .VehicleID = hndVehicle_ID.Value
                Else
                    .VehicleID = 81
                End If
            Else
                .VehicleID = 81
            End If



            .DriverName = "" & dtVoucher.Rows(0)("Driver_Name")
            .HostessName = "" & dtVoucher.Rows(0)("Hostess_Name")

        End With


        objTicketing.Save(False)
       

        '///////////////////////////////////////////////////////////////////////////////////////
        If mode = "2" Then
            Response.Redirect("Ticketing.aspx?mode=2&TSID=" & hndTicketing_Schedule_ID.Value.Trim())
        ElseIf mode = "1" Then
            Response.Redirect("Ticketing.aspx?mode=1&TSID=" & hndTicketing_Schedule_ID.Value.Trim())
        Else
            Response.Redirect("Ticketing.aspx?TSID=" & hndTicketing_Schedule_ID.Value.Trim())
        End If

    End Sub


    Private Sub BindTicketingRoute()
        Dim Counter As Integer = 0
        If "" & Request.QueryString("mode") <> "" Then
            mode = Request.QueryString("mode")
        Else
            mode = "1"
        End If
        Dim tbRowCounter As Integer

        tbRowCounter = tbSearch.Rows.Count

        For j As Integer = 0 To tbRowCounter - 1
            If tbSearch.Rows.Count > j Then
                tbSearch.Rows.RemoveAt(j)
            End If
        Next


       

        For Each dr As DataRow In table.Rows
            Dim tblRow As HtmlTableRow
            tblRow = New HtmlTableRow()



            Counter = Counter + 1
            Dim dummyDrimer = "0"


            Dim htmlCell_Load As New HtmlTableCell()
            htmlCell_Load.ID = "cell_1" & Counter
            htmlCell_Load.Attributes.Add("class", "tbThirdRow")

            Dim Departure_Time As String = dr("Departure_Time").ToString().Substring(0, 2) & ":" & dr("Departure_Time").ToString().Substring(2)
            Dim Departure_Time2 As String = dr("Departure_Time").ToString()

            'htmlCell_Load.InnerHtml = " <span class='dep_time_Load' > <a  class='dep_time_Load' href='#' onclick='LoadSchedule(" + dr("Ticketing_Schedule_ID").ToString() + " ," + IIf(dr("Sr_No").ToString() = "", "''", dr("Sr_No").ToString()) + " ," + "'" + dr("Departure_Time") + "'" + " ," + IIf(dr("Vehicle_ID").ToString() = "", "0", dr("Vehicle_ID").ToString()) + " ," + IIf(dr("Driver_Name").ToString() = "", "0", dr("Driver_Name").ToString()) + " ," + IIf(dr("Hostess_Name").ToString(), "0", dr("Hostess_Name").ToString()) + "  )' > Load </span> "
            htmlCell_Load.InnerHtml = " <span class='dep_time_Load' > <a  class='dep_time_Load' href='#' onclick='LoadSchedule(" + dr("Ticketing_Schedule_ID").ToString() + " ," + IIf(dr("Sr_No").ToString() = "", "''", dr("Sr_No").ToString()) + " ," + Departure_Time2 + " ," + IIf(dr("Vehicle_ID").ToString() = "", "0", dr("Vehicle_ID").ToString()) + " ," + dummyDrimer + " ," + dummyDrimer + ")' > Load </span> "
            tblRow.Cells.Insert(0, htmlCell_Load)


            Dim htmlCell_SerType As New HtmlTableCell()
            htmlCell_SerType.ID = "cell_2" & Counter
            htmlCell_SerType.Attributes.Add("class", "tbSecondRow")

            'htmlCell.InnerText = dr("Departure_Time") & " <br /> " & dr("Departure_Time")
            htmlCell_SerType.InnerHtml = " <span class='dep_time_Title' > " + dr("Schedule_Title") + " </span> "
            tblRow.Cells.Insert(0, htmlCell_SerType)




            Dim htmlCell_Title As New HtmlTableCell()
            htmlCell_Title.ID = "cell_3" & Counter
            htmlCell_Title.Attributes.Add("class", "tbSecondRow")

            'htmlCell.InnerText = dr("Departure_Time") & " <br /> " & dr("Departure_Time")
            htmlCell_Title.InnerHtml = " <span class='dep_time_Title' > " + dr("ServiceType_Name") + " </span> "
            tblRow.Cells.Insert(0, htmlCell_Title)





            Dim htmlCell As New HtmlTableCell()
            htmlCell.ID = "cell_" & Counter
            'htmlCell.InnerText = dr("Departure_Time") & " <br /> " & dr("Departure_Time")
            htmlCell.Attributes.Add("class", "tbFirstRow")
            htmlCell.InnerHtml = " <span class='dep_time_grd' > " + Departure_Time + " </span> "
            tblRow.Cells.Insert(0, htmlCell)


            tbSearch.Rows.Add(tblRow)
        Next


        'grdVoucher.DataSource = table
        'grdVoucher.DataBind()

    End Sub

    Private Sub RegisterClientEvents()
        'btnSave.Attributes.Add("onclick", "return validation();")
        'btnSave.Style.Add("display", "none")
    End Sub


#End Region

    Protected Sub cboServiceType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboServiceType.SelectedIndexChanged
        If cboRoute.SelectedValue <> "0" Then
            LoadDataOnline()
            objTicketing.CreateTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, "")
            loadTable()
            Me.BindTicketingRoute()

        End If
    End Sub

    Protected Sub rdoMerge_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdoMerge.SelectedIndexChanged
        If cboRoute.SelectedValue <> "0" Then
            LoadDataOnline()
            objTicketing.CreateTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, "")
            loadTable()
            Me.BindTicketingRoute()

        End If
    End Sub
End Class