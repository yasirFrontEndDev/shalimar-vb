Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid

Partial Public Class Vehicles
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objVehicles As clsVehicle
    Private table As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If
        objConnection = ConnectionManager.GetConnection()
        objUser = CType(Session("CurrentUser"), clsUser)
        objVehicles = New clsVehicle(objConnection)

        Me.RegisterClientEvents()

        Me.loadTable()

        If Not Me.IsPostBack Then
            Me.BindGrid()
        End If
    End Sub

    Private Sub Vehicles_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        ConnectionManager.CloseConnection(objConnection)
    End Sub

    Private Sub grdVehicles_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles grdVehicles.InitializeLayout
        CustomizeControl.SetGridLayout(e.Layout.Grid)
        With e.Layout.Grid

            .Columns.FromKey("Vehicle_Id").Hidden = True
            .Columns.FromKey("User_Id").Hidden = True
            .Columns.FromKey("Access_DateTime").Hidden = True
            .Columns.FromKey("Access_SysName").Hidden = True
            .Columns.FromKey("Access_Terminal_Id").Hidden = True
            .Columns.FromKey("Driver_ID").Hidden = True
            .Columns.FromKey("Veh_Code").Hidden = True

            .Columns.FromKey("Veh_Code").Header.Caption = "Code"
            .Columns.FromKey("Veh_Type").Header.Caption = "Type"
            .Columns.FromKey("Veh_Name").Header.Caption = "Name"
            .Columns.FromKey("Seats").Header.Caption = "Seats"
            .Columns.FromKey("IsCommissioned").Header.Caption = "Commissioned"
            .Columns.FromKey("Commission_Rate").Header.Caption = "Comm %"
            .Columns.FromKey("Comm_Owner").Header.Caption = "Comm Owner"
            .Columns.FromKey("Comm_Contact_Person").Header.Caption = "Comm Person"
            .Columns.FromKey("Comm_Contact_No").Header.Caption = "Comm Contact"
            .Columns.FromKey("Registration_No").Header.Caption = "Registration #"
            .Columns.FromKey("Engine_No").Header.Caption = "Engine #"
            .Columns.FromKey("Chasis_No").Header.Caption = "Chasis #"
            .Columns.FromKey("Driver_Name").Header.Caption = "Driver"


            .Columns.FromKey("Veh_Code").Width = Unit.Pixel(100)
            .Columns.FromKey("Veh_Type").Width = Unit.Pixel(100)
            .Columns.FromKey("Active").Width = Unit.Pixel(80)
            .Columns.FromKey("Seats").Width = Unit.Pixel(80)
            .Columns.FromKey("IsCommissioned").Width = Unit.Pixel(100)
            .Columns.FromKey("Commission_Rate").Width = Unit.Pixel(100)
            .Columns.FromKey("Registration_No").Width = Unit.Pixel(100)
            .Columns.FromKey("Engine_No").Width = Unit.Pixel(100)
            .Columns.FromKey("Chasis_No").Width = Unit.Pixel(100)
            .Columns.FromKey("Model").Width = Unit.Pixel(75)
            .Columns.FromKey("Maker").Width = Unit.Pixel(100)
            .Columns.FromKey("Driver_ID").Width = Unit.Pixel(100)
            .Columns.FromKey("Remarks").Width = Unit.Pixel(150)

            '.Columns.FromKey("Vehicle_Type").Type = ColumnType.DropDownList
            '.Columns.FromKey("Driver_ID").Type = ColumnType.DropDownList

        End With

        'Dim valList As New ValueList
        'Dim tbl As DataTable
        ' tbl = (New clsCity(objConnection)).GetCities()

        'For count As Integer = 0 To tbl.Rows.Count - 1
        'valList.ValueListItems.Add(tbl.Rows(count).Item("City_Id"), tbl.Rows(count).Item("City_Name"))
        'Next
        'valList.ValueListItems.Insert(0, New ValueListItem("", 0))

        'valList.DataSource = (New clsCity(objConnection)).GetCities()
        'valList.DisplayMember = "City_Name"
        'valList.ValueMember = "City_ID"
        'valList.DataBind()
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

    Private Sub grdVehicles_DeleteRowBatch(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles grdVehicles.DeleteRowBatch
        Dim trow As DataRow
        Dim pk = table.PrimaryKey(0).ColumnName
        Dim key As Object = e.Row.Cells.FromKey(pk).Value
        trow = table.Rows.Find(key)
        If Not trow Is Nothing Then
            trow.Delete()
        End If
    End Sub

    Private Sub grdVehicles_UpdateGrid(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.UpdateEventArgs) Handles grdVehicles.UpdateGrid
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
                            If UCase(row.Cells(i).Column.Key) = UCase("Commission_Rate") Or UCase(row.Cells(i).Column.Key) = UCase("Driver_ID") Then
                                addedRow(i) = 0
                            ElseIf UCase(row.Cells(i).Column.Key) = "USER_ID" Then
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
                                If UCase(row.Cells(i).Column.Key) = UCase("Commission_Rate") Or UCase(row.Cells(i).Column.Key) = UCase("Driver_ID") Then
                                    ModifiedRow(i) = 0
                                ElseIf UCase(row.Cells(i).Column.Key) = "USER_ID" Then
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
        Dim dtVehicles As DataTable
        Dim getMaxVechile_Id As Integer = 0
        dtVehicles = objVehicles.GetAll()
        getMaxVechile_Id = objVehicles.getMaxVechile_Id()

        Dim pk(0) As DataColumn
        pk(0) = dtVehicles.Columns("Vehicle_Id")
        pk(0).AutoIncrement = True
        dtVehicles.PrimaryKey = pk
        dtVehicles.AcceptChanges()

        If (dtVehicles.Rows.Count > 0) Then
            pk(0).AutoIncrementSeed = getMaxVechile_Id + 1
        Else
            pk(0).AutoIncrementSeed = 1
        End If

        table = dtVehicles

    End Sub

    Private Sub BindGrid()
        grdVehicles.DataSource = table
        grdVehicles.DataBind()
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        For Each dRow As DataRow In table.Rows
            If Not dRow.RowState = DataRowState.Unchanged Then
                objVehicles = New clsVehicle(objConnection)
                If dRow.RowState = DataRowState.Deleted Then
                    objVehicles.Id = dRow.Item("Vehicle_Id", DataRowVersion.Original)
                    objVehicles.Delete()
                Else
                    With objVehicles
                        .Code = "" & dRow.Item("Veh_Code")
                        .Active = dRow.Item("Active")
                        .Type = "" & dRow.Item("Veh_Type")
                        .Name = "" & dRow.Item("Veh_Name")
                        If "" & dRow.Item("Seats") <> "" Then
                            .Seats = dRow.Item("Seats")
                        Else
                            .Seats = 0
                        End If
                        .IsCommissioned = dRow.Item("IsCommissioned")
                        .CommissionRate = "" & dRow.Item("Commission_Rate")
                        .CommOwner = "" & dRow.Item("Comm_Owner")
                        .CommContactPerson = "" & dRow.Item("Comm_Contact_Person")
                        .CommContactNo = "" & dRow.Item("Comm_Contact_No")
                        .RegistrationNo = "" & dRow.Item("Registration_No")
                        .EngineNu = "" & dRow.Item("Engine_No")
                        .ChasisNo = "" & dRow.Item("Chasis_No")
                        .Model = "" & dRow.Item("Model")
                        .Maker = "" & dRow.Item("Maker")
                        'If "" & dRow.Item("Driver_ID") <> "" Then
                        '.DriverID = dRow.Item("Driver_ID")
                        'Else
                        .DriverID = 0
                        'End If
                        .DriverName = "" & dRow.Item("Driver_Name")
                        .Remarks = "" & dRow.Item("Remarks")

                        .UserId = objUser.Id
                        .AccessDate = DateTime.Now
                        .ComputerName = "PC1"
                        .TerminalId = "1"
                    End With
                    If dRow.RowState = DataRowState.Added Then

                        If ValidateVehicle("" & dRow.Item("Registration_No")) = False Then
                            objVehicles.Save(True)
                        Else
                            lblError.Text = "Registration No " & "" & dRow.Item("Registration_No") & " is duplicate. Please resolve."

                        End If



                    ElseIf dRow.RowState = DataRowState.Modified Then
                        objVehicles.Id = "" & dRow.Item("Vehicle_Id")
                        objVehicles.Save(False)
                    End If
                End If

            End If
        Next

        Me.loadTable()
        Me.BindGrid()
    End Sub

    Private Function ValidateVehicle(ByVal Registration_No As String) As Boolean

        'ValidateCity
        Dim ds As DataSet

        ds = objVehicles.ValidateVehicle(Registration_No)
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
End Class