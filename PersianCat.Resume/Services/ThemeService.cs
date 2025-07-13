
namespace PersianCat.Resume.Services
{
    public class ThemeService
    {

        private readonly JavaScriptInterop _javaScriptInterop;

        public ThemeService(JavaScriptInterop javaScriptInterop)
        {
            ArgumentNullException.ThrowIfNull(javaScriptInterop);
            _javaScriptInterop = javaScriptInterop;
        }

        public Theme CurrentTheme { get; private set; } = Theme.Light;

        public event Action<Theme>? OnChangedTheme;

        public async Task InitializeThemeAsync()
        {
            await _javaScriptInterop.InitializeThemeAsync();
            CurrentTheme = await GetCurrentThemeAsync();
            OnChangedTheme?.Invoke(CurrentTheme);
        }

        public async Task ToggleThemeAsync()
        {
            await _javaScriptInterop.ToggleThemeAsync();
            CurrentTheme = await GetCurrentThemeAsync();
            OnChangedTheme?.Invoke(CurrentTheme);
        }

        public async Task SetThemeAsync(Theme theme)
        {
            var themeString = MapThemeToString(theme);
            await _javaScriptInterop.SetThemeAsync(themeString);
            CurrentTheme = await GetCurrentThemeAsync();
            OnChangedTheme?.Invoke(CurrentTheme);
        }

        private async Task<Theme> GetCurrentThemeAsync()
        {
            var themeString = await _javaScriptInterop.GetThemeAsync();
            if (Enum.TryParse<Theme>(themeString, true, out var theme))
            {
                return theme;
            }
            return Theme.System; // Default to System if parsing fails
        }

        private static string MapThemeToString(Theme theme)
        {
            return theme switch
            {
                Theme.Light => "light",
                Theme.Dark => "dark",
                _ => "system"
            };
        }
    }

    public enum Theme
    {
        Light,
        Dark,
        System
    }
}
