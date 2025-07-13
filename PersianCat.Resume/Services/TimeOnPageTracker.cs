namespace PersianCat.Resume.Services;

public sealed class TimeOnPageTracker
{
    private readonly IGoogleAnalyticsService _analyticsService;
    private DateTime _pageEnterTime;
    private string? _currentPage;

    public TimeOnPageTracker(IGoogleAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService ?? throw new ArgumentNullException(nameof(analyticsService));
    }

    public async Task StartTracking(string pageName)
    {
        if (!string.IsNullOrEmpty(_currentPage))
        {
            await EndTracking().ConfigureAwait(false);
        }

        _currentPage = pageName;
        _pageEnterTime = DateTime.Now;
    }

    public async Task EndTracking()
    {
        if (string.IsNullOrEmpty(_currentPage))
            return;

        var timeSpent = (int)(DateTime.Now - _pageEnterTime).TotalSeconds;

        await _analyticsService.TrackEvent(
            "engagement",
            "time_on_page",
            _currentPage,
            timeSpent).ConfigureAwait(false);

        _currentPage = null;
    }
}