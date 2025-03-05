<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Refund.aspx.vb" Inherits="FMovers.Ticketing.UI.Refund"  MasterPageFile="~/main.Master"   %>

<%@ Register assembly="Infragistics35.WebUI.UltraWebGrid.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.UltraWebGrid" tagprefix="igtbl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"  runat="server">

    <div class="col-lg-12">
  <div class="topRow">


                                            <table width="90%" align="center" border="0" class="TableBorder" cellspacing="0">
                                                <tr class="HeaderStyle">
                                                    <td align="left" valign="middle" colspan=3 >Refund Ticket</td>
                                                </tr>                                                
                                                
<tr>
                                                    <td class="Generallabel"  colspan="3" >
                                                        <asp:Label ID="lblMessage" runat="server" 
                                                            Font-Bold="True" Font-Italic="False" ForeColor="Green"  Font-Size="Small"></asp:Label>
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
                                                        <table  style="width:550px" >
                                                            <tr>
                                                                <td>
                                                                    Ticket Number</td>
                                                                <td>
                                                        <asp:TextBox ID="txtTicketNumber" runat="server" Height="62px" 
                                                            TextMode="MultiLine"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                        <asp:Button ID="btnValidate" runat="server" Text="Validate" CssClass="ButtonStyle" 
                                                            Width="97px" />
                                                                </td>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>                                                                                                
                                                
<tr>
                                                    <td class="style2">
                                                        &nbsp;</td>
                                                    <td align="left" valign="middle" height="5px" class="style1">
                                                        &nbsp;</td>
                                                    <td align="left" valign="middle" height="5px">
                                                        &nbsp;</td>
                                                </tr>                                                                                                
                                                
<tr>
                                                    <td class="style2" colspan="3">
                                                    
                                                    <igtbl:UltraWebGrid ID="grdRoutes"  runat="server" Height="200px" 
    Width="100%">
                                                                <bands>
                                                                    <igtbl:UltraGridBand  >
                                                                        <addnewrow view="NotSet" visible="NotSet">
                                                                        </addnewrow>
                                                                    </igtbl:UltraGridBand>
                                                                </bands>
                                                                <displaylayout allowcolsizingdefault="Free" allowcolumnmovingdefault="OnServer" 
            allowdeletedefault="Yes" allowsortingdefault="OnClient" 
            allowupdatedefault="Yes" bordercollapsedefault="Separate" 
            headerclickactiondefault="SortMulti" name="UltraWebGrid1" 
            rowheightdefault="20px" rowselectorsdefault="No" 
            selecttyperowdefault="Extended" stationarymargins="Header" 
            stationarymarginsoutlookgroupby="True" tablelayout="Fixed" version="4.00" 
            viewtype="OutlookGroupBy" cellclickactiondefault="Edit" 
            AllowAddNewDefault="Yes">
                                                                    <framestyle 
                borderstyle="Solid" borderwidth="1px" font-names="Microsoft Sans Serif" 
                font-size="8.25pt" height="200px" width="100%" cssclass="GridFrame">
                                                                        <BorderDetails ColorBottom="#F8F8F8" ColorRight="#F8F8F8" />
                                                                    </framestyle>
                                                                    <RowAlternateStyleDefault BackColor="#FFEFA3" CssClass="GridItem">
                                                                    </RowAlternateStyleDefault>
                                                                    <pager minimumpagesfordisplay="2">
                                                                        <PagerStyle BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                                                                        <borderdetails colorleft="White" colortop="White" widthleft="1px" 
                    widthtop="1px" />
                                                                        </PagerStyle>
                                                                    </pager>
                                                                    <editcellstyledefault borderstyle="None" borderwidth="0px">
                                                                    </editcellstyledefault>
                                                                    <footerstyledefault backcolor="LightGray" borderstyle="Solid" borderwidth="1px">
                                                                        <borderdetails colorleft="White" colortop="White" widthleft="1px" 
                    widthtop="1px" />
                                                                    </footerstyledefault>
                                                                    <headerstyledefault borderstyle="Solid" 
                horizontalalign="Left" backcolor="#EEEEEE" 
                cssclass="GridHeader" font-bold="True" font-names="Verdana" 
                font-size="12pt">
                                                                        <borderdetails colorleft="#666666" colortop="#666666" widthleft="1px" 
                    widthtop="1px" colorbottom="666666" colorright="#666666" />
                                                                    </headerstyledefault>
                                                                    <rowstyledefault backcolor="#FFFFFF" borderstyle="Solid" 
                borderwidth="1px" font-names="Microsoft Sans Serif" font-size="8.25pt" 
                cssclass="GridItem">
                                                                        <padding left="3px" />
                                                                        <borderdetails colorleft="#F8F8F8" colortop="#F8F8F8" />
                                                                    </rowstyledefault>
                                                                    <groupbyrowstyledefault backcolor="Control" bordercolor="Window">
                                                                    </groupbyrowstyledefault>
                                                                    <SelectedRowStyleDefault BackColor="#99CCFF">
                                                                    </SelectedRowStyleDefault>
                                                                    <groupbybox Hidden="True">
                                                                        <boxstyle backcolor="ActiveBorder" bordercolor="Window">
                                                                        </boxstyle>
                                                                    </groupbybox>
                                                                    <addnewbox>
                                                                        <boxstyle backcolor="Window" bordercolor="InactiveCaption" borderstyle="Solid" 
                    borderwidth="1px">
                                                                            <borderdetails colorleft="White" colortop="White" widthleft="1px" 
                        widthtop="1px" />
                                                                        </boxstyle>
                                                                    </addnewbox>
                                                                    <activationobject bordercolor="" borderwidth="">
                                                                    </activationobject>
                                                                    <filteroptionsdefault>
                                                                        <filterdropdownstyle backcolor="White" bordercolor="Silver" borderstyle="Solid" 
                    borderwidth="1px" customrules="overflow:auto;" 
                    font-names="Verdana,Arial,Helvetica,sans-serif" font-size="11px" height="300px" 
                    width="200px">
                                                                            <padding left="2px" />
                                                                        </filterdropdownstyle>
                                                                        <filterhighlightrowstyle backcolor="#151C55" forecolor="White">
                                                                        </filterhighlightrowstyle>
                                                                        <filteroperanddropdownstyle backcolor="White" bordercolor="Silver" 
                    borderstyle="Solid" borderwidth="1px" customrules="overflow:auto;" 
                    font-names="Verdana,Arial,Helvetica,sans-serif" font-size="11px">
                                                                            <padding left="2px" />
                                                                        </filteroperanddropdownstyle>
                                                                    </filteroptionsdefault>
                                                                </displaylayout>
                                                            </igtbl:UltraWebGrid>
    
                                                    </td>
                                                </tr>                                                                                                
                                                
<tr>
                                                    <td class="style2">
                                                        &nbsp;</td>
                                                    <td align="left" valign="middle" height="5px" class="style1">
                                                        &nbsp;</td>
                                                    <td align="left" valign="middle" height="5px">
                                                        <asp:Button ID="btnSave" runat="server" Text="Refund" CssClass="ButtonStyle" 
                                                            Width="81px" OnClientClick="javascript:return Validatepass();" 
                                                            Visible="False" />
                                                    </td>
                                                </tr>                                                                                                
                                                
<tr>
                                                    <td class="style2">
                                                        &nbsp;</td>
                                                    <td align="left" valign="middle" height="5px" class="style1">
                                                        &nbsp;</td>
                                                    <td align="left" valign="middle" height="5px">
                                                        &nbsp;</td>
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
<asp:Content ID="Content2" runat="server" contentplaceholderid="head">

    <style type="text/css">
        .style1
        {
            width: 282px;
        }
        .style2
        {
            width: 100%;
        }
    </style>

</asp:Content>
