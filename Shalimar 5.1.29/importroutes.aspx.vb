Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online

Partial Public Class importroutes
    Inherits System.Web.UI.Page

    Private objOnlineImport As eImportUtil
    Dim objConnection As Object
    Dim objCity As clsCity
    Dim objRoute As clsRoute
    Dim objRouteDetail As clsRouteDetail

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objOnlineImport = New eImportUtil()
            Dim dtOnlineRecords As DataTable
            Dim dtOnlineRecordsDetails As DataTable

            dtOnlineRecords = objOnlineImport.GetAllRecords("Route")
            dtOnlineRecordsDetails = objOnlineImport.GetAllRecords("Route_Detail")

            objConnection = ConnectionManager.GetConnection()
            objRoute = New clsRoute(objConnection)
            objRouteDetail = New clsRouteDetail(objConnection)

            If Not (dtOnlineRecords Is Nothing Or dtOnlineRecordsDetails Is Nothing) Then
                If dtOnlineRecords.Rows.Count > 0 Then
                    If dtOnlineRecordsDetails.Rows.Count > 0 Then
                        objRouteDetail.DeleteAll()
                    End If
                    objRoute.DeleteAll()
                    objRoute.ImportAllRoutes(dtOnlineRecords)
                    If dtOnlineRecordsDetails.Rows.Count > 0 Then
                        objRouteDetail.ImportAllRoutes(dtOnlineRecordsDetails)
                    End If

                End If
            End If

        Catch ex As Exception
            Response.Redirect("ImportOnlineData.aspx?status=failed&m=2")
        End Try

        Response.Redirect("ImportOnlineData.aspx?status=success&m=2")
    End Sub

End Class