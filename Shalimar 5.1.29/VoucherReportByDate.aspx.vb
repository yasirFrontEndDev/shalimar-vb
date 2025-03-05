Partial Public Class VoucherReportByDate
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        dtFrom.CalendarLayout.Culture = FMovers.Ticketing.Entity.clsUtil.GetDateChooserCulture()
        dtTo.CalendarLayout.Culture = FMovers.Ticketing.Entity.clsUtil.GetDateChooserCulture()
  End Sub

  Private Sub btnShowReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowReport.Click
    Session("FromDate") = dtFrom.Value
    Session("ToDate") = dtTo.Value
    Response.Write("<script>window.open('Reports/VoucherReport.aspx')</script>")
  End Sub

End Class