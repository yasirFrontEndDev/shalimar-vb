Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid
Imports FMovers.Ticketing.Online
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Data.SqlClient


Partial Public Class SelectCity
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objScheduleList As clsSchedules
    Dim objCity As clsCity
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
           
        End If

        Call loadTable()
        'Call RegisterClientEvents()

        If Not Me.IsPostBack Then
            If cboFromCity.Text <> "" And cboToCity.Text <> "" Then
                Call BindTicketingRoute()
            End If
        End If

    End Sub



    Private Sub UserLogin_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        ConnectionManager.CloseConnection(objConnection)
    End Sub

#End Region

#Region " Control Events "

    Private Sub cboFromCity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboFromCity.SelectedIndexChanged
        If cboFromCity.SelectedValue <> "0" And cboToCity.SelectedValue <> "0" Then
            loadTable()
            Me.BindTicketingRoute()
        End If
    End Sub

    Private Sub cboToCity_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboToCity.SelectedIndexChanged
        If cboFromCity.SelectedValue <> "0" And cboToCity.SelectedValue <> "0" Then
            loadTable()
            Me.BindTicketingRoute()
        End If
    End Sub


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

            .ScheduleID = hndRoute.Value 

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

            .DriverName = "" & hndDriver_Name.Value
            .HostessName = "" & hndHostess_Name.Value

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
        If cboFromCity.SelectedValue <> "0" And cboToCity.SelectedValue Then
            loadTable()
            Me.BindTicketingRoute()
        End If
    End Sub

    



    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

        objTicketing.UserId = objUser.Id
        objTicketing.ComputerName = "System"
        objTicketing.AccessTerminalId = 1

        'If Not ViewState("RouteID") Is Nothing Then
        '    objRoute.Id = ViewState("RouteID")
        'End If

        For Each dRow As DataRow In table.Rows
            If Not dRow.RowState = DataRowState.Unchanged Then
                If dRow.RowState = DataRowState.Deleted Then
                    'objRouteDetail.Id = dRow.Item("Route_Detail_ID", DataRowVersion.Original)
                    'clsUtil.DeleteRow(objConnection, "Route_Detail", objRouteDetail.Id)
                Else
                    With objTicketing
                        .Id = dRow.Item("Ticketing_Schedule_ID")
                        If (Not IsDBNull(.Id)) AndAlso .Id <> 0 Then
                            .GetById()
                        End If

                        .ScheduleID = dRow.Item("Schedule_Id")
                        .SerialNo = dRow.Item("Sr_No")
                        .DepartureTime = dRow.Item("Departure_Time")
                        If "" & dRow.Item("Vehicle_ID") <> "" Then
                            .VehicleID = dRow.Item("Vehicle_ID")
                        Else
                            .VehicleID = 0
                        End If
                        .DriverName = "" & dRow.Item("Driver_Name")
                        .HostessName = "" & dRow.Item("Hostess_Name")
                    End With

                    If dRow.RowState = DataRowState.Modified Then
                        objTicketing.Save(False)
                    End If
                End If
            End If

        Next

        'RouteID = objRoute.Id
        'ViewState("RouteID") = objRoute.Id
        Call loadTable()
        Call BindTicketingRoute()
        'Call getMaxNumber()
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

                    .ScheduleID = dRow.Item("Schedule_ID")

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
        Response.Redirect("Ticketing.aspx?mode=2&TSID=" & hidTSID.Value.Trim() & "&FROMID=" & cboFromCity.SelectedValue.Trim())

    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'Response.Redirect("Routes.aspx")
    End Sub

#End Region

#Region " Functions And Procedure  "

    Private Sub loadCombos()

        'cboRoute.DataSource = objScheduleList.GetAll() '.get.GetRoute()
        'cboRoute.DataValueField = "Schedule_Id"
        'cboRoute.DataTextField = "Schedule_Title"
        'cboRoute.DataBind()

        'cboRoute.Items.Insert(0, New ListItem("Select", "0"))


        'cboServiceType.DataSource = objServiceType.GetServiceTypes() '.get.GetRoute()
        'cboServiceType.DataValueField = "ServiceType_Id"
        'cboServiceType.DataTextField = "ServiceType_Name"
        'cboServiceType.DataBind()

        'cboServiceType.Items.Insert(0, New ListItem("Select", "0"))

        Dim dtSystemInfo As New DataTable

        objCity = New clsCity(objConnection)

        cboFromCity.DataSource = objCity.GetCities() '.get.GetRoute()
        dtSystemInfo = objCity.GetSystemInfo()

        cboFromCity.DataValueField = "City_Id"
        cboFromCity.DataTextField = "City_Name"
        cboFromCity.DataBind()

        cboFromCity.Items.Insert(0, New ListItem("Select", "0"))

        cboToCity.DataSource = objCity.GetCities() '.get.GetRoute()
        cboToCity.DataValueField = "City_Id"
        cboToCity.DataTextField = "City_Name"
        cboToCity.DataBind()

        cboToCity.Items.Insert(0, New ListItem("Select", "0"))

        cboFromCity.SelectedValue = dtSystemInfo.Rows(0)("Source_City_Id")

    End Sub

    Private Sub loadTable()
        Dim count = 0
        Dim dtVoucher As New DataTable
        Dim dtVoucher_Online As New DataTable
        Dim dtVouchertemp As New DataTable
        objOnlineTicketing = New eTicketing

        dtVoucher = objTicketing.GetTicketingScheduleCityWise(cboFromCity.SelectedValue, cboToCity.SelectedValue, dtSchedule.Value, 0, objUser.Vendor_Id)


        If Not dtVoucher Is Nothing Then

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
        End If


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


        Dim Counter As Integer = 0
        If "" & Request.QueryString("mode") <> "" Then
            mode = Request.QueryString("mode")
        Else
            mode = "1"
        End If
        tbSearch.Rows.Clear()

        If table Is Nothing Then

            Dim htmlCell_Load As New HtmlTableCell()
            htmlCell_Load.ID = "cell_a_1" & Counter
            htmlCell_Load.Attributes.Add("class", "tbThirdRow")
            Dim tblRow As HtmlTableRow
            tblRow = New HtmlTableRow()


            htmlCell_Load.InnerHtml = " <span class='dep_time_Load' > Not Record Found ! </span> "
            tblRow.Cells.Insert(0, htmlCell_Load)

            tbSearch.Rows.Add(tblRow)

        Else

            For Each dr As DataRow In table.Rows
                Dim tblRow As HtmlTableRow
                tblRow = New HtmlTableRow()

                Counter = Counter + 1



                Dim htmlCell_Load As New HtmlTableCell()
                htmlCell_Load.ID = "cell_a_1" & Counter
                htmlCell_Load.Attributes.Add("class", "tbThirdRow")
              
                Dim Departure_Time As String = dr("Departure_Time").ToString().Substring(0, 2) & ":" & dr("Departure_Time").ToString().Substring(2)
                Dim Departure_Time2 As String = "0"


                'htmlCell_Load.InnerHtml = " <span class='dep_time_Load' > <a  class='dep_time_Load' href='#' onclick='LoadSchedule(" + dr("Ticketing_Schedule_ID").ToString() + " ," + IIf(dr("Sr_No").ToString() = "", "''", dr("Sr_No").ToString()) + " ," + "'" + dr("Departure_Time") + "'" + " ," + IIf(dr("Vehicle_ID").ToString() = "", "0", dr("Vehicle_ID").ToString()) + " ," + IIf(dr("Driver_Name").ToString() = "", "0", dr("Driver_Name").ToString()) + " ," + IIf(dr("Hostess_Name").ToString(), "0", dr("Hostess_Name").ToString()) + "  )' > Load </span> "
                htmlCell_Load.InnerHtml = " <span class='dep_time_Load' > <a  class='dep_time_Load' href='#' onclick='LoadSchedule(" + dr("Ticketing_Schedule_ID").ToString() + " ," + IIf(dr("Sr_No").ToString() = "", "''", dr("Sr_No").ToString()) + " ," + Departure_Time2 + " ," + IIf(dr("Vehicle_ID").ToString() = "", "0", dr("Vehicle_ID").ToString()) + " ," + IIf(dr("Driver_Name").ToString() = "", "0", dr("Driver_Name").ToString()) + " ," + IIf(dr("Hostess_Name").ToString() = "", "0", dr("Hostess_Name").ToString()) + " ," + dr("Schedule_Id").ToString() + ")' > Load </span> "
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


        End If



    End Sub

    Private Sub RegisterClientEvents()
        'btnSave.Attributes.Add("onclick", "return validation();")
        'btnSave.Style.Add("display", "none")
    End Sub


#End Region

    'Protected Sub cboServiceType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboServiceType.SelectedIndexChanged
    '    If cboFromCity.SelectedValue <> "0" And cboToCity.SelectedValue <> "0" Then
    '        LoadDataOnline()
    '        '  objTicketing.CreateTicketingScheduleCityWise(cboFromCity.SelectedValue, cboFromCity.SelectedValue, dtSchedule.Value, "")
    '        loadTable()
    '        Me.BindTicketingRoute()

    '    End If
    'End Sub
End Class