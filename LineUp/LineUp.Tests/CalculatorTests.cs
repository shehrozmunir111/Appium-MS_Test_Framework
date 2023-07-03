using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium;

namespace LineUp.Tests
{
    [TestClass]
    public class CalculatorTests
    {
        static AndroidDriver<AppiumWebElement>? driver;

        //[AssemblyInitialize()]
        [ClassInitialize()]
        //[TestInitialize()]
        [Obsolete]
        public static void Setup(TestContext context)
        {
            //Appium's Android Example: https://stackoverflow.com/a/58600438/1690709
            //Appium Desired Capabilities: https://appium.io/docs/en/writing-running-appium/caps/

            string apkPath = Path.Combine(Directory.GetCurrentDirectory(), "apks", "calculator.apk");

            DesiredCapabilities capabilities = new DesiredCapabilities();
            AppiumOptions options = new AppiumOptions();
            options.PlatformName = "Android";
            options.AddAdditionalCapability("deviceName", "Android Emulator");
            options.AddAdditionalCapability("platformVersion", "11.0");
            options.AddAdditionalCapability("automationName", "UiAutomator2");
            options.AddAdditionalCapability("app", apkPath);
            options.AddAdditionalCapability("appPackage", "com.google.android.calculator");
            options.AddAdditionalCapability("androidInstallTimeout", 180000);

            Uri url = new Uri("http://127.0.0.1:4723/wd/hub");
            driver = new AndroidDriver<AppiumWebElement>(url, options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        //[AssemblyCleanup()]
        [ClassCleanup()]
        //[TestCleanup()]
        public static void CleanUp()
        {
            // shutdown
            driver!.Quit();
        }

        [TestMethod]
        [DataRow(0, 0)]
        [DataRow(1, 1)]
        [DataRow(9, 7)]
        public void Addition(int value1, int value2)
        {
            ClickSpecificDigit(value1);
            driver!.FindElementById("com.google.android.calculator:id/op_add").Click();
            ClickSpecificDigit(value2);
            driver.FindElementById("com.google.android.calculator:id/eq").Click();
            var result = driver.FindElementById("com.google.android.calculator:id/result_final").Text;
            Assert.AreEqual(result, (value1 + value2).ToString(), $"Result of {value1} + {value2} should be {(value1 + value2)}");
        }

        [TestMethod]
        [DataRow(0, 0)]
        [DataRow(5, 2)]
        [DataRow(9, 7)]
        public void Subtraction(int value1, int value2)
        {
            ClickSpecificDigit(value1);
            driver!.FindElementById("com.google.android.calculator:id/op_sub").Click();
            ClickSpecificDigit(value2);
            driver.FindElementById("com.google.android.calculator:id/eq").Click();
            var result = driver.FindElementById("com.google.android.calculator:id/result_final").Text;
            Assert.AreEqual(result, (value1 - value2).ToString(), $"Result of {value1} - {value2} should be {(value1 - value2)}");
        }

        [TestMethod]
        [DataRow(0, 0)]
        [DataRow(5, 2)]
        [DataRow(9, 7)]

        public void Multiplication(int value1, int value2) //ZIM
        {
            ClickSpecificDigit(value1);
            driver!.FindElementById("com.google.android.calculator:id/op_mul").Click();
            ClickSpecificDigit(value2);
            driver.FindElementById("com.google.android.calculator:id/eq").Click();
            var result = driver.FindElementById("com.google.android.calculator:id/result_final").Text;
            Assert.AreEqual((value1 * value2).ToString(), result, $"Result of {value1} * {value2} should be {(value1 * value2)}");
        }

        [TestMethod]
        [DataRow(0, 0)]
        [DataRow(5, 2)]
        [DataRow(9, 7)]
        [DataRow(2, 0)]
        public void Division(int value1, int value2) //ZIM
        {
            ClickSpecificDigit(value1);
            driver!.FindElementById("com.google.android.calculator:id/op_div").Click();
            ClickSpecificDigit(value2);
            driver.FindElementById("com.google.android.calculator:id/eq").Click();
            if (value2 == 0)
            {
                string expected = "Can't divide by 0";
                string resultpreview = driver.FindElementById("com.google.android.calculator:id/result_preview").Text;
                Assert.AreEqual(expected, resultpreview, false, $"Result of {(float)value1} / {(float)value2} should be {expected}");
                driver.FindElementById("com.google.android.calculator:id/clr").Click();
            }
            else
            {
                var result = driver.FindElementById("com.google.android.calculator:id/result_final").Text;
                double a = (double)value1 / (double)value2; a = Math.Round(a, 12);
                Assert.AreEqual(a.ToString(), result, $"Result of {(float)value1} / {(float)value2} should be {(float)(value1 / value2)}");
            }

        }

        #region Helper Functions
        private void ClickSpecificDigit(int value)
        {
            switch (value)
            {
                case 0:
                    driver.FindElementById("com.google.android.calculator:id/digit_0").Click();
                    break;
                case 1:
                    driver.FindElementById("com.google.android.calculator:id/digit_1").Click();
                    break;
                case 2:
                    driver.FindElementById("com.google.android.calculator:id/digit_2").Click();
                    break;
                case 3:
                    driver.FindElementById("com.google.android.calculator:id/digit_3").Click();
                    break;
                case 4:
                    driver.FindElementById("com.google.android.calculator:id/digit_4").Click();
                    break;
                case 5:
                    driver.FindElementById("com.google.android.calculator:id/digit_5").Click();
                    break;
                case 6:
                    driver.FindElementById("com.google.android.calculator:id/digit_6").Click();
                    break;
                case 7:
                    driver.FindElementById("com.google.android.calculator:id/digit_7").Click();
                    break;
                case 8:
                    driver.FindElementById("com.google.android.calculator:id/digit_8").Click();
                    break;
                case 9:
                    driver.FindElementById("com.google.android.calculator:id/digit_9").Click();
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}