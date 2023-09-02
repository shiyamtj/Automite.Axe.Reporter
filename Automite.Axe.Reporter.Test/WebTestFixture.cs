using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace Automite.Axe.Reporter.Test
{
    [TestFixture]
    public class WebTestFixture
    {
        internal IWebDriver driver;
        internal IAxeReporter reporter;

        public WebTestFixture()
        {
            reporter = new AxeReporterImpl();
        }

        [SetUp]
        public void Setup()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");
            driver = new ChromeDriver(chromeOptions);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            driver.Manage().Window.Maximize();
            driver.Url = "https://opensource-demo.orangehrmlive.com/";
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }
    }
}
