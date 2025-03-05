<%@ Page Language="vb" EnableViewState="true" AutoEventWireup="false" CodeBehind="Ticketing.aspx.vb" Inherits="FMovers.Ticketing.UI.Ticketing" %>

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
    <link rel="stylesheet" href="styles/jquery.ui.all.css" />
	<link rel="stylesheet" href="styles/demos.css" />
    <link rel="stylesheet" href="styles/jquery.ui.all.css" />
	<script type="text/javascript" src="script/jquery-1.5.1.js"></script>
	<script type="text/javascript" src="script/jquery.ui.core.js"></script>
	<script type="text/javascript" src="script/jquery.ui.widget.js"></script>
	<script type="text/javascript" src="script/jquery.ui.button.js"></script>
	<script type="text/javascript" src="script/jquery.ui.position.js"></script>
	<script type="text/javascript" src="script/jquery.ui.autocomplete.js"></script>
	<link rel="stylesheet" href="styles/demos.css" />
<style type="text/css">
    
    #tblTickets tr td 
    {
    	
    	max-width:25px !important;
    	height:37px;
        
    	
    	}
    .ui-button {
        margin-left: -2px;
        height: 30px;
        float: right;
    }
      .voucherClosingColor{
        background-color:orangered;
        padding: 10px 10px;
        color:white;
        font-weight:bold;
        border:none;
        cursor:pointer;
    }
    .ui-button-icon-only .ui-button-text {
        padding: 0.35em;
    }

    .ui-autocomplete-input {
        margin: 0;
        padding: 0.48em 0 0.47em 0.45em;
    }

    .route.text {
        width: 250px;
    }

    .route input[type="text"] {
        width: 250px !important;
    }

    .ui-button {
        margin-left: -1px;
    }

    .ui-button-icon-only .ui-button-text {
        padding: 0.35em;
    }

    .ui-autocomplete-input {
        margin: 0;
        padding: 0.48em 0 0.47em 0.45em;
    }

    .style1 {
        width: 52%;
    }

    .style3 {
        height: 29px;
    }

    #lblupper{
        padding-left:20%;
    }

      #lblSleeperbusUP1{
        padding-left:46%;
    }

    #lblSleeperbusUP2{
       padding-left:45px;
    }
    #lblSleeperbusUP3{
       padding-left:45px;
    }

       #lblSleeperbusDown1{
        padding-left:36%;
    }
       #lblSleeperbusDown3{
        padding-left:6%;
    }

      #lblSleeperbusDown2{
        padding-left:40px;
    }

    #Label74{

        padding-left:25%;

    }

      #Label75{

        padding-left:13%;

    }
    #lblSleeperTitles{
        padding-top:15%;
        padding-left:10%;
    }
</style>
    <script type="text/javascript" >
        $(document).ready(function () {


            $("#Loalitybox").hide();
        

        });

    </script>
<script type="text/javascript" >
  
 

      function getCustomerInfo( ) {
                if (document.getElementById("txtContactNo").value != "" ) {
                    $.get("getCustomerInfo.aspx?type=con&ContactNo=" + document.getElementById("txtContactNo").value, function(data) {
                        var res = data.split("|");
                        document.getElementById("lblDouple").innerHTML ="";
                        //alert(res.length );
                         if (res[2] != 'undefined') { 
                        if (res.length == 3)
                                 document.getElementById("lblDouple").innerHTML  = res[2];
                                   }
                        if (res[0] != 'undefined') {
                            if (res[0] != '')
                                document.getElementById("txtPassengerName").value = res[0];
                            { }
                            if (document.getElementById("txtCNIC2").value == '') {
                                if (res[1] != 'undefined') {
                                    document.getElementById("txtCNIC2").value = res[1];
                                }
                               

                            }
                        }
                    });
                }
            
            
            //__doPostBack('lnkCustomerInfo', '');
            //document.getElementById("txtCNIC2").focus();
        }
            
    function validateCNICWatchList() {

        if (document.getElementById("txtCNIC2").value != "") {
            $.get("getCustomerWatchList.aspx?type=CNIC&ContactNo=" + document.getElementById("txtCNIC2").value, function(data) {
                var res = data.split("|");
                if (res[0] != 'undefined') {
                    if (res[0] != '')
                        document.getElementById("txtPassengerName").value = res[0];
                    { }
                    if (document.getElementById("txtCNIC2").value == '') {
                        document.getElementById("txtCNIC2").value = res[1];
                    }
                }
            });
        }


        //__doPostBack('lnkCustomerInfo', '');
        //document.getElementById("txtCNIC2").focus();


    }


    function validatedrop() {

        
        var restults = confirm("Are you sure you want to drop this time ?");
        
        return restults;
    }
    function validateCustomer() {
        alert("i am here");
        var custNumber = document.getElementById("txtCustomerNumber").value;
        if (custNumber != '') {

            alert(custNumber);

        }

    }

    function warpTicketing_Error(oPanel, oEvent, flags) {
        if (flags == 1) {
            alert("Exception on server. Before full postback.");
        }
    }

    function warpTicketing_InitializePanel(oPanel) {
        oPanel.getProgressIndicator().setImageUrl("images/ajax-loader.gif");
        ig_shared.getCBManager()._timeLimit = 70000;

    }

</script>

<script type="text/javascript">
    function changecolors(cityName, color) {
        cityName = cityName.replace("_", " ");
        //var cols = document.getElementById('tblTickets').getElementsByTagName('td'), colslen = cols.length, i = -1;
        //while (++i < colslen) {
        // alert(cols[i].getAttribute('title'));
        //  alert(cols[i].innerHTML);
        // }

        $("#tblTickets tr td").each(function() {
            var n = $(this).attr('title').indexOf(cityName);
            if (n > 0) {
                $(this).attr('style', 'background-color:#' + color + ' !important');

            }
            //  if ($(this).attr('title') == "Exit Row Seat") {
            //    var count = $(this).attr("seq");
            //   $(this).addClass('exitRow');
            //  }
        });
    }
</script>
   


<script type="text/javascript" >


    var UserName = "<% = UserName %>";
    var CurrentUserID = "<% = CurrentUserID %>";
    var TerminalId = "<% = TerminalId %>";




    function serverNotOnline() {
        alert("Server is not online please contact to IT team");
    }

    function btnVoucherReport_onclick(showval) {
        if (document.getElementById("VoucherStatus").value == "1") {
            window.open('Reports/PrintVoucherReport.aspx?ShowALl=' + showval + '&TSID=' + '<% =hidTSID.Value %>&OnlineTSID=' + '<% =hndOnlineTSNo.Value %>&status=1');
        }
        if (document.getElementById("VoucherStatus").value == "2") {
            window.open('Reports/PrintVoucherReport.aspx?ShowALl=' + showval + '&TSID=' + '<% =hidTSID.Value %>&OnlineTSID=' + '<% =hndOnlineTSNo.Value %>&status=2');
        }
        return false;
    }

    function btnCancelReport_onclick() {
            window.open('Reports/CancelReport.aspx?TSID2=' + '<% =hidTSID.Value %>&TSID=' + '<% =hndOnlineTSNo.Value %>&status=1');
        return false;
    }


    function btnVoucherReportAll_onclick(showval) {
            window.open('Reports/PrintVoucherReportAllNew.aspx?ShowALl=' + showval + '&TSID=' + '<% =hidTSID.Value %>&OnlineTSID=' + '<% =hndOnlineTSNo.Value %>&status=1');
            return false;
    }

    function btnVoucherReportSmart_onclick(showval) {
        if (document.getElementById("VoucherStatus").value == "1") {
            window.open('Reports/PrintVoucherSmartReport.aspx?ShowALl=' + showval + '&TSID=' + '<% =hidTSID.Value %>&OnlineTSID=' + '<% =hndOnlineTSNo.Value %>&status=1');
        }
        if (document.getElementById("VoucherStatus").value == "2") {
            window.open('Reports/PrintVoucherSmartReport.aspx?ShowALl=' + showval + '&TSID=' + '<% =hidTSID.Value %>&OnlineTSID=' + '<% =hndOnlineTSNo.Value %>&status=2');
        }
        return false;
    }

  
    function rdodata() {
         
       // var rdoval = document.getElementsByName("rdoIDNumber").value;  Loality

        var rdo = $('#<%=rdoIDNumber.ClientID %> input[type=radio]:checked').val();
        if (rdo == "Passport") {

            document.getElementById('Label2912556').innerHTML = "Passport No:";
            document.getElementsByName('txtCNIC2')[0].placeholder = 'Please Enter Passport No';
            $("#Loalitybox").hide();
        }

        else if (rdo == "CNIC") {
            document.getElementById('Label2912556').innerHTML = "CNIC # :";
            document.getElementsByName('txtCNIC2')[0].placeholder = 'Please Enter CNIC';
            $("#Loalitybox").hide();

        }
        else if (rdo == "Bayform") {
            document.getElementById('Label2912556').innerHTML = "BayForm # :";
            document.getElementsByName('txtCNIC2')[0].placeholder = 'Please Enter Bay Form';
            $("#Loalitybox").hide();

        }

        else {

            
            $("#Loalitybox").show();

        }
       
      

    }



    function btnVoucherReportCNIC_onclick() {
        window.open('Reports/CNIC.aspx?TSID=' + '<% =hidTSID.Value %>&status=2');
        return false;
    }


    function btnVoucherReportSmartCNIC_onclick() {
        window.open('Reports/CNICSmartReport.aspx?TSID=' + '<% =hidTSID.Value %>&status=2');
        return false;
    }

    function btnVoucherReportValueAddedServices_onclick() {
        window.open('Reports/ValueAddedServiceReport.aspx?TSID=' + '<% =hidTSID.Value %>&status=2');
           return false;
       }


    function abc() {
        document.getElementById("ProgressBarwapper").style.display = 'none';
        document.getElementById("divTransit").style.display = 'none';
        return false;
    }

    function reLoadWidow(VoucherId) {
        window.location = "Ticketing.aspx?mode=1&TSID=" + VoucherId;


    }


    function confirmTransit() {

        var rtn = window.confirm("Are are you sure you want to transit ?");
        if (rtn) {
            document.getElementById("ProgressBarwapper").style.display = 'block';
            document.getElementById("divTransit").style.display = 'block';

        }
        return false;

    }
    function CancelTransit() {

        document.getElementById("ProgressBarwapper").style.display = 'none';
        document.getElementById("divTransit").style.display = 'none';
        return false;



    }

    function confirmCancel() {
        document.getElementById("ProgressBarwapper").style.display = 'none';
    }


    function btnVoucherReportNull_onclick() {

        if (document.getElementById("VoucherStatus").value == "1") {
            window.open('Reports/PrintVoucherReportNull.aspx?TSID=' + '<% =hidTSID.Value %>&status=1');
        }
        if (document.getElementById("VoucherStatus").value == "2") {
            window.open('Reports/PrintVoucherReportNull.aspx?TSID=' + '<% =hidTSID.Value %>&status=2');
        }
        return false;
    }

    function chkOnline_onclick() {

        if (document.getElementById("chkOnline").checked == true)
            document.getElementById("lnkMapping").style.display = "";
        else
            document.getElementById("lnkMapping").style.display = "none";
    }

    function OperateOnSeat(cell, SeatNo, OperationType, status, SeatUserId) {
        try {


            document.getElementById('tblTickets').setAttribute('disabled', 'disabled');
            if (document.getElementById("tblTickets").disabled == "undefined") {
                //document.getElementById("tblTickets").disabled = false;
                //
            }
            //ProgressBar
            //document.getElementById("ProgressBarwapper").style.display = "block";



            if (1 == 1) {
                if (OperationType == 2) {

                    if (document.getElementById("hidMode").value == "2") {
                        if (status != 4) {
                            if (document.getElementById("txtSeatNo").value == "")
                                document.getElementById("txtSeatNo").value = SeatNo;
                            else
                                document.getElementById("txtSeatNo").value = document.getElementById("txtSeatNo").value + "," + SeatNo;
                            document.getElementById("hidSeatNo").value = SeatNo;
                        }
                        if (status == 1) {
                            __doPostBack('lnkReserve', '')

                        }
                        else if (status == 2) {
                            __doPostBack('lnkMakeAvailable', '')
                        }
                        else if (status == 3) {
                            document.getElementById("txtSeatNo").value = SeatNo;
                            __doPostBack('lnkSeatDetail', '');
                            //document.getElementById("btnCancelTicket").value = "Cancel Booking"
                        }
                    }
                    else {

                        //alert("i am here");
                        document.getElementById("tblTickets").disabled = false;
                        if (document.getElementById("txtSeatNo").value == "")
                            document.getElementById("txtSeatNo").value = SeatNo;
                        else
                            document.getElementById("txtSeatNo").value = document.getElementById("txtSeatNo").value + "," + SeatNo;
                        document.getElementById("hidSeatNo").value = SeatNo;

                        if (status == 2) {
                            __doPostBack('lnkMakeAvailable', '')
                        }
                        else if (status == 3) {
                            document.getElementById("txtSeatNo").value = SeatNo;
                            __doPostBack('lnkSeatDetail', '');
                            //document.getElementById("btnCancelTicket").value = "Cancel Booking"
                            //document.getElementById("btnSave").style.display = ""
                        }
                        else if (status == 4) {
                            if ((SeatUserId == CurrentUserID) || (UserName == "admin")) {
                                document.getElementById("txtSeatNo").value = SeatNo;
                                __doPostBack('lnkSeatDetail', '');
                            }
                            //// else if

                            document.getElementById("txtSeatNo").value = SeatNo;
                            __doPostBack('lnkSeatDetail', '');
                            //alert(document.getElementById("btnCancelTicket"));
                            //document.getElementById("btnCancelTicket").value = "Cancel Ticket"
                        }
                        else if (status == 1) {
                            __doPostBack('lnkReserve', '')
                        }
                        document.getElementById("tblTickets").disabled = true;
                    }
                }
                else if (OperationType == 1) {
                    //for single click
                }
                //document.getElementById("ProgressBarwapper").style.display = "none";

            }

        }
        catch (e) {
            alert("Can't connect to server:\n" + e.toString());
            //  document.getElementById("ProgressBarwapper").style.display = "none";

        }
    }

    function txtSeatNo_onblur() {
//        var Seats = document.getElementById("txtSeatNo").value.split(",");
//        var Fare = parseInt(document.getElementById("txtFare").value);

//        document.getElementById("txtTotal").value = Seats.length * Fare;


    }

    function validateclose() {

        var DriverName = document.getElementById("txtDriverName").value;
        var HostessName = document.getElementById("txtHostessName").value;
        var VechileNo = document.getElementById("hndVechileNo").value;

        if (VechileNo == "81" || VechileNo == "") {

            alert("Please select vechile and press save button . ");
            return false;

        }

        if (DriverName.length == "" || DriverName.length < 5) {
            alert("Please enter driver name. ");
            return false;
        }

        return true;
    }

    function txtAmount_onblur() {
        var Fare = parseInt(document.getElementById("txtFare").value);
        var Total = parseInt(document.getElementById("txtTotal").value);
        var Amount = parseInt(document.getElementById("txtAmount").value);

        if (Total == NaN) {
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
        if (document.getElementById("txtCNIC2").value == "") {
            alert("Please Specify the Passenger CNIC!");
            document.getElementById("txtCNIC2").focus();
            return false;
        }

        if (document.getElementById("txtCNIC2").value.length != 13) {
            alert("Please enter valid CNIC number.");
            document.getElementById("txtCNIC2").focus();
            return false;
        }

        if (document.getElementById("txtCNIC2").value == "0000000000000") {
            alert("Please Specify the Passenger CNIC!");
            document.getElementById("txtCNIC2").focus();
            return false;
        }

        if (document.getElementById("txtCNIC2").value == "1111111111111") {
            alert("Please Specify the Passenger CNIC!");
            document.getElementById("txtCNIC2").focus();
            return false;
        }

        if (document.getElementById("txtCNIC2").value == "2222222222222") {
            alert("Please Specify the Passenger CNIC!");
            document.getElementById("txtCNIC2").focus();
            return false;
        }
        if (document.getElementById("txtCNIC2").value == "3333333333333") {
            alert("Please Specify the Passenger CNIC!");
            document.getElementById("txtCNIC2").focus();
            return false;
        }

        
        if (document.getElementById("txtSeatNo").value == "") {
            alert("Please Specify the Seat #!");
            return false;
        }
        if (validateDiscount() == false) {
            alert("Please Enter Valid Discount Amount");
            return false;
        }
        return true;
    }

    


    function printWindows() {

        alert("Hello");

    }

    function print() {
        if (document.getElementById("hdnPrint").value == "1") {
            document.getElementById("hdnPrint").value = "0"
            var TicketNo = document.getElementById("txtTicketNo").value

            var PassengerName = document.getElementById("txtPassengerName").value
            var ContractNo = document.getElementById("txtContactNo").value

            var SeatNo = document.getElementById("txtSeatNo").value
            var Fare = document.getElementById("txtFare").value

            var Route = document.getElementById("hidRoute").value
            var DepartureDateTime = document.getElementById("txtDepartureDate").value + ' ' + document.getElementById("txtDepartureTime").value
            var VehicleNo = document.getElementById("txtVehicleNo").value
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
        var commFinal = (Totalss * txtComPer) / 100;


        document.getElementById("txtcommission").value = Math.round(commFinal);


        var HostessSalary = parseInt(document.getElementById("txtHostessSalary").value);
        var DriverSalary = parseInt(document.getElementById("txtDriverSalary").value);
        var GuardSalary = parseInt(document.getElementById("txtGuardSalary").value);
        var ServiceCharges = parseInt(document.getElementById("txtServiceCharges").value);
        var CleaningCharges = parseInt(document.getElementById("txtCleaningCharges").value);
        var HookCharges = parseInt(document.getElementById("txtHookCharges").value);
        var BusCharges = parseInt(document.getElementById("txtBusCharges").value);

        var Toll = parseInt(document.getElementById("txtToll").value);
        var Commission = parseInt(document.getElementById("txtcommission").value);

        var Refreshment = parseInt(document.getElementById("txtRefreshment").value);
        var Reward = parseInt(document.getElementById("txtReward").value);
        var Misc = parseInt(document.getElementById("txtMisc").value);
        var TerminalExpense = parseInt(document.getElementById("txtTerminalExpense").value);
        var PaidToDriver = parseInt(document.getElementById("txtPaidToDriver").value);

        if (isNaN(Commission))
            Commission = 0;

        if (isNaN(HostessSalary))
            HostessSalary = 0;

        if (isNaN(DriverSalary))
            DriverSalary = 0;

        if (isNaN(GuardSalary))
            GuardSalary = 0;


        if (isNaN(PaidToDriver))
            PaidToDriver = 0;

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

        if (isNaN(Reward))
            Reward = 0;

        if (isNaN(Misc))
            Misc = 0;

        if (isNaN(TerminalExpense))
            TerminalExpense = 0;

        var total = TerminalExpense + Misc + Reward + Commission + HostessSalary + DriverSalary + GuardSalary + ServiceCharges + CleaningCharges + HookCharges + BusCharges + Refreshment + Toll + PaidToDriver;
        document.getElementById("txtTotalDeductions").value = total;
    }

    function closeVoucher() {
//  var VechileNo = document.getElementById("cmbVehicle").value;

//        if (VechileNo == "81" || VechileNo == "0"|| VechileNo == "") {

//            alert("Please select vechile other then advance . ");
//            return false;

//        }
//        return true;


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


        if (document.getElementById("hidPrint").value == '1') {
            if (document.getElementById("hdnPrint").value == "1") {
                document.getElementById("hdnPrint").value = "0"
                //print();
            }

            document.getElementById("hidPrint").value = 0;
        }
    }

</script>
   
<script type="text/javascript">

    function calcComm() {

    }


    function getTriggerId() {
        // Get array of possible elements which
        // could trigger asynchronous request.
        // Note: that array may conain up to 3 values.
        // Those values represent ids related to async request.
        // That can be the id of clicked (or mouseup, or mouse down) html
        // element or the 1st parameter passed to __doPostBack.
        var ids = ig_shared.getCBManager().triggers;
        // The most probable id is located at index 0.
        var idOfTrigger = ids[0];
        if (!idOfTrigger)
            idOfTrigger = ids[1];
        if (!idOfTrigger)
            idOfTrigger = ids[2];
        return idOfTrigger;
    }
</script>
 
 
<script type="text/javascript" id="igClientScript">
<!--


    //	var message = "function disabled"; 
    //	function rtclickcheck(keyp){ if (navigator.appName == "Netscape" && keyp.which == 3){ 	alert(message); return false; } 
    //	if (navigator.appVersion.indexOf("MSIE") != -1 && event.button == 2) { 	alert(message); 	return false; } } 
    //	document.onmousedown = rtclickcheck;
    //        var Global_Counter = 0;


    function myGlobalAsyncSubmitListener(id) {
        // if async postback was triggered by Button1
        // then trigger full postback
        if (id.length > 6 && id.indexOf('Button1') == id.length - 7)
            return 'fullPostBack';
        // if async postback was triggered by Button2
        // then cancel submit and request to server
        if (id == 'Button2')//assume that Button2 is not in a NamingContainer
            return 'cancelSubmit';
        // if async postback was triggered by Button3
        // then cancel response (notify server about Button3 click)
        if (id == 'Button3')//assume that Button3 is not in a NamingContainer
            return 'cancelResponse';
    }
    //        ig_shared.addCBSubmitListener(myGlobalAsyncSubmitListener);
    function warpTicketing_RefreshResponse(oPanel, oEvent, id) {

        //           alert(id);
        var i = 0;

        i = i + 1;
        var idOfTrigger = getTriggerId();

        //            alert(idOfTrigger.length);
        //document.getElementById("ProgressBarwapper").innerHTML = "Please wait while loading ......" + i;

        //Add code to handle your event here.
    }

    function CalcualteTotalTime() {


    }

    function warpTicketing_RefreshRequest(oPanel, oEvent) {
        Global_Counter = 0;
        //alert("starts");
        //var t = setTimeout(function() { CalcualteTotalTime() }, 5);
        document.getElementById("ProgressBarwapper").style.display = 'block';


        //Add code to handle your event here.
    }

    function warpTicketing_RefreshComplete(oPanel, oEvent, id) {
        document.getElementById("ProgressBarwapper").style.display = 'none';
    }


    function displayKeyCode(evt) {
        var textBox = getObject('txtChar');

        var charCode = (evt.which) ? evt.which : event.keyCode


        if (charCode == 13) {

            var txtCNICActual = document.getElementById('txtCNIC2');
            var dummyEl = document.getElementById('txtCNICNew');
            var txtCNIC = document.getElementById('txtCNICNew');
            var txtPassengerName = document.getElementById('txtPassengerName');
            var txtContactNo = document.getElementById('txtContactNo');


            // check for focus
            var isFocused = (document.activeElement === dummyEl);
            if (isFocused == true) {


                var n = txtCNIC.value.length;

                if (n > 260) {

                    var res = txtCNIC.value.substring(80, 27);

                    res = res.substring(30, 13);
                    var res2 = res.substring(3, 17);

                    var searchResult = res2.search("╠");
                    if (searchResult > 0) {
                        alert(res);
                    }

                    txtCNIC.value = res2.trim();
                    txtCNICActual.value = "";
                    txtPassengerName.value = "";
                    txtContactNo.value = "";
                    txtCNICActual.value = txtCNIC.value;
                    getCNICInfo();
                    txtCNIC.value = "";



                }
                if (n == 26) {
                    var res = txtCNIC.value.substring(12, 26);
                    res = res.substring(13, 0);
                    txtCNIC.value = res.trim();
                    txtCNICActual.value = "";
                    txtPassengerName.value = "";
                    txtContactNo.value = "";
                    txtCNICActual.value = txtCNIC.value;
                    getCNICInfo();
                    txtCNIC.value = "";

                }

            }
            else {
                // alert("Page Event");
                if (validation()) {
                    __doPostBack('btnSave', '')
                }
                if (validateDiscount()) {
                    __doPostBack("btnSave", "")
                }
                if (codeVerification()) {
                    __doPostBack("btnSave", "")
                }
            }
        }
        //	return false;

    }
    //function displayKeyCode(evt)
    // {
    //     var textBox = getObject('txtChar');
    //     var txtCNIC = document.getElementById('txtCNIC');
    //	 var charCode = (evt.which) ? evt.which : event.keyCode
    //	
    //	 
    //	 if (charCode == 13) {
    //	     
    //	  // alert("Page Event");
    // 	   if (validation())
    // 	   {
    //         __doPostBack('btnSave', '')
    // 	      
    // 	   }
    //	 }
    ////	return false;

    // }


    function displayKeyCodeCNIC(evt) {
        //     var textBox = getObject('txtChar');
        //     var txtCNIC = document.getElementById('txtCNIC');
        var charCode = (evt.which) ? evt.which : event.keyCode
        //	
        //
        if (charCode == 13) {
            var n = txtCNIC.value.length;
            if (n > 260) {
                var res = txtCNIC.value.substring(44, 27);
                //alert(res);
                res = res.substring(14, 0);
                txtCNIC.value = res.trim();
            }
            if (n == 26) {
                var res = txtCNIC.value.substring(12, 26);
                res = res.substring(13, 0);
                txtCNIC.value = res.trim();
            }
        }
        //	  // alert("Page Event");
        ////	   if (validation())
        ////	   {
        ////	    //  __doPostBack('btnSave', '')
        ////	      
        ////	   }
        //	 }
        //	return false;

    }



    function loadvoucher() {
        //alert(id);
        __doPostBack('cmbVehicle_TextChanged', '')

    }

    function validateVehicle(obj) {

        var VechileNo = document.getElementById("cmbVehicle").value;

        if (VechileNo == "81" || VechileNo == "0"|| VechileNo == "") {

            alert("Please select vechile other then advance . ");
            return false;

        }

        return true;
    }
    function validateTime(obj) {




        var timeValue = document.getElementById("txtActualDepartureTime").value;
        var t = timeValue.split(':');


        //return false;

        if (timeValue == "" || timeValue.indexOf(":") < 0) {
            alert("Invalid Time format");
            return false;
        }

        var firstvalue = t[0];
        var secondvalue = t[1];

        if (firstvalue.length != 2) {
            alert("Invalid Time format");
            return false;
        }

        if (secondvalue.length != 2) {
            alert("Invalid Time format");
            return false;
        }

        if (pageInit(firstvalue) > 24) {
            alert("Invalid Time format");
            return false;
        }

        if (pageInit(secondvalue) > 60) {
            alert("Invalid Time format");
            return false;
        }

        //  alert("That is fine");
        return true;
    }


    function getObject(obj) {
        var theObj;
        if (document.all) {
            if (typeof obj == 'string') {
                return document.all(obj);
            } else {
                return obj.style;
            }
        }
        if (document.getElementById) {
            if (typeof obj == 'string') {
                return document.getElementById(obj);
            } else {
                return obj.style;
            }
        }
        return null;
    }
    //-->

    // -->

    function displaynone() {

        document.getElementById("lnkCreateOnline").style.display = "none";

    }

</script>
   
    
</head>
<body onload="disable();" bgcolor="white" onkeydown="javascript:return displayKeyCode(event)" >
    <form id="form1" runat="server"  >
    <igmisc:WebAsyncRefreshPanel ID="warpTicketing" runat="server" 
        RefreshResponse="warpTicketing_RefreshResponse" 
        RefreshComplete="warpTicketing_RefreshComplete" 
        BrowserTarget="UpLevel"  InitializePanel="warpTicketing_InitializePanel" 
        ClientError="warpTicketing_Error" 
        RefreshRequest="warpTicketing_RefreshRequest" 
         >
        
<div class="ProgressBar"  style="display:none"  id="ProgressBarwapper"  >
 
</div>

<table width="99%" align="center" border="0" class="TableBorder" cellspacing="0">
<tr class="HeaderStyle" >

                <td align="left"  colspan="2" >
 
 <table width="100%" >
 <tr>
  <td  style="width:70%;text-align:center;vertical-align:middle;"  >
 
   <h2> <asp:Label ID="lblheader" runat="server" style="font-size:19px;color:White"    CssClass="Generallabel" 
      
                        Text="Voucher Time and Rout"></asp:Label></h2>
    
  </td>

  <td   align="right" style="text-align:right" style="width:30%"  >
  <div runat="server" id="divCompany" style="color:white" >
        
        </div>
  </td>

 </tr>


 </table>
  
      
                    </td>
            </tr>
<tr>
    <td align="center" valign="top" class="style1">
                                                       
        <table cellpadding="2" cellspacing="0" style="width: 84%">                                                            
            <tr    >
                <td align="right"></td>
                <td align="left" style="display:none" >
                                                                    
                    <igtbl:UltraWebGrid ID="cboVoucherNo_1" runat="server" Height="200px" 
                        >
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
                                                            
            <tr style= "display:none" >
                <td align="right" >
                    <asp:Label ID="Label16" runat="server" CssClass="Generallabel" Text="Voucher # :"  ></asp:Label>&nbsp;
                </td>
                <td align="left">
                    <asp:TextBox ID="txtVoucherNo" Enabled="false"  runat="server" ReadOnly="true" Width="250px"></asp:TextBox>
                </td>
            </tr>
            <tr  runat="server" id="trSource" >
                <td align="right">
                    <asp:Label ID="Label9" runat="server" CssClass="Generallabel" Text="Source :" Font-Bold="True" Font-Size="15px"></asp:Label>
                </td>
                <td align="left">
                    <asp:DropDownList ID="cmbSource"  runat="server" Font-Size="20px" onclick="txtSeatNo_onblur();" Width="255px"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2" >
                    <asp:Label ID="lblWatchList" CssClass="Generallabel"  ForeColor="Red" Font-Size="Larger" Font-Bold="True" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:Label ID="lblDouple" runat="server" CssClass="Generallabel" Font-Bold="true" Font-Size="Larger" ForeColor="Red" Text=""></asp:Label>
                    <br />
                    <asp:Label ID="lblNotScan" runat="server" CssClass="Generallabel" Font-Bold="True" Font-Size="Larger" ForeColor="Red"></asp:Label>
                
                    <asp:Label ID="lblboardingpoint" runat="server" CssClass="Generallabel" Font-Bold="True" Font-Size="Larger" ForeColor="green"></asp:Label>
                      <br />
                    <asp:Label ID="lblurdu" runat="server" CssClass="Generallabel" Font-Bold="true" Font-Size="Small" Font-Names="JAMEEL NOORI NASTALEEQ"  ForeColor="Red" Text=""></asp:Label>
                    </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label15" runat="server" CssClass="Generallabel" Font-Bold="True" Font-Size="15px" Text="Destination :"></asp:Label>
                </td>
                <td align="left">
                    <asp:DropDownList ID="cmbDestination" runat="server" AutoPostBack="True" Font-Bold="True" Font-Size="20px" onclick="txtSeatNo_onblur();" Width="255px"></asp:DropDownList>
                </td>
            </tr>
                  
                             <tr id="BookingDropPoint" runat="server">
                <td align="right" >
                    <asp:Label ID="Label37" runat="server"  Font-Size="12px"  CssClass="Generallabel" Font-Bold="True" Text="Drop Point At :"></asp:Label>
                </td>
                <td align="left">
                    <asp:DropDownList ID="cmbDropAt" runat="server"  Font-Bold="True" Font-Size="20px" onclick="txtSeatNo_onblur();" Width="255px"></asp:DropDownList>
                </td>
            </tr>


            <tr>
                <td align="right">
                    <asp:Label ID="Label35" runat="server" CssClass="Generallabel" Text="Gender :"></asp:Label>
                </td>
                <td align="left">
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                    <asp:HiddenField ID="hndTimeDrop" runat="server" Value="0" />
                    <asp:HiddenField ID="hndOnlineTSNo" runat="server" />
                    <asp:HiddenField ID="hndDisbaleCount" runat="server" />
                    <asp:RadioButtonList ID="rdoGender" runat="server" Font-Bold="True" 
                        Font-Names="verdana" Font-Size="12px" RepeatDirection="Horizontal" 
                        Width="221px">
                        <asp:ListItem selected="True">Male</asp:ListItem>
                        <asp:ListItem>Female</asp:ListItem>
                     <%--   <asp:ListItem>Disable</asp:ListItem>--%>
                    </asp:RadioButtonList>
                </td>
            </tr>
           
             <tr>
                <td align="right">
                    <asp:Label ID="Label65" runat="server" CssClass="Generallabel" Text="I'D :"></asp:Label>
                </td>
                <td align="left">
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <asp:HiddenField ID="HiddenField3" runat="server" Value="0" />
                    <asp:HiddenField ID="HiddenField4" runat="server" />
                    <asp:HiddenField ID="HiddenField5" runat="server" />
                    <asp:RadioButtonList ID="rdoIDNumber" OnClick="rdodata()" runat="server" Font-Bold="True" 
                        Font-Names="verdana" Font-Size="12px" RepeatDirection="Horizontal" 
                        Width="221px">
                  
                     <%--   <asp:ListItem>Disable</asp:ListItem>--%>
                        <asp:ListItem selected="True" Value="CNIC">CNIC</asp:ListItem>
                        <asp:ListItem Value="Passport">Passport</asp:ListItem>
                        <asp:ListItem Value="Bayform">BayForm</asp:ListItem>
                        <%--<asp:ListItem Value="Loality">Loality</asp:ListItem>--%>
                        
                    </asp:RadioButtonList>
                </td>
            </tr>
            
            <tr style="display:none" >
                <td align="right" >
                    <asp:Label ID="Label2" runat="server" CssClass="Generallabel" Text="Ticket Sr. # :" Font-Bold="False"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtTicketNo" Enabled="false" runat="server" Width="250px"></asp:TextBox>
                </td>
            </tr>
            
            
            
            <tr id="Loalitybox">
                <td align="right" class="style4">
                    <asp:Label ID="Label2912555" runat="server" CssClass="Generallabel" 
                        Font-Bold="True" Text="Loality Card :"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtCustomerNumber" runat="server" 
                        placeholder="Please enter Cusomter No." Visible="True" Width="170px"></asp:TextBox>
                    <asp:Button ID="btnValidateCustomers" runat="server" CssClass="ButtonStyle" 
                        Height="21px" Text="Validate" Visible="True" Width="80px" />
                </td>
            </tr>
            
            <tr runat=server visible=false id = "ValidateCustomerPIN"    >
                <td align="right" class="style4">
                    <asp:Label ID="Label31" runat="server" CssClass="Generallabel" 
                        Font-Bold="True" Text="Enter Customer PIN :"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtCustomerPIN" runat="server" 
                        placeholder="Please enter Cusomter No." Visible="True" Width="170px"></asp:TextBox>
                    <asp:Button ID="btnValidatePIN" runat="server" CssClass="ButtonStyle" 
                        Height="21px" Text="Validate"  Width="80px" />
                    &nbsp;<asp:Label ID="lblCustomerApproved" runat="server" Font-Bold="True" 
                        Font-Size="XX-Small" ForeColor="#00CC66"></asp:Label>
                    <asp:HiddenField ID="hidCustomerPIN" runat="server" />
                </td>
            </tr>

            <tr>
                <td align="right" >
                    &nbsp;<asp:Label ID="Label73" runat="server" CssClass="Generallabel" 
                        Font-Bold="True" Text="PNR # :"></asp:Label>
&nbsp;</td>
                <td align="left">
                    <div style="float:left" >
                    </div>
                    <div style="float:left" >
                    </div>
                    <asp:TextBox ID="txtPNR2" runat="server" MaxLength="14"  placeholder="Please Enter Bookkaru PNR #." 
                        onkeypress="return displayKeyCodeCNIC(event);" Width="250px"></asp:TextBox>
                </td>
            </tr>
                                                            
            <tr>
            
            <tr>
                <td align="right" >
                    &nbsp;<asp:Label ID="Label2912556" runat="server" CssClass="Generallabel" 
                        Font-Bold="True" Text="CNIC # :"></asp:Label>
&nbsp;</td>
                <td align="left">
                    <div style="float:left" >
                    </div>
                    <div style="float:left" >
                    </div>
                    <asp:TextBox ID="txtCNIC2" runat="server" MaxLength="13"  placeholder="Please enter CNIC." 
                        onkeypress="return displayKeyCodeCNIC(event);" Width="250px"></asp:TextBox>
                </td>
            </tr>
                                                            
            <tr>
                <td align="right" >
                    <asp:Label ID="Label17" runat="server" CssClass="Generallabel" Text="Contact # :" Font-Bold="True"></asp:Label><asp:Button ID="btnValidateBooking" runat="server" Text="Validate Booking" CssClass="ButtonStyle" Height="20px" Width="137px" Visible="False" />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtContactNo"  MaxLength="11" runat="server"  onblur="getCustomerInfo();" Width="250px" placeholder="Please enter Contact No."  ></asp:TextBox>
                </td>
            </tr>
                                                            
            <tr>
                <td align="right" >
                    <asp:Label ID="Label11" runat="server" CssClass="Generallabel" Text="Passenger Name :" Font-Bold="True"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtPassengerName"  onblur="txtSeatNo_onblur();" runat="server" Width="250px" placeholder="Please enter Passenger Name"  ></asp:TextBox>
                </td>
            </tr>
            <tr style="display:none" >
                <td align="right">
                    <asp:Label ID="Label47" runat="server" CssClass="Generallabel" Font-Bold="True" Text="Miles Upto Date :"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtTotalGainMiles" runat="server" MaxLength="15" Width="250px" Enabled="false" ReadOnly="true"></asp:TextBox>                                                             
                </td>
            </tr>
            <tr style="display:none">
                <td align="right">
                    <asp:Label ID="Label48" runat="server" CssClass="Generallabel" Font-Bold="True" Text="Current Miles :"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtKM" runat="server" MaxLength="15" Width="250px" Enabled="false" ReadOnly="true"></asp:TextBox>                                                             
                </td>
            </tr>
            <tr style="display:none">
                <td align="right">
                    <asp:Label ID="Label49" runat="server" CssClass="Generallabel" Font-Bold="True" Text="Total Miles :"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtTotalMiles" runat="server" MaxLength="15" Width="250px" Enabled="false" ReadOnly="true"></asp:TextBox>                                                             
                </td>
            </tr>
            <tr >
                <td align="right">
                    <asp:Label ID="Label13" runat="server" CssClass="Generallabel" Font-Bold="True" Text="Fare :"></asp:Label>
                </td>
                <td align="left">
                    <div style="float:left" >
                    </div>
                    <table cellpadding="0" cellspacing="0" class="style6">
                        <tr>
                            <td>
                                <asp:TextBox ID="txtFare" runat="server" Enabled="false" ReadOnly="True" 
                                    Width="50px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label2912557" runat="server" CssClass="Generallabel" 
                                    Font-Bold="True" Text="Seats :"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCount" runat="server" Enabled="false" ReadOnly="True" 
                                    Width="50px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label2912558" runat="server" CssClass="Generallabel" 
                                    Font-Bold="True" Text="Total :"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotals" runat="server" Enabled="false" ReadOnly="True" 
                                    Width="50px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="display:none">
                <td align="right">
                    <asp:Label ID="Label52" runat="server" CssClass="Generallabel" Font-Bold="True" Text="Discount Percentage :"></asp:Label>
                </td>
                <td align="left">
                    <div style="float:left" >
                        <asp:TextBox ID="txtDiscountPercentage" runat="server" Enabled="false" ReadOnly="True" Width="250px" Text="10%"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr style="display:none">
                <td align="right">
                    <asp:Label ID="Label53" runat="server" CssClass="Generallabel" Font-Bold="True" Text="Discount Amount :"></asp:Label>
                </td>
                <td align="left">
                    <div style="float:left">
                        <asp:TextBox ID="txtDiscountAmount" runat="server" Enabled="false" ReadOnly="True" Width="250px"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr style="display:none">
                <td align="right">
                    <asp:Label ID="Label54" runat="server" CssClass="Generallabel" Font-Bold="True" Text="Discount Wallet Upto Date :"></asp:Label>
                </td>
                <td align="left">
                    <div style="float:left">
                        <asp:TextBox ID="txtDiscountWalletUptoDate" runat="server" Enabled="false" ReadOnly="True" Width="250px"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr style="display:none">
                <td align="right">
                    <asp:Label ID="Label55" runat="server" CssClass="Generallabel" Font-Bold="True" Text="Total Discount Wallet :"></asp:Label>
                </td>
                <td align="left">
                    <div style="float:left">
                        <asp:TextBox ID="txtTotalDiscountWallet" runat="server" Enabled="false" ReadOnly="True" Width="250px"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr style="display:none">
                <td align="right">
                    <asp:Label ID="Label57" runat="server" CssClass="Generallabel" Font-Bold="True" Text="Discount Gain Upto Date :"></asp:Label>
                </td>
                <td align="left">
                    <div style="float:left">
                        <asp:TextBox ID="txtDiscountGainUptoDate" runat="server" Enabled="false" ReadOnly="True" Width="250px"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr style="display:none">
                <td align="right">
                    <asp:Label ID="Label59" runat="server" CssClass="Generallabel" Font-Bold="True" Text="Maximum Discount can get :"></asp:Label>
                </td>
                <td align="left">
                    <div style="float:left">
                        <asp:TextBox ID="txtMaxDiscount" runat="server" Enabled="false" ReadOnly="True" Width="250px"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr style="display:none">
                <td align="right">
                    <asp:Label ID="Label56" runat="server" CssClass="Generallabel" Font-Bold="True" Text="Get Discount :"></asp:Label>
                </td>
                <td align="left">
                    <asp:CheckBox ID="txtGetDiscount" runat="server" Width="45px" onclick="EnableTextBox(this)" />
                    <span style="display:none">
                        <asp:TextBox ID="txtVerificationCodeDB" runat="server" Enabled="false" ReadOnly="True" Width="200px"></asp:TextBox>
                    </span>
                </td>
            </tr>
            <tr style="display:none">
                <td align="right">
                    <asp:Label ID="Label58" runat="server" CssClass="Generallabel" Font-Bold="True" Text="Enter Amount to Get Discount :"></asp:Label>
                </td>
                <td align="left">
                    <div style="float:left">
                        <asp:TextBox ID="txtDiscountGain" runat="server" Enabled="false" Width="250px" onkeyup="validateDiscount()" />
                    </div>
                </td>
            </tr>
            <tr style="display:none">
                <td align="right">
                    <asp:Label ID="Label60" runat="server" CssClass="Generallabel" Font-Bold="True" Text="Enter Verification Code :"></asp:Label>
                </td>
                <td align="left">
                    <div style="float:left">
                        <asp:TextBox ID="txtVerificationCode" runat="server" Enabled="false" Width="250px" onblur="codeVerification()"></asp:TextBox>
                    </div>
                </td>
            </tr>

            <tr style="display:none">
                <td align="right">
                    <asp:Label ID="Label43" runat="server" CssClass="Generallabel" Font-Bold="True" 
                        Text="Scan CNIC # :"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtCNICNew" runat="server" MaxLength="15" onkeypress="return displayKeyCodeCNIC(event);" TextMode="MultiLine" Width="250px"></asp:TextBox>
                </td>
            </tr>

          
                                                            
            <tr >
                <td align="right" >
                    <asp:Label ID="Label8" runat="server" CssClass="Generallabel" Text="Seat # :"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtSeatNo" runat="server" Font-Bold="True" ForeColor="Red" Width="115px" Enabled="False"></asp:TextBox>
                    <asp:Button ID="btnUndo" runat="server" CssClass="ButtonStyle" Height="21px" Text="Undo" Visible="False" Width="42px" />
                      <asp:Label ID="Label50" runat="server" CssClass="Generallabel" Text="Discount:"></asp:Label>

                      <asp:TextBox ID="DiscountFare" runat="server" Font-Bold="True" ReadOnly="True" ForeColor="Red" Width="65px"></asp:TextBox>
                    <asp:Button ID="btnDiscountFare" runat="server" CssClass="ButtonStyle" Height="21px" Text="Undo" Visible="False" Width="42px" />
                </td>

               
            </tr>
            

            <tr style="display:none" >
                <td align="right" >
                    <asp:Label ID="Label18" runat="server" CssClass="Generallabel" Text="Cash :" Font-Bold="True"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtAmount" runat="server" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr style="display:none" >
                <td align="right">
                    <asp:Label ID="Label27" runat="server" CssClass="Generallabel" Font-Bold="True" Text="Total :"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtTotal" runat="server" Enabled="false" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr style="display:none" >
                <td align="right">
                    <asp:Label ID="Label20" runat="server" CssClass="Generallabel" Font-Bold="True" Text="Balance :"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtBalance" runat="server" Enabled="false" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr id="trDeduction" runat="server">
                <td align="right"  >
                    <asp:Label ID="Label19" runat="server" CssClass="Generallabel" Font-Bold="True" Text="Deduction :"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtDeduction" runat="server" Width="100px"></asp:TextBox>
                </td>
            </tr>                                                            
                                                            
            <tr id="TicketCollectionPoint" runat="server">
                <td align="right" class="style3" >
                    <asp:Label ID="Label41" runat="server" CssClass="Generallabel" Font-Bold="True" Text="Collection Point :"></asp:Label>
                </td>
                <td align="left" class="style3">
                    <asp:DropDownList ID="cmbTicketCollectPoint" runat="server"></asp:DropDownList>
                </td>
            </tr>
                                                            
            <tr style="display:none" >
                <td align="right">
                    <asp:Label ID="Label42" runat="server" CssClass="Generallabel" Font-Bold="True" Text="PNR Number :"></asp:Label>
                </td>
                <td align="left">
                    <!-- End demo -->
                    &nbsp;<asp:TextBox ID="txtPNR" runat="server" Width="100px"></asp:TextBox>
                    <asp:Button ID="btnScanResult" runat="server" CssClass="ButtonStyle" onclientclick="return validateTime(this);" Text="Go" Visible="True" Width="50px" />
                </td>
            </tr>
                                                            

                                                            
            <tr style= "display:none">
                <td align="right"  >
                    <asp:Label ID="Label5" runat="server" Text="Departure Date :" 
                        CssClass="Generallabel"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtDepartureDate" runat="server" ReadOnly="True" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr style="display:none" >
                <td align="right">
                    <asp:Label ID="Label6" runat="server" CssClass="Generallabel" 
                        Text="Departure Time :"></asp:Label>
                    </td>
                <td align="left">
                    <asp:TextBox ID="txtDepartureTime" runat="server" Width="150px" ReadOnly="True"></asp:TextBox> 
                    <input id="bkDate" runat="server" type="hidden" value="" />
                    <input id="hidTerminal" runat="server" type="hidden" value="" />
                    <input id="Desct" runat="server" type="hidden" value="" />
                    <input id="CloseNo" runat="server" type="hidden" value="" />
                    <input id="CloseSMS" runat="server" type="hidden" value="" />
                    <input id="hidMode" runat="server" type="hidden" value="" />
                    <input id="hidTotal" runat="server" type="hidden" value="" />
                    <input id="hidSource" runat="server" type="hidden" value="" />
                    <input id="hidRoute" runat="server" type="hidden" value="" />
                    <input id="hidPrint" runat="server" type="hidden" value="" />
                    <input id="hidSeatNo" runat="server" type="hidden" value="" />
                    <input id="hidTSID" runat="server" type="hidden" value="0" />
                    <input id="hidSMSDataPort" runat="server" type="hidden" value="40" />
                    <input id="BookingSMS" runat="server" type="hidden" value="" />
                    <input id="hidSMSData" runat="server" type="hidden" value="" />
                    <input id="VoucherStatus" runat="server" type="hidden" value="1" />
                </td>
            </tr>
              <tr>
                <td align="right" >
                    <asp:Label ID="Label63" runat="server" CssClass="Generallabel" Text="Vehicile # :"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtVehicle" runat="server" Font-Bold="True" ForeColor="Red" Width="250px">Advance</asp:TextBox>
          
                </td>
            </tr>

            <tr id="ActualDepartureTime" runat="server">
                <td align="right">
                    <asp:Label ID="Label28" runat="server" CssClass="Generallabel" 
                        Text="Actual Dep Time :"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtActualDepartureTime" runat="server" Width="150px"></asp:TextBox>
                    &nbsp;<asp:Button ID="btnSaveDep" runat="server" CssClass="ButtonStyle" 
                        Text="Save" Visible="True" Width="50px" 
                        onclientclick="return validateTime(this);" />
                    </td>
            </tr>

            <tr id="Tr1" runat="server">
                <td align="right">
                    <asp:Label ID="Label70" runat="server" CssClass="Generallabel" 
                        Text="Refreshment Charges:"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtRefreshmentiteam" Enabled="false" runat="server" Width="150px"></asp:TextBox>
                   
                    </td>
            </tr>
             <tr id="Tr2" runat="server">
                <td align="right">
                    <asp:Label ID="Label71"  runat="server" CssClass="Generallabel" 
                        Text="T.F Charges :"></asp:Label>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtTerminalIssuance" Enabled="false" runat="server" Width="150px"></asp:TextBox>
                   
                    </td>
            </tr>
                <tr>
               
               
                     <td align="left">
                 
                    &nbsp;<asp:Button ID="btnpospayment" runat="server"  CssClass="ButtonStyle" 
                        Text="Pay" Visible="True" Width="50px" 
                        onclientclick="return validateTime(this);"  />
                    </td>
                    
            </tr>
              <tr>
                <td align="right">
                    <asp:Label ID="Label69" runat="server" CssClass="Generallabel" Text="Select Printer :"></asp:Label>
                </td>
                <td align="left">
                    <asp:HiddenField ID="HiddenField6" runat="server" />
                    <asp:HiddenField ID="HiddenField7" runat="server" Value="0" />
                    <asp:HiddenField ID="HiddenField8" runat="server" />
                    <asp:HiddenField ID="HiddenField9" runat="server" />
                    <asp:RadioButtonList ID="rdoprints" runat="server" Font-Bold="True" 
                        Font-Names="verdana" Font-Size="12px" RepeatDirection="Horizontal" 
                        Width="221px">
                        <asp:ListItem selected="True" >Normal</asp:ListItem>
                        <asp:ListItem   Value="Business">Business</asp:ListItem>
                      
                  
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label72" runat="server" CssClass="Generallabel" Text="Value Added Services :"></asp:Label>
                </td>
                <td align="left">
                    <asp:HiddenField ID="HiddenField10" runat="server" />
                    <asp:HiddenField ID="HiddenField11" runat="server" Value="0" />
                    <asp:HiddenField ID="HiddenField12" runat="server" />
                    <asp:HiddenField ID="HiddenField13" runat="server" />
                    <asp:RadioButtonList ID="TFrdoprints" runat="server" Font-Bold="True" 
                        Font-Names="verdana" Font-Size="12px" RepeatDirection="Horizontal" 
                        Width="221px">
                        <asp:ListItem selected="True" >Yes</asp:ListItem>
                       <%-- <asp:ListItem  Value="WithoutTF">No</asp:ListItem>--%>
                    
                      
                  
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    &nbsp;</td>
                <td align="left">
                    <asp:Button ID="btnMissed" runat="server"  CssClass="ButtonStyle" Text="Missed" 
                        Visible="False" Width="81px"/>
                    <asp:Button ID="btnReprint"  runat="server"   CssClass="ButtonStyle" 
                        Text="Print E Ticket" Visible="False" Width="133px"  />
                    &nbsp;<asp:Button ID="btnCancelTicket" runat="server" CssClass="ButtonStyle" 
                        Text="Cancel Booking" Visible="False" Width="81px" />
                    &nbsp;<asp:Button ID="btnSkip" runat="server" CssClass="ButtonStyle" Text="Skip" 
                        Visible="False" Width="50px" />
                    <asp:Button ID="btnSave" runat="server" CssClass="ButtonStyle" Text="Print" 
                        Width="81px" />
                    <input id="hdnPrint" runat="server" type="hidden" value="0" />
                </td>
            </tr>
            <tr>
                <td align="right">
                        &nbsp;</td>
                <td align="left">
                    &nbsp;</td>
            </tr>
            
            <tr>
                                                                <td align="left" colspan="2">
                                                                    <table width=100% >
                                                                    <tr>
                                                                     <td class="Generallabel" style="font-weight:bold;">
                                                                          Booked</td>
                                                                    
                                                                     
                                                                  <td class="Generallabel" style="font-weight:bold;">
                                                                      Avaiable</td>
                                                                    
                                                                    
                                                                     <td class="Generallabel" style="font-weight:bold;">
                                                                         Total Issued</td>
                                                                
                                                                        <tr>
                                                                             
                                                                            <td  class="TicketBooked" height="22px" 
                                                                                style="color:#fff;padding:10px;margin:10px;color:White;text-align:center">
                                                                                <asp:Label ID="lblB" runat="server"  Text=" "></asp:Label>
                                                                                 </td>
                                                                             <td  class="TicketAvailable" style="color:#000;padding:10px;margin:10px;text-align:center;">
                                                                                 <asp:Label ID="lblA" runat="server"  
                                                                                     style="font-size:x-large" Text="0"></asp:Label>
                                                                            </td>
                                                                             
                                                                            <td  class="TicketConfirmed" style=";padding:10px;margin:10px;text-align:center; font-size: x-large;">
                                                                                &nbsp;
                                                                                <asp:Label ID="lblC" runat="server" CssClass="Generallabel" 
                                                                                    style="color:White !important;font-size:x-large" Text="0"  ></asp:Label>
                                                                                &nbsp;
                                                                                <asp:Label ID="Label66" runat="server" CssClass="Generallabel" 
                                                                                    style="color:White !important;font-size:x-large;display:none" Text="0"></asp:Label>
                                                                                </td>
                                                                             
                                                                           
                                                                        </tr>
                                                                  </td>
                                                            
                                                            
                                                            
                                                                    </table>
                                                                    
                                                                    </td>
                                                            </tr>
            <tr>
                <td align="left" colspan="2">
                    <div ID="divCityList" runat="server">
                    </div>
                </td>
            </tr>
                                                              
            <tr>
                                                                
                <td align="center" colspan=2 >
                                                                    
                    <table cellpadding="2" cellspacing="2" class="TableBorder" width="80%">
                        <tr class="HeaderStyle">
                            <td align="center" valign="middle">
                                Previous Activity</td>
                        </tr>
                        <tr>
                            <td ID="tdSeatNo" runat="server" align="left" class="Generallabel">
                            </td>
                        </tr>
                        <tr>
                            <td ID="tdPassenger" runat="server" align="left" class="Generallabel">
                            </td>
                        </tr>
                        <tr>
                            <td ID="tdExtraComm" runat="server" align="left" class="Generallabel">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <igtbl:UltraWebGrid ID="grdCityInfo" runat="server" Height="200px" Width="97%">
                        <Bands>
                            <igtbl:UltraGridBand>
                                <RowEditTemplate>
                                    <br>
                                    <p align="center">
                                        <input ID="igtbl_reOkBtn" onclick="igtbl_gRowEditButtonClick(event);" 
                                            style="width:50px;" type="button" value="OK"> &nbsp;
                                        <input ID="igtbl_reCancelBtn" onclick="igtbl_gRowEditButtonClick(event);" 
                                            style="width:50px;" type="button" value="Cancel"> </input> </input>
                                    </p>
                                    </br>
                                </RowEditTemplate>
                                <RowTemplateStyle BackColor="White" BorderColor="White" BorderStyle="Ridge">
                                    <BorderDetails WidthBottom="3px" WidthLeft="3px" WidthRight="3px" 
                                        WidthTop="3px" />
                                </RowTemplateStyle>
                                <AddNewRow View="NotSet" Visible="NotSet">
                                </AddNewRow>
                            </igtbl:UltraGridBand>
                        </Bands>
                        <DisplayLayout AllowColSizingDefault="NotSet" 
                            AllowColumnMovingDefault="OnServer" AllowSortingDefault="NotSet" 
                            AllowUpdateDefault="NotSet" BorderCollapseDefault="Separate" 
                            HeaderClickActionDefault="NotSet" Name="UltraWebGrid1" RowHeightDefault="35px" 
                            RowSelectorsDefault="No" SelectTypeRowDefault="Extended" 
                            StationaryMargins="Header" StationaryMarginsOutlookGroupBy="True" 
                            TableLayout="Fixed" Version="4.00" ViewType="OutlookGroupBy">
                            <FrameStyle BorderColor="InactiveCaption" BorderStyle="Solid" BorderWidth="2px" 
                                Font-Names="Verdana" Font-Overline="False" Font-Size="Larger" Height="200px" 
                                Width="97%">
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
                            <HeaderStyleDefault BackColor="#0033CC" BackgroundImage="./images/bg_002.gif" 
                                BorderStyle="Solid" Font-Bold="True" ForeColor="White" HorizontalAlign="Left">
                                <Margin Bottom="10px" Left="10px" Right="10px" Top="10px" />
                                <Padding Bottom="10px" Left="10px" Right="10px" Top="10px" />
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
                            <GroupByBox Hidden="True">
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
            </table>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Timer ID="Timer1" runat="server" >
        </asp:Timer>
        <br>
                                                            
                                                        
                                                            
                                                         
    </td>
    <td align="center" valign="top" width="60%" visible="False">
        <table width="100%" align="center" border="0" class="TableBorder" cellspacing="0">

            <tr class="HeaderStyle">
                <td align="center" valign="middle">

                    <asp:Label ID="lblTitles" runat="server" Text="Select Seat" ></asp:Label></td>
                    

            </tr> 
            <tr class="HeaderStyle">
                <td align="left" valign="middle">

                           <asp:Label ID="lblSleeperTitles" runat="server" Text="Down" Font-Size="20px" Font-Bold="true"  ForeColor="White" ></asp:Label>
                    <asp:Label ID="Label75" runat="server" Text="Down " Font-Size="20px" Font-Bold="true"  ForeColor="White"></asp:Label>
                      <asp:Label ID="lblupper" runat="server" Text="Up" Font-Size="20px" Font-Bold="true"  ForeColor="White"></asp:Label>
                    <asp:Label ID="Label74" runat="server" Text="Up" Font-Size="20px" Font-Bold="true"  ForeColor="White"></asp:Label>
                    
                    <asp:Label ID="lblSleeperbusUP3" runat="server" Text="Down"  Font-Size="20px" Font-Bold="true"  ForeColor="White"></asp:Label>
                    <asp:Label ID="lblSleeperbusDown3" runat="server" Text="UP"  Font-Size="20px" Font-Bold="true"  ForeColor="White"></asp:Label>

                    <asp:Label ID="lblSleeperbusUP1" runat="server" Text="UP"  Font-Size="20px" Font-Bold="true"  ForeColor="White"></asp:Label>
                    <asp:Label ID="lblSleeperbusUP2" runat="server" Text="UP"  Font-Size="20px" Font-Bold="true"  ForeColor="White"></asp:Label>

                    

                    <asp:Label ID="lblSleeperbusDown1" runat="server" Text="Down"  Font-Size="20px" Font-Bold="true"  ForeColor="White"></asp:Label>
                    <asp:Label ID="lblSleeperbusDown2" runat="server" Text="Down"  Font-Size="20px" Font-Bold="true"  ForeColor="White"></asp:Label>

                    

                </td>
                
                    

            </tr> 

            

                                                                                                     
            <tr>
                <td align="right" valign="middle">
                        <div style="position:absolute;background: rgba(204, 204, 204, 0.5);vertical-align: middle;text-align:center;width:50%;height:56%;vertical-align:middle;font-size:50px;color:Red;font-weight:bold;"  runat="server" id="TicketingWapper" >
                        <h1>This Voucher is Closed</h1> 
                        </div>
                        <table id="tblTickets" runat="server"  disabled="disabled"  align="center" cellpadding="2" cellspacing="5" style="width:100%;border-radius: 3px;-moz-border-radius: 3px;border: solid #E7E7E7 3px">
                                                                        
                    </table>
                </td>
            </tr>
        </table>
        <asp:CheckBox ID="chkOnline" runat="server"  CssClass="Generallabel"
            Font-Bold="True" ForeColor="Red" Text="ONLINE" Checked="True" />
        &nbsp;&nbsp;<asp:LinkButton ID="lnkMapping"  runat="server" CssClass="linkButton" style="display:none;" >Online Mapping</asp:LinkButton>&nbsp;&nbsp;
        <asp:Button ID="btnRefresh" runat="server" CssClass="ButtonStyle" Text="Refresh" 
            Width="67px" Height="23px" /><br>
        <asp:Label ID="lblErr" runat="server" CssClass="Errorlabel"></asp:Label>
        &nbsp;<asp:LinkButton ID="lnkCreateOnline"  OnClientClick="javascript:displaynone();" runat="server" CssClass="linkButton">Create  Online</asp:LinkButton><br>
        
                <table width="100%"  cellpadding=2 cellspacing=2 >
        <tr>
          <td width="50%"  style="text-align:center" >
              <input ID="btnVoucherReport3" runat="server" class="ButtonStyle" 
                  onclick="btnVoucherReport_onclick(0);" type="button" value="Voucher" style="font-size:12px !important;width:130px" /></td>
            
                     <td style="text-align:center">
                         <input ID="btnVoucherReport1" runat="server" class="ButtonStyle" 
                             onclick="btnVoucherReportAll_onclick(1);" 
                             style="font-size:12px !important;width:130px" type="button" 
                             value="Voucher All " /></td>
  
            <td style="text-align:center">
                <input ID="btnCNICSheet" runat="server" class="ButtonStyle" 
                    onclick="btnVoucherReportCNIC_onclick();" 
                    style="font-size:12px !important;width:130px" type="button" 
                    value="CNIC Sheet" /></td>
              <td style="text-align:center">
                        <input ID="Button1" runat="server" class="ButtonStyle" 
                            onclick="btnVoucherReportSmartCNIC_onclick(1);" 
                            style="font-size:12px !important;width:130px" type="button" 
                            value="Smart CNIC " /></td>

             <td style="text-align:center">
                        <input ID="Button2" runat="server" class="ButtonStyle" 
                            onclick="btnVoucherReportValueAddedServices_onclick(1);" 
                            style="font-size:12px !important;width:130px" type="button" 
                            value="VAS Passengers" /></td>
           
  
        </tr>
        
         <tr>
          <td width="50%"  style="text-align:center">
              <input ID="btnVoucherReport2" runat="server" class="ButtonStyle" 
                  onclick="btnVoucherReportSmart_onclick(0);" 
                  style="font-size:12px !important;width:130px" type="button" 
                  value="Smart Voucher" /></td>
            
              <td style="text-align:center">
                 <input ID="btnVoucherReport4" runat="server" class="ButtonStyle" 
                     onclick="btnVoucherReportSmart_onclick(1);" 
                     style="font-size:12px !important;width:130px" type="button" 
                     value="Smart Voucher All " /></td>
             <td style="text-align:center">
                 <asp:Button ID="btnTransit" runat="server" CssClass="ButtonStyle" 
                     style="font-size:12px !important;width:130px" Text="Transit" />
             </td>
             
                  <td style="text-align:center">
                <input ID="btnVoucherReportNull" runat="server" class="ButtonStyle" 
                    onclick="btnVoucherReportNull_onclick();" 
                    style="font-size:12px !important;width:130px" type="button" 
                    value="Zero Voucher" /></td>
                  
             
            

            
            

        </tr>
        
         <tr>
          <td width="50%" style="text-align:center" >
              <asp:Button ID="btnCloseVoucher" runat="server" CssClass="voucherClosingColor" 
                  style="font-size:12px !important;width:130px" Text="Close Voucher" />
          </td>
            
                
                    <td style="text-align:center">
                        <input ID="btnClose" class="ButtonStyle" 
                            onclick="window.location = 'TicketingSchedule.aspx'" 
                            style="font-size:12px !important;width:130px" type="button" value="Close Window" /></td>
              
            

             <td style="text-align:center">
                 <input id="btnCancelReport" class="ButtonStyle" 
                     onclick="return btnCancelReport_onclick();" 
                     style="font-size:12px !important;width:180px" type="button" 
                     value="Booking Cancel Report" /></td>
             <td style="text-align:center">
                 <asp:Button ID="btnDropTime" OnClientClick="return validatedrop();" runat="server" CssClass="voucherClosingColor" 
                     Font-Bold="True" style="font-size:12px !important;width:130px" 
                     Text="Drop Time"  />
             </td>
              
                <td style="text-align:center">
                        <input ID="btnPreview" class="ButtonStyle" 
                            onclick=" windowOpen()" 
                            style="font-size:12px !important;width:130px" type="button" value="Preview Voucher"  /></td>
              

        </tr>
        
         <tr>
          <td width="50%" >
              &nbsp;</td>
            
                 
                    <td>
                        &nbsp;</td>
           
             <td>
                 &nbsp;</td>
             <td>
             </td>
           
        </tr>
        
        
        </table>                                            
    </td>
</tr>                                                
        <tr>
            <td colspan="2">
                &nbsp;</td>
        </tr>
        </table>
        <tr>
            <td colspan="2">
                &nbsp;</td>
        </tr>
        </table>
 
                                                     
        </td>
        </tr>
        <tr>
            <td align="center" class="style1" valign="top">
                <table ID="tblCity" class="" style="width:50%">
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                </table>
            </td>
            <td align="Left" valign="middle">
                &nbsp;</td>
        </tr>
        <tr>
            <td align="center" class="style1" valign="top">
            </td>
            <td align="right" valign="middle">
                &nbsp;</td>
        </tr>
        <tr>
            <td align="right" colspan="2" height="5px" valign="middle">
                <asp:LinkButton ID="lkWatchList" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="lnkMakeAvailable" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="lnkReserve" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="lnkSeatDetail" runat="server"></asp:LinkButton>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
            </td>
        </tr>
        </table>
<div id="divTransit"  style="left: 235px;width: 50%;top:0;position:absolute;z-index:100000;display:none;height:800px"  >
<table id="tblTransit" width=100% height="800px"  runat="server" cellpadding="2" cellspacing="0" >
<tr class="HeaderStyle">
    <td align="center" >Transit</td>
</tr>
                                                
<tr >
    <td align="left" valign=top >
        <iframe  frameborder=0 src=Transit.aspx?TS_ID=<%=Request.QueryString("TSID") %> height=500px width="100%" >
                                                              
        </iframe>
    </td>
</tr>

                                                
</table>
</div>
<table ID="tblDeductions"  style="left: 235px;width: 90%;" runat="server" cellpadding="2" cellspacing="0" >
        <tr>
            <td align="left" valign="middle">
                <table cellpadding="2" cellspacing="0" style="width: 100%" class="TableBorder">
                    <tr class="HeaderStyle">
                        <td align="center" colspan="6">Voucher Summary</td>
                    </tr>
                        <tr class="HeaderStyle">
                        <td align="center" colspan="2">
                            Deductions</td>
                        <td align="center">
                            &nbsp;</td>
                            <td align="center" >
                                Expenses</td>
                            <td align="center">
                                &nbsp;</td>
                            <td align="center">
                                Information</td>
                    </tr>
                    <tr>
                <td align="right" class="style4">
                    <asp:Label ID="Label3" runat="server" CssClass="Generallabel" Font-Bold="True" 
                        Text="Company :"></asp:Label>
                </td>
                <td align="left">
                    <asp:DropDownList ID="cmbCompany" runat="server" AutoPostBack="True" 
                        Font-Bold="True" Font-Size="15px" onclick="txtSeatNo_onblur();" Width="150px" >
                    </asp:DropDownList>
                </td>

                    <td align="right" class="style4">
                    <asp:Label ID="Label30" runat="server" CssClass="Generallabel" Font-Bold="True" 
                        Text="Misc :"></asp:Label>
                </td>

                        <td>
                              <asp:TextBox ID="txtMisc" runat="server" CssClass="txtbox" MaxLength="40" 
                                onblur="updateTotal();" Width="150px" ></asp:TextBox>
                        </td>
            </tr>
                                                            
            <tr>
                <td align="right" class="style4" >
                     
                    <asp:Label ID="Label4" runat="server" Text="Vehicle No. :" 
                        CssClass="Generallabel"></asp:Label>
                    &nbsp;
                </td>
                <td align="left">
                     <asp:TextBox ID="TextBox1" runat="server" Font-Bold="True" ForeColor="Red" Width="250px">Advance</asp:TextBox>
                     <asp:TextBox ID="txtVehicleNo" runat="server" ReadOnly="True" Width="150px" 
                        Visible="false"> 
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </asp:TextBox>
                    <div class="demo">
                        <div class="ui-widget">
                            <asp:DropDownList ID="cmbVehicle" runat="server" AutoPostBack="True" visible="false" >
                            </asp:DropDownList>
                            <asp:HiddenField ID="hidReoutName" runat="server" />
                            <asp:HiddenField ID="hidTicketChange" runat="server" />
                            <asp:HiddenField ID="hidTicketRefund" runat="server" />
                            <asp:HiddenField ID="hndVechileNo" runat="server" />
                            <asp:HiddenField ID="hndPrintDateTime" runat="server" />
                            <asp:HiddenField ID="hndCustID" runat="server" />
                            <asp:HiddenField ID="hndCanChangeFare" runat="server" Value="0" />
                            <asp:HiddenField ID="hndDisable" runat="server" Value="0" />
                            <asp:HiddenField ID="hndTimDropped" runat="server" Value="0" />
                            <asp:HiddenField ID="hndServiceType" runat="server" />
                            <asp:Button ID="btnSaveVehicle" runat="server" CssClass="ButtonStyle" 
                                onclientclick="return validateVehicle(this);" Text="Save" Visible="True" 
                                Width="50px" />
                            <asp:Label ID="lblErrVechile" runat="server" CssClass="Errorlabel"></asp:Label>
                        </div>
                    </div>
                    <!-- End demo -->
                </td>
                 <td align="right" class="style4" >
                     
                    <asp:Label ID="Label7" runat="server" Text="Refreshment" 
                        CssClass="Generallabel"></asp:Label>
                    &nbsp;

                    
                </td>
                <td>
                     <asp:TextBox ID="txtRefreshment" runat="server" CssClass="txtbox" 
                                    MaxLength="40" onblur="updateTotal();" Width="150px"></asp:TextBox>
                </td>
                 
            </tr>
                        <tr>
                        <td align="right" class="style8">
                            <asp:Label ID="Label32" runat="server" CssClass="Generallabel" 
                                Text="Commission %:"></asp:Label>
                            &nbsp;
                        </td>
                        <td class="style7">
                            <asp:TextBox ID="txtComPer" runat="server" CssClass="txtbox" 
                                MaxLength="40" Width="150px" ReadOnly="True" onblur="updateTotal();" 
                                Text="0"></asp:TextBox>
                        </td>
                            <td align="right" >
                                <asp:Label ID="Label2412542" runat="server" CssClass="Generallabel" 
                                    Text="Manual Commission:"></asp:Label>
                            </td>
                            <td>
                               
                                
                                <asp:TextBox ID="txtBusCharges" runat="server" CssClass="txtbox" 
                                MaxLength="40" Width="150px"></asp:TextBox>
                            </td>
                            <td align="right" >
                                <asp:Label ID="Label10" runat="server" CssClass="Generallabel" Font-Bold="True" 
                                    Text="Control Number:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtScheduleNo" runat="server" CssClass="txtbox" 
                                    Font-Bold="True" ForeColor="Red" MaxLength="40" Width="150px" 
                                    Visible="False"></asp:TextBox>
                                     <div class="ui-widget">
                            
                            
                           
                                <asp:DropDownList ID="cmbControlerNumber" 
                                onclick="Checkbus();" runat="server" AutoPostBack="False">
                                </asp:DropDownList> </div>
                            </td>

                              <td align="right" class="style8">
                            <asp:Label ID="Label51" runat="server" CssClass="Generallabel" Font-Bold="True"
                                Text="Cargo Cash :" ></asp:Label>
                            &nbsp;
                            </td>
                        <td class="style7">
                            <asp:TextBox ID="txtcargocash" runat="server" CssClass="txtbox" 
                                MaxLength="40" Width="150px" ></asp:TextBox>
                        </td>




                    </tr>
                    <tr>
                         <td align="right">
                           
                        </td>
                        <td >
                           
                        </td>
                         <td align="right">
                           
                             <asp:Label ID="Label38" runat="server" CssClass="Generallabel" 
                                 Text="Terminal Expense :"></asp:Label>
                           
                        </td>
                        <td >
                            
                            <asp:TextBox ID="txtTerminalExpense" runat="server" CssClass="txtbox" 
                                MaxLength="40" onblur="updateTotal();" Width="150px"></asp:TextBox>
                            
                        </td>
                          <td align="right">
                              <asp:Label ID="Label33" runat="server" CssClass="Generallabel" 
                                  Text="Driver Name :"></asp:Label>
                        </td>
                        <td >
                            <asp:TextBox ID="txtDriverName" runat="server" CssClass="txtbox" MaxLength="40" 
                                Width="150px"></asp:TextBox>
                        </td>
                          <td align="right" class="style8">
                            <asp:Label ID="Label62" runat="server" CssClass="Generallabel" Font-Bold="True"
                                Text="Cargo Commission :" ></asp:Label>
                            &nbsp;
                            </td>
                        <td class="style7">
                            <asp:TextBox ID="txtcargocommission" runat="server" CssClass="txtbox" 
                                MaxLength="40" Width="150px"  onkeyup="calculateCargoCommission()" ></asp:TextBox>
                        </td>



                    </tr>
                    <tr  >
                        <td align="right" class="style8">
                            <asp:Label ID="Label29" runat="server" CssClass="Generallabel" 
                                Text="Commission:"></asp:Label>
                            &nbsp;
                        </td>
                        <td class="style7" >
                            <asp:TextBox ID="txtcommission" runat="server" CssClass="txtbox" MaxLength="40" 
                                ReadOnly="True" Width="150px">0</asp:TextBox>
                        </td>
                        <td align="right" >
                            <asp:Label ID="Label39" runat="server" CssClass="Generallabel" Text="Reward :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReward" runat="server" CssClass="txtbox" MaxLength="40" 
                                onblur="updateTotal();" Width="150px"></asp:TextBox>
                        </td>
                        <td align="right" >
                            <asp:Label ID="Label34" runat="server" CssClass="Generallabel" 
                                Text="Hostress Name :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtHostessName" runat="server" CssClass="txtbox" 
                                MaxLength="40" Width="150px"></asp:TextBox>
                        </td>
                          <td align="right" class="style8">
                            <asp:Label ID="Label64" runat="server" CssClass="Generallabel" Font-Bold="True"
                                Text="Bus Cargo Cash:" ></asp:Label>
                            &nbsp;
                            </td>
                        <td class="style7">
                            <asp:TextBox ID="txtcargoIncom" runat="server" CssClass="txtbox" 
                                MaxLength="40" Width="150px"    ></asp:TextBox>
                        </td>



                    </tr>
                     
                    <tr>
                        <td align="right" class="style8">
                            <asp:Label ID="Label67" runat="server" CssClass="Generallabel" 
                                Text="B.K Commission % :"></asp:Label>
                        </td>
                        <td class="style7">
                            <asp:TextBox ID="txtBKCommPer" runat="server" CssClass="txtbox" MaxLength="40" 
                                 Width="150px">0</asp:TextBox>
                        </td>
                        <td align="right" >
                            <asp:Label ID="Label40" runat="server" CssClass="Generallabel" Text="Toll GBS / Parchi :"></asp:Label>
                        </td>
                        <td>
                          

                             <asp:TextBox ID="txtToll" runat="server" CssClass="txtbox" 
                                MaxLength="40" Width="150px"></asp:TextBox>
                        </td>
                        <td align="right" >
                            <asp:Label ID="Label12" runat="server" CssClass="Generallabel" 
                                Text="Guard Name :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtGuard" runat="server" CssClass="txtbox" MaxLength="40" 
                                Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="style8">
                            <asp:Label ID="Label68" runat="server" CssClass="Generallabel" 
                                Text="B.K Commission:"></asp:Label>
                        </td>
                        <td class="style7">
                            <asp:TextBox ID="txtBKComm" runat="server" CssClass="txtbox" 
                                MaxLength="40" Width="150px" ReadOnly="True">0</asp:TextBox>
                        </td>
                        <td align="right" >
                            <asp:Label ID="Label21" runat="server" CssClass="Generallabel" 
                                Text="Hostess Salary :"></asp:Label>
                        </td>
                        <td>
                             <asp:TextBox ID="txtHostessSalary" runat="server" CssClass="txtbox" 
                                MaxLength="40" Width="150px" ></asp:TextBox>
                        </td>
                        <td align="right" >
                            <asp:Label ID="Label61" runat="server" CssClass="Generallabel" 
                                Text="CC Number :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCCNumber" runat="server" CssClass="txtbox" MaxLength="40" 
                                Width="150px"></asp:TextBox>
                                 
                        </td>
                    </tr>
                    <tr >
                        <td align="right" class="style8">
                            <asp:Label ID="Label14" runat="server" CssClass="Generallabel" 
                                Text="Hostess Salary :" Visible="false"></asp:Label>
                            &nbsp;
                            </td>
                        <td class="style7">
                            <asp:TextBox ID="txtHostessSalary1" runat="server" CssClass="txtbox" 
                                MaxLength="40" Width="150px" Visible="false"></asp:TextBox>
                        </td>
                        <td align="right" >
                            <asp:Label ID="Label2611" runat="server" CssClass="Generallabel" 
                                Font-Bold="True" Text="Paid To Driver :"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalDeductions1" runat="server" CssClass="txtbox" 
                                Font-Bold="True" MaxLength="40" Visible="false" Width="150px"></asp:TextBox>

                           <asp:TextBox ID="txtPaidToDriver" runat="server" CssClass="txtbox" 
                                MaxLength="40" onblur="updateTotal();" Width="150px"></asp:TextBox>
                        </td>

                        <td align="right">
                            &nbsp;</td>
                        <td id="tdOKCancel" runat=server >
                            <input ID="btnCancel" runat="server" class="ButtonStyle" 
                                onclick="document.getElementById('tblDeductions').style.display = 'none';" 
                                type="button" value="Cancel" />
                            <asp:Button ID="btnOK" runat="server" CssClass="ButtonStyle" 
                                OnClientClick="javascript:return validateclose();" Text="OK" Width="81px" />
                        </td>
                        
                    </tr>
                   
                    <tr>
                        <td align="right" class="style8">
                            &nbsp;
                            <asp:Label ID="Label22" runat="server" CssClass="Generallabel" 
                                Text="Gaurd Salary :" Visible="false"></asp:Label>
                        </td>
                        <td class="style7">
                            <asp:TextBox ID="txtGuardSalary" runat="server" CssClass="txtbox" 
                                MaxLength="40" Width="150px" Visible="false"></asp:TextBox>
                        </td>
                        <td align="right" class="style8">
                            <asp:Label ID="Label1" runat="server" CssClass="Generallabel" Font-Bold="True"
                                Text="Total Deduction :" ></asp:Label>
                            &nbsp;
                            </td>
                        <td class="style7">
                            <asp:TextBox ID="txtTotalDeductions" runat="server" CssClass="txtbox" 
                                MaxLength="40" Width="150px" ></asp:TextBox>
                        </td>






                        <td align="right">
                            <asp:Label ID="Label36" runat="server" CssClass="Generallabel" Font-Bold="True" 
                                Text="Cash Paid To Driver :" ></asp:Label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkDriverCash" runat="server" />
                        </td>
                        <asp:Label ID="lblCCP" runat="server" Text="" Font-Size="Large" ForeColor="Red" ></asp:Label>
                        <td>
                            &nbsp;</td>
                        <td >
                            &nbsp;</td>
                    </tr>
                    <tr style="display:none">
                        <td align="right" class="style8">
                            <asp:Label ID="Label23" runat="server" CssClass="Generallabel" 
                                Text="Service Charges :"></asp:Label>
                            &nbsp;
                        </td>
                        <td class="style7">
                            <asp:TextBox ID="txtServiceCharges" runat="server" CssClass="txtbox" 
                                MaxLength="40" Width="150px"></asp:TextBox>
                        </td>
                        <td align="right">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr >
                        <td align="right" class="style8">
                            <asp:Label ID="Label24" runat="server" CssClass="Generallabel" 
                                Text="Cleaning Charges :" Visible="false"></asp:Label>
                            &nbsp;
                        </td>
                        <td class="style7">
                            <asp:TextBox ID="txtCleaningCharges" runat="server" CssClass="txtbox" 
                                MaxLength="40" Width="150px" Visible="false"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr style="display:none"  >
                        <td align="right" class="style8">
                            <asp:Label ID="Label25" runat="server" CssClass="Generallabel" 
                                Text="Hook Charges :"></asp:Label>
                            &nbsp;
                        </td>
                        <td class="style7">
                            <asp:TextBox ID="txtHookCharges" runat="server" CssClass="txtbox" 
                                MaxLength="40" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="Label26" runat="server" CssClass="Generallabel" 
                                Text="Driver Salary :" Visible="false"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDriverSalary" runat="server" CssClass="txtbox" 
                                MaxLength="40" Visible="false" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="right" class="style8">
                            <asp:Label ID="Label2912554" runat="server" CssClass="Generallabel" 
                                Text="Manual Commission :" Visible="false"></asp:Label>
                            &nbsp;
                        </td>
                        <td class="style7">
                            <asp:TextBox ID="txtBusCharges1" runat="server" CssClass="txtbox" 
                                MaxLength="40" Width="150px" Visible="false"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="right" class="style8">
                            <asp:Label ID="Label32574" runat="server" CssClass="Generallabel" 
                                Text="Toll GBS / Parchi :" Visible="false"></asp:Label>
                            &nbsp;
                        </td>
                        <td class="style7">
                            <asp:TextBox ID="txtToll1" runat="server" CssClass="txtbox" 
                                MaxLength="40" Width="150px" Visible="false"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="right" class="style8">
                            <asp:Label ID="Label44" runat="server" CssClass="Generallabel" 
                                Text="Total Fare :" Visible="false"></asp:Label>
                            &nbsp;
                        </td>
                        <td class="style7">
                            <asp:TextBox ID="txtTotalFare" runat="server" CssClass="txtbox" 
                                MaxLength="40" Width="150px" onkeyup="calculateCommission()" 
                                ReadOnly="True" Visible="false"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>

                    <tr style="display:none">
                        <td align="right" class="style8">
                            <asp:Label ID="Label45" runat="server" CssClass="Generallabel" 
                                Text="Fare Commission % :"></asp:Label>
                            &nbsp; </td>
                        <td class="style7">
                            <asp:TextBox ID="txtFareCommission" runat="server" CssClass="txtbox" 
                                MaxLength="40" Width="150px" onkeyup="calculateCommission()"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr style="display:none">
                        <td align="right" class="style8">
                            <asp:Label ID="Label46" runat="server" CssClass="Generallabel" 
                                Text="Calculated Commission :"></asp:Label>
                            &nbsp; </td>
                        <td class="style7">
                            <asp:TextBox ID="txtCalculatedCommission" runat="server" CssClass="txtbox" 
                                MaxLength="40" ReadOnly="True" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="right" class="style8">
                            &nbsp;</td>
                        <td class="style7">
                            &nbsp;</td>
                        <td id="Td1" runat="server">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td id="Td2" runat="server">
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
</table>

                                                        
                                                        
                                                        
<script type="text/javascript" >

    function windowOpen() {




        //var win = window,
        //    doc = document,
        //    docElem = doc.documentElement,
        //    body = doc.getElementsByTagName('body')[0],
        //    x = win.innerWidth || docElem.clientWidth || body.clientWidth,
        //    y = win.innerHeight || docElem.clientHeight || body.clientHeight;
        ////alert(x + ' × ' + y);



        //myWindow = window.open('VoucherPreview.aspx', '_blank', 'width=' + x + ',height=' + y + ', scrollbars=yes,resizable=no')

        var params = [
            'height=' + screen.height,
            'width=' + screen.width,
            'fullscreen=yes' // only works in IE, but here for completeness
        ].join(',');
        // and any other options from
    

       // var popup = window.open('VoucherPreview.aspx', 'popup_window', params);
        var popup = window.open('VoucherPreview.aspx?TSID=' + '<% =hidTSID.Value %>', 'popup_window', params);

        popup.moveTo(0, 0);


        //myWindow = window.open('VoucherPreview.aspx', '_blank', 'height=1600,width=1800,resizable=yes,scrollbars=yes,toolbar=yes,menubar=yes,location=yes')

        //myWindow.focus()
    }


    function disable() {

    }

    function calculateCargoCommission() {
      
        CargoCash = document.getElementById("txtcargocash").value;
        CargoCommission = document.getElementById("txtcargocommission").value;

        Calculated = CargoCash - CargoCommission;
        document.getElementById("txtcargoIncom").value = Calculated;
   

    }


    function calculateCommission() {
        Actual = parseInt(document.getElementById("hidTotal").value);
        Percentage = document.getElementById("txtFareCommission").value;
        
        Calculated = (Percentage * Actual) / 100;
        document.getElementById("txtCalculatedCommission").value = Calculated;
        updateTotal();
        
    }
    function validateDiscount() {
        var maxDiscount = document.getElementById("txtMaxDiscount").value;
        var discountGain = document.getElementById("txtDiscountGain").value;
        if (+discountGain > +maxDiscount) {
            alert("Maximum discount is " + maxDiscount + ".");
            document.getElementById("txtDiscountGain").value = "";
            return false;
        }
        else if (+discountGain < 0) {
            alert("Invalid input");
            document.getElementById("txtDiscountGain").value = "";
            return false;
        }
        else {
            return true;
        }
    }
    function codeVerification() {
        var verificationCodeDB = document.getElementById("txtVerificationCodeDB").value;
        var verificationCode = document.getElementById("txtVerificationCode").value;
        if (verificationCodeDB == verificationCode) {
            return true;
        }
        else {
            alert("Invalid Code");
            document.getElementById("txtVerificationCode").value = "";
            document.getElementById("txtDiscountGain").focus();
            return false;
        }
        
    }
    function EnableTextBox(chk) {
        if (chk.checked == true) {
            document.getElementById("txtVerificationCode").disabled = false;
            document.getElementById("txtDiscountGain").disabled = false;
        }
        else {
            document.getElementById("txtVerificationCode").disabled = true;
            document.getElementById("txtDiscountGain").disabled = true;
        }
    }
</script>

<script type="text/javascript" >
    (function($) {
        $.widget("ui.cmbControlerNumber", {
            _create: function() {
                var self = this,
                    select = this.element.hide(),
                    selected = select.children(":selected"),
                    value = selected.val() ? selected.text() : "";
                var input = this.input = $("<input>")
                    .insertAfter(select)
                    .val(value)
                    .autocomplete({
                        delay: 0,
                        minLength: 0,
                        source: function(request, response) {
                            var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
                            response(select.children("option").map(function() {
                                var text = $(this).text();
                                if (this.value && (!request.term || matcher.test(text)))
                                    return {
                                        label: text.replace(
                                            new RegExp(
                                                "(?![^&;]+;)(?!<[^<>]*)(" +
                                                $.ui.autocomplete.escapeRegex(request.term) +
                                                ")(?![^<>]*>)(?![^&;]+;)", "gi"
                                            ), "<strong>$1</strong>"),
                                        value: text,
                                        option: this
                                    };
                            }));
                        },
                        select: function(event, ui) {


                            ui.item.option.selected = true;
                            self._trigger("selected", event, {
                                item: ui.item.option
                            });
                            //  alert('i am here');									
                            __doPostBack('cmbVehicle', '')

                        },
                        change: function(event, ui) {
                            if (!ui.item) {
                                var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($(this).val()) + "$", "i"),
                                    valid = false;

                                select.children("option").each(function() {
                                    if ($(this).text().match(matcher)) {

                                        this.selected = valid = true;

                                        return false;
                                    }

                                });
                                if (!valid) {
                                    // remove invalid value, as it didn't match anything
                                    $(this).val("");
                                    select.val("");
                                    input.data("autocomplete").term = "";
                                    return false;
                                }
                            }
                        }
                    })
                    .addClass("ui-widget ui-widget-content ui-corner-left");

                input.data("autocomplete")._renderItem = function(ul, item) {
                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + item.label + "</a>")
                        .appendTo(ul);
                };

                this.button = $("<button type='button'>&nbsp;</button>")
                    .attr("tabIndex", -1)
                    .attr("title", "Show All Items")
                    .insertAfter(input)
                    .button({
                        icons: {
                            primary: "ui-icon-triangle-1-s"
                        },
                        text: false
                    })
                    .removeClass("ui-corner-all")
                    .addClass("ui-corner-right ui-button-icon")
                    .click(function() {
                        // close if already visible
                        if (input.autocomplete("widget").is(":visible")) {
                            input.autocomplete("close");
                            return;
                        }

                        // work around a bug (likely same cause as #5265)
                        $(this).blur();

                        // pass empty string as value to search for, displaying all results
                        input.autocomplete("search", "");
                        input.focus();
                    });
            },

            destroy: function() {
                this.input.remove();
                this.button.remove();
                this.element.show();
                $.Widget.prototype.destroy.call(this);
            }
        });
    })(jQuery);

    $(function() {
   $("#cmbControlerNumber").cmbVehicle();

        $("#toggle").click(function() {
            
        
          $("#cmbControlerNumber").toggle();
    

        });
    });

    
   
</script>


<script type="text/javascript" >
    (function($) {
        $.widget("ui.cmbVehicle", {
            _create: function() {
                var self = this,
                    select = this.element.hide(),
                    selected = select.children(":selected"),
                    value = selected.val() ? selected.text() : "";
                var input = this.input = $("<input>")
                    .insertAfter(select)
                    .val(value)
                    .autocomplete({
                        delay: 0,
                        minLength: 0,
                        source: function(request, response) {
                            var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
                            response(select.children("option").map(function() {
                                var text = $(this).text();
                                if (this.value && (!request.term || matcher.test(text)))
                                    return {
                                        label: text.replace(
                                            new RegExp(
                                                "(?![^&;]+;)(?!<[^<>]*)(" +
                                                $.ui.autocomplete.escapeRegex(request.term) +
                                                ")(?![^<>]*>)(?![^&;]+;)", "gi"
                                            ), "<strong>$1</strong>"),
                                        value: text,
                                        option: this
                                    };
                            }));
                        },
                        select: function(event, ui) {


                            ui.item.option.selected = true;
                            self._trigger("selected", event, {
                                item: ui.item.option
                            });
                            //  alert('i am here');									
                            __doPostBack('cmbVehicle', '')

                        },
                        change: function(event, ui) {
                            if (!ui.item) {
                                var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($(this).val()) + "$", "i"),
                                    valid = false;

                                select.children("option").each(function() {
                                    if ($(this).text().match(matcher)) {

                                        this.selected = valid = true;

                                        return false;
                                    }

                                });
                                if (!valid) {
                                    // remove invalid value, as it didn't match anything
                                    $(this).val("");
                                    select.val("");
                                    input.data("autocomplete").term = "";
                                    return false;
                                }
                            }
                        }
                    })
                    .addClass("ui-widget ui-widget-content ui-corner-left");

                input.data("autocomplete")._renderItem = function(ul, item) {
                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + item.label + "</a>")
                        .appendTo(ul);
                };

                this.button = $("<button type='button'>&nbsp;</button>")
                    .attr("tabIndex", -1)
                    .attr("title", "Show All Items")
                    .insertAfter(input)
                    .button({
                        icons: {
                            primary: "ui-icon-triangle-1-s"
                        },
                        text: false
                    })
                    .removeClass("ui-corner-all")
                    .addClass("ui-corner-right ui-button-icon")
                    .click(function() {
                        // close if already visible
                        if (input.autocomplete("widget").is(":visible")) {
                            input.autocomplete("close");
                            return;
                        }

                        // work around a bug (likely same cause as #5265)
                        $(this).blur();

                        // pass empty string as value to search for, displaying all results
                        input.autocomplete("search", "");
                        input.focus();
                    });
            },

            destroy: function() {
                this.input.remove();
                this.button.remove();
                this.element.show();
                $.Widget.prototype.destroy.call(this);
            }
        });
    })(jQuery);

    $(function() {
        $("#cmbVehicle").cmbVehicle();
        $("#toggle").click(function() {
            $("#cmbVehicle").toggle();
        });
    });


$(function Checkbus() {

    var controlerNo =  $("#cmbControlerNumber option:selected").text();
 
    $.ajax({

type:'GET',
url:'https://prod.faisalmovers.co:8080/ords/wms/sch/schedule?VEHICLE_ID='+controlerNo,
success:function(sch){

$.each(sch, function(i,schi){
  // console.log(schi);
 
    document.getElementById("txtCCNumber").value = sch.ccp_no;
    document.getElementById("txtDriverName").value = sch.driver;
    var BusNo = document.getElementById("txtVehicle").value;


    if (BusNo == null || BusNo == "Advance") {
        document.getElementById("txtVehicle").value = sch.vehicle_id;
    }
    var BusNumber = document.getElementById("TextBox1").value;

    if (BusNumber == null || BusNumber == "Advance") {
        document.getElementById("TextBox1").value = sch.vehicle_id;
        document.getElementById("txtVehicle").value = sch.vehicle_id;
    }

})


}
})
  
})


  
</script>


<style>
ul { height:150px;overflow:scroll }

</style>


    </igmisc:WebAsyncRefreshPanel>
</form>
</body>
</html>
