<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SC_New.aspx.vb" Inherits="FMovers.Ticketing.UI.SC_New"  MasterPageFile="~/main.Master"%>

<%@ Register tagprefix="igsch" namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics35.WebUI.WebDateChooser.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"  runat="server"> 

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
	
	    <script language=javascript >

	        function callByChild() {
	            alert("I am here");
	        }
	        function Validatepass() {

	            if (document.getElementById("txtTicketNumber").value == "") {
	                alert("Please Enter Ticket Number Correctly");
	                document.getElementById("txtTicketNumber").focus();
	                return false;

	            }
	            else {

	                return true ;
	            }

	        }
    
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
	        $("#cboTime").cboTime();
	        $("#toggle").click(function() {
	            $("#cboTime").toggle();
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
    

    

   
     <table class="style1">
         <tr>
             <td>
                 &nbsp;</td>
             <td>
                 &nbsp;</td>
            <td>

    
                &nbsp;</td>
            <td colspan="8">

    
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
                 &nbsp;</td>
             <td>
                 &nbsp;</td>
            <td colspan="12">

    
               <h4> <strong> Ticket Change</strong></h3></td>
            <td>
                &nbsp;</td>
        </tr>
         <tr>
             <td>
                 &nbsp;</td>
             <td>
                 &nbsp;</td>
            <td>

    
                &nbsp;</td>
            <td colspan="8">

    
                <asp:Label ID="lblMessage" runat="server"  
                    Font-Bold="True" Font-Italic="False"  ForeColor = "Red" Font-Size="Medium" ></asp:Label>
             </td>
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
                 &nbsp;</td>
             <td>
             </td>
             <td class=Generallabel>
                 Ticket Number :</td>
             <td class=Generallabel>
                                                        <asp:TextBox ID="txtTicketNumber" runat="server"></asp:TextBox>
                 <asp:ImageButton ID="ImageButton1" runat="server" 
                     ImageUrl="~/images/Search.png" OnClientClick="javascript:return Validatepass();" />
                                                    </td>
             <td>
                 &nbsp;</td>
             <td class=Generallabel>
                 Schedule</td>
            <td>
 <div class="demo">
<div class="ui-widget">
    
     <asp:DropDownList ID="cboRoute" runat="server" AutoPostBack="True" Width="144px" 
                     ></asp:DropDownList>
</div>
</div>
    
            </td>
          <td class=Generallabel> Service Type :
             </td>
            <td>
                              <asp:DropDownList ID="cboServiceType" runat="server" AutoPostBack="True" Width="144px" 
                    Height="16px"></asp:DropDownList>
</td>
            <td>
                &nbsp;</td>
            <td>
                Date :</td>
            <td>
            <div  >
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
                <asp:Button ID="Button1" runat="server" OnClientClick="javascript:return Validatepass();" CssClass="ButtonStyle" Text="Load" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        </table>
<asp:hiddenfield ID="SerialNo" runat="server"></asp:hiddenfield>
<asp:hiddenfield ID="Driver_Name" runat="server"></asp:hiddenfield>
<asp:hiddenfield ID="Hostess_Name" runat="server"></asp:hiddenfield>
<asp:hiddenfield ID="Vehicle_ID" runat="server"></asp:hiddenfield>

    
    <iframe id="iframe1" runat="server" src="" height="800px" style="Border:0px" frameborder=0 width="100%"></iframe>
    

</asp:Content>
