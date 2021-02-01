using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechTalk.SpecFlow;

namespace KiwiSaverCalcBase.DriverUtilities
{
    public class TestBaseRunner
    {
        private PlatformDriver PlatformDriverObj;
        private readonly FeatureContext Context;

        /// <summary>
        /// Use this Constructor for Specflow-nunit Framework
        /// </summary>
        /// <param name="injectedContext"></param>
        public TestBaseRunner(FeatureContext injectedContext)
        {
            Context = injectedContext;
        }
        // Use this Constructor for Nunit Framework
        public TestBaseRunner() { }

        /// <summary>
        /// Using this Constructor for PlatformDriver
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="isHeadless"></param>
        /// <param name="implicitWaitTime"></param>
        public PlatformDriver Setup(string browser = "DesktopChrome", bool isHeadless = false, int implicitWaitTime = 30)
        {
            if (Context != null)
            {
                if (Context.FeatureInfo.Title.ToLower().Contains("desktop"))
                {
                    PlatformDriverObj = new DesktopPlatformDriver(browser, isHeadless, implicitWaitTime);
                }
                else if (Context.FeatureInfo.Title.ToLower().Contains("service"))
                {
                    PlatformDriverObj = new PlatformDriver();
                }
            }
            else
            {
                if (TestContext.CurrentContext.Test.Name.ToLower().Contains("desktop"))
                {
                    PlatformDriverObj = new DesktopPlatformDriver(browser, isHeadless, implicitWaitTime);
                }
                else if (TestContext.CurrentContext.Test.Name.ToLower().Contains("service"))
                {
                    PlatformDriverObj = new PlatformDriver();
                }
            }
            return PlatformDriverObj;
        }
        /// <summary>
        /// Teardown is used to Identify a method to called immediately after each test is run
        /// </summary>        
        public void TearDown()
        {
            if (Context != null)
            {
                if (Context.FeatureInfo.Title.ToLower().Contains("desktop"))
                {
                    ((DesktopPlatformDriver)PlatformDriverObj).UiActionsDw.QuitBrowser();
                }
            }
            else
            {
                if (TestContext.CurrentContext.Test.Name.ToLower().Contains("desktop"))
                {
                    ((DesktopPlatformDriver)PlatformDriverObj).UiActionsDw.QuitBrowser();
                }
            }

        }
    }
}