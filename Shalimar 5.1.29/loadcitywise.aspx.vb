Public Partial Class loadcitywise
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mode As String = Request.QueryString("mode")
        iframe1.Attributes.Add("src", "SelectCity.aspx?mode=" + mode + "")
    End Sub

End Class