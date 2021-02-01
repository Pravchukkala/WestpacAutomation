using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KiwiSaverCalcBase.ServiceFunctionality;

namespace KiwiSaverCalcBase.DriverUtilities
{
    public enum Loglevel {DEBUG, INFO, WARN, ERROR, FATAL }
    public class PlatformDriver
    {
        public RestServiceFunctionality ServiceActions;
    }
}