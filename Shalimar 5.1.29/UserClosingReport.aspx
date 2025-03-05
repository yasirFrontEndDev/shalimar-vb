<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserClosingReport.aspx.vb" Inherits="FMovers.Ticketing.UI.UserClosingReport1"   MasterPageFile="~/main.Master"  %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"  runat="server">



<%@ Register tagprefix="igsch" namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics35.WebUI.WebDateChooser.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igmisc" namespace="Infragistics.WebUI.Misc" Assembly="Infragistics35.WebUI.Misc.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

    <script language=javascript >

        function loadreport() {

     
            var Type = document.getElementById("ctl00_ContentPlaceHolder1_cboDate");
            var Values = Type.options[Type.selectedIndex].value;


            window.open('Reports/CommissionReport.aspx?BookId=' + Values + "&UserId=" + document.getElementById("ctl00_ContentPlaceHolder1_UserId").value);

            return false;


        }

        function loadreportUnClosed() {

            var Type = document.getElementById("ctl00_ContentPlaceHolder1_cboDate");
            var Values = Type.options[Type.selectedIndex].value;
            Values = 0;

            window.open('Reports/CommissionReportUnClosed.aspx?BookId=' + Values + "&UserId=" + document.getElementById("ctl00_ContentPlaceHolder1_UserId").value);

            return false;


        }


    </script>

    <link href="styles/styles.css" rel="stylesheet" type="text/css" />
    <script src="script/scriptlib.js" type="text/javascript"></script>  
    <div class="col-lg-12">
  <div class="topRow">
    <table width="600px" align="center" border="0" class="TableBorder" cellspacing="0">
         <tr>
            <td align="center" class="HeaderStyle">Commission Report</td>
            
        </tr>
        <tr>
            <td align="center" valign="top">
              <igmisc:WebAsyncRefreshPanel ID="warpRefresh" runat="server">
                                                        <table cellpadding="2" cellspacing="1" width="90%">
                                                            <tr>
                                                                <td align="right" width="150" height="5px"></td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" width="150">
                                                                    <asp:Label ID="Label1" runat="server" Text="Closing Date :" 
                                                                        CssClass="Generallabel"></asp:Label>&nbsp;</td>
                                                                <td align="left">
                                                                    <asp:DropDownList ID="cboDate" runat="server" Width="150px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                      </igmisc:WebAsyncRefreshPanel>
                                                        
           </td>
       </tr>
       <tr>
            <td align="right" class="">
                <asp:HiddenField ID="UserId" runat="server" />
                <asp:Button ID="btnShowReport" runat="server" Text="Show Report"  
                    OnClientClick="return loadreport();" CssClass="ButtonStyle" Width="137px" />&nbsp;
                <asp:Button ID="btnShowUnClosed" runat="server" Text="Un closed Report"  
                    OnClientClick="return loadreportUnClosed();" CssClass="ButtonStyle" 
                    Width="178px" />&nbsp; </td>
       </tr>
       <tr>
            <td align="right" height="3px"></td>
       </tr>
    </table>

</div>

</div>
</asp:Content>