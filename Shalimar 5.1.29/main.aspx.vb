Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid

Partial Public Class main1
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objScheduleList As clsSchedules
    Dim objTicketing As clsTicketing
    Private table As DataTable
    Dim dtVehicle As DataTable
    Dim mode As String

    Dim objServiceType As clsServiceType

#Region " Form Events "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load




        objUser = CType(Session("CurrentUser"), clsUser)
        If objUser.Is_CurrentTicketing = False Then
            Response.Redirect("Permissions.aspx")

        Else


        End If


        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If


        objConnection = ConnectionManager.GetConnection()
        objUser = CType(Session("CurrentUser"), clsUser)

        objServiceType = New clsServiceType(objConnection)
        objScheduleList = New clsSchedules(objConnection)

        objTicketing = New clsTicketing(objConnection)

        'dtVehicle = (New clsVehicle(objConnection)).GetAll()

        dtSchedule.CalendarLayout.Culture = clsUtil.GetDateChooserCulture()


        Call RegisterClientEvents()

        If Not Me.IsPostBack Then
            loadCombos()
            If cboRoute.Text <> "" Then
                Call BindTicketingRoute()
            End If
        End If

    End Sub

    Private Function GetdtOperatedStatus() As DataTable
        Dim objDbManager As IDBManager
        Dim objDataSet As DataSet
        objDbManager = DBManager.GetDatabaseManager()
        objDbManager.SetDBConnection(objConnection)
        Dim objDBParameters As New clsDBParameters
        'Session("TicketingScheduleId") = TicketingScheduleId

        objDBParameters.Parameters.Add(New clsDBParameter("@Terminal_Id", objUser.TerminalId, "int"))
        objDataSet = objDbManager.GetData("GetOperatedCompany", objDBParameters)
        If Not objDataSet Is Nothing Then
            Return objDataSet.Tables(0)
        Else
            Return Nothing
        End If
    End Function

    Private Sub UserLogin_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        ConnectionManager.CloseConnection(objConnection)
    End Sub

#End Region

#Region " Control Events "



#End Region

#Region " Functions And Procedure  "

    Private Sub loadCombos()

        cboRoute.DataSource = objScheduleList.GetCurrentRoutes() '.get.GetRoute()

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


            ' Session("ServiceType") = cboServiceType.SelectedItem.Text

            Dim count = 0
            Dim dtVoucher As New DataTable
            Dim dtVouchertemp As New DataTable

            dtVouchertemp = objTicketing.GetTicketingSchedule2(cboRoute.SelectedValue, dtSchedule.Value, cboServiceType.SelectedValue)

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





    Private Sub loadOperatedByTable()
        Try


            ' Session("ServiceType") = cboServiceType.SelectedItem.Text
            Dim SchId = cboRoute.SelectedValue
            Dim tbOperatorName As New DataTable
            tbOperatorName = GetOperatorNameDetails()

            OperatedDropDownList.Items.Clear()

            OperatedDropDownList.DataSource = tbOperatorName

            OperatedDropDownList.DataValueField = "Operated_By"
            OperatedDropDownList.DataTextField = "Operated_By"
            OperatedDropDownList.DataBind()

            OperatedDropDownList.Items.Insert(0, New ListItem("Select", "0"))


        Catch ex As Exception
            Response.Write(ex.Message)

        End Try
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

    Private Sub dtSchedule_ValueChanged(ByVal sender As Object, ByVal e As Infragistics.WebUI.WebSchedule.WebDateChooser.WebDateChooserEventArgs) Handles dtSchedule.ValueChanged
        If cboRoute.SelectedValue <> "0" Then
            objTicketing.CreateTicketingSchedule2(cboRoute.SelectedValue, dtSchedule.Value, "")
            loadTable()
            Me.BindTicketingRoute()
            iframe1.Attributes("src") = "blank.aspx"

        End If
    End Sub

    Private Sub OperatedDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles OperatedDropDownList.SelectedIndexChanged
        If OperatedDropDownList.SelectedValue <> "0" Then


            Dim ManageCompany = OperatedDropDownList.SelectedValue


            Dim settings = System.Configuration.ConfigurationManager.AppSettings

            settings.Set("Operated_By", ManageCompany)



        End If
    End Sub

    Private Sub cboRoute_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboRoute.SelectedIndexChanged
        If cboRoute.SelectedValue <> "0" Then
            objTicketing.CreateTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, "")
            loadTable()
            'Dim dtOperated As New DataTable
            ' dtOperated = GetdtOperatedStatus()

            ' Dim row As DataRow = dtOperated.Rows(0)
            '  Dim AllowOperated = row("AllowOperatedCompany").ToString()

            If (objUser.AllowOperatedCompany = True) Then

                OperatedDropDownList.Visible = True
                lblOperated.Visible = True
                loadOperatedByTable()
            Else
                OperatedDropDownList.Visible = False
                lblOperated.Visible = False
            End If


            Me.BindTicketingRoute()



            iframe1.Attributes("src") = "blank.aspx"


        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click

        Button1.Enabled = False

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

        iframe1.Attributes("src") = "Ticketing.aspx?mode=1&TSID=" & cboTime.SelectedValue

        Button1.Enabled = True

        'For Each dRow As DataRow In dtVehicle.Rows

        '    If "" & dRow.Item("Ticketing_Schedule_ID") = cboTime.SelectedValue Then

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
        '                .VehicleID = 81
        '            End If

        '            .DriverName = "" & dRow.Item("Driver_Name")
        '            .HostessName = "" & dRow.Item("Hostess_Name")

        '        End With

        '        objTicketing.Save(False)
        '        Exit For
        '    End If
        'Next

        '///////////////////////////////////////////////////////////////////////////////////////




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

    Protected Sub cboServiceType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboServiceType.SelectedIndexChanged
        cboRoute_SelectedIndexChanged(sender, e)
    End Sub
End Class