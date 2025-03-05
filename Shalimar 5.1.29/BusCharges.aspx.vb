Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid

Partial Public Class TicketingDeduction
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objBusCharges As clsBusCharges

#Region " Form Events "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If

        objConnection = ConnectionManager.GetConnection()
        objUser = CType(Session("CurrentUser"), clsUser)
        objBusCharges = New clsBusCharges(objConnection)

        Call RegisterClientEvents()

    End Sub

    Private Sub UserLogin_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        ConnectionManager.CloseConnection(objConnection)
    End Sub

#End Region

#Region " Control Events "

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

        With objBusCharges
            '.VoucherNo = ""
            .HostessSalary = txtHostessSalary.Text
            .DriverSalary = txtDriverSalary.Text
            .GuardSalary = txtGuardSalary.Text
            .ServiceCharges = txtServiceCharges.Text
            .CleaningCharges = txtCleaningCharges.Text
            .HookCharges = txtHookCharges.Text
            .BusCharges = txtBusCharges.Text
            .Refreshment = txtRefreshment.Text
            .Toll = txtToll.Text

            If .Id Is Nothing Then
                .Save(True)
            Else
                .Save(False)
            End If

        End With
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'Response.Redirect("Routes.aspx")
    End Sub

#End Region

#Region " Functions And Procedure  "

    Private Sub RegisterClientEvents()
        'btnSave.Attributes.Add("onclick", "return validation();")
        txtHostessSalary.Attributes.Add("onblur", "updateTotal();")
        txtDriverSalary.Attributes.Add("onblur", "updateTotal();")
    End Sub

#End Region

End Class