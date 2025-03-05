Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online
Imports Infragistics.WebUI.UltraWebGrid
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Web
Imports System.Text

Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine

Imports GsmComm.PduConverter
Imports GsmComm.GsmCommunication
Imports System.Drawing.Printing


Partial Public Class UserClosingReport1
    Inherits System.Web.UI.Page

    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objTicketingSeat As clsSeatTicketing
    Dim objTicketingSeatList As clsSeatTicketingList
    Dim objFare As clsFare
    Dim Veicle_id As Integer
    Dim Schedule_ID As String
    Dim objVehicle As clsVehicle
    Dim objTicketing As clsTicketing
    Dim TicketingScheduleId As String
    Dim objOnlineTicketing As eTicketing
    Public UserName As String
    Public CurrentUserID As String


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            If Session("CurrentUser") Is Nothing Then
                Response.Redirect("UserLogin.aspx")
            Else
                objUser = CType(Session("CurrentUser"), clsUser)
                UserId.Value = "" & objUser.Id
            End If
            loadCombos()

        End If
    End Sub
    Private Sub loadCombos()

        objConnection = ConnectionManager.GetConnection()
        objTicketing = New clsTicketing(objConnection)
        Dim dtDataTable As New DataTable

        dtDataTable = objTicketing.getClosingDates(CInt(Val(UserId.Value)))
        cboDate.Items.Clear()
        cboDate.DataTextField = "Dates"
        cboDate.DataValueField = "Book_Id"
        cboDate.DataSource = dtDataTable
        cboDate.DataBind()


    End Sub


End Class