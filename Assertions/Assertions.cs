using AventStack.ExtentReports;
using KiwiSaverCalcBase.DriverUtilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechTalk.SpecFlow;

namespace KiwiSaverCalcBase.Assertions
{
    public class Assertions
    {
        private ScenarioContext Context;
        private DesktopPlatformDriver PlatformDriverObj;

        /// <summary>
        /// Use this Contructor for Specflow-Nunit Framework
        /// </summary>
        /// <param name="context"></param>
        /// <param name="platformDriverObj"></param>
        public Assertions(ScenarioContext context, PlatformDriver platformDriverObj)
        {
            PlatformDriverObj = (DesktopPlatformDriver)platformDriverObj;
            Context = context;
        }
        /// <summary>
        /// Using this Constructor for Nunit Framework
        /// </summary>
        /// <param name="platformDriverObj"></param>
        public Assertions(PlatformDriver platformDriverObj)
        {
            PlatformDriverObj = (DesktopPlatformDriver)platformDriverObj;

        }
        /// <summary>
        /// Using this Method for Assertions
        /// </summary>
        /// <param name="successMessage"></param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="FailMessage"></param>
        /// <param name="ContinueOnfail"></param>
        public void AssertExpectedValue(string successMessage, string expected, string actual, string FailMessage, bool ContinueOnfail = true)
        {
            try
            {
                if (FailMessage == "") Assert.AreEqual(expected, actual);
                else Assert.AreEqual(expected, actual, FailMessage);
            }
            catch
            {
                if (Context != null)
                    Context.StepContext.Set<string>("Expected:" + successMessage + "Actual:" + FailMessage, "AssertionErrorMsg");
            }

        }
    }
}