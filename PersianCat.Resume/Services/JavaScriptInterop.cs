using Microsoft.JSInterop;

namespace PersianCat.Resume.Services;

public sealed class JavaScriptInterop
{
    private readonly IJSRuntime _jsRuntime;

    public JavaScriptInterop(IJSRuntime jsRuntime)
    {
        ArgumentNullException.ThrowIfNull(jsRuntime);
        _jsRuntime = jsRuntime;
    }

    public ValueTask InitializeUiAsync()
    {
        return _jsRuntime.InvokeVoidAsync("blazorInterop.initializeUi");
    }

    public ValueTask ToggleThemeAsync()
    {
        return _jsRuntime.InvokeVoidAsync("blazorInterop.toggleTheme");
    }

    public ValueTask SetThemeAsync(string theme)
    {
        return _jsRuntime.InvokeVoidAsync("blazorInterop.setTheme", theme);
    }

    public ValueTask<string> GetThemeAsync()
    {
        return _jsRuntime.InvokeAsync<string>("blazorInterop.getTheme");
    }

    public ValueTask InitializeThemeAsync()
    {
        return _jsRuntime.InvokeVoidAsync("blazorInterop.initializeTheme");
    }


    public ValueTask<string[]> GetBrowserLanguagesAsync()
    {
        return _jsRuntime.InvokeAsync<string[]>("blazorInterop.getBrowserLanguages");
    }

    public ValueTask SetBodyLanguageAsync()
    {
        return _jsRuntime.InvokeVoidAsync("blazorInterop.setBodyLanguage");
    }
}
