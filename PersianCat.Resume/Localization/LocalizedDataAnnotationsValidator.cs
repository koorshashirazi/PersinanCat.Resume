using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace PersianCat.Resume.Localization;

public class LocalizedDataAnnotationsValidator : ComponentBase, IDisposable
{
    private bool _disposed;
    private ValidationMessageStore? _messageStore;

    [CascadingParameter] private EditContext? CurrentEditContext { get; set; }
    [Inject] private IStringLocalizer<SharedResource> Localizer { get; set; } = default!;

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        if (CurrentEditContext == null)
        {
            throw new InvalidOperationException($"{nameof(LocalizedDataAnnotationsValidator)} requires a cascading parameter of type {nameof(EditContext)}.");
        }

        _messageStore = new ValidationMessageStore(CurrentEditContext);
        CurrentEditContext.OnValidationRequested += HandleValidationRequested;
        CurrentEditContext.OnFieldChanged += HandleFieldChanged;
    }

    private void HandleValidationRequested(object? sender, ValidationRequestedEventArgs e)
    {
        _messageStore?.Clear();
        AddDataAnnotationsValidation();
    }

    private void HandleFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        _messageStore?.Clear(e.FieldIdentifier);
        AddDataAnnotationsValidation();
    }

    private void AddDataAnnotationsValidation()
    {
        if (CurrentEditContext == null || _messageStore == null)
            return;

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(CurrentEditContext.Model);

        Validator.TryValidateObject(CurrentEditContext.Model, validationContext, validationResults, true);

        foreach (var validationResult in validationResults)
        {
            if (validationResult.MemberNames.Any())
            {
                foreach (var memberName in validationResult.MemberNames)
                {
                    var fieldIdentifier = new FieldIdentifier(CurrentEditContext.Model, memberName);

                    var localizedMessage = Localizer[validationResult.ErrorMessage ?? ""].Value;
                    if (localizedMessage == validationResult.ErrorMessage)
                    {
                        localizedMessage = validationResult.ErrorMessage ?? "";
                    }

                    _messageStore.Add(fieldIdentifier, localizedMessage);
                }
            }
        }

        CurrentEditContext.NotifyValidationStateChanged();
    }



    public void Dispose()
    {
        if (_disposed) return;
        if (CurrentEditContext != null)
        {
            CurrentEditContext.OnValidationRequested -= HandleValidationRequested;
            CurrentEditContext.OnFieldChanged -= HandleFieldChanged;
        }

        _messageStore?.Clear();
        _messageStore = null;
        CurrentEditContext = null;
        Localizer = null!;
        _disposed = true;
    }
}