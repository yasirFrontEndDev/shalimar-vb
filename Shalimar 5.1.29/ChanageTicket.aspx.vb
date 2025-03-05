
Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online


Partial Public Class ChanageTicket
    Inherits System.Web.UI.Page
    Dim objTicketing As clsSeatTicketing
    Dim objConnection As Object
    Dim objOnline As eTicketing
    Dim objUser As clsUser

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        End If
        If Not Page.IsPostBack Then
            objUser = CType(Session("CurrentUser"), clsUser)
        End If

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click

        objConnection = ConnectionManager.GetConnection()
        objTicketing = New clsSeatTicketing(objConnection)
        objUser = CType(Session("CurrentUser"), clsUser)

        Dim dt As New DataTable

        dt = objTicketing.getSeatRecord(txtTicketNumber.Text)
        If Not dt Is Nothing Then


            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("Status") <> "4" Then
                    lblMessage.Text = "No Record Found."
                Else

                    objTicketing.Ticketing_Seat_ID = dt.Rows(0)("Ticketing_Seat_ID")
                    objTicketing.IssuedBy = objUser.Id
                    objTicketing.AddMissed("TicketChange")
                    lblMessage.Text = "Record saves sucessfully."


                End If
            Else
                lblMessage.Text = "No Record Found."
            End If
        Else
            lblMessage.Text = "No Record Found."

        End If

    End Sub
End Class