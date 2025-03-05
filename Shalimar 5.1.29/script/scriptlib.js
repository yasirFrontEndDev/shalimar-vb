
function openPopup(Url, parentWindow, strparam) {    
    if (strparam == "")
        strparam = "center:yes;dialogHeight:500px;dialogWidth:700px;status:no";
    
    newWin = parentWindow.showModalDialog(Url, strparam);
}