Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid

Public Class BookkaruPrintReport
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
            loadCombos()
        End If
    End Sub

    Private Sub loadCombos()

        objConnection = ConnectionManager.GetConnection()
        objScheduleList = New clsSchedules(objConnection)
        cboRoute.DataSource = objScheduleList.GetAllUsers() '.get.GetRoute()

        cboRoute.DataValueField = "User_Id"
        cboRoute.DataTextField = "Comeplete"
        cboRoute.DataBind()
        cboRoute.Items.Insert(0, New ListItem("Select", "0"))

    End Sub

End Class