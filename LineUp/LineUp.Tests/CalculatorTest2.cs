using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineUp.Tests
{
    [TestClass]
    public class CalculatorTest2
    {
        static AndroidDriver<AppiumWebElement>? driverOne;
        static AndroidDriver<AppiumWebElement>? driverTwo;


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
            //Use this property to run specific emulator using `adb devices` using cmd
            options.AddAdditionalCapability("udid", "emulator-5556");               
            options.AddAdditionalCapability("platformVersion", "11.0");
            options.AddAdditionalCapability("automationName", "UiAutomator2");
            options.AddAdditionalCapability("app", apkPath);
            options.AddAdditionalCapability("appPackage", "com.google.android.calculator");
            options.AddAdditionalCapability("androidInstallTimeout", 180000);

            Uri urlOne = new Uri("http://127.0.0.1:4723/wd/hub");
            driverOne = new AndroidDriver<AppiumWebElement>(urlOne, options);
            driverOne.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

            options.AddAdditionalCapability("udid", "emulator-5554");

            Uri urlTwo = new Uri("http://127.0.0.1:4724/wd/hub");
            driverTwo = new AndroidDriver<AppiumWebElement>(urlTwo, options);
            driverTwo.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        }

        //[AssemblyCleanup()]
        [ClassCleanup()]
        //[TestCleanup()]
        public static void CleanUp()
        {
            // shutdown
            driverOne!.Quit();
            driverTwo!.Quit();
        }

        [TestMethod]
        [DataRow(0, 0)]
        [DataRow(1, 1)]
        [DataRow(9, 7)]
        public void Addition(int value1, int value2)
        {
            ClickSpecificDigit(value1);
            driverOne!.FindElementById("com.google.android.calculator:id/op_add").Click();
            driverTwo!.FindElementById("com.google.android.calculator:id/op_add").Click();
            ClickSpecificDigit(value2);
            driverOne.FindElementById("com.google.android.calculator:id/eq").Click();
            driverTwo.FindElementById("com.google.android.calculator:id/eq").Click();
            var resultOne = driverOne.FindElementById("com.google.android.calculator:id/result_final").Text;
            var resultTwo = driverTwo.FindElementById("com.google.android.calculator:id/result_final").Text;
            Assert.AreEqual(resultOne, (value1 + value2).ToString(), $"Result of {value1} + {value2} should be {(value1 + value2)}");
            Assert.AreEqual(resultTwo, (value1 + value2).ToString(), $"Result of {value1} + {value2} should be {(value1 + value2)}");
        }

        [TestMethod]
        [DataRow(0, 0)]
        [DataRow(5, 2)]
        [DataRow(9, 7)]
        public void Subtraction(int value1, int value2)
        {
            ClickSpecificDigit(value1);
            driverOne!.FindElementById("com.google.android.calculator:id/op_sub").Click();
            driverTwo!.FindElementById("com.google.android.calculator:id/op_sub").Click();
            ClickSpecificDigit(value2);
            driverOne.FindElementById("com.google.android.calculator:id/eq").Click();
            driverTwo.FindElementById("com.google.android.calculator:id/eq").Click();
            var resultOne = driverOne.FindElementById("com.google.android.calculator:id/result_final").Text;
            var resultTwo = driverTwo.FindElementById("com.google.android.calculator:id/result_final").Text;
            Assert.AreEqual(resultOne, (value1 - value2).ToString(), $"Result of {value1} - {value2} should be {(value1 - value2)}");
            Assert.AreEqual(resultTwo, (value1 - value2).ToString(), $"Result of {value1} - {value2} should be {(value1 - value2)}");
        }

        [TestMethod]
        [DataRow(0, 0)]
        [DataRow(5, 2)]
        [DataRow(9, 7)]

        public void Multiplication(int value1, int value2)
        {
            ClickSpecificDigit(value1);
            driverOne!.FindElementById("com.google.android.calculator:id/op_mul").Click();
            driverTwo!.FindElementById("com.google.android.calculator:id/op_mul").Click();
            ClickSpecificDigit(value2);
            driverOne.FindElementById("com.google.android.calculator:id/eq").Click();
            driverTwo.FindElementById("com.google.android.calculator:id/eq").Click();
            var resultOne = driverOne.FindElementById("com.google.android.calculator:id/result_final").Text;
            var resultTwo = driverTwo.FindElementById("com.google.android.calculator:id/result_final").Text;
            Assert.AreEqual((value1 * value2).ToString(), resultOne, $"Result of {value1} * {value2} should be {(value1 * value2)}");
            Assert.AreEqual((value1 * value2).ToString(), resultTwo, $"Result of {value1} * {value2} should be {(value1 * value2)}");
        }

        [TestMethod]
        [DataRow(0, 0)]
        [DataRow(5, 2)]
        [DataRow(9, 7)]
        [DataRow(2, 0)]
        public void Division(int value1, int value2)
        {
            ClickSpecificDigit(value1);
            driverOne!.FindElementById("com.google.android.calculator:id/op_div").Click();
            driverTwo!.FindElementById("com.google.android.calculator:id/op_div").Click();
            ClickSpecificDigit(value2);
            driverOne.FindElementById("com.google.android.calculator:id/eq").Click();
            driverTwo.FindElementById("com.google.android.calculator:id/eq").Click();
            if (value2 == 0)
            {
                string expected = "Can't divide by 0";
                string resultpreviewOne = driverOne.FindElementById("com.google.android.calculator:id/result_preview").Text;
                string resultpreviewTwo = driverTwo.FindElementById("com.google.android.calculator:id/result_preview").Text;
                Assert.AreEqual(expected, resultpreviewOne, false, $"Result of {(float)value1} / {(float)value2} should be {expected}");
                Assert.AreEqual(expected, resultpreviewTwo, false, $"Result of {(float)value1} / {(float)value2} should be {expected}");
                driverOne.FindElementById("com.google.android.calculator:id/clr").Click();
                driverTwo.FindElementById("com.google.android.calculator:id/clr").Click();
            }
            else
            {
                var resultOne = driverOne.FindElementById("com.google.android.calculator:id/result_final").Text;
                var resultTwo = driverTwo.FindElementById("com.google.android.calculator:id/result_final").Text;
                double a = (double)value1 / (double)value2; a = Math.Round(a, 12);
                Assert.AreEqual(a.ToString(), resultOne, $"Result of {(float)value1} / {(float)value2} should be {(float)(value1 / value2)}");
                Assert.AreEqual(a.ToString(), resultTwo, $"Result of {(float)value1} / {(float)value2} should be {(float)(value1 / value2)}");
            }

        }

        #region Helper Functions
        private void ClickSpecificDigit(int value)
        {
            switch (value)
            {
                case 0:
                    driverOne.FindElementById("com.google.android.calculator:id/digit_0").Click();
                    driverTwo.FindElementById("com.google.android.calculator:id/digit_0").Click();
                    break;
                case 1:
                    driverOne.FindElementById("com.google.android.calculator:id/digit_1").Click();
                    driverTwo.FindElementById("com.google.android.calculator:id/digit_1").Click();
                    break;
                case 2:
                    driverOne.FindElementById("com.google.android.calculator:id/digit_2").Click();
                    driverTwo.FindElementById("com.google.android.calculator:id/digit_2").Click();
                    break;
                case 3:
                    driverOne.FindElementById("com.google.android.calculator:id/digit_3").Click();
                    driverTwo.FindElementById("com.google.android.calculator:id/digit_3").Click();
                    break;
                case 4:
                    driverOne.FindElementById("com.google.android.calculator:id/digit_4").Click();
                    driverTwo.FindElementById("com.google.android.calculator:id/digit_4").Click();
                    break;
                case 5:
                    driverOne.FindElementById("com.google.android.calculator:id/digit_5").Click();
                    driverTwo.FindElementById("com.google.android.calculator:id/digit_5").Click();
                    break;
                case 6:
                    driverOne.FindElementById("com.google.android.calculator:id/digit_6").Click();
                    driverTwo.FindElementById("com.google.android.calculator:id/digit_6").Click();
                    break;
                case 7:
                    driverOne.FindElementById("com.google.android.calculator:id/digit_7").Click();
                    driverTwo.FindElementById("com.google.android.calculator:id/digit_7").Click();
                    break;
                case 8:
                    driverOne.FindElementById("com.google.android.calculator:id/digit_8").Click();
                    driverTwo.FindElementById("com.google.android.calculator:id/digit_8").Click();
                    break;
                case 9:
                    driverOne.FindElementById("com.google.android.calculator:id/digit_9").Click();
                    driverTwo.FindElementById("com.google.android.calculator:id/digit_9").Click();
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
