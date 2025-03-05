Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid


Partial Public Class CustomerInfo
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objRoutes As clsRoute
    Private table As DataTable


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If

        objConnection = ConnectionManager.GetConnection()
        objUser = CType(Session("CurrentUser"), clsUser)
        objRoutes = New clsRoute(objConnection)



        If Not Me.IsPostBack Then
            Call loadTable()
        End If

    End Sub

    Private Sub grdRoutes_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles grdCustomerInfo.InitializeLayout
        CustomizeControl.SetGridLayout(e.Layout.Grid)
        With e.Layout.Grid

        
            .Columns.FromKey("Cust_Code").Width = Unit.Percentage(5)
            .Columns.FromKey("First_Name").Width = Unit.Percentage(15)
            .Columns.FromKey("Last_Name").Width = Unit.Percentage(15)
            .Columns.FromKey("EmailAddress").Width = Unit.Percentage(15)
            .Columns.FromKey("CNIC").Width = Unit.Percentage(15)
            .Columns.FromKey("MobileNo").Width = Unit.Percentage(10)
            .Columns.FromKey("Address").Width = Unit.Percentage(30)

        End With
    End Sub

    Private Sub loadTable()


        Dim count = 0
        Dim dtRoute As DataTable

        dtRoute = objRoutes.GetCustomerInformation()
        grdCustomerInfo.DataSource = dtRoute
        grdCustomerInfo.DataBind()

    End Sub
End Class