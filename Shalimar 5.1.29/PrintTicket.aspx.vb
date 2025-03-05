Partial Public Class PrintTicket
    Inherits System.Web.UI.Page

    'Dim TicketNo As String
    'Dim PassengerName As String
    'Dim ContractNo As String
    'Dim SeatNo As String
    'Dim Fare As String
    'Dim Route As String
    'Dim DepartureDateTime As String
    'Dim VehicleNo As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If

        lblTicketNo.Text = "" & Request.QueryString("TicketNo")
        lblPassengerName.Text = "" & Request.QueryString("PassengerName")
        lblContactNo.Text = "" & Request.QueryString("ContractNo")
        lblSeatNo.Text = "" & Request.QueryString("SeatNo")
        lblFare.Text = "" & Request.QueryString("Fare")
        lblRoute.Text = "" & Request.QueryString("Route")
        lblDepartureDateTime.Text = "" & Request.QueryString("DepartureDateTime")
        lblVehicleNo.Text = "" & Request.QueryString("VehicleNo")
    End Sub

End Class