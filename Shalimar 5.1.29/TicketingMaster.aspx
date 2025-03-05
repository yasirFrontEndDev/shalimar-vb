<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TicketingMaster.aspx.vb" Inherits="FMovers.Ticketing.UI.TicketingMaster" %>

<%@ Register tagprefix="cc1" namespace="Infragistics.Web.UI.LayoutControls" Assembly="Infragistics35.Web.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igtbl" namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics35.WebUI.UltraWebGrid.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register tagprefix="igcmbo" namespace="Infragistics.WebUI.WebCombo" Assembly="Infragistics35.WebUI.WebCombo.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register tagprefix="igmisc" namespace="Infragistics.WebUI.Misc" Assembly="Infragistics35.WebUI.Misc.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register assembly="Infragistics35.WebUI.WebDataInput.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.WebDataInput" tagprefix="igtxt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">

    <title>Ticketing</title>
    <link href="styles/styles.css" rel="stylesheet" type="text/css" />
    <script src="script/scriptlib.js" type="text/javascript"></script>
    <link rel="stylesheet" href="styles/jquery.ui.all.css">
	<link rel="stylesheet" href="styles/demos.css">
	<style>
	.ui-button { margin-left: -1px; }
	.ui-button-icon-only .ui-button-text { padding: 0.35em; } 
	.ui-autocomplete-input { margin: 0; padding: 0.48em 0 0.47em 0.45em; }
	</style>
	
    
    <script type="text/javascript" >


function warpTicketing_InitializePanel(oPanel)
{

    alert("i am here");
  var pi = oPanel.getProgressIndicator();
  //pi.setTemplate('<div>wait...</div>');
  pi.setRelativeContainer(document.body);
oPanel._oldFixPI = oPanel.fixPI;
oPanel.fixPI = function(pi)
{

this._oldFixPI(pi);
var body = document.body;

var html = body.parentNode;
var y = body.scrollTop, x = body.scrollLeft, w = body.offsetWidth, h = body.offsetHeight;if(y == 0)

{

y = html.scrollTop;

x = html.scrollLeft;

w = html.clientWidth;

h = html.clientHeight;

}
if(w > 0)

x += w / 2 - 275 / 2;
if(h > 0)

y += h / 2 - 85 / 2;

pi.setLocation({x:x, y:y});

}

}
  
	</script>

    
    <script type="text/javascript" >
    

    var UserName = "admin";
    var CurrentUserID = "1";


    function btnVoucherReport_onclick()
    {
        if (document.getElementById("VoucherStatus").value=="1")
        {
            window.open('Reports/PrintVoucherReport.aspx?TSID=' + '<% =hidTSID.Value %>&status=1');
        }
        if (document.getElementById("VoucherStatus").value=="2")
        {
            window.open('Reports/PrintVoucherReport.aspx?TSID=' + '<% =hidTSID.Value %>&status=2');
        }        
        return false;
    }
        
    function btnVoucherReportNull_onclick()
    {

        if (document.getElementById("VoucherStatus").value=="1")
        {
            window.open('Reports/PrintVoucherReportNull.aspx?TSID=' + '<% =hidTSID.Value %>&status=1');
        }
        if (document.getElementById("VoucherStatus").value=="2")
        {
            window.open('Reports/PrintVoucherReportNull.aspx?TSID=' + '<% =hidTSID.Value %>&status=2');
        }        
        return false;
    }

    function chkOnline_onclick()
    {
 
        if (document.getElementById("chkOnline").checked == true)
            document.getElementById("lnkMapping").style.display = "";
        else
            document.getElementById("lnkMapping").style.display = "none";
    }
    
    function OperateOnSeat(cell, SeatNo, OperationType, status, SeatUserId)
    {
        try
        {
            //ProgressBar
        document.getElementById("ProgressBarwapper").style.visibility = "visible";
        if (document.getElementById("tblTickets").disabled == false)
        {
            if (OperationType == 2) {               
                if (document.getElementById("hidMode").value == "2"){
                    if (status != 4){
                        if (document.getElementById("txtSeatNo").value == "")
                            document.getElementById("txtSeatNo").value = SeatNo;
                        else
                            document.getElementById("txtSeatNo").value = document.getElementById("txtSeatNo").value + "," + SeatNo;
                        document.getElementById("hidSeatNo").value = SeatNo; 
                    }
                    if (status == 1) {
                        __doPostBack('lnkReserve', '')
                    }
                    else if (status == 2){
                        __doPostBack('lnkMakeAvailable', '')
                    }
                    else if (status == 3) {
                        document.getElementById("txtSeatNo").value = SeatNo;
                        __doPostBack('lnkSeatDetail', '');
                        //document.getElementById("btnCancelTicket").value = "Cancel Booking"
                    }
                }
                else
                {
                    
                    //alert("i am here");
                    document.getElementById("tblTickets").disabled = false;
                    if (document.getElementById("txtSeatNo").value == "")
                        document.getElementById("txtSeatNo").value = SeatNo;
                    else
                        document.getElementById("txtSeatNo").value = document.getElementById("txtSeatNo").value + "," + SeatNo;
                    document.getElementById("hidSeatNo").value = SeatNo;
                    
                    if (status == 2){
                        __doPostBack('lnkMakeAvailable', '')
                    }
                    else if (status == 3) {
                        document.getElementById("txtSeatNo").value = SeatNo;
                        __doPostBack('lnkSeatDetail', '');
                        //document.getElementById("btnCancelTicket").value = "Cancel Booking"
                        //document.getElementById("btnSave").style.display = ""
                    }
                    else if (status == 4) {
                        if ((SeatUserId == CurrentUserID) || (UserName == "admin"))
                        {
                            document.getElementById("txtSeatNo").value = SeatNo;
                            __doPostBack('lnkSeatDetail', '');
                        }
                        else
                            alert("No Access.");
                        //alert(document.getElementById("btnCancelTicket"));
                        //document.getElementById("btnCancelTicket").value = "Cancel Ticket"
                    }
                    else if (status == 1) {
                        __doPostBack('lnkReserve', '')
                    }
                    document.getElementById("tblTickets").disabled = true ;                    
                }
            }
            else if (OperationType == 1) {
            //for single click
            }
            document.getElementById("ProgressBarwapper").style.visibility = "hidden";

        }

        }
        catch (e)
			{
			    alert("Can't connect to server:\n" + e.toString());
			    document.getElementById("ProgressBarwapper").style.display = "none";

			}
    }
    
    function txtSeatNo_onblur(){
        var Seats = document.getElementById("txtSeatNo").value.split(",");
        var Fare = parseInt(document.getElementById("txtFare").value);
        
        document.getElementById("txtTotal").value = Seats.length * Fare;
        
       
    }

function validateclose ()
{

        var DriverName = document.getElementById("txtDriverName").value;
        var HostessName = document.getElementById("txtHostessName").value;
        
        if (DriverName=="")
        {
        
           alert("Please enter driver name. ");
           return false;
        
        
        }

        if (HostessName=="")
        {
        
           alert("Please enter driver Hostess Name. ");
           return false;
        
        
        }

return true;
}

    function txtAmount_onblur() {
        var Fare = parseInt(document.getElementById("txtFare").value);
        var Total = parseInt(document.getElementById("txtTotal").value);
        var Amount = parseInt(document.getElementById("txtAmount").value);
        
        if (Total == NaN){
            txtSeatNo_onblur();
            Total = parseInt(document.getElementById("txtTotal").value);
        }

        if ((Total != NaN) && (Amount != NaN)) {
            if ((Amount - Total) > 0)
                document.getElementById("txtBalance").value = Amount - Total;
            else
                document.getElementById("txtBalance").value = "0";
        }
    }

    function validation() {
        //var combo;
        //var index;

//        combo = igcmbo_getComboById('cboVoucherNo');
//        index = -1;
//        index = combo.getSelectedIndex();

//        if (index == -1) {
//            alert("Please select Voucher Number.");                
//            return false;                
//        }

//        combo = igcmbo_getComboById('cboSource');
//        index = -1;
//        index = combo.getSelectedIndex();

//        if (index == -1) {
//            alert("Please select Source City.");
//            return false;                
//        }

//        combo = igcmbo_getComboById('cboDestination');
//        index = -1;
//        index = combo.getSelectedIndex();

//        if (index == -1) {
//            alert("Please select Desitnation City.");
//            return false;                
//        }

        if (document.getElementById("cmbSource").value == "") {
            alert("No Source City!");
            //document.getElementById("cmbSource").focus();
            return false;
        }
        
        if (document.getElementById("cmbDestination").value == "") {
            alert("Please select Desitnation City!");
            document.getElementById("cmbDestination").focus();
            return false;
        }
        if (document.getElementById("cmbDestination").value == "Please select") {
            alert("Please select Desitnation City!");
            document.getElementById("cmbDestination").focus();
            return false;
        }
        if (pageInit(document.getElementById("txtFare").value) == 0) {
            alert("Please select Desitnation which have fare!");
            document.getElementById("cmbDestination").focus();
            return false;
        }

            
//        if (document.getElementById("txtPassengerName").value == "") {
//            alert("Please Specify the Passenger Name!");
//            document.getElementById("txtPassengerName").focus();
//            return false;
//        }
          
        if (document.getElementById("txtSeatNo").value == "") {
            alert("Please Specify the Seat #!");
            return false;
        }
            
        return true;
    }
        
        function print(){
            if (document.getElementById("hdnPrint").value == "1")
            {
                document.getElementById("hdnPrint").value = "0"
                var TicketNo = document.getElementById("txtTicketNo").value
                
                var PassengerName= document.getElementById("txtPassengerName").value
                var ContractNo= document.getElementById("txtContactNo").value
                
                var SeatNo= document.getElementById("txtSeatNo").value
                var Fare= document.getElementById("txtFare").value
                
                var Route= document.getElementById("hidRoute").value
                var DepartureDateTime= document.getElementById("txtDepartureDate").value + ' ' + document.getElementById("txtDepartureTime").value
                var VehicleNo= document.getElementById("txtVehicleNo").value
                document.getElementById("txtFare").value = "";
                document.getElementById("txtTotal").value = "";
                document.getElementById("txtAmount").value = "";
                document.getElementById("txtBalance").value = "";
                
                
                //alert("PrintTicket.aspx?TicketNo=" + TicketNo + "&PassengerName=" + PassengerName + "&ContractNo=" + ContractNo + "&SeatNo=" + SeatNo + "&Fare=" + Fare + "&Route=" + Route + "&DepartureDateTime=" + DepartureDateTime + "&VehicleNo=" + VehicleNo);
                //window.open("http://localhost/print.asp?TicketNo=" + TicketNo + "&PassengerName=" + PassengerName + "&ContractNo=" + ContractNo + "&SeatNo=" + SeatNo + "&Fare=" + Fare + "&Route=" + Route + "&DepartureDateTime=" + DepartureDateTime + "&VehicleNo=" + VehicleNo,"PrintTicket","width=10px,height=10px");
            }
        }
   


        function updateTotal() {


            var Totalss = parseInt(document.getElementById("hidTotal").value);
            var txtComPer = parseInt(document.getElementById("txtComPer").value);
            var commFinal = (Totalss*txtComPer)/100;


            document.getElementById("txtcommission").value = Math.round(commFinal);

            
            var HostessSalary = parseInt(document.getElementById("txtHostessSalary").value);
            var DriverSalary = parseInt(document.getElementById("txtDriverSalary").value);
            var GuardSalary = parseInt(document.getElementById("txtGuardSalary").value);
            var ServiceCharges = parseInt(document.getElementById("txtServiceCharges").value);
            var CleaningCharges = parseInt(document.getElementById("txtCleaningCharges").value);
            var HookCharges = parseInt(document.getElementById("txtHookCharges").value);
            var BusCharges = parseInt(document.getElementById("txtBusCharges").value);
            var Refreshment = parseInt(document.getElementById("txtRefreshment").value);
            var Toll = parseInt(document.getElementById("txtToll").value);
            var Commission = parseInt(document.getElementById("txtcommission").value);
            
            if (isNaN(Commission))
                Commission = 0;

            if (isNaN(HostessSalary))
                HostessSalary = 0;

            if (isNaN(DriverSalary))
                DriverSalary = 0;

            if (isNaN(GuardSalary))
                GuardSalary = 0;

            if (isNaN(ServiceCharges))
                ServiceCharges = 0;

            if (isNaN(CleaningCharges))
                CleaningCharges = 0;

            if (isNaN(HookCharges))
                HookCharges = 0;

            if (isNaN(BusCharges))
                BusCharges = 0;

            if (isNaN(Refreshment))
                Refreshment = 0;

            if (isNaN(Toll))
                Toll = 0;

            var total = Commission + HostessSalary + DriverSalary + GuardSalary + ServiceCharges + CleaningCharges + HookCharges + BusCharges + Refreshment + Toll ;
            document.getElementById("txtTotalDeductions").value = total;
        }

        function closeVoucher() {           
            //var combo = igcmbo_getComboById('cboVoucherNo');

            // get the currently selected row in webcombo
            //var selectedIndex = -1;
            //selectedIndex = combo.getSelectedIndex();
            //if (selectedIndex == -1)
          //  if (document.getElementById("txtVoucherNo").value == ""){
            //    alert("No Voucher created for this schedule.");
             //   return false;
          //  }
            return true;


            // get grid reference from webcombo
//            var grd = combo.getGrid();
//            var oRow;
//            var VoucherNo;

//            if (selectedIndex == -1) {
//                alert('Please select Voucher Number.');
//                return false;
//            }

//            if (selectedIndex >= 0) {
//                VoucherNo = grd.Rows.getRow(selectedIndex).getCell(2).getValue();
//            }
//            
//            var strparam = "center:yes;dialogHeight:300px;dialogWidth:1500px;status:no"
//            openPopup("BusCharges.aspx?", this, strparam);
       }
        
        function pageInit() {  
                        
            if (document.getElementById("hidPrint").value == '1')
            {
                if (document.getElementById("hdnPrint").value == "1")
                {
                    document.getElementById("hdnPrint").value = "0"
                    //print();
                }
                
                document.getElementById("hidPrint").value = 0;
            }                              
        }    
    
    </script>
   
    <script type="text/javascript">
    
  function calcComm()
{
 
}
    </script>
    <script type="text/javascript" id="igClientScript">
<!--

function warpTicketing_RefreshResponse(oPanel,oEvent){
    //alert("warpTicketing_RefreshResponse");
    //alert(oPanel);
    //alert(oEvent);
	//Add code to handle your event here.
}

function warpTicketing_RefreshComplete(oPanel,oEvent){
    //alert("warpTicketing_RefreshComplete");
    //alert(oPanel);
    //alert(oEvent);
	//Add code to handle your event here.
	print();
}
function displayKeyCode(evt)
 {
	var textBox = getObject('txtChar');
	 var charCode = (evt.which) ? evt.which : event.keyCode

	 if (charCode == 13) 
	 {
	   //alert("i am here");
	   if (validation())
	   {
	      __doPostBack('btnSave', '')
	      
	   }
	 }



	//return false;

 }
 
   function loadvoucher()
    {
       //alert(id);
       	   __doPostBack('cboTime_TextChanged','')
       
    }
    
    
 function getObject(obj)
  {
	  var theObj;
	  if (document.all) {
		  if (typeof obj=='string') {
			  return document.all(obj);
		  } else {
			  return obj.style;
		  }
	  }
	  if (document.getElementById) {
		  if (typeof obj=='string') {
			  return document.getElementById(obj);
		  } else {
			  return obj.style;
		  }
	  }
	  return null;
  }
 //-->

// -->

function displaynone()
{

  document.getElementById("lnkCreateOnline").style.display ="none";
 
}
</script>
   
    
</head>
<body onload="pageInit();" bgcolor="white" onkeydown="javascript:return displayKeyCode(event)" >
    <form id="form1" runat="server"  >
    <igmisc:WebAsyncRefreshPanel ID="warpTicketing" runat="server" 
        RefreshResponse="warpTicketing_RefreshResponse" 
        RefreshComplete="warpTicketing_RefreshComplete" Overflow="Visible" 
        BrowserTarget="UpLevel"  >
        
        <div class="ProgressBar" id="ProgressBarwapper"  >
            Nauman
        </div>

                                            <table width="90%" align="center" border="0" class="TableBorder" cellspacing="0">
                                            <tr class="HeaderStyle" >
                                                                <td align="center"  colspan=2 >
                                                                <asp:Label ID="lblheader" runat="server" CssClass="Generallabel" 
                                                                        Text="Voucher Time and Rout"></asp:Label>
                                                                    </td>
                                                            </tr>
                                                <tr>
                                                    <td align="center" valign="top" width="60%">
                                                       
                                                        <table cellpadding="2" cellspacing="0" style="width: 95%">                                                            
                                                            
                                                            <tr>
                                                                <td align="right" >
                                                                    &nbsp;</td>
                                                                <td align="left">
                                                                    <igtbl:UltraWebGrid ID="cboVoucherNo" runat="server" Height="200px" 
                                                                        Width="325px">
                                                                        <Bands>
                                                                            <igtbl:UltraGridBand>
                                                                                <AddNewRow View="NotSet" Visible="NotSet">
                                                                                </AddNewRow>
                                                                            </igtbl:UltraGridBand>
                                                                        </Bands>
                                                                        <DisplayLayout AllowColSizingDefault="Free" AllowColumnMovingDefault="OnServer" 
                                                                            AllowDeleteDefault="Yes" AllowSortingDefault="OnClient" 
                                                                            AllowUpdateDefault="Yes" BorderCollapseDefault="Separate" 
                                                                            HeaderClickActionDefault="SortMulti" Name="UltraWebGrid1" 
                                                                            RowHeightDefault="20px" RowSelectorsDefault="No" 
                                                                            SelectTypeRowDefault="Extended" StationaryMargins="Header" 
                                                                            StationaryMarginsOutlookGroupBy="True" TableLayout="Fixed" Version="4.00" 
                                                                            ViewType="OutlookGroupBy">
                                                                            <FrameStyle BackColor="Window" BorderColor="InactiveCaption" 
                                                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Microsoft Sans Serif" 
                                                                                Font-Size="8.25pt" Height="200px" Width="325px">
                                                                            </FrameStyle>
                                                                            <Pager MinimumPagesForDisplay="2">
                                                                                <PagerStyle BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                                                                                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                                                    WidthTop="1px" />
                                                                                </PagerStyle>
                                                                            </Pager>
                                                                            <EditCellStyleDefault BorderStyle="None" BorderWidth="0px">
                                                                            </EditCellStyleDefault>
                                                                            <FooterStyleDefault BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                                                                                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                                                    WidthTop="1px" />
                                                                            </FooterStyleDefault>
                                                                            <HeaderStyleDefault BackColor="LightGray" BorderStyle="Solid" 
                                                                                HorizontalAlign="Left">
                                                                                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                                                    WidthTop="1px" />
                                                                            </HeaderStyleDefault>
                                                                            <RowStyleDefault BackColor="Window" BorderColor="Silver" BorderStyle="Solid" 
                                                                                BorderWidth="1px" Font-Names="Microsoft Sans Serif" Font-Size="8.25pt">
                                                                                <Padding Left="3px" />
                                                                                <BorderDetails ColorLeft="Window" ColorTop="Window" />
                                                                            </RowStyleDefault>
                                                                            <GroupByRowStyleDefault BackColor="Control" BorderColor="Window">
                                                                            </GroupByRowStyleDefault>
                                                                            <GroupByBox>
                                                                                <BoxStyle BackColor="ActiveBorder" BorderColor="Window">
                                                                                </BoxStyle>
                                                                            </GroupByBox>
                                                                            <AddNewBox Hidden="False">
                                                                                <BoxStyle BackColor="Window" BorderColor="InactiveCaption" BorderStyle="Solid" 
                                                                                    BorderWidth="1px">
                                                                                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                                                        WidthTop="1px" />
                                                                                </BoxStyle>
                                                                            </AddNewBox>
                                                                            <ActivationObject BorderColor="" BorderWidth="">
                                                                            </ActivationObject>
                                                                            <FilterOptionsDefault>
                                                                                <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" 
                                                                                    BorderWidth="1px" CustomRules="overflow:auto;" 
                                                                                    Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px" Height="300px" 
                                                                                    Width="200px">
                                                                                    <Padding Left="2px" />
                                                                                </FilterDropDownStyle>
                                                                                <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                                                                                </FilterHighlightRowStyle>
                                                                                <FilterOperandDropDownStyle BackColor="White" BorderColor="Silver" 
                                                                                    BorderStyle="Solid" BorderWidth="1px" CustomRules="overflow:auto;" 
                                                                                    Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px">
                                                                                    <Padding Left="2px" />
                                                                                </FilterOperandDropDownStyle>
                                                                            </FilterOptionsDefault>
                                                                        </DisplayLayout>
                                                                    </igtbl:UltraWebGrid>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Label ID="Label16" runat="server" CssClass="Generallabel" 
                                                                        Text="Voucher # :"></asp:Label>
                                                                    &nbsp;
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtVoucherNo" runat="server" Enabled="false" ReadOnly="true" 
                                                                        Width="250px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Label ID="Label9" runat="server" CssClass="Generallabel" Text="Source :"></asp:Label>
                                                                    &nbsp;
                                                                </td>
                                                                <td align="left">
                                                                    <asp:DropDownList ID="cmbSource"  onclick ="" runat="server" Width="255px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" >
                                                                    <asp:Label ID="Label15" runat="server" CssClass="Generallabel" 
                                                                        Text="Destination :"></asp:Label>&nbsp;
                                                                </td>
                                                                <td align="left">
                                                                    <asp:DropDownList ID="cmbDestination"  runat="server" Width="255px" 
                                                                        AutoPostBack="True" onclick ="txtSeatNo_onblur();" >
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" >
                                                                    <asp:Label ID="Label2" runat="server" CssClass="Generallabel" 
                                                                        Text="Ticket Sr. # :" Font-Bold="False"></asp:Label>&nbsp;
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtTicketNo" Enabled=false runat="server" Width="250px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" >
                                                                    <asp:Label ID="Label17" runat="server" CssClass="Generallabel" 
                                                                        Text="Contact # :" Font-Bold="True"></asp:Label>&nbsp;<asp:Button 
                                                                        ID="btnValidateBooking" runat="server"
                                                                            Text="Validate Booking" CssClass="ButtonStyle" Height="20px" 
                                                                        Width="137px" />
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtContactNo" runat="server" Width="250px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            
                                                            <tr>
                                                                <td align="right" >
                                                                    <asp:Label ID="Label11" runat="server" CssClass="Generallabel" 
                                                                        Text="Passenger Name :" Font-Bold="True"></asp:Label>&nbsp;
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtPassengerName"  onblur="txtSeatNo_onblur();" runat="server" Width="250px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" >
                                                                    <asp:Label ID="Label31" runat="server" CssClass="Generallabel" 
                                                                        Text="CNIC # :" Font-Bold="True"></asp:Label>&nbsp;
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtCNIC" runat="server" Width="250px"></asp:TextBox>
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                               <td align="right" >
                                                                    <asp:Label ID="Label13" runat="server" CssClass="Generallabel" Text="Fare :" 
                                                                        Font-Bold="True"></asp:Label>&nbsp;
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtFare" Enabled =false runat="server" Width="100px" ReadOnly="True"></asp:TextBox>
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    <asp:Label ID="Label27" runat="server" CssClass="Generallabel" Font-Bold="True" 
                                                                        Text="Total :"></asp:Label>
                                                                    <asp:TextBox ID="txtTotal" Enabled =false runat="server" Width="100px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" >
                                                                    <asp:Label ID="Label18" runat="server" CssClass="Generallabel" 
                                                                        Text="Cash :" Font-Bold="True"></asp:Label>&nbsp;
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtAmount" runat="server" Width="100px"></asp:TextBox>
                                                                &nbsp;&nbsp;
                                                                    <asp:Label ID="Label20" runat="server" CssClass="Generallabel" 
                                                                        Text="Balance :" Font-Bold="True"></asp:Label>
                                                                    <asp:TextBox ID="txtBalance" Enabled =false runat="server" Width="100px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr id="trDeduction" runat="server">
                                                                <td align="right"  >
                                                                    <asp:Label ID="Label19" runat="server" CssClass="Generallabel" Text="Deduction :" Font-Bold="True"></asp:Label>&nbsp;
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtDeduction"  runat="server" Width="100px"></asp:TextBox>
                                                                </td>
                                                            </tr>                                                            
                                                            <tr>
                                                                <td align="right" >
                                                                    <asp:Label ID="Label8" runat="server" CssClass="Generallabel" Text="Seat # :"></asp:Label>&nbsp;
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtSeatNo" runat="server" Width="250px" 
                                                                        Font-Bold="True" ForeColor="Red"></asp:TextBox>
                                                                    <asp:Button ID="btnUndo" runat="server" CssClass="ButtonStyle" Height="21px" 
                                                                        Text="Undo" Visible="False" Width="42px" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" >
                                                                    <asp:Label ID="Label1" runat="server" CssClass="Generallabel" 
                                                                        Text="Vehicle No. :"></asp:Label>&nbsp;
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtVehicleNo" Visible=false runat="server" ReadOnly="True" Width="150px">
                                                                    </asp:TextBox>
                                                                   

 <div class="demo">
<div class="ui-widget">
<asp:DropDownList ID="cmbVehicle" runat="server"></asp:DropDownList>
</div>
</div><!-- End demo -->
                                                                </td>
                                                            </tr>
                                                            
                                                            <tr>
                                                                <td align="right" >
                                                                    <asp:Label ID="Label5" runat="server" Text="Departure Date :" CssClass="Generallabel"></asp:Label>&nbsp;
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtDepartureDate" runat="server" ReadOnly="True" Width="150px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right"  >
                                                                    <asp:Label ID="Label6" runat="server" Text="Departure Time :" CssClass="Generallabel"></asp:Label>&nbsp;
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtDepartureTime" runat="server" ReadOnly="True" Width="150px"></asp:TextBox>
                                                                     <input id="bkDate" type="hidden" runat="server" value=''/>
                                                                     <input id="hidTerminal" type="hidden" runat="server" value=''/>                                                                     
                                                                     <input id="Desct" type="hidden" runat="server" value=''/>
                                                                     <input id="CloseNo" type="hidden" runat="server" value=''/>
                                                                     <input id="CloseSMS" type="hidden" runat="server" value=''/>
                                                                     <input id="hidMode" type="hidden" runat="server" value=''/>
                                                                     <input id="hidTotal" type="hidden" runat="server" value=''/>                                                                     
                                                                    <input id="hidSource" type="hidden" runat="server" value=''/>
                                                                    <input id="hidRoute" type="hidden" runat="server" value=''/>
                                                                    <input id="hidPrint" type="hidden" runat="server" value=''/>
                                                                    <input id="hidSeatNo" type="hidden" runat="server" value=''/>
                                                                    <input id="hidTSID" type="hidden" runat="server" value='0'/>
                                                                    <input id="hidSMSDataPort" type="hidden" runat="server" value='40'/>                                                                    
                                                                    <input id="BookingSMS" type="hidden" runat="server" value=''/>                                                                    
                                                                    <input id="hidSMSData" type="hidden" runat="server" value=''/>                                                                    
                                                                    <input id="VoucherStatus" type="hidden" runat="server" value='1'/>                                                                    
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Label ID="Label28" runat="server" CssClass="Generallabel" Text="Actual Departure Time :"></asp:Label>&nbsp;
                                                                    </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtActualDepartureTime" runat="server" Width="150px"></asp:TextBox> &nbsp;<asp:Button ID="btnSaveDep" runat="server" CssClass="ButtonStyle" Text="Save" 
                                                                        Visible="True" Width="50px" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    &nbsp;</td>
                                                                <td align="left">
                                                                    <asp:Button ID="btnCancelTicket" runat="server" CssClass="ButtonStyle" 
                                                                        Text="Cancel Ticket" Visible="False" Width="133px" />
                                                                    &nbsp;<asp:Button ID="btnSkip" runat="server" CssClass="ButtonStyle" Text="Skip" 
                                                                        Visible="False" Width="50px" />
                                                                    <asp:Button ID="btnSave"  runat="server" CssClass="ButtonStyle" Text="Print" 
                                                                        Width="81px" />
                                                                    <input id="hdnPrint" runat="server" value="0" type="hidden" />
                                                                    </td>
                                                            </tr>
                                                            </table><br>
                                                            <table cellspacing="2" cellpadding="2" class="TableBorder" width="50%">
                                                            <tr class="HeaderStyle">
                                                    <td align="center" valign="middle">Previous Activity</td>
                                                </tr>    
                                                            <tr>
                                                                <td align="left"  id="tdSeatNo" runat="server" class="Generallabel"></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" id="tdPassenger" runat="server" class="Generallabel"></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" id="tdExtraComm" runat="server" class="Generallabel"></td>
                                                            </tr>
                                                            </table><br>
                                                            
                                                         
                                                    </td>
                                                    <td align="center" valign="top" width="40%" visible="False">
                                                        <table width="280px" align="center" border="0" class="TableBorder" cellspacing="0">
                                                            <tr class="HeaderStyle">
                                                                <td align="center" valign="middle">
                                                                    <asp:Label ID="lblTitles" runat="server" Text="Select Seat"></asp:Label></td>
                                                            </tr> 
                                                            <tr class="HeaderStyle">
                                                              <td>
                                                               <div style="float:left;width:150px" >Left Hand </div>
                                                               <div style="float:left" >Right Hand </div>
                                                               </td>
                                                            </tr>                                               
                                                            <tr>
                                                                <td align="right" valign="middle">
                                                                    <table id="tblTickets" runat="server" align="center" cellpadding="0" cellspacing="0" style="width:280px">
                                                                        <tr>
                                                                            <td title="Available" SeatStatus="Available" class="TicketAvailable" id="cell_0_0_1"  onclick="OperateOnSeat(this, 1, 2);">&nbsp;</td>
                                                                            <td class="TicketConfirmed" id="cell_0_0_2"  onclick="OperateOnSeat(this, 2, 2);">&nbsp;</td>
                                                                            <td class="TicketAvailable" id="cell_0_0_3"  onclick="OperateOnSeat(this, 3, 2);">&nbsp;</td>
                                                                            <td class="TicketAvailable" id="cell_0_0_4"  onclick="OperateOnSeat(this, 4, 2);">&nbsp;</td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:CheckBox ID="chkOnline" runat="server" CssClass="Generallabel"
                                                            Font-Bold="True" ForeColor="Red" Text="ONLINE" Checked="True" />
                                                        &nbsp;&nbsp;<asp:LinkButton ID="lnkMapping" runat="server" CssClass="linkButton" style="display:none;">Online Mapping</asp:LinkButton>&nbsp;&nbsp;
                                                        <asp:Button ID="btnRefresh" runat="server" CssClass="ButtonStyle" Text="Refresh" 
                                                            Width="67px" Height="23px" /><br>
                                                        <asp:Label ID="lblErr" runat="server" CssClass="Errorlabel"></asp:Label>
                                                        &nbsp;<asp:LinkButton ID="lnkCreateOnline"  OnClientClick="javascript:displaynone();" runat="server" CssClass="linkButton">Create 
                                                        Online</asp:LinkButton><br>
                                                        <table cellpadding="0" cellspacing="0" width="250px">
                                                            <tr>
                                                                <td class="Generallabel" width="150">
                                                                    Booked</td>
                                                                <td class="TicketBooked" height="22px">
                                                                    <asp:Label ID="lblB" runat="server" CssClass="Generallabel" Text=" "></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="Generallabel" style="font-weight:bold;">
                                                                    Soled</td>
                                                                <td class="TicketConfirmed">
                                                                    <asp:Label ID="lblC" runat="server" CssClass="Generallabel" Text=" "></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="Generallabel">
                                                                    Available</td>
                                                                <td class="TicketAvailable" height="22px">
                                                                    <asp:Label ID="lblA" runat="server" CssClass="Generallabel" Text=" "></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>                                                       
                                                        
                                                    </td>
                                                </tr>                                                
                                                <tr>
                                                    <td align="center" valign="top" width="60%">
                                                        &nbsp;</td>
                                                    <td align="right" valign="middle">
                                                        &nbsp;</td>
                                                </tr>                                                
                                                <tr>
                                                    <td align="center" valign="top">
                                                        
                                                    </td>
                                                    <td align="right" valign="middle">
                                                        &nbsp;</td>
                                                </tr>                                                
                                               <tr>
                                                    <td align="right" valign="middle" height="5px" colspan="2">                                                        
                                                        <asp:LinkButton ID="lnkMakeAvailable" runat="server"></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkReserve" runat="server"></asp:LinkButton>
                                                        <asp:LinkButton ID="lnkSeatDetail" runat="server"></asp:LinkButton>
                                                        <input 
                                                            ID="btnVoucherReport0" runat="server" class="ButtonStyle" 
                                                            onclick="btnVoucherReport_onclick();" type="button" value="Voucher Report" />                                                        
                                                        <input type="button" id="btnVoucherReportNull" runat="server" value="Voucher Null" class="ButtonStyle" onclick="btnVoucherReportNull_onclick();" />                                                                                                                
                                                        <asp:Button ID="btnCloseVoucher" runat="server" CssClass="ButtonStyle" 
                                                            Text="Close Voucher" Width="115px" />
                                                        &nbsp;<input ID="btnClose" class="ButtonStyle" type="button" value="Close" onclick="window.location='TicketingSchedule.aspx'" />
                                                      
                                                    </td>
                                                </tr>  
                                              </table>
                                              
                                              <table ID="tblDeductions"  style="left: 235px;width: 50%;" runat="server" cellpadding="2" cellspacing="0" >
                                                                <tr>
                                                                    <td align="left" valign="middle">
                                                                        <table cellpadding="2" cellspacing="0" style="width: 100%" class="TableBorder">
                                                                            <tr class="HeaderStyle">
                                                                                <td align="center" colspan="2">Voucher Deductions</td>
                                                                            </tr>
                                                                             <tr>
                                                                                <td align="right" width="200px">
                                                                                    <asp:Label ID="Label32" runat="server" CssClass="Generallabel" 
                                                                                        Text="Commission %:"></asp:Label>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtComPer" onblur="updateTotal();" runat="server" CssClass="txtbox" 
                                                                                        MaxLength="40" Text="10" Width="150px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>

                                                                             <tr>
                                                                                <td align="right" width="200px">
                                                                                    <asp:Label ID="Label29" runat="server" CssClass="Generallabel" 
                                                                                        Text="Commission:"></asp:Label>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtcommission" runat="server" CssClass="txtbox" 
                                                                                        MaxLength="40" Width="150px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" width="200px">
                                                                                    <asp:Label ID="Label3" runat="server" CssClass="Generallabel" 
                                                                                        Text="Hostess Salary :"></asp:Label>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtHostessSalary" runat="server" CssClass="txtbox" 
                                                                                        MaxLength="40" Width="150px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <asp:Label ID="Label4" runat="server" CssClass="Generallabel" 
                                                                                        Text="Driver Salary :"></asp:Label>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtDriverSalary" runat="server" CssClass="txtbox" 
                                                                                        MaxLength="40" Width="150px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <asp:Label ID="Label7" runat="server" CssClass="Generallabel" 
                                                                                        Text="Gaurd Salary :"></asp:Label>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtGuardSalary" runat="server" CssClass="txtbox" 
                                                                                        MaxLength="40" Width="150px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <asp:Label ID="Label14" runat="server" CssClass="Generallabel" 
                                                                                        Text="Service Charges :"></asp:Label>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtServiceCharges" runat="server" CssClass="txtbox" 
                                                                                        MaxLength="40" Width="150px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <asp:Label ID="Label21" runat="server" CssClass="Generallabel" 
                                                                                        Text="Cleaning Charges :"></asp:Label>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtCleaningCharges" runat="server" CssClass="txtbox" 
                                                                                        MaxLength="40" Width="150px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <asp:Label ID="Label22" runat="server" CssClass="Generallabel" 
                                                                                        Text="Hook Charges :"></asp:Label>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtHookCharges" runat="server" CssClass="txtbox" 
                                                                                        MaxLength="40" Width="150px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <asp:Label ID="Label23" runat="server" CssClass="Generallabel" 
                                                                                        Text="Bus Charges :"></asp:Label>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtBusCharges" runat="server" CssClass="txtbox" 
                                                                                        MaxLength="40" Width="150px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <asp:Label ID="Label24" runat="server" CssClass="Generallabel" 
                                                                                        Text="Refreshment :"></asp:Label>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtRefreshment" runat="server" CssClass="txtbox" 
                                                                                        MaxLength="40" Width="150px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <asp:Label ID="Label25" runat="server" CssClass="Generallabel" 
                                                                                        Text="Toll GBS :"></asp:Label>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtToll" runat="server" CssClass="txtbox" 
                                                                                        MaxLength="40" Width="150px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <asp:Label ID="Label26" runat="server" CssClass="Generallabel" 
                                                                                        Text="Total Deduction :"></asp:Label>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtTotalDeductions" runat="server" CssClass="txtbox" 
                                                                                        MaxLength="40" Width="150px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
		<tr>
                                                                                <td align="right">
                                                                                    <asp:Label ID="Label33" runat="server" CssClass="Generallabel" 
                                                                                        Text="Driver Name :"></asp:Label>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtDriverName" runat="server" CssClass="txtbox" 
                                                                                        MaxLength="40" Width="150px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <asp:Label ID="Label34" runat="server" CssClass="Generallabel" 
                                                                                        Text="Hostress Name :"></asp:Label>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtHostessName" runat="server" CssClass="txtbox" 
                                                                                        MaxLength="40" Width="150px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <asp:Label ID="Label10" runat="server" CssClass="Generallabel" 
                                                                                        Text="Guard Name :"></asp:Label>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtGuard" runat="server" CssClass="txtbox" 
                                                                                        MaxLength="40" Width="150px"></asp:TextBox>
                                                                                </td>
                                                                            </tr>


                                                                            <tr>
                                                                                <td align="right">
                                                                                    &nbsp;</td>
                                                                                <td ID="tdOKCancel" runat="server">
                                                                                    <asp:Button ID="btnOK" runat="server" CssClass="ButtonStyle" Text="OK" 
                                                                                        Width="81px" OnClientClick="javascript:return validateclose();"  />
                                                                                    &nbsp;<input ID="btnCancel" class="ButtonStyle" runat="server" 
                                                                                        onclick="document.getElementById('tblDeductions').style.display='none';" 
                                                                                        type="button" value="Cancel" /></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                        </table>
                                              
                                              </igmisc:WebAsyncRefreshPanel>
                   
    </form>
</body>
</html>
