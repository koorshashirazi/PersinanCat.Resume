using Microsoft.JSInterop;

namespace PersianCat.Resume.Services;

public class NetworkService
{
    private readonly IJSRuntime _jsRuntime;

    public NetworkService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> IsOnlineAsync()
    {
        return await _jsRuntime.InvokeAsync<bool>("navigator.onLine");
    }
}