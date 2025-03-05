<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ScheduleDetail.aspx.vb" Inherits="FMovers.Ticketing.UI.ScheduleDetail" %>

<%@ Register tagprefix="cc1" namespace="Infragistics.Web.UI.LayoutControls" Assembly="Infragistics35.Web.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igtbl" namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics35.WebUI.UltraWebGrid.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register tagprefix="igsch" namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics35.WebUI.WebDateChooser.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register tagprefix="igtxt" namespace="Infragistics.WebUI.WebDataInput" Assembly="Infragistics35.WebUI.WebDataInput.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Schedule Details</title>
    <link href="styles/styles.css" rel="stylesheet" type="text/css" />
    <script language="javascript">

        function validation() {                   
            if (document.getElementById("txtTitle").value == "") {
                alert("Please Specify Schedule Title!");
                document.getElementById("txtTitle").focus()
                return false;
            }

            if ((document.getElementById("cmbRoutes").value == "") || (document.getElementById("cmbRoutes").value == "0")) {
                alert("Please Specify the Route!");
                document.getElementById("cmbRoutes").focus()
                return false;
            }
            
            var grid = igtbl_getGridById("grdSchedules");
            var rows = grid.Rows;
            var srNo;
            for (var i=0; i < rows.length; i++){
                srNo = rows.getRow(i).getCell(1).getValue();
                if (isNaN(parseInt(srNo))){
                    alert("Invalid Serial #!");
                    return false;
                }
                for (var j = i+1; j < rows.length; j++){
                    if (srNo == rows.getRow(j).getCell(1).getValue()){
                        alert("Invalid Serial #!")
                        return false;
                    }
                }
            }
            return true;
        }

    function btnAdd_onclick() {
        var Grid = igtbl_getGridById('grdSchedules')
        igtbl_addNew("grdSchedules", 0)
        
	    var row=Grid.Rows.getRow(Grid.Rows.length-1)
	    igtbl_clearSelectionAll(Grid.Id)
	    if(row!=null)
	        row.setSelected(true)
    }
    
    function btnDelete_onclick() {    
        var grid = igtbl_getGridById("grdSchedules")
        if(grid!=null)
            grid.deleteSelectedRows()
    }

    function pageInit() {  
                  
    }
    
    
    </script>
   
    <script type="text/javascript" id="igClientScript">
<!--

function grdSchedules_AfterRowInsertHandler(gridName, rowId, index){
    
    var grid = igtbl_getGridById("grdSchedules");
    var rows = grid.Rows;
    var RowIndex = rowId.substring(15,rowId.length);
    var DefaultRow = rows.getRow(0);
    var newRow = rows.getRow(RowIndex);
    //alert(DefaultRow.getCell(0));
    
    newRow.getCell(0).setValue(DefaultRow.getCell(0).getValue());
    newRow.getCell(1).setValue(rows.length-1);
    
    //alert(newRow.cells.length)
    
    for (var i=0; i < (newRow.cells.length - 2)/6; i++)
	{
        newRow.getCell((i*6)+3).setValue(DefaultRow.getCell((i*6)+3).getValue());
        if (i == 0)
        {
            newRow.getCell((i*6)+4).setValue(0);
            newRow.getCell((i*6)+5).setValue(0);
            newRow.getCell((i*6)+7).setValue(0);
        }
        else
        {
            newRow.getCell((i*6)+4).setValue(DefaultRow.getCell((i*6)+4).getValue());
            newRow.getCell((i*6)+5).setValue(DefaultRow.getCell((i*6)+5).getValue());
            newRow.getCell((i*6)+7).setValue(DefaultRow.getCell((i*6)+7).getValue());
        }
        //newRow.getCell((i*6)+3).setValue(DefaultRow.getCell((i*6)+3).getValue());
	} 
    
    //alert(newRow);
	//Add code to handle your event here.
}

function grdSchedules_CellChangeHandler(gridName, cellId)
{
    var grid = igtbl_getGridById("grdSchedules");
    var rows = grid.Rows;
    var cellindexes = cellId.substring(16, cellId.length).split("_");
    //var DefaultRow = rows.getRow(0);
    var changedRow = rows.getRow(cellindexes[0]);
    
    if (cellindexes[1] == "6")
    {
        //alert(cellindexes[1]);
        var depTime = changedRow.getCell(6).getValue();
        var ArrTime, TravelTime, StayPeriod;
        var dts, tts, sps, dhrs, dmins;
        for (var i=0; i < (changedRow.cells.length - 2)/6; i++)
	    {
	        //alert(0)
	        if (i != 0)
	        {
	            
	            //ArrTime = changedRow.getCell((i*6)+4).getValue();
	            StayPeriod = changedRow.getCell((i*6)+5).getValue();
	            TravelTime = changedRow.getCell((i*6)+7).getValue();
	            dts = depTime.split(":");
	            tts = TravelTime.split(":");	            
	            sps = StayPeriod.split(":");            
	            
	            dhrs = CastInt(dts[0]) + CastInt(tts[0]); //+ CastInt(sps[0]
	            dmins= CastInt(dts[1]) + CastInt(tts[1]); // + CastInt(sps[1]
	            
	            if (dmins > 59)
	            {
	                dhrs = dhrs + 1;
	                dmins = dmins - 60;
	            }
	            
	            if (dhrs > 23)
	                dhrs = dhrs - 24
	            
	            ArrTime = SetMinHrs(dhrs) + ":" + SetMinHrs(dmins)
	            
	            dhrs = dhrs + CastInt(sps[0]);
	            dmins = dmins + CastInt(sps[1]);
	            
	            if (dmins > 59)
	            {
	                dhrs = dhrs + 1;
	                dmins = dmins - 60;
	            }
	            if (dhrs > 23)
	                dhrs = dhrs - 24
	            
	            if (i + 1 < (changedRow.cells.length - 2)/6)
	                depTime = SetMinHrs(dhrs) + ":" + SetMinHrs(dmins)
	            else
	                depTime = "00:00"
	            
	            changedRow.getCell((i*6)+4).setValue(ArrTime);
	            changedRow.getCell((i*6)+6).setValue(depTime);
	        }
	    }
        
    }
  
}

function grdSchedules_BeforeRowDeletedHandler(gridId, RowId){

var RowIndex = rowId.substring(15,rowId.length);
if (RowIndex == 0)
    return false;
//alert(gridId);
//alert(RowId);

}

function CastInt(exp)
{
    if (exp == "00")
        return 0;
    if ((exp == "01") || (exp == "02") || (exp == "03") || (exp == "04") || (exp == "05") || (exp == "06") || (exp == "07") || (exp == "08") || (exp == "09"))
        return parseInt(exp.replace("0", ""));
    else if ((exp != null) && (exp != NaN))
        return parseInt(exp);
    else
        return 0;
}

function SetMinHrs(intValue)
{
    if (intValue == 0)
        return "00"
    else if ((intValue == 1) || (intValue == 2) || (intValue == 3) || (intValue == 4) || (intValue == 5) || (intValue == 6) || (intValue == 7) || (intValue == 8) || (intValue == 9))
        return "0" + intValue
    else 
        return intValue 
}

// -->
</script>
   </head>
<body onload="pageInit();">
    <form id="frm1" runat="server">
    <div>
    
    </div>
                                            <table width="90%" align="center" border="0" class="TableBorder" cellspacing="0">
                                                <tr class="HeaderStyle">
                                                    <td align="left" valign="middle">
                                                        Schedule Details</td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="middle">
                                                        <table cellpadding="2" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td align="right" width="20%">
                                                                    <asp:Label ID="Label1" runat="server" Text="Schedule Title: " 
                                                                        CssClass="Generallabel"></asp:Label>
                                                                </td>
                                                                <td width="30%">
                                                                    <asp:TextBox ID="txtTitle" runat="server" Width="80%" CssClass="txtbox" 
                                                                        MaxLength="40" Height="20px"></asp:TextBox>
                                                                </td>
                                                                <td align="right" width="20%" >
                                                                    <asp:Label ID="Label3" runat="server" Text="Schedule Code: " 
                                                                        CssClass="Generallabel" style="display:none" ></asp:Label>
                                                                </td>
                                                                <td width="30%">
                                                                    <asp:TextBox ID="txtCode" runat="server" Width="80%" CssClass="txtbox" 
                                                                        MaxLength="40" Height="20px" style="display:none" ></asp:TextBox>
                                                                </td>
                                                                
                                                            </tr>
                                                            <tr>
                                                                <td align="right" width="20%">
                                                                    <asp:Label ID="Label4" runat="server" Text="Schedule Date: " 
                                                                        CssClass="Generallabel"></asp:Label>
                                                                </td>
                                                                <td width="30%">
                                                                    <igsch:WebDateChooser ID="dtSchedule" runat="server" Width="80%" 
                                                                        NullDateLabel="Select Date" AllowNull="False" Editable="False">
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
                                                                <td align="right" width="20%">
                                                                    <asp:Label ID="Label5" runat="server" Text="With Effect From: " 
                                                                        CssClass="Generallabel"></asp:Label>
                                                                </td>
                                                                <td width="30%">
                                                                    <igsch:WebDateChooser ID="dtWEF" runat="server" Width="80%" 
                                                                        NullDateLabel="Select Date" AllowNull="False" Editable="False">
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
                                                                <td align="right" width="20%">
                                                                    <asp:Label ID="Label6" runat="server" Text="Route: " 
                                                                        CssClass="Generallabel"></asp:Label>
                                                                </td>
                                                                <td width="30%">
                                                                    <asp:DropDownList ID="cmbRoutes" runat="server" Width="80%" AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td align="right" width="20%">
                                                                    <asp:Label ID="Label7" runat="server" Text="Comission :" 
                                                                        CssClass="Generallabel"></asp:Label>
                                                                </td>
                                                                <td width="30%"><input id="hidSchID" type="hidden" runat="server" value='0' />
                                                                    <asp:TextBox ID="txtComission" runat="server" Width="80%" CssClass="txtbox" 
                                                                        MaxLength="40" Height="20px"></asp:TextBox>
                                                                </td>                                                                
                                                            </tr>                                                           
                                                            <tr>
                                                                <td align="right" width="20%">
                                                                    &nbsp;</td>
                                                                <td width="30%">
                                                                    &nbsp;</td>
                                                                <td align="right" width="20%">
                                                                    <asp:Label ID="Label8" runat="server" Text="Extra Fare :" 
                                                                        CssClass="Generallabel"></asp:Label>
                                                                </td>
                                                                <td width="30%">
                                                                    <asp:TextBox ID="txtExtraFare" runat="server" Width="80%" CssClass="txtbox" 
                                                                        MaxLength="40" Height="20px"></asp:TextBox>
                                                                </td>                                                                
                                                            </tr>                                                           
                                                        </table>
                                                    </td>
                                                </tr> 
                                                <tr>
                                                    <td align="right" valign="middle">
    <igtbl:UltraWebGrid ID="grdSchedules" runat="server" Height="200px" Width="100%">
        <bands>
            <igtbl:UltraGridBand>
                <addnewrow view="NotSet" visible="NotSet">
                </addnewrow>
            </igtbl:UltraGridBand>
        </bands>
        <displaylayout 
            allowdeletedefault="Yes" 
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
            <ClientSideEvents AfterRowInsertHandler="grdSchedules_AfterRowInsertHandler" 
                aftercellupdatehandler="grdSchedules_CellChangeHandler" 
                BeforeRowDeletedHandler="grdSchedules_BeforeRowDeletedHandler" />
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
                                                    <td align="right" valign="middle" height="5px">
                                                        &nbsp;
                                                        </td>
                                                </tr>  
                                               <tr>
                                                    <td align="right" valign="middle" height="5px">                                                        
                                                        <input id="btnAdd" runat="server" value="New" class="ButtonStyle" type="button" 
                                                            Width="81px" onclick="btnAdd_onclick()" style="width: 80px;"/>&nbsp;<input 
                                                            id="btnDelete" runat="server" value="Delete" class="ButtonStyle" type="button" 
                                                            Width="81px" onclick="btnDelete_onclick()" style="width: 80px;"/>&nbsp;<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ButtonStyle" Width="81px" />&nbsp;<asp:Button ID="btnSavenClose" runat="server" Text="Save & Close" 
                                                            CssClass="ButtonStyle" Width="110px" />&nbsp;<asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ButtonStyle" width="100px"/>
                                                    </td>
                                                </tr>  
                                              </table>
                   
    
    </form>
</body>
</html>
