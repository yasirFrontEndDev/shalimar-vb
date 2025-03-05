Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online

Partial Public Class importfare
    Inherits System.Web.UI.Page

    Private objOnlineImport As eImportUtil
    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objFare As clsFare
    Dim objServiceType As clsServiceType
    Dim objServiceTypeExtraFare As clsServiceTypeExtraFare

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objOnlineImport = New eImportUtil()
            Dim dtOnlineRecords As DataTable
            Dim dtOnlineRecordsServiceType As DataTable
            Dim dtServiceTypeExtraFare As DataTable

            dtOnlineRecords = objOnlineImport.GetAllRecords("Fare_Details")
            dtOnlineRecordsServiceType = objOnlineImport.GetAllRecords("ServiceType")
            dtServiceTypeExtraFare = objOnlineImport.GetAllRecords("ServiceTypeExtraFare")

            objConnection = ConnectionManager.GetConnection()


            objFare = New clsFare(objConnection)
            objServiceType = New clsServiceType(objConnection)
            objServiceTypeExtraFare = New clsServiceTypeExtraFare(objConnection)


            objFare.ImportAllFareDetails(dtOnlineRecords)

            objServiceType.ImportAllCities(dtOnlineRecordsServiceType)
             

            objServiceTypeExtraFare.ImportAllFareDetails(dtServiceTypeExtraFare)

        Catch ex As Exception
            Response.Redirect("ImportOnlineData.aspx?status=failed&m=7")
        End Try

        Response.Redirect("ImportOnlineData.aspx?status=success&m=7")
    End Sub

End Class