

using System.Text.Encodings.Web;
using PersianCat.Resume.Services;

public sealed class Initializer
{
    private readonly ThemeService _themeService;
    private readonly LanguageService _languageService;

    public Initializer(
        ThemeService themeService,
        LanguageService languageService)
    {
        ArgumentNullException.ThrowIfNull(themeService);
        ArgumentNullException.ThrowIfNull(languageService);

        _themeService = themeService;
        _languageService = languageService;
    }

    public async Task InitializeAsync()
    {
        await _languageService.InitLanguageAsync();
        await _themeService.InitializeThemeAsync();
    }
}