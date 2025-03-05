Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online


Partial Public Class importcomission
    Inherits System.Web.UI.Page

    Private objOnlineImport As eImportUtil
    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objVehicleVendors As clsVehicleVendors
    Dim objVendorComission As clsVendorComission
    Dim objServiceTypeExtraFare As clsServiceTypeExtraFare

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objOnlineImport = New eImportUtil()
            Dim dtOnlineRecords As DataTable
            Dim dtOnlineRecordsVendorComission As DataTable
            Dim dtOnlineRecordsVendorComissionDetail As DataTable
            Dim dtOnlineRecordsRoute As DataTable
            Dim dtOnlineRecordsMerge As DataTable
            Dim dtOnlineRecordsMergeDetail As DataTable

            dtOnlineRecordsMergeDetail = objOnlineImport.GetAllRecords("MergeTimeDetail")
            dtOnlineRecordsMerge = objOnlineImport.GetAllRecords("MergeTime")
            dtOnlineRecords = objOnlineImport.GetAllRecords("VehicleVenders")
            dtOnlineRecordsVendorComission = objOnlineImport.GetAllRecords("VendorComission")
            dtOnlineRecordsVendorComissionDetail = objOnlineImport.GetAllRecords("VendorComission_Detail")
            dtOnlineRecordsRoute = objOnlineImport.GetAllRecords("ComissionRoute")

            objConnection = ConnectionManager.GetConnection()


            objVehicleVendors = New clsVehicleVendors(objConnection)
            objVendorComission = New clsVendorComission(objConnection)
 

            objVehicleVendors.ImportAllVehicleVendors(dtOnlineRecords)
            objVendorComission.ImportAllComission(dtOnlineRecordsVendorComission)
            objVendorComission.ImportAllComissionDetail(dtOnlineRecordsVendorComissionDetail)
            objVendorComission.ImportAllRoute(dtOnlineRecordsRoute)
            objVendorComission.ImportAllMerge(dtOnlineRecordsMerge)
            objVendorComission.ImportAllMergeDetail(dtOnlineRecordsMergeDetail)

            'objServiceType.ImportAllCities(dtOnlineRecordsServiceType)


            'objServiceTypeExtraFare.ImportAllFareDetails(dtServiceTypeExtraFare)

        Catch ex As Exception
            Response.Redirect("ImportOnlineData.aspx?status=failed&m=8")
        End Try

        Response.Redirect("ImportOnlineData.aspx?status=success&m=8")
    End Sub



End Class