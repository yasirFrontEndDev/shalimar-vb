<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TicketListTest.aspx.vb" Inherits="FMovers.Ticketing.UI.TicketListTest" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Ticket Test</title>
    <link href="styles/styles.css" rel="stylesheet" type="text/css" />
    <script language="javascript">
    
    function OperateOnSeat(cell, SeatNo, OperationType){
        if (OperationType == 2){        
            alert(cell);
            alert(SeatNo);
            alert(OperationType);
            alert(cell.SeatStatus);
        }
        else{
        cell.innerText = "test"
        }
    }
    
    
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
     <table width="280px" align="center" border="0" class="TableBorder" cellspacing="0">
                                                <tr class="HeaderStyle">
                                                    <td align="center" valign="middle">Select Seat</td>
                                                </tr>                                                
                                                <tr>
                                                    <td align="right" valign="middle">
                                                        <table id="tblTickets" runat="server" align="center" cellpadding="0" cellspacing="0" style="width:280px">
                                                            <tr>
                                                                <td title="Available" SeatStatus="Available" class="TicketAvailable" id="cell_0_0_1"  ondblclick="OperateOnSeat(this, 1, 2);">&nbsp;</td>
                                                                <td class="TicketConfirmed" id="cell_0_0_2"  ondblclick="OperateOnSeat(this, 2, 2);">&nbsp;</td>
                                                                <td class="TicketAvailable" id="cell_0_0_3"  ondblclick="OperateOnSeat(this, 3, 2);">&nbsp;</td>
                                                                <td class="TicketAvailable" id="cell_0_0_4"  ondblclick="OperateOnSeat(this, 4, 2);">&nbsp;</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>                                                
                                               <tr>
                                                    <td align="right" valign="middle" height="5px">
                                                        &nbsp;&nbsp;</td>
                                                </tr>  
                                              </table>
                   
    </form>
</body>
</html>
