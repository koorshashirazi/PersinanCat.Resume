using BoDi;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using PersianCat.E2E.Tests.E2E;
using TechTalk.SpecFlow;

namespace PersianCat.E2E.Tests.Hooks;

[Binding]
public class TestHooks
{
    private readonly IObjectContainer _objectContainer;
    private IPage _page = null!;

    public TestHooks(IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
    }

    [BeforeTestRun]
    public static async Task BeforeTestRun()
    {
        var logger = new LoggerFactory().CreateLogger<ResumeAppContainer>();
        TestRunContext.ResumeApp = await ResumeAppContainer.StartAsync(logger);
        TestRunContext.Driver = await WebAppDriver.CreateAsync();
        TestRunContext.Translations = new TranslationService();
        TestRunContext.Config = new TestConfiguration
        {
            SutUrl = TestRunContext.ResumeApp.GetBaseUrl()
        };
    }

    [AfterTestRun]
    public static async Task AfterTestRun()
    {
        if (TestRunContext.Driver is not null)
        {
            await TestRunContext.Driver.DisposeAsync();
        }

        if (TestRunContext.ResumeApp is not null)
        {
            await TestRunContext.ResumeApp.DisposeAsync();
        }
    }

    [BeforeScenario]
    public async Task BeforeScenario()
    {
        _page = await TestRunContext.Driver!.Browser.NewPageAsync();
        await _page.GotoAsync(TestRunContext.Config!.SutUrl);
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle, new() { Timeout = 30000 });
        await Task.Delay(500); // Extra safety delay
        _objectContainer.RegisterInstanceAs(_page);
        _objectContainer.RegisterInstanceAs(TestRunContext.Driver);
        _objectContainer.RegisterInstanceAs(TestRunContext.ResumeApp!);
        _objectContainer.RegisterInstanceAs(TestRunContext.Translations!);
    }

    [AfterScenario]
    public async Task AfterScenario()
    {
        await _page.CloseAsync();
    }
}
