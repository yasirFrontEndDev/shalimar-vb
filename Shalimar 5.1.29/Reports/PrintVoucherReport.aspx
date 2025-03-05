<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PrintVoucherReport.aspx.vb" Inherits="FMovers.Ticketing.UI.PrintVoucherReport" %>
<%@ Register assembly="CrystalDecisions.Web, Version=12.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<HTML>
	<HEAD>
		<title>Report1</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../Styles/ReportStyles.css" type="text/css" rel="stylesheet">
		<style>
		#CRV_ctl02
		{
			visibility: visible !important;
			}

		</style>
		
	</HEAD>
	<body MS_POSITIONING="GridLayout" onmousedown="somefunction();">
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
                        <asp:Button ID="btnPrint" runat="server" Text="Print" />
                        <asp:Button ID="Button1"  runat="server" Text="Test" />                        
                    </TD>
				</TR>

				<TR>
					<TD align="center">
					<div id="printReady">
					    <CR:CrystalReportViewer ID="CRV" runat="server" 
                            AutoDataBind="true" PrintMode="ActiveX" />
                            </div>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
	<script language="javascript" type="text/javascript" >
	    function cleanupCR() {
	        alert("Closing");
	        return false;
	        //__doPostBack('', 'DisposeOfCR');
	    }

</script>
   
<script>

    window.close();
 

    var isClose = false;
    //this code will handle the F5 or Ctrl+F5 key+
    //need to handle more cases like ctrl+R whose codes are not listed here
    document.onkeydown = checkKeycode
    window.onbeforeunload = doUnload;
    function checkKeycode(e) {
        var keycode;
        if (window.event)
            keycode = window.event.keyCode;
        else if (e)
            keycode = e.which;
        if (keycode == 116) {
            isClose = true;
        }
    }

    function somefunction() {
        isClose = true;
    }

    function doUnload() {
        if (!isClose) {
            window.location = "LogOut.aspx"
        }
    }
</script>
</HTML>
