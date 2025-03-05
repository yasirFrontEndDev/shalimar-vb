<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ChanageTicket.aspx.vb" Inherits="FMovers.Ticketing.UI.ChanageTicket"  MasterPageFile="~/main.Master" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"  runat="server">

    <div class="col-lg-12">
  <div class="topRow">
  
  
    <div>
    
    </div>
                                            <table width="90%" align="center" border="0" class="TableBorder" cellspacing="0">
                                                <tr class="HeaderStyle">
                                                    <td align="left" valign="middle" colspan=2 >Change Ticket</td>
                                                </tr>                                                
                                                
<tr>
                                                    <td class="Generallabel"  colspan="2" >
                                                        <asp:Label ID="lblMessage" runat="server" CssClass="Errorlabel" 
                                                            Font-Bold="True" Font-Italic="False" Font-Size="XX-Small"></asp:Label>
                                                    </td>
                                                </tr>                                                                                                
                                                
<tr>
                                                    <td class="Generallabel" align=right >
                                                        Ticket Number: </td>
                                                    <td align="left" height="5px">
                                                        <asp:TextBox ID="txtTicketNumber" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>                                                                                                
                                                
<tr>
                                                    <td class="style2">
                                                        &nbsp;</td>
                                                    <td align="left" valign="middle" height="5px">
                                                        &nbsp;</td>
                                                </tr>                                                                                                
                                                
<tr>
                                                    <td class="style2">
                                                        &nbsp;</td>
                                                    <td align="left" valign="middle" height="5px">
                                                        <asp:Button ID="btnSave" runat="server" Text="Update" CssClass="ButtonStyle" 
                                                            Width="81px" OnClientClick="javascript:return Validatepass();" />
                                                    </td>
                                                </tr>                                                                                                
                                                
<tr>
                                                    <td align="center" valign="middle" height="5px" class="style2">
                                                        &nbsp;</td>
                                                </tr>                                                                                                
                                               <tr>
                                                    <td align="center" valign="middle" height="5px" class="style2">
                                                        &nbsp;</td>
                                                </tr>  
                                              </table>
                                              
                   

</div>

</div>
</asp:Content>