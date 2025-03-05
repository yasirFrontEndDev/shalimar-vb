Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online

Partial Public Class importvehicles
    Inherits System.Web.UI.Page

    Private objOnlineImport As eImportUtil
    Dim objConnection As Object
    Dim objVehicle As clsVehicle

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objOnlineImport = New eImportUtil()
            Dim dtOnlineRecords As DataTable
            dtOnlineRecords = objOnlineImport.GetAllRecords("Vehicle")

            objConnection = ConnectionManager.GetConnection()
            objVehicle = New clsVehicle(objConnection)
            If Not dtOnlineRecords Is Nothing Then
                If dtOnlineRecords.Rows.Count > 0 Then
                    objVehicle.ImportAllCities(dtOnlineRecords)
                End If
            End If
        Catch ex As Exception
            Response.Redirect("ImportOnlineData.aspx?status=failed&m=6")
        End Try

        Response.Redirect("ImportOnlineData.aspx?status=success&m=6")
    End Sub

End Class