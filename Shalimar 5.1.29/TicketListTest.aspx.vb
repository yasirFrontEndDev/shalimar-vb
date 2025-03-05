Imports FMovers.Ticketing.DAL
Imports FMovers.Ticketing.Entity

Partial Public Class TicketListTest
    Inherits System.Web.UI.Page

    Dim objConnection As Object

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objConnection = ConnectionManager.GetConnection()
        PopulateTicketList(72)
    End Sub

    Public Sub PopulateTicketList(Optional ByVal SeatCount As Integer = 45)
        Dim dtseatsInfo As DataTable = Me.getSeatsInfo(7)
        Dim dvSeatsInfo As New DataView(dtseatsInfo)
        Dim RowIndex As Short = 0
        Dim tblRow As HtmlTableRow
        For i As Integer = 0 To SeatCount - 1
            If RowIndex = 0 Then
                tblRow = tblTickets.Rows(0)
                tblRow.Cells(0).InnerText = i + 1
                tblRow.Cells(1).InnerText = i + 2
                tblRow.Cells(2).InnerText = i + 3
                tblRow.Cells(3).InnerText = i + 4
            Else
                tblRow = New HtmlTableRow()
                Dim htmlCell As New HtmlTableCell()
                htmlCell.ID = "cell_" & RowIndex & "_0_" & i + 1

                If Not i + 1 > SeatCount Then
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 1)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    htmlCell.Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 1 & ", 2);")
                    htmlCell.InnerText = i + 1
                End If
                tblRow.Cells.Insert(0, htmlCell)

                htmlCell = New HtmlTableCell()
                htmlCell.ID = "cell_" & RowIndex & "_1_" & i + 2
                htmlCell.Attributes.Add("class", "TicketAvailable")
                htmlCell.Attributes.Add("title", "")
                If Not i + 2 > SeatCount Then
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 2)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    htmlCell.Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 2 & ", 2);")
                    htmlCell.InnerText = i + 2
                End If
                tblRow.Cells.Insert(1, htmlCell)

                htmlCell = New HtmlTableCell()
                htmlCell.ID = "cell_" & RowIndex & "_2_" & i + 3
                htmlCell.Attributes.Add("class", "TicketAvailable")
                htmlCell.Attributes.Add("title", "")
                If Not i + 3 > SeatCount Then
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 3)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    htmlCell.Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 3 & ", 2);")
                    htmlCell.InnerText = i + 3
                End If
                tblRow.Cells.Insert(2, htmlCell)

                htmlCell = New HtmlTableCell()
                htmlCell.ID = "cell_" & RowIndex & "_3_" & i + 4
                htmlCell.Attributes.Add("class", "TicketAvailable")
                htmlCell.Attributes.Add("title", "")
                If Not i + 4 > SeatCount Then
                    dvSeatsInfo.RowFilter = "Seat_No=" & (i + 4)
                    Me.ApplyCellSettings(htmlCell, dvSeatsInfo)
                    htmlCell.Attributes.Add("ondblclick", "OperateOnSeat(this, " & i + 4 & ", 2);")
                    htmlCell.InnerText = i + 4
                End If
                tblRow.Cells.Insert(3, htmlCell)

                tblRow.Cells.Add(New HtmlTableCell())
                tblTickets.Rows.Add(tblRow)
                'tblRow = tblTickets.Rows(0)
            End If

            RowIndex = RowIndex + 1
            i = i + 3
        Next

    End Sub

    Public Sub ApplyCellSettings(ByRef cell As HtmlTableCell, ByRef dvSeatInfo As DataView)
        If dvSeatInfo.Count > 0 Then
            If dvSeatInfo.Item(0)("Status") = "1" Then
                cell.Attributes.Add("class", "TicketAvailable")
                cell.Attributes.Add("title", "Available" & vbCrLf & "<br>test\nsahfhf\nahsfh")
            ElseIf dvSeatInfo.Item(0)("Status") = "2" Then
                cell.Attributes.Add("class", "TicketConfirmed")
                cell.Attributes.Add("title", "Reserved")
            ElseIf dvSeatInfo.Item(0)("Status") = "3" Then
                cell.Attributes.Add("class", "TicketConfirmed")
                cell.Attributes.Add("title", "Confirmed" & vbCrLf & "Passenger: " & dvSeatInfo.Item(0)("Passenger_Name") & vbCrLf & "Confirmed By: " & dvSeatInfo.Item(0)("Passenger_Name") & vbCrLf & "Issued On: " & dvSeatInfo.Item(0)("Issue_Date"))
            End If
        Else
            cell.Attributes.Add("class", "TicketAvailable")
            cell.Attributes.Add("title", "Available")
        End If
    End Sub

    Public Function getSeatsInfo(ByVal TicketingScheduleID As Integer)
        Dim objTicketingList As New clsSeatTicketingList(objConnection)
        Return objTicketingList.GetAll(TicketingScheduleID, True)
    End Function


    Private Sub TicketListTest_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        ConnectionManager.CloseConnection(objConnection)
    End Sub
End Class