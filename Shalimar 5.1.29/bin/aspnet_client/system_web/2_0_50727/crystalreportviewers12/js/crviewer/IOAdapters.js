/* Copyright (c) Business Objects 2006. All rights reserved. */




/**
 * Abstract base class. IOAdapters allow the ViewerListener to request data from 
 * a server without knowing the details of how a particular framework requires
 * the request to be made.
 */
bobj.crv.IOAdapterBase = {
    /**
     * Send a viewer request to the Server.
     * 
     * @param pageState   [Object] The composite view state for all viewers on the page
     * @param viewerName  [String] The name of the viewer that should handle the request
     * @param eventArgs   [Object] Event arguements
     * @param allowAsynch [bool]   True if asynchronous requests are allowed
     * 
     * @return [MochiKit.Async.Deferred] Returns a Deferred if an asynchronous
     *       request is pending.
     */
    request: function() {},
    
    /**
     * Add a parameter to server requests.
     * 
     * @param fldName  [String]  Name of the parameter
     * @param fldValue [String]  Value of the parameter
     */
    addRequestField: function() {},
    
    /**
     * Save the page state. Persist the state in a manner apropriate
     * for the framework.
     *
     * @param pageState   [Object] The composite view state for all viewers on the page
     * @param viewerName  [String] The name of the viewer that making the request
     */
    saveViewState: function(pageState, viewerName) {},
    
    /**
     * Get the postback data queryString to use for Active X print control
     *
     * @param pageState   [Object] The composite view state for all viewers on the page
     * @param viewerName  [String] The name of the viewer that making the request
     * @return [String] the postback data in a query string format
     */
    getPostDataForPrinting: function(pageState, viewerName) {},
    
    canUseAjax: function() {
        try {
            return (MochiKit.Async.getXMLHttpRequest() !== null);
        }
        catch (e) {
            return false;
        }
    }
};

/*
================================================================================
ServletAdapter. ServletAdapter issues requests to the Java DHTML viewer.
================================================================================
*/

/**
 * Constructor for ServletAdapter. 
 *
 * @param pageUrl [string]  URL of the page 
 * @param servletUrl [string]  URL to which requests to a servlet should be made
 *                             It doubles as the url for all asyncronous requests
 */
bobj.crv.ServletAdapter = function(pageUrl, servletUrl) {
    this._pageUrl = pageUrl;
    this._servletUrl = servletUrl;
    this._form = null;
};

bobj.crv.ServletAdapter._requestParams = {
    STATE: 'CRVCompositeViewState',
    TARGET: 'CRVEventTarget',
    ARGUMENT: 'CRVEventArgument'
};
    

bobj.crv.ServletAdapter.prototype = MochiKit.Base.merge(bobj.crv.IOAdapterBase, {
    
    request: function(pageState, viewerName, eventArgs, allowAsync) {
        if (!this._form) {
            this._createForm();
        }
        
        var rp = bobj.crv.ServletAdapter._requestParams;
        var toJSON = MochiKit.Base.serializeJSON;
        
        this._form[rp.STATE].value = encodeURIComponent(toJSON(pageState));  
        this._form[rp.TARGET].value = encodeURIComponent(viewerName);
        this._form[rp.ARGUMENT].value = encodeURIComponent(toJSON(eventArgs));
        
        var deferred = null;
        if (allowAsync && this._servletUrl) {
            var req = MochiKit.Async.getXMLHttpRequest();
            req.open("POST", this._servletUrl, true);
            req.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            req.setRequestHeader('Accept','application/json');
            deferred = MochiKit.Async.sendXMLHttpRequest(req, MochiKit.Base.queryString(this._form));
        }
        else {
            this._form.submit();
        }

        // once the form is submitted it is not intended to be reused.
        this._form = null;
        return deferred;
    },

    redirectToServlet: function () {
        if (!this._form) {
            this._createForm();
        }

        this._form.action = this._servletUrl;
    },

    _createForm: function() {
        var d = MochiKit.DOM;
        var rp = bobj.crv.ServletAdapter._requestParams;
        
        this._form = d.FORM({
                name: bobj.uniqueId(), 
                style: 'display:none',
                method: 'POST',
                enctype: 'application/x-www-form-urlencoded;charset=utf-8',
                action: this._pageUrl},
            d.INPUT({type: 'hidden', name: rp.STATE}),
            d.INPUT({type: 'hidden', name: rp.TARGET}),
            d.INPUT({type: 'hidden', name: rp.ARGUMENT}));
            
        document.body.appendChild(this._form);
    },
    
    addRequestField: function(fldName, fldValue) {
        if (fldName && fldValue) {
            if (!this._form) {
                this._createForm();
            }
            
            var existingElem = this._form[fldName];
            if (existingElem) {
                existingElem.value = fldValue;
            }
            else {
                this._form.appendChild(MochiKit.DOM.INPUT({type: 'hidden', name:fldName, value:fldValue}));
            }
        }
    },
  
    getPostDataForPrinting: function(pageState, viewerName) {
        var toJSON = MochiKit.Base.serializeJSON;
        var rp = bobj.crv.ServletAdapter._requestParams;
        var state = toJSON(pageState);
        
        var postData = rp.STATE;
        postData += '=';
        postData += encodeURIComponent(state);
        postData += '&';
        postData += rp.TARGET;
        postData += '=';
        postData += encodeURIComponent(viewerName);
        postData += '&';
        postData += rp.ARGUMENT;
        postData += '=';
        postData += encodeURIComponent('"axprint="');
        if(document.getElementById('com.sun.faces.VIEW')) {
            postData += "&com.sun.faces.VIEW=" + encodeURIComponent(document.getElementById('com.sun.faces.VIEW').value);
        }
        
        return postData;
    }
});

/*
================================================================================
AspDotNetAdapter. AspDotNetAdapter issues requests to the WebForm DHTML viewer.
================================================================================
*/

/**
 * Constructor for AspDotNetAdapter. 
 *
 * @param postbackEventReference [string]  The full functiona call to the ASP.NET dopostback function
 * @param replacementParameter [string] the string to replace in the dopostback function
 * @param stateID [string] the name of the input field to save the state to 
 */
bobj.crv.AspDotNetAdapter = function(postbackEventReference, replacementParameter, stateID, callbackEventReference) {
    this._postbackEventReference = postbackEventReference;
    this._replacementParameter = replacementParameter;
    this._stateID = stateID;
    this._form = null;
    this._callbackEventReference = callbackEventReference;
    this._additionalReqFlds = null;
    var tmpState = bobj.getElementByIdOrName(this._stateID);
    if (tmpState) {
        this._form = tmpState.form;
    }
};


bobj.crv.AspDotNetAdapter.prototype = MochiKit.Base.merge(bobj.crv.IOAdapterBase, {
    
    request: function(pageState, viewerName, eventArgs, allowAsync, callbackHandler, errbackHandler) {
    var toJSON = MochiKit.Base.serializeJSON;
    if (eventArgs && this._additionalReqFlds) {
        eventArgs = MochiKit.Base.update(eventArgs, this._additionalReqFlds);
    }
    this._additionalReqFlds = null;
    
    var jsonEventArgs = toJSON(eventArgs);

        if (allowAsync) {
            this.saveViewState(pageState, viewerName);
            if(typeof WebForm_InitCallback == "function") {
                __theFormPostData = ""; //Used by WeForm_InitCallback
                __theFormPostCollection = [];  //Used by WeForm_InitCallback
                WebForm_InitCallback(); //Need to call this to work around a problem where ASP.NET2 callback does not collect form data prior to request
            }
            var callback = this._callbackEventReference.replace("'arg'", "jsonEventArgs");
            callback = callback.replace("'cb'", "callbackHandler");
            callback = callback.replace("'errcb'", "errbackHandler");
            callback = callback.replace("'frmID'", "this._form.id");
            return eval(callback);
        }
        else {
            var postbackCall;
            if(this._postbackEventReference.indexOf("'" + this._replacementParameter + "'") >= 0){
                postbackCall = this._postbackEventReference.replace("'" + this._replacementParameter + "'", "jsonEventArgs");
            }
            else {
                postbackCall = this._postbackEventReference.replace('"' + this._replacementParameter + '"', "jsonEventArgs");
            }
            eval(postbackCall);
        }
    },
    
    saveViewState: function(pageState, viewerName) {
        var toJSON = MochiKit.Base.serializeJSON;
        var viewState = pageState[viewerName];
        var state = bobj.getElementByIdOrName(this._stateID);
        if (state) {
            state.value = toJSON(viewState);
        }
    },
    
    getPostDataForPrinting: function(pageState, viewerName) {
        var nv = MochiKit.DOM.formContents(this.form);
        var names = nv[0];
        var values = nv[1];
        names.push('crprint');
        values.push(viewerName);
        var queryString = MochiKit.Base.queryString(names, values);
        return queryString;
    },
    
    addRequestField: function(fldName, fldValue) {
        if (!this._additionalReqFlds) {
            this._additionalReqFlds = {};
        }
        this._additionalReqFlds[fldName] = fldValue;
        
        /*if (fldName && fldValue) {
            var existingElem = this._form[fldName];
            if (existingElem) {
                existingElem.value = fldValue;
            }
            else {
                this._form.appendChild(MochiKit.DOM.INPUT({type: 'hidden', name:fldName, value:fldValue}));
            }
        }*/
    }
});

/*
================================================================================
FacesAdapter. FacesAdapter issues requests to a JSF viewer component. 
================================================================================
*/

/**
 * Constructor for FacesAdapter. 
 *
 * @param formName [string]  Name of the form to submit
 * @param servletUrl [string]  URL to which requests to a servlet should be made
 *                             It doubles as the url for all asyncronous requests
 */
bobj.crv.FacesAdapter = function(formName, servletUrl) {
    this._formName = formName;
    this._servletUrl = servletUrl;
    this._useServlet = false;
    if (!bobj.crv.FacesAdapter._hasInterceptedSubmit) { 
        this._interceptSubmit();
        bobj.crv.FacesAdapter._hasInterceptedSubmit = true;
    }
};

bobj.crv.FacesAdapter._requestParams = {
    STATE: 'CRVCompositeViewState',
    TARGET: 'CRVEventTarget',
    ARGUMENT: 'CRVEventArgument'
};

bobj.crv.FacesAdapter.prototype = MochiKit.Base.merge(bobj.crv.IOAdapterBase, {
    
    request: function(pageState, viewerName, eventArgs, allowAsync) {
        
        var rp = bobj.crv.FacesAdapter._requestParams;
        var toJSON = MochiKit.Base.serializeJSON;
        var INPUT =  MochiKit.DOM.INPUT;
        
        var deferred = null;
        
        var form = this._getForm();
        if (!form) {
            return;
        }
        
        if (!form[rp.TARGET]) {
            form.appendChild( INPUT({type: 'hidden', name: rp.TARGET}) );    
        }
        form[rp.TARGET].value = encodeURIComponent(viewerName);
        
        if (!form[rp.ARGUMENT]) {
            form.appendChild( INPUT({type: 'hidden', name: rp.ARGUMENT}) );
        }
        form[rp.ARGUMENT].value = encodeURIComponent(toJSON(eventArgs));
        
        if (!form[rp.STATE]) {
            form.appendChild( INPUT({type: 'hidden', name: rp.STATE}) );
        }
        form[rp.STATE].value = encodeURIComponent(toJSON(pageState));  
        
        if (allowAsync && this._servletUrl) {
            var req = MochiKit.Async.getXMLHttpRequest();
            req.open("POST", this._servletUrl, true);
            req.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            req.setRequestHeader('Accept','application/json');
            deferred = MochiKit.Async.sendXMLHttpRequest(req, MochiKit.Base.queryString(form));
        }
        else {
            var pageUrl = form.action;
            if (this._useServlet === true) {
                form.action = this._servletUrl;
            }
            
            form.submit();
            
            form.action = pageUrl;
            this._useServlet = false;
        }
        
        // clear out the request fields so that the form can be reused
        form[rp.TARGET].value = "";
        form[rp.ARGUMENT].value = "";
        form[rp.STATE].value = "";
        
        return deferred;
    },
    
    redirectToServlet: function () {
        this._useServlet = true;
    },
    
    addRequestField: function(fldName, fldValue) {
        if (fldName && fldValue) {
            var form = this._getForm();
            
            if (form) {            
                var existingElem = form[fldName];
                if (existingElem) {
                    existingElem.value = fldValue;
                }
                else {
                    form.appendChild(MochiKit.DOM.INPUT({type: 'hidden', name:fldName, value:fldValue}));
                }
            }
        }
    },
    
    
    saveViewState: function(pageState, viewerName) {
        if (!bobj.crv.FacesAdapter._isStateSaved) {
            var form = this._getForm();
            if (form) {
                var rp = bobj.crv.FacesAdapter._requestParams;
                var toJSON = MochiKit.Base.serializeJSON;
                var INPUT =  MochiKit.DOM.INPUT;
                
                if (!form[rp.STATE]) {
                    form.appendChild( INPUT({type: 'hidden', name: rp.STATE}) );
                }
                form[rp.STATE].value = encodeURIComponent(toJSON(pageState));  
            }
            bobj.crv.FacesAdapter._isStateSaved = true;
        }
    },
    
    _getForm: function() {
        return document.forms[this._formName];   
    },
    
    _interceptSubmit: function() {
        var form = this._getForm();
        if (form) {
            var oldSubmit = form.submit;
            form.submit = function() {
                bobj.event.publish('saveViewState');
                form.submit = oldSubmit; // IE needs this
                form.submit(); // Can't apply args because IE misbehaves
            };
        }
    },
      
    getPostDataForPrinting: function(pageState, viewerName) {
        var toJSON = MochiKit.Base.serializeJSON;
        var rp = bobj.crv.ServletAdapter._requestParams;
        var state = toJSON(pageState);
        
        var postData = rp.STATE;
        postData += '=';
        postData += encodeURIComponent(state);
        postData += '&';
        postData += rp.TARGET;
        postData += '=';
        postData += encodeURIComponent(viewerName);
        postData += '&';
        postData += rp.ARGUMENT;
        postData += '=';
        postData += encodeURIComponent('"axprint="');
        if(document.getElementById('com.sun.faces.VIEW')) {
            postData += "&com.sun.faces.VIEW=" + encodeURIComponent(document.getElementById('com.sun.faces.VIEW').value);
        }
        
        return postData;
    }
});