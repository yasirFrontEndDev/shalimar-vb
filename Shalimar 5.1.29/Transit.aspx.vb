Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid
Imports System.Data

Partial Public Class Transit
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objScheduleList As clsSchedules
    Dim objTicketing As clsTicketing
    Private table As DataTable
    Dim dtVehicle As DataTable
    Dim mode As String

#Region " Form Events "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If


        objConnection = ConnectionManager.GetConnection()
        objUser = CType(Session("CurrentUser"), clsUser)

        objScheduleList = New clsSchedules(objConnection)

        objTicketing = New clsTicketing(objConnection)

        dtVehicle = (New clsVehicle(objConnection)).GetAll()

        dtSchedule.CalendarLayout.Culture = clsUtil.GetDateChooserCulture()


        'Call RegisterClientEvents()

        If Not Me.IsPostBack Then
            loadCombos()
            loadComboVechile(0)
            If cboRoute.Text <> "" Then
                Call BindTicketingRoute()
            End If
        End If

    End Sub

    Private Sub UserLogin_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        ConnectionManager.CloseConnection(objConnection)
    End Sub

#End Region

#Region " Control Events "



#End Region

#Region " Functions And Procedure  "

    Private Sub loadCombos()

        cboRoute.DataSource = objScheduleList.GetAll() '.get.GetRoute()

        cboRoute.DataValueField = "Schedule_Id"
        cboRoute.DataTextField = "Schedule_Title"
        cboRoute.DataBind()

        cboRoute.Items.Insert(0, New ListItem("Select", "0"))




    End Sub

    Private Sub loadTable()
        Try


            Dim count = 0
            Dim dtVoucher As New DataTable
            Dim dtVouchertemp As New DataTable

            dtVouchertemp = objTicketing.GetTicketingSchedule2(cboRoute.SelectedValue, dtSchedule.Text, 3)

            cboTime.Items.Clear()

            cboTime.DataSource = dtVouchertemp

            cboTime.DataValueField = "Ticketing_Schedule_ID"
            cboTime.DataTextField = "Departure_Time"
            cboTime.DataBind()

            cboTime.Items.Insert(0, New ListItem("Select", "0"))

        Catch ex As Exception
            Response.Write(ex.Message)

        End Try


    End Sub

    Private Sub BindTicketingRoute()

    End Sub

    Private Sub RegisterClientEvents()
        'btnSave.Attributes.Add("onclick", "return validation();")
        'btnSave.Style.Add("display", "none")
    End Sub

#End Region




    Private Sub dtSchedule_ValueChanged(ByVal sender As Object, ByVal e As Infragistics.WebUI.WebSchedule.WebDateChooser.WebDateChooserEventArgs) Handles dtSchedule.ValueChanged
        If cboRoute.SelectedValue <> "0" Then
            objTicketing.CreateTicketingSchedule2(cboRoute.SelectedValue, dtSchedule.Value, "")
            loadTable()
            Me.BindTicketingRoute()


        End If
    End Sub

    Private Sub cboRoute_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboRoute.SelectedIndexChanged
        If cboRoute.SelectedValue <> "0" Then
            objTicketing.CreateTicketingSchedule(cboRoute.SelectedValue, dtSchedule.Value, "")
            loadTable()
            Me.BindTicketingRoute()



        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click

        Button1.Enabled = False
        objTicketing.UserId = objUser.Id
        objTicketing.ComputerName = "System"
        objTicketing.AccessTerminalId = 1


        With objTicketing
            .Id = cboTime.SelectedValue

            If (Not IsDBNull(.Id)) AndAlso .Id <> 0 Then
                .GetById()
            End If

            .ScheduleID = cboRoute.SelectedValue

            .SerialNo = SerialNo.Value
            .DepartureTime = cboTime.SelectedItem.Text

            If Vehicle_ID.Value <> "" Then
                .VehicleID = Vehicle_ID.Value
            Else
                .VehicleID = 81
            End If

            .DriverName = Driver_Name.Value
            .HostessName = Hostess_Name.Value

        End With

        objTicketing.Save(False)


        Button1.Enabled = True

        'For Each dRow As DataRow In dtVehicle.Rows

        '    If "" & dRow.Item("Ticketing_Schedule_ID") = cboTime.SelectedValue Then

        '        With objTicketing
        '            .Id = dRow.Item("Ticketing_Schedule_ID")

        '            If (Not IsDBNull(.Id)) AndAlso .Id <> 0 Then
        '                .GetById()
        '            End If

        '            .ScheduleID = cboRoute.SelectedValue

        '            .SerialNo = dRow.Item("Sr_No")
        '            .DepartureTime = dRow.Item("Departure_Time")

        '            If "" & dRow.Item("Vehicle_ID") <> "" Then
        '                .VehicleID = dRow.Item("Vehicle_ID")
        '            Else
        '                .VehicleID = 81
        '            End If

        '            .DriverName = "" & dRow.Item("Driver_Name")
        '            .HostessName = "" & dRow.Item("Hostess_Name")

        '        End With

        '        objTicketing.Save(False)
        '        Exit For
        '    End If
        'Next

        '///////////////////////////////////////////////////////////////////////////////////////




    End Sub

    Protected Sub cboTime_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboTime.SelectedIndexChanged
        If cboTime.SelectedValue <> "0" Then
            Dim newDT As DataTable
            objTicketing.Id = cboTime.SelectedItem.Value
            newDT = objTicketing.SP_GetVoucher_SINGLE()

            If newDT.Rows.Count > 0 Then

                SerialNo.Value = "" & newDT.Rows(0)(0)
                Driver_Name.Value = "" & newDT.Rows(0)(1)
                Hostess_Name.Value = "" & newDT.Rows(0)(2)
                Vehicle_ID.Value = "" & newDT.Rows(0)(3)

            End If


        End If
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        Try
            lblGenError.Text = ""
            lblGenOK.Text = ""
            Dim ReturnString As String = objTicketing.ValidateTransit(Convert.ToInt64(Request.QueryString("TS_ID").ToString()), cboTime.SelectedValue)

            If (ReturnString <> "") Then

                lblGenError.Text = "Following seats(s) conflict is occured. " & ReturnString & " Seats(s) have been already sold please clear that error "

            Else

                With objTicketing
                    .Id = cboTime.SelectedValue

                    If (Not IsDBNull(.Id)) AndAlso .Id <> 0 Then
                        .GetById()
                    End If

                    .ScheduleID = cboRoute.SelectedValue

                    .SerialNo = SerialNo.Value
                    .DepartureTime = cboTime.SelectedItem.Text

                    If Vehicle_ID.Value <> "" Then
                        .VehicleID = Vehicle_ID.Value
                    Else
                        .VehicleID = 81
                    End If

                    .DriverName = Driver_Name.Value
                    .HostessName = Hostess_Name.Value

                End With

                objTicketing.Save(False)

                objTicketing.GenerateTransit(Convert.ToInt64(Request.QueryString("TS_ID").ToString()), cboTime.SelectedValue, objUser.Id)
                objTicketing.UserId = objUser.Id
                objTicketing.ComputerName = "System"
                objTicketing.AccessTerminalId = 1

                If chkPrint.Checked Then
                    PrintReport()
                End If

                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Script", " LoadActualVoucher(" & cboTime.SelectedValue & ");", True)

            End If

        Catch ex As Exception
            lblGenError.Text = ex.Message
        End Try

    End Sub
    Private Sub loadComboVechile(ByVal VId As Integer)

        Dim tbVechile As New DataTable
        Dim objVehicle As clsVehicle
        objVehicle = New clsVehicle(objConnection)
        tbVechile = objVehicle.GetAll()

        cboVechile.DataSource = tbVechile
        cboVechile.DataValueField = "Vehicle_ID"
        cboVechile.DataTextField = "Registration_No"
        cboVechile.DataBind()




    End Sub


    Private Function PrintReport() As Boolean

        Try

            Dim oReport As CrystalDecisions.CrystalReports.Engine.ReportDocument
            oReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument
            oReport.Load(Request.PhysicalApplicationPath & "Reports\rptPrintTicket.rpt")
            objUser = CType(Session("CurrentUser"), clsUser)

            Dim CurrentUserID As String = "" & objUser.Id
            Dim UserName As String = objUser.Id

            Dim dtDatTable As DataTable = objTicketing.SP_GetDataForTransitPrinting(Convert.ToInt64(cboTime.SelectedValue))

            For Each dr As DataRow In dtDatTable.Rows

                Dim objPrintInfo As New clsPrintInfo
                With objPrintInfo

                    .TicketingScheduleID = cboTime.SelectedValue
                    .TicketNumber = dr("Ticket_Sr_No").ToString()
                    .PassengerName = dr("Passenger_Name").ToString()
                    .ContactNumber = dr("Contact_No").ToString()
                    .SeatNumber = dr("Seat_No").ToString()
                    .Fare = dr("Fare").ToString()
                    .Route = cboRoute.SelectedItem.Text
                    .DepartureTime = cboTime.SelectedItem.Text
                    .VehicleNumber = cboVechile.SelectedItem.Text

                    cReportUtility.PassParameter("TicketNo", cboTime.SelectedValue, oReport)
                    cReportUtility.PassParameter("Name", .PassengerName, oReport)
                    cReportUtility.PassParameter("Source", dr("Source_Name").ToString(), oReport)
                    cReportUtility.PassParameter("Destination", dr("City_Name").ToString(), oReport)
                    cReportUtility.PassParameter("TDate", dtSchedule.Text, oReport)
                    cReportUtility.PassParameter("TTime", cboTime.SelectedItem.Text, oReport)
                    cReportUtility.PassParameter("Fare", .Fare, oReport)
                    cReportUtility.PassParameter("CoachNo", cboVechile.SelectedItem.Text, oReport)
                    cReportUtility.PassParameter("SeatNo", .SeatNumber, oReport)
                    cReportUtility.PassParameter("ActualDepartureTime", cboTime.SelectedItem.Text, oReport)
                    cReportUtility.PassParameter("UserName", UserName, oReport)
                    cReportUtility.PassParameter("PrintDate", Now.Date.Day & "/" & Format(Now.Date.Month, "00") & "/" & Now.Date.Year, oReport)
                    cReportUtility.PassParameter("RePrint", "Transit", oReport)
                    cReportUtility.PassParameter("TerminalName", objUser.TerminalName, oReport)
                    cReportUtility.PassParameter("DropAt", "", oReport)
                End With

                Dim i As Integer
                Dim doctoprint As New System.Drawing.Printing.PrintDocument()

                Dim rawKind As Integer

                doctoprint.PrinterSettings.PrinterName = System.Configuration.ConfigurationManager.AppSettings.Item("PrinterName")

                For i = 0 To doctoprint.PrinterSettings.PaperSizes.Count - 1
                    If doctoprint.PrinterSettings.PaperSizes(i).PaperName = "FMF TICKET" Then

                        rawKind = CInt(doctoprint.PrinterSettings.PaperSizes(i).GetType().GetField("kind", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic).GetValue(doctoprint.PrinterSettings.PaperSizes(i)))

                        'lblErr.Text = rawKind

                        oReport.PrintOptions.PaperSize = rawKind

                        Exit For
                    End If
                Next

                If rawKind = 0 Then
                    rawKind = 119

                    oReport.PrintOptions.PaperSize = rawKind
                Else
                    oReport.PrintOptions.PaperSize = rawKind
                End If

                oReport.PrintOptions.PrinterName = System.Configuration.ConfigurationManager.AppSettings.Item("PrinterName")

                oReport.PrintToPrinter(1, False, 0, 0)


            Next

            oReport.Close()
            oReport.Dispose()

            Return True

        Catch ex As Exception
            lblGenError.Text = ex.Message

            Return False

        End Try



    End Function
End Class