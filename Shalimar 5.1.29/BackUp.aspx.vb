Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity

Partial Public Class BackUp

    Inherits System.Web.UI.Page
    Dim objTicketing As clsSeatTicketing
    Dim objConnection As Object

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnShowReport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnShowReport.Click

        objConnection = ConnectionManager.GetConnection()
        objTicketing = New clsSeatTicketing(objConnection)

        objTicketing.BackUp()
        Label1.Visible = True


    End Sub
End Class