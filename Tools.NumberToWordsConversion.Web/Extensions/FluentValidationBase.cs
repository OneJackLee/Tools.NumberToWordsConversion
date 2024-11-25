using FluentValidation;
using FluentValidation.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Tools.NumberToWordsConversion.Extensions;

public abstract class FluentValidationBase<TModel, TValidator> : ComponentBase
    where TValidator : AbstractValidator<TModel>, new()
{
    private ValidationMessageStore? _messageStore;
    private readonly TValidator _validator = new();

    [CascadingParameter] private EditContext? CurrentEditContext { get; set; }

    protected override void OnInitialized()
    {
        if (CurrentEditContext is null)
        {
            throw new InvalidOperationException(
                $"{nameof(FluentValidationBase<TModel, TValidator>)} requires a cascading parameter.");
        }

        _messageStore = new ValidationMessageStore(CurrentEditContext);

        CurrentEditContext.OnValidationRequested += delegate { ValidateModel(); };

        CurrentEditContext.OnFieldChanged += (_, eventArgs) => ValidateField(eventArgs.FieldIdentifier);
    }

    private void ValidateModel()
    {
        var modelType = CurrentEditContext?.Model.GetType();
        if (modelType != typeof(TModel))
            return;

        var validationResults = _validator.Validate((TModel)CurrentEditContext!.Model);

        _messageStore!.Clear();
        foreach (var error in validationResults.Errors)
            _messageStore.Add(CurrentEditContext.Field(error.PropertyName), error.ErrorMessage);

        CurrentEditContext.NotifyValidationStateChanged();
    }

    private void ValidateField(in FieldIdentifier fieldIdentifier)
    {
        var modelType = CurrentEditContext?.Model.GetType();
        if (modelType != typeof(TModel))
            return;

        var properties = new[] { fieldIdentifier.FieldName };

        var context = new ValidationContext<TModel>(
            (TModel)fieldIdentifier.Model,
            new PropertyChain(), new MemberNameValidatorSelector(properties));

        var validationResults = _validator.Validate(context);

        _messageStore!.Clear();
        _messageStore.Add(fieldIdentifier, validationResults.Errors.Select(error => error.ErrorMessage));

        CurrentEditContext!.NotifyValidationStateChanged();
    }
}