using Microsoft.JSInterop;


namespace PersianCat.Resume.Services;

public interface IGoogleAnalyticsService
{
    Task TrackPageView(string pagePath, string pageTitle);
    Task TrackEvent(string eventCategory, string eventAction, string eventLabel = null, int? eventValue = null);
}

public sealed class GoogleAnalyticsService : IGoogleAnalyticsService
{
    public const string MeasurementId = "G-VRSSE3SHWF";
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<GoogleAnalyticsService> _logger;

    public GoogleAnalyticsService(IJSRuntime jsRuntime, ILogger<GoogleAnalyticsService> logger)
    {
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        _logger = logger;
    }

    public async Task TrackPageView(string pagePath, string pageTitle)
    {
        
        try
        {
            await _jsRuntime.InvokeVoidAsync("gtag", "config", MeasurementId,
                new { page_path = pagePath, page_title = pageTitle });
        }
        catch (Exception )
        {
            _logger.LogError("Tracking {PagePath} is failed.", pagePath);
        }
    }

    public async Task TrackEvent(string eventCategory, string eventAction, string? eventLabel = null,
        int? eventValue = null)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("gtag", "event", eventAction, new
            {
                event_category = eventCategory,
                event_label = eventLabel,
                value = eventValue
            });
        }
        catch (Exception )
        {
            _logger.LogError("Tracking {EventCategory}: {EventLabel} is failed.", eventCategory, eventLabel);
        }
    }
}

