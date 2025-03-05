<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NextDeparture.aspx.vb" Inherits="FMovers.Ticketing.UI.NextDeparture" MasterPageFile="~/main.Master"%>

<%@ Register tagprefix="igsch" namespace="Infragistics.WebUI.WebSchedule" Assembly="Infragistics35.WebUI.WebDateChooser.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" %>

<%@ Register assembly="Infragistics35.WebUI.UltraWebGrid.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.UltraWebGrid" tagprefix="igtbl" %>

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
        $("#ctl00_ContentPlaceHolder1_cboRoute").cboTime();
        $("#toggle").click(function() {
            $("#ctl00_ContentPlaceHolder1_cboRoute").toggle();
        });
    });

    function rdodata() {

      
        var rdo = $('#<%=rdoTicketTracking.ClientID %> input[type=radio]:checked').val();
         if (rdo == "Bookkaru") {

            
             txtTicketNumber.Attributes.Add("placeholder", "Enter Bookkaru PNR No:");
         }

         else {


             txtTicketNumber.Attributes.Add("placeholder", "Enter Ticket Number:");


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
	        $("#ctl00_ContentPlaceHolder1_cboTime").cboTime();
	        $("#toggle").click(function() {
	            $("#ctl00_ContentPlaceHolder1_cboTime").toggle();
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
             <td>&nbsp;                 </td>
             <td>&nbsp;                 </td>
            <td>&nbsp;                </td>
            <td colspan="3">&nbsp;                </td>
            <td>&nbsp;                </td>
            <td>&nbsp;                </td>
            <td>&nbsp;                </td>
        </tr>
         <tr>
             <td>&nbsp;                 </td>
             <td>&nbsp;                 </td>
            <td colspan="6">

    
               <h4> <strong> Next Departure </strong></h3></td>
            <td>&nbsp;                </td>
        </tr>
         <tr>
             <td>&nbsp;                 </td>
             <td>&nbsp;                 </td>
             
            <td colspan="6">

    
                <asp:Label ID="lblMessage" runat="server"  
                    Font-Bold="True" Font-Italic="False"  ForeColor = "Red" Font-Size="Medium" ></asp:Label>             </td>
            
            <td>&nbsp;                </td>
        </tr>
         <tr>
             <td>&nbsp;                 </td>
             <td>&nbsp;                 </td>
            <td>&nbsp;                </td>
         
              <td align="left">
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                    <asp:HiddenField ID="hndTimeDrop" runat="server" Value="0" />
                    <asp:HiddenField ID="hndOnlineTSNo" runat="server" />
                    <asp:HiddenField ID="hndDisbaleCount" runat="server" />
                    <asp:RadioButtonList ID="rdoTicketTracking"  OnClick="rdodata()" runat="server" Font-Bold="True" 
                        Font-Names="verdana" Font-Size="12px" RepeatDirection="Horizontal" 
                        Width="221px">
                        <asp:ListItem selected="True">Counter Ticketing</asp:ListItem>
                        <asp:ListItem>Bookkaru</asp:ListItem>
                     <%--   <asp:ListItem>Disable</asp:ListItem>--%>
                    </asp:RadioButtonList>
                </td>

        <td><span class="Generallabel">Schedule</span>                </td>
        <td>&nbsp;                </td>
           <td><span class="Generallabel">Service Type :</span>                </td>
        <td>Date            </td>
            <td><span class="Generallabel">Time</span></td>
       </tr>
         <tr>
             <td>&nbsp;                 </td>
             <td>             </td>
             <td class=Generallabel>           </td>
             <td class=Generallabel>
                                                        <asp:TextBox ID="txtTicketNumber" runat="server"></asp:TextBox>
                 <asp:ImageButton ID="ImageButton1" runat="server" 
                     ImageUrl="~/images/Search.png"  OnClientClick="javascript:return Validatepass();" />                                                    </td>
           <td>
 <div class="demo">
<div class="ui-widget">
    
     <asp:DropDownList ID="cboRoute" runat="server" AutoPostBack="True" Width="144px" 
                     ></asp:DropDownList>
</div>
</div>            </td>
          <td class=Generallabel>&nbsp;</td>
<td>
                              <asp:DropDownList ID="cboServiceType" runat="server" AutoPostBack="True" Width="144px" 
                    Height="16px"></asp:DropDownList></td>
            <td><div  >
                <igsch:WebDateChooser ID="dtSchedule"  AutoPostBack-ValueChanged =true  
                    runat="server" Width="80%" 
                                                                        
                    NullDateLabel="Select Date" AllowNull="False" Editable="False">
<AutoPostBack ValueChanged="True"></AutoPostBack>

                                                                        <DropButton>
                                                                            <Style Font-Names="Verdana">
                                                                            </Style>
                                                                        </DropButton>
                                                                        <CalendarLayout>
                                                                            <CalendarStyle BackColor="#F9FAFF" Font-Names="Verdana" Font-Size="10pt">                                                                            </CalendarStyle>
                                                                            <DayHeaderStyle BackColor="#6699FF" />
                                                                        </CalendarLayout>
              </igsch:WebDateChooser>
</div></td>
           <td>
            <div class="demo">
<div class="ui-widget">
           <span class="ui-widget">
             <asp:DropDownList ID="cboTime" runat="server" AutoPostBack="True"> </asp:DropDownList>
           </span>        </div> </div>   </td>
            <td class=Generallabel ><asp:Button ID="Button1" style="width:50px" runat="server" onclientclick="javascript:return Validatepass();" CssClass="ButtonStyle" Text="Load" /></td>
        </tr>
         <tr>
             <td>&nbsp;                 </td>
             <td>&nbsp;                 </td>
             <td class=Generallabel>&nbsp;                 </td>
             <td class=Generallabel>&nbsp;                                                        </td>
            <td>&nbsp;                </td>
            <td>&nbsp;                </td>
            <td>&nbsp;                </td>
            <td>&nbsp;                </td>
            <td>&nbsp;                </td>
            <td>&nbsp;                </td>
        </tr>
         <tr>
             <td>&nbsp;                 </td>
             <td>&nbsp;                 </td>
             <td class=Generallabel colspan="8">
                                                    
                                                    <igtbl:UltraWebGrid ID="grdRoutes"  runat="server" 
    Width="100%">
                                                                <bands>
                                                                    <igtbl:UltraGridBand  >
                                                                        <addnewrow view="NotSet" visible="NotSet">                                                                        </addnewrow>
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
                font-size="8.25pt" width="100%" cssclass="GridFrame">
                                                                        <BorderDetails ColorBottom="#F8F8F8" ColorRight="#F8F8F8" />
                                                                    </framestyle>
                                                                    <RowAlternateStyleDefault BackColor="#FFEFA3" CssClass="GridItem">                                                                    </RowAlternateStyleDefault>
                                                                    <pager minimumpagesfordisplay="2">
                                                                        <PagerStyle BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                                                                        <borderdetails colorleft="White" colortop="White" widthleft="1px" 
                    widthtop="1px" />
                                                                        </PagerStyle>
                                                                    </pager>
                                                                    <editcellstyledefault borderstyle="None" borderwidth="0px">                                                                    </editcellstyledefault>
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
                                                                    <groupbyrowstyledefault backcolor="Control" bordercolor="Window">                                                                    </groupbyrowstyledefault>
                                                                    <SelectedRowStyleDefault BackColor="#99CCFF">                                                                    </SelectedRowStyleDefault>
                                                                    <groupbybox Hidden="True">
                                                                        <boxstyle backcolor="ActiveBorder" bordercolor="Window">                                                                        </boxstyle>
                                                                    </groupbybox>
                                                                    <addnewbox>
                                                                        <boxstyle backcolor="Window" bordercolor="InactiveCaption" borderstyle="Solid" 
                    borderwidth="1px">
                                                                            <borderdetails colorleft="White" colortop="White" widthleft="1px" 
                        widthtop="1px" />
                                                                        </boxstyle>
                                                                    </addnewbox>
                                                                    <activationobject bordercolor="" borderwidth="">                                                                    </activationobject>
                                                                    <filteroptionsdefault>
                                                                        <filterdropdownstyle backcolor="White" bordercolor="Silver" borderstyle="Solid" 
                    borderwidth="1px" customrules="overflow:auto;" 
                    font-names="Verdana,Arial,Helvetica,sans-serif" font-size="11px" height="300px" 
                    width="200px">
                                                                            <padding left="2px" />
                                                                        </filterdropdownstyle>
                                                                        <filterhighlightrowstyle backcolor="#151C55" forecolor="White">                                                                        </filterhighlightrowstyle>
                                                                        <filteroperanddropdownstyle backcolor="White" bordercolor="Silver" 
                    borderstyle="Solid" borderwidth="1px" customrules="overflow:auto;" 
                    font-names="Verdana,Arial,Helvetica,sans-serif" font-size="11px">
                                                                            <padding left="2px" />
                                                                        </filteroperanddropdownstyle>
                                                                    </filteroptionsdefault>
                                                                </displaylayout>
               </igtbl:UltraWebGrid>                                                    </td>
        </tr>
    </table>
<asp:hiddenfield ID="SerialNo" runat="server"></asp:hiddenfield>
<asp:hiddenfield ID="Driver_Name" runat="server"></asp:hiddenfield>
<asp:hiddenfield ID="Hostess_Name" runat="server"></asp:hiddenfield>
<asp:hiddenfield ID="Vehicle_ID" runat="server"></asp:hiddenfield>

    
<asp:hiddenfield ID="hndNDFare" runat="server"></asp:hiddenfield>
<asp:hiddenfield ID="hndDestinationId" runat="server"></asp:hiddenfield>
<asp:hiddenfield ID="hndoldTicketing_SeatId" runat="server"></asp:hiddenfield>
    
    <iframe id="iframe1" runat="server" src="" height="800px" style="Border:0px" frameborder=0 width="100%"></iframe>
    

</asp:Content>