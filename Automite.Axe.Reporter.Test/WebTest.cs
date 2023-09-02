using Deque.AxeCore.Commons;
using Deque.AxeCore.Selenium;
using FluentAssertions;

namespace Automite.Axe.Reporter.Test
{
    public class WebTest: WebTestFixture
    {
        [Test]
        public void Test_WholePage()
        {
            AxeResult axeResult = new AxeBuilder(driver).Analyze();
            axeResult.Violations.Should().BeEmpty();
        }

        [Test]
        public void Test_WholePage_WithReport()
        {
            Thread.Sleep(3000);
            AxeResult axeResult = new AxeBuilder(driver).Analyze();
            reporter.GenerateHtml(axeResult);
            reporter.GenerateHtml(axeResult, fileName: "testing report name");
            axeResult.Violations.Should().BeEmpty();
        }

        [Test]
        public void Test_WholePage_WithLog()
        {
            Thread.Sleep(5000);
            AxeResult axeResult = new AxeBuilder(driver).Analyze();
            reporter.Generate(axeResult);
            axeResult.Violations.Should().BeEmpty();
        }
    }
}