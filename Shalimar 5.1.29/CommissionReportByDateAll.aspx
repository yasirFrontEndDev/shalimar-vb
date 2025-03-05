<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CommissionReportByDateAll.aspx.vb" Inherits="FMovers.Ticketing.UI.CommissionReportByDateAll"  MasterPageFile="~/main.Master" %>




<%@ Register tagprefix="igsch" namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics35.WebUI.WebDateChooser.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igmisc" namespace="Infragistics.WebUI.Misc" Assembly="Infragistics35.WebUI.Misc.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"  runat="server">

    <div class="col-lg-12">
  <div class="topRow">


    <script language=javascript >

    function loadreport() {
      


        var Route = document.getElementById("ctl00_ContentPlaceHolder1_cboRoute");
        var Users = document.getElementById("ctl00_ContentPlaceHolder1_cboUser");
        var Vehicle = document.getElementById("ctl00_ContentPlaceHolder1_cboVehicle");
        var Type = document.getElementById("ctl00_ContentPlaceHolder1_cboReportType");
        window.open('Reports/ComissionReportNew.aspx?dfrom=' + document.getElementById("ctl00_ContentPlaceHolder1_dtFrom_input").value + "&to=" + document.getElementById("ctl00_ContentPlaceHolder1_dtTo_input").value + '&Route=' + Route.options[Route.selectedIndex].value + '&Users=' + Users.options[Users.selectedIndex].value + '&Vehicle=' + Vehicle.options[Vehicle.selectedIndex].value + '&Type=' + Type.options[Type.selectedIndex].value);

         return false;


    }
    
    </script>

    <link href="styles/styles.css" rel="stylesheet" type="text/css" />
    <script src="script/scriptlib.js" type="text/javascript"></script>   

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
                                                                <td class="style1">&nbsp;</td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" width="150"><asp:Label ID="Label1" runat="server" Text="From:" 
                                                                        CssClass="Generallabel"></asp:Label>&nbsp;</td>
                                                                <td class="style1">
                                                                    &nbsp;</td>
                                                                <td>
                                                                    <igsch:WebDateChooser ID="dtFrom" runat="server" AllowNull="False" 
                                                                        Editable="False" MinDate="2010-03-01" NullDateLabel="Select Date" Width="235px">
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
                                                                    </igsch:WebDateChooser>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" width="150"><asp:Label ID="Label3" runat="server" Text="To:" 
                                                                        CssClass="Generallabel"></asp:Label></td>
                                                                <td class="style1">
                                                                    &nbsp;</td>
                                                                <td>
                                                                    <igsch:WebDateChooser ID="dtTo" runat="server" AllowNull="False" 
                                                                        Editable="False" MinDate="2010-03-01" NullDateLabel="Select Date" Width="235px">
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
                                                                    </igsch:WebDateChooser>
                                                                </td>
                                                            </tr>
                                                            <tr  >
                                                                <td align="right" width="150" height="5px">
                                                                    <asp:Label ID="Label4" runat="server" Text="User :" 
                                                                        CssClass="Generallabel"></asp:Label>&nbsp;</td>
                                                                <td class="style1">
                                                                    &nbsp;</td>
                                                                <td>
                                                                    <asp:DropDownList ID="cboUser" runat="server" AutoPostBack="True" 
                                                                        Visible="True" Width="235px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            
                                                            <tr >
                                                                <td align="right" height="5px" width="150">
                                                                    <asp:Label ID="Label5" runat="server" CssClass="Generallabel" Text="Vehicle :"></asp:Label>
                                                                </td>
                                                                <td class="style1">
                                                                    &nbsp;</td>
                                                                <td>
                                                                    <asp:DropDownList ID="cboVehicle" runat="server" AutoPostBack="True" 
                                                                        Visible="True" Width="235px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr >
                                                                <td align="right" height="5px" width="150">
                                                                    <asp:Label ID="Label6" runat="server" CssClass="Generallabel" Text="Route :"></asp:Label>
                                                                </td>
                                                                <td class="style1">
                                                                    &nbsp;</td>
                                                                <td>
                                                                    <asp:DropDownList ID="cboRoute" runat="server" AutoPostBack="True" 
                                                                        Visible="True" Width="235px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" height="5px" width="150">
                                                                    <asp:Label ID="Label7" runat="server" CssClass="Generallabel" 
                                                                        Text="Report Type : "></asp:Label>
                                                                </td>
                                                                <td class="style1">
                                                                    &nbsp;</td>
                                                                <td>
                                                                    <asp:DropDownList ID="cboReportType" runat="server" 
                                                                        Visible="True" Width="235px">
                                                                        <asp:ListItem Value="1">Detail</asp:ListItem>
                                                                        <asp:ListItem Value="2">Summary</asp:ListItem>
                                                                        <asp:ListItem Value="3">User Wise Closing Summary</asp:ListItem>                                                                        
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                      </igmisc:WebAsyncRefreshPanel>
                                                        
           </td>
       </tr>
       <tr>
            <td align="right" class="">
                <asp:Button ID="btnShowReport" runat="server" Text="Show Report"  OnClientClick="return loadreport();" CssClass="ButtonStyle" Width="130px" />&nbsp; </td>
       </tr>
       <tr>
            <td align="right" height="3px"></td>
       </tr>
    </table>
</div>

</div>
</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="head">

    <style type="text/css">
        .style1
        {
            width: 40px;
        }
    </style>

</asp:Content>

