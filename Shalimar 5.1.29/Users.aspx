<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Users.aspx.vb" Inherits="FMovers.Ticketing.UI.Users" %>
<%@ Register tagprefix="cc1" namespace="Infragistics.Web.UI.LayoutControls" Assembly="Infragistics35.Web.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igtbl" namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics35.WebUI.UltraWebGrid.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Users</title>
    
    <link href="styles/styles.css" rel="stylesheet" type="text/css" />
<script>
    function CheckTheDuplicateValues() {
        var textvalue = document.getElementById("User_Name").value;
        var grid = igtbl_getGridById("grdUsers");
        var rows = grid.Rows;

        

        if (rows != null) {
            var len = rows.length;

            for (var i = 0; i < rows.length; ++i) {
                var row = grid.Rows.getRow(i);

                // if(row!=null)
                // {
                var cell = row.getCell(1);
                var cellval = cell.getValue();
                if (cellval != textvalue) {
                    //row.setHidden(true);
                    alert("User name already exits.");
                    document.getElementById("User_Name").value = "";
                    document.getElementById("User_Name").focus();
                    return false;
                }

                //return true;
            }
        }
        return false;        
    }

 </script>    
    
    
        <script type="text/javascript">  
    var selectedIDs;   
    var selectedBgColor = "#eee";   
  
    function search_Click()   
    {

        var row, cell, value, color;
        var grid = igtbl_getGridById('grdUsers')

        var searchText = $get("searchText").value;
        var rows        = grid.get_rows();   
  
        selectedIDs = new Array();   
           
        for (var i = 0; i < rows.get_length(); i++)   
        {   
            row = rows.get_row(i);   
               
            for (var j = 0; j < row.get_cellCount(); j++)   
            {   
                cell = row.get_cell(j);   
                   
                color = "";   
                   
                if (searchText.length > 0)   
                {   
                    value = String(cell.get_value()).toLowerCase();   
                       
                    if (value.indexOf(searchText.toLowerCase()) != -1)   
                    {   
                        color = selectedBgColor;   
                        addSelectedID(row.get_cell(0).get_value());   
                    }   
                }   
                   
                cell.get_element().style.backgroundColor = color;   
            }   
        }

        $get("total").innerHTML = selectedIDs.length;

        return false;
    }   
  
    function addSelectedID(id)   
    {   
        var found = false;   
           
        for (var i = 0; i < selectedIDs.length; i++)   
        {   
            if (selectedIDs[i] == id)   
            {   
                found = true;   
                break;   
            }   
        }   
  
        if (!found)   
        {   
            selectedIDs[selectedIDs.length] = id;   
        }   
    }   
    </script>  
    <script language="javascript">
    
    function btnAdd_onclick() 
    {
        var Grid=igtbl_getGridById('grdUsers')
        //alert(Grid);
        igtbl_addNew("grdUsers",0)
        //alert();
	    var row=Grid.Rows.getRow(Grid.Rows.length-1)
	    igtbl_clearSelectionAll(Grid.Id)
	    if(row!=null)
	        row.setSelected(true)
	    Grid.beginEditTemplate()
    }
    
    function btnDelete_onclick() 
    {
        var grid=igtbl_getGridById("grdUsers")
        if(grid!=null)
            grid.deleteSelectedRows()
    }
    
    </script>
    </head>
<body>
    <form id="form1" runat="server">
    <table width="90%" align="center" border="0" class="TableBorder" cellspacing="0">
                                                <tr class="HeaderStyle">
                                                    <td align="left" valign="middle">All Users</td>
                                                </tr>                                                
                                                                                             
                                                <tr >
                                                    <td align="left" valign="middle"><asp:Label ID="lblError" runat="server" CssClass="Errorlabel"></asp:Label></td>
                                                </tr>                                                
                                                <tr>
                                                    <td align="right" valign="middle">
    <igtbl:UltraWebGrid ID="grdUsers" runat="server" Height="300px" Width="100%">
        <bands>
            <igtbl:UltraGridBand>
                <RowEditTemplate>
                    <P align="center">
<TABLE  class="TableBorder" id="Table2" style="WIDTH: 539px;" cellSpacing="1" cellPadding="1" border="0">
															<TR onmouseup="dragStart=false;" onmousedown="dragStart=true" class="HeaderStyle">
																<TD vAlign="middle">
																	<asp:Label id="lblTer" runat="server" Width="251px" CssClass="quicklinksmain">User 
                                                                    Detail</asp:Label></TD>
															</TR>
															<TR>
																<TD style="vAlign="middle" align="center">
																	<TABLE id="Table3" style="WIDTH: 534px;" cellSpacing="1"
																		cellPadding="1" border="0">
																		<TR>
																			<TD class="style1" vAlign="bottom" align="right">
																				<asp:Label id="lblName" runat="server" Width="112px" Height="19px" 
                                                                                    CssClass="Generallabel">Full Name: </asp:Label>
																				</TD>
																			<TD style="HEIGHT: 24px" vAlign="bottom" align="left"><INPUT id="Full_Name" 
                                                                                    style="HEIGHT: 22px; width: 200px;" type="text" maxLength="50" onchange=""
																					size="50" columnkey="Full_Name"></TD>
																		</TR>
																		<tr>
                                                                            <td align="right" class="style1" 
                                                                                valign="bottom">
                                                                                <asp:Label ID="lblName4" runat="server" CssClass="Generallabel" Height="19px" 
                                                                                    Width="112px">User Name: </asp:Label>
                                                                            </td>
                                                                            <td align="left" style="HEIGHT: 24px" valign="bottom">
                                                                                <INPUT id="User_Name" style="HEIGHT: 22px; width: 200px;" type="text" maxLength="50" onchange=""
																					size="50" columnkey="User_Name">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="right" class="style1" 
                                                                                valign="bottom">
                                                                                <asp:Label ID="lblName5" runat="server" CssClass="Generallabel" Height="19px" 
                                                                                    Width="112px">Password: </asp:Label>
                                                                            </td>
                                                                            <td align="left" style="HEIGHT: 24px" valign="bottom">
                                                                                 <input id="Password" columnkey="Password" maxlength="50" onchange="" size="21" 
                                                                                    style="WIDTH: 200px; HEIGHT: 22px" type="password">
                                                                                </input>                                                                               
                                                                            </td>
                                                                        </tr>
																		<TR>
																			<TD class="style1" vAlign="bottom" align="right">
																				<asp:Label ID="lblName0" runat="server" CssClass="Generallabel" Height="19px" 
                                                                                    Width="112px">Admin: </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 24px" vAlign="bottom" align="left">
                                                                              <INPUT id="IsAdmin" type="checkbox" value="Active" onchange="" columnkey="IsAdmin">
                                                                            </TD>
																		</TR>
																		<TR>
																			<TD class="style2" vAlign="bottom" align="right">
																				<asp:Label ID="lblName1" runat="server" CssClass="Generallabel" Height="19px">Is 
                                                                                Active: </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 19px" vAlign="bottom" align="left">
																			<INPUT id="Is_Active" type="checkbox" checked value="Active" onchange="" columnkey="Is_Active">
																			</TD>
																		</TR>	
																		<TR>
																			<TD class="style2" vAlign="bottom" align="right">
																				<asp:Label ID="Label1" runat="server" CssClass="Generallabel" Height="19px">Is 
                                                                                User Application Type : </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 19px" vAlign="bottom" align="left">
                                                                                <asp:DropDownList ID="cmbUserType" runat="server" columnkey="UserType" >
                                                                                 <asp:ListItem Text="Ticketing" Value = "Ticketing" ></asp:ListItem>
                                                                                 <asp:ListItem Text="Web Reports" Value = "Web Reports"></asp:ListItem>
                                                                                 <asp:ListItem Text="Cargo" Value = "Cargo"></asp:ListItem>                                                                                 
                                                                                 <asp:ListItem Text="Accounts" Value = "Accounts"></asp:ListItem>                                                                                                                                                                  
                                                                                 <asp:ListItem Text="Voucher Closing" Value = "Voucher Closing"></asp:ListItem>                                                                                                                                                                                                                                                   
                                                                                </asp:DropDownList>
																			
																			</TD>
																		</TR>																		

																			<TR>
																			<TD class="style2" vAlign="bottom" align="right">
																				<asp:Label ID="Label2" runat="server" CssClass="Generallabel" Height="19px">Is 
                                                                                Emp Code : </asp:Label>
                                                                            </TD>
																			<TD style="HEIGHT: 19px" vAlign="bottom" align="left">
																			
                                                                  <INPUT id="Emp_Code" style="HEIGHT: 22px; width: 200px;" type="text" maxLength="50" onchange=""
																					size="50" columnkey="Emp_Code">
                                                                                
																			
																			</TD>
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
                                                        <input id="btnDelete" runat="server" style="display:none" value="Delete" class="ButtonStyle" type="button" onclick="btnDelete_onclick()"
                                                            Width="81px" />&nbsp;<input id="btnAdd" runat="server" value="New" class="ButtonStyle" type="button"
                                                            Width="81px" onclick="btnAdd_onclick();" />&nbsp;<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ButtonStyle" 
                                                            Width="81px" />
                                                    </td>
                                                </tr>  
                                              </table>
                   
    </form>
</body>
</html>
