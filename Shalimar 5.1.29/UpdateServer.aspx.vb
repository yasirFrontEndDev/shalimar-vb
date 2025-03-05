Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports Infragistics.WebUI.UltraWebGrid
Imports FMovers.Ticketing.Online


Partial Public Class UpdateServer

    Inherits System.Web.UI.Page

    Dim objScheduleList As clsSchedules
    Dim objConnection As Object
    Dim objConnectionOnline As Object
    Dim objUser As clsUser
    Dim objTicketing As clsTicketing
    Private table As DataTable
    Dim dtVehicle As DataTable
    Dim mode As String
    Dim objOnlineTicketing As eTicketing



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        dtFrom.CalendarLayout.Culture = FMovers.Ticketing.Entity.clsUtil.GetDateChooserCulture()
        dtTo.CalendarLayout.Culture = FMovers.Ticketing.Entity.clsUtil.GetDateChooserCulture()
        If Page.IsPostBack = False Then

            objConnectionOnline = eConnectionManager.GetConnection()

            objConnection = ConnectionManager.GetConnection()

        End If

    End Sub

    Private Sub btnShowReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowReport.Click

        'Response.Write(FormateDate(CDate(dtFrom.Value).ToString("dd/MM/yyyy")))
        'Response.Write("<br />")
        'Response.Write(FormateDate(CDate(dtTo.Value).ToString("dd/MM/yyyy")))
        Dim dtOnline As New DataTable
        Dim dtLocal As New DataTable

        objOnlineTicketing = New eTicketing
        objTicketing = New clsTicketing(objConnection)

        dtLocal = objTicketing.Validate_Online(FormateDate(CDate(dtFrom.Value).ToString("dd/MM/yyyy")), _
                                                FormateDate(CDate(dtTo.Value).ToString("dd/MM/yyyy")))


        dtOnline = objOnlineTicketing.Validate_Online(FormateDate(CDate(dtFrom.Value).ToString("dd/MM/yyyy")), _
                                                FormateDate(CDate(dtTo.Value).ToString("dd/MM/yyyy")))

        Response.Write(dtOnline.Rows)
        Response.Write(dtLocal.Rows)

    End Sub
    Public Function FormateDate(ByVal str_Date As String) As String

        Dim sdate As String() = str_Date.Split("/")
        Return sdate(2) & "-" & sdate(1) & "-" & sdate(0)


    End Function
End Class