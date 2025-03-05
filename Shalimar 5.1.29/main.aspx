<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="main.aspx.vb" Inherits="FMovers.Ticketing.UI.main1" MasterPageFile="~/main.Master" %>

<%@ Register tagprefix="igsch" namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics35.WebUI.WebDateChooser.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"  runat="server"> 
  
         
   <link rel="stylesheet" href="styles/jquery.ui.all.css">
	<script src="script/jquery-1.5.1.js"></script>
	<script src="script/jquery.ui.core.js"></script>
	<script src="script/jquery.ui.widget.js"></script>
	<script src="script/jquery.ui.button.js"></script>
	<script src="script/jquery.ui.position.js"></script>
	<script src="script/jquery.ui.autocomplete.js"></script>
	<link rel="stylesheet" href="styles/demos.css">
	<style>
	.ui-button  
	{ 
		margin-left: -2px;
height: 30px;
float: right;
		}
		
		 
	.ui-button-icon-only .ui-button-text { padding: 0.35em; } 
	.ui-autocomplete-input { margin: 0; padding: 0.48em 0 0.47em 0.45em; }
	
	
	.route.text{
    width:250px;
}


.route input[type="text"] {
  width:250px !important;
}
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
    $("#ctl00_ContentPlaceHolder1_cboRoute").cboTime();
        $("#toggle").click(function() {
        $("#ctl00_ContentPlaceHolder1_cboRoute").toggle();
        });
    });
	</script>

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
	    $("#ctl00_ContentPlaceHolder1_cboTime").cboTime();
	        $("#toggle").click(function() {
	        $("#ctl00_ContentPlaceHolder1_cboTime").toggle();
	        });
	    });
	</script>
	
<script>
    (function($) {
    $.widget("ui.cboServiceType", {
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
    $("#ctl00_ContentPlaceHolder1_cboServiceType").cboTime();
        $("#toggle").click(function() {
        $("#ctl00_ContentPlaceHolder1_cboServiceType").toggle();
        });
    });
	</script>
             

    

       <div class="container-fluid">
 <div style="display:none;visibility:hidden" >
</div>               
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
            <div class="row">
                <div class="col-lg-12">
       <div class="topRow">
       
       
               


         <div  style="float:left;padding: 0px 10px 0px 0px;margin-top: 12px;" >
         Select Route : 
         </div>
         <div style="float:left;padding: 0px 10px 0px 0px;margin-top: 7px;"  >
          <div id="route" >
        <div class="demo">
<div class="ui-widget">
   
     <asp:DropDownList ID="cboRoute" runat="server" AutoPostBack="True" Width="144px" 
                    Height="16px"></asp:DropDownList>
                    
                    </div>
</div>
</div>
         </div>
         
          <div  style="float:left;padding: 0px 10px 0px 0px;margin-top: 12px;" >
         Service Type : 
         </div>
         <div style="float:left;padding: 0px 10px 0px 0px;margin-top: 7px;"  >
          <div id="Div1" >
        <div class="demo">
<div class="ui-widget">
   
     <asp:DropDownList ID="cboServiceType" runat="server" AutoPostBack="True" Width="144px" 
                    Height="16px"></asp:DropDownList>
                    
                    </div>
</div>
</div>
         </div>
         <div  style="float:left;padding: 3px 10px 10px 92px;margin-top: 6px;" >
         Time :
         </div>
         
         <div  style="float:left;padding: 0px 10px 0px 0px;margin-top: 5px;" >
           <asp:DropDownList ID="cboTime" runat="server" AutoPostBack="True">
                </asp:DropDownList>
         </div>

            <div  style="float:left;padding: 3px 10px 10px 92px;margin-top: 6px;" >
         <asp:Label ID="lblOperated" runat="server" Text=" Bus Operated By :"></asp:Label>
         </div>
         
 <div  style="float:left;padding: 0px 10px 0px 0px;margin-top: 5px;" >
   
     <asp:DropDownList ID="OperatedDropDownList" runat="server" AutoPostBack="True" Width="144px" 
                    Height="30px" CssClass="form-control"></asp:DropDownList>
                    
                    </div>
           <div  >
     <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="Load" />
         </div>
       </div>

        </div>         
        
         
        </div>
            </div>

<asp:hiddenfield ID="SerialNo" runat="server"></asp:hiddenfield>
<asp:hiddenfield ID="Driver_Name" runat="server"></asp:hiddenfield>
<asp:hiddenfield ID="Hostess_Name" runat="server"></asp:hiddenfield>
<asp:hiddenfield ID="Vehicle_ID" runat="server"></asp:hiddenfield>

	<style>
	.custom-combobox {
		position: relative;
		display: inline-block;
	}
	.custom-combobox-toggle {
		position: absolute;
		top: 0;
		bottom: 0;
		margin-left: -1px;
		padding: 0;
	}
	.custom-combobox-input {
		margin: 0;
		padding: 5px 10px;
	}
	</style>
	
  <iframe id="iframe1" runat="server" src="" height="800px" style="Border:0px" frameborder=0 width="100%"></iframe>

</asp:Content>