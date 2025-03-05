Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid

Partial Public Class ExcludeTerminals
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objExcludeTerminals As clsExcludeTerminals
    Dim objSchedule As clsSchedule
    Private table As DataTable

#Region " Form Events "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        'If Session("CurrentUser") Is Nothing Then
        '    Response.Redirect("UserLogin.aspx")
        'End If

        objConnection = ConnectionManager.GetConnection()
        objUser = CType(Session("CurrentUser"), clsUser)
        objExcludeTerminals = New clsExcludeTerminals(objConnection)
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

            .Columns.FromKey("ID").Hidden = True
 

            .Columns.FromKey("FromCity").Header.Caption = "From City"
            .Columns.FromKey("ToCity").Header.Caption = "To City"
 

            .Columns.FromKey("FromCity").Width = Unit.Percentage(50)
            .Columns.FromKey("ToCity").Width = Unit.Percentage(50)

        End With
    End Sub

    Private Sub btnAdd_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.ServerClick
        Response.Redirect("Exclude_Terminal_Detail.aspx?AddNew=1")
    End Sub

    Private Sub btnModify_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnModify.ServerClick
        'If grdScheules.Rows.Count > 1 Then
        'If Not grdRoutes.DisplayLayout.ActiveRow Is Nothing Then
        'If grdScheules.DisplayLayout.SelectedRows.Count > 0 Then
        'Dim SchID As Integer = grdScheules.DisplayLayout.SelectedRows.Item(0).Cells.FromKey("Schedule_Id").Value 'grdRoutes.DisplayLayout.ActiveRow.Cells.FromKey("Route_ID").Value
        'Dim RouteCode As String = grdScheules.DisplayLayout.SelectedRows.Item(0).Cells(2).Value
        'Dim RouteName As String = grdScheules.DisplayLayout.SelectedRows.Item(0).Cells(3).Value
        Response.Redirect("Exclude_Terminal_Detail.aspx?SchID=" & hidSchID.Value.Trim())
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
        dtSchedules = objExcludeTerminals.GetAll()
        grdScheules.DataSource = dtSchedules
        grdScheules.DataBind()

    End Sub

#End Region

End Class