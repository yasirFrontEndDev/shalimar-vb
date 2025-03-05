Partial Public Class Advance
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim mode As String = Request.QueryString("mode")
        myframe.Attributes.Add("src", "TicketingSchedule.aspx?mode=" + mode + "")
    End Sub

End Class