Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online


Partial Public Class changepassword
    Inherits System.Web.UI.Page
    Dim objUser As clsUser
    Dim objOnline As eTicketing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        objUser = CType(Session("CurrentUser"), clsUser)
        If objUser.Is_ChangePassword = False Then
            Response.Redirect("Permissions.aspx")

        Else

        End If

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If
        If Not Page.IsPostBack Then
            objUser = CType(Session("CurrentUser"), clsUser)
            txtOldPass.Value = objUser.Password
        End If
        'If Page.IsPostBack = False Then
        '    Dim objUser As clsUser
        '    objUser = CType(Session("CurrentUser"), clsUser)
        '    Response.Write(objUser.Id)

        'End If

    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click


        objUser = CType(Session("CurrentUser"), clsUser)
        objUser.UpdatePassword(objUser.Id, txtPassword.Text)
        objOnline = New eTicketing()

        objOnline.UpdatePassword(objUser.Id, txtPassword.Text)

    End Sub
End Class