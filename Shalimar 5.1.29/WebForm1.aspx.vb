Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online
Imports Infragistics.WebUI.UltraWebGrid
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Web
Imports System.Text
Imports System.Data


Partial Public Class WebForm1
    Inherits System.Web.UI.Page
    Dim objOnlineTicketing As eTicketing


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Page.IsPostBack = False Then

            objOnlineTicketing = New eTicketing

            Dim dt As DataTable = objOnlineTicketing.GetOnlineSchedules()

 

        End If

    End Sub


End Class