Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid
Imports Microsoft.Synchronization


Partial Public Class UserRights
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Private table As DataTable
    Dim objSessionUser As clsUser

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        objSessionUser = CType(Session("CurrentUser"), clsUser)
        objConnection = ConnectionManager.GetConnection()
        objUser = New clsUser(objConnection)

        Me.RegisterClientEvents()

        Me.loadTable()

        If Not Me.IsPostBack Then
            Me.BindGrid()
        End If

    End Sub

    Private Sub Users_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        ConnectionManager.CloseConnection(objConnection)
    End Sub

    Private Sub RegisterClientEvents()
    End Sub

    Private Sub loadTable()
        Dim count = 0
        Dim dtUsers As DataTable
        dtUsers = objUser.GetAll()
        'cmbUserType

        ''cmbUserType.items.clear()


        Dim pk(0) As DataColumn
        pk(0) = dtUsers.Columns("User_Id")
        pk(0).AutoIncrement = True
        dtUsers.PrimaryKey = pk
        dtUsers.AcceptChanges()

        If (dtUsers.Rows.Count > 0) Then
            pk(0).AutoIncrementSeed = dtUsers.Rows(dtUsers.Rows.Count - 1).Item("User_Id") + 1
        Else
            pk(0).AutoIncrementSeed = 1
        End If

        table = dtUsers

    End Sub

    Private Sub BindGrid()
        grdUsers.DataSource = table
        grdUsers.DataBind()
    End Sub

    Private Sub grdUsers_DeleteRowBatch(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles grdUsers.DeleteRowBatch
        Dim trow As DataRow
        Dim pk = table.PrimaryKey(0).ColumnName
        Dim key As Object = e.Row.Cells.FromKey(pk).Value
        trow = table.Rows.Find(key)
        If Not trow Is Nothing Then
            trow.Delete()
        End If
    End Sub

    Private Sub grdUsers_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles grdUsers.InitializeLayout
        CustomizeControl.SetGridLayout(e.Layout.Grid)
        With e.Layout.Grid

            .Columns.FromKey("User_Id").Hidden = True
            .Columns.FromKey("Password").Hidden = True
            .Columns.FromKey("Created_By").Hidden = True
            .Columns.FromKey("Created_On").Hidden = True
            .Columns.FromKey("Changed_By").Hidden = True
            .Columns.FromKey("Changed_On").Hidden = True

            .Columns.FromKey("Full_Name").Header.Caption = "Name"
            .Columns.FromKey("User_Name").Header.Caption = "Login"
            .Columns.FromKey("IsSuperAdmin").Header.Caption = "Super Admin"
            .Columns.FromKey("IsAdmin").Header.Caption = "Admin"
            .Columns.FromKey("LastLogin").Header.Caption = "Last Login"
            .Columns.FromKey("Is_Active").Header.Caption = "Active"

            .Columns.FromKey("User_Name").Width = Unit.Pixel(150)
            .Columns.FromKey("IsSuperAdmin").Width = Unit.Pixel(100)
            .Columns.FromKey("IsAdmin").Width = Unit.Pixel(80)
            .Columns.FromKey("LastLogin").Width = Unit.Pixel(150)
            .Columns.FromKey("Is_Active").Width = Unit.Pixel(80)
            .Columns.FromKey("Full_Name").Width = Unit.Percentage(100)

        End With
    End Sub

    Private Sub grdUsers_UpdateGrid(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.UpdateEventArgs) Handles grdUsers.UpdateGrid
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
                            If UCase(row.Cells(i).Column.Key) = UCase("LastLogin") Or UCase(row.Cells(i).Column.Key) = UCase("Created_On") Or UCase(row.Cells(i).Column.Key) = UCase("Changed_On") Then
                                addedRow(i) = DateTime.Now
                            ElseIf UCase(row.Cells(i).Column.Key) = UCase("Created_By") Or UCase(row.Cells(i).Column.Key) = UCase("Changed_By") Then
                                addedRow(i) = objSessionUser.Id
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
                                If UCase(row.Cells(i).Column.Key) = UCase("LastLogin") Or UCase(row.Cells(i).Column.Key) = UCase("Created_On") Or UCase(row.Cells(i).Column.Key) = UCase("Changed_On") Then
                                    ModifiedRow(i) = DateTime.Now
                                ElseIf UCase(row.Cells(i).Column.Key) = UCase("Created_By") Or UCase(row.Cells(i).Column.Key) = UCase("Changed_By") Then
                                    ModifiedRow(i) = objSessionUser.Id
                                ElseIf (UCase(row.Cells(i).Column.DataType.ToString) = UCase("System.Boolean")) Then
                                    ModifiedRow(i) = False
                                ElseIf (UCase(row.Cells(i).Column.DataType.ToString) = UCase("System.Int32")) Or (UCase(row.Cells(i).Column.DataType.ToString) = UCase("System.Int64")) Then
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

    Protected Sub btnAssignRights_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAssignRights.Click
        Response.Write(grdUsers)
    End Sub
End Class