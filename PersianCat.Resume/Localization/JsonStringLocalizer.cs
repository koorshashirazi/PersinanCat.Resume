using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Localization;

namespace PersianCat.Resume.Localization;

public class JsonStringLocalizer : IStringLocalizer
{
    private readonly HttpClient _httpClient;

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private Dictionary<string, string> _resourceData = [];

    public JsonStringLocalizer(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        _httpClient = httpClient;
    }

    public LocalizedString this[string name]
    {
        get
        {
            var value = GetString(name);
            return new LocalizedString(name, value, string.IsNullOrEmpty(value));
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var actualValue = this[name];
            return !actualValue.ResourceNotFound
                ? new LocalizedString(name, string.Format(actualValue.Value, arguments), false)
                : actualValue;
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        return _resourceData.Select(x => new LocalizedString(x.Key, x.Value, false));
    }

    private string GetString(string key)
    {
        return _resourceData.GetValueOrDefault(key, key);
    }

    public void ClearResourceData()
    {
        _resourceData.Clear();
    }

    public async Task LoadResourceData()
    {
        if (_resourceData.Count > 0) return; // Data already loaded

        var culture = CultureInfo.CurrentCulture;
        var filePath = Path.Combine("Resources", $"{culture.Name}.json");
        try
        {
            var json = await _httpClient.GetStringAsync(filePath);
            _resourceData = JsonSerializer.Deserialize<Dictionary<string, string>>(json, _serializerOptions) ?? [];
        }
        catch
        {
            // Log or handle the error appropriately
            _resourceData = new Dictionary<string, string>(); // Initialize empty to prevent repeated attempts
        }
    }
}
