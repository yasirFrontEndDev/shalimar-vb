Imports FMovers.Ticketing.Entity

Partial Public Class SelectTicketing
    Inherits System.Web.UI.Page
    Dim objUser As clsUser
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        objUser = CType(Session("CurrentUser"), clsUser)
        Dim mode As String = Request.QueryString("mode")

        If mode = 1 Then

            If objUser.Is_AdvTicketing = 0 Then
                Response.Redirect("Permissions.aspx")

            Else

            End If

        Else
            If objUser.Is_Booking = 0 Then
                Response.Redirect("Permissions.aspx")

            Else

            End If

        End If



    End Sub

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button3.Click
        Dim mode As String = Request.QueryString("mode")
        Response.Redirect("Advance.aspx?mode=" + mode + "")
    End Sub




    Protected Sub Button4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button4.Click
        Dim mode As String = Request.QueryString("mode")
        Response.Redirect("loadcitywise.aspx?mode=" + mode + "")
    End Sub
End Class