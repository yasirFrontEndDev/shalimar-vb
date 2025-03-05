Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid

Partial Public Class SC_New
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objScheduleList As clsSchedules

    Dim objServiceType As clsServiceType

    Dim objTicketing As clsTicketing
    Private table As DataTable
    Dim dtVehicle As DataTable
    Dim mode As String

#Region " Form Events "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        objUser = CType(Session("CurrentUser"), clsUser)
        If objUser.Is_TicketChange = False Then
            Response.Redirect("Permissions.aspx")

        Else

        End If

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If


        objConnection = ConnectionManager.GetConnection()


        objScheduleList = New clsSchedules(objConnection)
        objServiceType = New clsServiceType(objConnection)
        objTicketing = New clsTicketing(objConnection)

        dtVehicle = (New clsVehicle(objConnection)).GetAll()

        dtSchedule.CalendarLayout.Culture = clsUtil.GetDateChooserCulture()


        'Call RegisterClientEvents()

        If Not Me.IsPostBack Then
            loadCombos()
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
        Try


            Dim count = 0
            Dim dtVoucher As New DataTable
            Dim dtVouchertemp As New DataTable

            'dtVouchertemp = objTicketing.GetTicketingSchedule2(cboRoute.SelectedValue, dtSchedule.Text)

            dtVouchertemp = objTicketing.GetTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, cboServiceType.SelectedValue)


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

    Private Sub BindTicketingRoute()

    End Sub

    Private Sub RegisterClientEvents()
        'btnSave.Attributes.Add("onclick", "return validation();")
        'btnSave.Style.Add("display", "none")
    End Sub

#End Region




    Private Sub dtSchedule_ValueChanged(ByVal sender As Object, ByVal e As Infragistics.WebUI.WebSchedule.WebDateChooser.WebDateChooserEventArgs) Handles dtSchedule.ValueChanged
        If cboRoute.SelectedValue <> "0" Then
            objTicketing.CreateTicketingSchedule2(cboRoute.SelectedValue, dtSchedule.Value, "")
            loadTable()
            Me.BindTicketingRoute()
            iframe1.Attributes("src") = "blank.aspx"

        End If
    End Sub

    Private Sub cboRoute_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboRoute.SelectedIndexChanged
        If cboRoute.SelectedValue <> "0" Then
            objTicketing.CreateTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, "")
            loadTable()
            Me.BindTicketingRoute()

            iframe1.Attributes("src") = "blank.aspx"


        End If
    End Sub


    Private Function ValidateTicket() As Boolean

        Dim objTicketingChange As clsSeatTicketing

        objConnection = ConnectionManager.GetConnection()
        objTicketingChange = New clsSeatTicketing(objConnection)
        objUser = CType(Session("CurrentUser"), clsUser)

        Dim dt As New DataTable

        dt = objTicketingChange.getSeatRecord(txtTicketNumber.Text)
        If Not dt Is Nothing Then


            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("Status") <> "4" Then

                    lblMessage.Text = "No Record Found."
                    Return False
                Else
                    cboRoute.SelectedValue = dt.Rows(0)("Schedule_ID")
                    lblMessage.Text = ""
                    lblMessage.Text = "We found your ticket sucessfully. Change amount will be charged " & dt.Rows(0)("AmountChange")
                    objTicketing.CreateTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, "")

                    If cboTime.Items.Count <= 0 Then
                        loadTable()
                    End If

                    Me.BindTicketingRoute()

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




    End Function
    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click


        Button1.Enabled = False

        If Not ValidateTicket() Then
            Exit Sub

        End If

        objTicketing.UserId = objUser.Id
        objTicketing.ComputerName = "System"
        objTicketing.AccessTerminalId = 1


        Dim DepartureTime As String = cboTime.SelectedItem.Text
        Dim Arr As Array = DepartureTime.Split("-")


        With objTicketing
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

        objTicketing.Save(False)

        iframe1.Attributes("src") = "Ticketing.aspx?mode=1&TSID=" & cboTime.SelectedValue & "&TicketNumber=" & txtTicketNumber.Text

        Button1.Enabled = True

     



    End Sub

    Protected Sub cboTime_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboTime.SelectedIndexChanged
        If cboTime.SelectedValue <> "0" Then
            Dim newDT As DataTable
            objTicketing.Id = cboTime.SelectedItem.Value
            newDT = objTicketing.SP_GetVoucher_SINGLE()

            If newDT.Rows.Count > 0 Then

                SerialNo.Value = "" & newDT.Rows(0)(0)
                Driver_Name.Value = "" & newDT.Rows(0)(1)
                Hostess_Name.Value = "" & newDT.Rows(0)(2)
                Vehicle_ID.Value = "" & newDT.Rows(0)(3)

            End If


        End If
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        ValidateTicket()
    End Sub
    Protected Sub cboServiceType_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboServiceType.TextChanged
        If cboServiceType.SelectedValue <> "0" Then
            objTicketing.CreateTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, "")
            loadTable()
            Me.BindTicketingRoute()
            iframe1.Attributes("src") = "blank.aspx"
        End If
    End Sub
End Class