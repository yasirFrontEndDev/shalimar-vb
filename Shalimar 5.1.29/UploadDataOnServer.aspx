<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UploadDataOnServer.aspx.vb" Inherits="FMovers.Ticketing.UI.UploadDataOnServer" %>

<%@ Register tagprefix="cc1" namespace="Infragistics.Web.UI.LayoutControls" Assembly="Infragistics35.Web.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>
<%@ Register tagprefix="igtbl" namespace="Infragistics.WebUI.UltraWebGrid" Assembly="Infragistics35.WebUI.UltraWebGrid.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register tagprefix="igmisc" namespace="Infragistics.WebUI.Misc" Assembly="Infragistics35.WebUI.Misc.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register tagprefix="igsch" namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics35.WebUI.WebDateChooser.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/tr/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
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
	.ui-button { margin-left: -1px; }
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
    
    
    </script>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 262px;
        }
        #txtTime
        {
            width: 97px;
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
                                                        <input id="hidTSID" type="hidden" runat="server" value='' />                                                        
                                                        <input id="hidVoucherNo" type="hidden" runat="server" value='' /> 
    <div>
    
    </div>
                                            <table width="90%" align="center" border="0" class="tableBorder" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="middle">
                                                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                            <tr class="HeaderStyle" >
                                                                <td align="center"  colspan=2 >
                                                                Upload Data On Server
                                                                    </td>
                                                            </tr>
                                                            </table>
                                                    </td>
                                                </tr>                                                
                                                <tr>
                                                    <td align="left" valign="middle">
                                                        <table cellpadding="0" cellspacing="0" class="style1">
                                                            <tr>
                                                                <td>
                                                                    </td>
                                                                <td>
                                                                    </td>
                                                                <td align="right" ><asp:Label ID="Label2" runat="server" 
                                                                        Text="Schedule:" CssClass="Generallabel"></asp:Label>&nbsp;</td>
<td class="style2">
                                                                   
 <div class="demo">
<div class="ui-widget">
 <asp:DropDownList ID="cboRoute" runat="server" AutoPostBack="True" Width="235px"></asp:DropDownList>
</div>
</div><!-- End demo -->
&nbsp;</td>
                                                                <td align="right" ><asp:Label ID="Label1" runat="server" Text="Voucher Date:" CssClass="Generallabel"></asp:Label>&nbsp;</td>
                                                                <td>
                                                                    </td>
                                                                <td>
                                                                    <igsch:WebDateChooser ID="dtSchedule" runat="server" Width="150px" 
                                                                        NullDateLabel="Select Date" AllowNull="False" Editable="False" 
                                                                        MinDate="2010-01-01">
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
                                                                </td>
                                                               <td>
                                                                    </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    </td>
                                                                <td>
                                                                    </td>
                                                                <td align="right" >
                                                                    <asp:Label ID="Label3" runat="server" 
                                                                        Text="Time:" CssClass="Generallabel"></asp:Label></td>
<td class="style2">
                                                                   
                                                                                                                &nbsp;
                                                        <asp:TextBox ID="txtTime" Width="50px" runat="server"></asp:TextBox>
                                                                

                                                        
                                                                        &nbsp;
                                                                

                                                        
                                                                        <asp:Button ID="btnLoad" runat="server" Text="Load" 
                                                            Class="ButtonStyle" Width="80px"   
                                                                        />
                                                                </td>
                                                                <td align="right" >
                                                                    </td>
                                                                <td>
                                                                    </td>
                                                                <td>
                                                                

                                                        
                                                                        <asp:Button ID="btnUpload" runat="server" Text="Upload Data" Class="ButtonStyle"   
                                                                        />
                                                                </td>
                                                               <td>
                                                                    </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>                                                
                                                <tr>
                                                    <td align="left" valign="middle">
                                                        &nbsp;
                                                        <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
                                                        </td>
                                                </tr>                                                
                                                <tr>
                                                    <td align="left" valign="middle">&nbsp;</td>
                                                </tr>                                                
                                                <tr class="HeaderStyle">
                                                    <td align="left" valign="middle">
                                                        <asp:Label ID="lblHeading" runat="server" CssClass="HeaderStyle" Text="Ticketing Schedule"></asp:Label>
                                                        </td>
                                                </tr>                                                
                                                <tr>
                                                    <td id="GirdSearch" valign="middle" style="width: 100%">

                                                        <igmisc:WebAsyncRefreshPanel ID="warpVoucher" runat="server" Width="100%" 
                                                            TriggerControlIDs="cboRoute,dtSchedule">
                                                            <igtbl:UltraWebGrid ID="grdVoucher"  runat="server" Height="600px" 
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
                                                                        <BorderDetails ColorBottom="172, 199, 246" ColorRight="172, 199, 246" />
                                                                    </framestyle>
                                                                    <RowAlternateStyleDefault BackColor="#F9FAFF" CssClass="GridItem">
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
                horizontalalign="Left" backgroundimage="./images/PageBG.jpg" 
                cssclass="GridHeader" font-bold="True" font-names="Verdana" 
                font-size="8pt">
                                                                        <borderdetails colorleft="172, 199, 246" colortop="172, 199, 246" widthleft="1px" 
                    widthtop="1px" colorbottom="172, 199, 246" colorright="172, 199, 246" />
                                                                    </headerstyledefault>
                                                                    <rowstyledefault backcolor="#F9FAFF" borderstyle="Solid" 
                borderwidth="1px" font-names="Microsoft Sans Serif" font-size="8.25pt" 
                cssclass="GridItem">
                                                                        <padding left="3px" />
                                                                        <borderdetails colorleft="172, 199, 246" colortop="172, 199, 246" />
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
                                               </table>
                                              
    <asp:HiddenField ID="totalvalue" Value="0" runat="server" />
    </form>
</body>
</html>
