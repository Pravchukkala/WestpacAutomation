using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KiwiSaverCalcBase.Enums
{
    /// <summary>
    /// Using Enums for Selenium findby elements,waitconditions,httpOperaions(Further extenstion)
    /// </summary>
    public enum Elements
    {
        Xpath, Css, Linktext, PartialLinkText, Id, Name, ClassName
    }
    internal enum WaitConditions
    { 
        Clickable, IsVisible, AlertIsPresent, FrameAvailablity, PresenceOfAllElements, VisibilityOfAll
    }
    public enum HttpOperations
    { 
        GET, POST, PUT, PATCH, DELETE
    }
}