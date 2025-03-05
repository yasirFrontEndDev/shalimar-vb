<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdvanceReport.aspx.vb" Inherits="FMovers.Ticketing.UI.AdvanceReport"  MasterPageFile="~/main.Master"  %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"  runat="server">

<%@ Register tagprefix="igsch" namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics35.WebUI.WebDateChooser.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igmisc" namespace="Infragistics.WebUI.Misc" Assembly="Infragistics35.WebUI.Misc.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>



    <script language=javascript >



        function getParameterByName(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
               results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));

        }
        
        function loadreport() {
            var foo = getParameterByName('type');

            var Type = document.getElementById("ctl00_ContentPlaceHolder1_cboType").value;
            var cmbSource1 = document.getElementById("ctl00_ContentPlaceHolder1_cmbSource");
            var cmbUser = document.getElementById("ctl00_ContentPlaceHolder1_cboUsers");

            if (foo == null) {
                window.open('Reports/AD.aspx?from=' + document.getElementById("ctl00_ContentPlaceHolder1_dtFrom_input").value + "&to=" + document.getElementById("ctl00_ContentPlaceHolder1_dtTo_input").value + "&Dfrom=" + document.getElementById("ctl00_ContentPlaceHolder1_txtTimeFrom").value + "&Dto=" + document.getElementById("ctl00_ContentPlaceHolder1_txtTimeTo").value + "&Type=" + Type + "&SID=" + cmbSource1.options[cmbSource1.selectedIndex].value);
                return false;
            }

     
            
            if (Type == "Log") {
                window.open('Reports/AD.aspx?from=' + document.getElementById("ctl00_ContentPlaceHolder1_dtFrom_input").value + "&to=" + document.getElementById("ctl00_ContentPlaceHolder1_dtTo_input").value + "&Dfrom=" + document.getElementById("ctl00_ContentPlaceHolder1_txtTimeFrom").value + "&Dto=" + document.getElementById("ctl00_ContentPlaceHolder1_txtTimeTo").value + "&Type=" + Type + "&SID=" + cmbSource1.options[cmbSource1.selectedIndex].value);
            }
            else {
          
                window.open('Reports/PreviousReport.aspx?from=' + document.getElementById("ctl00_ContentPlaceHolder1_dtFrom_input").value + "&to=" + document.getElementById("ctl00_ContentPlaceHolder1_dtTo_input").value + "&Dfrom=" + document.getElementById("ctl00_ContentPlaceHolder1_txtTimeFrom").value + "&Dto=" + document.getElementById("ctl00_ContentPlaceHolder1_txtTimeTo").value + "&Type=" + Type + "&SID=" + cmbSource1.options[cmbSource1.selectedIndex].value + "&UserId=" + cmbUser.options[cmbUser.selectedIndex].value);
            } 
           // 

            return false;

        }
    </script>

    <link href="styles/styles.css" rel="stylesheet" type="text/css" />
    <script src="script/scriptlib.js" type="text/javascript"></script>   

    <div class="col-lg-12">
  <div class="topRow">
  

    <table width="600px" align="center" border="0" class="TableBorder" cellspacing="0">
         <tr>
            <td align="center" class="HeaderStyle">Advance Report</td>
            
        </tr>
        <tr>
            <td align="center" valign="top">
              <igmisc:webasyncrefreshpanel ID="warpRefresh" runat="server">
                                                        <table cellpadding="2" cellspacing="1" width="90%">
                                                            <tr>
                                                                <td align="right" width="150" height="5px"></td>
                                                                <td></td>
                                                            </tr>
                                                            
<tr>
                                                                <td align="right" width="150"><asp:Label ID="Label6" runat="server" Text="Schedule :" 
                                                                        CssClass="Generallabel"></asp:Label>&nbsp;</td>
                                                                <td>
                                                                  <asp:DropDownList ID="cmbSource"  onclick ="" runat="server" Width="255px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>                                                            
                                                            <tr>
                                                                <td align="right" width="150"><asp:Label ID="Label1" runat="server" Text="From:" 
                                                                        CssClass="Generallabel"></asp:Label>&nbsp;</td>
                                                                <td>
                                                                    <igsch:webdatechooser ID="dtFrom" runat="server" Width="235px" 
                                                                        NullDateLabel="Select Date" AllowNull="False" Editable="False" 
                                                                        MinDate="2010-03-01">
                                                                        <AutoPostBack ValueChanged="True" />
                                                                        <DropButton>
                                                                            <Style Font-Names="Verdana">
                                                                            </Style>
                                                                        </DropButton>
                                                                        <CalendarLayout>
                                                                            <CalendarStyle BackColor="#F9FAFF" Font-Names="Verdana" Font-Size="10pt">
                                                                            </CalendarStyle>
                                                                            <DayHeaderStyle BackColor="#6699FF" />
                                                                        </CalendarLayout>
                                                                    </igsch:webdatechooser>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" width="150"><asp:Label ID="Label3" runat="server" Text="To:" 
                                                                        CssClass="Generallabel"></asp:Label></td>
                                                                <td>
                                                                    <igsch:webdatechooser ID="dtTo" runat="server" Width="235px" 
                                                                        NullDateLabel="Select Date" AllowNull="False" Editable="False" 
                                                                        MinDate="2010-03-01">
                                                                        <AutoPostBack ValueChanged="True" />
                                                                        <DropButton>
                                                                            <Style Font-Names="Verdana">
                                                                            </Style>
                                                                        </DropButton>
                                                                        <CalendarLayout>
                                                                            <CalendarStyle BackColor="#F9FAFF" Font-Names="Verdana" Font-Size="10pt">
                                                                            </CalendarStyle>
                                                                            <DayHeaderStyle BackColor="#6699FF" />
                                                                        </CalendarLayout>
                                                                    </igsch:webdatechooser>
                                                                </td>
                                                            </tr>
                                                            <tr style="display:none" >
                                                                <td align="right" width="150" height="5px">
                                                                    <asp:Label ID="Label4" runat="server" Text="User Name:" 
                                                                        CssClass="Generallabel"></asp:Label>&nbsp;</td>
                                                                <td>
                                                                    <asp:DropDownList ID="cboUser" Visible="false" runat="server" AutoPostBack="True" 
                                                                        Width="235px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            
                                                            <tr style="display:none" >
                                                                <td align="right" width="150" height="5px">
                                                                    <asp:Label ID="Label2" runat="server" Text="Time :" 
                                                                        CssClass="Generallabel"></asp:Label>&nbsp;</td>
                                                                <td>
                                                                    <asp:TextBox ID="txtTimeFrom"  Width="50px" runat="server"></asp:TextBox>  <asp:TextBox ID="txtTimeTo"  Width="50px" runat="server"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" height="5px" width="150">
                                                                    <asp:Label ID="Label7" runat="server" CssClass="Generallabel" Text="Users :"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    
                                                                     <asp:DropDownList ID="cboUsers" runat="server" AutoPostBack="True" 
                                                                        Width="235px">
                                                                    </asp:DropDownList>
                                                                    </td>
                                                            </tr>
 <tr>
                                                                <td align="right" width="150" height="5px">
                                                                    <asp:Label ID="Label5" runat="server" Text="Report Type :" 
                                                                        CssClass="Generallabel"></asp:Label>&nbsp;</td>
                                                                <td >
                                                                     <asp:DropDownList ID="cboType"  runat="server" Width="238px" >
                    <asp:ListItem Value="1">Current Advance</asp:ListItem>
                    <asp:ListItem Value="3">Phone Booking  </asp:ListItem>
                    <asp:ListItem Value="Log">Previous Advance</asp:ListItem>
                    <asp:ListItem Value="4">Missed</asp:ListItem>
                    <asp:ListItem Value="5">Refund </asp:ListItem>
                    <asp:ListItem Value="6">Ticket Change </asp:ListItem>
                    <asp:ListItem Value="7">Next Departure</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                    
                                                                </td>
                                                            </tr>
                                                        </table>
                      </igmisc:webasyncrefreshpanel>
                                                        
           </tdh
       </tr>
       <tr>
            <td align="right" class="">
               </td>
       </tr>
       <tr>
            <td align="right" class="">
                <asp:HiddenField ID="hndType" runat="server" />
                <asp:Button ID="btnShowReport" runat="server" Text="Show Report"  OnClientClick="return loadreport();" CssClass="ButtonStyle" Width="115px" />&nbsp; </td>
       </tr>
       <tr>
            <td align="right" height="3px"></td>
       </tr>
    </table>


</div>

</div>
</asp:Content>


