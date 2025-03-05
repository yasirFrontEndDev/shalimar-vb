
Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online
Imports Infragistics.WebUI.UltraWebGrid
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Web
Imports System.Text

Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine

Imports GsmComm.PduConverter
Imports GsmComm.GsmCommunication
Imports System.Drawing.Printing

Imports System.Management

Partial Public Class SearResults
    Inherits System.Web.UI.Page



    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objScheduleList As clsSchedules

    Dim objServiceType As clsServiceType

    Dim objTicketing As clsTicketing
    Dim objOnlineTicketing As eTicketing

    Private table As DataTable
    Dim dtVehicle As DataTable
    Dim mode As String
    Dim objTicketingSeat As clsSeatTicketing

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try



            objConnection = ConnectionManager.GetConnection()
            objTicketing = New clsTicketing(objConnection)
            objTicketingSeat = New clsSeatTicketing(objConnection)
          
            If Request.QueryString("type") = 4 Then

                objOnlineTicketing = New eTicketing
                Dim dtOnlineSearch As DataTable


                dtOnlineSearch = objOnlineTicketing.getInvoiceSearch(Request.QueryString("value"))

                If Not dtOnlineSearch Is Nothing Then


                    If dtOnlineSearch.Rows.Count <= 0 Then
                        lblMessage.Text = "No record found for invoice number " & Request.QueryString("value") & ""
                    Else


                        objTicketing.CreateTicketingScheduleWithServiceType(dtOnlineSearch.Rows(0)("Schedule_Id"), CDate(dtOnlineSearch.Rows(0)("ts_date")), _
                                                            dtOnlineSearch.Rows(0)("Departure_Time"), dtOnlineSearch.Rows(0)("ServiceType_Id"))


                        table = objTicketingSeat.getSingleVoucher(dtOnlineSearch.Rows(0)("Schedule_Id"), CDate(dtOnlineSearch.Rows(0)("ts_date")), _
                                                            dtOnlineSearch.Rows(0)("Departure_Time"), dtOnlineSearch.Rows(0)("ServiceType_Id"), dtOnlineSearch.Rows(0)("Passenger_Name"), dtOnlineSearch.Rows(0)("Contact_No"), dtOnlineSearch.Rows(0)("CNIC"))




                        If Not table Is Nothing Then
                            If table.Rows.Count = 0 Then
                                lblMessage.Text = "No record found for invoice number " & Request.QueryString("value") & ""
                            Else
                                Me.BindTicketingRoute()

                            End If
                        Else
                            lblMessage.Text = "No record found for invoice number " & Request.QueryString("value") & ""
                        End If

                    End If


                Else
                    lblMessage.Text = "Server is not online. Please check your internet connection."
                End If



            Else
                table = objTicketing.GetVoucherDataByNumber(Request.QueryString("value"), Request.QueryString("type"))

                If Not table Is Nothing Then
                    If table.Rows.Count = 0 Then
                        lblMessage.Text = "No Record Found."
                    Else
                        Me.BindTicketingRoute()

                    End If
                Else
                    lblMessage.Text = "No Record Found."
                End If

            End If


        Catch ex As Exception
            lblMessage.Text = ex.Message
        End Try


    End Sub

    Private Sub BindTicketingRoute()


        Dim col1 As New DataColumn
        Dim col2 As New DataColumn
        Dim col3 As New DataColumn

        If Request.QueryString("type") = 1 Or Request.QueryString("type") = 2 Then

            col1.ColumnName = "Load Terminal Voucher"
            col2.ColumnName = "Load City Voucher"
            col3.ColumnName = "Load All Route Voucher"
            table.Columns.Add(col1)
            table.Columns.Add(col2)
            table.Columns.Add(col3)
        ElseIf Request.QueryString("type") = 1 Or Request.QueryString("type") = 3 Then

            col1.ColumnName = "Load User Closing"
            col2.ColumnName = "Load Advance Report"

            table.Columns.Add(col1)
            table.Columns.Add(col2)

        ElseIf Request.QueryString("type") = 4 Then

            col1.ColumnName = "Load Voucher"
            table.Columns.Add(col1)

        End If

        table.AcceptChanges()
        grdMain.DataSource = table
        grdMain.DataBind()



    End Sub
     

     

     
    Protected Sub grdMain_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdMain.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            If Request.QueryString("type") = 1 Or Request.QueryString("type") = 2 Then

                e.Row.Cells(8).Text = "<a href='#' onclick ='return LoadVoucher (" & e.Row.Cells(0).Text & ",1);' > Click here to load</a>"
                e.Row.Cells(9).Text = "<a href='#' onclick ='return LoadVoucher (" & e.Row.Cells(0).Text & ",2);' > Click here to load</a>"
                e.Row.Cells(10).Text = "<a href='#' onclick ='return LoadVoucher (" & e.Row.Cells(0).Text & ",3);' > Click here to load</a>"
            ElseIf Request.QueryString("type") = 4 Then

                e.Row.Cells(0).Text = "<a href='#' onclick ='return LoadTSById (" & e.Row.Cells(0).Text & ",1);' > " & e.Row.Cells(0).Text & "</a>"
                '     e.Row.Cells(8).Text = "<a href='#' onclick ='return LoadTSById (" & e.Row.Cells(0).Text & ",1);' > Click here to load</a>"

                'e.Row.Cells(10).Text = "<a href='#' onclick ='"Ticketing.aspx?mode=2&TSID=" & hidTSID.Value.Trim()' > Click here to load</a>"

            ElseIf Request.QueryString("type") = 1 Or Request.QueryString("type") = 3 Then

                e.Row.Cells(4).Text = "<a href='#' onclick ='return LoadUserClosing (" & e.Row.Cells(0).Text & ",1);' > Click here to load</a>"
                e.Row.Cells(5).Text = "<a href='#' onclick ='return LoadAdvance (" & e.Row.Cells(0).Text & ",2);' > Click here to load</a>"

            End If


        End If

    End Sub
End Class