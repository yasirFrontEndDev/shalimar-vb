<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ExcludeTerminals.aspx.vb" Inherits="FMovers.Ticketing.UI.ExcludeTerminals" %>

<%@ Register tagprefix="cc1" namespace="Infragistics.Web.UI.LayoutControls" Assembly="Infragistics35.Web.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igtbl" namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics35.WebUI.UltraWebGrid.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Schedules</title>
    <link href="styles/styles.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
    
  function btnmodify_Click() {
            var grd = igtbl_getGridById('grdScheules');
            var oRow;
            //var selectedIndex = -1;
            var i;
            var SchID = 0; //, RouteCode, RouteName;

            for (i = 0; i < grd.Rows.length; i++) {
                oRow = grd.Rows.getRow(i);
                if (oRow.getSelected() == true) {
                    SchID = oRow.getCell(0).getValue();
                    //selectedIndex = i;
                    break;
                }
            }

            if (SchID == 0) {
                alert('Please select a record to modify.');
                return false;
            }            

//            if (selectedIndex >= 0) {
//                RouteID = grd.Rows.getRow(selectedIndex).getCell(0).getValue();
//                RouteCode = grd.Rows.getRow(selectedIndex).getCell(2).getValue();
//                RouteName = grd.Rows.getRow(selectedIndex).getCell(3).getValue();
//                
//            }

            document.getElementById("hidSchID").value = SchID
            return true;
//            if (RouteCode == null)
//                RouteCode = '';

//            if (RouteName == null)
//                RouteName = '';
            
//            document.getElementById("hidRouteCode").value = RouteCode
//            document.getElementById("hidRouteName").value = RouteName
        }

        function btndelete_Click() {
            var grd = igtbl_getGridById('grdScheules');
            var oRow;
            //var selectedIndex = -1;
            var i
            var SchID = 0;

            for (i = 0; i < grd.Rows.length; i++) {
                oRow = grd.Rows.getRow(i);
                if (oRow.getSelected() == true) {
                    SchID = oRow.getCell(0).getValue();
                    //selectedIndex = i;
                    break;
                }
            }

            if (SchID == 0) {
                alert('Please select a record to delete.');
                return false;
            }

//            if (selectedIndex >= 0) {
//                RouteID = grd.Rows.getRow(selectedIndex).getCell(0).getValue();
//            }

            document.getElementById("hidSchID").value = SchID
            return true;
        }

        

    </script>
    </head>
<body>
    <form id="form1" runat="server">
   <table width="90%" align="center" border="0" class="TableBorder" cellspacing="0">
                                                <tr class="HeaderStyle">
                                                    <td align="left" valign="middle">Exclude Boarding Points </td>
                                                </tr>                                                
                                                <tr>
                                                    <td align="right" valign="middle">
    <igtbl:UltraWebGrid ID="grdScheules" runat="server" Height="300px" Width="100%">
        <bands>
            <igtbl:UltraGridBand>
                              
<AddNewRow Visible="NotSet" View="NotSet"></AddNewRow>
               
            </igtbl:UltraGridBand>
        </bands>
        <displaylayout allowcolsizingdefault="Free" allowcolumnmovingdefault="OnServer" 
            allowsortingdefault="OnClient" bordercollapsedefault="Separate" 
            headerclickactiondefault="SortMulti" name="UltraWebGrid1" 
            rowheightdefault="20px" 
            selecttyperowdefault="Single" stationarymargins="Header" 
            stationarymarginsoutlookgroupby="True" tablelayout="Fixed" version="4.00" 
            cellclickactiondefault="RowSelect">
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
                                                    <td align="right" valign="middle" height="5px"><input id="hidSchID" type="hidden" runat="server" value='' />
                                                        <input id="btnAdd" runat="server" value="Add" class="ButtonStyle" type="button" Width="81px"  />
                                                        <input id="btnModify" runat="server" value="Modify" class="ButtonStyle" type="submit" Width="81px" onclick="return btnmodify_Click();" />
                                                        <input id="btnDelete" runat="server" value="Delete" class="ButtonStyle" type="submit" Width="81px" onclick="return btndelete_Click();" />
                                                     </td>
                                                </tr>  
                                              </table>
                   
    </form>
</body>
</html>
