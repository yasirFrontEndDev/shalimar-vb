Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid

Partial Public Class DepartureReportByDate
    Inherits System.Web.UI.Page
    Dim objConnection As Object
    Dim objScheduleList As clsSchedules
    Dim objServiceType As clsServiceType
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objConnection = ConnectionManager.GetConnection()

        dtFrom.CalendarLayout.Culture = FMovers.Ticketing.Entity.clsUtil.GetDateChooserCulture()
        dtTo.CalendarLayout.Culture = FMovers.Ticketing.Entity.clsUtil.GetDateChooserCulture()

        If Page.IsPostBack = False Then
            loadCombos()
        End If

    End Sub

    Private Sub loadCombos()

        objScheduleList = New clsSchedules(objConnection)
        objServiceType = New clsServiceType(objConnection)


        cboService.DataSource = objServiceType.GetServiceTypes() '.get.GetRoute()

        cboService.DataValueField = "ServiceType_Id"
        cboService.DataTextField = "ServiceType_Name"
        cboService.DataBind()
        cboService.Items.Insert(0, New ListItem("Select", "0"))


        cboRoute.DataSource = objScheduleList.GetAll() '.get.GetRoute()

        cboRoute.DataValueField = "Schedule_Id"
        cboRoute.DataTextField = "Schedule_Title"
        cboRoute.DataBind()
        cboRoute.Items.Insert(0, New ListItem("Select", "0"))

        'cboService


    End Sub
End Class