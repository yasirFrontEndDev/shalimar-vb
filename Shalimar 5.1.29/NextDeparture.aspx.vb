Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online
Imports Infragistics.WebUI.UltraWebGrid

Partial Public Class NextDeparture
    Inherits System.Web.UI.Page
    Dim objTicketing As clsSeatTicketing

    Dim objServiceType As clsServiceType

    Dim objConnection As Object
    Dim objOnline As eTicketing
    Dim objUser As clsUser
    Dim MainDt As New DataTable
    Dim objScheduleList As clsSchedules
    Dim objTicketingSchedule As clsTicketing


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objUser = CType(Session("CurrentUser"), clsUser)
        If objUser.Is_NextDeparture = False Then
            Response.Redirect("Permissions.aspx")

        Else

        End If

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If
        If Not Page.IsPostBack Then
            objConnection = ConnectionManager.GetConnection()


            'objScheduleList
            objScheduleList = New clsSchedules(objConnection)
            objServiceType = New clsServiceType(objConnection)
            objTicketingSchedule = New clsTicketing(objConnection)


            cboServiceType.DataSource = objServiceType.GetServiceTypes() '.get.GetRoute()
            cboServiceType.DataValueField = "ServiceType_Id"
            cboServiceType.DataTextField = "ServiceType_Name"
            cboServiceType.DataBind()

            cboServiceType.Items.Insert(0, New ListItem("Select", "0"))

            InitiateLoad_Form_Load()
        End If

        If Not Me.IsPostBack Then
            loadCombos()

        End If

    End Sub

    Private Sub UserLogin_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        ConnectionManager.CloseConnection(objConnection)
    End Sub

    Private Sub loadTable()
        Try


            Dim count = 0
            Dim dtVoucher As New DataTable
            Dim dtVouchertemp As New DataTable

            'dtVouchertemp = objTicketing.GetTicketingSchedule2(cboRoute.SelectedValue, dtSchedule.Text)
            objTicketingSchedule = New clsTicketing(objConnection)

            dtVouchertemp = objTicketingSchedule.GetTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, cboServiceType.SelectedValue)


            cboTime.Items.Clear()

            cboTime.DataSource = dtVouchertemp

            cboTime.DataValueField = "Ticketing_Schedule_ID"
            cboTime.DataTextField = "Departure_Time"
            cboTime.DataBind()

            cboTime.Items.Insert(0, New ListItem("Select", "0"))

        Catch ex As Exception
            Response.Write(ex.Message)

        End Try


    End Sub
    Private Sub dtSchedule_ValueChanged(ByVal sender As Object, ByVal e As Infragistics.WebUI.WebSchedule.WebDateChooser.WebDateChooserEventArgs) Handles dtSchedule.ValueChanged
        If cboRoute.SelectedValue <> "0" Then
            objConnection = ConnectionManager.GetConnection()     
            objTicketingSchedule = New clsTicketing(objConnection)      
            objTicketingSchedule.CreateTicketingSchedule2(cboRoute.SelectedValue, dtSchedule.Value, "")
            loadTable()
            iframe1.Attributes("src") = "blank.aspx"
        End If
    End Sub

    Private Sub cboRoute_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboRoute.SelectedIndexChanged
        If cboRoute.SelectedValue <> "0" Then
            objConnection = ConnectionManager.GetConnection()
            objTicketingSchedule = New clsTicketing(objConnection)

            objTicketingSchedule.CreateTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, "")
            loadTable()



            iframe1.Attributes("src") = "blank.aspx"


        End If
    End Sub


    Private Sub InitiateLoad_Form_Load()
        Try


            MainDt.Clear()

            MainDt.Columns.Add("Ticketing_Schedule_ID", Type.GetType("System.String"))
            MainDt.Columns.Add("Ticketing_Seat_ID", Type.GetType("System.String"))
            MainDt.Columns.Add("Seat_No", Type.GetType("System.String"))
            MainDt.Columns.Add("Status", Type.GetType("System.String"))
            MainDt.Columns.Add("AmountChange", Type.GetType("System.String"))
            MainDt.Columns.Add("AmountRefund", Type.GetType("System.String"))
            MainDt.Columns.Add("Source", Type.GetType("System.String"))
            MainDt.Columns.Add("Destination", Type.GetType("System.String"))
            MainDt.Columns.Add("Schedule_Title", Type.GetType("System.String"))
            MainDt.Columns.Add("Dep_Time", Type.GetType("System.String"))
            MainDt.Columns.Add("Passenger_Name", Type.GetType("System.String"))
            MainDt.Columns.Add("Contact_No", Type.GetType("System.String"))
            MainDt.Columns.Add("CNIC", Type.GetType("System.String"))
            grdRoutes.DataSource = MainDt
            grdRoutes.DataBind()

        Catch ex As Exception
            lblMessage.Text = "Please contact IT Team ------Error" + ex.Message


        End Try

    End Sub

    Private Function ValidateTicket() As Boolean

        Dim objTicketingChange As clsSeatTicketing

        objConnection = ConnectionManager.GetConnection()
        objTicketingChange = New clsSeatTicketing(objConnection)
        objUser = CType(Session("CurrentUser"), clsUser)

        Dim dt As New DataTable


        If rdoTicketTracking.SelectedValue = "Bookkaru" Then


            dt = objTicketingChange.getSeatRecordForBookkaru(txtTicketNumber.Text, "NextDeparture")
            If Not dt Is Nothing Then


                If dt.Rows.Count > 0 Then
                    If dt.Rows(0)("Status") <> "4" Then

                        lblMessage.Text = "No Record Found."
                        Return False
                    Else
                        cboRoute.SelectedValue = dt.Rows(0)("Schedule_ID")
                        lblMessage.Text = ""

                        If dt.Rows(0)("Terminal_id") = 86 Then

                            lblMessage.Text = "We found your ticket sucessfully. Bookkaru Next Departure amount will be charged " & dt.Rows(0)("Bookkaru_Next_Departure")
                            objTicketingSchedule = New clsTicketing(objConnection)
                            objTicketingSchedule.CreateTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, "")
                            hndDestinationId.Value = dt.Rows(0)("DestinationId")
                            hndoldTicketing_SeatId.Value = dt.Rows(0)("Ticketing_Seat_Id")

                            hndNDFare.Value = dt.Rows(0)("Bookkaru_Next_Departure")
                            If cboTime.Items.Count <= 0 Then
                                loadTable()
                            End If



                            iframe1.Attributes("src") = "blank.aspx"

                            Return True

                        Else
                            lblMessage.Text = "We found your ticket sucessfully. Change amount will be charged " & dt.Rows(0)("NextDeparture")
                            objTicketingSchedule = New clsTicketing(objConnection)
                            objTicketingSchedule.CreateTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, "")
                            hndDestinationId.Value = dt.Rows(0)("DestinationId")
                            hndoldTicketing_SeatId.Value = dt.Rows(0)("Ticketing_Seat_Id")

                            hndNDFare.Value = dt.Rows(0)("NextDeparture")
                            If cboTime.Items.Count <= 0 Then
                                loadTable()
                            End If



                            iframe1.Attributes("src") = "blank.aspx"

                            Return True

                        End If



                        lblMessage.Text = "We found your ticket sucessfully. Change amount will be charged " & dt.Rows(0)("NextDeparture")
                        objTicketingSchedule = New clsTicketing(objConnection)
                        objTicketingSchedule.CreateTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, "")
                        hndDestinationId.Value = dt.Rows(0)("DestinationId")
                        hndoldTicketing_SeatId.Value = dt.Rows(0)("Ticketing_Seat_Id")

                        hndNDFare.Value = dt.Rows(0)("NextDeparture")
                        If cboTime.Items.Count <= 0 Then
                            loadTable()
                        End If



                        iframe1.Attributes("src") = "blank.aspx"

                        Return True


                    End If
                Else
                    lblMessage.Text = "No Record Found."
                    Return False

                End If
            Else
                lblMessage.Text = "No Record Found."
                Return False

            End If

        Else

            dt = objTicketingChange.getSeatRecord(txtTicketNumber.Text, "NextDeparture")
            If Not dt Is Nothing Then


                If dt.Rows.Count > 0 Then
                    If dt.Rows(0)("Status") <> "4" Then

                        lblMessage.Text = "No Record Found."
                        Return False
                    Else
                        cboRoute.SelectedValue = dt.Rows(0)("Schedule_ID")
                        lblMessage.Text = ""

                        If dt.Rows(0)("Terminal_id") = 86 Then

                            lblMessage.Text = "We found your ticket sucessfully. Bookkaru Next Departure amount will be charged " & dt.Rows(0)("Bookkaru_Next_Departure")
                            objTicketingSchedule = New clsTicketing(objConnection)
                            objTicketingSchedule.CreateTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, "")
                            hndDestinationId.Value = dt.Rows(0)("DestinationId")
                            hndoldTicketing_SeatId.Value = dt.Rows(0)("Ticketing_Seat_Id")

                            hndNDFare.Value = dt.Rows(0)("Bookkaru_Next_Departure")
                            If cboTime.Items.Count <= 0 Then
                                loadTable()
                            End If



                            iframe1.Attributes("src") = "blank.aspx"

                            Return True

                        Else
                            lblMessage.Text = "We found your ticket sucessfully. Change amount will be charged " & dt.Rows(0)("NextDeparture")
                            objTicketingSchedule = New clsTicketing(objConnection)
                            objTicketingSchedule.CreateTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, "")
                            hndDestinationId.Value = dt.Rows(0)("DestinationId")
                            hndoldTicketing_SeatId.Value = dt.Rows(0)("Ticketing_Seat_Id")

                            hndNDFare.Value = dt.Rows(0)("NextDeparture")
                            If cboTime.Items.Count <= 0 Then
                                loadTable()
                            End If



                            iframe1.Attributes("src") = "blank.aspx"

                            Return True

                        End If



                        lblMessage.Text = "We found your ticket sucessfully. Change amount will be charged " & dt.Rows(0)("NextDeparture")
                        objTicketingSchedule = New clsTicketing(objConnection)
                        objTicketingSchedule.CreateTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, "")
                        hndDestinationId.Value = dt.Rows(0)("DestinationId")
                        hndoldTicketing_SeatId.Value = dt.Rows(0)("Ticketing_Seat_Id")

                        hndNDFare.Value = dt.Rows(0)("NextDeparture")
                        If cboTime.Items.Count <= 0 Then
                            loadTable()
                        End If



                        iframe1.Attributes("src") = "blank.aspx"

                        Return True


                    End If
                Else
                    lblMessage.Text = "No Record Found."
                    Return False

                End If
            Else
                lblMessage.Text = "No Record Found."
                Return False

            End If

        End If







    End Function
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click

        Button1.Enabled = False

 
        objUser = CType(Session("CurrentUser"), clsUser)

        objConnection = ConnectionManager.GetConnection()
        objTicketingSchedule = New clsTicketing(objConnection)


        objTicketingSchedule.UserId = objUser.Id
        objTicketingSchedule.ComputerName = "System"
        objTicketingSchedule.AccessTerminalId = 1
        Dim DepartureTime As String = cboTime.SelectedItem.Text
        Dim Arr As Array = DepartureTime.Split("-")


        With objTicketingSchedule
            .Id = cboTime.SelectedValue

            If (Not IsDBNull(.Id)) AndAlso .Id <> 0 Then
                .GetById()
            End If

            .ScheduleID = cboRoute.SelectedValue

            .SerialNo = Val("" & SerialNo.Value)
            If Arr.Length = 1 Then
                .DepartureTime = cboTime.SelectedItem.Text
            Else
                .DepartureTime = Arr(0)
            End If
            '.DepartureTime = cboTime.SelectedItem.Text

            If Vehicle_ID.Value <> "" Or Val("" & Vehicle_ID.Value) <> 0 Then
                .VehicleID = Vehicle_ID.Value
            Else
                .VehicleID = 81
            End If

            .DriverName = Driver_Name.Value
            .HostessName = Hostess_Name.Value

        End With

        objTicketingSchedule.Save(False)

        iframe1.Attributes("src") = "Ticketing.aspx?mode=1&TSID=" & cboTime.SelectedValue & "&TicketNumber=" & txtTicketNumber.Text & "&DestinationId=" & hndDestinationId.Value & "&Fare=" & hndNDFare.Value & "&oldTicketing_SeatId=" & hndoldTicketing_SeatId.Value

        Button1.Enabled = True

    End Sub

    Private Sub loadCombos()

        cboRoute.DataSource = objScheduleList.GetAll() '.get.GetRoute()

        cboRoute.DataValueField = "Schedule_Id"
        cboRoute.DataTextField = "Schedule_Title"
        cboRoute.DataBind()

        cboRoute.Items.Insert(0, New ListItem("Select", "0"))




    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        objConnection = ConnectionManager.GetConnection()
        objTicketing = New clsSeatTicketing(objConnection)
        objUser = CType(Session("CurrentUser"), clsUser)

        Dim dt As New DataTable
        lblMessage.Text = ""

        If Not ValidateTicket() Then
            Exit Sub

        End If

        If rdoTicketTracking.SelectedValue = "Bookkaru" Then
            For Each line As String In Me.txtTicketNumber.Text.Split(vbLf)

                dt = objTicketing.getSeatRecordForBookkaru(line.Trim(), "NextDeparture")

                If Not dt Is Nothing Then
                    ' MainDt = dt.Copy()
                    MainDt.Merge(dt)
                Else
                    MainDt = Nothing


                End If
                If Not dt Is Nothing Then
                    If dt.Rows.Count > 0 Then
                        If dt.Rows(0)("Status") <> "4" Then
                            lblMessage.Text = "No Record Found. For ticket number  " & line.ToString()
                            lblMessage.ForeColor = Drawing.Color.Red
                            Exit Sub
                        Else
                        End If
                    Else
                        lblMessage.Text = "No Record Found. For ticket number  " & line.ToString()
                        lblMessage.ForeColor = Drawing.Color.Red

                        Exit Sub
                    End If
                Else
                    lblMessage.Text = "No Record Found. For ticket number  " & line.ToString()
                    lblMessage.ForeColor = Drawing.Color.Red

                    Exit Sub
                End If
            Next
        Else
            For Each line As String In Me.txtTicketNumber.Text.Split(vbLf)

                dt = objTicketing.getSeatRecord(line.Trim(), "NextDeparture")

                If Not dt Is Nothing Then
                    ' MainDt = dt.Copy()
                    MainDt.Merge(dt)
                Else
                    MainDt = Nothing


                End If
                If Not dt Is Nothing Then
                    If dt.Rows.Count > 0 Then
                        If dt.Rows(0)("Status") <> "4" Then
                            lblMessage.Text = "No Record Found. For ticket number  " & line.ToString()
                            lblMessage.ForeColor = Drawing.Color.Red
                            Exit Sub
                        Else
                        End If
                    Else
                        lblMessage.Text = "No Record Found. For ticket number  " & line.ToString()
                        lblMessage.ForeColor = Drawing.Color.Red

                        Exit Sub
                    End If
                Else
                    lblMessage.Text = "No Record Found. For ticket number  " & line.ToString()
                    lblMessage.ForeColor = Drawing.Color.Red

                    Exit Sub
                End If
            Next
        End If




        cboRoute.SelectedValue = dt.Rows(0)("Schedule_ID")
        cboServiceType.SelectedValue = dt.Rows(0)("ServiceType_Id")

        lblMessage.Text = ""

        If dt.Rows(0)("Terminal_id") = 86 Then
            lblMessage.Text = "We found your ticket sucessfully. Bookkaru Next Departure amount will be charged " & dt.Rows(0)("Bookkaru_Next_Departure")
        Else
            lblMessage.Text = "We found your ticket sucessfully. Change amount will be charged " & dt.Rows(0)("NextDeparture")
        End If


        'objTicketingSchedule.CreateTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, "")

        loadTable()

        iframe1.Attributes("src") = "blank.aspx"

        grdRoutes.DataSource = MainDt
        grdRoutes.DataBind()

        '  btnSave.Visible = True
    End Sub

    Protected Sub cboServiceType_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboServiceType.TextChanged
        If cboServiceType.SelectedValue <> "0" Then
            objConnection = ConnectionManager.GetConnection()
            objTicketingSchedule = New clsTicketing(objConnection)
            objTicketingSchedule.CreateTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, "")
            loadTable()

            iframe1.Attributes("src") = "blank.aspx"
        End If
    End Sub

End Class