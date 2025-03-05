Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid

Partial Public Class Schedules
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objSchedules As clsSchedules
    Dim objSchedule As clsSchedule
    Private table As DataTable

#Region " Form Events "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If

        objConnection = ConnectionManager.GetConnection()
        objUser = CType(Session("CurrentUser"), clsUser)
        objSchedules = New clsSchedules(objConnection)
        objSchedule = New clsSchedule(objConnection)
        If Not Page.IsPostBack Then
            Me.BindGrid()
        End If

    End Sub

    Private Sub UserLogin_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        ConnectionManager.CloseConnection(objConnection)
    End Sub

#End Region

#Region " Control Events "

    Private Sub grdScheules_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles grdScheules.InitializeLayout
        CustomizeControl.SetGridLayout(e.Layout.Grid)
        With e.Layout.Grid

            .Columns.FromKey("Schedule_Id").Hidden = True
            .Columns.FromKey("Active").Hidden = True
            .Columns.FromKey("Route_id").Hidden = True
            .Columns.FromKey("User_Id").Hidden = True
            .Columns.FromKey("Access_DateTime").Hidden = True
            .Columns.FromKey("Access_SysName").Hidden = True
            .Columns.FromKey("Access_Terminal_Id").Hidden = True
            .Columns.FromKey("Schedule_Code").Hidden = True

            .Columns.FromKey("Schedule_Code").Header.Caption = "Code"
            .Columns.FromKey("Schedule_Title").Header.Caption = "Schedule Title"
            .Columns.FromKey("Sch_Date").Header.Caption = "Date"
            .Columns.FromKey("Sch_wef").Header.Caption = "Schedule w.e.f."
            .Columns.FromKey("Route_Name").Header.Caption = "Route"

            .Columns.FromKey("Schedule_Code").Width = Unit.Percentage(15)
            .Columns.FromKey("Schedule_Title").Width = Unit.Percentage(35)
            .Columns.FromKey("Sch_Date").Width = Unit.Percentage(15)
            .Columns.FromKey("Sch_wef").Width = Unit.Percentage(15)
            .Columns.FromKey("Route_Name").Width = Unit.Percentage(20)

        End With
    End Sub

    Private Sub btnAdd_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.ServerClick
        Response.Redirect("Schedule_detail_new.aspx?AddNew=1")
    End Sub

    Private Sub btnModify_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnModify.ServerClick
        'If grdScheules.Rows.Count > 1 Then
        'If Not grdRoutes.DisplayLayout.ActiveRow Is Nothing Then
        'If grdScheules.DisplayLayout.SelectedRows.Count > 0 Then
        'Dim SchID As Integer = grdScheules.DisplayLayout.SelectedRows.Item(0).Cells.FromKey("Schedule_Id").Value 'grdRoutes.DisplayLayout.ActiveRow.Cells.FromKey("Route_ID").Value
        'Dim RouteCode As String = grdScheules.DisplayLayout.SelectedRows.Item(0).Cells(2).Value
        'Dim RouteName As String = grdScheules.DisplayLayout.SelectedRows.Item(0).Cells(3).Value
        Response.Redirect("Schedule_detail_new.aspx?SchID=" & hidSchID.Value.Trim())
        'End If
        'End If
    End Sub

    Private Sub btnDelete_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.ServerClick
        'If grdScheules.Rows.Count > 1 Then
        '    If Not grdScheules.DisplayLayout.ActiveRow Is Nothing Then
        objSchedule.Id = hidSchID.Value.Trim  'grdScheules.DisplayLayout.ActiveRow.Cells.FromKey("Schedule_ID").Value
        objSchedule.Delete()
        hidSchID.Value = "0"
        Me.BindGrid()
        '    End If
        'End If
    End Sub

#End Region

#Region " Functions And Procedure  "

    Private Sub BindGrid()
        Dim dtSchedules As DataTable
        dtSchedules = objSchedules.GetAll()
        grdScheules.DataSource = dtSchedules
        grdScheules.DataBind()

    End Sub

#End Region

End Class