Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity
Imports FMovers.Ticketing.Online


Partial Public Class getCustomerInfo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Dim objConnection As Object
        Dim objTicketingSeatList As clsSeatTicketingList
        Dim str_Return As String = ""
        objConnection = ConnectionManager.GetConnection()
        If Request.QueryString("type") = "con" Then
            If Not Request.QueryString("ContactNo") Is Nothing Then
                Dim dtTable As New DataTable
                Dim dtTable2 As New DataTable
                objTicketingSeatList = New clsSeatTicketingList(objConnection)
                dtTable = objTicketingSeatList.GetCustomerInfo(Request.QueryString("type"), Request.QueryString("ContactNo").Trim()).Tables(0)
                dtTable2 = objTicketingSeatList.GetCustomerInfo(Request.QueryString("type"), Request.QueryString("ContactNo").Trim()).Tables(1)
                If Not dtTable Is Nothing Then

                    If dtTable.Rows.Count > 0 Then
                        str_Return = str_Return & dtTable.Rows(0)("Passenger_Name").ToString() & "|" & dtTable.Rows(0)("CNIC").ToString()
                    End If
                    str_Return = str_Return & "|"
                    If dtTable2.Rows.Count > 0 Then
                        str_Return = str_Return & " Passenger already have booked seat at"
                        For Each dr As DataRow In dtTable2.Rows
                            str_Return = str_Return & " Date : " & dr("ddate") & " Route : " & dr("Schedule_Title") & " no. of seats : " & dr("number")
                        Next
                    End If
                    Response.Write(str_Return)
                End If
            End If
        End If
        If Request.QueryString("type") = "cnic" Then
            If Not Request.QueryString("ContactNo") Is Nothing Then
                Dim dtTable As New DataTable
                objTicketingSeatList = New clsSeatTicketingList(objConnection)
                dtTable = objTicketingSeatList.GetCustomerInfo(Request.QueryString("type"), Request.QueryString("ContactNo").Trim()).Tables(0)
                If Not dtTable Is Nothing Then

                    If dtTable.Rows.Count > 0 Then
                        Response.Write(dtTable.Rows(0)("Passenger_Name").ToString() & "|" & dtTable.Rows(0)("Contact_No").ToString())
                    End If
                End If
            End If

        End If


    End Sub

End Class