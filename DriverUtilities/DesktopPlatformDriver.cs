
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KiwiSaverCalcBase.DriverUtilities
{
    public class DesktopPlatformDriver:PlatformDriver
    {
        public string PlatformName;
        private readonly bool IsHeadless;
        public SeleniumUICommonFunctions UiActionsDw;

        /// <summary>
        /// Using this Constructor for DesktopPlatformDriver
        /// </summary>
        /// <param name="platformName"></param>
        /// <param name="isHeadless"></param>
        /// <param name="implicitWaitTime"></param>
        public DesktopPlatformDriver(String platformName, bool isHeadless, int implicitWaitTime)
        {
            PlatformName = platformName;
            IsHeadless = isHeadless;           
            IntializePlatformDriver(implicitWaitTime);
        }
        private void IntializePlatformDriver(int implicitWaitTime)
        {
            if (PlatformName.ToLower().Contains("desktop"))
            {
                UiActionsDw = new SeleniumUICommonFunctions(PlatformName, IsHeadless, implicitWaitTime);
            }

        }
    }
}