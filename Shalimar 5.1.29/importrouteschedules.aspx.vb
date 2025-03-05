Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online

Partial Public Class importrouteschedules
    Inherits System.Web.UI.Page

    Private objOnlineImport As eImportUtil
    Dim objConnection As Object
    Dim objUser As clsUser
    Dim objSchedule As clsSchedule
    Dim objScheduleDetail As clsScheduleDetail

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            objOnlineImport = New eImportUtil()
            Dim dtOnlineRecords As DataTable
            Dim dtOnlineRecordDetails As DataTable

            dtOnlineRecords = objOnlineImport.GetAllRecords("Schedule")
            dtOnlineRecordDetails = objOnlineImport.GetAllRecords("Schedule_Detail")

            objConnection = ConnectionManager.GetConnection()
            objSchedule = New clsSchedule(objConnection)
            objScheduleDetail = New clsScheduleDetail(objConnection)

            If Not (dtOnlineRecords Is Nothing Or dtOnlineRecordDetails Is Nothing) Then
                If dtOnlineRecords.Rows.Count > 0 Then
                    If dtOnlineRecordDetails.Rows.Count > 0 Then
                        objScheduleDetail.DeleteAll()
                    End If
                    objSchedule.DeleteAll()
                    objSchedule.ImportAllSchedules(dtOnlineRecords)

                    If dtOnlineRecords.Rows.Count > 0 Then
                        objScheduleDetail.ImportAllSchedules(dtOnlineRecordDetails)
                    End If

                End If
            End If

        Catch ex As Exception
            Response.Redirect("ImportOnlineData.aspx?status=failed&m=3")
        End Try
        Response.Redirect("ImportOnlineData.aspx?status=success&m=3")

    End Sub

End Class