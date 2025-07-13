namespace PersianCat.Resume.Services;

public class ToastService
{
    public event Func<ToastMessage, Task>? OnToastShow;
    public event Func<Task>? OnToastHide;

    public async Task ShowToast(string message, ToastType type)
    {
        var toastMessage = new ToastMessage
        {
            Message = message,
            Type = type,
            IsVisible = true
        };

        if (OnToastShow != null)
        {
            await OnToastShow.Invoke(toastMessage);
        }
    }

    public async Task ShowSuccess(string message)
    {
        await ShowToast(message, ToastType.Success);
    }

    public async Task ShowError(string message)
    {
        await ShowToast(message, ToastType.Error);
    }

    public async Task ShowWarning(string message)
    {
        await ShowToast(message, ToastType.Warning);
    }

    public async Task ShowInfo(string message)
    {
        await ShowToast(message, ToastType.Info);
    }

    public async Task HideToast()
    {
        if (OnToastHide != null)
        {
            await OnToastHide.Invoke();
        }
    }
}

public class ToastMessage
{
    public string Message { get; set; } = string.Empty;
    public ToastType Type { get; set; } = ToastType.Info;
    public bool IsVisible { get; set; }
}

public enum ToastType
{
    Success,
    Error,
    Warning,
    Info
}
