using System.Globalization;
using Blazored.LocalStorage;
using PersianCat.Resume.Localization;

namespace PersianCat.Resume.Services;

public sealed class LanguageService
{
    public event Action? OnChange;
    public bool IsInitialized { get; private set; }

    public string CurrentLang = string.Empty;

    public static readonly IDictionary<string, string> SupportedLanguages = new Dictionary<string, string>()
    {
        { "English", "en-US" }, { "German", "de-DE" }, { "Persian", "fa-IR" }
    };

    private static readonly HashSet<string> SupportedLanguageNames = ["en-US", "de-DE", "fa-IR"];
    private readonly JavaScriptInterop _javaScriptInterop;
    private readonly ILocalStorageService _localStorage;
    private readonly JsonStringLocalizer _jsonStringLocalizer;

    public LanguageService(
        JavaScriptInterop javaScriptInterop,
        ILocalStorageService localStorage,
        JsonStringLocalizer jsonStringLocalizer)
    {
        ArgumentNullException.ThrowIfNull(javaScriptInterop);
        ArgumentNullException.ThrowIfNull(localStorage);
        ArgumentNullException.ThrowIfNull(jsonStringLocalizer);

        _javaScriptInterop = javaScriptInterop;
        _localStorage = localStorage;
        _jsonStringLocalizer = jsonStringLocalizer;
    }

    public async Task InitLanguageAsync()
    {
        var language = await _localStorage.GetItemAsStringAsync("language") ?? string.Empty;

        if (string.IsNullOrEmpty(language))
        {
            var languages = await _javaScriptInterop.GetBrowserLanguagesAsync();
            language = languages.FirstOrDefault(lang => SupportedLanguageNames.Contains(lang)) ?? "en-US";
        }

        if (!SupportedLanguageNames.Contains(language))
        {
            language = "en-US";
        }

        await SetLanguageAsync(language);
        IsInitialized = true;
    }

    public static string GetLanguageDirection() => CultureInfo.CurrentUICulture.Name == "fa-IR" ? "rtl" : "ltr";

    public async Task SetLanguageAsync(string lang)
    {
        var currentLang = lang;
        if (!SupportedLanguageNames.Contains(currentLang))
        {
            currentLang = "en-US";
        }

        await _localStorage.SetItemAsStringAsync("language", currentLang);

        var culture = new CultureInfo(currentLang);
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        _jsonStringLocalizer.ClearResourceData();
        await _jsonStringLocalizer.LoadResourceData();

        CurrentLang = currentLang;

        await _javaScriptInterop.SetBodyLanguageAsync();

        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
