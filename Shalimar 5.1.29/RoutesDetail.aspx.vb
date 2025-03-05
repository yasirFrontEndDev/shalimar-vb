Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid

Partial Public Class RoutesDetail
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objRoute As clsRoute
    Dim objRouteDetail As clsRouteDetail
    Dim objValidate As clsValidate
    Private table As DataTable
    Dim RouteID As String
    Dim RouteCode As String
    Dim RouteName As String

#Region " Form Events "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If

        objConnection = ConnectionManager.GetConnection()
        objUser = CType(Session("CurrentUser"), clsUser)
        objRoute = New clsRoute(objConnection)
        objRouteDetail = New clsRouteDetail(objConnection)

        RouteID = "" & Request.QueryString("RouteID")
        RouteCode = "" & Request.QueryString("RouteCode")
        RouteName = "" & Request.QueryString("RouteName")

        If (RouteID = "" Or RouteID = "0") And "" & ViewState("RouteID") <> "" And "" & ViewState("RouteID") <> "0" Then
            RouteID = ViewState("RouteID")
        End If


        Call RegisterClientEvents()
        Call loadTable()

        If Not Me.IsPostBack Then
            txtRouteID.Text = RouteCode
            txtRouteName.Text = RouteName
            ViewState("RouteID") = RouteID
            Call BindRouteDetail()
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

            .Columns.FromKey("Route_Detail_ID").Hidden = True
            .Columns.FromKey("Route_ID").Hidden = True
            .Columns.FromKey("Fare").Hidden = False

            .Columns.FromKey("Sr_No").Header.Caption = "Sr #"
            .Columns.FromKey("City_ID").Header.Caption = "City"
            .Columns.FromKey("Travel_Time").Header.Caption = "Travel Time"
            .Columns.FromKey("Stay_Time").Header.Caption = "Stay Time"
            .Columns.FromKey("Is_TransitCity").Header.Caption = "Transit City"

            .Columns.FromKey("Sr_No").Width = Unit.Percentage(10)
            .Columns.FromKey("City_ID").Width = Unit.Percentage(40)
            .Columns.FromKey("Travel_Time").Width = Unit.Percentage(20)
            .Columns.FromKey("Stay_Time").Width = Unit.Percentage(20)
            .Columns.FromKey("Fare").Width = Unit.Percentage(10)


            .DisplayLayout.AllowUpdateDefault = Infragistics.WebUI.UltraWebGrid.AllowUpdate.Yes
            .DisplayLayout.CellClickActionDefault = Infragistics.WebUI.UltraWebGrid.CellClickAction.Edit

            Dim vList As New ValueList
            Dim dt As DataTable
            dt = (New clsCity(objConnection)).GetCities()

            For Each drow As DataRow In dt.Rows
                vList.ValueListItems.Add(drow.Item("City_Id"), drow.Item("City_Name"))
            Next

            .Columns.FromKey("City_Id").Type = ColumnType.DropDownList
            .Columns.FromKey("City_Id").ValueList = vList

        End With
    End Sub

    Private Sub grdRoutes_DeleteRowBatch(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles grdRoutes.DeleteRowBatch
        Dim trow As DataRow
        Dim pk = table.PrimaryKey(0).ColumnName
        Dim key As Object = e.Row.Cells.FromKey(pk).Value

        trow = table.Rows.Find(key)
        If Not trow Is Nothing Then
            trow.Delete()
        End If
    End Sub

    Private Sub grdRoutes_UpdateGrid(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.UpdateEventArgs) Handles grdRoutes.UpdateGrid
        Dim row As Infragistics.WebUI.UltraWebGrid.UltraGridRow
        Dim pk = table.PrimaryKey(0).ColumnName

        'create and get the rows enumeration for the changed rows
        Dim updatedRows As Infragistics.WebUI.UltraWebGrid.UltraGridRowsEnumerator
        updatedRows = e.Grid.Bands(0).GetBatchUpdates()

        'for each row in the Updated rows check if the current row is an addedrow, if so create the row and add it to the dataset
        While updatedRows.MoveNext
            row = updatedRows.Current

            Select Case row.DataChanged
                Case DataChanged.Added

                    Dim i As Integer
                    Dim addedRow As DataRow
                    addedRow = table.NewRow()
                    Dim isEmpty As Boolean = True

                    For i = 0 To row.Cells.Count - 1
                        If (row.Cells(i).Column.Key <> pk) Then
                            If (Not row.Cells(i).Value Is Nothing) Then
                                isEmpty = False
                                addedRow(i) = Trim(row.Cells(i).Value)
                            Else
                                If UCase(row.Cells(i).Column.Key) = "ROUTE_ID" Then
                                    addedRow(i) = 0
                                ElseIf UCase(row.Cells(i).Column.Key) = "FARE" Then
                                    addedRow(i) = 0
                                Else
                                    addedRow(i) = ""
                                End If
                            End If
                        End If
                    Next

                    If (Not isEmpty) Then
                        table.Rows.Add(addedRow)
                    End If

                Case DataChanged.Modified
                    Dim i As Integer
                    Dim ModifiedRow As DataRow
                    Dim key As Object = row.Cells.FromKey(pk).Value
                    ModifiedRow = table.Rows.Find(key)

                    If Not ModifiedRow Is Nothing Then
                        For i = 0 To row.Cells.Count - 1
                            If (row.Cells(i).Column.ToString <> pk) Then
                                If (Not row.Cells(i).Value Is Nothing) Then
                                    ModifiedRow(i) = Trim(row.Cells(i).Value)
                                Else
                                    If UCase(row.Cells(i).Column.Key) = "FARE" Then
                                        ModifiedRow(i) = 0
                                    Else
                                        ModifiedRow(i) = ""
                                    End If
                                End If
                            End If
                        Next
                    End If
            End Select

        End While
    End Sub

    Private Function ValidateCity(ByVal Route_Code As String, ByVal Route_Name As String) As Boolean

        'ValidateCity
        Dim ds As DataSet

        ds = objValidate.vaidateRoute(Route_Code, Route_Name)
        If Not ds Is Nothing Then

            For Each dt As DataTable In ds.Tables
                If Not dt Is Nothing Then
                    If dt.Rows.Count > 0 Then
                        Return True
                        Exit Function
                    End If
                End If

            Next
        End If

        Return False

    End Function
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

        objValidate = New clsValidate(objConnection)

        objRoute.Code = txtRouteID.Text
        objRoute.Name = txtRouteName.Text
        objRoute.UserId = objUser.Id
        objRoute.ComputerName = "System"
        objRoute.AccessTerminalId = 1

        If Not ViewState("RouteID") Is Nothing Then
            objRoute.Id = ViewState("RouteID")
        End If

        If objRoute.Id Is Nothing Or objRoute.Id = 0 Then
            If ValidateCity(txtRouteID.Text, txtRouteName.Text) = True Then
                lblError.Text = "Route Code " & txtRouteID.Text & " or route name " & txtRouteName.Text & " is duplicate. Please resolve."
            Else
                objRoute.Save(True)
                ViewState("RouteID") = objRoute.Id
            End If



        Else
            objRoute.Save(False)
        End If

        For Each dRow As DataRow In table.Rows
            If Not dRow.RowState = DataRowState.Unchanged Then
                If dRow.RowState = DataRowState.Deleted Then
                    objRouteDetail.Id = dRow.Item("Route_Detail_ID", DataRowVersion.Original)
                    clsUtil.DeleteRow(objConnection, "Route_Detail", objRouteDetail.Id)
                Else
                    With objRouteDetail
                        .RouteID = objRoute.Id
                        .SerialNo = dRow.Item("Sr_No")
                        .CityID = dRow.Item("City_ID")
                        .TravelTime = "" & dRow.Item("Travel_Time")
                        .StayTime = "" & dRow.Item("Stay_Time")
                        .TransitCity = "" & IIf(dRow.Item("Is_TransitCity") = True, 1, 0)
                    End With

                    If dRow.RowState = DataRowState.Added Then
                        objRouteDetail.Save(True)
                    ElseIf dRow.RowState = DataRowState.Modified Then
                        objRouteDetail.Id = "" & dRow.Item("Route_Detail_ID")
                        objRouteDetail.Save(False)
                    End If
                End If

            End If
        Next

        RouteID = objRoute.Id
        ViewState("RouteID") = objRoute.Id
        Call loadTable()
        Call BindRouteDetail()
    End Sub

    Private Sub btnSavenClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSavenClose.Click
        Call btnSave_Click(sender, e)
        Call btnClose_Click(sender, e)
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("Routes.aspx")
    End Sub

#End Region

#Region " Functions And Procedure  "

    Private Sub loadTable()
        Dim count = 0
        Dim dtRouteDetail As DataTable

        If RouteID = "" Then
            RouteID = "0"
        End If


        dtRouteDetail = objRouteDetail.GetRouteDetail(CInt(RouteID))

        Dim pk(0) As DataColumn
        pk(0) = dtRouteDetail.Columns("Route_Detail_ID")
        pk(0).AutoIncrement = True
        dtRouteDetail.PrimaryKey = pk
        dtRouteDetail.AcceptChanges()

        If (dtRouteDetail.Rows.Count > 0) Then
            pk(0).AutoIncrementSeed = dtRouteDetail.Rows(dtRouteDetail.Rows.Count - 1).Item("Route_Detail_ID") + 1
        Else
            pk(0).AutoIncrementSeed = 1
        End If

        table = dtRouteDetail
    End Sub

    Private Sub BindRouteDetail()
        grdRoutes.DataSource = table
        grdRoutes.DataBind()
    End Sub


    Private Sub RegisterClientEvents()
        btnSave.Attributes.Add("onclick", "return validation();")
        btnSavenClose.Attributes.Add("onclick", "return validation();")
    End Sub

#End Region

End Class