<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BusCharges.aspx.vb" Inherits="FMovers.Ticketing.UI.TicketingDeduction" %>

<%@ Register tagprefix="cc1" namespace="Infragistics.Web.UI.LayoutControls" Assembly="Infragistics35.Web.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igtbl" namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics35.WebUI.UltraWebGrid.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Routes</title>
    <link href="styles/styles.css" rel="stylesheet" type="text/css" />
    <script language="javascript">

        function validation() {                   
//            if (document.getElementById("txtRouteID").value == "") {
//                alert("Please Specify Route Code!");
//                return false;
//            }

//            if (document.getElementById("txtRouteName").value == "") {
//                alert("Please Specify Route Name!");
//                return false;
//            }

//            var Grid = igtbl_getGridById('grdRoutes')
//            if (Grid.Rows.length == 0) {
//                alert("Please enter Route Information");
//                return false;
//            }
        }

        function updateTotal() {
            var HostessSalary = parseInt(document.getElementById("txtHostessSalary").value);
            var DriverSalary = parseInt(document.getElementById("txtDriverSalary").value);
            var GuardSalary = parseInt(document.getElementById("txtGuardSalary").value);
            var ServiceCharges = parseInt(document.getElementById("txtServiceCharges").value);
            var CleaningCharges = parseInt(document.getElementById("txtCleaningCharges").value);
            var HookCharges = parseInt(document.getElementById("txtHookCharges").value);
            var BusCharges = parseInt(document.getElementById("txtBusCharges").value);
            var Refreshment = parseInt(document.getElementById("txtRefreshment").value);
            var Toll = parseInt(document.getElementById("txtToll").value);

            if ((HostessSalary != NaN) && (DriverSalary != NaN) ) {
            
            }
            var total = HostessSalary + DriverSalary + GuardSalary + ServiceCharges + CleaningCharges + HookCharges + BusCharges + Refreshment + Toll;
            document.getElementById("txtTotalDeductions").value = total; 
        }

    
        function pageInit() {  
                      
        }
    
    
    </script>
    </head>
<body onload="pageInit();">
    <form id="form1" runat="server">
    <div>
    
    </div>
        <table width="30%" align="center" border="0" class="TableBorder" 
        cellspacing="0">
            <tr>
                <td align="left" valign="middle">
                    <table cellpadding="2" cellspacing="0" style="width: 100%">
                        <tr>
                            <td align="right" style="width: 150px">
                                <asp:Label ID="Label1" runat="server" Text="Hostess Salary :" 
                                    CssClass="Generallabel"></asp:Label>&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtHostessSalary" runat="server" Width="100px" CssClass="txtbox" 
                                    MaxLength="40" Height="20px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label2" runat="server" Text="Driver Salary :" 
                                    CssClass="Generallabel"></asp:Label>&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtDriverSalary" runat="server" Width="100px" CssClass="txtbox" 
                                    MaxLength="40" Height="20px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label3" runat="server" Text="Gaurd Salary :" 
                                    CssClass="Generallabel"></asp:Label>&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtGuardSalary" runat="server" Width="100px" CssClass="txtbox" 
                                    MaxLength="40" Height="20px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label4" runat="server" Text="Service Charges :" 
                                    CssClass="Generallabel"></asp:Label>&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtServiceCharges" runat="server" Width="100px" CssClass="txtbox" 
                                    MaxLength="40" Height="20px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label5" runat="server" Text="Cleaning Charges :" 
                                    CssClass="Generallabel"></asp:Label>&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtCleaningCharges" runat="server" Width="100px" CssClass="txtbox" 
                                    MaxLength="40" Height="20px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label6" runat="server" Text="Hook Charges :" 
                                    CssClass="Generallabel"></asp:Label>&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtHookCharges" runat="server" Width="100px" CssClass="txtbox" 
                                    MaxLength="40" Height="20px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label7" runat="server" Text="Bus Charges :" 
                                    CssClass="Generallabel"></asp:Label>&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtBusCharges" runat="server" Width="100px" CssClass="txtbox" 
                                    MaxLength="40" Height="20px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label8" runat="server" Text="Refreshment :" 
                                    CssClass="Generallabel"></asp:Label>&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtRefreshment" runat="server" Width="100px" CssClass="txtbox" 
                                    MaxLength="40" Height="20px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label9" runat="server" Text="Toll GBS :" 
                                    CssClass="Generallabel"></asp:Label>&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtToll" runat="server" Width="100px" CssClass="txtbox" 
                                    MaxLength="40" Height="20px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="Label10" runat="server" Text="Total Deduction :" 
                                    CssClass="Generallabel"></asp:Label>&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotalDeductions" runat="server" Width="100px" CssClass="txtbox" 
                                    MaxLength="40" Height="20px" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>                                                
           <tr>
                <td align="right" valign="middle" height="5px">
                    &nbsp;
                </td>
            </tr>  
           <tr>
                <td align="right" valign="middle" height="5px">                                                        
                    <asp:Button ID="btnSave" runat="server" Text="Save" 
                    CssClass="ButtonStyle" Width="81px" />&nbsp;<asp:Button ID="btnClose" runat="server" Text="Close" 
                    CssClass="ButtonStyle" width="100px"/>
                </td>
            </tr>  
          </table>
                   
    </form>
</body>
</html>
