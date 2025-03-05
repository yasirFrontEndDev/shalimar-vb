Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid

Partial Public Class Exclude_Terminal_Detail

    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objScheduleDetailList As clsScheduleDetailList
    Dim objSchedule As clsExcludeTerminals
    Private table As DataTable


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If

        objConnection = ConnectionManager.GetConnection()
        objUser = CType(Session("CurrentUser"), clsUser)
        objSchedule = New clsExcludeTerminals(objConnection)
        objScheduleDetailList = New clsScheduleDetailList(objConnection)

        Call RegisterClientEvents()

        If Not Me.IsPostBack Then
            Me.FillCombos()
            If "" & Request.QueryString("AddNew") <> "1" Then

                hidSchID.Value = Request.QueryString("SchID")
                Me.BindScheduleData()

            End If
        End If

        Me.loadTable()

        If Not Me.IsPostBack Then
            If "" & Request.QueryString("AddNew") <> "1" Then
                hidSchID.Value = Request.QueryString("SchID")
                Me.BindScheduleData()
                Call BindScheduleDetail()
            End If
        End If

    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        ConnectionManager.CloseConnection(objConnection)
    End Sub

    Private Sub grdSchedules_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles grdSchedules.InitializeLayout
        CustomizeControl.SetGridLayout(e.Layout.Grid)

        

    End Sub

    Private Sub grdSchedules_DeleteRowBatch(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles grdSchedules.DeleteRowBatch
        Dim trow As DataRow
        Dim pk = table.PrimaryKey(0).ColumnName
        Dim key As Object = e.Row.Cells.FromKey(pk).Value
        trow = table.Rows.Find(key)
        If Not trow Is Nothing Then
            trow.Delete()
        End If
    End Sub

    Private Sub grdSchedules_UpdateGrid(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.UpdateEventArgs) Handles grdSchedules.UpdateGrid
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
                    'If (row.Cells(i).Column.Key <> pk) Then
                    If (Not row.Cells(i).Value Is Nothing) Then
                        isEmpty = False
                        addedRow(i) = row.Cells(i).Value
                    Else
                        'If UCase(row.Cells(i).Column.Key) = "USER_ID" Then
                        '    addedRow(i) = objUser.Id
                        'ElseIf UCase(row.Cells(i).Column.Key) = UCase("Access_DateTime") Then
                        '    addedRow(i) = DateTime.Now
                        'ElseIf UCase(row.Cells(i).Column.Key) = UCase("Access_SysName") Then
                        '    addedRow(i) = "PC1"
                        'ElseIf UCase(row.Cells(i).Column.Key) = UCase("Access_Terminal_Id") Then
                        '    addedRow(i) = objUser.TerminalId
                        If (UCase(row.Cells(i).Column.DataType.ToString) = UCase("System.Boolean")) Then
                            addedRow(i) = False
                        ElseIf (UCase(row.Cells(i).Column.DataType.ToString) = UCase("System.Int32")) Or (UCase(row.Cells(i).Column.DataType.ToString) = UCase("System.Int64")) Then
                            addedRow(i) = 0
                        Else
                            addedRow(i) = ""
                        End If
                    End If
                    'End If
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
                                'If UCase(row.Cells(i).Column.Key) = "USER_ID" Then
                                '    ModifiedRow(i) = objUser.Id
                                'ElseIf UCase(row.Cells(i).Column.Key) = UCase("Access_DateTime") Then
                                '    ModifiedRow(i) = DateTime.Now
                                'ElseIf UCase(row.Cells(i).Column.Key) = UCase("Access_SysName") Then
                                '    ModifiedRow(i) = "PC1"
                                'ElseIf UCase(row.Cells(i).Column.Key) = UCase("Access_Terminal_Id") Then
                                '    ModifiedRow(i) = objUser.TerminalId
                                'Else
                                If (UCase(row.Cells(i).Column.DataType.ToString) = UCase("System.Boolean")) Then
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

#Region " Functions And Procedure  "

    Private Sub FillCombos()

        clsUtil.BindControl(cmbFromCity, objConnection, "SP_GetCities", "City_Id", "City_Name", Nothing)
        cmbFromCity.Items.Insert(0, New ListItem("---Select City---", 0))

        clsUtil.BindControl(cmbToCity, objConnection, "SP_GetCities", "City_Id", "City_Name", Nothing)
        cmbToCity.Items.Insert(0, New ListItem("---Select City---", 0))


    End Sub

    Private Sub BindScheduleData()

        objSchedule = New clsExcludeTerminals(objConnection)
        objSchedule.Id = CType(hidSchID.Value.Trim(), Long)
        ' objSchedule.GetById()

        'txtTitle.Text = objSchedule.Title
        'txtCode.Text = objSchedule.Code
        'txtComission.Text = objSchedule.Comission
        'txtExtraFare.Text = objSchedule.ExtraFare
        'dtSchedule.Value = objSchedule.ScheduleDate

        'dtWEF.Value = objSchedule.ScheduleWEF

        'If Not cmbRoutes.Items.FindByValue(objSchedule.RouteID) Is Nothing Then
        '    cmbRoutes.SelectedValue = objSchedule.RouteID
        'End If

    End Sub

    Private Sub loadTable()
       





    End Sub

    Private Sub BindScheduleDetail()
        grdSchedules.DataSource = table
        grdSchedules.DataBind()
    End Sub

    Private Sub RegisterClientEvents()
        btnSave.Attributes.Add("onclick", "return validation();")
        btnSavenClose.Attributes.Add("onclick", "return validation();")
    End Sub

#End Region

    

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        objSchedule.Source = cmbFromCity.SelectedValue

        objSchedule.Destination = cmbToCity.SelectedValue



        objSchedule.Save(True)
        hidSchID.Value = objSchedule.Id




        ''Save from datatable
        Dim objScheduleDetail As clsScheduleDetail
        For Each dRow As DataRow In table.Rows
            If Not dRow.RowState = DataRowState.Unchanged Then
                objScheduleDetail = New clsScheduleDetail(objConnection)
                If dRow.RowState = DataRowState.Deleted Then
                    If IsDBNull(dRow.Item("Schedule_Id", DataRowVersion.Original)) = False Then
                        objScheduleDetail.Delete(0, dRow.Item("Schedule_Id", DataRowVersion.Original), dRow.Item("Sr_No", DataRowVersion.Original))
                    End If

                Else
                    'With objScheduleDetail
                    If table.Columns.Count > 2 Then
                        For i As Integer = 0 To ((table.Columns.Count - 2) / 6) - 1
                            With objScheduleDetail
                                .ScheduleID = objSchedule.Id 'dRow.Item("Schedule_Id") Loc=0
                                .SerialNo = dRow.Item("Sr_No") ' Loc=1
                                .RegularTime = dRow.Item("Is_RegularTime") ' Loc=1
                                .RouteDetailID = grdSchedules.Columns((i * 6) + 3).Key.Replace("City_Id_", "")
                                .CityID = dRow.Item((i * 6) + 3)
                                .ArrivalTime = dRow.Item((i * 6) + 4) 'DateTime.Now
                                .StayPeriod = dRow.Item((i * 6) + 5)
                                .DeprtTime = dRow.Item((i * 6) + 6)
                            End With
                            If dRow.RowState = DataRowState.Added Then
                                objScheduleDetail.Save(True)
                            Else
                                objScheduleDetail.Id = dRow.Item((i * 6) + 2)
                                objScheduleDetail.Save(False)
                            End If
                        Next
                    End If
                    '.CityID = dRow.Item("City_Id")
                    '.Abbriviation = "" & dRow.Item("Terminal_Abbr")
                    '.Active = dRow.Item("Active")
                    '.Type = dRow.Item("Terminal_Type")
                    '.Address = "" & dRow.Item("Terminal_Address")
                    '.Phone = "" & dRow.Item("Terminal_Phone")
                    '.Fax = "" & dRow.Item("Terminal_Fax")
                    '.CityID = dRow.Item("City_ID")

                    '.UserId = objUser.Id
                    '.AccessDate = DateTime.Now
                    '.ComputerName = "PC1"
                    '.AccessTerminalId = "1"
                    'End With
                    'If dRow.RowState = DataRowState.Added Then
                    '    objTerminals.Save(True)
                    'ElseIf dRow.RowState = DataRowState.Modified Then
                    '    objTerminals.Id = "" & dRow.Item("Terminal_Id")
                    '    objTerminals.Save(False)
                    'End If
                End If
            ElseIf table.Rows.Count = 1 Then
                'Dim dtScheduleDetails As New DataTable
                'If hidSchID.Value.Trim <> "" And hidSchID.Value.Trim <> "0" Then
                '    dtScheduleDetails = objScheduleDetailList.GetAll(hidSchID.Value.Trim())
                'End If
                'If dtScheduleDetails.Rows.Count = 0 And grdSchedules.Rows.Count > 0 Then
                '    objScheduleDetail = New clsScheduleDetail(objConnection)
                '    If table.Columns.Count > 2 Then
                '        For i As Integer = 0 To ((table.Columns.Count - 2) / 6) - 1
                '            With objScheduleDetail
                '                .ScheduleID = objSchedule.Id 'dRow.Item("Schedule_Id") Loc=0
                '                .SerialNo = grdSchedules.Rows(0).Cells.FromKey("Sr_No").Value  ' Loc=1
                '                .RouteDetailID = grdSchedules.Columns((i * 6) + 3).Key.Replace("City_Id_", "")
                '                .CityID = grdSchedules.Rows(0).Cells((i * 6) + 3).Value
                '                .ArrivalTime = grdSchedules.Rows(0).Cells((i * 6) + 4).Value 'DateTime.Now
                '                .StayPeriod = 0 'grdSchedules.Rows(0).Cells((i * 6) + 5).Value
                '                .DeprtTime = grdSchedules.Rows(0).Cells((i * 6) + 6).Value
                '                .Save(True)
                '            End With
                '        Next
                '    End If
                'End If
            End If
        Next

        Me.loadTable()
        Me.BindScheduleDetail()


    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
        Response.Redirect("ExcludeTerminals.aspx")
    End Sub

    Protected Sub btnSavenClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSavenClose.Click
        btnSave_Click(sender, e)
        btnClose_Click(sender, e)
    End Sub

    Private Sub grdSchedules_InitializeRow(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles grdSchedules.InitializeRow
        If e.Row.Index = 0 And ("" & e.Row.Cells(2).Value = "" Or "" & e.Row.Cells(2).Value = "0") Then
            e.Row.Hidden = True
        End If
    End Sub
End Class