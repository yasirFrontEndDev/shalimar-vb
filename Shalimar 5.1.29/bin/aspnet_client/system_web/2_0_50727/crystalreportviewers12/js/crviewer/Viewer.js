/* Copyright (c) Business Objects 2006. All rights reserved. */

if (typeof(bobj.crv.Viewer) == 'undefined') {
    bobj.crv.Viewer = {};
}

bobj.crv.Viewer.LayoutTypes = {
    FIXED: 'fixed',
    CLIENT: 'client',
    FITREPORT: 'fitreport'
};

/**
 * Viewer Constructor
 *
 * kwArgs.layoutType [String]  Tells the viewer how to size itself. Can be 
 *                             "client" (fill window), "fitReport", or "fixed"
 * kwArgs.width      [Int]          Width in pixels when layoutType=fixed
 * kwArgs.height     [Int]          Height in pixels when layoutType=fixed
 */
bobj.crv.newViewer = function(kwArgs) {
    kwArgs = MochiKit.Base.update({
        id: bobj.uniqueId(),
        layoutType  : bobj.crv.Viewer.LayoutTypes.FIXED, 
        visualStyle : {
            className       : null,
            backgroundColor : null,
            borderWidth     : null,
            borderStyle     : null,
            borderColor     : null,
            fontFamily      : null,
            fontWeight      : null,
            textDecoration  : null,
            color           : null,
            width           : '800px',
            height          : '600px',
            fontStyle       : null,
            fontSize        : null
       }        
    }, kwArgs);
    var o = newWidget(kwArgs.id);

    bobj.fillIn(o, kwArgs);  
    o.widgetType = 'Viewer';
    
    o._topToolbar = null;
    o._botToolbar = null;
    o._reportAlbum = null;
    o._separator = null;
    o._print = null;
    o._export = null;
    o._promptDlg = null;
    o._reportProcessing = null;
    
    // Attach member functions 
    o.initOld = o.init;
    MochiKit.Base.update(o, bobj.crv.Viewer);
    
    return o;    
};

bobj.crv.Viewer.addChild = function(widget) { 
    if (widget.widgetType == 'ReportAlbum') {
        this._reportAlbum = widget; 
    }
    else if (widget.widgetType == 'Toolbar') {
        if (widget.layoutAlign == 'bottom') {
            this._botToolbar = widget;    
        }
        else {
            this._topToolbar = widget;
            this._separator = bobj.crv.newSeparator();
        }
    }
    else if (widget.widgetType == 'PrintUI') {
    	this._print = widget;
    }
    else if (widget.widgetType == 'ExportUI') {
    	this._export = widget;
    }
    else if (widget.widgetType == 'ReportProcessingUI') {
    	this._reportProcessing = widget;
    }
};

/**
 * This function will remove flash activation boxes displayed by IE 
 * by replacing each flash object by its clone.
 */
bobj.crv.Viewer.removeFlashActivations = function() {
    if(_ie && this.layer && this.layer.getElementsByTagName) {
        var flashObjects = this.layer.getElementsByTagName("object");
        for(var i = 0 ; i < flashObjects.length; i++) {
             var flashObject = flashObjects[i];
             var flashObjectClone = flashObject.cloneNode(true);
             var flashObjectParent = flashObject.parentNode;
             var flashObjectSibling = flashObject.nextSibling;
             
             if(flashObjectParent && flashObjectClone) {
                flashObjectParent.removeChild(flashObject);
                flashObjectParent.insertBefore(flashObjectClone, flashObjectSibling);
                flashObjectClone.outerHTML = flashObjectClone.outerHTML;
             }
        }
    }    
};

bobj.crv.Viewer.getHTML = function() {
    var h = bobj.html;
    
    var layerStyle = {
        overflow: 'hidden',
        position: 'relative'
    };
    
    var html = h.DIV({dir: 'ltr', id:this.id, style:layerStyle,  'class':'dialogzone'},
        this._topToolbar ? this._topToolbar.getHTML() : '',
        this._separator ? this._separator.getHTML() : '',
        this._reportAlbum ? this._reportAlbum.getHTML() : '',
        this._botToolbar ? this._botToolbar.getHTML() : '');

    return html + bobj.crv.getInitHTML(this.widx);
};

bobj.crv.Viewer.init = function() {
    this.initOld();
    bobj.setVisualStyle(this.layer, this.visualStyle);
      
    var connect = MochiKit.Signal.connect;
    var signal = MochiKit.Signal.signal;
    var partial = MochiKit.Base.partial;
    
    if (this.layoutType.toLowerCase() == bobj.crv.Viewer.LayoutTypes.CLIENT) {
        connect(window, 'onresize', this, '_doLayout');    
    }
    
    this._doLayout();
    
    if (this._topToolbar) { // TODO do for each toolbar
        connect(this._topToolbar, 'showGroupTree', this, '_onShowGroupTreeClick');
        connect(this._topToolbar, 'showParamPanel', this, '_onShowParamPanelClick');
        connect(this._topToolbar, 'zoom', partial(signal, this, 'zoom'));
        connect(this._topToolbar, 'drillUp', partial(signal, this, 'drillUp'));
        
        // Page Navigation events
        connect(this._topToolbar, 'firstPage', partial(signal, this, 'firstPage'));
        connect(this._topToolbar, 'prevPage', partial(signal, this, 'prevPage'));
        connect(this._topToolbar, 'nextPage', partial(signal, this, 'nextPage'));
        connect(this._topToolbar, 'lastPage', partial(signal, this, 'lastPage'));
        connect(this._topToolbar, 'selectPage', partial(signal, this, 'selectPage'));
        connect(this._topToolbar, 'refresh', partial(signal, this, 'refresh'));
        connect(this._topToolbar, 'search', partial(signal, this, 'search'));
        connect(this._topToolbar, 'export', partial(signal, this, 'export'));
        connect(this._topToolbar, 'print', partial(signal, this, 'print'));
    }

    if (this._reportAlbum) {
        connect(this._reportAlbum, 'resizeToolPanel', partial(signal, this, 'resizeToolPanel'));
        connect(this._reportAlbum, 'selectView', this, '_onSelectView'); 
        connect(this._reportAlbum, 'removeView', this, '_onRemoveView');
        connect(this._reportAlbum, 'grpDrilldown', partial(signal, this, 'grpDrilldown'));
        connect(this._reportAlbum, 'grpNodeRetrieveChildren', partial(signal, this, 'grpNodeRetrieveChildren'));
        connect(this._reportAlbum, 'grpNodeCollapse', partial(signal, this, 'grpNodeCollapse'));
        connect(this._reportAlbum, 'grpNodeExpand', partial(signal, this, 'grpNodeExpand'));
        
        if (this._topToolbar) {    
            var panelType = this._reportAlbum._curView.toolPanel.initialViewType;
            this._topToolbar.updateToolPanelButtons(panelType);
        }
    }
    
    if (this._print) {
    	connect(this._print, 'printSubmitted', partial(signal, this, 'printSubmitted'));
    }
    
    if (this._export) {
    	connect(this._export, 'exportSubmitted', partial(signal, this, 'exportSubmitted'));
    }
    
    this.scrollToHighlighted();
    this._updateRefreshButton();
    
    connect(window, 'onload', this, 'removeFlashActivations');
    signal(this, 'initialized');
};


bobj.crv.Viewer._updateRefreshButton = function() {
    if(this._reportAlbum) {
        var currentView = this._reportAlbum.getSelectedView();
        if(currentView) {
            if(!currentView.isMainReport()) {
                this.disableRefreshButton(true);
            }
        }  
    }
};

bobj.crv.Viewer._doLayout = function() {
    var topToolbarH = this._topToolbar ? this._topToolbar.getHeight() : 0;
    var topToolbarW = this._topToolbar ? this._topToolbar.getWidth() : 0;
    var botToolbarH = this._botToolbar ? this._botToolbar.getHeight() : 0;
    var separatorH = this._separator ? this._separator.getHeight() : 0; 
    
    var layout = this.layoutType.toLowerCase();
    
    if (bobj.crv.Viewer.LayoutTypes.CLIENT == layout) {
        this.css.width = '100%';
        this.css.height = '100%';
    }
    else if (bobj.crv.Viewer.LayoutTypes.FITREPORT == layout) {
        var albumSize = this._reportAlbum.getBestFitSize();
        var viewerWidth = (albumSize.width < topToolbarW) ? topToolbarW : albumSize.width;
        var viewerHeight = (albumSize.height + topToolbarH + botToolbarH + separatorH); 
        
        this.css.height = viewerHeight + 'px';
        this.css.width  = viewerWidth + 'px';  
    }
    else { // fixed layout 
        this.css.width = this.visualStyle.width;
        this.css.height = this.visualStyle.height;
    }
    
    var innerW = this.getWidth();
    var innerH = this.getHeight();
    
    var albumH = Math.max(0, innerH - topToolbarH - botToolbarH - separatorH);
    
    if (this._reportAlbum) {
        this._reportAlbum.resizeOuter(innerW, albumH);
    }
    
    if (this._botToolbar) {
        this._botToolbar.move(0, topToolbarH + separatorH + albumH);
    }

    if (this._print && this._print.layer) {
    	this._print.center();
    }
    
    if (this._export && this._export.layer) {
    	this._export.center();
    }

    if (this._reportProcessing && this._reportProcessing.layer) {
    	this._reportProcessing.center();
    }
};


bobj.crv.Viewer._onShowGroupTreeClick = function(isChecked) { 
    // translate checked==false to panelType==none
    var panelType = isChecked ? bobj.crv.ToolPanelType.GroupTree : bobj.crv.ToolPanelType.None;
    this.selectToolPanel(panelType);
};

bobj.crv.Viewer._onShowParamPanelClick = function(isChecked) { 
    // translate checked==false to panelType==none
    var panelType = isChecked ? bobj.crv.ToolPanelType.ParameterPanel : bobj.crv.ToolPanelType.None;
    this.selectToolPanel(panelType);
};

bobj.crv.Viewer.selectToolPanel = function(panelType) {
    var Type = bobj.crv.ToolPanelType;
    if (this._reportAlbum) {    
        this._reportAlbum._curView.toolPanel.setView (panelType);
        this._reportAlbum._curView.setDisplayToolPanel(Type.None !== panelType);
    }
    if (Type.GroupTree == panelType) {
        MochiKit.Signal.signal(this, 'showGroupTree');
    }
    else if (Type.ParameterPanel == panelType) {
        MochiKit.Signal.signal(this, 'showParamPanel');
    }
    else if (Type.None == panelType) {
        MochiKit.Signal.signal(this, 'hideToolPanel');
    }
};

bobj.crv.Viewer._onSelectView = function(view) {
    MochiKit.Signal.signal(this, 'selectView', view);
};

bobj.crv.Viewer._onRemoveView = function(view) {
    MochiKit.Signal.signal(this, 'removeView', view);
};

bobj.crv.Viewer.resize = function(w, h) {
    if (bobj.isNumber(w)) {
        w = w + 'px';   
    }

    if (bobj.isNumber(h)) {
        h = h + 'px'; 
    }
    
    this.visualStyle.width = w; 
    this.visualStyle.height = h; 
    this._doLayout();
};

bobj.crv.Viewer.disableRefreshButton = function(isDisabled) {
    if (this._topToolbar) {
        var refreshButton = this._topToolbar.refreshButton;
        if(refreshButton) {
            refreshButton.setDisabled(isDisabled);
        }
    }
    if (this._botToolbar) {
        var refreshButton = this._botToolbar.refreshButton;
        if(refreshButton) {
            refreshButton.setDisabled(isDisabled);
        }
    }
};

/** 
 * Set the page number. Updates toolbars with current page and number of pages
 * info.
 *
 * @param curPageNum [String]  
 * @param numPages   [String] (eg. "1" or "1+");
 */
bobj.crv.Viewer.setPageNumber = function(curPageNum, numPages) {
    if (this._topToolbar) {
        this._topToolbar.setPageNumber(curPageNum, numPages);
    }
    if (this._botToolbar) {
        this._botToolbar.setPageNumber(curPageNum, numPages);
    }
};

/**
 * Display the prompt dialog.
 *
 * @param html [string] HTML fragment to display inside the dialog's form.
 */
bobj.crv.Viewer.showPromptDialog = function(html) {
    html = html || '';
    
    if (!this._promptDlg) {
        this._promptDlg = bobj.crv.params.newParameterDialog({id: this.id + '_promptDlg'});    
    }
    
    this._promptDlg.setPromptHTML(html);
    setTimeout(MochiKit.Base.bind(this._promptDlg.show,this._promptDlg,true),1);
    
};

/**
 * Hide the prompt dialog
 */ 
bobj.crv.Viewer.hidePromptDialog = function() {
    if (this._promptDlg) {
        this._promptDlg.show(false);        
    }
}

/**
 * Display an error message dialog.
 *
 * @param text [String]    Short, user-friendly error message
 * @param details [String] Technical info that's hidden unless the user chooses to see it  
 */
bobj.crv.Viewer.showError = function(text, details) {
    var dlg = bobj.crv.ErrorDialog.getInstance();
    dlg.setText(text, details);
    dlg.setTitle(L_bobj_crv_Error);
    dlg.show(true);
};


/**
 * Update the UI using the given properties
 *
 * @param update [Object] Component properties 
 */
bobj.crv.Viewer.update = function(update) {
    if (!update) {
        return;    
    }
    
    if (this._topToolbar && update.topToolbar) { 
        this._topToolbar.update(update.topToolbar);
    }

    if (this._reportAlbum && update.reportAlbum) {
        this._reportAlbum.update(update.reportAlbum);
    }     
};

bobj.crv.Viewer.getParameterPanel = function() {
    if (this._reportAlbum) {
        var view = this._reportAlbum.getSelectedView();
        if (view && view.toolPanel) { 
            return view.toolPanel.getParameterPanel();
        }
    }
};

bobj.crv.Viewer.scrollToHighlighted = function() {
    var currentView = this._reportAlbum.getSelectedView();
    var reportPage = getLayer(currentView.reportPage.id);
    var crystalHighlighted = getLayer("CrystalHighLighted");
    
    if(!crystalHighlighted || !reportPage) { 
        return;
    }   
 
    if(MochiKit.DOM.isParent(crystalHighlighted,reportPage)) {
        var layoutType = this.layoutType.toLowerCase();
        var position;
        if(layoutType == bobj.crv.Viewer.LayoutTypes.FITREPORT) {
            position = MochiKit.Style.getElementPosition(crystalHighlighted);
            window.scrollTo(position.x,position.y);
        }
        else {
            position = MochiKit.Style.getElementPosition(crystalHighlighted,reportPage);
            reportPage.scrollLeft = position.x;
            reportPage.scrollTop = position.y;
        }  
    }
};