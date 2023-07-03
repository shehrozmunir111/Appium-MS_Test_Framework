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
    public class SignUpTests
    {
        private const string UserNameTextBoxId = "com.powersoft19.lineup:id/tb_Username";
        private const string UserRoleEditTextId = "com.powersoft19.lineup:id/cb_UserRole";
        private const string AdminUserRole = "/hierarchy/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.ListView/android.widget.TextView[3]";
        //private const string AdminUserRole = "com.powersoft19.lineup:id/cb_UserRole";
        //private const string SuperAdminUserRole = "com.powersoft19.lineup:id/cb_UserRole";
        //private const string DevelperUserRole = "com.powersoft19.lineup:id/cb_UserRole";
        private const string EmailTextBoxId = "com.powersoft19.lineup:id/tb_UserEmail";
        private const string PasswordTextBoxId = "com.powersoft19.lineup:id/tb_UserPassword";
        private const string ConfirmPasswordTextBoxId = "com.powersoft19.lineup:id/tb_UserConfirmPassword";
        private const string SignUpButtonId = "com.powersoft19.lineup:id/btn_SignUpClicked";
        private const string LabelSignUpStatusId = "com.powersoft19.lineup:id/lbl_SignUpStatus";

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
            options.AddAdditionalCapability("appActivity", "crc64c3fac46b915e394f.MainActivity");
            options.AddAdditionalCapability("androidInstallTimeout", 180000);

            Uri url = new Uri("http://127.0.0.1:4723/wd/hub");
            driver = new AndroidDriver<AppiumWebElement>(url, options);
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
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
        //[DataRow("", "", "", "", "", "Fill All Fields")]
        //[DataRow("abc", AdminUserRole, "abc", "abc", "abc", "Email format must be like ads@sdf.com")]
        //[DataRow("", AdminUserRole, "abc@abc.com", "abc", "abc", "Fill All Fields")]
        //[DataRow("abc", "", "abc@abc.com", "abc", "abc", "Password Must have 8 characters and one Upper case Letter and digit and Special Character")] // UserRole cannot be empty
        ////[DataRow("abc", "", "abc@abc.com", "Ps19@atm", "Ps19@atm", "UserRole cannot be empty")] // application crash
        //[DataRow("abc", AdminUserRole, "abc@abc.com", "ASDFGHJK", "ASDFGHJK", "Password Must have 8 characters and one Upper case Letter and digit and Special Character")] // Password Must have 8 characters and must have one Upper case Letter, one Lower case Letter, one digit and one Special Character
        //[DataRow("abc", AdminUserRole, "abc@abc.com", "123456789", "123456789", "Password Must have 8 characters and one Upper case Letter and digit and Special Character")] // Password Must have 8 characters and must have one Upper case Letter, one Lower case Letter, one digit and one Special Character
        //[DataRow("abc", AdminUserRole, "abc@abc.com", "!@#$%^&*(", "!@#$%^&*(", "Password Must have 8 characters and one Upper case Letter and digit and Special Character")] // Password Must have 8 characters and must have one Upper case Letter, one Lower case Letter, one digit and one Special Character
        //[DataRow("abc", AdminUserRole, "abc@abc.com", "asdfghjkl", "asdfghjkl", "Password Must have 8 characters and one Upper case Letter and digit and Special Character")] // Password Must have 8 characters and must have one Upper case Letter, one Lower case Letter, one digit and one Special Character
        ////[DataRow("abc", AdminUserRole, "abc@abc.com", "Ps19@atm", "Ps19@atmatm", "The Profile Image field is required.")] // apk issue
        [DataRow("abc", AdminUserRole, "abc@abc2.com", "Ps19@atm", "Ps19@atm", "")] // application crash
        public void SignUp(string userName, string userRole , string email, string password, string confirmPassword, string statusMessage)
                {

            try
            {
                if (driver.FindElementById(UserNameTextBoxId) is null) { } ;
            }
            catch (Exception ex)
            {
                if(ex.Message== "An element could not be located on the page using the given search parameters.")
                {
                    AppiumWebElement signUpLink = driver.FindElementById("com.powersoft19.lineup:id/lbl_SignUpLink");
                    signUpLink.Click();
                }

            }
            AppiumWebElement userNameTextBox = driver.FindElementById(UserNameTextBoxId);
            userNameTextBox.SendKeys(userName);

            if (!string.IsNullOrEmpty(userRole))
            {
                driver.FindElementById(UserRoleEditTextId).Click();
                driver.FindElementByXPath(userRole).Click(); 
            }

            AppiumWebElement emailTextBox = driver.FindElementById(EmailTextBoxId);
            emailTextBox.SendKeys(email);

            AppiumWebElement passwordTextBox = driver.FindElementById(PasswordTextBoxId);
            passwordTextBox.SendKeys(password);

            AppiumWebElement confirmPasswordTextBox = driver.FindElementById(ConfirmPasswordTextBoxId);
            confirmPasswordTextBox.SendKeys(confirmPassword);

            driver.FindElementById(SignUpButtonId).Click();

            var result = driver.FindElementById(LabelSignUpStatusId).Text;

            Thread.Sleep(2000);

            userNameTextBox.Clear();
            emailTextBox.Clear();
            passwordTextBox.Clear();
            confirmPasswordTextBox.Clear();
            //driver.FindElementById("com.powersoft19.lineup:id/lbl_SignInLink").Click();

            Assert.AreEqual(statusMessage, result);
        }
    }
}
