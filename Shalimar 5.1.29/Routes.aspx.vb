Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid

Partial Public Class Routes
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objRoutes As clsRoute
    Private table As DataTable

#Region " Form Events "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If

        objConnection = ConnectionManager.GetConnection()
        objUser = CType(Session("CurrentUser"), clsUser)
        objRoutes = New clsRoute(objConnection)

        Call RegisterClientEvents()

        If Not Me.IsPostBack Then
            Call loadTable()
        End If

    End Sub

    Private Sub UserLogin_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        ConnectionManager.CloseConnection(objConnection)
    End Sub

#End Region

#Region " Control Events "

    Private Sub grdRoutes_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles grdRoutes.InitializeLayout
        CustomizeControl.SetGridLayout(e.Layout.Grid)
        With e.Layout.Grid

            .Columns.FromKey("Route_Id").Hidden = True
            .Columns.FromKey("Active").Hidden = True
            .Columns.FromKey("User_Id").Hidden = True
            .Columns.FromKey("Access_DateTime").Hidden = True
            .Columns.FromKey("Access_Sys_Name").Hidden = True
            .Columns.FromKey("Access_Terminal_Id").Hidden = True
            .Columns.FromKey("Route_Code").Hidden = True

            .Columns.FromKey("Route_Code").Header.Caption = "Route Code"
            .Columns.FromKey("Route_Name").Header.Caption = "Route Name"

            .Columns.FromKey("Route_Code").Width = Unit.Percentage(30)
            .Columns.FromKey("Route_Name").Width = Unit.Percentage(90)

        End With
    End Sub

    Private Sub btnAdd_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.ServerClick
        Response.Redirect("RoutesDetail.aspx")
    End Sub

    Private Sub btnModify_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnModify.Click
        Response.Redirect("RoutesDetail.aspx?RouteID=" & hidRouteID.Value & "&RouteCode=" & hidRouteCode.Value & "&RouteName=" & hidRouteName.Value)
    End Sub

    Private Sub btnDelete_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        objRoutes.Id = hidRouteID.Value
        objRoutes.Delete()
        Call loadTable()
    End Sub

#End Region

#Region " Functions And Procedure  "

    Private Sub loadTable()
        Dim count = 0
        Dim dtRoute As DataTable

        dtRoute = objRoutes.GetRoute()
        grdRoutes.DataSource = dtRoute
        grdRoutes.DataBind()
    End Sub

    Private Sub RegisterClientEvents()
        btnModify.Attributes.Add("onclick", "return modifyRoute_Click();")
        btnDelete.Attributes.Add("onclick", "return deleteRoute_Click();")
    End Sub

#End Region

End Class