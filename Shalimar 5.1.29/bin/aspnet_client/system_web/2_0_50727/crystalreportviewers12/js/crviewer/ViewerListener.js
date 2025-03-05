/* Copyright (c) Business Objects 2006. All rights reserved. */

/**
 * ViewerListener Constructor. Handles Viewer UI events.
 */
bobj.crv.ViewerListener = function(viewerName, ioHandler) {
    this._name = viewerName;
    this._viewer = null;
    this._promptPage = null; 
    this._paramCtrl = null;
    this._ioHandler = ioHandler;
    this._reportProcessing = null;
    
    var connect = MochiKit.Signal.connect;
    var subscribe = bobj.event.subscribe;
    var bind = MochiKit.Base.bind;
    
    var widget = getWidgetFromID(viewerName);
    if (widget) {
        if (widget.widgetType == 'Viewer') {
            this._viewer = widget;
            this._reportProcessing = this._viewer._reportProcessing;
        }
        else if (widget.widgetType == 'PromptPage') {
            this._promptPage = widget;    
            this._reportProcessing = this._promptPage._reportProcessing;
        }
    }
    
    if (this._viewer) {
        // ReportAlbum events
        connect(this._viewer, 'selectView', this, '_onSelectView');
        connect(this._viewer, 'removeView', this, '_onRemoveView');
        
        // Toolbar events
        connect(this._viewer, 'firstPage', this, '_onFirstPage');
        connect(this._viewer, 'prevPage', this, '_onPrevPage');
        connect(this._viewer, 'nextPage', this, '_onNextPage');
        connect(this._viewer, 'lastPage', this, '_onLastPage');
        connect(this._viewer, 'selectPage', this, '_onSelectPage');
        connect(this._viewer, 'zoom', this, '_onZoom');
        connect(this._viewer, 'drillUp', this, '_onDrillUp');
        connect(this._viewer, 'refresh', this, '_onRefresh');  
        connect(this._viewer, 'search', this, '_onSearch');
        connect(this._viewer, 'export', this, '_onExport');
        connect(this._viewer, 'print', this, '_onPrint');
        
        // Tool Panel events
        connect(this._viewer, 'resizeToolPanel', this, '_onResizeToolPanel');
        connect(this._viewer, 'hideToolPanel', this, '_onHideToolPanel');
        connect(this._viewer, 'grpDrilldown', this, '_onDrilldownGroupTree');
        connect(this._viewer, 'grpNodeRetrieveChildren', this, '_onRetrieveGroupTreeNodeChildren');
        connect(this._viewer, 'grpNodeCollapse', this, '_onCollapseGroupTreeNode');
        connect(this._viewer, 'grpNodeExpand', this, '_onExpandGroupTreeNode');
        connect(this._viewer, 'showParamPanel', this, '_onShowParamPanel');
        connect(this._viewer, 'showGroupTree', this, '_onShowGroupTree');
        
        connect(this._viewer, 'printSubmitted', this, '_onSubmitExport');
        connect(this._viewer, 'exportSubmitted', this, '_onSubmitExport');
        
        this._setInteractiveParams();
    }
    
    // Report Page Events
    subscribe('drilldown', this._forwardTo('_onDrilldown')); 
    subscribe('drilldownGraph', this._forwardTo('_onDrilldownGraph'));
    subscribe('drilldownSubreport', this._forwardTo('_onDrilldownSubreport'));
    subscribe('sort', this._forwardTo('_onSort'));
    
    // Prompt events
    subscribe('crprompt_param', this._forwardTo('_onSubmitStaticPrompts'));
    subscribe('crprompt_pmtEngine', this._forwardTo('_onSubmitPromptEnginePrompts'));
    subscribe('crprompt_logon', this._forwardTo('_onSubmitDBLogon'));
    subscribe('crprompt_cancel',this._forwardTo('_onCancelParamDlg'));
    
    // Report Part Events
    subscribe('pnav', this._forwardTo('_onNavigateReportPart'));
    subscribe ('navbookmark', this._forwardTo('_onNavigateBookmark'));
    
    // Universal Events, No Target Checks
    subscribe('saveViewState', bind(this._onSaveViewState, this)); 
};

bobj.crv.ViewerListener.prototype = {

    /**
     * Public
     *
     * @return The current report view 
     */
     getCurrentView: function() {
         if(this._viewer && this._viewer._reportAlbum) {
             return this._viewer._reportAlbum.getSelectedView();
         }
         
         return null;
     },
     
    /**
     * Private. Wraps an event handler function with an event target check.
     *
     * @param handlerName [String]  Name of the event handler method
     *
     * @return Function that can be used as a callback for subscriptions
     */
    _forwardTo: function(handlerName) {
        return MochiKit.Base.bind(function(target) {
            if (target == this._name) {
                var args = bobj.slice(arguments, 1);
                this[handlerName].apply(this, args);
            }
        }, this);    
    },

    _onSaveViewState: function() {
        this._saveViewState();
    },

    _onSelectView: function(view) {
        if (view) {
            if (view.hasContent()) {
                this._updateUIState();
            }
            else {
                // Since "restore state" happens before events, we need to 
                // change the curViewId before making the server request
                var state = bobj.crv.stateManager.getComponentState(this._name);
                if (state) {
                    state.curViewId = view.viewStateId;
                    this._request({selectView: view.viewStateId}, false);
                }
            }
        }
    },
    
    _onRemoveView: function(view) {
        if (view) {
            var viewerState = bobj.crv.stateManager.getComponentState(this._name);
            if (viewerState) {
                // Remove view from viewer state
                delete viewerState[view.viewStateId];
            }
            
            var commonState = this._getCommonState();
            if (commonState) {
                // Remove view from taborder
                var idx = MochiKit.Base.findValue(commonState.rptAlbumOrder, view.viewStateId);
                if (idx != -1) {
                    arrayRemove(commonState, 'rptAlbumOrder', idx);   
                }
            }
        }
    },
    
    _onFirstPage: function() {
        this._request({tb:'first'}, false);
        
    },
    
    _onPrevPage: function() {
        this._request({tb:'prev'}, false);
    },
    
    _onNextPage: function() {
        this._request({tb:'next'}, false);
    },
    
    _onLastPage: function() {
        this._request({tb:'last'}, false);
    },

    _onDrillUp: function() {
        this._request({tb:'drillUp'}, false);
    },
    
    _onSelectPage: function(pgTxt) {
        this._request({tb:'gototext', text:pgTxt}, false);    
    },
    
    _onZoom: function(zoomTxt) {
        this._request({tb:'zoom', value:zoomTxt}, false);    
    },
    
    _onExport: function() {
        if (this._viewer._export) {
            this._viewer._export.show(true);
        }
    },
    
    _onPrint: function() {
        var printComponent = this._viewer._print;
        if (printComponent) {
            if (printComponent.isActxPrinting) {
                var pageState = bobj.crv.stateManager.getCompositeState();
                var postBackData = this._ioHandler.getPostDataForPrinting(pageState, this._name);
                this._viewer._print.show(true, postBackData);
            }
            else {
                this._viewer._print.show(true);
            }
        }
    },

    _onResizeToolPanel: function(width) {
        this._setCommonProperty('toolPanelWidth', width);
        this._setCommonProperty('toolPanelWidthUnit', 'px');
    },
    
    _onHideToolPanel: function() {
        this._setCommonProperty('toolPanelType', bobj.crv.ToolPanelType.None);
    },
    
    _onShowParamPanel: function() {
        this._setCommonProperty('toolPanelType', bobj.crv.ToolPanelType.ParameterPanel);
    },
    
    _onShowGroupTree: function() {
        this._setCommonProperty('toolPanelType', bobj.crv.ToolPanelType.GroupTree);
    },
    
    _onDrilldown: function(drillargs) {
        this._request(drillargs, false);
    },
    
    _onDrilldownSubreport: function(drillargs) {
        this._request(drillargs, false);
    },
    
    _onDrilldownGraph: function(event, graphName, branch, offsetX, offsetY, pageNumber, nextpart, twipsPerPixel) {
        if (event) {
            var mouseX, mouseY;
            if(bobj.isNumber(event.layerX)) {
                mouseX = event.layerX;
                mouseY = event.layerY;
            } else {
                mouseX = event.offsetX;
                mouseY = event.offsetY;
            }
            
            var zoomFactor = parseInt(this._getCommonProperty('zoom'), 10);
            zoomFactor = (isNaN(zoomFactor) ? 1 : zoomFactor/100);
            
            this._request({ name:encodeURIComponent(graphName),
                            brch:branch,
                            coord:(mouseX*twipsPerPixel/zoomFactor + parseInt(offsetX, 10)) + '-' + (mouseY*twipsPerPixel/zoomFactor +parseInt(offsetY, 10)),
                            pagenumber:pageNumber,
                            nextpart:encodeURIComponent(nextpart)}, 
                            false);
        }
    },
    
    _onDrilldownGroupTree: function(groupName, groupPath, isVisible) {
        var encodedGroupName = encodeURIComponent(groupName);
        if (!isVisible) {
            this._request({brch:groupPath, drillname:encodedGroupName}, false);
        }
        else {
            this._request({grp:groupPath, drillname:encodedGroupName}, false);
        }
    },
    
    _onRetrieveGroupTreeNodeChildren: function(groupPath) {
        this._request({grow:groupPath}, false);
    },
    
    _onCollapseGroupTreeNode: function(groupPath) {
        var pos = groupPath.lastIndexOf('-');
        if (pos == -1) {
            groupPath = '';
        }
        else {
            groupPath = groupPath.substring(0, pos);
        }
        
        this._setViewProperty('dispGroupPath', groupPath);
    },
    
    _onExpandGroupTreeNode: function(groupPath) {
        this._setViewProperty('dispGroupPath', groupPath);
    },
    
    _onRefresh: function() {
        this._request({tb:'refresh'}, false);
    },
    
    _onSearch: function(searchText) {
        this._request({tb:'search', text:encodeURIComponent(searchText)}, false);
    },
    
    _onSubmitPromptEnginePrompts: function(formName) {
        var isPromptDialogVisible = (this._viewer && this._viewer._promptDlg && this._viewer._promptDlg.isVisible());
        var useAsync = isPromptDialogVisible;

        this._addRequestField('ValueID', document.getElementById('ValueID').value);
        
        if (document.getElementById('ContextID')) {
            this._addRequestField('ContextID', document.getElementById('ContextID').value);
	    }
	    
	    if (document.getElementById('ContextHandleID')) {
            this._addRequestField('ContextHandleID', document.getElementById('ContextHandleID').value);
        }
            
        this._request({'crprompt':'pmtEngine'}, useAsync);
    },
    
    _onSubmitStaticPrompts: function(formName) {
        this._addRequestFields(formName);
        this._request({'crprompt':'param'}, false);
    },
    
    _onSubmitDBLogon: function(formName) {
        this._addRequestFieldsFromContent(this._promptPage.contentId);
        this._request({'crprompt':'logon'}, false);
    },

    _onSubmitExport: function (start, end, format) {
        var isRange = true;
        if (!start && !end) {
            isRange = false;
        }
        
        if (!format) {
            format = 'PDF';
        }
        
        var reqObj = {tb:'crexport', text:format, range:isRange+''};
        if (isRange) {
            reqObj['from'] = start + '';
            reqObj['to'] = end + '';
        }
    	
        // we want to redirect export requests to a Servlet (ASP should do nothing different)
        if (this._ioHandler instanceof bobj.crv.ServletAdapter || this._ioHandler instanceof bobj.crv.FacesAdapter) {
            this._ioHandler.redirectToServlet ();
            this._ioHandler.addRequestField ('ServletTask', 'Export');
        }

        var callback = null;
        this._request(reqObj, false, callback, false);
    },
    
    _onCancelParamDlg: function() {
        this._viewer.hidePromptDialog();
    },
    
    _onReceiveParamDlg: function(html) {
        this._viewer.showPromptDialog(html);
    },
    
    _onSort: function(sortArgs) {
        this._request(sortArgs, false);
    },
    
    _onNavigateReportPart: function(navArgs) {
        this._request(navArgs, false);
    },
    
    _onNavigateBookmark: function(navArgs) {
        this._request(navArgs, false);
    },
    
    applyParams: function(params) {
        // TODO Dave can we just set the parsm into state since they've 
        // gone through client side validation?
        if (params) {
            this._request({crprompt: 'paramPanel', paramList: params});
        }
    },
    
    showAdvancedParamDialog: function(param) {
        if (param) {
            var clonedParam = MochiKit.Base.clone(param);
            clonedParam.defaultValues = null; //ADAPT00776482
            this._request({promptDlg: clonedParam}, true);    
        }
    },
    
    /**
     * Set a property in the state associated with the current report view
     *
     * @param propName [String]  The name of the property to set
     * @param propValue [Any]    The value of the property to set
     */
    _setViewProperty: function(propName, propValue) {
        var viewState = this._getViewState();
        if (viewState) {
            viewState[propName] = propValue;    
        }    
    },
    
    /**
     * Get a property in the state associated with the current report view
     *
     * @param propName [String]  The name of the property to retrieve
     */
    _getViewProperty: function(propName) {
        var viewState = this._getViewState();
        if (viewState) {
            return viewState[propName];
        }
        return null;
    },
    
    /**
     * Set a property that's shared by all report views from the state 
     *
     * @param propName [String]  The name of the property to set
     * @param propValue [String]  The value to set
     */
    _setCommonProperty: function(propName, propValue) {
        var state = this._getCommonState();
        if (state) {
            state[propName] = propValue;
        }
    },
    
    /**
     * Get a property that's shared by all report views from the state 
     *
     * @param propName [String]  The name of the property to retrieve
     */
    _getCommonProperty: function(propName) {
        var state = this._getCommonState();
        if (state) {
            return state[propName];
        }
        return null;
    },
    
    /**
     * Set the UI properties to match the state associated with viewId
     *
     * @param viewId [String - optional]  
     */ 
    _updateUIState: function(viewId) {
        
    },
    
    /**
     * Get the state associated with the current report view
     *
     * @return State object or null 
     */
    _getViewState: function() {
        var compState = bobj.crv.stateManager.getComponentState(this._name);
        if (compState && compState.curViewId) {
            return compState[compState.curViewId];
        }
        return null;
    },
    
    /**
     * Get the state that's common to all report views
     *
     * @return State object or null
     */ 
    _getCommonState: function() {
        var compState = bobj.crv.stateManager.getComponentState(this._name);
        if (compState) {
            return compState.common;
        }
        return null;
    },
    
    /**
     * Create CRPrompt instances from interactive parameters in state and pass 
     * them to the Viewer widget so it can display them in the parameter panel.
     */
    _setInteractiveParams: function(paramList) {
        if (!this._ioHandler.canUseAjax()) {
            var paramPanel = this._viewer.getParameterPanel();
            if (paramPanel) {
                paramPanel.showError(L_bobj_crv_InteractiveParam_NoAjax);
            }
            return;
        }
        
        if (!paramList) {
            var stateParamList = this._getCommonProperty('iactParams');
        
            if (stateParamList) {
                var Parameter = bobj.crv.params.Parameter;
                var paramList = [];
            
                for (var i = 0; i < stateParamList.length; ++i) {
                    paramList.push(new Parameter(stateParamList[i]));     
                }
            }
        }
        
        if (paramList && paramList.length) {
            var paramPanel = this._viewer.getParameterPanel();
            if (paramPanel) {
                var paramOpts = this._getCommonProperty('paramOpts');
                this._paramCtrl = new bobj.crv.params.ParameterController(paramPanel, this, paramOpts);
                this._paramCtrl.setParameters(paramList);
            }
        }
    },
    
    _updateInteractiveParams: function(update) {
        if (update.resolvedFields) {
            this._viewer.hidePromptDialog(); 
            
            // there may be more than one resolved field (DCP)
            this._paramCtrl.updateIsDirtyUnSelParams(false);
            for (var i = 0; i < update.resolvedFields.length; i++) {
                this._paramCtrl.updateParameter(new bobj.crv.params.Parameter(update.resolvedFields[i]));
            }
            this._paramCtrl._updateToolbar();
        }
        else {
            this._viewer.showPromptDialog(update.html); 
        }    
    },
    
    _request: function(evArgs, allowAsynch, callback, showIndicator) {
        var pageState = bobj.crv.stateManager.getCompositeState();
        var bind = MochiKit.Base.bind;
        var defaultCallback = bind(this._onResponse, this);
        var defaultErrCallback = bind(this._onIOError, this);
        if (!bobj.isBoolean (showIndicator)) {
        	showIndicator = true;
       	}

        if (this._reportProcessing && showIndicator) {
            this._reportProcessing.delayedShow (allowAsynch);
        }
        
        var deferred = this._ioHandler.request(pageState, this._name, evArgs, allowAsynch, defaultCallback, defaultErrCallback);

        if (deferred) {            
            if (this._reportProcessing && showIndicator) {
                this._reportProcessing.setDeferred (deferred);
            }
        
            deferred.addCallback(defaultCallback);
            if (callback) {
                deferred.addCallback(callback);
            }
            deferred.addErrback(defaultErrCallback);
        }
    },
    
    _onResponse: function(response) {
        var json = null;
        if (bobj.isString(response)) {
            json = MochiKit.Base.evalJSON(response);
        } else {
            json = MochiKit.Async.evalJSONRequest(response);
        }

        if (this._reportProcessing) {
            this._reportProcessing.cancelShow ();
        }

        if (json) {
            if (json.status && this._viewer && (json.status.errorMessage || json.status.debug) ) { 
                var errorMessage = json.status.errorMessage || L_bobj_crv_RequestError;
                this._viewer.showError(errorMessage, json.status.debug);
            }
            
            if (json.state) {
                var jsonState = json.state;
                if (bobj.isString(jsonState)) {
                    jsonState = MochiKit.Base.evalJSON(jsonState);
                }
                bobj.crv.stateManager.setComponentState(this._name, jsonState);
            }
            
            if (json.update && json.update.promptDlg) {
                this._updateInteractiveParams(json.update.promptDlg);    
            }
            if (json.update && this._viewer) {
                this._viewer.update(json.update);
            }
        }
    },
    
    _onIOError: function(response) { 
    
    	if (this._reportProcessing.wasCancelled () == true) {
            return;
        }

        if (this._viewer) {
            var detailText = '';
	    if (bobj.isString(response)) {
	      detailText = response;
	    }
	    else {
              for (var i in response) {
                if (bobj.isString(response[i]) || bobj.isNumber(response[i])) {
                    detailText += i + ': ' + response[i] + '\n';     
                }
              }
        }
            
            this._viewer.showError(L_bobj_crv_RequestError, detailText);    
        }

        if (this._reportProcessing) {
            this._reportProcessing.cancelShow ();
        }
    },
    
    _saveViewState: function() {
        var pageState = bobj.crv.stateManager.getCompositeState();
        this._ioHandler.saveViewState(pageState, this._name);
    },
    
    /**
     * Private. Retrieve all children of the given form and add them to the request.
     *
     * @param formName [String]  Name of the form
     */
    _addRequestFields: function(formName) {
        var frm = document.getElementById(formName);
        if (frm) {
            for (var i in frm) {
                var frmElem = frm[i];
                if (frmElem && frmElem.name && frmElem.value) {
                    this._addRequestField(frmElem.name, frmElem.value);
                }
            }
        }
    },

    /**
     * Private. Retrieve all input fields inside the content div element and add them to the request.
     *
     * @param contentId [String]  Id of the containing div element
     */
    _addRequestFieldsFromContent: function(contentId) {
        var parent = document.getElementById(contentId);
        if (!parent)
            return;

    	var elements = MochiKit.DOM.getElementsByTagAndClassName("input", null, parent);
            
        for (var i in elements) {
            var inputElement = elements[i];
            if (inputElement && inputElement.name && inputElement.value) {
                this._addRequestField(inputElement.name, inputElement.value);
            }
        }
    },
    
    /**
     * Private. Add the given name and value as a request variable.
     *
     * @param fldName [String]  Name of the field
     * @param fldValue [String] Value of the field
     */
    _addRequestField: function(fldName, fldValue) {
        this._ioHandler.addRequestField(fldName, fldValue);
    }
};


