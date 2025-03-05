<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReturnTicketing.aspx.vb" Inherits="FMovers.Ticketing.UI.ReturnTicketing" %>

<%@ Register assembly="Infragistics35.WebUI.WebDateChooser.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.WebSchedule" tagprefix="igsch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/tr/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Return Ticketing</title>
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
	(function( $ ) {
		$.widget( "ui.cboTime", {
			_create: function() {
				var self = this,
					select = this.element.hide(),
					selected = select.children( ":selected" ),
					value = selected.val() ? selected.text() : "";
				var input = this.input = $( "<input>" )
					.insertAfter( select )
					.val( value )
					.autocomplete({
						delay: 0,
						minLength: 0,
						source: function( request, response ) {
							var matcher = new RegExp( $.ui.autocomplete.escapeRegex(request.term), "i" );
							response( select.children( "option" ).map(function() {
								var text = $( this ).text();
								if ( this.value && ( !request.term || matcher.test(text) ) )
									return {
										label: text.replace(
											new RegExp(
												"(?![^&;]+;)(?!<[^<>]*)(" +
												$.ui.autocomplete.escapeRegex(request.term) +
												")(?![^<>]*>)(?![^&;]+;)", "gi"
											), "<strong>$1</strong>" ),
										value: text,
										option: this
									};
							}) );
						},
						select: function( event, ui ) 
						{
						   

							ui.item.option.selected = true;
							self._trigger( "selected", event, {
								item: ui.item.option
							});
                                     //  alert('i am here');									
            						    __doPostBack('cboTime', '')	                                        
						
						},
						change: function( event, ui ) {
							if ( !ui.item ) {
								var matcher = new RegExp( "^" + $.ui.autocomplete.escapeRegex( $(this).val() ) + "$", "i" ),
									valid = false;
									
								select.children( "option" ).each(function() {
									if ( $( this ).text().match( matcher ) ) 
									{

										this.selected = valid = true;
            						 
										return false;
									}
									
								});
								if ( !valid ) {
									// remove invalid value, as it didn't match anything
									$( this ).val( "" );
									select.val( "" );
									input.data( "autocomplete" ).term = "";
									return false;
								}
							}
						}
					})
					.addClass( "ui-widget ui-widget-content ui-corner-left" );

				input.data( "autocomplete" )._renderItem = function( ul, item ) {
					return $( "<li></li>" )
						.data( "item.autocomplete", item )
						.append( "<a>" + item.label + "</a>" )
						.appendTo( ul );
				};

				this.button = $( "<button type='button'>&nbsp;</button>" )
					.attr( "tabIndex", -1 )
					.attr( "title", "Show All Items" )
					.insertAfter( input )
					.button({
						icons: {
							primary: "ui-icon-triangle-1-s"
						},
						text: false
					})
					.removeClass( "ui-corner-all" )
					.addClass( "ui-corner-right ui-button-icon" )
					.click(function() {
						// close if already visible
						if ( input.autocomplete( "widget" ).is( ":visible" ) ) {
							input.autocomplete( "close" );
							return;
						}

						// work around a bug (likely same cause as #5265)
						$( this ).blur();

						// pass empty string as value to search for, displaying all results
						input.autocomplete( "search", "" );
						input.focus();
					});
			},

			destroy: function() {
				this.input.remove();
				this.button.remove();
				this.element.show();
				$.Widget.prototype.destroy.call( this );
			}
		});
	})( jQuery );

	$(function() {
		$( "#cboTime" ).cboTime();
		$( "#toggle" ).click(function() {
			$( "#cboTime" ).toggle();
		});
	});
	</script>
	
	<script>
	(function( $ ) {
		$.widget( "ui.cboRoute", {
			_create: function() {
				var self = this,
					select = this.element.hide(),
					selected = select.children( ":selected" ),
					value = selected.val() ? selected.text() : "";
				var input = this.input = $( "<input>" )
					.insertAfter( select )
					.val( value )
					.autocomplete({
						delay: 0,
						minLength: 0,
						source: function( request, response ) {
							var matcher = new RegExp( $.ui.autocomplete.escapeRegex(request.term), "i" );
							response( select.children( "option" ).map(function() {
								var text = $( this ).text();
								if ( this.value && ( !request.term || matcher.test(text) ) )
									return {
										label: text.replace(
											new RegExp(
												"(?![^&;]+;)(?!<[^<>]*)(" +
												$.ui.autocomplete.escapeRegex(request.term) +
												")(?![^<>]*>)(?![^&;]+;)", "gi"
											), "<strong>$1</strong>" ),
										value: text,
										option: this
									};
							}) );
						},
						select: function( event, ui ) 
						{
						   

							ui.item.option.selected = true;
							self._trigger( "selected", event, {
								item: ui.item.option
							});
						    __doPostBack('cboRoute', '')							
						},
						change: function( event, ui ) {
							if ( !ui.item ) {
								var matcher = new RegExp( "^" + $.ui.autocomplete.escapeRegex( $(this).val() ) + "$", "i" ),
									valid = false;
									
								select.children( "option" ).each(function() {
									if ( $( this ).text().match( matcher ) ) 
									{
										this.selected = valid = true;
										return false;
									}
									
								});
								if ( !valid ) {
									// remove invalid value, as it didn't match anything
									$( this ).val( "" );
									select.val( "" );
									input.data( "autocomplete" ).term = "";
									return false;
								}
							}
						}
					})
					.addClass( "ui-widget ui-widget-content ui-corner-left" );

				input.data( "autocomplete" )._renderItem = function( ul, item ) {
					return $( "<li></li>" )
						.data( "item.autocomplete", item )
						.append( "<a>" + item.label + "</a>" )
						.appendTo( ul );
				};

				this.button = $( "<button type='button'>&nbsp;</button>" )
					.attr( "tabIndex", -1 )
					.attr( "title", "Show All Items" )
					.insertAfter( input )
					.button({
						icons: {
							primary: "ui-icon-triangle-1-s"
						},
						text: false
					})
					.removeClass( "ui-corner-all" )
					.addClass( "ui-corner-right ui-button-icon" )
					.click(function() {
						// close if already visible
						if ( input.autocomplete( "widget" ).is( ":visible" ) ) {
							input.autocomplete( "close" );
							return;
						}

						// work around a bug (likely same cause as #5265)
						$( this ).blur();

						// pass empty string as value to search for, displaying all results
						input.autocomplete( "search", "" );
						input.focus();
					});
			},

			destroy: function() {
				this.input.remove();
				this.button.remove();
				this.element.show();
				$.Widget.prototype.destroy.call( this );
			}
		});
	})( jQuery );

	$(function() {
		$( "#cboRoute" ).cboRoute();
		$( "#toggle" ).click(function() {
			$( "#cboRoute" ).toggle();
		});
	});
	</script>
	
	<script>
	    (function($) {
	        $.widget("ui.cboCity", {
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
					        __doPostBack('cboCity', '')
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
	        $("#cboCity").cboCity();
	        $("#toggle").click(function() {
	            $("#cboCity").toggle();
	        });
	    });
	</script>
	
	<link rel="stylesheet" href="styles/demos.css">
    
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 82px;
        }
        .style3
        {
            font-family: Verdana;
            font-size: 12px;
            color: Black;
            width: 82px;
        }
    </style>
    
    </head>
<body >

    
     <form id="form1" runat="server">

    

   
     <table class="style1">
    <tr class="" >
                                                                <td align="center"  colspan=8 >
                                                                    &nbsp;</td>
                                                            </tr>
    <tr class="HeaderStyle" >
                                                                <td align="center"  colspan=8 >
                                                                <asp:Label ID="lblheader" runat="server" CssClass="Generallabel" 
                                                                        Text="Return Ticketing"></asp:Label>
                                                                    </td>
                                                            </tr>
         <tr>
             <td>
                 &nbsp;</td>
            <td class="style2">

    
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
            <td class="style2">

    
                &nbsp;</td>
            <td>

    
                <asp:Label ID="Label3" runat="server" CssClass="Generallabel" 
                    Text="Travel From"></asp:Label>
             </td>
            <td>
                <asp:Label ID="Label2" runat="server" CssClass="Generallabel" Text="Schedule"></asp:Label>
             </td>
            <td>
                <asp:Label ID="Label1" runat="server" CssClass="Generallabel" Text="Date "></asp:Label>
             </td>
           
            <td>
                <asp:Label ID="Label4" runat="server" CssClass="Generallabel" Text="Time "></asp:Label>
             </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
         <tr>
             <td>
             </td>
             <td class=style3>
    
                 &nbsp;</td>
             <td class=Generallabel>
    
     <asp:DropDownList ID="cboCity" runat="server" AutoPostBack="True" Width="144px" 
                    Height="16px"></asp:DropDownList>
             </td>
            <td>
 <div class="demo">
<div class="ui-widget">
    
     <asp:DropDownList ID="cboRoute" runat="server" AutoPostBack="True" Width="144px" 
                    Height="16px" ></asp:DropDownList>
</div>
</div>
    
            </td>
            <td>
            <div >
                <igsch:WebDateChooser ID="dtSchedule" runat="server" Width="100px" 
                                                                        
                    NullDateLabel="Select Date" AllowNull="False" Editable="False" 
                                                                        MinDate="2010-01-01" 
                    >
                                                                        <AutoPostBack ValueChanged="True" />
                                                                        <DropButton>
                                                                            <Style Font-Names="Verdana"   >
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

    
    <iframe id="iframe1" runat="server" src="" height="700px" width="100%"></iframe>

</form>
   
</body>

</html>
