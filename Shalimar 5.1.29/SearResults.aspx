<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SearResults.aspx.vb" Inherits="FMovers.Ticketing.UI.SearResults" MasterPageFile="~/main.Master"   %>


<%@ Register assembly="Infragistics35.WebUI.UltraWebGrid.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.UltraWebGrid" tagprefix="igtbl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"  runat="server">

    <script language=javascript >

    function LoadVoucher(TSID, type) {
        if (type == 4) {
            
//            window.open('Reports/PrintVoucherReport.aspx?ShowALl=' + type + '&TSID=' + TSID + "&status=2");
//            return false;

        }
        else {
            window.open('Reports/PrintVoucherReport.aspx?ShowALl=' + type + '&TSID=' + TSID + "&status=2");
            return false;
        
        }    
            
            
        }
        
        function LoadUserClosing(TSID, type) {
            window.open('Reports/CommissionReport.aspx?BookId=' + TSID + "&UserId=" + type);        
            return false;
        }

        function LoadTSById(TSID, type) {
            window.location = 'LoadSingleVoucher.aspx?TSID=' + TSID;
            return false;
        }
        
        function LoadAdvance(TSID, type) {
            window.open('Reports/PrintVoucherReport.aspx?ShowALl=' + type + '&TSID=' + TSID + "&status=2");
            return false;
        }        
</script>

    <div class="col-lg-12">
  <div class="topRow">


                                            <table width="100%" align="center" border="0" class="TableBorder" cellspacing="0">
                                                                                                
<tr>
                                                    <td class="Generallabel" align=center  colspan="3" >
                                                        <asp:Label ID="lblMessage" runat="server" 
                                                            Font-Bold="True" Font-Italic="False" ForeColor="#CC3300"  
                                                            Font-Size="X-Large"></asp:Label>
                                                    </td>
                                                </tr>                                                                                                
                                                
<tr>
                                                    <td class="Generallabel" align=right >
                                                        &nbsp;</td>
                                                    <td align="left" height="5px" class="style1">
                                                        &nbsp;</td>
                                                    <td align="left" height="5px">
                                                        &nbsp;</td>
                                                </tr>                                                                                                
                                                
<tr>
                                                    <td class="Generallabel" align=left colspan="3" >
                                                
                                                        <asp:GridView ID="grdMain" Width="100%" runat="server" CellPadding="10" ForeColor="#333333" 
                                                            GridLines="None" BorderColor="#666666" BorderStyle="Solid" 
                                                            BorderWidth="2px" CellSpacing="5">
                                                            <RowStyle BackColor="#EFF3FB" />
                                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                            <HeaderStyle BackColor="#23558F" Font-Bold="True" ForeColor="#CCCCCC" 
                                                                Height="35px" />
                                                            <EditRowStyle BackColor="#2461BF" />
                                                            <AlternatingRowStyle BackColor="White" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>                                                                                                
                                                
<tr>
                                                    <td    colspan="3" >
                                                      <iframe id="iframe1" runat="server" src="" height="800px" style="Border:0px" frameborder=0 width="100%"></iframe>
                                                        </td>
                                                </tr>                                                                                                
 </table>
                                              
                   
</div>

</div>
</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="head">

    <style type="text/css">
        .style1
        {
            width: 282px;
        }
        </style>

</asp:Content>
