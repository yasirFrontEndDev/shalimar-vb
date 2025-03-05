<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ImportOnlineData.aspx.vb" Inherits="FMovers.Ticketing.UI.ImportOnlineData" MasterPageFile="~/main.Master"   %>

<%@ Register tagprefix="cc1" namespace="Infragistics.Web.UI.LayoutControls" Assembly="Infragistics35.Web.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igtbl" namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics35.WebUI.UltraWebGrid.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"  runat="server">

    <div class="col-lg-12">
  <div class="topRow">

    <table width="90%" align="center" border="0" class="TableBorder" cellspacing="0">
                                                <tr class="HeaderStyle">
                                                    <td align="left" valign="middle">Online Import</td>
                                                </tr>                                                
                                                <tr>
                                                    <td align="center" valign="middle" height="80">
                                                                    <asp:Label ID="Label11" runat="server" CssClass="Generallabel" 
                                                                        Text="Select Type :" Font-Bold="True"></asp:Label>&nbsp;<asp:DropDownList 
                                                            ID="cmbType" runat="server" Width="255px">
                                                                        <asp:ListItem Selected Value="0">   -- Select Type --   </asp:ListItem>
                                                                        <asp:ListItem Value="1">Cities</asp:ListItem>                                                                      
                                                                        <asp:ListItem Value="7">Fare</asp:ListItem>
                                                                        <asp:ListItem Value="2">Routes</asp:ListItem>
                                                                        <asp:ListItem Value="3">Route Schedules</asp:ListItem>
                                                                        <asp:ListItem Value="4">Terminals</asp:ListItem>
                                                                        <asp:ListItem Value="5">Users</asp:ListItem>
                                                                        <asp:ListItem Value="6">Vehicles</asp:ListItem>
                                                                        <asp:ListItem Value="8">Comission</asp:ListItem>
                                                                        <asp:ListItem Value="9">Vehicle Controler</asp:ListItem>
                                                                        <asp:ListItem Value="10">Value Added Services</asp:ListItem>
                                                                        <asp:ListItem Value="11">Alert Informations</asp:ListItem>
                                                                        <asp:ListItem Value="12">Operater Companies</asp:ListItem>
                                                                    </asp:DropDownList>
                                                    </td>
                                                </tr>                                                
                                               <tr>
                                                    <td align="center" valign="middle" height="5px">
                                                        <asp:Label ID="lblErr" runat="server" CssClass="Errorlabel"></asp:Label>
                                                    </td>
                                                </tr>  
                                               <tr>
                                                    <td align="right" valign="middle" height="5px">
                                                        &nbsp;&nbsp;<asp:Button ID="btnImport" runat="server" Text="Import" CssClass="ButtonStyle" 
                                                            Width="81px" />
                                                    </td>
                                                </tr>  
                                              </table>
                   
</div>

</div>
</asp:Content>