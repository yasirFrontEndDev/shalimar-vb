<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BS.aspx.vb" Inherits="FMovers.Ticketing.UI.BS" %>

<%@ Register assembly="CrystalDecisions.Web, Version=12.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
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
				<TR>
					<TD align="center">
				        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
                            AutoDataBind="true" />
                    </TD>
				</TR>
			</TABLE>
		        
		</form>
	</body>
</HTML>
