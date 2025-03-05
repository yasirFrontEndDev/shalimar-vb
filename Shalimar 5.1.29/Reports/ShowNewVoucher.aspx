<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ShowNewVoucher.aspx.vb" Inherits="FMovers.Ticketing.UI.ShowNewVoucher" %>

<%@ Register assembly="CrystalDecisions.Web, Version=12.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <CR:CrystalReportViewer ID="CRV" runat="server" 
            AutoDataBind="true" />
    
    </div>
    </form>
</body>
</html>
