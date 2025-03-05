Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid

Partial Public Class Fares
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objFare As clsFare
    Private table As DataTable
    Dim FareID As String

#Region " Form Events "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If

        objConnection = ConnectionManager.GetConnection()
        objUser = CType(Session("CurrentUser"), clsUser)
        objFare = New clsFare(objConnection)
        Call RegisterClientEvents()
        Call loadTable()

    
        If Not Me.IsPostBack Then
            Call FillCombos()
            Call BindFareDetail()
          
        End If

    End Sub

    Private Sub Fares_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        ConnectionManager.CloseConnection(objConnection)
    End Sub

#End Region

#Region " Control Events "

    Private Sub grdDetails_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles grdDetails.InitializeLayout
        CustomizeControl.SetGridLayout(e.Layout.Grid)
        With e.Layout.Grid

            .Columns.FromKey("Fare_ID").Hidden = True
            .Columns.FromKey("User_ID").Hidden = True
            .Columns.FromKey("Access_Date").Hidden = True
            .Columns.FromKey("Computer_Name").Hidden = True
            .Columns.FromKey("Terminal_Id").Hidden = True
            .Columns.FromKey("ServiceType_Id").Hidden = True


            .Columns.FromKey("Source_City_Id").Header.Caption = "Source"
            .Columns.FromKey("Destination_City_Id").Header.Caption = "Destination"
            .Columns.FromKey("Fare_Amount").Header.Caption = "Fare"

            .Columns.FromKey("Source_City_Id").Width = Unit.Percentage(25)
            .Columns.FromKey("Destination_City_Id").Width = Unit.Percentage(25)
            .Columns.FromKey("Fare_Amount").Width = Unit.Percentage(20)
            .Columns.FromKey("Ticket_Refund").Width = Unit.Percentage(15)
            .Columns.FromKey("Ticket_Refund").Width = Unit.Percentage(15)
            .Columns.FromKey("Next_Departure").Width = Unit.Percentage(15)


            .DisplayLayout.AllowUpdateDefault = Infragistics.WebUI.UltraWebGrid.AllowUpdate.Yes
            .DisplayLayout.CellClickActionDefault = Infragistics.WebUI.UltraWebGrid.CellClickAction.Edit

            Dim vList As New ValueList
            Dim dt As DataTable
            dt = (New clsCity(objConnection)).GetCities()

            For Each drow As DataRow In dt.Rows
                vList.ValueListItems.Add(drow.Item("City_Id"), drow.Item("City_Name"))
            Next

            .Columns.FromKey("Source_City_Id").Type = ColumnType.DropDownList
            .Columns.FromKey("Source_City_Id").ValueList = vList

            Dim dList As New ValueList
            For Each drow As DataRow In dt.Rows
                dList.ValueListItems.Add(drow.Item("City_Id"), drow.Item("City_Name"))
            Next

            .Columns.FromKey("Destination_City_Id").Type = ColumnType.DropDownList
            .Columns.FromKey("Destination_City_Id").ValueList = dList

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
            If "" & row.Cells.FromKey("Source_City_Id").Value <> "" And "" & row.Cells.FromKey("Source_City_Id").Value <> "" And "" & row.Cells.FromKey("Fare_Amount").Value <> "" Then


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

                                    ElseIf UCase(row.Cells(i).Column.Key) = "SERVICETYPE_ID" Then
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
                                        ElseIf UCase(row.Cells(i).Column.Key) = "SERVICETYPE_ID" Then
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
                    objFare.Id = dRow.Item("Fare_ID", DataRowVersion.Original)
                    objFare.Delete(objFare.Id)
                Else
                    With objFare
                        .Source = dRow.Item("Source_City_Id")
                        .Destination = dRow.Item("Destination_City_Id")
                        .Fare = dRow.Item("Fare_Amount")
                        .UserID = objUser.Id
                        .ComputerName = "PC1"
                        .TerminalID = objUser.TerminalId
                        .Ticket_Refund = dRow.Item("Ticket_Refund")
                        .Ticket_Change = dRow.Item("Ticket_Change")
                        .Next_Departure = dRow.Item("Next_Departure")
                        .KM = dRow.Item("KM")
                        .Points = dRow.Item("Points")
                        .ServiceType_Id = cboServiceType.SelectedValue
                    End With

                    If dRow.RowState = DataRowState.Added Then
                        objFare.Save(True)
                    ElseIf dRow.RowState = DataRowState.Modified Then
                        objFare.Id = "" & dRow.Item("Fare_ID")
                        objFare.Save(False)
                    End If
                End If

            End If
        Next

        Call loadTable()
        Call BindFareDetail()
    End Sub

#End Region

#Region " Functions And Procedure "

    Private Sub RegisterClientEvents()
        btnSave.Attributes.Add("onclick", "return validation();")
    End Sub

    Private Sub FillCombos()

        clsUtil.BindControl(cboServiceType, objConnection, "SP_GetServiceType", "ServiceType_Id", "ServiceType_Name", Nothing)
        cboServiceType.Items.Insert(0, New ListItem("---Select Service Type---", 0))

    End Sub

    Private Sub loadTable()
        Dim count = 0
        Dim dtFareDetail As DataTable

        Dim getMaxVechile_Id As Integer = 0
        getMaxVechile_Id = objFare.getMaxFare_Id()


        If cboServiceType.Items.Count <> 0 Then
            dtFareDetail = objFare.GetAll(CInt(Val(cboServiceType.SelectedItem.Value)))
        Else
            dtFareDetail = objFare.GetAll(0)

        End If

        Dim pk(0) As DataColumn
        pk(0) = dtFareDetail.Columns("Fare_ID")
        pk(0).AutoIncrement = True
        dtFareDetail.PrimaryKey = pk
        dtFareDetail.AcceptChanges()

        If (dtFareDetail.Rows.Count > 0) Then
            pk(0).AutoIncrementSeed = getMaxVechile_Id + 1
        Else
            pk(0).AutoIncrementSeed = 1
        End If

        table = dtFareDetail
    End Sub

    Private Sub BindFareDetail()
        grdDetails.DataSource = table
        grdDetails.DataBind()
    End Sub
    Protected Sub cboServiceType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboServiceType.SelectedIndexChanged
        Call loadTable()
        Call BindFareDetail()
    End Sub
#End Region
    
End Class