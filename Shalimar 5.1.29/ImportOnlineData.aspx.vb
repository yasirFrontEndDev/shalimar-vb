Imports System.Net
Imports System.Net.NetworkInformation
Imports FMovers.Ticketing.Entity

Partial Public Class ImportOnlineData
    Inherits System.Web.UI.Page
    Dim objUser As clsUser
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        objUser = CType(Session("CurrentUser"), clsUser)
        lblErr.Visible = False
        lblErr.Text = ""

        If objUser.Is_ImportData = False Then
            Response.Redirect("Permissions.aspx")

        Else

        End If
        If Not IsPostBack Then
            Try




                Dim ping As New Ping
                Dim pingreply As PingReply = ping.Send(FMovers.Ticketing.DAL.Crypto.Decrypt(System.Configuration.ConfigurationManager.AppSettings("ServerIPAddress").ToString, ""))



                If pingreply.Status = IPStatus.Success Then
                    btnImport.Visible = True
                Else

                    btnImport.Visible = False
                    lblErr.Text = "Server is not online"
                    lblErr.Visible = True
                End If





            Catch ex As Exception
                btnImport.Visible = False
                lblErr.Text = "Server is not online"
                lblErr.Visible = True
            End Try
        End If

        If Not IsPostBack Then
            If "" & Request.QueryString("status") <> "" Then
                If "" & Request.QueryString("status") = "success" Then
                    lblErr.Text = "Import Successfully Completed!"
                    lblErr.Visible = True
                ElseIf "" & Request.QueryString("status") = "failed" Then
                    lblErr.Text = "Import is failed. Try again or Contact Software Vendor!"
                    lblErr.Visible = True
                End If
            End If
        End If
    End Sub

    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImport.Click
        If cmbType.SelectedValue <> 0 Then
            If cmbType.SelectedValue = 1 Then
                Response.Redirect("importcities.aspx")
            ElseIf cmbType.SelectedValue = 2 Then
                Response.Redirect("importroutes.aspx")
            ElseIf cmbType.SelectedValue = 3 Then
                Response.Redirect("importrouteschedules.aspx")
            ElseIf cmbType.SelectedValue = 4 Then
                Response.Redirect("importterminals.aspx")
            ElseIf cmbType.SelectedValue = 5 Then
                Response.Redirect("importusers.aspx")
            ElseIf cmbType.SelectedValue = 6 Then
                Response.Redirect("importvehicles.aspx")
            ElseIf cmbType.SelectedValue = 7 Then
                Response.Redirect("importfare.aspx")
            ElseIf cmbType.SelectedValue = 8 Then
                Response.Redirect("importcomission.aspx")
            ElseIf cmbType.SelectedValue = 9 Then
                Response.Redirect("importVehiclecontroler.aspx")
            ElseIf cmbType.SelectedValue = 10 Then
                Response.Redirect("importRefreshment.aspx")

            ElseIf cmbType.SelectedValue = 11 Then
                Response.Redirect("ImportAlertInformation.aspx")

            ElseIf cmbType.SelectedValue = 12 Then
                Response.Redirect("OperatedCompany.aspx")

            End If



            Return

        End If
    End Sub

End Class