<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="changepassword.aspx.vb" Inherits="FMovers.Ticketing.UI.changepassword"  MasterPageFile="~/main.Master"  %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"  runat="server">



    <script language=javascript >
        function Validatepass() {

            if (document.getElementById("txtoldPassword").value == document.getElementById("txtOldPass").value) {
            }
            else {
                alert("Please enter old password correctly");
                return false;
            }

            if (document.getElementById("txtPassword").value == "") {
                alert("Please enter password . It can not be blanked.");
                return false;

            }

            return true;
        }
    
    </script>

    <div class="col-lg-12">
  <div class="topRow">


                                            <table width="90%" align="center" border="0" class="TableBorder" cellspacing="0">
                                                <tr class="HeaderStyle">
                                                    <td align="left" valign="middle" colspan=2 >Update Password</td>
                                                </tr>                                                
                                                
<tr>
                                                    <td class="Generallabel" align=right >
                                                        Old Password: </td>
                                                    <td align="left" height="5px">
                                                        <asp:TextBox ID="txtoldPassword" runat="server" TextMode="Password"></asp:TextBox>
                                                    </td>
                                                </tr>                                                                                                
                                                
<tr>
                                                    <td class="Generallabel"  align=right>
                                                        New Password :</td>
                                                    <td align="left" valign="middle" height="5px">
                                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                                                    </td>
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
                                              
                   
    <asp:HiddenField ID="txtOldPass" Value="" runat="server" />
                                              


</div>

</div>
</asp:Content>