Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid

Partial Public Class AdvanceReport
    Inherits System.Web.UI.Page
    Dim objScheduleList As clsSchedules
    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objTicketing As clsTicketing
    Private table As DataTable
    Dim dtVehicle As DataTable
    Dim mode As String



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        dtFrom.CalendarLayout.Culture = FMovers.Ticketing.Entity.clsUtil.GetDateChooserCulture()
        dtTo.CalendarLayout.Culture = FMovers.Ticketing.Entity.clsUtil.GetDateChooserCulture()

        If Page.IsPostBack = False Then
            If Not Request.QueryString("Type") Is Nothing Then
                hndType.Value = "Log"
            Else
                hndType.Value = ""
            End If
            loadCombos()
        End If

    End Sub
    Private Sub loadCombos()

        objConnection = ConnectionManager.GetConnection()
        objScheduleList = New clsSchedules(objConnection)
        cmbSource.DataSource = objScheduleList.GetAll() '.get.GetRoute()

        cmbSource.DataValueField = "Schedule_Id"
        cmbSource.DataTextField = "Schedule_Title"
        cmbSource.DataBind()
        cmbSource.Items.Insert(0, New ListItem("Select", "0"))

        objConnection = ConnectionManager.GetConnection()
        objScheduleList = New clsSchedules(objConnection)
        cboUsers.DataSource = objScheduleList.GetAllUsers() '.get.GetRoute()

        cboUsers.DataValueField = "User_Id"
        cboUsers.DataTextField = "Comeplete"
        cboUsers.DataBind()
        cboUsers.Items.Insert(0, New ListItem("Select", "0"))

    End Sub
End Class