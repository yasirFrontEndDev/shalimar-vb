<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SelectCity.aspx.vb" Inherits="FMovers.Ticketing.UI.SelectCity" %>

<%@ Register tagprefix="cc1" namespace="Infragistics.Web.UI.LayoutControls" Assembly="Infragistics35.Web.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register tagprefix="igmisc" namespace="Infragistics.WebUI.Misc" Assembly="Infragistics35.WebUI.Misc.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register tagprefix="igsch" namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics35.WebUI.WebDateChooser.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/tr/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>CITY WISE BOOKING</title>
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
	.ui-button { margin-left: -1px;height:30px }
	.ui-button-icon-only .ui-button-text { padding: 0.35em; } 
	.ui-autocomplete-input { margin: 0; padding: 0.48em 0 0.47em 0.45em; }
	</style>
	<script>
	    function LoadSchedule(Ticketing_Schedule_ID , Sr_No , Departure_Time , Vehicle_ID , Driver_Name , Hostess_Name , RouteID)
    {
  
      document.getElementById("hndTicketing_Schedule_ID").value = Ticketing_Schedule_ID;
      document.getElementById("hndSr_No").value = Sr_No;
      document.getElementById("hndDeparture_Time").value = Departure_Time;
      document.getElementById("hndVehicle_ID").value = Vehicle_ID;
      document.getElementById("hndDriver_Name").value = Driver_Name;
      document.getElementById("hndHostess_Name").value = Hostess_Name;
      document.getElementById("hndRoute").value = RouteID;      
                                __doPostBack('lnkReserve', '')
    
    
    }
</script>
	<script language="javascript"  >
	(function( $ ) {
		$.widget( "ui.cboFromCity", {
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
						    __doPostBack('cboFromCity', '')							
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
		$( "#cboFromCity" ).cboFromCity();
		$( "#toggle" ).click(function() {
			$( "#cboFromCity" ).toggle();
		});
	});
	</script>
    
    
    <script>
        (function($) {
        $.widget("ui.cboToCity", {
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
        $("#cboToCity").cboToCity();
            $("#toggle").click(function() {
            $("#cboToCity").toggle();
            });
        });
	</script>
    
    <script language="javascript">
function loadsearch()
{
    __doPostBack('cboFromCity', '');
    return false;

}


        function Ticketing(TSID)
        {
            document.getElementById("hidTSID").value = TSID;
            __doPostBack('lnkTicketing', '');
            
            //window.location = "Ticketing.aspx?TSID=" + TSID;
        }
        
        function TicketBooking(TSID)
        {   
            document.getElementById("hidTSID").value = TSID;
            __doPostBack('lnkTicketing', '');
            
            //window.location = "Ticketing.aspx?mode=2&TSID=" + TSID;
        }

    function pageInit() {  
                  
    }
    
//    function grdSchedules_AfterRowInsertHandler(gridName, rowId, index){
//    
//        
//        var grid = igtbl_getGridById("grdSchedules");
//        
//    }
    
    
    
    

function myFunction() {
  var input, filter, table, tr, td, i, txtValue;
  input = document.getElementById("txtTime");
  filter = input.value.toUpperCase();
  table = document.getElementById("GirdSearch");
  tr = table.getElementsByTagName("tr");
  for (i = 0; i < tr.length; i++) {
    td = tr[i].getElementsByTagName("td")[0];
    if (td) {
      txtValue = td.textContent || td.innerText;
      if (txtValue.toUpperCase().indexOf(filter) > -1) {
        tr[i].style.display = "";
      } else {
        tr[i].style.display = "none";
      }
    }       
  }
}

    </script>
    <style type="text/css">
        #txtTime
        {
            width: 97px;
        }
    </style>
    
  
  
    
    </head>
<body onload="pageInit();">
    <form id="form1" runat="server">
    
        
    <asp:HiddenField ID="hndTicketing_Schedule_ID" runat="server" Value="0" />
    <asp:HiddenField ID="hndSr_No" runat="server" Value="0" />
    <asp:HiddenField ID="hndDeparture_Time" runat="server" Value="0" />
    <asp:HiddenField ID="hndVehicle_ID" runat="server" Value="0" />
    <asp:HiddenField ID="hndDriver_Name" runat="server" Value="0" />
    <asp:HiddenField ID="hndRoute" runat="server" Value="0" />
    <asp:HiddenField ID="hndHostess_Name" runat="server" Value="0" />
<asp:LinkButton ID="lnkReserve" runat="server"></asp:LinkButton>


    <h3> <asp:Label ID="lblHeading" font-size="20px" runat="server" Text="Label"></asp:Label></h3>
    <div style="padding-top:0px;width:100%" >
       <div style="float:left;margin-top: "0px" margin-right: 35px" >
           From City:
       </div>

         <div style="float:left" >
        <div class="demo">
         <div class="ui-widget">
                <asp:DropDownList ID="cboFromCity" runat="server" AutoPostBack="True"  ></asp:DropDownList>
         </div>
        </div>
        </div>
        
        
        <div style="float:left;margin-top: 0px;margin-right: 10px;margin-left: 35px" >
         To City :
       </div>
       
       <div style="float:left" >
        <div class="demo">
         <div class="ui-widget">
                <asp:DropDownList ID="cboToCity" runat="server" AutoPostBack="True"  ></asp:DropDownList>
         </div>
        </div>
        </div>
       
               <div style="float:left;margin-top: 0px;margin-right: 10px;margin-left: 35px" >
         Voucher Date :
       </div>
       


               <div style="float:left" >
     <igsch:WebDateChooser ID="dtSchedule" runat="server" Width="150px" 
                                                                        NullDateLabel="Select Date" AllowNull="False" Editable="False" 
                                                                        MinDate="2010-01-01">
                                                                        <AutoPostBack ValueChanged="True" />
                                                                        <DropButton>
                                                                            <Style Font-Names="Verdana"   >
                                                                            </Style>
                                                                        </DropButton>
                                                                        <CalendarLayout>
                                                                            <CalendarStyle BackColor="#FFFFFF" Font-Names="Verdana" Font-Size="10pt">
                                                                            </CalendarStyle>
                                                                            <DayHeaderStyle BackColor="#6699FF" />
                                                                        </CalendarLayout>
                                                                    </igsch:WebDateChooser>
       </div>
       
        
       
        <div style="float: left;
margin-top: 0px;
margin-right: 8px;
margin-left: 37px;" >
         Time :
       </div>
          <div style="float:left" >
  <asp:TextBox ID="txtTime" runat="server" onkeyup="myFunction()"></asp:TextBox>
         </div>


</div>
<div style="clear:both" >
</div>

<div >
&nbsp;&nbsp;
</div>


                                            <table width="100%" align="center" border="0" class="tableBorder" cellspacing="0">
                                                                                            
                                                <tr>
                                                    <td id="GirdSearch" valign="middle" style="width: 100%">

                                                        <igmisc:WebAsyncRefreshPanel ID="warpVoucher" runat="server" Width="100%" 
                                                            TriggerControlIDs="cboRoute,dtSchedule">
                                                                 <table runat=server id="tbSearch" >
                                                    
                                                    </table>
                                                        </igmisc:WebAsyncRefreshPanel>

                                                    </td>
                                                </tr>                                                
                                               <tr>
                                                    <td align="right" valign="middle" height="5px">
                                                        &nbsp;
                                                        </td>
                                                </tr>  
                                               <tr>
                                                    <td align="right" valign="middle" height="5px">
                                                        <input id="hidVoucherNo" type="hidden" runat="server" value='' /> 
                                                        <input id="hidTSID" type="hidden" runat="server" value='' />                                                        
                                                        <asp:LinkButton ID="lnkTicketing" runat="server"></asp:LinkButton>
                                                        <asp:Button ID="btnSave" runat="server" Text="Save" 
                                                        CssClass="ButtonStyle"/><asp:Button ID="btnClose" runat="server" Text="Close" 
                                                        CssClass="ButtonStyle" width="100px" Visible="true"  OnClientClick="return nauman();" />
                                                        &nbsp;</tr>  
                                              </table>
                                              
    <asp:HiddenField ID="totalvalue" Value="0" runat="server" />
   
   
    
    </form>
</body>
</html>
