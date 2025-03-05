<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LoadSingleVoucher.aspx.vb" Inherits="FMovers.Ticketing.UI.LoadSingleVoucher" MasterPageFile="~/main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"  runat="server">
<script language=javascript>
    function getUrlParameter(name) {
        name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
        var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
        var results = regex.exec(location.search);
        return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
    };
    
</script>
    <div class="col-lg-12">
  <div class="topRow">
    <iframe id="iframe1" runat="server" height="800px" style="Border:0px" frameborder=0 width="100%"></iframe>
</div>

</div>
<script language=javascript >
    var str_TSID = getUrlParameter('TSID');
    window.frames["ctl00_ContentPlaceHolder1_iframe1"].src = "Ticketing.aspx?mode=1&TSId=" + str_TSID;
 </script>

</asp:Content>
