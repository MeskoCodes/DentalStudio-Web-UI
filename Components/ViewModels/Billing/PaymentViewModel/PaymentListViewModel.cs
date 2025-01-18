using Components.Dialog;
using Components.Entities.Billing.Payments;
using Services.Common.Dto;
using Services.Common.Dto.Billing;
using System.Collections.ObjectModel;

namespace ViewModels;

public class PaymentListViewModel : ComponentBaseViewModel
{
    protected bool Loading;
    protected ObservableCollection<PaymentDto> Payments { get; set; } = new ObservableCollection<PaymentDto>();
    protected string? SearchPaymentMethod { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadPayments();
        Loading = false;
    }

    protected async Task CreateOrUpdatePayment(PaymentDto paymentDto)
    {
        DialogParameters parameters = new DialogParameters();
        if (paymentDto.PaymentId == 0)
        {
            var paymentCreate = paymentDto.Adapt<PaymentCreateDto>();
            parameters = new DialogParameters { ["PaymentCreate"] = paymentCreate };
        }
        else
        {
            var paymentUpdate = paymentDto.Adapt<PaymentUpdateDto>();
            parameters = new DialogParameters { ["PaymentUpdate"] = paymentUpdate };
        }

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium };
        var dialogTitle = paymentDto.PaymentId == 0 ? "Create Payment" : "Update Payment";
        var dialog = await DialogService!.ShowAsync<PaymentFormComponent>(dialogTitle, parameters, options);

        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await LoadPayments();
        }
    }

    private async Task LoadPayments()
    {
        try
        {
            Payments = await PaymentService!.GetAll();
            StateHasChanged();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    protected async Task DeletePayment(PaymentDto payment)
    {
        var parameters = new DialogParameters();
        const string text = "Are you sure you want to delete this payment?";

        parameters.Add("ContentText", text);
        parameters.Add("ButtonText", "Delete");
        parameters.Add("Color", Color.Success);

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
        var dialog = await DialogService!.ShowAsync<ConfirmComponent>("Delete Payment", parameters, options);
        var result = await dialog.Result;

        if (result!.Canceled)
        {
            return;
        }

        var response = await PaymentService!.Delete(payment.PaymentId);
        HandleResponse(response, payment);
    }

    protected bool FilterFunc(PaymentDto element)
    {
        return string.IsNullOrWhiteSpace(SearchPaymentMethod) ||
               element.PaymentMethod!.Contains(SearchPaymentMethod, StringComparison.OrdinalIgnoreCase);
    }

    private void HandleResponse(GeneralResponseDto response, PaymentDto payment)
    {
        if (response.IsSuccess)
        {
            // Nakon uspešnog brisanja pozivamo LoadPayments() da osvežimo tabelu
            Payments.Remove(payment);
            StateHasChanged();
            Snackbar!.Add("Success!", Severity.Success);
            LoadPayments(); // Osvežavamo podatke nakon brisanja
        }
        else
        {
            Snackbar!.Add("Error", Severity.Error);
        }
    }
}
