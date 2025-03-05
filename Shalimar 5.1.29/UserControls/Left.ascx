<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Left.ascx.vb" Inherits="FMovers.Ticketing.UI.Left" %>


<style type="text/css">


.ButtonStyle
{
	background-image:url('../images/BtnBG.jpg');
	COLOR: White;
	border-width: thin;
	border-color:Blue; 
  	font-family: verdana; 
	font-size: 10pt;
	font-weight: bold; 
	height:24px;
    margin-left: 0px;
    margin-right: 0px;
    margin-top: 0px;
    width: 128px;
}

</style>

				       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
<table width="100%" border="0" cellpadding="0" cellspacing="0" >
			
			
		
				                <tr>
				                		<td align="left" valign="top"  style="height:120px;background-color:#FFFFFF"  >	
				                		<table width="100%" border="0" cellspacing="0" cellpadding="0">
				
				<tr>
					                <td align="left" valign="middle" class="left_bg_02">	
   					                  <a href="javascript:void(0)" class="left_links" onclick="document.getElementById('mainframe').src='#'"> 
   					                  PNR Search
   					                  </a>
   					                  
					                </td>
				                  </tr>
	  			
				

				          <tr>
				          <td>
				          <table width ="100%" style="font-family: verdana; font-size: 10px;" >
  
                                                  <tr>
                                                      <td>
                                                                    <asp:TextBox ID="txtPNRSearch" runat="server" CssClass="Generallabel" 
                                                                        Text=""></asp:TextBox>
                                                                </td>
                                                      <td>
                                                                   <asp:Button ID="Button1" runat="server" Text="GO" CssClass="ButtonStyle" 
                    Width="50px" ForeColor="Black" />
                                                                </td>
                                                  </tr>
                                                 
                                                 
                                                 <tr>
                                                 <td>
                                                 <div id= "divPNRResult" runat=server >
                                                 
                                                 </div>
                                                 </td>
                                                 </tr>
                                              </table>
				          </td>
				          </tr>
		                  
  			                 
    
		               



				  


				</table>
				                		</td>
				                
				                </tr>  
				                  
				                <tr>
					                <td align="left" valign="middle" class="left_bg_02">	
   					                  <a href="javascript:void(0)" class="left_links" onclick="document.getElementById('mainframe').src='#'"> 
   					                  Current Cash Position
   					                  </a>
   					                  
					                </td>
				                  </tr>
	  			
	  			   
	  			

			  <tr>
				<td align="left" valign="top"  style="height:720px;background-color:#FFFFFF"  >	
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
				
				
				
<tr >
				      <td align="left" valign="middle"  class="left_td_02" >

				      	  <table width ="100%" style="font-family: verdana; font-size: 10px;" >
  
                                                  <tr>
                                                      <td>
                                                                    <asp:Label ID="Label10" runat="server" CssClass="Generallabel" 
                                                                        Text="Opening Balanace :"></asp:Label>
                                                                </td>
                                                      <td>
                                                                    <asp:Label ID="lblOpeniningBal" runat="server" CssClass="Generallabel" 
                                                                        Font-Bold="True"></asp:Label>
                                                                </td>
                                                  </tr>
                                                  <tr>
                                                      <td>
                                                                    <asp:Label ID="Label5" runat="server" CssClass="Generallabel" 
                                                                        Text="Total Cash Hold :"></asp:Label>
                                                                    </td>
                                                      <td>
                                                                    <asp:Label ID="lblCashCollection" runat="server" CssClass="Generallabel" 
                                                                        Font-Bold="True"></asp:Label>
                                                                    </td>
                                                  </tr>
                                                  <tr>
                                                      <td>
                                                                    <asp:Label ID="Label13" runat="server" CssClass="Generallabel" 
                                                                        Text="Total Deduction :"></asp:Label>
                                                                </td>
                                                      <td>
                                                                    <asp:Label ID="lblDeduction" runat="server" CssClass="Generallabel" 
                                                                        Font-Bold="True"></asp:Label>
                                                                </td>
                                                  </tr>
                                                  <tr>
                                                      <td>
                                                          <asp:Label ID="Label6" runat="server" CssClass="Generallabel" 
                                                              Text="Advance Ticketing :"></asp:Label>
                                                      </td>
                                                      <td>
                                                          <asp:Label ID="lblAdance" runat="server" CssClass="Generallabel" 
                                                              Font-Bold="True"></asp:Label>
                                                      </td>
                              </tr>
                              
                                                                                <tr>
                                                      <td>
                                                          <asp:Label ID="Label1" runat="server" CssClass="Generallabel" 
                                                              Text="Missed Cash :"></asp:Label>
                                                      </td>
                                                      <td>
                                                          <asp:Label ID="lblMissed" runat="server" CssClass="Generallabel" 
                                                              Font-Bold="True"></asp:Label>
                                                      </td>
                              </tr>

                                                                                <tr>
                                                      <td>
                                                          <asp:Label ID="Label3" runat="server" CssClass="Generallabel" 
                                                              Text="Refund Cash :"></asp:Label>
                                                      </td>
                                                      <td>
                                                          <asp:Label ID="lblRefund" runat="server" CssClass="Generallabel" 
                                                              Font-Bold="True"></asp:Label>
                                                      </td>
                              </tr>


                                                                                <tr>
                                                      <td>
                                                          <asp:Label ID="Label2" runat="server" CssClass="Generallabel" 
                                                              Text="Change Cash :"></asp:Label>
                                                      </td>
                                                      <td>
                                                          <asp:Label ID="lblChange" runat="server" CssClass="Generallabel" 
                                                              Font-Bold="True"></asp:Label>
                                                      </td>
                              </tr>

                              
                                                  <tr>
                                                      <td colspan="2">
                                                          --------------------------------------</td>
                                                  </tr>
                                                  <tr>
                                                      <td>
                                                                    <asp:Label ID="Label14" runat="server" CssClass="Generallabel" 
                                                              Text="Total :"></asp:Label>
                                                                </td>
                                                      <td>
                                                                    <asp:Label ID="lblTotal" runat="server" CssClass="Generallabel" 
                                                                        Font-Bold="True"></asp:Label>
                                                                </td>
                                                  </tr>
                                                  <tr>
                                                      <td colspan="2">
                <asp:Button ID="btnShowReport" runat="server" Text="Refresh" CssClass="ButtonStyle" 
                    Width="115px" ForeColor="Black" />
                                                                    </td>
                                                  </tr>
                                              </table>
                                              
                                                                   
                                   


				      
				      	</td>
				          </tr>
				          
				            <tr>
					                <td align="left" valign="middle" class="left_bg_02">	
   					                  <a href="javascript:void(0)" class="left_links" onclick="document.getElementById('mainframe').src='#'"> 
   					                    Departure Time Live Feeds
   					                  </a>
   					                  
					                </td>
				                  </tr>
				          
<tr>
<td align="left" valign="middle"  class="left_td_02" >


                                             <div runat=server id="CurrentPosition" style="overflow:scroll" >
                                             
                                             </div>                                            

                                              
</td>



</tr>

				                  
  			                 
                                 
	 <tr>
					                <td align="left" valign="middle" class="left_bg_02">	
   					                  <a href="javascript:void(0)" class="left_links" onclick="document.getElementById('mainframe').src='#'"> 
   					                    Arrival Time Live Feeds
   					                  </a>
   					                  
					                </td>
				                  </tr>
				          
<tr>
<td align="left" valign="middle"  class="left_td_02" >


                                             <div runat=server id="CurrentPosition_arr" style="overflow:scroll" >
                                             
                                             </div>                                            

                                              
</td>



</tr>			               



				  


				</table>
				</td>
			  </tr>
			  
			</table>
       </ContentTemplate>
                            </asp:UpdatePanel>
                            