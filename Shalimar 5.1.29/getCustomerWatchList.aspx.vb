
Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online


Partial Public Class getCustomerWatchList
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objTicketingSeat As clsSeatTicketing
    Dim objTicketingSeatList As clsSeatTicketingList
    Dim objFare As clsFare
    Dim Veicle_id As Integer
    Dim Schedule_ID As String
    Dim objVehicle As clsVehicle
    Dim objTicketing As clsTicketing
    Dim TicketingScheduleId As String
    Dim objOnlineTicketing As eTicketing
    Public UserName As String
    Public CurrentUserID As String
    Public TerminalId As String
    Public dtRouteList As DataTable
    Public dtRouteListAll As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        'Dim dtReturn As New DataTable
        'Dim dsReturn As New DataSet

        'Dim dtReturnOnline As New DataTable
        'objOnlineTicketing = New eTicketing

        'Dim objCustomers As New clsWatchList(objConnection)

        'If Request.QueryString("type") = "cnic" Then
        '    dsReturn = objTicketingSeat.validateWatchList("", Request.QueryString("ContactNo"))
        '    dtReturn = dsReturn.Tables(0)
        'Else
        '    dsReturn = objTicketingSeat.validateWatchList(Request.QueryString("ContactNo"), "")
        '    dtReturn = dsReturn.Tables(1)

        'End If

        'If Not dtReturn Is Nothing Then
        '    If dtReturn.Rows.Count > 0 Then
        '        Response.Write("Found It.")
        '    Else
        '        If ServerPing() Then
        '            dtReturnOnline = objOnlineTicketing.ValidateCustomer(Request.QueryString("ContactNo"))
        '            If Not dtReturnOnline Is Nothing Then
        '                If dtReturnOnline.Rows.Count > 0 Then
        '                    Response.Write("Found It.")
        '                    objCustomers.ImportRecord(dtReturnOnline)
        '                Else
        '                    Response.Write("")
        '                End If
        '            Else
        '                Response.Write("")
        '            End If
        '        Else
        '            Response.Write("")

        '        End If

        '    End If
        'Else
        '    Response.Write("")

        '    'lblErr.Text = "Customer Not Found !"
        '    'hndCustID.Value = 0
        '    'txtPassengerName.Text = ""
        '    'txtCNIC2.Text = ""
        '    'txtContactNo.Text = ""

        'End If
    End Sub


    'Private Function ServerPing() As Boolean

    '    Try
    '        Dim ping As New Ping
    '        Dim pingreply As PingReply = ping.Send(FMovers.Ticketing.DAL.Crypto.Decrypt(System.Configuration.ConfigurationManager.AppSettings("ServerIPAddress").ToString, ""))



    '        If pingreply.Status = IPStatus.Success Then
    '            Return True
    '        Else

    '            Return False
    '        End If

    '    Catch ex As Exception
    '        Return False

    '    End Try


    'End Function
End Class