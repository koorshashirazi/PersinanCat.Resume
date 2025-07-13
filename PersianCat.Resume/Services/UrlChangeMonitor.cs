using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace PersianCat.Resume.Services;

public sealed class UrlChangeMonitor : IAsyncDisposable
{
    private readonly NavigationManager _nav;
    private readonly IJSRuntime _js;
    private DotNetObjectReference<UrlChangeMonitor>? _dotNetRef;

    public event Action<string>? UrlChanged;

    public UrlChangeMonitor(NavigationManager nav, IJSRuntime js)
    {
        _nav = nav;
        _js = js;

       _nav.LocationChanged += HandleBlazorUrlChange;
    }

    public async Task InitializeAsync()
    {
        _dotNetRef = DotNetObjectReference.Create(this);
        await _js.InvokeVoidAsync("urlMonitor.init", _dotNetRef);
    }

    private void HandleBlazorUrlChange(object? sender, LocationChangedEventArgs e)
    {
        UrlChanged?.Invoke(e.Location);
    }

    [JSInvokable]
    public void OnUrlChanged(string url)
    {
        UrlChanged?.Invoke(url);
    }

    public async ValueTask DisposeAsync()
    {
        _nav.LocationChanged -= HandleBlazorUrlChange;
        _dotNetRef?.Dispose();

        if (_js is not null)
        {
            await _js.InvokeVoidAsync("urlMonitor.dispose");
        }
    }
}
