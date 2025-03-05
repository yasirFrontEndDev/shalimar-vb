<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BookingReport.aspx.vb" Inherits="FMovers.Ticketing.UI.BookingReport" %>

<%@ Register tagprefix="igsch" namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics35.WebUI.WebDateChooser.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igmisc" namespace="Infragistics.WebUI.Misc" Assembly="Infragistics35.WebUI.Misc.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Day Wise Passenger</title>
    <script language=javascript >

    function loadreport()
    {
      
        	var Type = document.getElementById("cboType");
        	
        	var cmbSource1 = document.getElementById("cmbSource");
         
//         alert( document.getElementById("dtTo_input").value    );
//         
          window.open('Reports/rpt_BookingReport.aspx?from=' + document.getElementById("dtFrom_input").value + "&to="+document.getElementById("dtTo_input").value+ "&Dfrom="+document.getElementById("txtTimeFrom").value+ "&Dto="+document.getElementById("txtTimeTo").value+"&Status="+Type.options[Type.selectedIndex].value+"&SID="+cmbSource1.options[cmbSource1.selectedIndex].value);

         return false;


    }
    
    </script>

    <link href="styles/styles.css" rel="stylesheet" type="text/css" />
    <script src="script/scriptlib.js" type="text/javascript"></script>   
</head>
<body>
    <form id="form1" runat="server">
    <table width="600px" align="center" border="0" class="TableBorder" cellspacing="0">
         <tr>
            <td align="center" class="HeaderStyle">Day Wise Passenger</td>
            
        </tr>
        <tr>
            <td align="center" valign="top">
              <igmisc:WebAsyncRefreshPanel ID="warpRefresh" runat="server">
                                                        <table cellpadding="2" cellspacing="1" width="90%">
                                                            <tr>
                                                                <td align="right" width="150" height="5px"></td>
                                                                <td></td>
                                                            </tr>
                                                            
<tr>
                                                                <td align="right" width="150"><asp:Label ID="Label6" runat="server" Text="Schedule :" 
                                                                        CssClass="Generallabel"></asp:Label>&nbsp;</td>
                                                                <td>
                                                                  <asp:DropDownList ID="cmbSource"  onclick ="" runat="server" Width="255px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>                                                            
                                                            <tr>
                                                                <td align="right" width="150"><asp:Label ID="Label1" runat="server" Text="From:" 
                                                                        CssClass="Generallabel"></asp:Label>&nbsp;</td>
                                                                <td>
                                                                    <igsch:WebDateChooser ID="dtFrom" runat="server" Width="235px" 
                                                                        NullDateLabel="Select Date" AllowNull="False" Editable="False" 
                                                                        MinDate="2010-03-01">
                                                                        <AutoPostBack ValueChanged="True" />
                                                                        <DropButton>
                                                                            <Style Font-Names="Verdana">
                                                                            </Style>
                                                                        </DropButton>
                                                                        <CalendarLayout>
                                                                            <CalendarStyle BackColor="#F9FAFF" Font-Names="Verdana" Font-Size="10pt">
                                                                            </CalendarStyle>
                                                                            <DayHeaderStyle BackColor="#6699FF" />
                                                                        </CalendarLayout>
                                                                    </igsch:WebDateChooser>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" width="150"><asp:Label ID="Label3" runat="server" Text="To:" 
                                                                        CssClass="Generallabel"></asp:Label></td>
                                                                <td>
                                                                    <igsch:WebDateChooser ID="dtTo" runat="server" Width="235px" 
                                                                        NullDateLabel="Select Date" AllowNull="False" Editable="False" 
                                                                        MinDate="2010-03-01">
                                                                        <AutoPostBack ValueChanged="True" />
                                                                        <DropButton>
                                                                            <Style Font-Names="Verdana">
                                                                            </Style>
                                                                        </DropButton>
                                                                        <CalendarLayout>
                                                                            <CalendarStyle BackColor="#F9FAFF" Font-Names="Verdana" Font-Size="10pt">
                                                                            </CalendarStyle>
                                                                            <DayHeaderStyle BackColor="#6699FF" />
                                                                        </CalendarLayout>
                                                                    </igsch:WebDateChooser>
                                                                </td>
                                                            </tr>
                                                            <tr style="display:none" >
                                                                <td align="right" width="150" height="5px">
                                                                    <asp:Label ID="Label4" runat="server" Text="User Name:" 
                                                                        CssClass="Generallabel"></asp:Label>&nbsp;</td>
                                                                <td>
                                                                    <asp:DropDownList ID="cboUser" Visible="false" runat="server" AutoPostBack="True" 
                                                                        Width="235px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            
                                                            <tr style="display:none" >
                                                                <td align="right" width="150" height="5px">
                                                                    <asp:Label ID="Label2" runat="server" Text="Time :" 
                                                                        CssClass="Generallabel"></asp:Label>&nbsp;</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtTimeFrom"  Width="50px" runat="server"></asp:TextBox>  <asp:TextBox ID="txtTimeTo"  Width="50px" runat="server"></asp:TextBox>
                                                                </td>
                                                            </tr>
 <tr style="display:none">
                                                                <td align="right" width="150" height="5px">
                                                                    <asp:Label ID="Label5" runat="server" Text="Report Type :" 
                                                                        CssClass="Generallabel"></asp:Label>&nbsp;</td>
                                                                <td >
                                                                     <asp:DropDownList ID="cboType"  runat="server" Width="238px" >
                    <asp:ListItem Value="4">Advance</asp:ListItem>
                    <asp:ListItem Value="3">Booking</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    
                                                                </td>
                                                            </tr>
                                                        </table>
                      </igmisc:WebAsyncRefreshPanel>
                                                        
           </td>
       </tr>
       <tr>
            <td align="right" class="">
               </td>
       </tr>
       <tr>
            <td align="right" class="">
                <asp:Button ID="btnShowReport" runat="server" Text="Show Report"  OnClientClick="return loadreport();" CssClass="ButtonStyle" Width="115px" />&nbsp; </td>
       </tr>
       <tr>
            <td align="right" height="3px"></td>
       </tr>
    </table>
    </form>
</body>
</html>


