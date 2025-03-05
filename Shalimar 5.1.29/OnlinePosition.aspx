<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OnlinePosition.aspx.vb" Inherits="FMovers.Ticketing.UI.OnlinePosition"  MasterPageFile="~/main.Master"   %>

<%@ Register assembly="Infragistics35.WebUI.UltraWebGrid.v8.2, Version=8.2.20082.1000, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" namespace="Infragistics.WebUI.UltraWebGrid" tagprefix="igtbl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"  runat="server">

    <div class="col-lg-12">
  <div class="topRow">
  
                                            <table width="99%" align="center" border="0" class="TableBorder" cellspacing="0">
                                            <tr class="HeaderStyle" >
                                                                <td align="center"   >
    <div style="float:left;width:40%" >
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="XX-Large" 
            ForeColor="#0066CC" Text="Arrivals"></asp:Label>
</div>

    <div style="float:left;width:40%" >
        <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="XX-Large" 
            ForeColor="#0066CC" Text="Departures"></asp:Label>
</div>

    
    <div style="float:left;width:40%;height:500px" >
                                                                    <igtbl:UltraWebGrid ID="dgArival" runat="server" Height="100%" Width="100%">
                                                                        <Bands>
                                                                            <igtbl:UltraGridBand>
                                                                                <RowEditTemplate>
                                                                                    <br>
                                                                                    <p align="center">
                                                                                        <input ID="igtbl_reOkBtn" onclick="igtbl_gRowEditButtonClick(event);" 
                                                                                            style="width:50px;" type="button" value="OK"> &nbsp;
                                                                                        <input ID="igtbl_reCancelBtn" onclick="igtbl_gRowEditButtonClick(event);" 
                                                                                            style="width:50px;" type="button" value="Cancel"> </input> </input>
                                                                                    </p>
                                                                                    </br>
                                                                                </RowEditTemplate>
                                                                                <RowTemplateStyle BackColor="White" BorderColor="White" BorderStyle="Ridge">
                                                                                    <BorderDetails WidthBottom="3px" WidthLeft="3px" WidthRight="3px" 
                                                                                        WidthTop="3px" />
                                                                                </RowTemplateStyle>
                                                                                <AddNewRow View="NotSet" Visible="NotSet">
                                                                                </AddNewRow>
                                                                            </igtbl:UltraGridBand>
                                                                        </Bands>
                                                                        <DisplayLayout AllowColSizingDefault="NotSet" 
                                                                            AllowColumnMovingDefault="OnServer" AllowSortingDefault="NotSet" 
                                                                            AllowUpdateDefault="NotSet" BorderCollapseDefault="Separate" 
                                                                            HeaderClickActionDefault="NotSet" Name="UltraWebGrid1" RowHeightDefault="35px" 
                                                                            RowSelectorsDefault="No" SelectTypeRowDefault="Extended" 
                                                                            StationaryMargins="Header" StationaryMarginsOutlookGroupBy="True" 
                                                                            TableLayout="Fixed" Version="4.00" ViewType="OutlookGroupBy">
                                                                            <FrameStyle BorderColor="InactiveCaption" BorderStyle="Solid" BorderWidth="2px" 
                                                                                Font-Names="Verdana" Font-Overline="False" Font-Size="Larger" Height="100%" 
                                                                                Width="100%">
                                                                            </FrameStyle>
                                                                            <Pager MinimumPagesForDisplay="2">
                                                                                <PagerStyle BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                                                                                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                                                    WidthTop="1px" />
                                                                                </PagerStyle>
                                                                            </Pager>
                                                                            <EditCellStyleDefault BorderStyle="None" BorderWidth="0px">
                                                                            </EditCellStyleDefault>
                                                                            <FooterStyleDefault BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                                                                                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                                                    WidthTop="1px" />
                                                                            </FooterStyleDefault>
                                                                            <HeaderStyleDefault BackColor="#0033CC" BackgroundImage="./images/bg_002.gif" 
                                                                                BorderStyle="Solid" Font-Bold="True" ForeColor="White" 
                                                                                HorizontalAlign="Left">
                                                                                <Margin Bottom="10px" Left="10px" Right="10px" Top="10px" />
                                                                                <Padding Bottom="10px" Left="10px" Right="10px" Top="10px" />
                                                                                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                                                    WidthTop="1px" />
                                                                            </HeaderStyleDefault>
                                                                            <RowStyleDefault BackColor="Window" BorderColor="Silver" BorderStyle="Solid" 
                                                                                BorderWidth="1px" Font-Names="Microsoft Sans Serif" Font-Size="8.25pt">
                                                                                <Padding Left="3px" />
                                                                                <BorderDetails ColorLeft="Window" ColorTop="Window" />
                                                                            </RowStyleDefault>
                                                                            <GroupByRowStyleDefault BackColor="Control" BorderColor="Window">
                                                                            </GroupByRowStyleDefault>
                                                                            <GroupByBox Hidden="True">
                                                                                <BoxStyle BackColor="ActiveBorder" BorderColor="Window">
                                                                                </BoxStyle>
                                                                            </GroupByBox>
                                                                            <AddNewBox Hidden="False">
                                                                                <BoxStyle BackColor="Window" BorderColor="InactiveCaption" BorderStyle="Solid" 
                                                                                    BorderWidth="1px">
                                                                                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                                                        WidthTop="1px" />
                                                                                </BoxStyle>
                                                                            </AddNewBox>
                                                                            <ActivationObject BorderColor="" BorderWidth="">
                                                                            </ActivationObject>
                                                                            <FilterOptionsDefault>
                                                                                <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" 
                                                                                    BorderWidth="1px" CustomRules="overflow:auto;" 
                                                                                    Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px" Height="300px" 
                                                                                    Width="200px">
                                                                                    <Padding Left="2px" />
                                                                                </FilterDropDownStyle>
                                                                                <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                                                                                </FilterHighlightRowStyle>
                                                                                <FilterOperandDropDownStyle BackColor="White" BorderColor="Silver" 
                                                                                    BorderStyle="Solid" BorderWidth="1px" CustomRules="overflow:auto;" 
                                                                                    Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px">
                                                                                    <Padding Left="2px" />
                                                                                </FilterOperandDropDownStyle>
                                                                            </FilterOptionsDefault>
                                                                        </DisplayLayout>
                                                                    </igtbl:UltraWebGrid>
    
    </div>

    <div style="float:left;width:40%;height:500px" >
    
                                                                    <igtbl:UltraWebGrid ID="gdDeparture" runat="server" Height="100%" Width="100%">
                                                                        <Bands>
                                                                            <igtbl:UltraGridBand>
                                                                                <RowEditTemplate>
                                                                                    <br>
                                                                                    <p align="center">
                                                                                        <input ID="igtbl_reOkBtn0" onclick="igtbl_gRowEditButtonClick(event);" 
                                                                                            style="width:50px;" type="button" value="OK"> &nbsp;
                                                                                        <input ID="igtbl_reCancelBtn0" onclick="igtbl_gRowEditButtonClick(event);" 
                                                                                            style="width:50px;" type="button" value="Cancel"> </input> </input>
                                                                                    </p>
                                                                                    </br>
                                                                                </RowEditTemplate>
                                                                                <RowTemplateStyle BackColor="White" BorderColor="White" BorderStyle="Ridge">
                                                                                    <BorderDetails WidthBottom="3px" WidthLeft="3px" WidthRight="3px" 
                                                                                        WidthTop="3px" />
                                                                                </RowTemplateStyle>
                                                                                <AddNewRow View="NotSet" Visible="NotSet">
                                                                                </AddNewRow>
                                                                            </igtbl:UltraGridBand>
                                                                        </Bands>
                                                                        <DisplayLayout AllowColSizingDefault="NotSet" 
                                                                            AllowColumnMovingDefault="OnServer" AllowSortingDefault="NotSet" 
                                                                            AllowUpdateDefault="NotSet" BorderCollapseDefault="Separate" 
                                                                            HeaderClickActionDefault="NotSet" Name="UltraWebGrid1" RowHeightDefault="35px" 
                                                                            RowSelectorsDefault="No" SelectTypeRowDefault="Extended" 
                                                                            StationaryMargins="Header" StationaryMarginsOutlookGroupBy="True" 
                                                                            TableLayout="Fixed" Version="4.00" ViewType="OutlookGroupBy">
                                                                            <FrameStyle BorderColor="InactiveCaption" BorderStyle="Solid" BorderWidth="2px" 
                                                                                Font-Names="Verdana" Font-Overline="False" Font-Size="Larger" Height="100%" 
                                                                                Width="100%">
                                                                            </FrameStyle>
                                                                            <Pager MinimumPagesForDisplay="2">
                                                                                <PagerStyle BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                                                                                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                                                    WidthTop="1px" />
                                                                                </PagerStyle>
                                                                            </Pager>
                                                                            <EditCellStyleDefault BorderStyle="None" BorderWidth="0px">
                                                                            </EditCellStyleDefault>
                                                                            <FooterStyleDefault BackColor="LightGray" BorderStyle="Solid" BorderWidth="1px">
                                                                                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                                                    WidthTop="1px" />
                                                                            </FooterStyleDefault>
                                                                            <HeaderStyleDefault BackColor="#0033CC" BackgroundImage="./images/bg_002.gif" 
                                                                                BorderStyle="Solid" Font-Bold="True" ForeColor="White" 
                                                                                HorizontalAlign="Left">
                                                                                <Margin Bottom="10px" Left="10px" Right="10px" Top="10px" />
                                                                                <Padding Bottom="10px" Left="10px" Right="10px" Top="10px" />
                                                                                <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                                                    WidthTop="1px" />
                                                                            </HeaderStyleDefault>
                                                                            <RowStyleDefault BackColor="Window" BorderColor="Silver" BorderStyle="Solid" 
                                                                                BorderWidth="1px" Font-Names="Microsoft Sans Serif" Font-Size="8.25pt">
                                                                                <Padding Left="3px" />
                                                                                <BorderDetails ColorLeft="Window" ColorTop="Window" />
                                                                            </RowStyleDefault>
                                                                            <GroupByRowStyleDefault BackColor="Control" BorderColor="Window">
                                                                            </GroupByRowStyleDefault>
                                                                            <GroupByBox Hidden="True">
                                                                                <BoxStyle BackColor="ActiveBorder" BorderColor="Window">
                                                                                </BoxStyle>
                                                                            </GroupByBox>
                                                                            <AddNewBox Hidden="False">
                                                                                <BoxStyle BackColor="Window" BorderColor="InactiveCaption" BorderStyle="Solid" 
                                                                                    BorderWidth="1px">
                                                                                    <BorderDetails ColorLeft="White" ColorTop="White" WidthLeft="1px" 
                                                                                        WidthTop="1px" />
                                                                                </BoxStyle>
                                                                            </AddNewBox>
                                                                            <ActivationObject BorderColor="" BorderWidth="">
                                                                            </ActivationObject>
                                                                            <FilterOptionsDefault>
                                                                                <FilterDropDownStyle BackColor="White" BorderColor="Silver" BorderStyle="Solid" 
                                                                                    BorderWidth="1px" CustomRules="overflow:auto;" 
                                                                                    Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px" Height="300px" 
                                                                                    Width="200px">
                                                                                    <Padding Left="2px" />
                                                                                </FilterDropDownStyle>
                                                                                <FilterHighlightRowStyle BackColor="#151C55" ForeColor="White">
                                                                                </FilterHighlightRowStyle>
                                                                                <FilterOperandDropDownStyle BackColor="White" BorderColor="Silver" 
                                                                                    BorderStyle="Solid" BorderWidth="1px" CustomRules="overflow:auto;" 
                                                                                    Font-Names="Verdana,Arial,Helvetica,sans-serif" Font-Size="11px">
                                                                                    <Padding Left="2px" />
                                                                                </FilterOperandDropDownStyle>
                                                                            </FilterOptionsDefault>
                                                                        </DisplayLayout>
                                                                    </igtbl:UltraWebGrid>
    
    </div>
                                                                </td>
</tr>
</table>


</div>

</div>
</asp:Content>