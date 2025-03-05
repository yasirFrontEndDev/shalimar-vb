<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TicketingSchedule.aspx.vb" Inherits="FMovers.Ticketing.UI.TicketingSchedule" %>

<%@ Register tagprefix="cc1" namespace="Infragistics.Web.UI.LayoutControls" Assembly="Infragistics35.Web.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igtbl" namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics35.WebUI.UltraWebGrid.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register tagprefix="igmisc" namespace="Infragistics.WebUI.Misc" Assembly="Infragistics35.WebUI.Misc.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register tagprefix="igsch" namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics35.WebUI.WebDateChooser.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/tr/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Ticketing Schedule</title>
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
        $("#cboServiceType").cboRoute();
            $("#toggle").click(function() {
            $("#cboServiceType").toggle();
            });
        });
	</script>
    
    <script language="javascript">
function loadsearch()
{
    __doPostBack('cboRoute', '');
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
    
    function LoadSchedule(Ticketing_Schedule_ID , Sr_No , Departure_Time , Vehicle_ID , Driver_Name , Hostess_Name)
    {
      document.getElementById("hndTicketing_Schedule_ID").value = Ticketing_Schedule_ID;
      document.getElementById("hndSr_No").value = Sr_No;
      document.getElementById("hndDeparture_Time").value = Departure_Time;
      document.getElementById("hndVehicle_ID").value = Vehicle_ID;
      document.getElementById("hndDriver_Name").value = Driver_Name;
      document.getElementById("hndHostess_Name").value = Hostess_Name;
                                __doPostBack('lnkReserve', '')
    
    
    }
    
    </script>
    <style type="text/css">
        #txtTime
        {
            width:50px;
        }

    </style>
    
    
    <script language=javascript >
     function setfocus()
    {

            var grid = ig_controls["grdVoucher"];
            var filtering = grid.get_behaviors().get_filtering();
            var key = 5;
            var val = "00:12";
            var newFilter = filtering.create_columnFilter(key);
            newFilter.get_condition().set_value(val);
            newFilter.get_condition().set_rule(3);
            filtering.add_columnFilter(newFilter);
            filtering.applyFilters();

    }
    
    function doFilter(filterText) {
var grid = igtbl_getGridById('<%= grdVoucher.ClientID %>');
var rId = 0;
var row = grid.Rows.getRow(rId++);
while (row) {
if (filterText == '')
row.setHidden(false);
else
row.setHidden(!row.find(filterText))
row = grid.Rows.getRow(rId++);
}
}




function CheckTheDuplicateValues()
{
var textvalue = document.getElementById("txtTime").value;
var grid = igtbl_getGridById("grdVoucher");
var rows = grid.Rows;

if (textvalue=="") 
{
    if(rows!=null)
    {
        var len = rows.length;

        for(var i=0; i<rows.length;++i)
        {
            var row = grid.Rows.getRow(i);

                 row.setHidden(false);   
        }
    }

 return false ;
 }
 


    if(rows!=null)
    {
        var len = rows.length;

        for(var i=0; i<rows.length;++i)
        {
            var row = grid.Rows.getRow(i);

           // if(row!=null)
           // {
            var cell = row.getCell(9);
            var cellval = cell.getValue();
            if (cellval!=textvalue)
            {
                 row.setHidden(true);   
                }
            
            //return true;
        }
    }
}


    
    function nauman ()
    {
           // var grid = ig_controls["grdVoucher"];
filterText ="01:00"           ;
    var grid = igtbl_getGridById("grdVoucher");     
    var row = grid.Rows.getRow(0);
CheckTheDuplicateValues()    
    //row.setHidden(true);    
    return false;
   // grid.rows[2].CssClass = "hideMyColumn";
    
       //alert(grid.rows[2])
       // alert(grid.get_rows());

            var filtering = grid.get_behaviors().get_filtering();
            
            var key = 5;
            
            var val = "00:12";
            var newFilter = filtering.create_columnFilter(key);
            
            newFilter.get_condition().set_value(val);
            newFilter.get_condition().set_rule(3);
            filtering.add_columnFilter(newFilter);
            filtering.applyFilters();
            return false;
    }
    
    function UltraWebGrid1_InitializeLayoutHandler(gridName)

{

    var grid = igtbl_getGridById(gridName);
    //alert(grid);
    
    //filterRow.getCell(3).getElement().firstChild.style.display = 'none';

    //var filterRow = grid.Rows.getFilterRow();

//    filterRow.getCell(3).getElement().firstChild.style.display = 'none';

}
    
    function searchme(word)
{

var grid = document.getElementById("grdVoucher_main");
counter = document.getElementById("txtCounter").value;
word = "<NOBR>" + document.getElementById("txtTime").value + "</NOBR>";
counter =  parseInt(counter);



            for (var i = 0; i < counter ; i++) 
            {
                var rows = eval("document.getElementById('grdVoucher_r_"+i+"')");
                var Timetgrd = eval("document.getElementById('grdVoucher_rc_"+i+"_9')");
                alert(Timetgrd.innerHTML);
                 
              
             } 

             
             
return false ;
 
	
}



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

function getElementsByClass(node,searchClass,tag) {
    var classElements = new Array();
    var els = node.getElementsByTagName(tag); // use "*" for all elements
    alert(els.innerHTML);
    var elsLen = els.length;
    var pattern = new RegExp("\b"+searchClass+"\b");
    for (i = 0, j = 0; i < elsLen; i++) {
         if ( pattern.test(els[i].className) ) {
             classElements[j] = els[i];
             j++;
         }
    }
    return classElements;
}

    </script>
    
    </head>
<body onload="pageInit();">
    <form id="form1" runat="server">
    
    
    <asp:HiddenField ID="hndTicketing_Schedule_ID" runat="server" Value="0" />
    <asp:HiddenField ID="hndSr_No" runat="server" Value="0" />
    <asp:HiddenField ID="hndDeparture_Time" runat="server" Value="0" />
    <asp:HiddenField ID="hndVehicle_ID" runat="server" Value="0" />
    <asp:HiddenField ID="hndDriver_Name" runat="server" Value="0" />
    <asp:HiddenField ID="hndHostess_Name" runat="server" Value="0" />
<asp:LinkButton ID="lnkReserve" runat="server"></asp:LinkButton>

                        
    <h3> <asp:Label ID="lblHeading" runat="server" Text="Label"></asp:Label></h3>
    <div style="padding-top:0px;width:100%" >
        
       <div style="float:left;margin-top: 7px; margin-right: 10px;" >
         Select Route :
       </div>

       <div style="float:left" >
        <div class="demo">
         <div class="ui-widget">
                <asp:DropDownList ID="cboRoute" runat="server" AutoPostBack="True"  ></asp:DropDownList>
         </div>
        </div>
        </div>
        
               <div style="float:left;margin-top: 6px;margin-right: 10px;margin-left: 1%" >
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
       
        <div style="float:left;margin-top: 6px;margin-right: 10px;margin-left:1%" >
         Bus Type :
       </div>
       
       <div style="float:left" >
        <div class="demo">
         <div class="ui-widget">
                <asp:DropDownList ID="cboServiceType" runat="server" AutoPostBack="True"  ></asp:DropDownList>
         </div>
        </div>
        </div>
        <div style="float: left;
margin-right: 0px;
margin-left: 0px;" >
         <asp:RadioButtonList ID="rdoMerge" runat="server" RepeatDirection="Horizontal" 
                AutoPostBack="True">
             <asp:ListItem Selected="True" Value="0">All</asp:ListItem>
             <asp:ListItem Value="1">Merge</asp:ListItem>
            </asp:RadioButtonList>
       </div>
       
        <div style="float: left;
margin-top: 5px;
margin-right: 8px;
margin-left: 1%;" >
         Time :
       </div>
          <div style="float:left" >
         &nbsp;</div>
      
       


      
       
            <asp:TextBox ID="txtTime" runat="server" onkeyup="myFunction()"></asp:TextBox>
      
      
       <br />

        <div style="display:flex; justify-content:center;align-items:center; position: absolute;top: 13%;">
          <div>
            <asp:Label ID="lblOperated" runat="server" Text="Operated By :"></asp:Label>
          </div>


             <div>
   
                <asp:DropDownList ID="OperatedDownList" runat="server" AutoPostBack="True" Width="144px" 
                    Height="30px" CssClass="form-control"></asp:DropDownList>
                    
                    </div>

          <div style="float:right;margin-right: 72px;" >
          &nbsp;</div>
       
         </div>

        </div>

               

       
<div style="clear:both" >
</div>

<div style="margin-bottom:40px;">
&nbsp;&nbsp;
</div>


                                            <table width="100%" align="center" border="0" class="tableBorder" cellspacing="0">
                                                                                            
                                                <tr>
                                                    <td id="GirdSearch" valign="middle" style="width: 100%">
                                                      <igmisc:WebAsyncRefreshPanel ID="warpVoucher" runat="server" Width="100%" 
                                                            TriggerControlIDs="cboRoute,dtSchedule">
                                                    <table runat=server id="tbSearch" >
                                                    
                                                    </table>
                                                    

                                                      
                                                            
<igtbl:UltraWebGrid ID="grdVoucher"  runat="server" Height="600px" Visible=false
    Width="100%">
                                                                <bands>
                                                                    <igtbl:UltraGridBand  >
                                                                        <addnewrow view="NotSet" visible="NotSet">
                                                                        </addnewrow>
                                                                    </igtbl:UltraGridBand>
                                                                </bands>
                                                                <displaylayout allowcolsizingdefault="Free" allowcolumnmovingdefault="OnServer" 
            allowdeletedefault="Yes" allowsortingdefault="OnClient" 
            allowupdatedefault="Yes" bordercollapsedefault="Separate" 
            headerclickactiondefault="SortMulti" name="UltraWebGrid1" 
            rowheightdefault="20px" rowselectorsdefault="No" 
            selecttyperowdefault="Extended" stationarymargins="Header" 
            stationarymarginsoutlookgroupby="True" tablelayout="Fixed" version="4.00" 
            viewtype="OutlookGroupBy" cellclickactiondefault="Edit" 
            AllowAddNewDefault="Yes">
                                                                    <framestyle 
                borderstyle="Solid" borderwidth="1px" font-names="Microsoft Sans Serif" 
                font-size="8.25pt" height="500px" width="100%" cssclass="GridFrame">
                                                                        <BorderDetails ColorBottom="#F8F8F8" ColorRight="#F8F8F8" />
                                                                    </framestyle>
                                                                    <RowAlternateStyleDefault BackColor="#FFEFA3" CssClass="GridItem">
                                                                    </RowAlternateStyleDefault>
                                                                    <pager minimumpagesfordisplay="2">
                                                                        <PagerStyle BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                                                                        <borderdetails colorleft="White" colortop="White" widthleft="1px" 
                    widthtop="1px" />
                                                                        </PagerStyle>
                                                                    </pager>
                                                                    <editcellstyledefault borderstyle="None" borderwidth="0px">
                                                                    </editcellstyledefault>
                                                                    <footerstyledefault backcolor="LightGray" borderstyle="Solid" borderwidth="1px">
                                                                        <borderdetails colorleft="White" colortop="White" widthleft="1px" 
                    widthtop="1px" />
                                                                    </footerstyledefault>
                                                                    <headerstyledefault borderstyle="Solid" 
                horizontalalign="Left" backcolor="#EEEEEE" 
                cssclass="GridHeader" font-bold="True" font-names="Verdana" 
                font-size="12pt">
                                                                        <borderdetails colorleft="#666666" colortop="#666666" widthleft="1px" 
                    widthtop="1px" colorbottom="666666" colorright="#666666" />
                                                                    </headerstyledefault>
                                                                    <rowstyledefault backcolor="#FFFFFF" borderstyle="Solid" 
                borderwidth="1px" font-names="Microsoft Sans Serif" font-size="8.25pt" 
                cssclass="GridItem">
                                                                        <padding left="3px" />
                                                                        <borderdetails colorleft="#F8F8F8" colortop="#F8F8F8" />
                                                                    </rowstyledefault>
                                                                    <groupbyrowstyledefault backcolor="Control" bordercolor="Window">
                                                                    </groupbyrowstyledefault>
                                                                    <SelectedRowStyleDefault BackColor="#99CCFF">
                                                                    </SelectedRowStyleDefault>
                                                                    <groupbybox Hidden="True">
                                                                        <boxstyle backcolor="ActiveBorder" bordercolor="Window">
                                                                        </boxstyle>
                                                                    </groupbybox>
                                                                    <addnewbox>
                                                                        <boxstyle backcolor="Window" bordercolor="InactiveCaption" borderstyle="Solid" 
                    borderwidth="1px">
                                                                            <borderdetails colorleft="White" colortop="White" widthleft="1px" 
                        widthtop="1px" />
                                                                        </boxstyle>
                                                                    </addnewbox>
                                                                    <activationobject bordercolor="" borderwidth="">
                                                                    </activationobject>
                                                                    <filteroptionsdefault>
                                                                        <filterdropdownstyle backcolor="White" bordercolor="Silver" borderstyle="Solid" 
                    borderwidth="1px" customrules="overflow:auto;" 
                    font-names="Verdana,Arial,Helvetica,sans-serif" font-size="11px" height="300px" 
                    width="200px">
                                                                            <padding left="2px" />
                                                                        </filterdropdownstyle>
                                                                        <filterhighlightrowstyle backcolor="#151C55" forecolor="White">
                                                                        </filterhighlightrowstyle>
                                                                        <filteroperanddropdownstyle backcolor="White" bordercolor="Silver" 
                    borderstyle="Solid" borderwidth="1px" customrules="overflow:auto;" 
                    font-names="Verdana,Arial,Helvetica,sans-serif" font-size="11px">
                                                                            <padding left="2px" />
                                                                        </filteroperanddropdownstyle>
                                                                    </filteroptionsdefault>
                                                                </displaylayout>
                                                            </igtbl:UltraWebGrid>
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
                                                        &nbsp;</tr>  
                                              </table>
                                              
    <asp:HiddenField ID="totalvalue" Value="0" runat="server" />
   
   
    
    </form>
</body>
</html>
