<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PrintVoucherSmartReport.aspx.vb" Inherits="FMovers.Ticketing.UI.PrintVoucherSmartReport" %>
<%@ Register assembly="CrystalDecisions.Web, Version=12.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<script language="javascript" type="text/javascript">

   


    function printDiv() {



        //var printContent = document.getElementsByClassName('dialogzone')[0];


        //var windowUrl = 'about:blank';
        //var uniqueName = new Date();
        //var windowName = 'Print' + uniqueName.getTime();
        //var printWindow = window.open();
        //printWindow.document.write(printContent.innerHTML);
        //printWindow.focus();
        //printWindow.print();
        //printWindow.close();
    
        //return false ;


    }
    var gAutoPrint = true; // Flag for whether or not to automatically call the print function 
    function printSpecial() {
        if (document.getElementById != null) {
            var html = '< H T M L >\n< H E A D >\n';
            if (document.getElementsByTagName != null) {
                var headTags = document.getElementsByTagName("head");
                if (headTags.length > 0) html += headTags[0].innerHTML;
            }
            if (gAutoPrint) {
                if (navigator.appName == "Microsoft Internet Explorer") {
                    html += '\n</ H E A D >\n<'
                    html += 'B O D Y onLoad="PrintCommandObject.ExecWB(6, -1);">\n';
                }
                else {
                    html += '\n</ H E A D >\n< B O D Y >\n';
                } 
            }
            else {
                html += '\n</ H E A D >\n< B O D Y >\n';
            }

            var printReadyElem = document.getElementById("printReady");
            if (printReadyElem != null) {
                html += printReadyElem.innerHTML;
            }
            else {
                alert("Could not find the printReady section in the HTML");
                return;
            }
            if (gAutoPrint) {
                if (navigator.appName == "Microsoft Internet Explorer") {
                    html += '< O B J E C T ID="PrintCommandObject" WIDTH=0 HEIGHT=0 '
                    html += 'CLASSID="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2"></ O B J E C T >\n</ B O D Y >\n</ H T M L >';
                }
                else {
                    html += '\n</ B O D Y >\n</ H T M L >';
                } 
            }
            else {
                html += '\n</ B O D Y >\n</ H T M L >';
            }
            var printWin = window.open("", "printSpecial");
            printWin.document.open();
            printWin.document.write(html);
            printWin.document.close();
            if (gAutoPrint) {
                if (navigator.appName != "Microsoft Internet Explorer") {
                    printWin.print();
                } 
            } 
        }
        else {
            alert("Sorry, the print ready feature is only available in modern browsers.");
        } 
    } 

    $(window).on('load', function () {
        $("#cboPrints").hide();
    })

</script>


<HTML>
	<HEAD>
		<title>Report1</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../Styles/ReportStyles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="100%" border="0" runat="server">
				<%--<TR>
					<TD align="right">
						<asp:button id="Button1" runat="server" Text="Export to Excel Format"></asp:button>&nbsp;<asp:button id="btnWord" runat="server" Text="Export to Word Format"></asp:button>&nbsp;<asp:button id="btnPDF" runat="server" Text="Export to PDF Format"></asp:button>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</TD>
				</TR>--%>
				<TR>
					<TD align="center">
                        <asp:DropDownList ID="cboPrints" runat="server">
                        </asp:DropDownList>
                       <%-- <asp:Button ID="btnPrint" runat="server" Text="Print" />--%>
                    </TD>
				</TR>

				<TR>
					<TD align="center">
					<%--<div id="printReady">
					    <CR:CrystalReportViewer ID="CRV" runat="server" 
                            AutoDataBind="true" PrintMode="ActiveX" />
                            </div>--%>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
