<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserLogin.aspx.vb" Inherits="FMovers.Ticketing.UI.UserLogin" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">    
    <title>Login</title>
    <link href="styles\styles.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="styles/Stylesheet.css">    


    <script language="javascript" type="text/javascript">
    function ValidateCredentials() {

        
    //    parent.HelloFromChild("Hello");
        
        if (document.getElementById("txtLoginName").value == "")
        {
            document.getElementById("lblError").style.display = "";
            document.getElementById("lblError").innerHTML = "Please enter User Name.";
            document.getElementById("txtLoginName").focus();
            return false;
        }
        if (document.getElementById("txtPwd").value == "")
        {
            document.getElementById("lblError").style.display = "";
            document.getElementById("lblError").innerHTML = "Please enter Password.";
            document.getElementById("txtPwd").focus();
            return false;
        }
        return true;
    }

    function GetMacAddress() {
        //This function requires following option to be enabled without prompting
      
        
    }

  
   
 

    function WriteMacAddress() {
    
        
    }
    </script>
    <style type="text/css">

.style1 {color: #3e5a8d}
.style5 {color: #3e5a8d; font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 12px; }
.style4 {font-family: Verdana, Arial, Helvetica, sans-serif; font-size: 11px; color: #333232; }
.style6 {
	color: #5a5a5a;
	font-family: Verdana, Arial, Helvetica, sans-serif;
	font-size: 12px;
	font-weight: bold;
}

</style>
</head>
<body>
  <form runat=server>
<!-- Form Mixin-->
<!-- Input Mixin-->
<!-- Button Mixin-->
<!-- Pen Title-->
<div class="pen-title">

<br />
<br />
<br />
<img src="images/fmlogo.png" />

    <h5 class="style2">Welcome to Online And Offline Ticketing System</h5>
  <h4 class="style2"> 
      <asp:Label ID="lblCompany" runat="server" Text=""></asp:Label>
  
  </h4>
    <h3>
   <asp:Label ID="lblError" runat="server" Font-Names="verdana" Font-Size="Small" ForeColor="Red"></asp:Label></h3>
</div>
<!-- Form Module-->
<div class="module form-module">
  <div  style="display:none" class="toggle"><i class="fa fa-times fa-pencil"></i>
    
  </div>
  <div class="form">
    <h2>Login to your account</h2>
    <form>
       <asp:TextBox ID="txtLoginName" runat="server" CssClass="textbox" TabIndex="1"  placeholder="Username" ></asp:TextBox>
       <asp:TextBox ID="txtPwd" runat="server" CssClass="textbox" TabIndex="2" TextMode="Password" placeholder="Password"></asp:TextBox>
       <asp:Button ID="btnLogin"  OnClientClick ="return GetMacAddress();"  runat="server" CssClass="ButtonStyle" Text="Login" />
    </form>
  </div>
 

        

  <div class="form">
    <h2>IT Contact Information </h2>
    <form>
        <h5><b>Noman: 0315-8675-222                           (Head Office - Lahore IT)</b></h5><br />
        <h5><b>Faian: 0315-8679696                            (Head Office - Lahore IT)</b></h5><br />
        <h5><b>Shahzad: 0315-8222693                          ( Rawalpindi IT)</b></h5>

   
    </form>
  </div>
  <div class="cta" id="toggle" >Any Problem  Contact IT-Team <div runat=server id="divFilesVer" ></div> </div>
</div>
  <script src='http://cdnjs.cloudflare.com/ajax/libs/jquery/2.1.3/jquery.min.js'></script>
<script src='http://codepen.io/andytran/pen/vLmRVp.js'></script>

    <script src="script/index.js"></script>
    <style>
    .at-button
    {
    	display:none !important;
    	}
    </style>
</form>
</body>
</html>
