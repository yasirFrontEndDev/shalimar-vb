Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid
Imports System.Net.NetworkInformation
Imports FMovers.Ticketing.Online
Imports System.Reflection
Partial Public Class main
    Inherits System.Web.UI.MasterPage
    Dim objUser As clsUser


    Dim objBusCharges As clsBusCharges
    Dim objConnection As Object

    Public UserName As String
    Public DefaultCityId As Integer
    Public CurrentUserID As Integer
    Dim objOnlineTicketing As eTicketing
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If

        objUser = CType(Session("CurrentUser"), clsUser)
        Dim versionNumber As Version
        versionNumber = Assembly.GetExecutingAssembly().GetName().Version
        lblUserName.Text = objUser.FullName & " (" & objUser.CompanyName & " )"

        lblTerminalName.Text = objUser.TerminalName & " V : " & versionNumber.ToString()
        '  LoadBalance()
        ServerPing()
    End Sub

    Private Sub ServerPing()

        Try
            Dim ping As New Ping
            Dim pingreply As PingReply = ping.Send(FMovers.Ticketing.DAL.Crypto.Decrypt(System.Configuration.ConfigurationManager.AppSettings("ServerIPAddress").ToString, ""))


            If CInt(pingreply.RoundtripTime) > 0 Then
                If CInt(pingreply.RoundtripTime) <= 50 Then
                    imgServerStatus.ImageUrl = "~/images/signals/verygood.png"
                    ' ' lblServer.Text = "Very good"
                ElseIf CInt(pingreply.RoundtripTime) <= 100 Then
                    imgServerStatus.ImageUrl = "~/images/signals/good.png"
                    ' lblServer.Text = "Good"

                ElseIf CInt(pingreply.RoundtripTime) <= 200 Then
                    imgServerStatus.ImageUrl = "~/images/signals/average.png"
                    ' lblServer.Text = "Aerage"

                ElseIf CInt(pingreply.RoundtripTime) <= 250 Then
                    imgServerStatus.ImageUrl = "~/images/signals/week.png"
                    ' lblServer.Text = "Weak"

                ElseIf CInt(pingreply.RoundtripTime) <= 300 Then
                    imgServerStatus.ImageUrl = "~/images/signals/veryweek.png"
                    ' lblServer.Text = "Very Weak"

                ElseIf CInt(pingreply.RoundtripTime) <= 500 Then
                    imgServerStatus.ImageUrl = "~/images/signals/veryweek.png"
                    ' lblServer.Text = "Huge Very Weak"
                Else

                    imgServerStatus.ImageUrl = "~/images/signals/veryweek.png"
                    ' lblServer.Text = "Huge Very Weak"

                End If
            Else
                If pingreply.Status = IPStatus.Success Then
                    imgServerStatus.ImageUrl = "~/images/signals/verygood.png"
                    ' lblServer.Text = "Very good"
                Else

                    imgServerStatus.ImageUrl = "~/images/signals/warning.png"
                    ' lblServer.Text = "Server Not connected"

                End If

            End If
        Catch ex As Exception
            imgServerStatus.ImageUrl = "~/images/signals/warning.png"
            ' lblServer.Text = "Server Not connected"

        End Try

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

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click

        Response.Redirect("SearResults.aspx?type=" & cboType.SelectedValue & "&value=" & txtSearch.Text & "")

    End Sub
End Class