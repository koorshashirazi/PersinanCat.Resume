using Microsoft.Playwright;

namespace PersianCat.E2E.Tests.E2E;

public class WebAppDriver : IAsyncDisposable
{
    public IBrowser Browser { get; }
    private readonly IPlaywright _playwright;

    private WebAppDriver(IPlaywright playwright, IBrowser browser)
    {
        _playwright = playwright;
        Browser = browser;
    }

    public static async Task<WebAppDriver> CreateAsync()
    {
        var playwright = await Playwright.CreateAsync();
        playwright.Selectors.SetTestIdAttribute("data-testid");
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
            Timeout = 6000
        });
        return new WebAppDriver(playwright, browser);
    }

    public async ValueTask DisposeAsync()
    {
        await Browser.CloseAsync();
        _playwright.Dispose();
    }
}
