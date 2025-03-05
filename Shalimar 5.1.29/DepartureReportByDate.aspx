<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DepartureReportByDate.aspx.vb" Inherits="FMovers.Ticketing.UI.DepartureReportByDate" MasterPageFile="~/main.Master"  %>

<%@ Register tagprefix="igsch" namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics35.WebUI.WebDateChooser.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igmisc" namespace="Infragistics.WebUI.Misc" Assembly="Infragistics35.WebUI.Misc.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"  runat="server">

    <div class="col-lg-12">
  <div class="topRow">


    
    
        <link href="styles/styles.css" rel="stylesheet" type="text/css" />
    <script src="script/scriptlib.js" type="text/javascript"></script>   
     <script language=javascript >
      function loadreport() {

          //         alert( document.getElementById("dtFrom_input").value    );
          //         
          //            alert( document.getElementById("cboList").value    );
          //window.open('Reports/DailyDepartureReport.aspx?from=' + document.getElementById("ctl00_ContentPlaceHolder1_dtFrom_input").value + "&to=" + document.getElementById("ctl00_ContentPlaceHolder1_dtTo_input").value + "&Dfrom=" + document.getElementById("ctl00_ContentPlaceHolder1_txtTimeFrom").value + "&Dto=" + document.getElementById("txtTimeTo").value + "&Type=0&Route=" + document.getElementById("ctl00_ContentPlaceHolder1_cboRoute").value);
          window.open('Reports/DailyDepartureReport.aspx?from=' + document.getElementById("ctl00_ContentPlaceHolder1_dtFrom_input").value + "&to=" + document.getElementById("ctl00_ContentPlaceHolder1_dtTo_input").value + "&Dfrom=" + document.getElementById("ctl00_ContentPlaceHolder1_txtTimeFrom").value + "&Dto=" + document.getElementById("ctl00_ContentPlaceHolder1_txtTimeTo").value + "&Type=0&Route=" + document.getElementById("ctl00_ContentPlaceHolder1_cboRoute").value + "&ServiceType_id=" + document.getElementById("ctl00_ContentPlaceHolder1_cboService").value);
          return false;


      }

      function loadreportSummary() {

          //         alert( document.getElementById("dtFrom_input").value    );
          //         
          //            alert( document.getElementById("cboList").value    );
          window.open('Reports/DailyDepartureReport.aspx?from=' + document.getElementById("ctl00_ContentPlaceHolder1_dtFrom_input").value + "&to=" + document.getElementById("ctl00_ContentPlaceHolder1_dtTo_input").value + "&Dfrom=" + document.getElementById("ctl00_ContentPlaceHolder1_txtTimeFrom").value + "&Dto=" + document.getElementById("ctl00_ContentPlaceHolder1_txtTimeTo").value + "&Type=1&Route=" + document.getElementById("ctl00_ContentPlaceHolder1_cboRoute").value + "&ServiceType_id=" + document.getElementById("ctl00_ContentPlaceHolder1_cboService").value);

          return false;


      }
      </script>
      
    <table width="600px" align="center" border="0" class="TableBorder" cellspacing="0">
         <tr>
            <td align="center" class="HeaderStyle">Departure Report</td>
            
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
                                                                <td align="right" width="150"><asp:Label ID="Label1" runat="server" Text="From:" 
                                                                        CssClass="Generallabel"></asp:Label>&nbsp;</td>
                                                                <td>
                                                                    <igsch:WebDateChooser ID="dtFrom" runat="server" Width="235px" 
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
                                                                    </igsch:WebDateChooser>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" width="150"><asp:Label ID="Label3" runat="server" Text="To:" 
                                                                        CssClass="Generallabel"></asp:Label></td>
                                                                <td>
                                                                    <igsch:WebDateChooser ID="dtTo" runat="server" Width="235px" 
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
                                                                    </igsch:WebDateChooser>
                                                                </td>
                                                            </tr>
                                                            <tr  >
                                                                <td align="right" width="150" height="5px">
                                                                    <asp:Label ID="Label6" runat="server" CssClass="Generallabel" Text="Service:"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:DropDownList ID="cboService" runat="server" AutoPostBack="True" 
                                                                        Width="235px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            
                                                            <tr>
                                                                <td align="right" height="5px" width="150">
                                                                    <asp:Label ID="Label4" runat="server" CssClass="Generallabel" Text="Route:"></asp:Label>
                                                                    &nbsp;</td>
                                                                <td>
                                                                    <asp:DropDownList ID="cboRoute" runat="server" AutoPostBack="True" 
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
                                                                <td align="right" width="150" height="5px">
                                                                    <asp:Label ID="Label5" runat="server" Text="Time :" 
                                                                        CssClass="Generallabel"></asp:Label>&nbsp;</td>
                                                                <td>
                                                                    <asp:DropDownList ID="cboList" runat="server">
                                                                     <asp:ListItem Text="By Date" Value=0></asp:ListItem>
                                                                     <asp:ListItem Text="By Transaction" Value=1></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>   
                                                            
                                                            
                                                            <tr>
                                                                <td align="right" height="5px" width="150">
                                                                    &nbsp;</td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                            
                                                            
                                                        </table>
                      </igmisc:WebAsyncRefreshPanel>
                                                        
           </td>
       </tr>
       <tr> 
<td align="right" class="" colspan="2">
                <asp:Button ID="btnShowReport" runat="server" Text="Show Report"  
                    OnClientClick="return loadreport();" CssClass="ButtonStyle" Width="150px" />&nbsp;
                <asp:Button ID="btnShowReportSummary" runat="server" 
                    Text="Show Report Summary"  OnClientClick="return loadreportSummary();" 
                    CssClass="ButtonStyle" Width="250px" />&nbsp;                 
                </td>
       </tr>
       <tr>
            <td  align="right" height="3px"></td>
       </tr>
    </table>
 
</div>

</div>
</asp:Content>