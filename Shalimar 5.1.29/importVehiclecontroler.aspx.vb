Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online

Partial Public Class importVehiclecontroler
    Inherits System.Web.UI.Page

    Private objOnlineImport As eImportUtil
    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objCity As clsVehicleControler

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objConnection = ConnectionManager.GetConnection()



            objOnlineImport = New eImportUtil()
            Dim dtOnlineRecords As DataTable

            dtOnlineRecords = objOnlineImport.GetAllRecords("Vehicle_Controler")

            objCity = New clsVehicleControler(objConnection)
            If Not objCity Is Nothing Then
                objCity.ImportAllCities(dtOnlineRecords)
            Else
                Response.Redirect("ImportOnlineData.aspx?status=failed&m=1")
            End If

        Catch ex As Exception
            Response.Redirect("ImportOnlineData.aspx?status=failed&m=1")
        End Try

        Response.Redirect("ImportOnlineData.aspx?status=success&m=1")
    End Sub

End Class