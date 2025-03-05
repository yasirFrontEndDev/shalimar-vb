<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DailyDepartureReportByDate.aspx.vb" Inherits="FMovers.Ticketing.UI.DailyDepartureReportByDate" %>
<%@ Register tagprefix="igsch" namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics35.WebUI.WebDateChooser.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igmisc" namespace="Infragistics.WebUI.Misc" Assembly="Infragistics35.WebUI.Misc.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Voucher Report</title>
    <script language=javascript >
        function loadreport() {

            //         alert( document.getElementById("dtFrom_input").value    );
            //         
            //            alert( document.getElementById("cboList").value    );
            window.open('Reports/DailyDepartureReport.aspx?Type=summary&from=' + document.getElementById("dtFrom_input").value + "&to=" + document.getElementById("dtTo_input").value + "&Dfrom=" + document.getElementById("txtTimeFrom").value + "&Dto=" + document.getElementById("txtTimeTo").value + "&Type=" + document.getElementById("cboList").value + "&Route=" + document.getElementById("cboRoute").value);

            return false;


        }

        function loadreportSummary() {

            //         alert( document.getElementById("dtFrom_input").value    );
            //
            //            alert( document.getElementById("cboList").value    );
            alert(document.getElementById("dtFrom_input").value);
            
            window.open('Reports/DailyDepartureReport.aspx?Type=detail&from=' + document.getElementById("dtFrom_input").value + "&to=" + document.getElementById("dtTo_input").value + "&Dfrom=" + document.getElementById("txtTimeFrom").value + "&Dto=" + document.getElementById("txtTimeTo").value + "&Type=" + document.getElementById("cboList").value + "&Route=" + document.getElementById("cboRoute").value);


      //      window.open('Reports/DailyDepartureReport.aspx?Type=detail&from=' + document.getElementById("dtFrom_input").value + "&to=" + document.getElementById("dtTo_input").value + "&Dfrom=" + document.getElementById("txtTimeFrom").value + "&Dto=" + document.getElementById("txtTimeTo").value + "&Type=" + document.getElementById("cboList").value + "&Route=" + document.getElementById("cboRoute").value);

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
                                                            <tr >
                                                                <td align="right" width="150" height="5px">
                                                                    <asp:Label ID="Label4" runat="server" Text="User Name:" 
                                                                        CssClass="Generallabel"></asp:Label>&nbsp;</td>
                                                                <td>
                                                                    <asp:DropDownList ID="cboUser" Visible="false" runat="server" AutoPostBack="True" 
                                                                        Width="235px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            
                                                            <tr >
                                                                <td align="right" width="150" height="5px">
                                                                    <asp:Label ID="Label6" runat="server" Text="Route :" 
                                                                        CssClass="Generallabel"></asp:Label>&nbsp;</td>
                                                                <td>
                                                                    <asp:DropDownList ID="cmbRoute" Visible="false" runat="server" AutoPostBack="True" 
                                                                        Width="235px">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>

                                                            <tr>
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
                                                        </table>
                      </igmisc:WebAsyncRefreshPanel>
                                                        
           </td>
       </tr>
       <tr>
            <td align="right" class="">
                <asp:Button ID="btnShowReport" runat="server" Text="Show Report"  
                    OnClientClick="return loadreport();" CssClass="ButtonStyle" Width="150px" />&nbsp;
                <asp:Button ID="btnShowReportSummary" runat="server" 
                    Text="Show Report Summary"  OnClientClick="return loadreportSummary();" 
                    CssClass="ButtonStyle" Width="250px" />&nbsp; </td>
       </tr>
       <tr>
            <td align="right" height="3px"></td>
       </tr>
    </table>
    </form>
</body>
</html>

