// <script>

/*
=============================================================
WebIntelligence(r) Report Panel
Copyright(c) 2001-2003 Business Objects S.A.
All rights reserved

Use and support of this software is governed by the terms
and conditions of the software license agreement and support
policy of Business Objects S.A. and/or its subsidiaries. 
The Business Objects products and technology are protected
by the US patent number 5,555,403 and 6,247,008

File: dialog.js
Dialog boxes
=============================================================
*/

DialogBoxWidget_modals=new Array;
DialogBoxWidget_instances=new Array
DialogBoxWidget_current=null

_promptDlgInfo=0
_promptDlgWarning=1
_promptDlgCritical=2

// ================================================================================
// ================================================================================
//
// OBJECT newDialogBoxWidget (Constructor)
//
// Class for dialog box
//
// ================================================================================
// ================================================================================

function newDialogBoxWidget(id,title,width,height,defaultCB,cancelCB,noCloseButton)
// CONSTRUCTOR
// id        [String]            the id for DHTML processing
// title     [String]            the caption dialog text
// width     [int optional]      dialog box width
// height    [int optional]      dialog box height
// defaultCB [Function optional] callback when enter key is hit
// cancelCB  [Function optional] callback when esc key is hit or close button pushed
// noCloseButton  [Boolean optional] if true no close (on top right) button is displayed
{
	var o=newWidget(id)

	o.title=title
	o.width=width
	o.height=height
	o.defaultCB=defaultCB
	o.cancelCB=cancelCB
	o.noCloseButton=noCloseButton?noCloseButton:false
	o.resizeable=false
	o.oldMouseDown=null
	o.oldCurrent=null
	o.modal=null
	o.hiddenVis=new Array
	o.lastLink=null
	o.firstLink=null
	o.titleLayer = null
	o.defaultBtn=null
	o.divLayer=null
	
	o.oldInit=o.init
	o.oldShow=o.show
	o.init=DialogBoxWidget_init
	o.setResize=DialogBoxWidget_setResize
	o.beginHTML=DialogBoxWidget_beginHTML
	o.endHTML=DialogBoxWidget_endHTML
	o.show=DialogBoxWidget_Show
	o.center=DialogBoxWidget_center
	o.focus=DialogBoxWidget_focus
	o.setTitle=DialogBoxWidget_setTitle
	o.getContainerWidth=DialogBoxWidget_getContainerWidth
	o.getContainerHeight=DialogBoxWidget_getContainerHeight
	DialogBoxWidget_instances[id]=o
	o.modal=newWidget('modal_'+id)
	o.placeIframe=DialogBoxWidget_placeIframe	
	o.oldResize=o.resize
	o.resize=DialogBoxWidget_resize
	o.attachDefaultButton=DialogBoxWidget_attachDefaultButton
	o.unload=DialogBoxWidget_unload

	if (!_ie)
	{
		if (o.width!=null) o.width=Math.max(0,width+4)
		if (o.height!=null) o.height=Math.max(0,height+4)
	}
		
	return o
}

// ================================================================================

function DialogBoxWidget_setResize(resizeCB,minWidth,minHeight,noResizeW,noResizeH)
// Allow the dialog box to be resized
// resizeCB  [Function] callback for resize
// minWidth  [int]      minimum dialog width
// minHeight [int]      minimum dialog height
// noResizeW [boolean]  disable vertical resizing
// noResizeH [boolean]  disable horizontal resizing
{
	var o=this;
	o.resizeable=true
	o.resizeCB=resizeCB
	o.minWidth=minWidth?minWidth:50
	o.minHeight=minHeight?minHeight:50
	
	o.noResizeW=noResizeW
	o.noResizeH=noResizeH
}

// ================================================================================

function DialogBoxWidget_setTitle(title)
// Change the dialog title
// title [String] the new title
{
	var o=this	
	o.title=title
	if (o.titleLayer == null)
		o.titleLayer = getLayer('titledialog_'+this.id);
	o.titleLayer.innerHTML=convStr(title)	
}

// ================================================================================

function DialogBoxWidget_setCloseIcon(lyr,isActive)
// PRIVATE
{
	changeOffset(lyr,0,(isActive==1?0:18))
}

// ================================================================================

function DialogBoxWidget_beginHTML()
// Get the widget beginning HTML
// Returns : [String] the HTML
{
	with (this)
	{
		var moveableCb=' onselectstart="return false" ondragstart="return false" onmousedown="'+_codeWinName+'.DialogBoxWidget_down(event,\''+id+'\',this,false);return false;" '
		var mdl='<div onselectstart="return false" ondragstart="return false" onmousedown="'+_codeWinName+'.eventCancelBubble(event)" border="0" hspace="0" vspace="0" src="'+_skin+'../transp.gif" id="modal_'+id+'" style="position:absolute;top:0px;left:0px;width:1px;height:1px">'+(_ie?img(_skin+'../transp.gif','100%','100%',null):'')+'</div>'
		var titleBG="background-image:url("+_skin+"dialogtitle.gif)"
				
		return mdl+
			'<a style="position:absolute;left:-30px;top:-30px; visibility:hidden" id="firstLink_'+this.id+'" href="javascript:void(0)" onfocus="'+_codeWinName+'.DialogBoxWidget_keepFocus(\''+this.id+'\');return false;" ></a>'+
			'<table dir="ltr" border="0" cellspacing="0" cellpadding="2" id="'+id+'" style="display:none;padding:0px;visibility:'+_hide+';position:absolute;'+sty("width",width?(""+width+"px"):null)+sty("height",height?(""+height+"px"):null)+'"><tr><td id="td_dialog_'+id+'" onresize="'+_codeWinName+'.DialogBoxWidget_resizeIframeCB(\''+id+'\',this)" class="dialogbox" valign="top">'+
			'<table width="100%" style="margin-bottom:4px" border="0" cellspacing="0" cellpadding="0"><tr valign="top">'+
			'<td '+moveableCb+' style="cursor:move;'+titleBG+'" class="titlezone">'+getSpace(5,18)+'</td>'+
			'<td '+moveableCb+' style="cursor:move;'+titleBG+'" class="titlezone" width="100%" valign="middle" align="left"><nobr><span id="titledialog_'+id+'" tabIndex="0" class="titlezone">'+convStr(title)+'</span></nobr></td>'+
			'<td class="titlezone" style="'+titleBG+'">'+
			(noCloseButton?'':'<a href="javascript:void(0)" onclick="'+_codeWinName+'.DialogBoxWidget_close(\''+id+'\');return false;" title="'+ _closeDialog +'">'+imgOffset(_skin+'dialogelements.gif',18,18,0,18,'dialogClose_'+this.id,'onmouseover="'+_codeWinName+'.DialogBoxWidget_setCloseIcon(this,1)" onmouseout="'+_codeWinName+'.DialogBoxWidget_setCloseIcon(this,0)" ',_closeDialog,'cursor:'+_hand)+'</a>')+
			'</td>'+
			'</tr></table><div id="div_dialog_'+id+'">'
	}
}
/*
function DialogBoxWidget_beginHTML()
// Get the widget beginning HTML
// Returns : [String] the HTML
{
	with (this)
	{
		var moveableCb=' onselectstart="return false" ondragstart="return false" onmousedown="'+_codeWinName+'.DialogBoxWidget_down(event,\''+id+'\',this,false);return false;" '
		var mdl='<div onselectstart="return false" ondragstart="return false" onmousedown="'+_codeWinName+'.eventCancelBubble(event)" border="0" hspace="0" vspace="0" src="'+_skin+'../transp.gif" id="modal_'+id+'" style="position:absolute;top:0px;left:0px;width:1px;height:1px">'+(_ie?img(_skin+'../transp.gif','100%','100%',null):'')+'</div>'
		var titleBG="background-image:url("+_skin+"dialogtitle.gif)"		
		
		return mdl+
			'<a style="position:absolute;left:-30px;top:-30px; visibility:hidden" id="firstLink_'+this.id+'" href="javascript:void(0)" onfocus="'+_codeWinName+'.DialogBoxWidget_keepFocus(\''+this.id+'\');return false;" ></a>'+
			'<table border="0" cellspacing="0" cellpadding="2" id="'+id+'" style="display:none;padding:0px;visibility:'+_hide+';position:absolute;top:-2000px;left:-2000px;'+sty("width",width?(""+width+"px"):null)+sty("height",height?(""+height+"px"):null)+'"><tr><td id="td_dialog_'+id+'" onresize="'+_codeWinName+'.DialogBoxWidget_resizeIframeCB(\''+id+'\',this)" class="dialogbox" valign="top">'+
			'<table width="100%" style="margin-bottom:4px" border="0" cellspacing="0" cellpadding="0"><tr valign="top">'+
			'<td '+moveableCb+' style="cursor:move;'+titleBG+'" class="titlezone">'+getSpace(5,18)+'</td>'+
			'<td '+moveableCb+' style="cursor:move;'+titleBG+'" class="titlezone" width="100%" valign="middle" align="left"><nobr><span id="titledialog_'+id+'" tabIndex="0" class="titlezone">'+convStr(title)+'</span></nobr></td>'+
			'<td class="titlezone" style="'+titleBG+'">'+
			(noCloseButton?'':'<a href="javascript:void(0)" onclick="'+_codeWinName+'.DialogBoxWidget_close(\''+id+'\');return false;" title="'+ _closeDialog +'">'+imgOffset(_skin+'dialogelements.gif',18,18,0,18,'dialogClose_'+this.id,'onmouseover="'+_codeWinName+'.DialogBoxWidget_setCloseIcon(this,1)" onmouseout="'+_codeWinName+'.DialogBoxWidget_setCloseIcon(this,0)" ',_closeDialog,'cursor:'+_hand)+'</a>')+
			'</td>'+
			'</tr></table>'
	}
}
*/

// ================================================================================

function DialogBoxWidget_endHTML()
// Get the widget end HTML
// Returns : [String] the HTML
{
	//var moveableCb=' onselectstart="return false" ondragstart="return false" onmousedown="'+_codeWinName+'.DialogBoxWidget_down(event,\''+this.id+'\',this,true);return false;" '
	//var resz=this.resizeable?('<tr  onselectstart="return false" height="18" valign="bottom" align="right"><td>'+img(_skin+"resize.gif",14,14,null, moveableCb + ' style="cursor:NW-resize" ')+'</td></tr>'):''
	return '</div></td></tr></table><a style="position:absolute;left:-30px;top:-30px; visibility:hidden" id="lastLink_'+this.id+'" href="javascript:void(0)" onfocus="'+_codeWinName+'.DialogBoxWidget_keepFocus(\''+this.id+'\');return false;" ></a>' 
}

// ================================================================================

function DialogBoxWidget_getContainerWidth()
// Get the width of the container
// Returns : [int] the width
{
	var o=this
	return o.width-(2+2)
}

// ================================================================================

function DialogBoxWidget_getContainerHeight() 
// Get the height of the container
// Returns : [int] the height
{
	var o=this
	return o.height-(2+18+2+2+2)
}


// ================================================================================

function DialogBoxWidget_close(id) 
// PRIVATE
{
	var o=DialogBoxWidget_instances[id]
	if (o)
	{
		o.show(false)
		if(o.cancelCB!=null) o.cancelCB()
	}
}

// ================================================================================

function DialogBoxWidget_resizeIframeCB(id,lyr)
{
	DialogBoxWidget_instances[id].placeIframe()
}

// ================================================================================

function DialogBoxWidget_placeIframe()
{
	var o=this
	if (o.iframe)
	{
		var lyr=o.td_lyr
		if (lyr==null)
		{
			o.td_lyr=lyr=getLayer("td_dialog_"+o.id)
		}
		o.iframe.resize(lyr.offsetWidth,lyr.offsetHeight)
		o.iframe.move(o.layer.offsetLeft,o.layer.offsetTop)
	}
}

// ================================================================================
//use when resize dialogbox by code (exemple: variableEditorDialog.html)
function DialogBoxWidget_resize(w,h)
{
	var o=this;
	o.oldResize(w,h);
	if (o.iframe)
	{
		o.iframe.resize(w,h);
	}	
}

// ================================================================================

function DialogBoxWidget_init()
// Init the widget layers
// Return [void]
{
	if (this.layer!=null)
		return

	var o=this
	o.oldInit();
	o.modal.init();
	o.lastLink=newWidget("lastLink_"+o.id)
	o.firstLink=newWidget("firstLink_"+o.id)
	o.lastLink.init()
	o.firstLink.init()
	if (!o.noCloseButton)
	{
		o.closeButton=getLayer('dialogClose_'+o.id)
		DialogBoxWidget_setCloseIcon(o.closeButton,false)
	}
}

// ================================================================================

function DialogBoxWidget_attachDefaultButton(btn)
// Attach a default button to the dialogbox
//the callback of this button will be call by Enter action
{	
	this.defaultBtn=btn;	
	this.defaultBtn.setDefaultButton();
}

// ================================================================================

_theLYR=null
_dlgResize=null

function DialogBoxWidget_down(e,id,obj,isResize)
// PRIVATE
{
	_dlgResize=isResize
	var o=DialogBoxWidget_instances[id],lyr=o.layer,mod=o.modal.layer

	lyr.onmousemove=mod.onmousemove=eval('_curWin.'+_codeWinName+'.DialogBoxWidget_move')
	lyr.onmouseup=mod.onmouseup=eval('_curWin.'+_codeWinName+'.DialogBoxWidget_up')

	lyr.dlgStartPosx=mod.dlgStartPosx=parseInt(lyr.style.left)
	lyr.dlgStartPosy=mod.dlgStartPosy=parseInt(lyr.style.top)
	
	lyr.dlgStartx=mod.dlgStartx=eventGetX(e)
	lyr.dlgStarty=mod.dlgStarty=eventGetY(e)
	
	lyr.dlgStartw=mod.dlgStartw=o.getWidth()
	lyr.dlgStarth=mod.dlgStarth=o.getHeight()

	lyr._widget=mod._widget=o.widx
	
	_theLYR=lyr
	
	eventCancelBubble(e)
	
	if (lyr.setCapture)
		lyr.setCapture(true)
}

// ================================================================================

function DialogBoxWidget_move(e)
// PRIVATE
{
	var o=_theLYR,dlg=getWidget(o)

	if (_dlgResize)
	{
		var newW=Math.max(dlg.minWidth,o.dlgStartw+eventGetX(e)-o.dlgStartx)
		var newH=Math.max(dlg.minHeight,o.dlgStarth+eventGetY(e)-o.dlgStarty)
		
		dlg.resize(dlg.noResizeW?null:newW,dlg.noResizeH?null:newH)
		
		if (dlg.firstTR)
		{
			if (!dlg.noResizeW)
				dlg.firstTR.style.width=newW-4

			if (!dlg.noResizeH)
				dlg.secondTR.style.height=newH-44
		}
		
		if (dlg.resizeCB)
			dlg.resizeCB(newW,newH)
	}
	else
	{
		var x=Math.max(0,o.dlgStartPosx-o.dlgStartx+eventGetX(e))
		var y=Math.max(0,o.dlgStartPosy-o.dlgStarty+eventGetY(e))
		
		//x = Math.min( Math.max(10,winWidth()-10), x)
		//y = Math.min( Math.max(10,winHeight()-18), y)
		
		
		dlg.iframe.move(x,y)
		dlg.move(x,y)
	}
	eventCancelBubble(e)
	return false
}

// ================================================================================

function DialogBoxWidget_up(e)
// PRIVATE
{
	var o=getWidget(_theLYR),lyr=o.layer,mod=o.modal.layer;
	lyr.onmousemove=mod.onmousemove=null;
	lyr.onmouseup=mod.onmouseup=null;
	
	if (lyr.releaseCapture)
		lyr.releaseCapture();
		
	_theLYR=null
}

// ================================================================================

function DialogBoxWidget_keypress(e)
// PRIVATE
{		
	eventCancelBubble(e)
	var dlg=DialogBoxWidget_current
	if (dlg!=null)
	{			
		switch( eventGetKey(e))
		{			
			case 13://enter
				if(dlg.yes && !dlg.no) //old code ???
				{ 
					if (dlg.defaultCB) dlg.defaultCB();return false; 
				} 
				
				//Behavior is different from textArea and textField with the Enter action
				if(isTextArea(_ie?_curWin.event:e)) return true;
				
				if(dlg.defaultBtn!=null && !dlg.defaultBtn.isDisabled())
				{
					dlg.defaultBtn.executeCB(); return false;
				}				
				break;				
			case 27://cancel
				dlg.show(false)
				hideBlockWhileWaitWidget()
				if (dlg.cancelCB!=null) dlg.cancelCB()				
				return false;
				break;
			case 8: //back space				
				return isTextInput(_ie?_curWin.event:e);
				break;
		}
	}
}

// ================================================================================

function DialogBoxWidgetResizeModals(e)
// PRIVATE
{
	for (var i in DialogBoxWidget_modals)
	{
		m_sty=DialogBoxWidget_modals[i]
		m_sty.width=documentWidth() + 'px';
		m_sty.height=documentHeight() + 'px';
	}
}

// ================================================================================

function DialogBoxWidget_center()
{
	var o=this,scrY=getScrollY(),scrX=getScrollX()

	o.height=o.layer.offsetHeight;
	o.width=o.layer.offsetWidth;

	o.move(Math.max(0,scrX+(winWidth()-o.width)/2),Math.max(0,scrY+(winHeight()-o.height)/2));
	o.placeIframe()
}

function DialogBoxWidget_Show(sh)
// Show or Hide the dialog
// sh [boolean] if true show
{
	with (this)
	{
		m_sty=modal.css
		l_sty=css
				
		if (sh)
		{
			if (!this.iframe)
			{				
				this.iframe=newWidget(getDynamicBGIFrameLayer().id)			
				this.iframe.init()
			}
			
			oldCurrent=DialogBoxWidget_current
			DialogBoxWidget_current=this
						
			if (_ie)
			{		
				layer.onkeydown=eval('_curWin.'+_codeWinName+'.DialogBoxWidget_keypress')
				modal.layer.onkeydown=eval('_curWin.'+_codeWinName+'.DialogBoxWidget_keypress')
			}
			else
			{
				_curDoc.addEventListener("keydown", eval('_curWin.'+_codeWinName+'.DialogBoxWidget_keypress'), false)
			}
			
			oldMouseDown=_curDoc.onmousedown
			_curDoc.onmousedown=null
						
			hideBlockWhileWaitWidget()
		}
		else		
		{
			DialogBoxWidget_current=oldCurrent
			oldCurrent=null

			if (_ie)
			{
				layer.onkeydown=null
				modal.layer.onkeydown=null
			}
			else
			{
				_curDoc.removeEventListener("keydown", eval('_curWin.'+_codeWinName+'.DialogBoxWidget_keypress'), false)
			}
			
			_curDoc.onmousedown=oldMouseDown
		}
		
		var sameState=(layer.isShown==sh)

		if (sameState)
			return
		layer.isShown=sh

		if (sh)
		{
			if (_curWin.DialogBoxWidget_zindex==null)
				_curWin.DialogBoxWidget_zindex=1000

			
			this.iframe.css.zIndex=_curWin.DialogBoxWidget_zindex++;
			m_sty.zIndex=_curWin.DialogBoxWidget_zindex++;
			l_sty.zIndex=_curWin.DialogBoxWidget_zindex++;

			DialogBoxWidget_modals[DialogBoxWidget_modals.length]=m_sty

			m_sty.display=''
			l_sty.display='block'
			this.iframe.setDisplay(true)
			holdBGIFrame(this.iframe.id);
			DialogBoxWidgetResizeModals()
			
			this.height=layer.offsetHeight;
			this.width=layer.offsetWidth;
			
			//add scroll bar for small resolution 800*600
			//do the resize action before the center
			if(_small && height)
			{									
				if(divLayer == null)
					divLayer=getLayer("div_dialog_"+id);
				if(divLayer)
				{
					divLayer.style.overflow="auto";
					divLayer.style.height = (winHeight()< height)? (winHeight()-40) : getContainerHeight();
					divLayer.style.width = (_moz?width+20:getContainerWidth());
				}
				resize(null,((winHeight()< height)? (winHeight()-10):null));			
			}
			
			if (isHidden(layer))
			{
				this.center()
			}
			
			if (!_small && this.resizeCB)
				this.resizeCB(width,height)				
										
		}
		else
		{
			var l=DialogBoxWidget_modals.length=Math.max(0,DialogBoxWidget_modals.length-1)

			m_sty.width='1px'
			m_sty.height='1px'

			m_sty.display='none'
			l_sty.display='none'
			

			if (this.iframe != null) {
				this.iframe.setDisplay(false)
				releaseBGIFrame(this.iframe.id)
			}	
		}	

		modal.show(sh);
		
		firstLink.show(sh)
		lastLink.show(sh)

		oldShow(sh);		
		
		if (DialogBoxWidget_current!=null && sh==true)
			DialogBoxWidget_current.focus();
	}
}


// must be called to release bg iframe
function DialogBoxWidget_unload()
{
	if (this.iframe) {
		releaseBGIFrame(this.iframe.id)
	}	
}


// ================================================================================

function DialogBoxWidget_keepFocus(id)
// PRIVATE
{					
	var o=DialogBoxWidget_instances[id];
	if (o) o.focus();
}

// ================================================================================

function DialogBoxWidget_focus()
// apply focus on the dialog box
{	
	with (this)			
	{
		if (titleLayer == null)
			titleLayer = getLayer('titledialog_'+id);					
		
		if (titleLayer.focus)	titleLayer.focus();									
	}
}

// ================================================================================
// ================================================================================
//
// OBJECT newAlertDialog (Constructor)
//
// Class for alert dialog box
//
// ================================================================================
// ================================================================================

function newAlertDialog(id,title,text,okLabel,promptType,yesCB)
// id         [String]            the id for DHTML processing
// title      [String]            the caption dialog text
// text       [String]            the alert text
// okLabel    [String]            label for the OK button
// promptType [_promptDlgInfo | _promptDlgWarning=1 | _promptDlgCritical]
// yesCB      [Function optional] callback when clicking on OK
{
	return newPromptDialog(id,title,text,okLabel,null,promptType,yesCB,null,true)
}

// ================================================================================
// ================================================================================
//
// OBJECT newPromptDialog (Constructor)
//
// Class for prompt dialog box
//
// ================================================================================
// ================================================================================

function newPromptDialog(id,title,text,okLabel,cancelLabel,promptType,yesCB,noCB,noCloseButton)
// id          [String]            the id for DHTML processing
// title       [String]            the caption dialog text
// text        [String]            the alert text
// okLabel     [String]            label for the OK button
// cancelLabel [String]            label for the Cancel button
// promptType  [_promptDlgInfo | _promptDlgWarning=1 | _promptDlgCritical]
// yesCB       [String or Function optional] callback when clicking on Yes 
// noCB       [String or Function optional] callback when clicking on No
{
	var o=newDialogBoxWidget(id,title,300,null,PromptDialog_defaultCB,PromptDialog_cancelCB,noCloseButton)
	o.text=text
	o.getHTML=PromptDialog_getHTML
	o.yes=okLabel?newButtonWidget(id+"_yesBtn",okLabel,'PromptDialog_yesCB(\"'+o.id+'\")',70):null
	o.no=cancelLabel?newButtonWidget(id+"_noBtn",cancelLabel,'PromptDialog_noCB(\"'+o.id+'\")',70):null
	o.yesCB=yesCB
	o.noCB=noCB
	o.promptType=promptType
	o.txtLayer = null
	o.imgLayer = null	
	
	o.setPromptType=PromptDialog_setPromptType
	o.setText=PromptDialog_setText
	
	//attach default button for prompt dialog
	if(o.yes) o.attachDefaultButton(o.yes)
	else if(o.no) o.attachDefaultButton(o.no)
	
	return o
}

// ================================================================================

function PromptDialog_getimgPath(promptType)
// PRIVATE
{	
	var imgPath=_skin
	switch(promptType)
	{
		case _promptDlgInfo: imgPath+='information_icon.gif';break
		case _promptDlgWarning: imgPath+='warning_icon.gif';break
		default: imgPath+='critical_icon.gif';break
	}	
	return imgPath
}

// ================================================================================

function PromptDialog_getimgAlt(promptType)
// PRIVATE
{
	var imgAlt=''

	/*
	switch(promptType)
	{
		case _promptDlgInfo: imgAlt=_topfs._labImgInfo;break
		case _promptDlgWarning: imgAlt=_topfs._labImgWarning;break
		default: imgAlt=_topfs._labImgError;break
	}
	*/
	return imgAlt
}

// ================================================================================

function PromptDialog_setPromptType(promptType)
// Change the prompt icon
// promptType [_promptDlgInfo | _promptDlgWarning=1 | _promptDlgCritical]
{
	var o=this
	
	if (o.imgLayer == null)
		o.imgLayer = getLayer("dlg_img_"+o.id)
	
	o.imgLayer.src=PromptDialog_getimgPath(promptType)
	o.imgLayer.alt=PromptDialog_getimgAlt(promptType)		
}

// ================================================================================

function PromptDialog_setText(text)
// Change the prompt text
// text [String] the prompt text
{
	var o=this	
	o.text=text
	
	if (o.txtLayer == null)
		o.txtLayer = getLayer("dlg_txt_"+o.id)

	o.txtLayer.innerHTML=convStr(text,false,true)	
}

// ================================================================================

function PromptDialog_getHTML()
// Get the widget HTML
// Returns : [String] the HTML
{
	var o=this
	var imgPath=PromptDialog_getimgPath(o.promptType)
	var imgAlt=PromptDialog_getimgAlt(o.promptType)			
	
	return o.beginHTML()+
		'<table class="dialogzone" width="290" cellpadding="0" cellspacing="5" border="0">'+
			'<tr><td>'+
				'<table class="dialogzone" cellpadding="5" cellspacing="0" border="0">'+
					'<tr><td align="right" width="32" >'+img(imgPath,32,32,null,'id="dlg_img_'+o.id+'"',imgAlt)+'</td><td></td><td id="dlg_txt_'+o.id+'" align="left" tabindex="0">'+convStr(o.text,false,true)+'</td></tr>'+
				'</table>'+
			'</td></tr>'+
			'<tr><td>'+getSep()+'</td></tr>'+
			'<tr><td align="right">'+
				'<table cellpadding="5" cellspacing="0" border="0">'+
					'<tr>'+
						(o.yes?'<td>'+o.yes.getHTML()+'</td>':'')+
						(o.no?'<td>'+o.no.getHTML()+'</td>':'')+
					'</tr>'+
				'</table>'+
			'</td></tr>'+
		'</table>'+
	o.endHTML()
}

// ================================================================================

function PromptDialog_defaultCB()
// PRIVATE
{
	var o=this
	if (o.yesCB)
	{
		if (typeof o.yesCB!="string")
			o.yesCB()
		else
			eval(o.yesCB)
	}
/*	
	if (this.yesCB)
		this.yesCB()
*/		
	this.show(false)
	
}

// ================================================================================

function PromptDialog_cancelCB()
// PRIVATE
{
	var o=this
	if (o.noCB)
	{
		if (typeof o.noCB!="string")
			o.noCB()
		else
			eval(o.noCB)
	}
	/*
	if (this.noCB)
		this.noCB()
	*/			
	this.show(false)
}

// ================================================================================

function PromptDialog_yesCB(id)
// PRIVATE
{
	DialogBoxWidget_instances[id].defaultCB()
}

// ================================================================================

function PromptDialog_noCB(id) 
// PRIVATE
{
	DialogBoxWidget_instances[id].cancelCB()
}


