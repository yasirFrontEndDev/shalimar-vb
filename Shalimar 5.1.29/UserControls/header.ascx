<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="header.ascx.vb" Inherits="FMovers.Ticketing.UI.header" %>

    <script language="javascript" >

        function loadreports() {

            alert('hello');
            window.open('Reports/Missed.aspx?type=Missed)');

            return false;


        }
    
    </script>


<script language="javascript" src="Javascripts/tabEvents.js"></script>
<link href="css/css.css" rel="stylesheet" type="text/css" />
 <asp:ScriptManager ID="ScriptManager1" runat="server">
                            </asp:ScriptManager>
 <style type="text/css">
<!--
.style1 {color: #FFFFFF}
-->
 </style>


 <table width="99%" border="0" cellpadding="0" cellspacing="0" class="top_bg_td"  >

  <tr>
   <td valign=top align=left >
   
   <div style="margin-top:10px;width:100%" >
    <div style="float:left;width:350px" >
      <img src= "images/ERP_LOGO_03_06.gif" />
    </div>
    <div style="float:right;width:350px" >
                 <div class="style1" style="margin-left: 60px; position: absolute; margin-top: 10px;" >
                                           Current User Login :  <asp:Label ID="lblName" runat="server" Font-Bold="True" Text="Label" CssClass="login_text"></asp:Label>
             </div>

    <div>
    
      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
 

                            <asp:Image ID="imgServerStatus" runat="server" 
                                ImageUrl="~/images/signals/good.png" />
                            <span class="style1"> Server Signal Strenght</span>
                  <asp:Label ID="lblServer" runat="server" Font-Bold="True" Text="Label" 
                                CssClass="login_text"></asp:Label>
                            
                                                                <asp:ImageButton ID="imgRefresh" runat="server" 
    ImageUrl="~/images/signals/ref.png" />

                                      
                                                            </ContentTemplate>
                            </asp:UpdatePanel>
             </div> 
    </div>
   
   

   </div>
           <div style="clear:both" >
           
           </div>                                                
   </td>
  </tr>
    <tr>
     <td>
     
     
  <div id="NavigationMenu" class="menu" style="float: left;">
	<ul class="level1 static" tabindex="0" style="position: relative; width: auto; float: left;" role="menubar">
		<li role="menuitem" class="static" style="position: relative; float: left;"><a href="javascript:void(0)" onclick="document.getElementById
('mainframe').src='ImportOnlineData.aspx'" class="level1 static" tabindex="-1">Import Data</a></li>
		<li role="menuitem" class="static" style="position: relative; float: left;"><a href="#" class="level1 static" tabindex="-1" onclick="document.getElementById
('mainframe').src='SC.aspx'">Current Ticketing</a></li>
		<li role="menuitem" class="static" style="position: relative; float: left;"><a href="#" class="level1 static" tabindex="-1" onclick="document.getElementById
('mainframe').src='TicketingSchedule.aspx?mode=2'">Booking</a></li>
		<li role="menuitem" class="static" style="position: relative; float: left;"><a href="#" class="level1 static" title="Departures  " tabindex="-1" 
onclick="document.getElementById('mainframe').src='TicketingSchedule.aspx?mode=1'">Advance Ticketing</a></li>
		<li role="menuitem" class="static" style="position: relative; float: left;"><a href="#" class="level1 static" tabindex="-1" onclick="document.getElementById
('mainframe').src='UserClosingReport.aspx'">User Closing Report</a></li>
		<li role="menuitem" class="static" style="position: relative; float: left;"><a href="#" class="level1 static" tabindex="-1" onclick="document.getElementById
('mainframe').src='DepartureReportByDate.aspx'">Departure Report </a></li>
		<li role="menuitem" class="static" style="position: relative; float: left;"><a href="#" class="level1 static" tabindex="-1" onclick="document.getElementById
('mainframe').src='AdvanceReport.aspx'">Advance Report</a></li>
		        <li role="menuitem" class="static" style="position: relative; float: left;"><a href="#" class="level1 static" tabindex="-1"  
onclick="document.getElementById('mainframe').src='ETicketingReport.aspx'">E-TicketingReport</a></li>
		<li role="menuitem" class="static" style="position: relative; float: left;"><a href="#" class="level1 static" tabindex="-1" onclick="document.getElementById
('mainframe').src='UserCashClosingMulti.aspx'">User Closing</a></li>
		<li role="menuitem" class="static" style="position: relative; float: left;"><a href="#" class="level1 static" tabindex="-1"  onclick="document.getElementById
('mainframe').src='changepassword.aspx'">Change Password</a></li>
		<li role="menuitem" class="static" style="position: relative; float: left;"><a href="#" class="level1 static" tabindex="-1"  onclick="document.getElementById
('mainframe').src='refund.aspx'">Ticket Refund</a></li>
		<li role="menuitem" class="static" style="position: relative; float: left;"><a href="#" class="level1 static" tabindex="-1"  onclick="document.getElementById
('mainframe').src='SC_New.aspx'">Ticket Change</a></li>

		<li role="menuitem" class="static" style="position: relative; float: left;" ><a href="#" class="level1 static" tabindex="-1"  
onclick="window.open('Reports/Missed.aspx');">Missed Pessanger</a></li>

        
	</ul>
</div>
</td>
  </tr>

  
</table>
