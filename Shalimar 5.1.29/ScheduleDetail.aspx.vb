Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid

Partial Public Class ScheduleDetail
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objScheduleDetailList As clsScheduleDetailList
    Dim objSchedule As clsSchedule
    Private table As DataTable


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If

        objConnection = ConnectionManager.GetConnection()
        objUser = CType(Session("CurrentUser"), clsUser)
        objSchedule = New clsSchedule(objConnection)
        objScheduleDetailList = New clsScheduleDetailList(objConnection)

        Call RegisterClientEvents()

        If Not Me.IsPostBack Then
            Me.FillCombos()
            If "" & Request.QueryString("AddNew") <> "1" Then
                hidSchID.Value = Request.QueryString("SchID")
                Me.BindScheduleData()
                cmbRoutes.Enabled = False
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

        With e.Layout.Grid
            .Columns(0).Hidden = False

            For i As Integer = 0 To ((.Columns.Count - 2) / 6) - 1
                .Columns((i * 6) + 2).Hidden = True
                .Columns((i * 6) + 3).Hidden = True
                If i = 0 Then
                    .Columns((i * 6) + 4).Hidden = True
                    .Columns((i * 6) + 5).Hidden = True
                Else
                    .Columns((i * 6) + 4).AllowUpdate = AllowUpdate.No
                    .Columns((i * 6) + 5).AllowUpdate = AllowUpdate.No
                    .Columns((i * 6) + 6).AllowUpdate = AllowUpdate.No
                End If
                .Columns((i * 6) + 7).Hidden = True
            Next

            .Columns.FromKey("Sr_No").Width = Unit.Pixel(25)

            .DisplayLayout.AllowUpdateDefault = Infragistics.WebUI.UltraWebGrid.AllowUpdate.Yes
            .DisplayLayout.CellClickActionDefault = Infragistics.WebUI.UltraWebGrid.CellClickAction.Edit

            Dim vList As New ValueList
            Dim dt As DataTable
            dt = (New clsServiceType(objConnection)).GetServiceTypes()

            For Each drow As DataRow In dt.Rows
                vList.ValueListItems.Add(drow.Item("SericeType_Id"), drow.Item("SericeType_Name"))
            Next

            .Columns.FromKey("SericeType_Id").Type = ColumnType.DropDownList
            .Columns.FromKey("SericeType_Id").ValueList = vList

            Dim dList As New ValueList
            For Each drow As DataRow In dt.Rows
                dList.ValueListItems.Add(drow.Item("SericeType_Id"), drow.Item("SericeType_Name"))
            Next

            .Columns.FromKey("SericeType_Id").Type = ColumnType.DropDownList
            .Columns.FromKey("SericeType_Id").ValueList = dList

            '.Columns.FromKey("Terminal_Name").Width = Unit.Pixel(150)
            '.Columns.FromKey("Terminal_Abbr").Width = Unit.Pixel(100)
            '.Columns.FromKey("Active").Width = Unit.Pixel(80)
            '.Columns.FromKey("Terminal_Type").Width = Unit.Pixel(80)
            '.Columns.FromKey("Terminal_Address").Width = Unit.Pixel(200)
            '.Columns.FromKey("City_ID").Width = Unit.Pixel(100)
            '.Columns.FromKey("Terminal_Phone").Width = Unit.Pixel(100)
            '.Columns.FromKey("Terminal_Fax").Width = Unit.Pixel(100)

            '.Columns.FromKey("City_ID").Type = ColumnType.DropDownList

            '.DisplayLayout.AllowUpdateDefault = Infragistics.WebUI.UltraWebGrid.AllowUpdate.Yes
            '.DisplayLayout.CellClickActionDefault = Infragistics.WebUI.UltraWebGrid.CellClickAction.Edit

        End With

        'Dim valList As New ValueList
        'Dim tbl As DataTable
        'tbl = (New clsCity(objConnection)).GetCities()

        'For count As Integer = 0 To tbl.Rows.Count - 1
        '    valList.ValueListItems.Add(tbl.Rows(count).Item("City_Id"), tbl.Rows(count).Item("City_Name"))
        'Next
        'valList.ValueListItems.Insert(0, New ValueListItem("", 0))

        ''valList.DataSource = (New clsCity(objConnection)).GetCities()
        ''valList.DisplayMember = "City_Name"
        ''valList.ValueMember = "City_ID"
        ''valList.DataBind()
        'e.Layout.Grid.Columns.FromKey("City_ID").ValueList = valList


        'Dim cmbo As HtmlSelect
        'cmbo = e.Layout.Grid.Controls.Item(0).FindControl("City_ID")
        'If (Not cmbo Is Nothing) Then
        '    clsUtil.BindControl(cmbo, objConnection, "SP_GetCities", "City_ID", "City_Name", Nothing)
        '    cmbo.Items.Insert(0, New ListItem("", ""))
        'End If

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
        clsUtil.BindControl(cmbRoutes, objConnection, "SP_GetRoutesForCombo", "Route_Id", "Route_Name", Nothing)
        cmbRoutes.Items.Insert(0, New ListItem("---Select Route---", 0))
    End Sub

    Private Sub BindScheduleData()
        objSchedule = New clsSchedule(objConnection)
        objSchedule.Id = CType(hidSchID.Value.Trim(), Long)
        objSchedule.GetById()

        txtTitle.Text = objSchedule.Title
        txtCode.Text = objSchedule.Code
        txtComission.Text = objSchedule.Comission
        txtExtraFare.Text = objSchedule.ExtraFare
        dtSchedule.Value = objSchedule.ScheduleDate

        dtWEF.Value = objSchedule.ScheduleWEF

        If Not cmbRoutes.Items.FindByValue(objSchedule.RouteID) Is Nothing Then
            cmbRoutes.SelectedValue = objSchedule.RouteID
        End If

    End Sub

    Private Sub loadTable()
        Dim dtRouteDetails As DataTable
        Dim objRouteDetailList As New clsRouteDetailList(objConnection)
        dtRouteDetails = objRouteDetailList.GetAll(CType(cmbRoutes.SelectedValue, Long), True)
        Dim dvRouteDetails As New DataView(dtRouteDetails)
        dvRouteDetails.Sort = "Sr_No"

        Dim count = 0
        Dim dtSchDetail As New DataTable
        dtSchDetail.Columns.Add(clsUtil.CreateColumn("Is_RegularTime", "Regular Time", "System.Boolean"))
        dtSchDetail.Columns.Add(clsUtil.CreateColumn("ServiceType_Id", "Service Type", "System.Int64"))

        dtSchDetail.Columns.Add(clsUtil.CreateColumn("Sr_No", "Sr. #", "System.Int32"))


        For i As Integer = 0 To dvRouteDetails.Count - 1
            dtSchDetail.Columns.Add(clsUtil.CreateColumn("Schedule_Detail_Id_" & dvRouteDetails.Item(i)("Route_Detail_Id"), "Schedule_Detail_Id", "System.Int64"))
            dtSchDetail.Columns.Add(clsUtil.CreateColumn("City_Id_" & dvRouteDetails.Item(i)("Route_Detail_Id"), "City_Id_" & dvRouteDetails.Item(i)("Route_Detail_Id"), "System.Int64"))
            dtSchDetail.Columns.Add(clsUtil.CreateColumn("Arrival_Time_" & dvRouteDetails.Item(i)("Route_Detail_Id"), "" & dvRouteDetails.Item(i)("City_Abbr") & "-" & "ARR", "System.String"))
            dtSchDetail.Columns.Add(clsUtil.CreateColumn("Stay_Period_" & dvRouteDetails.Item(i)("Route_Detail_Id"), "" & dvRouteDetails.Item(i)("City_Abbr") & "-" & "Stay", "System.String"))
            dtSchDetail.Columns.Add(clsUtil.CreateColumn("Departure_Time_" & dvRouteDetails.Item(i)("Route_Detail_Id"), "" & dvRouteDetails.Item(i)("City_Abbr") & "-" & "DEP", "System.String"))
            dtSchDetail.Columns.Add(clsUtil.CreateColumn("Travel_Time_" & dvRouteDetails.Item(i)("Route_Detail_Id"), "Travel_Time_" & dvRouteDetails.Item(i)("Route_Detail_Id"), "System.String"))

        Next

        dtSchDetail.Columns.Add(clsUtil.CreateColumn("Schedule_Id", "Schedule_Id", "System.Int64"))


        'Route_Detail_ID  Route_id  Sr_No City_ID   Travel_Time     Stay_Time   Fare


        'If RouteID = "" Then
        '    RouteID = "0"
        'End If
        Dim dtScheduleDetails As New DataTable
        If hidSchID.Value.Trim <> "" And hidSchID.Value.Trim <> "0" Then
            dtScheduleDetails = objScheduleDetailList.GetAll(hidSchID.Value.Trim())
        End If

        If Not dtScheduleDetails Is Nothing Then
            If dtScheduleDetails.Rows.Count > 0 Then
                Dim dvScheduleDetails As New DataView(dtScheduleDetails)
                dvScheduleDetails.Sort = "Sr_No"
                Dim LineNo As Integer = 1
                Dim drow As DataRow
                Dim j As Integer
                For i As Integer = 0 To dvScheduleDetails.Count - 1
                    If i = 0 Or LineNo <> dvScheduleDetails.Item(i)("Sr_No") Then
                        j = 0
                        LineNo = dvScheduleDetails.Item(i)("Sr_No")
                        drow = dtSchDetail.NewRow()
                        drow.Item("Schedule_Id") = dvScheduleDetails.Item(0)("Schedule_Id")
                        'drow.Item("Is_RegularTime") = dvScheduleDetails.Item(0)("Is_RegularTime")
                        drow.Item("Is_RegularTime") = dvScheduleDetails.Item(i)("Is_RegularTime")
                        drow.Item("Sr_No") = LineNo
                        'ElseIf LineNo <> dvScheduleDetails.Item(i)("Sr_No") Then
                        'LineNo = dvScheduleDetails.Item(i)("Sr_No")
                        'drow = dtSchDetail.NewRow()
                    End If

                    drow.Item("Schedule_Detail_Id_" & dvRouteDetails.Item(j)("Route_Detail_Id")) = dvScheduleDetails.Item(i)("Schedule_Detail_Id")
                    drow.Item("City_Id_" & dvRouteDetails.Item(j)("Route_Detail_Id")) = dvScheduleDetails.Item(i)("City_ID")
                    drow.Item("Arrival_Time_" & dvRouteDetails.Item(j)("Route_Detail_Id")) = "" & dvScheduleDetails.Item(i)("Arrival_Time")
                    drow.Item("Stay_Period_" & dvRouteDetails.Item(j)("Route_Detail_Id")) = "" & dvScheduleDetails.Item(i)("Stay_Period")
                    drow.Item("Departure_Time_" & dvRouteDetails.Item(j)("Route_Detail_Id")) = "" & dvScheduleDetails.Item(i)("Departure_Time")
                    drow.Item("Travel_Time_" & dvRouteDetails.Item(j)("Route_Detail_Id")) = "" & dvRouteDetails.Item(j)("Travel_Time")


                    If i + 1 < dvScheduleDetails.Count Then
                        If LineNo <> dvScheduleDetails.Item(i + 1)("Sr_No") Then
                            dtSchDetail.Rows.Add(drow)
                        End If
                    Else
                        dtSchDetail.Rows.Add(drow)
                    End If
                    j = j + 1
                Next
            End If
        End If

        'If dtSchDetail.Rows.Count < 1 Then
        'If dtRouteDetails.Rows.Count > 0 Then
        Dim dtrow As DataRow
        dtrow = dtSchDetail.NewRow()
        dtrow.Item(0) = 0 '("Schedule_Id")
        dtrow.Item(1) = 0 ' "Sr_No"

        For i As Integer = 0 To dtRouteDetails.Rows.Count - 1
            ''Schedule Detail Id - leave
            dtrow.Item((i * 7) + 3) = dtRouteDetails.Rows(i).Item("City_ID") ' "City_ID"
            dtrow.Item((i * 7) + 4) = "00:00" 'dtRouteDetails.Rows(i).Item("Arrival_Time") ' "Arrival_Time"
            dtrow.Item((i * 7) + 5) = dtRouteDetails.Rows(i).Item("Stay_Time") ' "Stay_Period"
            dtrow.Item((i * 7) + 6) = "00:00" 'dtRouteDetails.Rows(i).Item("Departure_Time") ' "Departure_Time"
            dtrow.Item((i * 7) + 7) = dtRouteDetails.Rows(i).Item("Travel_Time") ' "Travel_Time"
        Next
        dtSchDetail.Rows.InsertAt(dtrow, 0)
        'End If
        'End If



        'dtRouteDetail = objSchedules.GetAll(CInt(RouteID))

        Dim pk(0) As DataColumn
        pk(0) = dtSchDetail.Columns("Sr_No")
        'pk(0).AutoIncrement = True
        dtSchDetail.PrimaryKey = pk
        dtSchDetail.AcceptChanges()

        'If (dtRouteDetail.Rows.Count > 0) Then
        '    pk(0).AutoIncrementSeed = dtRouteDetail.Rows(dtRouteDetail.Rows.Count - 1).Item("Route_Detail_ID") + 1
        'Else
        '    pk(0).AutoIncrementSeed = 1
        'End If

        If dtSchDetail.Rows.Count > 2 Then
            dtSchDetail.DefaultView.Sort = dtSchDetail.Columns(6).ColumnName.ToString() & " DESC"
        End If


        

        table = dtSchDetail

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

    Private Sub cmbRoutes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbRoutes.SelectedIndexChanged
        Me.loadTable()
        BindScheduleDetail()
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        objSchedule.Code = txtCode.Text.Trim()
        objSchedule.Title = txtTitle.Text.Trim()
        objSchedule.ScheduleDate = dtSchedule.Value
        objSchedule.ScheduleWEF = dtWEF.Value
        objSchedule.RouteID = cmbRoutes.SelectedValue
        objSchedule.Active = True

        objSchedule.Comission = Val(txtComission.Text)
        objSchedule.ExtraFare = Val(txtExtraFare.Text)
        'txtExtraFare

        objSchedule.UserId = objUser.Id
        objSchedule.AccessDate = DateTime.Now
        objSchedule.ComputerName = "PC1"
        objSchedule.TerminalId = objUser.TerminalId

        If hidSchID.Value.Trim() <> "" And hidSchID.Value.Trim() <> "0" Then
            objSchedule.Id = hidSchID.Value.Trim()
            objSchedule.Save(False)
        Else
            objSchedule.Save(True)
            hidSchID.Value = objSchedule.Id
        End If

        cmbRoutes.Enabled = False

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
        Response.Redirect("Schedules.aspx")
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