using Components.Dialog;
using Components.Entities.Billing.Invoices;
using Services.Common.Dto;
using Services.Common.Dto.Billing;
using System.Collections.ObjectModel;

namespace ViewModels;

public class InvoiceListViewModel : ComponentBaseViewModel
{
    protected bool Loading;
    protected ObservableCollection<InvoiceDto> Invoices { get; set; } = new ObservableCollection<InvoiceDto>();
    protected string? SearchInvoiceNumber { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadInvoices();
        Loading = false;
    }

    protected async Task CreateOrUpdateInvoice(InvoiceDto invoiceDto)
    {
        DialogParameters parameters;
        if (invoiceDto.InvoiceId == 0)
        {
            var invoiceCreate = invoiceDto.Adapt<InvoiceCreateDto>();
            parameters = new DialogParameters { ["InvoiceCreate"] = invoiceCreate };
        }
        else
        {
            var invoiceUpdate = invoiceDto.Adapt<InvoiceUpdateDto>();
            parameters = new DialogParameters { ["InvoiceUpdate"] = invoiceUpdate };
        }

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium };
        var dialogTitle = invoiceDto.InvoiceId == 0 ? "Create Invoice" : "Update Invoice";
        var dialog = await DialogService!.ShowAsync<InvoiceFormComponent>(dialogTitle, parameters, options);

        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            await LoadInvoices(); // Osvježavanje liste faktura
        }
    }

    private async Task LoadInvoices()
    {
        try
        {
            Invoices = await InvoiceService!.GetAll();
            StateHasChanged();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    protected async Task DeleteInvoice(InvoiceDto invoice)
    {
        var parameters = new DialogParameters();
        const string text = "Are you sure you want to delete this invoice?";

        parameters.Add("ContentText", text);
        parameters.Add("ButtonText", "Delete");
        parameters.Add("Color", Color.Success);

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
        var dialog = await DialogService!.ShowAsync<ConfirmComponent>("Delete Invoice", parameters, options);
        var result = await dialog.Result;

        if (result!.Canceled)
        {
            return;
        }

        var response = await InvoiceService!.Delete(invoice.InvoiceId);
        HandleResponse(response, invoice);
    }

    protected bool FilterFunc(InvoiceDto element)
    {
        return string.IsNullOrWhiteSpace(SearchInvoiceNumber) ||
               element.InvoiceNumber!.Contains(SearchInvoiceNumber, StringComparison.OrdinalIgnoreCase);
    }

    private void HandleResponse(GeneralResponseDto response, InvoiceDto invoice)
    {
        if (response.IsSuccess)
        {
            Invoices.Remove(invoice);
            StateHasChanged();
            Snackbar!.Add("Success!", Severity.Success);
        }
        else
        {
            Snackbar!.Add("Error", Severity.Error);
        }
    }
}
