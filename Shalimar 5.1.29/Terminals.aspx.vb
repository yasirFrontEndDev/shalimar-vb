Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid

Partial Public Class Terminals
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objTerminals As clsTerminal
    Private table As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If
        objConnection = ConnectionManager.GetConnection()
        objUser = CType(Session("CurrentUser"), clsUser)
        objTerminals = New clsTerminal(objConnection)

        Me.RegisterClientEvents()

        Me.loadTable()

        If Not Me.IsPostBack Then
            Me.BindCities()
        End If

    End Sub

    Private Sub Terminals_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        ConnectionManager.CloseConnection(objConnection)
    End Sub

    Private Sub grdTerminal_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles grdTerminal.InitializeLayout
        CustomizeControl.SetGridLayout(e.Layout.Grid)
        With e.Layout.Grid

            .Columns.FromKey("Terminal_Id").Hidden = True

            .Columns.FromKey("Terminal_Email").Hidden = True
            .Columns.FromKey("User_Id").Hidden = True
            .Columns.FromKey("Access_DateTime").Hidden = True
            .Columns.FromKey("Access_SysName").Hidden = True
            .Columns.FromKey("Access_Terminal_Id").Hidden = True

            .Columns.FromKey("Terminal_Name").Header.Caption = "Terminal"
            .Columns.FromKey("Terminal_Abbr").Header.Caption = "Abbreviation"
            .Columns.FromKey("Terminal_Type").Header.Caption = "Type"
            .Columns.FromKey("Terminal_Address").Header.Caption = "Address"
            .Columns.FromKey("City_ID").Header.Caption = "City"
            .Columns.FromKey("Terminal_Phone").Header.Caption = "Phone"
            .Columns.FromKey("Terminal_Fax").Header.Caption = "Fax"


            .Columns.FromKey("Terminal_Name").Width = Unit.Pixel(150)
            .Columns.FromKey("Terminal_Abbr").Width = Unit.Pixel(100)
            .Columns.FromKey("Active").Width = Unit.Pixel(80)
            .Columns.FromKey("Terminal_Type").Width = Unit.Pixel(80)
            .Columns.FromKey("Terminal_Address").Width = Unit.Pixel(200)
            .Columns.FromKey("City_ID").Width = Unit.Pixel(100)
            .Columns.FromKey("Terminal_Phone").Width = Unit.Pixel(100)
            .Columns.FromKey("Terminal_Fax").Width = Unit.Pixel(100)

            .Columns.FromKey("City_ID").Type = ColumnType.DropDownList

            '.DisplayLayout.AllowUpdateDefault = Infragistics.WebUI.UltraWebGrid.AllowUpdate.Yes
            '.DisplayLayout.CellClickActionDefault = Infragistics.WebUI.UltraWebGrid.CellClickAction.Edit

        End With

        Dim valList As New ValueList
        Dim tbl As DataTable
        tbl = (New clsCity(objConnection)).GetCities()

        For count As Integer = 0 To tbl.Rows.Count - 1
            valList.ValueListItems.Add(tbl.Rows(count).Item("City_Id"), tbl.Rows(count).Item("City_Name"))
        Next
        valList.ValueListItems.Insert(0, New ValueListItem("", 0))

        'valList.DataSource = (New clsCity(objConnection)).GetCities()
        'valList.DisplayMember = "City_Name"
        'valList.ValueMember = "City_ID"
        'valList.DataBind()
        e.Layout.Grid.Columns.FromKey("City_ID").ValueList = valList


        Dim cmbo As HtmlSelect
        cmbo = e.Layout.Grid.Controls.Item(0).FindControl("City_ID")
        If (Not cmbo Is Nothing) Then
            clsUtil.BindControl(cmbo, objConnection, "SP_GetCities", "City_ID", "City_Name", Nothing)
            cmbo.Items.Insert(0, New ListItem("", ""))
        End If

        'Dim cmboCtr2 As ValueList
        'e.Layout.Bands(0).Columns.FromKey("City_ID").Type = ColumnType.DropDownList
        'cmboCtr2 = e.Layout.Bands(0).Columns.FromKey("City_ID").ValueList
        'e.Layout.Bands(0).Columns.FromKey("City_ID").NullText = ""

        'cmboCtr2.DataSource = (New clsCity(objConnection)).GetCities()
        'cmboCtr2.DisplayMember = "City_Name"
        'cmboCtr2.ValueMember = "City_ID"
        ''cmboCtr2.DataMember = "City"
        'cmboCtr2.DataBind()

    End Sub

    Private Sub grdTerminal_DeleteRowBatch(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles grdTerminal.DeleteRowBatch
        Dim trow As DataRow
        Dim pk = table.PrimaryKey(0).ColumnName
        Dim key As Object = e.Row.Cells.FromKey(pk).Value
        trow = table.Rows.Find(key)
        If Not trow Is Nothing Then
            trow.Delete()
        End If
    End Sub

    Private Sub grdTerminal_UpdateGrid(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.UpdateEventArgs) Handles grdTerminal.UpdateGrid
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
                            addedRow(i) = row.Cells(i).Value
                        Else
                            If UCase(row.Cells(i).Column.Key) = "USER_ID" Then
                                addedRow(i) = objUser.Id
                            ElseIf UCase(row.Cells(i).Column.Key) = UCase("Access_DateTime") Then
                                addedRow(i) = DateTime.Now
                            ElseIf UCase(row.Cells(i).Column.Key) = UCase("Access_SysName") Then
                                addedRow(i) = "PC1"
                            ElseIf UCase(row.Cells(i).Column.Key) = UCase("Access_Terminal_Id") Then
                                addedRow(i) = objUser.TerminalId
                            ElseIf (UCase(row.Cells(i).Column.DataType.ToString) = UCase("System.Boolean")) Then
                                addedRow(i) = False
                            ElseIf (UCase(row.Cells(i).Column.DataType.ToString) = UCase("System.Int32")) Or (UCase(row.Cells(i).Column.DataType.ToString) = UCase("System.Int64")) Then
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
                                ModifiedRow(i) = row.Cells(i).Value
                            Else
                                If UCase(row.Cells(i).Column.Key) = "USER_ID" Then
                                    ModifiedRow(i) = objUser.Id
                                ElseIf UCase(row.Cells(i).Column.Key) = UCase("Access_DateTime") Then
                                    ModifiedRow(i) = DateTime.Now
                                ElseIf UCase(row.Cells(i).Column.Key) = UCase("Access_SysName") Then
                                    ModifiedRow(i) = "PC1"
                                ElseIf UCase(row.Cells(i).Column.Key) = UCase("Access_Terminal_Id") Then
                                    ModifiedRow(i) = objUser.TerminalId
                                ElseIf (UCase(row.Cells(i).Column.DataType.ToString) = UCase("System.Boolean")) Then
                                    ModifiedRow(i) = False
                                ElseIf (UCase(row.Cells(i).Column.DataType.ToString) = UCase("System.Int32")) Or UCase(row.Cells(i).Column.DataType.ToString) = UCase("System.Int64") Then
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

    Private Sub RegisterClientEvents()
    End Sub

    Private Sub loadTable()
        Dim count = 0
        Dim dtTerminals As DataTable
        Dim getMaxVechile_Id As Integer = 0

        dtTerminals = objTerminals.GetAll()
        getMaxVechile_Id = objTerminals.getMaxTerminal_Id()



        Dim pk(0) As DataColumn
        pk(0) = dtTerminals.Columns("Terminal_Id")
        pk(0).AutoIncrement = True
        dtTerminals.PrimaryKey = pk
        dtTerminals.AcceptChanges()




        If (dtTerminals.Rows.Count > 0) Then
            pk(0).AutoIncrementSeed = getMaxVechile_Id + 1
        Else
            pk(0).AutoIncrementSeed = 1
        End If

        table = dtTerminals

    End Sub

    Private Sub BindCities()
        grdTerminal.DataSource = table
        grdTerminal.DataBind()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        'If grdTerminal.DisplayLayout.SelectedRows.Count > 0 Then
        '    grdTerminal.DisplayLayout.ActiveRow.Cells.FromKey("Terminal_ID")
        'End If

        For Each dRow As DataRow In table.Rows
            If Not dRow.RowState = DataRowState.Unchanged Then
                objTerminals = New clsTerminal(objConnection)
                If dRow.RowState = DataRowState.Deleted Then
                    objTerminals.Id = dRow.Item("Terminal_Id", DataRowVersion.Original)
                    objTerminals.Delete()
                Else
                    With objTerminals
                        .Name = "" & dRow.Item("Terminal_Name")
                        .Abbriviation = "" & dRow.Item("Terminal_Abbr")
                        .Active = dRow.Item("Active")
                        .Type = dRow.Item("Terminal_Type")
                        .Address = "" & dRow.Item("Terminal_Address")
                        .Phone = "" & dRow.Item("Terminal_Phone")
                        .Fax = "" & dRow.Item("Terminal_Fax")
                        .CityId = dRow.Item("City_ID")

                        .UserId = objUser.Id
                        .AccessDate = DateTime.Now
                        .ComputerName = "PC1"
                        .AccessTerminalId = "1"
                    End With
                    If dRow.RowState = DataRowState.Added Then
                        objTerminals.Save(True)
                    ElseIf dRow.RowState = DataRowState.Modified Then
                        objTerminals.Id = "" & dRow.Item("Terminal_Id")
                        objTerminals.Save(False)
                    End If
                End If

            End If
        Next

        Me.loadTable()
        Me.BindCities()
    End Sub

End Class