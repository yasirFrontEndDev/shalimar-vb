Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online


Partial Public Class SMSSetup
    Inherits System.Web.UI.Page
    Dim objUser As clsUser
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

        objUser = CType(Session("CurrentUser"), clsUser)
        objUser.UpdateSMSSetup(objUser.Id, cboSetup.SelectedItem.Text)
        lblOK.Visible = True

    End Sub
End Class