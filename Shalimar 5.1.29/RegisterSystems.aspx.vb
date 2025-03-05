Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid

Partial Public Class RegisterSystems
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objSystem_Config As clsSystem_Config
    Private table As DataTable
    Dim System_ConfigID As String

#Region " Form Events "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Session("CurrentUser") Is Nothing Then
        '    Response.Redirect("UserLogin.aspx")
        'End If

        objConnection = ConnectionManager.GetConnection()
        objUser = CType(Session("CurrentUser"), clsUser)
        objSystem_Config = New clsSystem_Config(objConnection)

        Call RegisterClientEvents()
        Call loadTable()

        If Not Me.IsPostBack Then
            Call BindSystem_ConfigDetail()
        End If

    End Sub

    Private Sub System_Configs_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        ConnectionManager.CloseConnection(objConnection)
    End Sub

#End Region

#Region " Control Events "

    Private Sub grdDetails_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles grdDetails.InitializeLayout
        CustomizeControl.SetGridLayout(e.Layout.Grid)
        With e.Layout.Grid

            .Columns.FromKey("ID").Hidden = True
            ' .Columns.FromKey("Terminal_Id").Hidden = True


            .Columns.FromKey("Terminal_Id").Header.Caption = "Terminal"
            .Columns.FromKey("Computer_Name").Header.Caption = "Computer Name"
            .Columns.FromKey("MAC").Header.Caption = "MAC Address"


            .Columns.FromKey("Terminal_Id").Width = Unit.Percentage(34)
            .Columns.FromKey("Computer_Name").Width = Unit.Percentage(33)
            .Columns.FromKey("MAC").Width = Unit.Percentage(33)


            .DisplayLayout.AllowUpdateDefault = Infragistics.WebUI.UltraWebGrid.AllowUpdate.Yes
            .DisplayLayout.CellClickActionDefault = Infragistics.WebUI.UltraWebGrid.CellClickAction.Edit

            Dim vList As New ValueList
            Dim dt As DataTable
            dt = (New clsTerminal(objConnection)).GetAll()

            For Each drow As DataRow In dt.Rows
                vList.ValueListItems.Add(drow.Item("Terminal_Id"), drow.Item("Terminal_Name"))
            Next

            .Columns.FromKey("Terminal_Id").Type = ColumnType.DropDownList
            .Columns.FromKey("Terminal_Id").ValueList = vList

            Dim dList As New ValueList
            For Each drow As DataRow In dt.Rows
                dList.ValueListItems.Add(drow.Item("Terminal_Id"), drow.Item("Terminal_Name"))
            Next

        End With
    End Sub

    Private Sub grdDetails_DeleteRowBatch(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles grdDetails.DeleteRowBatch
        Dim trow As DataRow
        Dim pk = table.PrimaryKey(0).ColumnName
        Dim key As Object = e.Row.Cells.FromKey(pk).Value

        trow = table.Rows.Find(key)
        If Not trow Is Nothing Then
            trow.Delete()
        End If
    End Sub

    Private Sub grdDetails_UpdateGrid(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.UpdateEventArgs) Handles grdDetails.UpdateGrid
        Dim row As Infragistics.WebUI.UltraWebGrid.UltraGridRow
        Dim pk = table.PrimaryKey(0).ColumnName

        'create and get the rows enumeration for the changed rows
        Dim updatedRows As Infragistics.WebUI.UltraWebGrid.UltraGridRowsEnumerator
        updatedRows = e.Grid.Bands(0).GetBatchUpdates()

        'for each row in the Updated rows check if the current row is an addedrow, if so create the row and add it to the dataset
        While updatedRows.MoveNext
            row = updatedRows.Current
            If 1 = 1 Then


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
                                    If UCase(row.Cells(i).Column.Key) = "USER_ID" Then
                                        addedRow(i) = 0
                                    ElseIf UCase(row.Cells(i).Column.Key) = "TERMINAL_ID" Then
                                        addedRow(i) = 0
                                    ElseIf UCase(row.Cells(i).Column.Key) = "ACCESS_DATE" Then
                                        addedRow(i) = DateTime.Now
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
                                        If UCase(row.Cells(i).Column.Key) = "USER_ID" Then
                                            ModifiedRow(i) = 0
                                        ElseIf UCase(row.Cells(i).Column.Key) = "TERMINAL_ID" Then
                                            ModifiedRow(i) = 0
                                        ElseIf UCase(row.Cells(i).Column.Key) = "ACCESS_DATE" Then
                                            ModifiedRow(i) = DateTime.Now
                                        Else
                                            ModifiedRow(i) = ""
                                        End If
                                    End If
                                End If
                            Next
                        End If
                End Select
            End If
        End While
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

        For Each dRow As DataRow In table.Rows
            If Not dRow.RowState = DataRowState.Unchanged Then
                If dRow.RowState = DataRowState.Deleted Then
                    objSystem_Config.Id = dRow.Item("ID", DataRowVersion.Original)
                    objSystem_Config.Delete(objSystem_Config.Id)
                Else
                    With objSystem_Config
                        .TerminalID = dRow.Item("Terminal_Id")
                        .ComputerName = dRow.Item("Computer_Name")
                        .MAC = dRow.Item("MAC")
                    End With

                    If dRow.RowState = DataRowState.Added Then
                        objSystem_Config.Save(True)
                    ElseIf dRow.RowState = DataRowState.Modified Then
                        objSystem_Config.Id = "" & dRow.Item("ID")
                        objSystem_Config.Save(False)
                    End If
                End If

            End If
        Next

        Call loadTable()
        Call BindSystem_ConfigDetail()
    End Sub

#End Region

#Region " Functions And Procedure "

    Private Sub RegisterClientEvents()
        btnSave.Attributes.Add("onclick", "return validation();")
    End Sub

    Private Sub loadTable()
        Dim count = 0
        Dim dtSystem_ConfigDetail As DataTable

        Dim getMaxVechile_Id As Integer = 0
        getMaxVechile_Id = objSystem_Config.getMaxSystem_Config_Id()

        dtSystem_ConfigDetail = objSystem_Config.GetAll()

        Dim pk(0) As DataColumn
        pk(0) = dtSystem_ConfigDetail.Columns("ID")
        pk(0).AutoIncrement = True
        dtSystem_ConfigDetail.PrimaryKey = pk
        dtSystem_ConfigDetail.AcceptChanges()

        If (dtSystem_ConfigDetail.Rows.Count > 0) Then
            pk(0).AutoIncrementSeed = getMaxVechile_Id + 1
        Else
            pk(0).AutoIncrementSeed = 1
        End If

        table = dtSystem_ConfigDetail
    End Sub

    Private Sub BindSystem_ConfigDetail()
        grdDetails.DataSource = table
        grdDetails.DataBind()
    End Sub

#End Region

End Class