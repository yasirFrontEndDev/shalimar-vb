Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid

Partial Public Class ServiceType
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objServiceType As clsServiceType
    Dim objValidate As clsValidate
    'clsValidate
    Private table As DataTable


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If
        objConnection = ConnectionManager.GetConnection()
        objUser = CType(Session("CurrentUser"), clsUser)
        objServiceType = New clsServiceType(objConnection)

        Me.RegisterClientEvents()

        Me.loadTable()

        If Not Me.IsPostBack Then
            Me.BindCities()
        End If

    End Sub

    Private Sub UserLogin_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        ConnectionManager.CloseConnection(objConnection)
    End Sub

    Private Sub grdCity_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles grdCity.InitializeLayout
        CustomizeControl.SetGridLayout(e.Layout.Grid)
        With e.Layout.Grid

            .Columns.FromKey("ServiceType_Id").Hidden = True
            .Columns.FromKey("ServiceType_Name").Header.Caption = "ServiceType"
            .DisplayLayout.AllowUpdateDefault = Infragistics.WebUI.UltraWebGrid.AllowUpdate.Yes
            .DisplayLayout.CellClickActionDefault = Infragistics.WebUI.UltraWebGrid.CellClickAction.Edit

        End With
    End Sub

    Private Sub grdCity_DeleteRowBatch(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles grdCity.DeleteRowBatch
        Dim trow As DataRow
        Dim pk = table.PrimaryKey(0).ColumnName
        Dim key As Object = e.Row.Cells.FromKey(pk).Value
        trow = table.Rows.Find(key)
        If Not trow Is Nothing Then
            trow.Delete()
        End If
    End Sub

    Private Sub grdCity_UpdateGrid(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.UpdateEventArgs) Handles grdCity.UpdateGrid
        Dim row As Infragistics.WebUI.UltraWebGrid.UltraGridRow
        Dim pk = table.PrimaryKey(0).ColumnName

        'create and get the rows enumeration for the changed rows
        Dim updatedRows As Infragistics.WebUI.UltraWebGrid.UltraGridRowsEnumerator
        updatedRows = e.Grid.Bands(0).GetBatchUpdates()
        'for each row in the Updated rows check if the current row is an addedrow, if so create the row and add it to the dataset
        While updatedRows.MoveNext
            row = updatedRows.Current
            If row.DataChanged = DataChanged.Added Then
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
                            'If UCase(row.Cells(i).Column.Key) = "USER_ID" Then
                            '    addedRow(i) = objUser.Id
                            'ElseIf UCase(row.Cells(i).Column.Key) = UCase("Access_DateTime") Then
                            '    addedRow(i) = DateTime.Now
                            'ElseIf UCase(row.Cells(i).Column.Key) = UCase("Access_Sys_Name") Then
                            '    addedRow(i) = "PC1"
                            'ElseIf UCase(row.Cells(i).Column.Key) = UCase("Access_Terminal_Id") Then
                            '    addedRow(i) = objUser.TerminalId
                            'ElseIf (UCase(row.Cells(i).Column.DataType.ToString) = UCase("System.Boolean")) Then
                            '    addedRow(i) = False
                            'ElseIf (UCase(row.Cells(i).Column.DataType.ToString) = UCase("System.Int32")) Or (UCase(row.Cells(i).Column.DataType.ToString) = UCase("System.Int64")) Then
                            '    addedRow(i) = 0
                            'Else
                            '    addedRow(i) = ""
                            'End If
                        End If
                    End If
                Next
                If (Not isEmpty) Then
                    table.Rows.Add(addedRow)
                End If
            End If

            If row.DataChanged = DataChanged.Modified Then
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
                                If (UCase(row.Cells(i).Column.DataType.ToString) = UCase("System.Boolean")) Then
                                    ModifiedRow(i) = False
                                ElseIf (UCase(row.Cells(i).Column.DataType.ToString) = UCase("System.Int32")) Then
                                    ModifiedRow(i) = 0
                                Else
                                    ModifiedRow(i) = ""
                                End If
                            End If
                        End If
                    Next
                End If
            End If
        End While
    End Sub

    Private Sub loadTable()
        Dim count = 0
        Dim dtCities As DataTable

        Dim getMaxServiceType_Id As Integer = 0

        getMaxServiceType_Id = objServiceType.getMaxServiceType_Id()
        dtCities = objServiceType.GetServiceTypes()

        Dim pk(0) As DataColumn
        pk(0) = dtCities.Columns("ServiceType_Id")
        pk(0).AutoIncrement = True
        dtCities.PrimaryKey = pk
        dtCities.AcceptChanges()

        If (dtCities.Rows.Count > 0) Then
            pk(0).AutoIncrementSeed = getMaxServiceType_Id + 1
        Else
            pk(0).AutoIncrementSeed = 1
        End If

        table = dtCities




    End Sub

    Private Sub BindCities()
        grdCity.DataSource = table
        grdCity.DataBind()
    End Sub

    Private Sub RegisterClientEvents()
        btnAdd.Attributes.Add("onclick", "btnAdd_onclick();")
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        lblError.Text = ""
        objValidate = New clsValidate(objConnection)

        For Each dRow As DataRow In table.Rows
            If Not dRow.RowState = DataRowState.Unchanged Then
                objServiceType = New clsServiceType(objConnection)
                If dRow.RowState = DataRowState.Deleted Then
                    objServiceType.Id = dRow.Item("ServiceType_Id", DataRowVersion.Original)
                    objServiceType.Delete()
                Else
                    With objServiceType
                        .Name = "" & dRow.Item("ServiceType_Name")
                    End With
                    If dRow.RowState = DataRowState.Added Then
                        objServiceType.Save(True)
                    ElseIf dRow.RowState = DataRowState.Modified Then
                        objServiceType.Id = "" & dRow.Item("ServiceType_Id")
                        objServiceType.Save(False)
                    End If
                End If

            End If
        Next

        Me.loadTable()
        Me.BindCities()
    End Sub

   
End Class