<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SMSSetup.aspx.vb" Inherits="FMovers.Ticketing.UI.SMSSetup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
       <link href="styles/styles.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style2
        {
            width: 453px;
        }
    </style>
    <script language=javascript >
     function Validatepass()
     {
     
        if (document.getElementById("txtoldPassword").value == document.getElementById("txtOldPass").value)
        {
        }
        else
        {
          alert("Please enter old password correctly");
          return false;
        }
        
        if (document.getElementById("txtPassword").value == "")
        {
          alert("Please enter password . It can not be blanked.");
          return false;
          
        }
       
       return true;
     }
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
                                            <table width="90%" align="center" border="0" class="TableBorder" cellspacing="0">
                                                <tr class="HeaderStyle">
                                                    <td align="left" valign="middle" colspan=2 >Update SMS Setup</td>
                                                </tr>                                                
                                                
<tr>
                                                    <td class="Generallabel"  align=center colspan="2">
                                                                    <asp:Label ID="lblOK" runat="server" Font-Bold="True" Font-Names="Verdana" 
                                                                        Font-Size="Medium" ForeColor="#339933" Visible="False">Record have been saved sucessfully</asp:Label>
                                                                </td>
                                                </tr>                                                                                                
                                                
<tr>
                                                    <td class="Generallabel"  align=right>
                                                        Select SMS Client:</td>
                                                    <td align="left" valign="middle" height="5px">
                                                        <asp:DropDownList ID="cboSetup" runat="server" Width="200px">
                                                            <asp:ListItem Selected="True">Zong</asp:ListItem>
                                                            <asp:ListItem>Telenor</asp:ListItem>
                                                            <asp:ListItem>OutReach</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>                                                                                                
                                                
<tr>
                                                    <td class="Generallabel"  align=right>
                                                        &nbsp;</td>
                                                    <td align="left" valign="middle" height="5px">
                                                        &nbsp;</td>
                                                </tr>                                                                                                
                                                
<tr>
                                                    <td class="style2">
                                                        &nbsp;</td>
                                                    <td align="left" valign="middle" height="5px">
                                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ButtonStyle" 
                                                            Width="81px" OnClientClick="javascript:return Validatepass();" />
                                                    </td>
                                                </tr>                                                                                                
                                                
<tr>
                                                    <td align="center" valign="middle" height="5px" class="style2">
                                                        &nbsp;</td>
                                                </tr>                                                                                                
                                               <tr>
                                                    <td align="center" valign="middle" height="5px" class="style2">
                                                        &nbsp;</td>
                                                </tr>  
                                              </table>
                                              
                   
    </form>
    </body>
</html>
