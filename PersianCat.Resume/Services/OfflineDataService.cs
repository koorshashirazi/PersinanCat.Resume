using System.Text.Json;
using Microsoft.JSInterop;

namespace PersianCat.Resume.Services;

public class OfflineDataService
{
    private readonly IJSRuntime _jsRuntime;

    public OfflineDataService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SaveDataAsync<T>(string key, T data)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem",
            key, JsonSerializer.Serialize(data));
    }

    public async Task<T?> GetDataAsync<T>(string key)
    {
        var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        return JsonSerializer.Deserialize<T>(json);
    }
}
