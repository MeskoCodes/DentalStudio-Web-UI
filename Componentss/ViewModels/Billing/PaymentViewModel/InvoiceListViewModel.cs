using Components.Dialog;
using System.Collections.ObjectModel;
using Components.Entities.Billing;
using MudBlazor;
using Components.Entities.Billing.Invoices;
using Contract.Billing;

namespace ViewModels.Billing
{
    public class InvoiceListComponent : ComponentBaseViewModel
    {
        protected bool Loading;
        protected ObservableCollection<InvoiceDto> Invoice { get; set; } = new ObservableCollection<InvoiceDto>();
        protected string? SearchInvoiceName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadInvoice();
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
                StateHasChanged();
            }
        }

        private async Task LoadInvoice()
        {
            try
            {
                Invoice = await InvoiceService!.GetAll();
                StateHasChanged();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected async Task DeleteInvoice(InvoiceDto invoice)
        {
            var parameters = new DialogParameters
            {
                { "ContentText", "Are you sure you want to delete this Invoice?" },
                { "ButtonText", "Delete" },
                { "Color", Color.Success }
            };

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
            return string.IsNullOrWhiteSpace(SearchInvoiceName) ||
                   element.InvoiceId.ToString().Contains(SearchInvoiceName, StringComparison.OrdinalIgnoreCase);
        }


        private void HandleResponse(GeneralResponseDto response, InvoiceDto invoice)
        {
            if (response.IsSuccess)
            {
                Invoice.Remove(invoice);
                StateHasChanged();
                Snackbar!.Add("Success!", Severity.Success);
            }
            else
            {
                Snackbar!.Add("Error", Severity.Error);
            }
        }
    }
}
