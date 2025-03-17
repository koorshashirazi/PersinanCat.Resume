using Microsoft.JSInterop;

public class JavaScriptInterop
{
    private readonly IJSRuntime _jsRuntime;

    public JavaScriptInterop(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAOS()
    {
        await _jsRuntime.InvokeVoidAsync("blazorInterop.initializeAOS");
    }

    public async Task HandleScroll()
    {
        await _jsRuntime.InvokeVoidAsync("blazorInterop.handleScroll");
    }

    public async Task InitializeMasonry()
    {
        await _jsRuntime.InvokeVoidAsync("blazorInterop.initializeMasonry");
    }

    public async Task InitializeBigPicture()
    {
        await _jsRuntime.InvokeVoidAsync("blazorInterop.initializeBigPicture");
    }

    public async Task InitializeGalleryPopup()
    {
        await _jsRuntime.InvokeVoidAsync("blazorInterop.initializeGalleryPopup");
    }
}