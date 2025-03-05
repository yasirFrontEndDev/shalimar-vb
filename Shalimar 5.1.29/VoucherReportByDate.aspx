<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VoucherReportByDate.aspx.vb" Inherits="FMovers.Ticketing.UI.VoucherReportByDate" %>
<%@ Register tagprefix="igsch" namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics35.WebUI.WebDateChooser.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igmisc" namespace="Infragistics.WebUI.Misc" Assembly="Infragistics35.WebUI.Misc.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Voucher Report</title>
    <link href="styles/styles.css" rel="stylesheet" type="text/css" />
    <script src="script/scriptlib.js" type="text/javascript"></script>   
</head>
<body>
    <form id="form1" runat="server">
    <table width="600px" align="center" border="0" class="TableBorder" cellspacing="0">
         <tr>
            <td align="center" class="HeaderStyle">Voucher Report</td>
            
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
                                                            <tr>
                                                                <td align="right" width="150" height="5px"></td>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                    </igmisc:WebAsyncRefreshPanel>
           </td>
       </tr>
       <tr>
            <td align="right" class="">
                <asp:Button ID="btnShowReport" runat="server" Text="Show Report" CssClass="ButtonStyle" Width="115px" />&nbsp; </td>
       </tr>
       <tr>
            <td align="right" height="3px">
              
              
            </td>
       </tr>
    </table>
    </form>
</body>
</html>
