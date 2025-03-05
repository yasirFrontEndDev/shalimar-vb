<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PrintTicket.aspx.vb" Inherits="FMovers.Ticketing.UI.PrintTicket" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script type="text/javascript">
        
        function Print() {                               
            window.print();
        }
        
    </script>
    
    <style type="text/css">
        .style1
        {
            width: 100%;
        }

        .Generallabel
        {
	         font-family:Verdana;
	         font-size:12px;	 
	         color:Black;
        }

    </style>
</head>
<body onload = "">
    <form id="form1" runat="server">
    <div>
    <script type="text/vbscript" language="vbscript"> 
    Sub Print() 
       OLECMDID_PRINT = 6 
       OLECMDEXECOPT_DONTPROMPTUSER = 2 
       OLECMDEXECOPT_PROMPTUSER = 1 
       call WB.ExecWB(OLECMDID_PRINT, OLECMDEXECOPT_DONTPROMPTUSER,1) 
    End Sub 
    
</script> 

<script language="javascript" type="text/javascript">

window.print(); 
window.close();
</script>
<object id='WB' width="0" height="0" classid='CLSID:8856F961-340A-11D0-A96B-00C04FD705A2'></object>
        <table cellpadding="0" cellspacing="0" class="style1">
            <tr>
                <td align="right" style="width: 200px">
                    <asp:Label ID="Label2" runat="server" CssClass="Generallabel" Text="Ticket Sr. No. :"></asp:Label>&nbsp;
                </td>
                <td>
                    <asp:Label ID="lblTicketNo" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label11" runat="server" CssClass="Generallabel" Text="Passenger Name :"></asp:Label>&nbsp;
                </td>
                <td>
                    <asp:Label ID="lblPassengerName" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label17" runat="server" CssClass="Generallabel" Text="Contact No. :"></asp:Label>&nbsp;
                </td>
                <td>
                    <asp:Label ID="lblContactNo" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label8" runat="server" CssClass="Generallabel" Text="Seat No. :"></asp:Label>&nbsp;
                </td>
                <td>
                    <asp:Label ID="lblSeatNo" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label13" runat="server" CssClass="Generallabel" Text="Fare :"></asp:Label>&nbsp;
                </td>
                <td>
                    <asp:Label ID="lblFare" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label18" runat="server" CssClass="Generallabel" Text="Route :"></asp:Label>&nbsp;
                </td>
                <td>
                    <asp:Label ID="lblRoute" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label5" runat="server" Text="Departure Date n Time :" 
                        CssClass="Generallabel"></asp:Label>&nbsp;
                </td>
                <td>
                    <asp:Label ID="lblDepartureDateTime" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label19" runat="server" Text="Vehicle No. :" CssClass="Generallabel"></asp:Label>&nbsp;
                </td>
                <td>
                    <asp:Label ID="lblVehicleNo" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
