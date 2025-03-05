<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WebForm1.aspx.vb" Inherits="FMovers.Ticketing.UI.WebForm1" %>

<%@ Register tagprefix="igtbl" namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics35.WebUI.UltraWebGrid.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igcmbo" namespace="Infragistics.WebUI.WebCombo" Assembly="Infragistics35.WebUI.WebCombo.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register assembly="Infragistics35.WebUI.UltraWebToolbar.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.UltraWebToolbar" tagprefix="igtbar" %>
<%@ Register assembly="Infragistics35.Web.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.LayoutControls" tagprefix="cc1" %>
<%@ Register assembly="Infragistics35.WebUI.WebDataInput.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.WebDataInput" tagprefix="igtxt" %>
<%@ Register assembly="Infragistics35.WebUI.Misc.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.Misc" tagprefix="igmisc" %>

<%@ Register assembly="Infragistics35.WebUI.UltraWebChart.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.UltraWebChart" tagprefix="igchart" %>
<%@ Register assembly="Infragistics35.WebUI.UltraWebChart.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.UltraChart.Resources.Appearance" tagprefix="igchartprop" %>
<%@ Register assembly="Infragistics35.WebUI.UltraWebChart.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.UltraChart.Data" tagprefix="igchartdata" %>
<%@ Register assembly="Infragistics35.Web.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.Web.UI.EditorControls" tagprefix="cc2" %>
<%@ Register assembly="Infragistics35.WebUI.WebSchedule.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.WebSchedule" tagprefix="igsch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">



.igdw_Control
{
	background-color:Transparent;
	border-width:0px;
}


.ig_Control
{
	background-color:White;
	font-size:xx-small;
	font-family: verdana;
	border:solid 1px #ABC1DE;
	cursor:default;
	color:Black;
}


.igdw_HeaderArea
{
	background-color:Transparent;
	font-weight:bold;
	border-width:0px;
	height: 24px;
	cursor:default;
	color:White;
}


.igdw_HeaderCornerLeft
{
	background-color:Transparent;
	background-position:top left ;
	background-image: url(E:/HPC700/E/FMovers/FMovers.Ticketing.UI/ig_res/Default/images/igdw_headercornerleft.gif);
	width: 9px;
}


.igdw_HeaderContent
{
	background-image: url(E:/HPC700/E/FMovers/FMovers.Ticketing.UI/ig_res/Default/images/igdw_headercontent.gif);
}


.igdw_HeaderButtonArea
{
	width: 120px;
	vertical-align:middle;
}


.igdw_HeaderCornerRight
{
	background-position:top right ;
	background-image: url(E:/HPC700/E/FMovers/FMovers.Ticketing.UI/ig_res/Default/images/igdw_headercornerright.gif);
	width: 9px;
}


.igdw_BodyEdgeLeft
{
	background-color:WhiteSmoke;
	background-repeat:repeat-y;
	border-right:solid 1px #BBBBBB;
	border-left:solid 1px #666666;
	width: 6px;
}


.igdw_BodyContentArea
{
	background-color:White;
	vertical-align:top;
}


.igdw_BodyContent
{
	border:solid 0px #999999;
}


.igdw_BodyEdgeRight
{
	background-color:WhiteSmoke;
	border-right:solid 1px #666666;
	border-left:solid 1px #BBBBBB;
	width: 6px;
}


.igdw_BodyCornerBottomLeft
{
	background-color:WhiteSmoke;
	background-repeat:no-repeat;
	background-image: url(E:/HPC700/E/FMovers/FMovers.Ticketing.UI/ig_res/Default/images/igdw_bodycornerbottomleft.gif);
	font-size:1px;
	height: 14px;
	width: 9px;
}


.igdw_BodyEdgeBottom
{
	background-color:WhiteSmoke;
	background-repeat:repeat-x;
	background-image: url(E:/HPC700/E/FMovers/FMovers.Ticketing.UI/ig_res/Default/images/igdw_bodyedgebottom.gif);
	font-size:1px;
	border-top:solid 1px #BBBBBB;
	height: 14px;
}


.igdw_BodyCornerBottomRight
{
	background-color:Gainsboro;
	background-repeat:no-repeat;
	background-image: url(E:/HPC700/E/FMovers/FMovers.Ticketing.UI/ig_res/Default/images/igdw_bodycornerbottomright.gif);
	font-size:1px;
	height: 14px;
	width: 9px;
}


    .GridFrame
{
	background-color:#F9FAFF;
}

.GridItem
{
	background-color:#F9FAFF;
}

.GridHeader
{
	background-image: url('styles/images/PageBG.jpg');
}


    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
    
    
    
        <table id="tblTickets" align="center" cellpadding="2" cellspacing="5" 
            disabled="disabled" 
            style="width:100%;border-radius: 3px;-moz-border-radius: 3px;border: solid #E7E7E7 3px">
            <tr>
                <td id="cell_0_0_2" class="TicketConfirmed" onclick="OperateOnSeat(this, 2, 2, 4, 1777);" title="Confirmed
Passenger: azeem mahar sb
From City : Sadiqabad
Destination: Multan
Confirmed By: saeed.ahmad
Issued On: 3/6/2020 2:27:33 AM
Mobile No: 00000000000
CNIC : 3130480904937
Cancelled History:  - ">
                    2<br />
                    <div style="float:center;font-size:9px;width:10px;">
                        SDQ
                    </div>
                </td>
                <td id="cell_0_0_1" class="TicketAvailable" 
                    onclick="OperateOnSeat(this, 1, 2, 1, 0);" title="Available
Cancelled History:  - ">
                    1</td>
                <td id="cell_0_0_3Spacer" class="Spacer">
                </td>
                <td id="cell_0_0_2Spacer" class="Spacer">
                </td>
                <td id="cell_0_0_1Spacer" class="Spacer">
                </td>
            </tr>
            <tr>
                <td id="cell_0_0_6Spacer" class="Spacer">
                </td>
                <td id="cell_0_0_5" class="TicketConfirmed" 
                    onclick="OperateOnSeat(this, 5, 2, 4, 1777);" title="Confirmed
Passenger: kleem u alla
From City : Sadiqabad
Destination: Multan
Confirmed By: saeed.ahmad
Issued On: 3/6/2020 3:27:49 AM
Mobile No: 03406025525
CNIC : 3820266236627
Cancelled History:  - ">
                    5<br />
                    <div style="float:center;font-size:9px;width:10px;">
                        SDQ
                    </div>
                </td>
                <td id="cell_0_0_4" class="TicketAvailableTransit" 
                    onclick="OperateOnSeat(this, 4, 2, 1, 0);" title="Available
Cancelled History:  - ">
                    4</td>
                <td id="cell_0_0_3" class="TicketAvailableTransit" 
                    onclick="OperateOnSeat(this, 3, 2, 1, 0);" title="Available
Cancelled History:  - ">
                    3</td>
            </tr>
            <tr>
                <td id="cell_0_0_8" class="TicketAvailableTransit" 
                    onclick="OperateOnSeat(this, 8, 2, 1, 0);" title="Available
Cancelled History:  - ">
                    8</td>
                <td id="cell_0_0_6Spacer0" class="Spacer">
                </td>
                <td id="cell_0_0_7" class="TicketAvailableTransit" 
                    onclick="OperateOnSeat(this, 7, 2, 1, 0);" title="Available
Cancelled History:  - ">
                    7</td>
                <td id="cell_0_0_6" class="TicketConfirmed" 
                    onclick="OperateOnSeat(this, 6, 2, 4, 1777);" title="Confirmed
Passenger: bilal
From City : Sadiqabad
Destination: Multan
Confirmed By: saeed.ahmad
Issued On: 3/6/2020 3:43:40 AM
Mobile No: 03067617693
CNIC : 2170321305003
Cancelled History:  - ">
                    6<br />
                    <div style="float:center;font-size:9px;width:10px;">
                        SDQ
                    </div>
                </td>
            </tr>
            <tr>
                <td id="cell_0_0_11" class="TicketAvailableTransit" 
                    onclick="OperateOnSeat(this, 11, 2, 1, 0);" title="Available
Cancelled History:  - ">
                    11</td>
                <td id="cell_0_0_7Spacer" class="Spacer">
                </td>
                <td id="cell_0_0_10" class="TicketAvailable" 
                    onclick="OperateOnSeat(this, 10, 2, 1, 1);" title="Available
Cancelled History:  - ">
                    10</td>
                <td id="cell_0_0_9" class="TicketAvailable" 
                    onclick="OperateOnSeat(this, 9, 2, 1, 0);" title="Available
Cancelled History:  - ">
                    9</td>
            </tr>
            <tr>
                <td id="cell_0_0_15" class="TicketAvailable" 
                    onclick="OperateOnSeat(this, 15, 2, 1, 1);" title="Available
Cancelled History:  - ">
                    15</td>
                <td id="cell_0_0_14" class="TicketAvailable" 
                    onclick="OperateOnSeat(this, 14, 2, 1, 1);" title="Available
Cancelled History:  - ">
                    14</td>
                <td id="cell_0_0_13" class="TicketAvailable" 
                    onclick="OperateOnSeat(this, 13, 2, 1, 0);" title="Available
Cancelled History:  - ">
                    13</td>
                <td id="cell_0_0_12" class="TicketAvailable" 
                    onclick="OperateOnSeat(this, 12, 2, 1, 0);" title="Available
Cancelled History:  - ">
                    12</td>
            </tr>
        </table>
    
    
    
    </div>
    </form>
</body>
</html>
