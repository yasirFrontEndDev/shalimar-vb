Imports System.Net
Imports System.Net.NetworkInformation
Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online


Partial Public Class Refund
    Inherits System.Web.UI.Page
    Dim objTicketing As clsSeatTicketing
    Dim objConnection As Object
    Dim objOnline As eTicketing
    Dim objUser As clsUser
    Dim MainDt As New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objUser = CType(Session("CurrentUser"), clsUser)
        If objUser.Is_Refund = False Then
            Response.Redirect("Permissions.aspx")

        Else

        End If


        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If


        If Not Page.IsPostBack Then
            objUser = CType(Session("CurrentUser"), clsUser)
            InitiateLoad_Form_Load()
        End If
        objConnection = ConnectionManager.GetConnection()
        Dim dtCanRefundMiss As Boolean = clsUtil.GetCanRefundMiss(objConnection)

        If dtCanRefundMiss = True Then btnValidate.Visible = False


    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

        objConnection = ConnectionManager.GetConnection()
        objTicketing = New clsSeatTicketing(objConnection)
        objUser = CType(Session("CurrentUser"), clsUser)
        lblMessage.Text = ""

        Dim dt As New DataTable

        For Each line As String In Me.txtTicketNumber.Text.Trim().Split(vbLf)

            dt = objTicketing.getSeatRecord(line.Trim())
            If Not dt Is Nothing Then
                If dt.Rows.Count > 0 Then
                    If dt.Rows(0)("Status") <> "4" Then

                        lblMessage.Text = "No Record Found."
                    Else

                        objTicketing.Ticketing_Seat_ID = dt.Rows(0)("Ticketing_Seat_ID")
                        objTicketing.IssuedBy = objUser.Id

                        PrintReport(dt.Rows(0)("Ticketing_Seat_ID").ToString + "-" + dt.Rows(0)("Seat_No").ToString(), dt.Rows(0)("Passenger_Name").ToString, dt.Rows(0)("Schedule_Title").ToString(), dt.Rows(0)("Source").ToString() _
                                    , dt.Rows(0)("Actual_Departure_Time").ToString, dt.Rows(0)("Destination").ToString, dt.Rows(0)("Seat_No").ToString, dt.Rows(0)("Fare").ToString _
                                    , "", "", "", dt.Rows(0)("AmountRefund"))
                        If ServerPing() Then

                            Dim objOnlineTicketing As eTicketing
                            objOnlineTicketing = New eTicketing()

                            Dim objTicketing As clsTicketing
                            Dim ScheduleID As Integer = dt.Rows(0)("Schedule_ID")

                            objTicketing = New clsTicketing(objConnection)
                            objOnlineTicketing = New eTicketing

                            objTicketing.DepartureTime = "" & dt.Rows(0)("Departure_Time").ToString
                            objTicketing.TSDate = CDate(dt.Rows(0)("TS_Date").ToString)
                            objTicketing.ScheduleID = "" & ScheduleID

                            Dim OnlineSchedule_Id As Integer = objOnlineTicketing.IsOnlineTicketingScheduleOnLoad(objTicketing, Val("" & dt.Rows(0)("ServiceType_Id").ToString), objUser.Vendor_Id)

                            objOnlineTicketing.MakeAvailable_New(OnlineSchedule_Id, dt.Rows(0)("Seat_No"), eTicketStatus.Available, objUser.Id, dt.Rows(0)("Issue_Terminal"))



                        End If

                        objTicketing.AddMissed("TicketRefund")
                        InitiateLoad_Form_Load()


                        lblMessage.Text = "Record saves sucessfully."

                    End If
                Else
                    lblMessage.Text = "No Record Found."
                End If
            Else
                lblMessage.Text = "No Record Found."

            End If
        Next


         

    End Sub


    Private Function ServerPing() As Boolean

        Try
            Dim ping As New Ping
            Dim pingreply As PingReply = ping.Send(FMovers.Ticketing.DAL.Crypto.Decrypt(System.Configuration.ConfigurationManager.AppSettings("ServerIPAddress").ToString, ""))



            If pingreply.Status = IPStatus.Success Then
                Return True
            Else

                Return False
            End If

        Catch ex As Exception
            Return False

        End Try

    End Function

    Private Function PrintReport(ByVal TicketNumber As String, ByVal p_Name As String, ByVal Title As String, _
                                 ByVal Source As String, ByVal p_ActualDepartureTime As String, ByVal Destination As String, ByVal SeatNo As String, _
                                 ByVal Fare As String, ByVal UserName As String, ByVal TerminalName As String, _
                                 ByVal TerminalId As String, ByVal Refund As String) As Boolean

        Try


            Dim oReport As CrystalDecisions.CrystalReports.Engine.ReportDocument
            oReport = New CrystalDecisions.CrystalReports.Engine.ReportDocument
            oReport.Load(Request.PhysicalApplicationPath & "Reports\rptRefund.rpt")
            objUser = CType(Session("CurrentUser"), clsUser)

            Dim objPrintInfo As New clsPrintInfo
            With objPrintInfo

                cReportUtility.PassParameter("TicketNo", TicketNumber, oReport)
                cReportUtility.PassParameter("Name", p_Name, oReport)
                cReportUtility.PassParameter("Source", Source, oReport)
                cReportUtility.PassParameter("Destination", Destination, oReport)
                cReportUtility.PassParameter("Fare", Fare, oReport)
                cReportUtility.PassParameter("UserName", objUser.Id, oReport)
                cReportUtility.PassParameter("SeatNo", SeatNo, oReport)
                cReportUtility.PassParameter("ActualDepartureTime", p_ActualDepartureTime, oReport)
                cReportUtility.PassParameter("Title", Title, oReport)
                cReportUtility.PassParameter("TerminalName", objUser.TerminalName, oReport)
                cReportUtility.PassParameter("Refund", Refund, oReport)

            End With

            Dim i As Integer

            Dim doctoprint As New System.Drawing.Printing.PrintDocument()


            'doctoprint.PrinterSettings.PrinterName = "EPSONLQ3002" 'System.Configuration.ConfigurationManager.AppSettings.Item("PrinterName") '"\\thokar\EpsonLQ-" '(ex. "Epson SQ-1170 ESC/P 2")

            Dim rawKind As Integer

            doctoprint.PrinterSettings.PrinterName = System.Configuration.ConfigurationManager.AppSettings.Item("PrinterName")

            For i = 0 To doctoprint.PrinterSettings.PaperSizes.Count - 1
                If doctoprint.PrinterSettings.PaperSizes(i).PaperName = "FMF TICKET" Then

                    rawKind = CInt(doctoprint.PrinterSettings.PaperSizes(i).GetType().GetField("kind", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic).GetValue(doctoprint.PrinterSettings.PaperSizes(i)))

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

            oReport.Close()
            oReport.Dispose()


            Return True



        Catch ex As Exception

            lblMessage.Text = ex.Message

            Return False

        End Try



    End Function

    Private Sub InitiateLoad_Form_Load()
        Try


            MainDt.Clear()

            MainDt.Columns.Add("Ticketing_Schedule_ID", Type.GetType("System.String"))
            MainDt.Columns.Add("Ticketing_Seat_ID", Type.GetType("System.String"))
            MainDt.Columns.Add("Seat_No", Type.GetType("System.String"))
            MainDt.Columns.Add("Status", Type.GetType("System.String"))
            MainDt.Columns.Add("AmountChange", Type.GetType("System.String"))
            MainDt.Columns.Add("AmountRefund", Type.GetType("System.String"))
            MainDt.Columns.Add("Source", Type.GetType("System.String"))
            MainDt.Columns.Add("Destination", Type.GetType("System.String"))
            MainDt.Columns.Add("Schedule_Title", Type.GetType("System.String"))
            MainDt.Columns.Add("Dep_Time", Type.GetType("System.String"))
            MainDt.Columns.Add("Passenger_Name", Type.GetType("System.String"))
            MainDt.Columns.Add("Contact_No", Type.GetType("System.String"))
            MainDt.Columns.Add("CNIC", Type.GetType("System.String"))
            grdRoutes.DataSource = MainDt
            grdRoutes.DataBind()

        Catch ex As Exception
            lblMessage.Text = "Please contact IT Team ------Error" + ex.Message


        End Try

    End Sub
    Protected Sub btnValidate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnValidate.Click

        objConnection = ConnectionManager.GetConnection()
        objTicketing = New clsSeatTicketing(objConnection)
        objUser = CType(Session("CurrentUser"), clsUser)

        Dim dt As New DataTable
        grdRoutes.DataSource = Nothing
        grdRoutes.DataBind()
        For Each line As String In Me.txtTicketNumber.Text.Split(vbLf)
            If line.Trim() <> "" Then
                dt = objTicketing.getSeatRecord(line.Trim())
                If Not dt Is Nothing Then
                    MainDt.Merge(dt)
                End If
                If Not dt Is Nothing Then
                    If dt.Rows.Count > 0 Then
                        If dt.Rows(0)("Status") <> "4" Then
                            lblMessage.Text = "No Record Found. For ticket number  " & line.ToString()
                            lblMessage.ForeColor = Drawing.Color.Red
                            Exit Sub
                        Else
                        End If
                    Else
                        lblMessage.Text = "No Record Found. For ticket number  " & line.ToString()
                        lblMessage.ForeColor = Drawing.Color.Red

                        Exit Sub
                    End If
                Else
                    lblMessage.Text = "No Record Found. For ticket number  " & line.ToString()
                    lblMessage.ForeColor = Drawing.Color.Red
                    lblMessage.Font.Size = 14



                    Exit Sub
                End If
            End If
        Next

        grdRoutes.DataSource = MainDt
        grdRoutes.DataBind()

        Dim row As DataRow = MainDt.Rows(0)



        lblMessage.Text = "Mr " + row("Passenger_Name").ToString() + " you will be charged Rs." + row("AmountRefund").ToString() + " On your Ticket Refund "
        lblMessage.ForeColor = Drawing.Color.Red
        btnSave.Visible = True


    End Sub

    Protected Sub grdRoutes_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles grdRoutes.InitializeLayout

        grdRoutes.Columns.FromKey("AmountChange").Hidden = True
        grdRoutes.Columns.FromKey("Ticketing_Schedule_ID").Hidden = True
        grdRoutes.Columns.FromKey("Ticketing_Seat_ID").Hidden = True
        grdRoutes.Columns.FromKey("Status").Hidden = True

    End Sub
End Class