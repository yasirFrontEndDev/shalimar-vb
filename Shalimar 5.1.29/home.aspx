<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="home.aspx.vb" Inherits="FMovers.Ticketing.UI.home" %>


<%@ Register tagprefix="igsch" namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics35.WebUI.WebDateChooser.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register Src="UserControls/footer.ascx" TagName="footer" TagPrefix="uc4" %>

<%@ Register Src="UserControls/header.ascx" TagName="header" TagPrefix="uc1" %>
<%@ Register Src="UserControls/Left.ascx" TagName="Left" TagPrefix="uc2" %>
<%@ Register Src="UserControls/main.ascx" TagName="main" TagPrefix="uc3" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script language=javascript src="Javascripts/web_design_calculater.js" ></script>
 <link href="styles/styles.css" rel="stylesheet" type="text/css" />
    
   <link rel="stylesheet" href="styles/jquery.ui.all.css">
	<script src="script/jquery-1.5.1.js"></script>
	<script src="script/jquery.ui.core.js"></script>
	<script src="script/jquery.ui.widget.js"></script>
	<script src="script/jquery.ui.button.js"></script>
	<script src="script/jquery.ui.position.js"></script>
	<script src="script/jquery.ui.autocomplete.js"></script>
	<link rel="stylesheet" href="styles/demos.css">
	<style>
	.ui-button { margin-left: -1px; }
	.ui-button-icon-only .ui-button-text { padding: 0.35em; } 
	.ui-autocomplete-input { margin: 0; padding: 0.48em 0 0.47em 0.45em; }
	</style>
	<script>
	    (function($) {
	        $.widget("ui.cboTime", {
	            _create: function() {
	                var self = this,
					select = this.element.hide(),
					selected = select.children(":selected"),
					value = selected.val() ? selected.text() : "";
	                var input = this.input = $("<input>")
					.insertAfter(select)
					.val(value)
					.autocomplete({
					    delay: 0,
					    minLength: 0,
					    source: function(request, response) {
					        var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
					        response(select.children("option").map(function() {
					            var text = $(this).text();
					            if (this.value && (!request.term || matcher.test(text)))
					                return {
					                    label: text.replace(
											new RegExp(
												"(?![^&;]+;)(?!<[^<>]*)(" +
												$.ui.autocomplete.escapeRegex(request.term) +
												")(?![^<>]*>)(?![^&;]+;)", "gi"
											), "<strong>$1</strong>"),
					                    value: text,
					                    option: this
					                };
					        }));
					    },
					    select: function(event, ui) {


					        ui.item.option.selected = true;
					        self._trigger("selected", event, {
					            item: ui.item.option
					        });
					        //  alert('i am here');									
					        __doPostBack('cboTime', '')

					    },
					    change: function(event, ui) {
					        if (!ui.item) {
					            var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($(this).val()) + "$", "i"),
									valid = false;

					            select.children("option").each(function() {
					                if ($(this).text().match(matcher)) {

					                    this.selected = valid = true;

					                    return false;
					                }

					            });
					            if (!valid) {
					                // remove invalid value, as it didn't match anything
					                $(this).val("");
					                select.val("");
					                input.data("autocomplete").term = "";
					                return false;
					            }
					        }
					    }
					})
					.addClass("ui-widget ui-widget-content ui-corner-left");

	                input.data("autocomplete")._renderItem = function(ul, item) {
	                    return $("<li></li>")
						.data("item.autocomplete", item)
						.append("<a>" + item.label + "</a>")
						.appendTo(ul);
	                };

	                this.button = $("<button type='button'>&nbsp;</button>")
					.attr("tabIndex", -1)
					.attr("title", "Show All Items")
					.insertAfter(input)
					.button({
					    icons: {
					        primary: "ui-icon-triangle-1-s"
					    },
					    text: false
					})
					.removeClass("ui-corner-all")
					.addClass("ui-corner-right ui-button-icon")
					.click(function() {
					    // close if already visible
					    if (input.autocomplete("widget").is(":visible")) {
					        input.autocomplete("close");
					        return;
					    }

					    // work around a bug (likely same cause as #5265)
					    $(this).blur();

					    // pass empty string as value to search for, displaying all results
					    input.autocomplete("search", "");
					    input.focus();
					});
	            },

	            destroy: function() {
	                this.input.remove();
	                this.button.remove();
	                this.element.show();
	                $.Widget.prototype.destroy.call(this);
	            }
	        });
	    })(jQuery);

	    $(function() {
	        $("#cboTime").cboTime();
	        $("#toggle").click(function() {
	            $("#cboTime").toggle();
	        });
	    });
	</script>
	
	<script>
	    (function($) {
	        $.widget("ui.cboRoute", {
	            _create: function() {
	                var self = this,
					select = this.element.hide(),
					selected = select.children(":selected"),
					value = selected.val() ? selected.text() : "";
	                var input = this.input = $("<input>")
					.insertAfter(select)
					.val(value)
					.autocomplete({
					    delay: 0,
					    minLength: 0,
					    source: function(request, response) {
					        var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
					        response(select.children("option").map(function() {
					            var text = $(this).text();
					            if (this.value && (!request.term || matcher.test(text)))
					                return {
					                    label: text.replace(
											new RegExp(
												"(?![^&;]+;)(?!<[^<>]*)(" +
												$.ui.autocomplete.escapeRegex(request.term) +
												")(?![^<>]*>)(?![^&;]+;)", "gi"
											), "<strong>$1</strong>"),
					                    value: text,
					                    option: this
					                };
					        }));
					    },
					    select: function(event, ui) {


					        ui.item.option.selected = true;
					        self._trigger("selected", event, {
					            item: ui.item.option
					        });
					        __doPostBack('cboRoute', '')
					    },
					    change: function(event, ui) {
					        if (!ui.item) {
					            var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($(this).val()) + "$", "i"),
									valid = false;

					            select.children("option").each(function() {
					                if ($(this).text().match(matcher)) {
					                    this.selected = valid = true;
					                    return false;
					                }

					            });
					            if (!valid) {
					                // remove invalid value, as it didn't match anything
					                $(this).val("");
					                select.val("");
					                input.data("autocomplete").term = "";
					                return false;
					            }
					        }
					    }
					})
					.addClass("ui-widget ui-widget-content ui-corner-left");

	                input.data("autocomplete")._renderItem = function(ul, item) {
	                    return $("<li></li>")
						.data("item.autocomplete", item)
						.append("<a>" + item.label + "</a>")
						.appendTo(ul);
	                };

	                this.button = $("<button type='button'>&nbsp;</button>")
					.attr("tabIndex", -1)
					.attr("title", "Show All Items")
					.insertAfter(input)
					.button({
					    icons: {
					        primary: "ui-icon-triangle-1-s"
					    },
					    text: false
					})
					.removeClass("ui-corner-all")
					.addClass("ui-corner-right ui-button-icon")
					.click(function() {
					    // close if already visible
					    if (input.autocomplete("widget").is(":visible")) {
					        input.autocomplete("close");
					        return;
					    }

					    // work around a bug (likely same cause as #5265)
					    $(this).blur();

					    // pass empty string as value to search for, displaying all results
					    input.autocomplete("search", "");
					    input.focus();
					});
	            },

	            destroy: function() {
	                this.input.remove();
	                this.button.remove();
	                this.element.show();
	                $.Widget.prototype.destroy.call(this);
	            }
	        });
	    })(jQuery);

	    $(function() {
	        $("#cboRoute").cboRoute();
	        $("#toggle").click(function() {
	            $("#cboRoute").toggle();
	        });
	    });
	</script>
	
	<link rel="stylesheet" href="styles/demos.css">
    
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
<script language=javascript>

    <!--

    function HelloFromChild(name) {
        alert("Hello " + name + " , (called from child window)");
    }
    function LoginAgain() {

        alert("I am here bro");

//        window.open("UserLogin.aspx");


    }


    function SetTime()
    {
      //CalculatePrice();
    }
    function UpdateCycle() {
        SetTime();
      //setTimeout("UpdateCycle()",300000);
    }
    //-->
    
    function showandhide()
    {
       if (document.getElementById("leftpart").style.display =="none" ||  document.getElementById("leftpart").style.display =="")
       {
        document.getElementById("leftpart").style.display="block"; 
        document.getElementById("showhide").innerHTML ="Hide"; 
        
       }
       else
       {
         document.getElementById("leftpart").style.display="none"; 
        document.getElementById("showhide").innerHTML ="Show";          
       }
       
      
    }
    


    
 </script>



<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Enterprise Resource Planing</title>
    <link rel="stylesheet" type="text/css" href="styles/Stylesheet.css">
    <!-- Bootstrap Core CSS -->
 
  

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
    <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
    <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->


</head>
  <form runat =server>
<div id="wrapper">

    <!-- Navigation -->
    <nav class="navbar navbar-inverse navbar-fixed-top" role="navigation">
        <div class="navbar-header"  >
             <img src="images/Fmlogos.png" style="margin-top:10px" />
        </div>

        <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
            <span class="sr-only">Toggle navigation</span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
        </button>

        <!-- Top Navigation: Left Menu -->


        <!-- Top Navigation: Right Menu -->
        <ul class="nav navbar-right navbar-top-links">

            <li class="dropdown navbar-inverse">
                <a class="dropdown-toggle" data-toggle="dropdown" href="#">
                    <i class="fa fa-file-text-o fa-fw"></i> <b class="caret"></b>
                </a>
                <ul class="dropdown-menu dropdown-alerts">
                    <li>
                        <a href="#">
                            <div>
                                <!--<i class="fa fa-comment fa-fw"></i> New Comment
                                <span class="pull-right text-muted small">4 minutes ago</span>-->

                            </div>
                        </a>
                    </li>
                    <li class="divider"></li>
                    <li>
                        <a class="text-center" href="#">
                            <strong>See All Alerts</strong>
                            <i class="fa fa-angle-right"></i>
                        </a>
                    </li>


                </ul>




            </li>
            
            <li class="dropdown navbar-inverse">
                <a class="dropdown-toggle" data-toggle="dropdown" href="#">
                    <i class="fa fa-bell fa-fw"></i> <b class="caret"></b>
                </a>
                <ul class="dropdown-menu dropdown-alerts">
                    <li>
                        <a href="#">
                            <div>
                                <!--<i class="fa fa-comment fa-fw"></i> New Comment
                        <span class="pull-right text-muted small">4 minutes ago</span>-->

                            </div>
                        </a>
                    </li>
                    <li class="divider"></li>
                    <li>
                        <a class="text-center" href="#">
                            <strong>See All Alerts</strong>
                            <i class="fa fa-angle-right"></i>
                        </a>
                    </li>


                </ul>




            </li>

            <li class="dropdown">
                <a class="dropdown-toggle" data-toggle="dropdown" href="#">
                    <i class="fa fa-user fa-fw"></i> Noman Ali <b class="caret"></b>
                </a>
                <ul class="dropdown-menu dropdown-user">
                    <li><a href="#"><i class="fa fa-user fa-fw"></i> User Profile</a>
                    </li>
                    <li><a href="#"><i class="fa fa-gear fa-fw"></i> Settings</a>
                    </li>
                    <li class="divider"></li>
                    <li><a href="#"><i class="fa fa-sign-out fa-fw"></i> Logout</a>
                    </li>
                </ul>
            </li>
        </ul>

        <!-- Sidebar -->
        <div class="navbar-default sidebar" role="navigation">
            <div class="sidebar-nav navbar-collapse">

                <ul class="nav" id="side-menu">
                    <li class="sidebar-search">
                        <div class="input-group custom-search-form">
                            <select style="width:220px;margin-bottom:5px" class="form-control" >

                                <option value="0">Select </option>
                                <option value="0">Ticket Number</option>
                                <option value="0">Voucher Number </option>
                                <option value="0">PNR Number </option>
                            </select>
                        </div>
                       
                        <div class="input-group custom-search-form">
                            <input type="text" class="form-control" placeholder="PNR Search...">
                                <span class="input-group-btn">
                                    <button class="btn btn-primary" type="button">
                                        <i class="fa fa-search"></i>
                                    </button>
                                </span>

                        </div>
                    </li>
                    <li>
                        <a href="#" class="active"><i class="fa fa-sitemap fa-fw"></i> Import Data</a>
                    </li>
                    <li>
                        <a href="#" class="active"><i class="fa fa-umbrella fa-fw"></i> Current Ticketing</a>
                        <a href="#" class="active"><i class="fa fa-lightbulb-o fa-fw"></i> Booking </a>
                        <a href="#" class="active"><i class="fa fa-exchange fa-fw"></i> Advance Ticketing</a>
                        <a href="#" class="active"><i class="fa fa-cloud-download fa-fw"></i> User Closing</a>
                        <a href="#" class="active"><i class="fa fa-cloud-upload fa-fw"></i> Ticket Change</a>
                        <a href="#" class="active"><i class="fa fa-user-md fa-fw"></i> Ticket Refund</a>
                        <a href="#" class="active"><i class="fa fa-stethoscope fa-fw"></i> Live Feeds</a>
                        <a href="#" class="active"><i class="fa fa-suitcase fa-fw"></i> Change Password</a>
                    </li>
                    <li>
                        <a href="#"><i class="fa fa-sitemap fa-fw"></i>Reports <span class="fa arrow"></span></a>
                        <ul class="nav nav-second-level">
                            <li>
                                <a href="#">Advance Report</a>
                                <a href="#">Departure Report</a>
                                <a href="#">User Closing Report</a>
                                <a href="#">E-Ticketing Report</a>
                             
                                <a href="#">Refud Change Details</a>
                            </li>
                           
                        </ul>
                    </li>
                </ul>

            </div>
        </div>
    </nav>

    <!-- Page Content -->
    <div id="page-wrapper">
        <div class="container-fluid">

            <div class="row">
                <div class="col-lg-12">
                    <table class="style1">
         <tr>
             <td>
                 &nbsp;</td>
            <td>

    
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
         <tr>
             <td>
             </td>
             <td class=Generallabel>
                 Schedule</td>
            <td>
 <div class="demo">
<div class="ui-widget">
    
     <asp:DropDownList ID="cboRoute" runat="server" AutoPostBack="True" Width="144px" 
                    Height="16px"></asp:DropDownList>
</div>
</div>
    
            </td>
            <td>
                </td>
            <td>
            <div style="display:none;visibility:hidden" >
                <igsch:WebDateChooser ID="dtSchedule"  AutoPostBack-ValueChanged =true  
                    runat="server" Width="80%" 
                                                                        
                    NullDateLabel="Select Date" AllowNull="False" Editable="False">
<AutoPostBack ValueChanged="True"></AutoPostBack>

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
</div>                                                                    
                
            </td>
            <td class=Generallabel >
                Time</td>
            <td>
 <div class="demo">
<div class="ui-widget">            
                <asp:DropDownList ID="cboTime" runat="server" AutoPostBack="True">
                </asp:DropDownList>
</div>
</div>                
            </td>
            <td>
                &nbsp;</td>
            <td>
                <asp:Button ID="Button1" runat="server" CssClass="ButtonStyle" Text="Load" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        </table>
<asp:hiddenfield ID="SerialNo" runat="server"></asp:hiddenfield>
<asp:hiddenfield ID="Driver_Name" runat="server"></asp:hiddenfield>
<asp:hiddenfield ID="Hostess_Name" runat="server"></asp:hiddenfield>
<asp:hiddenfield ID="Vehicle_ID" runat="server"></asp:hiddenfield>
                </div>
            </div>
            
            <!-- ... Your content goes here ... -->
            <iframe src="" runat=server id="iframe1" style="width:100%;min-height:600px;border:0px"  >  </iframe>
        </div>
    </div>

</div>

<script src="script/jquery.min.js"></script>

<!-- Bootstrap Core JavaScript -->
<script src="script/bootstrap.min.js"></script>

<!-- Metis Menu Plugin JavaScript -->
<script src="script/metisMenu.min.js"></script>

<!-- Custom Theme JavaScript -->
<script src="script/startmin.js"></script>


</body>
</form>  
</html>
