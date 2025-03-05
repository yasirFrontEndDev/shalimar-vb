Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity

Partial Public Class CommissionReportByDate
    Inherits System.Web.UI.Page


    Dim objConnection As Object

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objConnection = ConnectionManager.GetConnection()

        dtFrom.CalendarLayout.Culture = FMovers.Ticketing.Entity.clsUtil.GetDateChooserCulture()
        dtTo.CalendarLayout.Culture = FMovers.Ticketing.Entity.clsUtil.GetDateChooserCulture()

        If Not IsPostBack Then
            Call getUsers()
        End If

    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        ConnectionManager.CloseConnection(objConnection)
    End Sub

    Private Sub getUsers()
        Dim dt As DataTable = clsUtil.GetAllUsers(objConnection)

        If Not dt Is Nothing Then
            If dt.Rows.Count > 0 Then
                cboUser.DataSource = dt
                cboUser.DataValueField = "User_ID"
                cboUser.DataTextField = "User_Name"
                cboUser.DataBind()
                cboUser.Items.Insert("0", New ListItem("", "-1"))
            End If
        End If
    End Sub

  Private Sub btnShowReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowReport.Click

        Session("FromDate") = dtFrom.Value
        Session("ToDate") = dtTo.Value
        Session("UserID") = cboUser.SelectedValue
        Response.Write("<script>window.open('Reports/CommissionReport.aspx')</script>")

    End Sub

End Class