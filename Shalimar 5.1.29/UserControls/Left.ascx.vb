
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

Partial Public Class Left

    Inherits System.Web.UI.UserControl

    Dim objBusCharges As clsBusCharges
    Dim objConnection As Object
    Dim objUser As clsUser
    Public UserName As String
    Public DefaultCityId As Integer
    Public CurrentUserID As Integer
    Dim objOnlineTicketing As eTicketing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        LoadBalance()
        If ServerPing() Then
            LoadOnlineTicketing()
        End If

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



    Private Sub SearchPNR()

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

      


        objOnlineTicketing = New eTicketing
       
        Dim Openning As String = objOnlineTicketing.SearchPNR(txtPNRSearch.Text)

        divPNRResult.InnerHtml = Openning



    End Sub
    Private Sub LoadBalance()

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        Else
            objUser = CType(Session("CurrentUser"), clsUser)
            CurrentUserID = "" & objUser.Id
            UserName = objUser.LoginName.ToLower()
        End If

        If 1 = 1 Then


            Dim strClosing As String = ""
            Dim Arr As Array

            objConnection = ConnectionManager.GetConnection()

            objBusCharges = New clsBusCharges(objConnection)
            objBusCharges.UserId = CurrentUserID

            Dim Openning As Integer = objBusCharges.GetOpeningBalance(CurrentUserID)

            strClosing = objBusCharges.GetByTicketingAdvanceById()
            Arr = strClosing.Split("~")

            If Arr.Length > 1 Then

                lblAdance.Text = Arr(0)
                lblCashCollection.Text = Arr(1)
                lblDeduction.Text = Arr(2)
                lblMissed.Text = Arr(3)
                lblRefund.Text = Arr(5)
                lblChange.Text = Arr(4)
                lblTotal.Text = Math.Round(CDbl(Arr(0)) + CDbl(Arr(3)) + CDbl(Arr(4)) + CDbl(Arr(5)) + CDbl(Arr(1)) + CDbl(Arr(2)) + CDbl(Openning.ToString()), 0).ToString()

            Else

                lblAdance.Text = "0"
                lblCashCollection.Text = "0"
                lblDeduction.Text = "0"
                lblTotal.Text = "0"

            End If

            lblOpeniningBal.Text = Openning.ToString()

        End If

    End Sub

    Private Sub LoadOnlineTicketing()

        'Response.Cache.SetCacheability(HttpCacheability.NoCache)

        'Dim dt As DataTable
        'Dim dt_arr As DataTable

        'If Session("CurrentUser") Is Nothing Then
        '    Response.Redirect("UserLogin.aspx")
        'Else
        '    objUser = CType(Session("CurrentUser"), clsUser)
        '    DefaultCityId = objUser.CityId
        'End If

        'objOnlineTicketing = New eTicketing

        'dt = objOnlineTicketing.getOnlineTickeingStatus(DefaultCityId)
        'dt_arr = objOnlineTicketing.getOnlineTickeingStatus_arr(DefaultCityId)

        'For Each dr As DataRow In dt.Rows
        '    CurrentPosition.InnerHtml = CurrentPosition.InnerHtml & "<strong>" & dr("Schedule") & " " & dr("Time") & "</strong>" & "<br />" & " V : " & dr("Registration_No") & " S : " & dr("Sold") & " B : " & dr("Booked")
        '    CurrentPosition.InnerHtml = CurrentPosition.InnerHtml & " <hr >"
        '    CurrentPosition.InnerHtml = CurrentPosition.InnerHtml & vbCrLf
        'Next


        'For Each dr_arr As DataRow In dt_arr.Rows
        '    CurrentPosition_arr.InnerHtml = CurrentPosition_arr.InnerHtml & "<strong>" & dr_arr("Schedule") & " " & dr_arr("Time") & "</strong>" & "<br />" & " V : " & dr_arr("Registration_No") & "<br />" & " City : " & dr_arr("City")
        '    CurrentPosition_arr.InnerHtml = CurrentPosition_arr.InnerHtml & " <hr >"
        '    CurrentPosition_arr.InnerHtml = CurrentPosition_arr.InnerHtml & vbCrLf
        'Next



    End Sub

    Protected Sub btnShowReport_Click1(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        If ServerPing() Then
            SearchPNR()
        Else
            divPNRResult.InnerHtml = "Server is not online"

        End If
    End Sub

    Protected Sub btnShowReport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnShowReport.Click
        LoadBalance()
    End Sub
End Class