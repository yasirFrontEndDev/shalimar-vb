<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DailyDepartureReportSummary.aspx.vb" Inherits="FMovers.Ticketing.UI.DailyDepartureReportSummary" %>
<%@ Register assembly="CrystalDecisions.Web, Version=12.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<%@ Register assembly="CrystalDecisions.Web" namespace="CrystalDecisions.Web" tagprefix="CR" %>
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

<CR:CrystalReportViewer ID="CRV" runat="server" 
                            AutoDataBind="true" />       			
		</form>
	</body>
</HTML>
