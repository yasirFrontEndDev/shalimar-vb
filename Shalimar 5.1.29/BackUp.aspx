<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BackUp.aspx.vb" Inherits="FMovers.Ticketing.UI.BackUp" %>

<%@ Register tagprefix="igsch" namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics35.WebUI.WebDateChooser.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igmisc" namespace="Infragistics.WebUI.Misc" Assembly="Infragistics35.WebUI.Misc.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Voucher Report</title>
    <script language=javascript >

    function loadreport()
    {
      
//         alert( document.getElementById("dtFrom_input").value    );
//         alert( document.getElementById("dtTo_input").value    );
//         
          window.open('Reports/CommissionReport.aspx?from=' + document.getElementById("dtFrom_input").value + "&to="+document.getElementById("dtTo_input").value+ "&Dfrom="+document.getElementById("txtTimeFrom").value+ "&Dto="+document.getElementById("txtTimeTo").value);

         return false;


    }
    
    </script>

    <link href="styles/styles.css" rel="stylesheet" type="text/css" />
    <script src="script/scriptlib.js" type="text/javascript"></script>   
</head>
<body>
    <form id="form1" runat="server">
    <table width="600px" align="center" border="0" class="TableBorder" cellspacing="0">
         <tr>
            <td align="center" class="HeaderStyle">Back Data Base</td>
            
        </tr>
        <tr>
            <td align="center" valign="top">
                <asp:Label ID="Label1" runat="server" CssClass="Errorlabel" 
                    Text="Back up have been done sucessfully ." Visible="False"></asp:Label>
            </td>
       </tr>
       <tr>
            <td align="center" class="">
                <asp:Button ID="btnShowReport" runat="server" Text="Back Up"  
                    OnClientClick="return loadreport();" CssClass="ButtonStyle" Width="115px" />&nbsp; </td>
       </tr>
       <tr>
            <td align="right" height="3px"></td>
       </tr>
    </table>
    </form>
</body>
</html>

