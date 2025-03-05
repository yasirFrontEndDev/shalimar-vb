Public Partial Class TicketingMaster
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Write(cboVoucherNo.Rows(0).Cells.FromKey("Ticketing_Schedule_ID").Text)


    End Sub

End Class