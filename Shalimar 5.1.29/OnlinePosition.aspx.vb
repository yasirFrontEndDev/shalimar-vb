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

Imports System.Management

Partial Public Class OnlinePosition
    Inherits System.Web.UI.Page

   
    Dim objBusCharges As clsBusCharges
    Dim objConnection As Object
    Dim objUser As clsUser
    Public UserName As String
    Public DefaultCityId As Integer
    Public CurrentUserID As Integer
    Dim objOnlineTicketing As eTicketing
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        Dim dt As DataTable
        Dim dt_arr As DataTable

        If Session("CurrentUser") Is Nothing Then
            Response.Redirect("UserLogin.aspx")
        Else
            objUser = CType(Session("CurrentUser"), clsUser)
            DefaultCityId = objUser.CityId
        End If

        objOnlineTicketing = New eTicketing

        dt = objOnlineTicketing.getOnlineTickeingStatus(DefaultCityId, objUser.Vendor_Id)
        dt_arr = objOnlineTicketing.getOnlineTickeingStatus_arr(DefaultCityId, objUser.Vendor_Id)

        dgArival.DataSource = dt_arr
        dgArival.DataBind()
        gdDeparture.DataSource = dt
        gdDeparture.DataBind()


        'For Each dr As DataRow In dt.Rows
        '    CurrentPosition.InnerHtml = CurrentPosition.InnerHtml & "<strong>" & dr("Schedule") & " " & dr("Time") & "</strong>" & "<br />" & " V : " & dr("Registration_No") & " S : " & dr("Sold") & " B : " & dr("Booked")
        '    CurrentPosition.InnerHtml = CurrentPosition.InnerHtml & " <hr >"
        '    CurrentPosition.InnerHtml = CurrentPosition.InnerHtml & vbCrLf
        'Next


        'For Each dr_arr As DataRow In dt_arr.Rows
        '    CurrentPosition_arr.InnerHtml = CurrentPosition_arr.InnerHtml & "<strong>" & dr_arr("Schedule") & " " & dr_arr("Time") & "</strong>" & "<br />" & " V : " & dr_arr("Registration_No") & "<br />" & " City : " & dr_arr("City")
        '    CurrentPosition_arr.InnerHtml = CurrentPosition_arr.InnerHtml & " <hr >"
        '    CurrentPosition_arr.InnerHtml = CurrentPosition_arr.InnerHtml & vbCrLf
        'Next

    End Sub

End Class