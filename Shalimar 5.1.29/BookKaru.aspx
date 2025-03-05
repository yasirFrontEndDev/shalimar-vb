<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BookKaru.aspx.vb" Inherits="FMovers.Ticketing.UI.BookKaru"  MasterPageFile="~/main.Master"  %>



<%@ Register tagprefix="igsch" namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics35.WebUI.WebDateChooser.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igmisc" namespace="Infragistics.WebUI.Misc" Assembly="Infragistics35.WebUI.Misc.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"  runat="server">

<script language=javascript >
      function loadreport() {
        //  alert(document.getElementById("ctl00_ContentPlaceHolder1_dtFrom_input").value);
          //alert(document.getElementById("ctl00_ContentPlaceHolder1_dtTo_input").value);
        	var cboRoutes = document.getElementById("ctl00_ContentPlaceHolder1_cboRoute");
          window.open('Reports/BookKaruReport.aspx?from=' + document.getElementById("ctl00_ContentPlaceHolder1_dtFrom_input").value + "&to=" + document.getElementById("ctl00_ContentPlaceHolder1_dtTo_input").value+"&routeid="+cboRoutes.options[cboRoutes.selectedIndex].value);
          return false;


      }

      </script><div class="col-lg-12">
  <div class="topRow">


    
    
        <link href="styles/styles.css" rel="stylesheet" type="text/css" />
    <script src="script/scriptlib.js" type="text/javascript"></script>   


    <table width="600px" align="center" border="0" class="TableBorder" cellspacing="0">
         <tr>
            <td align="center" class="HeaderStyle">BokKaru Report</td>
            
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
                                                            
                                                            
                                                            <tr>
                                                                <td align="right" height="5px" width="150">
                                                                    <asp:Label ID="Label4" runat="server" CssClass="Generallabel" Text="Route :"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    
                                                                     <asp:DropDownList ID="cboRoute" runat="server" AutoPostBack="True" 
                                                                        Width="235px">
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
<td align="center" class="" colspan="2">
                <asp:Button ID="btnShowReport" runat="server" Text="Show Report"  
                    OnClientClick="return loadreport();" CssClass="ButtonStyle" Width="150px" />
                &nbsp; &nbsp;                 
                </td>
       </tr>
       <tr>
            <td  align="right" height="3px"></td>
       </tr>
    </table>
 
</div>

</div>
</asp:Content>
