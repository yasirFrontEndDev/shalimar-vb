<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CNIC.aspx.vb" Inherits="FMovers.Ticketing.UI.CNIC" %>

<%@ Register assembly="CrystalDecisions.Web, Version=12.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<script language="javascript" type="text/javascript">
    function printDiv() {

        var printContent = document.getElementById('printReady');
        var windowUrl = 'about:blank';
        var uniqueName = new Date();
        var windowName = 'Print' + uniqueName.getTime();
        var printWindow = window.open();
        printWindow.document.write(printContent.innerHTML());
        printWindow.focus();
        printWindow.print();
        printWindow.close();
    
        return false ;


    }
</script>
	<style>
		#CRV_ctl02
		{
			visibility: visible !important;
			}
		</style>
		
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
                        <asp:Button ID="btnPrint" OnClientClick ="javascript:printDiv();" runat="server" Text="Print" />
                        <asp:Button ID="Button1" OnClientClick ="javascript:printDiv();" runat="server" Text="Print" />                        
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
</HTML>
