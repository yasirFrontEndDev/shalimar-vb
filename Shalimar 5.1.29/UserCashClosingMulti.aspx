﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserCashClosingMulti.aspx.vb" Inherits="FMovers.Ticketing.UI.UserCashClosingMulti" MasterPageFile="~/main.Master" %>


<%@ Register tagprefix="igsch" namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics35.WebUI.WebDateChooser.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igmisc" namespace="Infragistics.WebUI.Misc" Assembly="Infragistics35.WebUI.Misc.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>




<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"  runat="server">


    <script language=javascript >

        function loadreport() {
            return window.confirm("Are you sure you want to close cash ? ")


        }



        function loadreportmissed(Type) {
 
            window.open("Reports/Missed.aspx?type=" + Type);
        }


        function loadclosingreport() {
            return window.confirm("Are you sure you want to close cash ? ")


        }
        
    </script>

    <link href="styles/styles.css" rel="stylesheet" type="text/css" />
    <script src="script/scriptlib.js" type="text/javascript"></script>   
    <style type="text/css">


.textbox
{
height:15px;
color:#000000;
font-family:Verdana;
font-size:11px;
}
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 20px;
        }
    </style>

    <div class="col-lg-12">
  <div class="topRow">


    <table width="600px" align="center" border="0" class="TableBorder" cellspacing="0">
         <tr>
            <td align="center" class="HeaderStyle">User Cash Closing</td>
            
        </tr>
        <tr>
            <td align="center" valign="top">

                                                        <table cellpadding="2" cellspacing="1" width="90%">
                                                            <tr>
                                                                <td align="left" height="5px" colspan="3">
                                                                    <asp:Label ID="lblError" runat="server" Font-Names="verdana" Font-Size="Small" 
                                                                        ForeColor="Red"></asp:Label>
                                                                    <asp:Label ID="lblOK" runat="server" Font-Bold="True" Font-Names="Verdana" 
                                                                        Font-Size="Medium" ForeColor="#339933" Visible="False">Shiting have been done sucessfully ...</asp:Label>
                                                                </td>
                                                            </tr>
                                                            
                                                            <tr>
                                                                <td align="right" width="150" height="5px">
                                                                    <asp:Label ID="Label10" runat="server" CssClass="Generallabel" 
                                                                        Text="Opening Balanace :"></asp:Label>
                                                                </td>
                                                                <td valign="top"  align=left >
                                                                    <asp:Label ID="lblOpeniningBal" runat="server" CssClass="Generallabel" 
                                                                        Font-Bold="True"></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            
                                                            
                                                            <tr>
                                                                <td align="right" height="5px" width="150">
                                                                    <asp:Label ID="Label5" runat="server" CssClass="Generallabel" 
                                                                        Text="Total Cash Hold :"></asp:Label>
                                                                    &nbsp;</td>
                                                                <td align="left" valign="top">
                                                                    <asp:Label ID="lblCashCollection" runat="server" CssClass="Generallabel" 
                                                                        Font-Bold="True"></asp:Label>
                                                                    &nbsp;</td>
                                                                <td align="left" valign="top">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            
                                                            
                                                            <tr>
                                                                <td align="right" height="5px" width="150">
                                                                    <asp:Label ID="Label13" runat="server" CssClass="Generallabel" 
                                                                        Text="Total Deduction :"></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <asp:Label ID="lblDeduction" runat="server" CssClass="Generallabel" 
                                                                        Font-Bold="True"></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center" colspan="2" height="5px" style="font-weight: bold">
                                                                    -------------------------------------------------------------------------------------</td>
                                                                <td align="left" valign="top">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" height="5px" width="150">
                                                                    <asp:Label ID="Label14" runat="server" CssClass="Generallabel" Text="Total :"></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <asp:Label ID="lblTotal" runat="server" CssClass="Generallabel" 
                                                                        Font-Bold="True"></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" height="5px" width="150">
                                                                    <asp:Label ID="Label6" runat="server" CssClass="Generallabel" 
                                                                        Text="Advance Ticketing :"></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <asp:Label ID="lblAdance" runat="server" CssClass="Generallabel" 
                                                                        Font-Bold="True"></asp:Label>
                                                                    &nbsp; 
                                                                    <a href = "#" onclick="return loadreportmissed('Advance')"  class=linkButton > ( Click Here to see report ) </a> 
                                                                    </td>
                                                                <td align="left" valign="top">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" height="5px" width="150">
                                                                    <asp:Label ID="Label21" runat="server" CssClass="Generallabel" 
                                                                        Text="Missed Cash :"></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <asp:Label ID="lblMissed" runat="server" CssClass="Generallabel" 
                                                                        Font-Bold="True">
                                                                        </asp:Label>
                                                                                                                                        &nbsp; 
                                                                    <a href = "#" onclick="return loadreportmissed('Missed')"  class=linkButton > ( Click Here to see report ) </a> 
                                                                    </td>
                                                                <td align="left" valign="top">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" height="5px" width="150">
                                                                    <asp:Label ID="Label22" runat="server" CssClass="Generallabel" 
                                                                        Text="Refund Cash :"></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <asp:Label ID="lblRefund" runat="server" CssClass="Generallabel" 
                                                                        Font-Bold="True"></asp:Label>
                                                                      &nbsp; 
                                                                    <a href = "#" onclick="return loadreportmissed('TicketRefund')"  class=linkButton > ( Click Here to see report ) </a> 
                                                                        
                                                                    </td>
                                                                <td align="left" valign="top">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" width="150">
                                                                    <asp:Label ID="Label23" runat="server" CssClass="Generallabel" 
                                                                        Text="Change Cash :"></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <asp:Label ID="lblChange" runat="server" CssClass="Generallabel" 
                                                                        Font-Bold="True"></asp:Label>
                                                                                                                                            &nbsp; 
                                                                    <a href = "#" onclick="return loadreportmissed('TicketChange')"  class=linkButton > ( Click Here to see report ) </a> 
                                                                    </td>
                                                                <td align="left" valign="top">
                                                                    </td>
                                                            </tr>
                                                            <tr style="display:none" >
                                                                <td align="right" height="5px" width="150">
                                                                    <asp:Label ID="Label11" runat="server" CssClass="Generallabel" 
                                                                        Text="Next Shift User Name :"></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <asp:TextBox ID="txtLoginName" runat="server" CssClass="textbox" TabIndex="1"></asp:TextBox>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr style="display:none">
                                                                <td align="right" height="5px" width="150">
                                                                    <asp:Label ID="Label12" runat="server" CssClass="Generallabel" 
                                                                        Text="Next Shift User Pass :"></asp:Label>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    <asp:TextBox ID="txtPwd" runat="server" CssClass="textbox" TabIndex="2" 
                                                                        TextMode="Password" Width="122px"></asp:TextBox>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" colspan="2">
                                                                    <table class="style1" style="display: none">
                                                                        <tr>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                            <td>
                                                                                <asp:Label ID="Label16" runat="server" CssClass="Generallabel" 
                                                                                    Text="Select User :"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label17" runat="server" CssClass="Generallabel" Text="Amount"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                            <td>
                                                                                
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="textbox" TabIndex="1"></asp:TextBox>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnAdd" runat="server" Text="Add" />
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                        </tr>
                                                                    </table>
                                                                    <table class="style1">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="Label15" runat="server" CssClass="Generallabel" 
                                                                                    Text="Select Schedule :"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="Label18" runat="server" CssClass="Generallabel" 
                                                                                    Text="Select Users :"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:DropDownList ID="cboSchedule" runat="server">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="cboUsers" runat="server">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Button ID="btnAddGrid" runat="server" CssClass="ButtonStyle" 
                                                                                     Text="Add" Width="80px" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                            <td>
                                                                                &nbsp;</td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" colspan="2">
                                                                    <asp:GridView ID="grdUsers" runat="server" AutoGenerateColumns="False" 
                                                                        CellPadding="2" CellSpacing="2" Font-Names="Verdana" Font-Size="10pt">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="SequenceNo" HeaderText="Seq. No" />
                                                                            <asp:BoundField DataField="Schedule" HeaderText="Schedule" />
                                                                            <asp:BoundField DataField="User" HeaderText="User" />
                                                                            <asp:BoundField DataField="Amount" HeaderText="Amount" />
                                                                            <asp:BoundField DataField="Schedule_Id" HeaderText="Schedule_Id">
                                                                                <ItemStyle Width="0px" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="User_Id" HeaderText="User_Id">
                                                                                <ItemStyle Width="0px" />
                                                                            </asp:BoundField>
                                                                            
                                                                        </Columns>
                                                                        <HeaderStyle BackColor="#0066CC" ForeColor="White" />
                                                                        <AlternatingRowStyle BackColor="#FFFFCC" />
                                                                    </asp:GridView>
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr runat="server" id="trUsers" >
                                                                <td align="right" height="5px" width="150">
                                                                    <asp:HiddenField ID="UserID" runat="server" />
                                                                    <asp:HiddenField ID="BookId2" runat="server" />
                                                                    <asp:HiddenField ID="SequenceNo" runat="server" />
                                                                </td>
                                                                <td align="left" valign="top">
                                                                    &nbsp;</td>
                                                                <td align="left" valign="top">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            
                                                            
                                                            
                                                            
                                                        </table>

                                                        
           </td>
       </tr>
       <tr>
            <td align="left" class="">
                <table class="style1">
                    <tr>
                        <td class="style2">
                            &nbsp;</td>
                        <td colspan="3">
                                                                    <asp:CheckBox ID="chkCashToCashier" 
                                runat="server" CssClass="Generallabel" 
                                                                        Text="Closing given to cashier [Without Advance Ticketing]" 
                                                                        Font-Bold="True" 
                                AutoPostBack="True" />
                                                                </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            &nbsp;</td>
                        <td>
                            <asp:Label ID="Label19" runat="server" CssClass="Generallabel" 
                                Text="Select User :"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label20" runat="server" CssClass="Generallabel" 
                                Text="Enter Amount :"></asp:Label>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style2">
                            &nbsp;</td>
                        <td>
                            <asp:DropDownList ID="cboUsers0" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                                                                    <asp:TextBox ID="txtAmounts" 
                                runat="server" CssClass="textbox" TabIndex="1"></asp:TextBox>
                                                                </td>
                        <td>
                            <asp:Button ID="btnAddGrid0" runat="server" CssClass="ButtonStyle" Text="Add" 
                                Width="80px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            &nbsp;</td>
                        <td colspan="3">
                                                                    <asp:GridView ID="grdDec" runat="server" AutoGenerateColumns="False" 
                                                                        CellPadding="2" 
                    CellSpacing="2" Font-Names="Verdana" Font-Size="10pt">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="SequenceNo" HeaderText="Seq. No" />
                                                                            <asp:BoundField DataField="User" HeaderText="User" />
                                                                            <asp:BoundField DataField="Amount" HeaderText="Amount" />

                                                                            <asp:BoundField DataField="User_Id" HeaderText="User_Id">
                                                                                <ItemStyle Width="0px" />
                                                                            </asp:BoundField>
                                                                            
                                                                        </Columns>
                                                                        <HeaderStyle BackColor="#0066CC" ForeColor="White" />
                                                                        <AlternatingRowStyle BackColor="#FFFFCC" />
                                                                    </asp:GridView>
                        </td>
                    </tr>
                    </table>
            </td>
       </tr>
       <tr>
            <td align="left" class="">
                                                                    &nbsp;</td>
       </tr>
       <tr>
            <td align="right" class="">
                <asp:Button ID="btnShowReport" runat="server" Text="Shift Cash"  
                    OnClientClick="return loadreport();" OnClick="btnShowReport_Click" CssClass="ButtonStyle" 
                    Width="115px" />
                    
                &nbsp;
                    
                <asp:Button ID="btnReset" runat="server" Text="Reset"  
                      CssClass="ButtonStyle" 
                    Width="115px" />
                    
                &nbsp; </td>
       </tr>
       <tr>
            <td align="right" height="3px"></td>
       </tr>
    </table>


</div>

</div>
</asp:Content>