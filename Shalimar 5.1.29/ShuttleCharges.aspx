<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ShuttleCharges.aspx.vb" Inherits="FMovers.Ticketing.UI.ShuttleCharges" %>

<%@ Register tagprefix="cc1" namespace="Infragistics.Web.UI.LayoutControls" Assembly="Infragistics35.Web.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igtbl" namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics35.WebUI.UltraWebGrid.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Shuttle Charges</title>
    <link href="styles/styles.css" rel="stylesheet" type="text/css" />
    <script language="javascript">

        function validation() {                   
            
            var Grid = igtbl_getGridById('grdDetails')
            if (Grid.Rows.length == 0) {
                alert("Please enter Fare Information");
                return false;
            }
        }

    function btnAdd_onclick() {
        var Grid = igtbl_getGridById('grdDetails')
        igtbl_addNew("grdDetails", 0)
        
	    var row=Grid.Rows.getRow(Grid.Rows.length-1)
	    igtbl_clearSelectionAll(Grid.Id)
	    if(row!=null)
	        row.setSelected(true)
    }
    
    function btnDelete_onclick() {    
        var grid = igtbl_getGridById("grdDetails")
        if(grid!=null)
            grid.deleteSelectedRows()
    }

    function pageInit() {  
                  
    }
    
    
    </script>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
    </head>
<body onload="pageInit();">
    <form id="form1" runat="server">
    <div>
    
    </div>
                                            <table width="90%" align="center" border="0" class="TableBorder" cellspacing="0">
                                                <tr class="HeaderStyle">
                                                    <td align="left" valign="middle">Shuttle Charges</td>
                                                </tr>                                                
                                                <tr   >
                                                    <td align="left" valign="middle">
                                                    
                                                        <table class="style1">
                                                            <tr>
                                                                <td width="150px" >
                                                                    Select Terminal : </td>
                                                                <td width="250px">
                                                                    <asp:DropDownList ID="cboTerminals" runat="server">
                                                                    </asp:DropDownList> </td>
                                                                <td>
                                                                    <asp:Button ID="btnLoad" runat="server" Text="Load" />  </td>
                                                                <td>
                                                                    &nbsp;</td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>                                                
                                                <tr>
                                                    <td align="right" valign="middle">
    <igtbl:UltraWebGrid ID="grdDetails" runat="server" Height="500px" Width="100%">
        <bands>
            <igtbl:UltraGridBand>
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
            <SelectedRowStyleDefault BackColor="#99CCFF">
            </SelectedRowStyleDefault>
            <groupbybox hidden="True">
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
                                                    <td align="right" valign="middle" height="5px">
                                                        &nbsp;
                                                        </td>
                                                </tr>  
                                               <tr>
                                                    <td align="right" valign="middle" height="5px">                                                        
                                                        <input 
                                                            id="btnDelete" runat="server" value="Delete" class="ButtonStyle" type="button" 
                                                            Width="81px" onclick="btnDelete_onclick()" style="width: 81px;"/>                                                        
                                                        <input id="btnAdd" runat="server" value="New" class="ButtonStyle" type="button" 
                                                            Width="81px" onclick="btnAdd_onclick()" style="width: 81px;"/>&nbsp;<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ButtonStyle" Width="81px" />&nbsp;</td>
                                                </tr>  
                                              </table>
                   
    </form>
</body>
</html>
