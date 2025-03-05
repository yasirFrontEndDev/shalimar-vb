Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online

Partial Public Class importusers
    Inherits System.Web.UI.Page

    Private objOnlineImport As eImportUtil
    Dim objConnection As Object
    Dim objUsers As clsUser
    Dim objUserRights As UserRollManage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objOnlineImport = New eImportUtil()
            Dim dtOnlineRecords As DataTable
            Dim dtUserRoll As DataTable

            dtOnlineRecords = objOnlineImport.GetAllRecords("Users")
            dtUserRoll = objOnlineImport.GetAllRecords("Ticketing_Rights")


            objConnection = ConnectionManager.GetConnection()

            objUsers = New clsUser(objConnection)
            objUserRights = New UserRollManage(objConnection)



            If Not dtOnlineRecords Is Nothing Then
                If dtOnlineRecords.Rows.Count > 0 Then
                    objUsers.ImportAllUsers(dtOnlineRecords)

                End If
            End If

            objUserRights.ImportAllUserRightDetails(dtUserRoll)


        Catch ex As Exception
            Response.Redirect("ImportOnlineData.aspx?status=failed&m=5")
        End Try

        Response.Redirect("ImportOnlineData.aspx?status=success&m=5")
    End Sub

End Class