using KiwiSaverCalcBase.Enums;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Windows.Forms;


namespace KiwiSaverCalcBase
{   
    public class SeleniumUICommonFunctions
    {
        /// <summary>
        /// SeleniumUICommonFunctions defines all the common functionality like Element find by
        /// DOM used methods like sentext,element availability etc.
        /// </summary>
        
        
        public IWebDriver Driver;
        public IJavaScriptExecutor DriverJs;
        public string path;

        /// <summary>
        /// Using this Constructor for SeleniumUICommonFunctions
        /// </summary>
        /// <param name="platformName"></param>
        /// <param name="isHeadless"></param>
        /// <param name="implicitWaitTime"></param>
        public SeleniumUICommonFunctions(string platformName, bool isHeadless, int ImplictWaittime)
        {

            switch (platformName.ToLower())
            {
                case "desktopchrome":
                    if (isHeadless)
                    {
                        var chromeOptions = new ChromeOptions();
                        chromeOptions.AddArguments(new List<string>() { "headless" });
                        chromeOptions.AddArgument("window-size=1920x1080");
                        chromeOptions.AddArgument("enable-automation");
                        chromeOptions.AddArgument("headless");
                        chromeOptions.AddArgument("no-sandbox");
                        chromeOptions.AddArgument("disable-extensions");
                        chromeOptions.AddArgument("dns-prefetch-disable");
                        chromeOptions.AddArgument("disable-gpu");
                        chromeOptions.AddAdditionalCapability("pageLoadStrategy", "none");

                        Driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), chromeOptions, TimeSpan.FromMinutes(3));
                        Driver.Manage().Timeouts().PageLoad.Add(System.TimeSpan.FromSeconds(120));

                    }
                    else
                    {
                        Driver = new ChromeDriver();
                    }
                    break;
                case "desktopie":
                    Driver = new InternetExplorerDriver();
                    break;
                case "desktopfirefox":
                    Driver = new FirefoxDriver();
                    break;
                default:
                    throw new Exception(string.Format("Selected {0} driver is invalid", platformName.ToLower()));

            }            
            Driver.Manage().Window.Maximize();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(ImplictWaittime);
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(100);
            DriverJs = (IJavaScriptExecutor)Driver;
        }
        public void GetURL(String url)
        {
            try
            {
                Driver.Url = url;
            }
            catch
            {
                Driver.Navigate().GoToUrl(url);
            }
        }

        /// <summary>
        /// Using this Interface for WebElement
        /// </summary>
        /// <param name="element"></param>       
        private IWebElement GetWebElement(string element)
        {
            try
            {
                return Driver.FindElement(GetLocator(element));
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Catch lock in GetWebElement");
                return WaitForElement<IWebElement>(element, WaitConditions.IsVisible);
            }

        }
        /// <summary>
        /// IList Using this Interface for List of WebElement
        /// </summary>
        /// <param name="element"></param>       
        public IList<IWebElement> GetWebElements(string element)
        {
            return Driver.FindElements(GetLocator(element));
        }

        private By GetLocator(string element)
        {
            string Delimiter = "###";
            string[] elementArray = element.Split(new string[] { Delimiter }, StringSplitOptions.None);
            return GetLocator(elementArray[1], elementArray[0]);
        }
        /// <summary>
        /// By Class Using to locate DOM elements
        /// </summary>
        /// <param name="element"></param>  
        /// <returns>by</returns>
        private By GetLocator(string element, string elementType)
        {
            By by = null;
            switch (elementType.ToUpper())
            {
                case "XPATH":
                    by = By.XPath(element);
                    break;
                case "CSS":
                    by = By.CssSelector(element);
                    break;
                case "LINKTEXT":
                    by = By.LinkText(element);
                    break;
                case "PARTIALLINKTEXT":
                    by = By.PartialLinkText(element);
                    break;
                case "ID":
                    by = By.Id(element);
                    break;
                case "NAME":
                    by = By.Name(element);
                    break;
                case "CLASSNAME":
                    by = By.ClassName(element);
                    break;
                default:
                    throw new Exception(string.Format("Invalid elementType : {0}", elementType));

            }
            return by;
        }
        /// <summary>
        /// GetText Using to get the value of DOM element
        /// </summary>
        /// <param name="element"></param>  
        public string GetText(string element)
        {
            return GetWebElement(element).Text;
        }
        /// <summary>
        /// PageTitle returns the title of the Page
        /// </summary>        
        public string PageTitle()
        {
            return Driver.Title;
        }
        /// <summary>
        /// GetCurrentPageUrl returns the Current Page URL
        /// </summary>        
        public string GetCurrentPageUrl()
        {
            return Driver.Url;
        }       
        /// <summary>
        /// WaitForElement using to wait for DOM elements to load
        /// </summary>
        /// <param name="element"></param>  
        public void WaitForElement(string element, bool FailOnInvisible = true, int waitUntilSeconds = 30)
        {
            int seconds = 0;
            while (seconds <= waitUntilSeconds)
            {
                seconds += 1;
                try
                {
                    WaitForElement<IWebElement>(element, WaitConditions.IsVisible, seconds);
                    break;
                }
                catch (Exception e)
                {
                    if (seconds == waitUntilSeconds && FailOnInvisible) throw e;
                }
            }

        }

        public bool IsElementClickable(string element, bool FailOnInvisible = false, int waitUntilSeconds = 30)
        {
            try
            {
                WaitForElement<IWebElement>(element, WaitConditions.Clickable);
                return true;
            }
            catch (Exception e)
            {
                if (FailOnInvisible) throw e;
                return false;
            }
        }
       
        /// </summary>
        /// <typeparam name="T">Provide IwebElement or IAlert or IList<IwebElement></typeparam>
        /// <param name="element"></param>
        /// <param name="waitUntilSeconds"></param>
        /// <returns></returns>
        private T WaitForElement<T>(string element, WaitConditions conditions, int waitUntilSeconds = 30)
        {
            Func<IWebDriver, object> condition;
            switch (conditions)
            {
                case WaitConditions.Clickable:
                    condition = ExpectedConditions.ElementToBeClickable(GetLocator(element));
                    break;
                case WaitConditions.IsVisible:
                    condition = ExpectedConditions.ElementIsVisible(GetLocator(element));
                    break;
                case WaitConditions.AlertIsPresent:
                    condition = ExpectedConditions.AlertIsPresent();
                    break;
                case WaitConditions.FrameAvailablity:
                    condition = ExpectedConditions.FrameToBeAvailableAndSwitchToIt(GetLocator(element));
                    break;
                case WaitConditions.PresenceOfAllElements:
                    condition = ExpectedConditions.PresenceOfAllElementsLocatedBy(GetLocator(element));
                    break;
                case WaitConditions.VisibilityOfAll:
                    condition = ExpectedConditions.VisibilityOfAllElementsLocatedBy(GetLocator(element));
                    break;
                default:
                    throw new Exception(string.Format("Code for {0} yet to be implemented", conditions.ToString()));
            }

            return (T)new WebDriverWait(Driver, System.TimeSpan.FromSeconds(waitUntilSeconds)).Until(condition);
        }

        public void ClickOnUsingJavaScriptExecution(string element)
        {
            DriverJs.ExecuteScript("arguments[0].click();", GetWebElement(element));
        }
        public void ScrollDownUsingJavaScriptExecution()
        {
            DriverJs.ExecuteScript("window.scrollBy(0,350)", "");
        }
        /// <summary>
        /// ClickOn Using to Click on a Element
        /// </summary>
        /// <param name="element"></param>         
        public void ClickOn(string element)
        {
            WaitForElement<IWebElement>(element, WaitConditions.Clickable).Click();
        }
        /// <summary>
        /// SendText Using to enter a value in Element
        /// </summary>
        /// <param name="element"></param>  
        /// <param name="value"></param>  
        public void SendText(string element, string value)
        {
            IWebElement wElement = GetWebElement(element);
            wElement.Clear();
            wElement.SendKeys(value);
        }

        public void RemoveReadolyForInputFieldInSalesForce()
        {
            IList<IWebElement> inputs = Driver.FindElements(By.TagName("input"));
            foreach (IWebElement input in inputs)
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].removeAttribute('readonly','readonly')", input);
            }
        }

        public void SelectFromDropDown(string element, string visibletext)
        {
            new SelectElement(GetWebElement(element)).SelectByText(visibletext);
        }

        public void ClickonGridRowValue(string Name, string element, string element2)
        {
            int rowCount = GetWebElements(element).Count();

            for (int i = 1; i <= rowCount; i++)
            {
                string sCellValue = GetWebElement(element2).Text;

                if (sCellValue.Equals(Name))
                {
                    GetWebElement(element2).Click();
                }
            }
        }


        public void SelectFromDropDownUsingOption(string element, string textvalue)
        {
            SelectElement ss = new SelectElement(GetWebElement(element));
            foreach (IWebElement element1 in ss.Options)
            {
                if (element1.GetAttribute("value") == textvalue)
                {
                    element1.Click();
                }
            }
        }

        public void SwitchtoCurrentWindow()
        {
            Driver.SwitchTo().Window(Driver.WindowHandles.Last());
        }
        public void SwitchtoParenttWindow()
        {
            Driver.SwitchTo().Window(Driver.WindowHandles.First());
        }

        public IOptions Manage()
        {
            return Driver.Manage();
        }

        public void SwitchtoFrame(string element)
        {
            Driver = WaitForElement<IWebDriver>(element, WaitConditions.FrameAvailablity);
        }

        public void SwitchDefaultFrame()
        {
            Driver.SwitchTo().DefaultContent();
        }

        public void HoveronElement(string element)
        {
            new Actions(Driver).MoveToElement(GetWebElement(element)).Build().Perform();
        }

        public void HoverAndClickonElement(string element)
        {
            new Actions(Driver).MoveToElement(GetWebElement(element)).Click().Build().Perform();

        }
        public void DragAndDropElement(string sourceelement, string targetelement)
        {
            new Actions(Driver).DragAndDrop(GetWebElement(sourceelement), GetWebElement(targetelement)).Build().Perform();

        }
        public void SwitchToAlertAndAccept()
        {
            IAlert alert = Driver.SwitchTo().Alert();
            //Thread.Sleep(2000);
            alert.Accept();
        }
        public void SwitchToAlertAndDismiss()
        {
            IAlert alert = Driver.SwitchTo().Alert();
            //Thread.Sleep(2000);
            alert.Dismiss();
        }

        public Boolean IsElementFound(string element)
        {
            try
            {
                GetWebElement(element);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public Boolean IsElementEnabled(string element)
        {
            try
            {
               return GetWebElement(element).Enabled;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public void DoubleClickOn(string element)
        {
            IWebElement wElement = GetWebElement(element);
            Actions action = new Actions(Driver);
            action.DoubleClick(wElement).Build().Perform();

        }
        public bool IsElementVisible(string element)
        {
            bool returnValue = false;
            try
            {
                WaitForElement(element);
                returnValue = GetWebElement(element).Displayed;
            }
            catch (NoSuchElementException e)
            {
                Console.WriteLine("Element " + element + "not found on page " + PageTitle());
                returnValue = false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown error " + e.Message + " occurred on page " + PageTitle());
                returnValue = false;
            }
            return returnValue;
        }

        public void SelectMulipleOptionsFromDropdrownList(string elementList, List<String> visibletextList)
        {
            foreach (IWebElement element in GetWebElements(elementList))
            {
                visibletextList.ForEach(a =>
                {
                    if (a.Trim() == element.Text.Trim())
                    {
                        element.Click();
                    }
                });
            }
        }

        public void SelectFromDropDownList(string elementList, string visibletext)
        {
            foreach (IWebElement element in GetWebElements(elementList))
            {
                if (visibletext.Trim() == element.Text.Trim())
                {
                    element.Click();
                    break;
                }
            }
        }

        public void GetListOfWebElements(string elementList)
        {
            GetWebElements(elementList).ToList();
        }

        public void SortWebTableColumn(string elementList1, string element, string elementList2)
        {
            var list1 = GetWebElements(elementList1).ToList();
            HoverAndClickonElement(element);           
            IsElementVisible(elementList2);
            var list2 = GetWebElements(elementList2).ToList();
            Assert.AreNotEqual(list1, list2);
        }

        public int GetCountOfElements(string elementList)
        {
            return GetWebElements(elementList).Count;
        }

        public void QuitBrowser()
        {
            Driver.Quit();

        }
        public void CloseBrowser()
        {
            Driver.Close();
        }

        public void OSFileUpload(String element, String filepath)
        {
            WaitForElement<IWebElement>(element, WaitConditions.Clickable).Click();
            SendKeys.SendWait(filepath);
            SendKeys.SendWait("{ENTER}");
            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(100);
        }
    }
}