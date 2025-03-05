
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Vehicles.aspx.vb" Inherits="FMovers.Ticketing.UI.Vehicles" %>
<%@ Register tagprefix="cc1" namespace="Infragistics.Web.UI.LayoutControls" Assembly="Infragistics35.Web.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igtbl" namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics35.WebUI.UltraWebGrid.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Vehicles</title>
    <link href="styles/styles.css" rel="stylesheet" type="text/css" />
    <script language="javascript">
    
    function checkComm(ctrl)
    {
        if (ctrl.checked == true)
        {
            document.getElementById("trComm1").style.display = "";
            document.getElementById("trComm2").style.display = "";
            document.getElementById("trComm3").style.display = "";
            document.getElementById("trComm4").style.display = "";
        }
        else
        {
            document.getElementById("trComm1").style.display = "none";
            document.getElementById("trComm2").style.display = "none";
            document.getElementById("trComm3").style.display = "none";
            document.getElementById("trComm4").style.display = "none";
            document.getElementById("Comm_Owner").value = "";
            document.getElementById("Comm_Contact_Person").value = "";
            document.getElementById("Comm_Contact_No").value = "";
        }
        //alert(ctrl.id);
    }
    
    function btnAdd_onclick() 
    {
        var Grid=igtbl_getGridById('grdVehicles')
        //alert(Grid);
        igtbl_addNew("grdVehicles",0)
        
	    var row=Grid.Rows.getRow(Grid.Rows.length-1)
	    alert(Grid.Id);
	    igtbl_clearSelectionAll(Grid.Id)
	    if(row!=null)
	        row.setSelected(true)
	    Grid.beginEditTemplate()
    }
    
    function btnDelete_onclick() 
    {
        var grid=igtbl_getGridById("grdVehicles")
        if(grid!=null)
            grid.deleteSelectedRows()
    }
    
    function pageInit()
    {
        
    
    }
    
    </script>
    <style type="text/css">
        .style1
        {
            height: 24px;
            width: 183px;
        }
        .style2
        {
            height: 19px;
            width: 183px;
        }
        .style3
        {
            height: 25px;
            width: 183px;
        }
        .style4
        {
            height: 8px;
            width: 183px;
        }
    </style>
    
    <script type="text/javascript" id="igClientScript">
<!--

function grdVehicles_AfterRowTemplateOpenHandler(gridName, rowId)
{
	checkComm(document.getElementById("IsCommissioned"))
}
// -->
</script>
    </head>
<body onload="pageInit();">
    <form id="form1" runat="server">
    <div>
    
    </div>
                                            <table width="90%" align="center" border="0" class="TableBorder" cellspacing="0">
                                                <tr class="HeaderStyle">
                                                    <td align="left" valign="middle">Vehicles</td>
                                                </tr>   
                                                <tr >
                                                    <td align="left" valign="middle"><asp:Label ID="lblError" runat="server" CssClass="Errorlabel"></asp:Label></td>
                                                </tr>                                               
                                                <tr>
                                                    <td align="right" valign="middle">
    <igtbl:UltraWebGrid ID="grdVehicles" runat="server" Height="300px" Width="100%">
        <bands>
            <igtbl:UltraGridBand>
                <FilterOptions AllowRowFiltering="OnClient" ApplyOnAdd="True" 
                    FilterRowView="Top" FilterUIType="FilterRow" RowFilterMode="AllRowsInBand">
                </FilterOptions>
                <RowEditTemplate>
                    <P align="center">
														<TABLE  class="TableBorder" id="Table2" style="WIDTH: 539px;" cellSpacing="1" cellPadding="1" border="0">
															<TR onmouseup="dragStart=false;" onmousedown="dragStart=true" class="HeaderStyle">
																<TD vAlign="middle">
																	<asp:Label id="lblTer" runat="server" Width="251px" CssClass="quicklinksmain">Vehicle</asp:Label></TD>
															</TR>
															<TR>
																<TD valign="middle" align="center">
																	<TABLE id="Table3" style="WIDTH: 534px;" cellSpacing="1"
																		cellPadding="1" border="0">
																		<TR style="display:none" >
																			<TD class="style1" vAlign="bottom" align="right">
																				<asp:Label id="lblName" runat="server" Width="112px" Height="19px" 
                                                                                    CssClass="Generallabel">Vehicle Code: </asp:Label>
																				</TD>
																			<TD style="HEIGHT: 24px" vAlign="bottom" align="left"><INPUT id="Veh_Code" 
                                                                                    style="HEIGHT: 22px; width: 200px;" type="text" maxLength="50" onchange=""
																					size="50" columnkey="Veh_Code"></TD>
																		</TR>
																		<tr>
                                                                            <td align="right" class="style1" 
                                                                                valign="bottom">
                                                                                <asp:Label ID="lblName4" runat="server" CssClass="Generallabel" Height="19px" 
                                                                                    Width="112px">Type: </asp:Label>
                                                                            </td>
                                                                            <td align="left" style="HEIGHT: 24px" valign="bottom">
                                                                                 <select id="Veh_Type" runat="server" columnkey="Veh_Type" name="D1" 
                                                                                    style="WIDTH: 200px; HEIGHT: 22px">
                                                                                    <option selected value="Type A">Type A</option>
                                                                                    <option value="Type B">Type B</option>
                                                                                </select>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right" class="style1" 
                                                                                valign="bottom">
                                                                                <asp:Label ID="lblName5" runat="server" CssClass="Generallabel" Height="19px" 
                                                                                    Width="112px">Vehicle Name: </asp:Label>
                                                                            </td>
                                                                            <td align="left" style="HEIGHT: 24px" valign="bottom">
                                                                                 <input id="Veh_Name" columnkey="Veh_Name" maxlength="50" name="Text15" 
                                                                                    onchange="" size="21" 
                                                                                    style="WIDTH: 200px; HEIGHT: 22px" type="text">
                                                                                </input>                                                                               
                                                                            </td>
                                                                        </tr>
																		<TR>
																			<TD class="style1" vAlign="bottom" align="right">
																				<asp:Label ID="lblName0" runat="server" CssClass="Generallabel" Height="19px" 
                                                                                    Width="112px"># of Seats: </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 24px" vAlign="bottom" align="left">
                                                                               <input id="Seats" columnkey="Seats" maxlength="50" name="Text15" 
                                                                                    onchange="" size="21" 
                                                                                    style="WIDTH: 200px; HEIGHT: 22px" type="text">
                                                                                </input>
                                                                            </TD>
																		</TR>
																		<TR>
																			<TD class="style2" vAlign="bottom" align="right">
																				<asp:Label ID="lblName1" runat="server" CssClass="Generallabel" Height="19px">Is 
                                                                                Commissioned: </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 19px" vAlign="bottom" align="left"><INPUT id="IsCommissioned" 
                                                                                    style="HEIGHT: 22px; width: 350;" type="checkbox" maxLength="50" onclick="checkComm(this);"
																					columnkey="IsCommissioned"></TD>
																		</TR>
																		<TR id="trComm1">
																			<TD class="style3" vAlign="bottom" align="right">
																				<asp:Label ID="lblName2" runat="server" CssClass="Generallabel" Height="19px">Commission %age: </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 25px" vAlign="bottom" align="left"><INPUT id="Commission_Rate" style="WIDTH: 162px; HEIGHT: 22px" type="text" maxLength="50" onchange=""
																					size="21" name="Text14" columnkey="Commission_Rate"></TD>
																		</TR>
																		<TR id="trComm2">
																			<TD class="style3" vAlign="bottom" align="right">
																				<asp:Label ID="Label7" runat="server" CssClass="Generallabel" Height="19px">Commission Owner: </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 25px" vAlign="bottom" align="left"><INPUT id="Comm_Owner" style="WIDTH: 162px; HEIGHT: 22px" type="text" maxLength="50" onchange=""
																					size="21" name="Text14" columnkey="Comm_Owner"></TD>
																		</TR>
																		<TR id="trComm3">
																			<TD class="style3" vAlign="bottom" align="right">
																				<asp:Label ID="Label8" runat="server" CssClass="Generallabel" Height="19px">Commission Contact Person: </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 25px" vAlign="bottom" align="left"><INPUT id="Comm_Contact_Person" style="WIDTH: 162px; HEIGHT: 22px" type="text" maxLength="50" onchange=""
																					size="21" name="Text14" columnkey="Comm_Contact_Person"></TD>
																		</TR>
																		<TR id="trComm4">
																			<TD class="style3" vAlign="bottom" align="right">
																				<asp:Label ID="Label9" runat="server" CssClass="Generallabel" Height="19px">Commission Contact #: </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 25px" vAlign="bottom" align="left"><INPUT id="Comm_Contact_No" style="WIDTH: 162px; HEIGHT: 22px" type="text" maxLength="50" onchange=""
																					size="21" name="Text14" columnkey="Comm_Contact_No"></TD>
																		</TR>																		
																		<TR>
																			<TD class="style4" vAlign="bottom" align="right">
																				<asp:Label ID="lblName3" runat="server" CssClass="Generallabel" Height="19px" 
                                                                                    Width="112px">Registration #: </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 8px" vAlign="bottom" align="left"><INPUT id="Registration_No" style="WIDTH: 162px; HEIGHT: 22px" type="text" maxLength="150"
																					onchange="" size="50" columnkey="Registration_No"></TD>
																		</TR>
																		<TR>
																			<TD class="style4" vAlign="bottom" align="right">
																				<asp:Label ID="Label1" runat="server" CssClass="Generallabel" Height="19px" 
                                                                                    Width="112px">Engine #: </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 8px" vAlign="bottom" align="left"><INPUT id="Engine_No" style="WIDTH: 162px; HEIGHT: 22px" type="text" maxLength="150"
																					onchange="" size="50" columnkey="Engine_No"></TD>
																		</TR>
																		<TR>
																			<TD class="style4" vAlign="bottom" align="right">
																				<asp:Label ID="Label2" runat="server" CssClass="Generallabel" Height="19px" 
                                                                                    Width="112px">Chasis #: </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 8px" vAlign="bottom" align="left"><INPUT id="Chasis_No" style="WIDTH: 162px; HEIGHT: 22px" type="text" maxLength="150"
																					onchange="" size="50" columnkey="Chasis_No"></TD>
																		</TR>
																		<TR>
																			<TD class="style4" vAlign="bottom" align="right">
																				<asp:Label ID="Label3" runat="server" CssClass="Generallabel" Height="19px" 
                                                                                    Width="112px">Model: </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 8px" vAlign="bottom" align="left"><INPUT id="Model" style="WIDTH: 162px; HEIGHT: 22px" type="text" maxLength="150"
																					onchange="" size="50" columnkey="Model"></TD>
																		</TR>
																		<TR>
																			<TD class="style4" vAlign="bottom" align="right">
																				<asp:Label ID="Label4" runat="server" CssClass="Generallabel" Height="19px" 
                                                                                    Width="112px">Maker: </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 8px" vAlign="bottom" align="left"><INPUT id="Maker" style="WIDTH: 162px; HEIGHT: 22px" type="text" maxLength="150"
																					onchange="" size="50" columnkey="Maker"></TD>
																		</TR>
																		<TR>
																			<TD class="style4" vAlign="bottom" align="right">
																				<asp:Label ID="Label5" runat="server" CssClass="Generallabel" Height="19px" 
                                                                                    Width="112px">Driver: </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 8px" vAlign="bottom" align="left">
																			    <INPUT id="Driver_Name" style="WIDTH: 162px; HEIGHT: 22px" type="text" maxLength="150"
																					onchange="" size="50" columnkey="Driver_Name">
																			    <%--<select id="Driver_ID" runat="server" columnkey="Driver_ID" name="D1" 
                                                                                    style="WIDTH: 200px; HEIGHT: 22px">
                                                                                    <option selected value="Type A">Type A</option>
                                                                                    <option value="Type B">Type B</option>
                                                                                </select>--%>
                                                                             </TD>
																		</TR>
																		<TR>
																			<TD class="style4" vAlign="bottom" align="right">
																				<asp:Label ID="Label6" runat="server" CssClass="Generallabel" Height="19px" 
                                                                                    Width="112px">Remarks: </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 8px" vAlign="bottom" align="left">
																			<INPUT id="Remarks" style="WIDTH: 162px; HEIGHT: 22px" type="text" maxLength="150"
																					onchange="" size="50" columnkey="Remarks"></TD>
																		</TR>
																		<TR>
																			<TD class="style4" vAlign="bottom" align="right">
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
            <ClientSideEvents AfterRowTemplateOpenHandler="grdVehicles_AfterRowTemplateOpenHandler" />
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
