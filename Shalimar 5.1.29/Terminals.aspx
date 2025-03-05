<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Terminals.aspx.vb" Inherits="FMovers.Ticketing.UI.Terminals" %>
<%@ Register tagprefix="cc1" namespace="Infragistics.Web.UI.LayoutControls" Assembly="Infragistics35.Web.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igtbl" namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics35.WebUI.UltraWebGrid.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Terminal</title>
    <link href="styles/styles.css" rel="stylesheet" type="text/css" />
    <script language="javascript">
    
    function btnAdd_onclick() 
    {
        var Grid=igtbl_getGridById('grdTerminal')
        //alert(Grid);
        igtbl_addNew("grdTerminal",0)
        //alert();
	    var row=Grid.Rows.getRow(Grid.Rows.length-1)
	    igtbl_clearSelectionAll(Grid.Id)
	    if(row!=null)
	        row.setSelected(true)
	    Grid.beginEditTemplate()
    }
    
    function btnDelete_onclick() 
    {
        var grid=igtbl_getGridById("grdTerminal")
        if(grid!=null)
            grid.deleteSelectedRows()
    }
    
    </script>
    </head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
                                            <table width="90%" align="center" border="0" class="TableBorder" cellspacing="0">
                                                <tr class="HeaderStyle">
                                                    <td align="left" valign="middle">City Terminals</td>
                                                </tr>                                                
                                                <tr>
                                                    <td align="right" valign="middle">
    <igtbl:UltraWebGrid ID="grdTerminal" runat="server" Height="300px" Width="100%">
        <bands>
            <igtbl:UltraGridBand>
                <RowEditTemplate>
                    <P align="center">
														<TABLE  class="TableBorder" id="Table2" style="WIDTH: 539px;" cellSpacing="1" cellPadding="1" border="0">
															<TR onmouseup="dragStart=false;" onmousedown="dragStart=true" class="HeaderStyle">
																<TD vAlign="middle">
																	<asp:Label id="lblTer" runat="server" Width="251px" CssClass="quicklinksmain">City 
                                                                    Terminal</asp:Label></TD>
															</TR>
															<TR>
																<TD style="vAlign="middle" align="center">
																	<TABLE id="Table3" style="WIDTH: 534px;" cellSpacing="1"
																		cellPadding="1" border="0">
																		<TR>
																			<TD class="normalcaptions" style="WIDTH: 133px; HEIGHT: 24px" vAlign="bottom" align="right">
																				<asp:Label id="lblName" runat="server" Width="112px" Height="19px" 
                                                                                    CssClass="Generallabel">Terminal Name: </asp:Label>
																				</TD>
																			<TD style="HEIGHT: 24px" vAlign="bottom" align="left"><INPUT id="Terminal_Name" 
                                                                                    style="HEIGHT: 22px; width: 200px;" type="text" maxLength="50" onchange=""
																					size="50" columnkey="Terminal_Name"></TD>
																		</TR>
																		<tr>
                                                                            <td align="right" class="normalcaptions" style="WIDTH: 133px; HEIGHT: 24px" 
                                                                                valign="bottom">
                                                                                <asp:Label ID="lblName4" runat="server" CssClass="Generallabel" Height="19px" 
                                                                                    Width="112px">Abbreviation: </asp:Label>
                                                                            </td>
                                                                            <td align="left" style="HEIGHT: 24px" valign="bottom">
                                                                                <input id="Terminal_Abbr" columnkey="Terminal_Abbr" maxlength="50" name="Text15" 
                                                                                    onchange="" size="21" 
                                                                                    style="WIDTH: 200px; HEIGHT: 22px" type="text">
                                                                                </input>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right" class="normalcaptions" style="WIDTH: 133px; HEIGHT: 24px" 
                                                                                valign="bottom">
                                                                                <asp:Label ID="lblName5" runat="server" CssClass="Generallabel" Height="19px" 
                                                                                    Width="112px">Terminal Type: </asp:Label>
                                                                            </td>
                                                                            <td align="left" style="HEIGHT: 24px" valign="bottom">
                                                                                <select id="Terminal_Type" runat="server" columnkey="Terminal_Type" name="D1" 
                                                                                    style="WIDTH: 200px; HEIGHT: 22px">
                                                                                    <option selected value="Main">Main</option>
                                                                                    <option value="Sub">Sub</option>
                                                                                </select>
                                                                            </td>
                                                                        </tr>
																		<TR>
																			<TD class="normalcaptions" style="WIDTH: 133px; HEIGHT: 24px" vAlign="bottom" align="right">
																				<asp:Label ID="lblName0" runat="server" CssClass="Generallabel" Height="19px" 
                                                                                    Width="112px">City: </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 24px" vAlign="bottom" align="left">
                                                                                <select id="City_ID" runat="server" columnkey="City_ID" name="City_ID" 
                                                                                    style="WIDTH: 200px; HEIGHT: 22px">
                                                                                    <option selected="" ></option>
                                                                                </select>
                                                                            </TD>
																		</TR>
																		<TR>
																			<TD class="normalcaptions" style="WIDTH: 133px; HEIGHT: 19px" vAlign="bottom" align="right">
																				<asp:Label ID="lblName1" runat="server" CssClass="Generallabel" Height="19px" 
                                                                                    Width="112px">Address: </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 19px" vAlign="bottom" align="left"><INPUT id="Terminal_Address" 
                                                                                    style="HEIGHT: 22px; width: 350;" type="text" maxLength="50" onchange=""
																					columnkey="Terminal_Address"></TD>
																		</TR>
																		<TR>
																			<TD class="normalcaptions" style="WIDTH: 133px; HEIGHT: 25px" vAlign="bottom" align="right">
																				<asp:Label ID="lblName2" runat="server" CssClass="Generallabel" Height="19px" 
                                                                                    Width="112px">Phone: </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 25px" vAlign="bottom" align="left"><INPUT id="Terminal_Phone" style="WIDTH: 162px; HEIGHT: 22px" type="text" maxLength="50" onchange=""
																					size="21" name="Text14" columnkey="Terminal_Phone"></TD>
																		</TR>
																		<TR>
																			<TD class="normalcaptions" style="WIDTH: 133px; HEIGHT: 8px" vAlign="bottom" align="right">
																				<asp:Label ID="lblName3" runat="server" CssClass="Generallabel" Height="19px" 
                                                                                    Width="112px">Fax: </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 8px" vAlign="bottom" align="left"><INPUT id="Terminal_Fax" style="WIDTH: 162px; HEIGHT: 22px" type="text" maxLength="150"
																					onchange="" size="50" columnkey="Terminal_Fax"></TD>
																		</TR>
																		<TR>
																			<TD class="normalcaptions" style="WIDTH: 133px; HEIGHT: 8px" vAlign="bottom" align="right">
                                                                                <asp:Label ID="lblName6" runat="server" CssClass="Generallabel" Height="19px" 
                                                                                    Width="112px">Active: </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 8px" vAlign="bottom" align="left"><INPUT id="Text1" type="checkbox" checked value="Active" 
																					onchange="" columnkey="Active"></TD>
																		</TR>
																	</TABLE>
																</TD>
															</TR>
															<TR>
																<TD class="normaltablesub" vAlign="bottom" align="right"><INPUT class="ButtonStyle" 
                                                                        id="igtbl_reOkBtn" style="WIDTH: 68px; HEIGHT: 24px" onclick="bClose=true;igtbl_gRowEditButtonClick(event);"
																		type="button" value="OK" runat="server">&nbsp;<INPUT class="ButtonStyle" 
                                                                        id="igtbl_reCancelBtn" style="WIDTH: 68px; HEIGHT: 24px" onclick="bClose=true;cancel=true;igtbl_gRowEditButtonClick(event);"
																		type="button" value="Cancel" runat="server"></TD>
															</TR>
														</TABLE>
													</P>
                </RowEditTemplate>
                <RowTemplateStyle BackColor="White" BorderColor="White" BorderStyle="Ridge">
                    <BorderDetails WidthBottom="3px" WidthLeft="3px" WidthRight="3px" 
                        WidthTop="3px" />
                </RowTemplateStyle>
               
<AddNewRow Visible="NotSet" View="NotSet"></AddNewRow>
               
            </igtbl:UltraGridBand>
        </bands>
        <displaylayout allowcolsizingdefault="Free" allowcolumnmovingdefault="OnServer" 
            allowdeletedefault="Yes" allowsortingdefault="OnClient" 
            allowupdatedefault="RowTemplateOnly" bordercollapsedefault="Separate" 
            headerclickactiondefault="SortMulti" name="UltraWebGrid1" 
            rowheightdefault="20px" 
            selecttyperowdefault="Extended" stationarymargins="Header" 
            stationarymarginsoutlookgroupby="True" tablelayout="Fixed" version="4.00" cellclickactiondefault="RowSelect" 
            AllowAddNewDefault="Yes">
            <framestyle 
                borderstyle="Solid" borderwidth="1px" font-names="Microsoft Sans Serif" 
                font-size="8.25pt" height="300px" width="100%" cssclass="GridFrame">
                <BorderDetails ColorBottom="172, 199, 246" ColorRight="172, 199, 246" />
            </framestyle>
            <RowAlternateStyleDefault BackColor="#F9FAFF" CssClass="GridItem">
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
                horizontalalign="Left" backgroundimage="./images/PageBG.jpg" 
                cssclass="GridHeader" font-bold="True" font-names="Verdana" font-size="8pt">
                <borderdetails colorleft="172, 199, 246" colortop="172, 199, 246" widthleft="1px" 
                    widthtop="1px" colorbottom="172, 199, 246" colorright="172, 199, 246" />
            </headerstyledefault>
            <rowstyledefault backcolor="#F9FAFF" borderstyle="Solid" 
                borderwidth="1px" font-names="Microsoft Sans Serif" font-size="8.25pt" 
                cssclass="GridItem">
                <padding left="3px" />
                <borderdetails colorleft="172, 199, 246" colortop="172, 199, 246" />
            </rowstyledefault>
            <groupbyrowstyledefault backcolor="Control" bordercolor="Window">
            </groupbyrowstyledefault>
            <groupbybox>
                <boxstyle backcolor="ActiveBorder" bordercolor="Window">
                </boxstyle>
            </groupbybox>
            <addnewbox View="Compact">
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
                                                    <td align="right" valign="middle" height="5px">
                                                        <input id="btnDelete" runat="server" value="Delete" class="ButtonStyle" type="button" onclick="btnDelete_onclick()"
                                                            Width="81px" />&nbsp;<input id="btnAdd" runat="server" value="New" class="ButtonStyle" type="button" onclick="btnAdd_onclick()"
                                                            Width="81px"/>&nbsp;<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ButtonStyle" 
                                                            Width="81px" />
                                                    </td>
                                                </tr>  
                                              </table>
                   
    </form>
</body>
</html>
