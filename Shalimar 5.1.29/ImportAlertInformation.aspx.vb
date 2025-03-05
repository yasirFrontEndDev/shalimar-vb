Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online

Public Class ImportAlertInformation
    Inherits System.Web.UI.Page
    Private objOnlineImport As eImportUtil
    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objBookkarualert As clcAlertInformaion
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objConnection = ConnectionManager.GetConnection()



            objOnlineImport = New eImportUtil()
            Dim dtOnlineRecords As DataTable

            dtOnlineRecords = objOnlineImport.GetAllRecords("Bookkaru_MessageAlert")

            objBookkarualert = New clcAlertInformaion(objConnection)
            If Not objBookkarualert Is Nothing Then
                objBookkarualert.ImportAllBookkaruMessages(dtOnlineRecords)
            Else
                Response.Redirect("ImportOnlineData.aspx?status=failed&m=1")
            End If

        Catch ex As Exception
            Response.Redirect("ImportOnlineData.aspx?status=failed&m=1")
        End Try
        Response.Redirect("ImportOnlineData.aspx?status=success&m=1")
    End Sub

End Class