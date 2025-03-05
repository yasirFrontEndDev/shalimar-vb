Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity

Partial Public Class CommissionReportByDateAll
    Inherits System.Web.UI.Page


    Dim objConnection As Object
    Dim objScheduleList As clsSchedules
    Dim objVehicle As clsVehicle
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objConnection = ConnectionManager.GetConnection()

        dtFrom.CalendarLayout.Culture = FMovers.Ticketing.Entity.clsUtil.GetDateChooserCulture()
        dtTo.CalendarLayout.Culture = FMovers.Ticketing.Entity.clsUtil.GetDateChooserCulture()

        If Not IsPostBack Then
            Call getUsers()
        End If

    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        ConnectionManager.CloseConnection(objConnection)
    End Sub

    Private Sub getUsers()
        Dim dt As DataTable = clsUtil.GetAllUsers(objConnection)
        Dim dtVehilce As DataTable = clsUtil.GetAllUsers(objConnection)
        objScheduleList = New clsSchedules(objConnection)

        objVehicle = New clsVehicle(objConnection)


        If Not dt Is Nothing Then
            If dt.Rows.Count > 0 Then
                cboUser.DataSource = dt
                cboUser.DataValueField = "User_ID"
                cboUser.DataTextField = "User_Name"
                cboUser.DataBind()
                cboUser.Items.Insert("0", New ListItem("", "0"))
            End If
        End If

        objScheduleList = New clsSchedules(objConnection)
        cboRoute.DataSource = objScheduleList.GetAll() '.get.GetRoute()
        cboRoute.DataValueField = "Schedule_Id"
        cboRoute.DataTextField = "Schedule_Title"
        cboRoute.DataBind()
        cboRoute.Items.Insert(0, New ListItem("Select", "0"))


        objVehicle = New clsVehicle(objConnection)
        cboVehicle.DataSource = objVehicle.GetAll() '.get.GetRoute()
        cboVehicle.DataValueField = "Vehicle_Id"
        cboVehicle.DataTextField = "Registration_No"
        cboVehicle.DataBind()
        cboVehicle.Items.Insert(0, New ListItem("Select", "0"))


    End Sub

    Private Sub btnShowReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowReport.Click

        Session("FromDate") = dtFrom.Value
        Session("ToDate") = dtTo.Value
        Session("UserID") = cboUser.SelectedValue
        Response.Write("<script>window.open('Reports/CommissionReport.aspx')</script>")

    End Sub

End Class