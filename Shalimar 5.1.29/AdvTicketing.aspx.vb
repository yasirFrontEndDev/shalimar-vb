Public Partial Class AdvTicketing
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Redirect("Ticketing.aspx?mode=3")
    End Sub

End Class