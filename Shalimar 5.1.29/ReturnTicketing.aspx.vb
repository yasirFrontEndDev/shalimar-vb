Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid


Partial Public Class ReturnTicketing
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objScheduleList As clsSchedules
    Dim objCity As clsCity

    Dim objTicketing As clsTicketing
    Private table As DataTable
    Dim dtVehicle As DataTable
    Dim mode As String


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If


        objConnection = ConnectionManager.GetConnection()
        objUser = CType(Session("CurrentUser"), clsUser)

        objScheduleList = New clsSchedules(objConnection)
        objCity = New clsCity(objConnection)

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

    Private Sub BindTicketingRoute()

    End Sub

    Private Sub loadTable()
        Try


            Dim count = 0
            Dim dtVoucher As New DataTable
            Dim dtVouchertemp As New DataTable

            dtVouchertemp = objTicketing.GetTicketingSchedule2(cboRoute.SelectedValue, dtSchedule.Text, 3)

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


    Private Sub loadCombos()

        cboCity.Items.Clear()


        cboRoute.DataSource = objScheduleList.GetAll() '.get.GetRoute()

        cboRoute.DataValueField = "Schedule_Id"
        cboRoute.DataTextField = "Schedule_Title"
        cboRoute.DataBind()

        cboRoute.Items.Insert(0, New ListItem("Select", "0"))


        cboCity.DataSource = objCity.GetCities() '.get.GetRoute()

        cboCity.DataValueField = "City_Id"
        cboCity.DataTextField = "City_Name"
        cboCity.DataBind()

        cboCity.Items.Insert(0, New ListItem("Select", "0"))




    End Sub



    Protected Sub cboRoute_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboRoute.SelectedIndexChanged
        If cboRoute.SelectedValue <> "0" Then
            objTicketing.CreateTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, "")
            loadTable()
            Me.BindTicketingRoute()

            iframe1.Attributes("src") = "blank.aspx"


        End If
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

    Protected Sub dtSchedule_ValueChanged(ByVal sender As Object, ByVal e As Infragistics.WebUI.WebSchedule.WebDateChooser.WebDateChooserEventArgs) Handles dtSchedule.ValueChanged
        If cboRoute.SelectedValue <> "0" Then

            objTicketing.CreateTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, "")
            loadTable()

            Me.BindTicketingRoute()
        End If
    End Sub

    Protected Sub cboCity_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboCity.SelectedIndexChanged

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click


        Button1.Enabled = False

        objTicketing.UserId = objUser.Id
        objTicketing.ComputerName = "System"
        objTicketing.AccessTerminalId = 1


        With objTicketing
            .Id = cboTime.SelectedValue

            If (Not IsDBNull(.Id)) AndAlso .Id <> 0 Then
                .GetById()
            End If

            .ScheduleID = cboRoute.SelectedValue

            .SerialNo = SerialNo.Value
            .DepartureTime = cboTime.SelectedItem.Text

            If Vehicle_ID.Value <> "" Or Val("" & Vehicle_ID.Value) <> 0 Then
                .VehicleID = Vehicle_ID.Value
            Else
                .VehicleID = 81
            End If

            .DriverName = Driver_Name.Value
            .HostessName = Hostess_Name.Value

        End With

        objTicketing.Save(False)

        iframe1.Attributes("src") = "Ticketing.aspx?mode=4&TSID=" & cboTime.SelectedValue & "&SCI=" & cboCity.SelectedValue


        Button1.Enabled = True



    End Sub
End Class