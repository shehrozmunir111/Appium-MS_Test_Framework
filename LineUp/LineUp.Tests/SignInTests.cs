using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Appium.Interfaces;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;

namespace LineUp.Tests
{
    [TestClass]
    public class SignInTests
    {
        private const string EmailTextBoxId = "com.powersoft19.lineup:id/tb_SignInEmailID";
        private const string PasswordTextBoxId = "com.powersoft19.lineup:id/tb_SignInPassword";
        private const string SignInButtonId = "com.powersoft19.lineup:id/btn_SignInClickedID";
        private const string LabelLoginStatusId = "android:id/message";
        private const string HomeTabId = "//android.widget.FrameLayout[@content-desc=\"Home\"]/android.widget.FrameLayout/android.widget.ImageView";

        static AndroidDriver<AppiumWebElement>? driver;

        //[AssemblyInitialize()]
        [ClassInitialize()]
        //[TestInitialize()]
        [Obsolete]
        public static void Setup(TestContext context)
        {
            //Appium's Android Example: https://stackoverflow.com/a/58600438/1690709
            //Appium Desired Capabilities: https://appium.io/docs/en/writing-running-appium/caps/

            string apkPath = Path.Combine(Directory.GetCurrentDirectory(), "apks", "com.powersoft19.lineup-Signed (1).apk");

            DesiredCapabilities capabilities = new DesiredCapabilities();
            AppiumOptions options = new AppiumOptions();
            options.PlatformName = "Android";
            options.AddAdditionalCapability("deviceName", "Android Emulator");
            options.AddAdditionalCapability("platformVersion", "11.0");
            options.AddAdditionalCapability("automationName", "UiAutomator2");
            options.AddAdditionalCapability("app", apkPath);
            options.AddAdditionalCapability("appPackage", "com.powersoft19.lineup");
            options.AddAdditionalCapability("androidInstallTimeout", 180000);

            Uri url = new Uri("http://127.0.0.1:4723/wd/hub");
            driver = new AndroidDriver<AppiumWebElement>(url, options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        ////[AssemblyCleanup()]
        //[ClassCleanup()]
        ////[TestCleanup()]
        //public static void CleanUp()
        //{
        //    // shutdown
        //    driver!.Quit();
        //}


        [TestMethod]
        [DataRow("", "", "Invalid username or password, try again")]
        [DataRow("abc", "", "Invalid username or password, try again")]
        [DataRow("", "abc", "Invalid username or password, try again")]
        //[DataRow("abc", "abc", "The Email field is not a valid e-mail address.")] // apk issue
        [DataRow("shehroz.munir@powersoft19.com", "Welcome@123", "")]
        public void SignIn(string email, string password, string statusMessage)
        {
            AppiumWebElement emailTextBox = driver.FindElementById(EmailTextBoxId);
            emailTextBox.SendKeys(email);

            AppiumWebElement passwordTestBox = driver.FindElementById(PasswordTextBoxId);
            passwordTestBox.SendKeys(password);

            driver.FindElementById(SignInButtonId).Click();

            //var result = driver.FindElementById(LabelLoginStatusId).Text;

            var result = "";
            try
            {
                result = driver.FindElementById(LabelLoginStatusId).Text;

                //Still on Login Page
                driver.FindElementById("android:id/button2").Click();
                Thread.Sleep(2000);
                emailTextBox.Clear();
                passwordTestBox.Clear();
            }
            catch (NoSuchElementException ex)
            {
                //Moved to Home Page
            }

            Assert.AreEqual(result, statusMessage);
        }
    }
}
