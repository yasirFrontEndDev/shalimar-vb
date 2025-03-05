Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online
Imports System.Net
Imports System.IO
Imports System.Net.NetworkInformation

Partial Public Class header
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load





        If Not Page.IsPostBack Then
            ' imgRefresh_Click(sender, e)
            ServerPing()
        End If


        Dim objUser As clsUser
        objUser = CType(Session("CurrentUser"), clsUser)

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        Else
            objUser = CType(Session("CurrentUser"), clsUser)
        End If
        lblName.Text = objUser.LoginName
    End Sub


    Protected Sub imgRefresh_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgRefresh.Click

        ServerPing()

    End Sub


    Private Sub ServerPing()

        Try
            Dim ping As New Ping
            Dim pingreply As PingReply = ping.Send(FMovers.Ticketing.DAL.Crypto.Decrypt(System.Configuration.ConfigurationManager.AppSettings("ServerIPAddress").ToString, ""))


            If CInt(pingreply.RoundtripTime) > 0 Then
                If CInt(pingreply.RoundtripTime) <= 50 Then
                    imgServerStatus.ImageUrl = "~/images/signals/verygood.png"
                    lblServer.Text = "Very good"
                ElseIf CInt(pingreply.RoundtripTime) <= 100 Then
                    imgServerStatus.ImageUrl = "~/images/signals/good.png"
                    lblServer.Text = "Good"

                ElseIf CInt(pingreply.RoundtripTime) <= 200 Then
                    imgServerStatus.ImageUrl = "~/images/signals/average.png"
                    lblServer.Text = "Aerage"

                ElseIf CInt(pingreply.RoundtripTime) <= 250 Then
                    imgServerStatus.ImageUrl = "~/images/signals/week.png"
                    lblServer.Text = "Weak"

                ElseIf CInt(pingreply.RoundtripTime) <= 300 Then
                    imgServerStatus.ImageUrl = "~/images/signals/veryweek.png"
                    lblServer.Text = "Very Weak"

                ElseIf CInt(pingreply.RoundtripTime) <= 500 Then
                    imgServerStatus.ImageUrl = "~/images/signals/veryweek.png"
                    lblServer.Text = "Huge Very Weak"
                Else

                    imgServerStatus.ImageUrl = "~/images/signals/veryweek.png"
                    lblServer.Text = "Huge Very Weak"

                End If
            Else
                If pingreply.Status = IPStatus.Success Then
                    imgServerStatus.ImageUrl = "~/images/signals/verygood.png"
                    lblServer.Text = "Very good"
                Else

                    imgServerStatus.ImageUrl = "~/images/signals/warning.png"
                    lblServer.Text = "Server Not connected"

                End If

            End If
        Catch ex As Exception
            imgServerStatus.ImageUrl = "~/images/signals/warning.png"
            lblServer.Text = "Server Not connected"

        End Try

    End Sub
End Class