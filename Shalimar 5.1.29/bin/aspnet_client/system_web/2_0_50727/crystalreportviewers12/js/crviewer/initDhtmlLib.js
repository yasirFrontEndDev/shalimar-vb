/* Copyright (c) Business Objects 2006. All rights reserved. */

if (bobj.crv.config.isDebug) {
    // DHTML Lib makes some errors hard to debug by setting window.onerror
    // (esp with mozilla and the customcombo widget)
    localErrHandler = null;    
}
initDom(bobj.crvUri("../dhtmllib/images/") + bobj.crv.config.skin + "/", bobj.crv.config.lang);
styleSheet();
